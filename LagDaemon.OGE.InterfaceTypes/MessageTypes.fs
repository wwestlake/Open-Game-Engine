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

module MessageTypes =

    type Severity = High | Medium  | Low
            
    type Criticality =
        | Info  of string
        | Warning of string
        | Error of string
        | Exception of string * System.Exception 


    type LogEntry = {
        TimeStamp: System.DateTime;
        Severity: Severity;
        Criticality: Criticality;
    }

    type Credentials = {
        Login: string;
        Password: string;
    }


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
