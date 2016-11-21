namespace LagDaemon.OGE.Logging

open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.FileManager.DirectoryManager
open LagDaemon.OGE.FileManager.FileIO

module ServerLog =

    let consoleLog msg = 
        let {
            TimeStamp = timeStamp;
            Severity = severity;
            Criticality = criticality;
            }  = msg
        match criticality with
        | Exception (msg, ex) -> printfn "%A - %A; %A" timeStamp severity msg
                                 printfn "Exception -> %s\n%s\n%s" msg ex.Message ex.StackTrace
        | _                   -> printfn "%A - %A; %A" timeStamp severity criticality
    

    let fileLog msg =
        let {
            TimeStamp = timeStamp;
            Severity = severity;
            Criticality = criticality;
            }  = msg
        let today = System.DateTime.Now
        let filename = sprintf "%d%d%d.log" today.Year  today.Month today.Day
        let filepath = makefilepath logs filename
        use writer = File.fopenAppend filepath
        match criticality with
        | Exception (msg, ex) -> let text = sprintf "%A - %A; %A\n" timeStamp severity msg
                                 let extext = sprintf "Exception -> %s\n%s\n%s\n" msg ex.Message ex.StackTrace
                                 writer.WriteLine( text )
                                 writer.WriteLine( extext )
        | _                   -> let text = sprintf "%A - %A; %A\n" timeStamp severity criticality
                                 writer.WriteLine( text )

    type SystemLogger(loggers) =
        let loggers = loggers
        let agent = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop () = async {
                let! msg = inbox.Receive()
                loggers |> Seq.iter (fun logger -> logger msg)
                return! messageLoop ()
            }
        
            messageLoop()
        )
        member this.Log msg = agent.Post msg


    let systemLog = new SystemLogger(
                        seq { 
                            yield consoleLog
                            yield fileLog 
                        })
    
    

