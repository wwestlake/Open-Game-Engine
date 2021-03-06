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

    Source code available at: https://github.com/wwestlake/open-game-research-engine
*)

namespace LagDaemon.OGE.Logging

open System
open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.FileManager.DirectoryManager
open LagDaemon.OGE.FileManager.FileManager
open LagDaemon.OGE.FileManager.FileIO
open LagDaemon.OGE.InterfaceTypes.ErrorHandling

module ServerLog =

    let formatLogEntry msg =
        let { TimeStamp = time; Login = login; Severity = sev; Criticality = crit; Message = m}  = msg
        match crit with
        | Exception (m,e) -> sprintf "%A|%s|%A|%A|%s\n%s"  time login sev crit m e.StackTrace |> succeed
        | Info | Warning | Error -> sprintf "%A|%s|%A|%A|%s"  time login sev crit m |> succeed

    /// writes a log entry to the console
    let consoleLog (msg: RopResult<LogEntry,string list>) = File.iorunner {
        let! message = msg
        let! text = formatLogEntry message
        do printfn "%s" text
        return () |> succeed
    }
    


    /// writes a log entry to the daily log file inthe system logs directory
    let fileLog (msg: RopResult<LogEntry,string list>) = File.iorunner {
        use! file = File.fopenAppend (currentLogFile ())
        let! message = msg
        let! text = formatLogEntry message
        let result = File.writeline file text
        return () |> succeed
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
    
    

