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

    Source code available at: https://github.com/wwestlake/KnightsTour
*)
open System
open System.Drawing
open OpenTK
open OpenTK.Graphics.OpenGL
open LagDaemon.OGE.FSOpenGL.Game

[<EntryPoint>]
let main argv = 
    printfn "Open Game Engine - Loader"

    let game = new Game(new GameWindow())
    game.run()


    printfn "Press Any Key to Exit"
    System.Console.ReadKey(false) |> ignore

    0 // return an integer exit code
