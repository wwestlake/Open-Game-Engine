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

namespace LagDaemon.OGE.FSOpenGL

open LagDaemon.OGE.InterfaceTypes.TimeKeeper

module SceneControl =


    [<AbstractClass>]
    type Renderable() =

        let mutable (children : Renderable list) = List.empty
        let mutable (parent : Renderable option) = None

        abstract member Update: FrameData -> unit
        abstract member Render: FrameData -> unit

        member internal x._Update (frameData: FrameData) = 
            children |> List.iter (fun x -> x._Update frameData)
            x.Update frameData

        member internal x._Render (frameData: FrameData) = 
            children |> List.iter (fun x -> x._Update frameData)
            x.Render frameData

        member internal x.Parent 
            with get() = parent
            and  set p = parent <- p 

        member internal x._addChild (child: Renderable) =
            do child.Parent <- Some x            
            do children <- child :: children

        member x.find<'T> () =
            let t = typeof<'T>
            children |> List.find (fun x -> x.GetType() = t) 

    [<AbstractClass>]
    type GameComponent() =
        inherit Renderable()

        member x.addComponent (comp: GameComponent) =
            base._addChild comp    
        
    [<AbstractClass>]            
    type GameObject() =
        inherit Renderable()                

    type Actor() =
        inherit GameObject()

        override x.Update (frameData: FrameData) =
            ()

        override x.Render (frameData: FrameData) =
            ()

