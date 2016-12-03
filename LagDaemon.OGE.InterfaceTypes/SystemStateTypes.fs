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

namespace LagDaemon.OGE.InterfaceTypes

open LagDaemon.OGE.InterfaceTypes.ErrorHandling

module SystemStateTypes =

    type SystemType =
         | ClientOnly
         | ServerOnly
         | ClientAndServer

    type SystemState =
         | Shutdown of (unit -> RopResult<string,string>)
         | Starting  of (unit -> RopResult<string,string>)
         | Initializing of (unit -> RopResult<string,string>)
         | Running of (unit -> RopResult<string,string>)
         | Maintenance of (unit -> RopResult<string,string>)
         
    let transition curstate desiredstate = 
        match curstate, desiredstate with
        | Starting _,       Initializing _      -> desiredstate |> succeed
        | Starting _,       _                   -> desiredstate |> fail
        | Initializing _,   Running _           -> desiredstate |> succeed
        | Initializing _,   _                   -> desiredstate |> fail
        | Running _,        Maintenance _       -> desiredstate |> succeed
        | Running _,        Shutdown _          -> desiredstate |> succeed
        | Running _,        _                   -> desiredstate |> fail
        | Maintenance _,    Running _           -> desiredstate |> succeed
        | Maintenance _,    Shutdown _          -> desiredstate |> succeed
        | Maintenance _,    _                   -> desiredstate |> fail
        | Shutdown _,       _                   -> desiredstate |> fail

    




