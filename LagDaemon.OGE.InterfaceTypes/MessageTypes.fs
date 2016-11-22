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


namespace LagDaemon.OGE.InterfaceTypes

open System.IO
open System.Runtime.Serialization
open System.Runtime.Serialization.Json

module MessageTypes =

    [<DataContract>]
    type Severity = | [<DataMember>] High | [<DataMember>] Medium  | [<DataMember>] Low
     
    [<DataContract>]        
    type Criticality =
        | [<DataMember>] Info  of string
        | [<DataMember>] Warning of string
        | [<DataMember>] Error of string
        | [<DataMember>] Exception of string * System.Exception 

    [<DataContract>]
    type LogEntry = {
        [<DataMember>] 
        TimeStamp: System.DateTime;

        [<DataMember>]
        Severity: Severity;

        [<DataMember>]
        Criticality: Criticality;
    }

    [<DataContract>]
    type Credentials = {
        [<DataMember>]
        Login: string;

        [<DataMember>]
        Password: string;
    }

    [<DataContract>]
    type Envelope =
    | [<DataMember>] LogEntry of LogEntry
    | [<DataMember>] Credentials of Credentials

    let createLogEnvelope logentry = LogEntry logentry

    let createInfoEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = Low;
                    Criticality = Info str;
                }

    let createWarningEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = Medium;
                    Criticality = Warning str;
                }

    let createErrorEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = High;
                    Criticality = Error str;
                }


    let createExceptionEntry str ex = {
                    TimeStamp = System.DateTime.Now;
                    Severity = High;
                    Criticality = Exception (str, ex);
                }

    let serialize (t: 'T) (stream: Stream) =
        let ser = new DataContractJsonSerializer(t.GetType())
        ser.WriteObject(stream, t)

