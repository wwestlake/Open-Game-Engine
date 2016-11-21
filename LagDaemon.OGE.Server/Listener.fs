namespace LagDaemon.OGE.Server

open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading
open LagDaemon.OGE.Logging.ServerLog
open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.Logging.ServerLog

module Listener =

    type Listener = {
        Address: string;
        IP: IPAddress;
        Listener: TcpListener
    }


    let listener ip port =
        new TcpListener(ip, port)

    let createListener address port =
        async {
            let! iplist = Async.AwaitTask (Dns.GetHostAddressesAsync(address))
            match iplist with
            | [||] -> return None
            | arr  -> let listener = listener arr.[0] port
                      return Some {
                        Address = address;
                        IP = arr.[0];
                        Listener = listener;
                      }
        } |> Async.RunSynchronously



    let service (client:TcpClient) =
        async {
            use stream = client.GetStream()
            use out = new StreamWriter(stream, AutoFlush = true)
            use inp = new StreamReader(stream)
            while not inp.EndOfStream do
                let! data = Async.AwaitTask (inp.ReadLineAsync())
                match data with
                | line -> printfn "< %s" line
                          Async.AwaitTask (out.WriteLineAsync(line)) |> ignore
            printfn "closed %A" client.Client.RemoteEndPoint
            client.Close |> ignore
        } |> Async.Start
     
    let EchoService address port = 
        let listener = 
            match createListener address port with
            | Some l -> l
            | None -> failwith "Unable to start server"

        do listener.Listener.Start()
        systemLog.Log (createInfoEntry (sprintf "echo service listening on %A" listener.Listener.Server.LocalEndPoint))
        while true do
            let client = listener.Listener.AcceptTcpClient()
            printfn "connect from %A" client.Client.RemoteEndPoint
            let job = async {
                let c = client in try service client with _ -> () }
            Async.Start job

        