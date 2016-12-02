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

open LagDaemon.OGE.Interop

module TimeKeeper =

    type DeltaTime = DeltaTime of float
    type FrameRate = FrameRate of float
    

    type FrameData = {
        deltaTime: DeltaTime
        frameRate: FrameRate    
    }

    
    let createFrameData deltaTime frameRate = {deltaTime = (DeltaTime deltaTime); frameRate = (FrameRate frameRate) }
    let getDeltaTime {deltaTime = (DeltaTime dt)} = dt
    let getFrameRate {frameRate = (FrameRate fr)} = fr   

    let getFrameData (timer: PrecisionTime) = 
        let elapsed = timer.ElapsedTime()
        createFrameData elapsed (1.0 / elapsed)

    let toString (frameData: FrameData) = 
        let dt = getDeltaTime frameData
        let fr = getFrameRate frameData
        sprintf "DeltaTime = %f, FrameRate = %f" dt fr

