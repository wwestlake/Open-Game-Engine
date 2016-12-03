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

open LagDaemon.OGE.FSOpenGL.LinearAlgebra
open LagDaemon.OGE.FSOpenGL.LinearAlgebra.Vector3
open LagDaemon.OGE.FSOpenGL.LinearAlgebra.Matrix3

module Transform =

    type T = {
        Location: Vector3.T
        Rotation: Vector3.T
        Scale: Vector3.T
    }

    /// creates a transform at [0,0,0] with rotation [0,0,0] with scale [1,1,1]
    let zero = { 
                Location = Vector3.zero
                Rotation = Vector3.zero
                Scale = Vector3.one
               }

    let move (dirVector: Vector3.T) (transform: T) =
        let {Location = loc; Rotation = rot; Scale = scl } = transform
        let newLocation = loc |+| dirVector
        {
            transform with Location = newLocation
        }

    let radians alpha = alpha * ( (System.Math.PI * 2.0) / 360.0) 
    let degrees theta = theta * ( 360.0 / (System.Math.PI * 2.0))
    let calc_w alpha = (System.Math.Cos ((radians alpha) / 2.0))
    let calc_r alpha beta = (System.Math.Sin ((radians alpha) / 2.0)) * System.Math.Sin(radians beta)


    //let rotate (rotation: Vector<float>) (transform: T) =
    //    let quat = {w = 1.0; x = 1.0; y = 1.0; z = 1.0 }
        

