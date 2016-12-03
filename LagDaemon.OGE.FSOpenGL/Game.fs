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

namespace LagDaemon.OGE.FSOpenGL

open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.InterfaceTypes
open LagDaemon.OGE.Logging.ServerLog
open LagDaemon.OGE.InterfaceTypes.ErrorHandling
open LagDaemon.OGE.InterfaceTypes.TimeKeeper

module Game =

    open System
    open System.Drawing
    open OpenTK
    open OpenTK.Graphics.OpenGL



    type Game(gameWindow: GameWindow) as x =
        let window = gameWindow
        do window.Load.Add(fun e -> x.load(e))
        do window.Resize.Add(fun e -> x.resize(e))
        do window.UpdateFrame.Add(fun e -> x.updateFrame(e))
        do window.RenderFrame.Add(fun e -> x.renderFrame(e))
        do window.Closing.Add(fun e -> x.closing(e))
        let mutable frameData = masterTime.getFrameData (initFrameData)

        member x.load(e) =
            createInfoEntry "Main game loading" |> succeed |> systemLog.Log
            do masterTime.start () |> ignore
        member x.resize(e) =
            do ()
        member x.updateFrame(e) =
            do frameData <- masterTime.getFrameData(frameData)
            let gt = createRunTime frameData
            printfn "%A %A" frameData gt
            //printfn "Delta Time = %f, Frame Rate = %f" deltaTime (1.0/deltaTime)
            do ()
        member x.renderFrame(e) =
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);
            window.SwapBuffers();

        member x.closing(e) =
            createInfoEntry "Main game closing" |> succeed |> systemLog.Log
        member x.run() =
            window.Run()

        
