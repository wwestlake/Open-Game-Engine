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

open System
open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.FileManager.DirectoryManager
open LagDaemon.OGE.FileManager.FileIO
open LagDaemon.OGE.InterfaceTypes.ErrorHandling

module ServerLog =

    /// writes a log entry to the console
    let consoleLog (msg: RopResult<LogEntry,string list>) = File.iorunner {
        let! { TimeStamp = time; Severity = sev; Criticality = crit; Message = m}  = msg
        let! result = match crit with
                      | Exception (m,e) -> printfn "%A|%A|%A|%s\n%s"  time sev crit m e.StackTrace |> succeed
                      | Info | Warning | Error -> printfn "%A|%A|%A|%s"  time sev crit m |> succeed
        return msg
    }
    

    /// writes a log entry to the daily log file inthe system logs directory
    let fileLog (msg: RopResult<LogEntry,string list>) = File.iorunner {
        let filename = sprintf "%i%02i%02i.txt" DateTime.Now.Year DateTime.Now.Month DateTime.Now.Day
        let filepath = makefilepath logs filename
        let! message = msg
        use! file = File.fopenAppend filepath 
        serialize message file.BaseStream         
        return msg
    }


    /// handles the system log mail box
    type SystemLogger(loggers) =
        let loggers = loggers
        let agent = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop () = async {
                let! msg = inbox.Receive()
                loggers |> Seq.iter (fun logger -> logger msg |> ignore)
                return! messageLoop ()
            }
        
            messageLoop()
        )
        member this.Log msg = agent.Post msg


    /// the system log used to send log entrys to the mailbox queue
    let systemLog = new SystemLogger(
                        seq { 
                            yield consoleLog
                            yield fileLog 
                        })
    
    

