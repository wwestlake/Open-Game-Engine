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

namespace LagDaemon.OGE.MessageService

open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.Logging.ServerLog

module Router =

    type MessageRouter() =
        let agent = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop () = async {
                let! msg = inbox.Receive()
                match msg with
                | LogEntry le -> systemLog.Log le
                | Credentials cr -> ()
                return! messageLoop ()
            }
        
            messageLoop()
        )

        member this.Send msg = agent.Post msg

    let router = new MessageRouter()

