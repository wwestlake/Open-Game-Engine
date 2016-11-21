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

namespace LagDaemon.OGE.UnitTests

open NUnit
open NUnit.Framework
open FsUnit
open LagDaemon.OGE.Collision.Collider
open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ColliderTests =



    [<Test>]
    let ``Given a sphere collider and a point outside the sphere the collider should return NoCollision`` () =
        let sphereLocation = world (vector [10.0;10.0;10.0])
        let sphereRadius = radius 1.0
        let testSphere = sphere sphereLocation sphereRadius
        let testPoint = vector [12.0;12.0;12.0]
        (
            match (collide testSphere testPoint) with
            | Collision -> true
            | NoCollision -> false
        ) |> should be False

    [<Test>]
    let ``Given a sphere collider and a point inside the sphere the collider should return Collision`` () =
        let sphereLocation = world (vector [10.0;10.0;10.0])
        let sphereRadius = radius 1.0
        let testSphere = sphere sphereLocation sphereRadius
        let testPoint = vector [9.8;9.8;9.8]
        (
            match (collide testSphere testPoint) with
            | Collision -> true
            | NoCollision -> false
        ) |> should be True

    [<Test>]
    let ``Given a box collider and a point outside the box the collider should return NoCollision`` () =
        let boxLocation = world (vector [10.0;10.0;10.0])
        let corner1 = vector [12.0;12.0;12.0]
        let corner2 = vector [8.0;8.0;8.0]
        let testBox = box boxLocation (corner1, corner2)
        let testPoint = vector [13.0;13.0;13.0]
        (
            match (collide testBox testPoint) with
            | Collision -> true
            | NoCollision -> false
        ) |> should be False

    [<Test>]
    let ``Given a box collider and a point inside the box the collider should return Collision`` () =
        let boxLocation = world (vector [10.0;10.0;10.0])
        let corner1 = vector [12.0;12.0;12.0]
        let corner2 = vector [8.0;8.0;8.0]
        let testBox = box boxLocation (corner1, corner2)
        let testPoint = vector [11.0;11.0;11.0]
        (
            match (collide testBox testPoint) with
            | Collision -> true
            | NoCollision -> false
        ) |> should be True

