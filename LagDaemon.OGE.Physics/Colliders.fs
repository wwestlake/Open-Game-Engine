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

namespace LagDaemon.OGE.Collision

open System
open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra

/// A module that represents operations on collision bodies
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Collider =


    /// The points we are dealing with may be either a point to 
    /// test or a set of points the represt and collision body
    type TestPoint = TestPoint of Vector<float>
    type WorldPosition = WorldPosition of Vector<float>
    type RelativePosition = RelativePosition of Vector<float>
    type VertexList = VertexList of Vector<float> list
    type VertexPair = VertexPair of (Vector<float> * Vector<float>)
    type Radius = Radius of float
    type Height = Height of float

    type Result =
        | Collision
        | NoCollision

    type Collider =
        | Sphere of WorldPosition * Radius
        | Box of WorldPosition * VertexPair
        | Capsule of WorldPosition * Radius * Height
    
    let point v = 
        (TestPoint v)

    let world v =
        (WorldPosition v)

    let vertexList vlist =
        (VertexList vlist)

    let radius x =
        (Radius x)

    let height x =
        (Height x)

    let sphere (wp: WorldPosition) (radius: Radius) =
        Sphere (wp, radius)    
    
    let box wp vlist =
        (Box (wp, vlist))

    let capsule wp radius height =
        (Capsule (wp, radius, height))

    let between (x: float) y a =
        let r = max x y
        let l = min x y
        (r <= a) && (a <= l)


    /// Normalize the point we are checking to be relative to zero 
    /// collider represents the location of the colider
    /// put represenst the point under test (put)
    let normalize collider (put: Vector<float>) =
        let (WorldPosition c) = collider
        put - c
    
        

    let collideSphere wp r point =
        let put = normalize wp point
        let len = put |> Vector.map (fun x -> x * x)
                  |> Vector.sum
                  |> System.Math.Sqrt
        match len <= r with
        | true -> Collision 
        | false -> NoCollision

    let collideBox wp vpair point =
        let put = normalize wp point
        let (va,vb) = vpair
        let v1  = normalize wp va 
        let v2  = normalize wp vb
        match ((between v1.[0] v2.[0] put.[0]), 
               (between v1.[1] v2.[1] put.[1]),
               (between v1.[2] v2.[2] put.[2])) with
        | (true, true, true) -> Collision
        | _                  -> NoCollision

    let collide collider point =
        match collider with
        | Sphere (wp, (Radius r)) -> collideSphere wp r point
        | Box (wp, (VertexPair vp)) -> collideBox wp vp point
        | Capsule (wp, radius, height) -> NoCollision
