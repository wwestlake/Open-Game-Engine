(*
    Open Game Engine
    Copyright (C) 2016  William W. Westlake
    wwestlake@lagdaemon.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Source code available at: https://github.com/wwestlake/Open-Game-Engine
*)

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
    
    

