namespace LagDaemon.OGE.Loader

open LagDaemon.OGE.InterfaceTypes.Constants
open System
open System.Collections.Generic
open System.Text
open System.Threading.Tasks
open Newtonsoft.Json
open System.IO
open System.Net
open System.Net.Sockets
open System.Security.Cryptography
open System.Runtime.InteropServices



module Authentication =


    let base64urlencodeNoPadding (buffer: byte[]) =
        let strip chars = String.collect (fun c -> 
            if Seq.exists((=)c) chars then "" else string c
        )
        let base64 = Convert.ToBase64String(buffer)
        base64 |> (Seq.map (fun x -> 
            match x with
            | '+' -> '-'
            | '/' -> '_'
            | x -> x
        )) |> String.Concat |> strip "="

    let sha256 (inputString: string) =
        let bytes = Encoding.ASCII.GetBytes(inputString)
        let sha = new SHA256Managed()
        sha.ComputeHash(bytes)


    let randomDataBase64url (length: uint32) =
        let rng = new RNGCryptoServiceProvider()
        let bytes = Array.zeroCreate (int length)
        rng.GetBytes(bytes)
        base64urlencodeNoPadding(bytes)

    
    let userinfoCall access_token = async {
        let userinfoRequestURI = userInfoEndpoint
        let userinfoRequest = WebRequest.Create(userinfoRequestURI) :?> HttpWebRequest
        do userinfoRequest.Method <- "GET"
        do userinfoRequest.Headers.Add(sprintf "Authorization: Bearer %s" access_token)
        do userinfoRequest.ContentType <- "application/x-www-form-urlencoded"
        do userinfoRequest.Accept <- "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        let! userinfoResponse = userinfoRequest.GetResponseAsync() |> Async.AwaitTask
        use userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream())
        let! result = userinfoResponseReader.ReadToEndAsync() |> Async.AwaitTask
        return result
    }


    let performCodeExchange code code_verifier redirectURI = async {
        let tokenRequestURI = tokenEndpoint

        let tokenRequestBody = sprintf "code=%s&redirect_uri=%s&client_id=%s&code_verifier=%s&client_secret=%s&scope=&grant_type=authorization_code"
                                    code
                                    (System.Uri.EscapeDataString(redirectURI))
                                    clientID
                                    code_verifier
                                    clientSecret

        let tokenRequest = WebRequest.Create(tokenRequestURI) :?> HttpWebRequest
        do tokenRequest.Method <- "POST"
        do tokenRequest.ContentType <- "application/x-www-form-urlencoded"
        do tokenRequest.Accept <- "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        let _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody)
        do tokenRequest.ContentLength <- (int64 (Array.length _byteVersion))
        let stream = tokenRequest.GetRequestStream()
        do! stream.WriteAsync(_byteVersion, 0, _byteVersion.Length) |> Async.AwaitTask
        do stream.Close()
        try 
            let! tokenResponse = tokenRequest.GetResponseAsync() |> Async.AwaitTask
            use reader = new StreamReader(tokenResponse.GetResponseStream())
            let! responseText = reader.ReadToEndAsync() |> Async.AwaitTask
            let tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText)
            let access_token = tokenEndpointDecoded.["access_token"]
            let! userInfo = userinfoCall(access_token)
            return userInfo
        with
            | ex -> printfn "%A" ex
                    return ""
    }
        