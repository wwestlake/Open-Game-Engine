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
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec.1.0.2\lib\net40-client\FParsecCS.dll"
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec.1.0.2\lib\net40-client\FParsec.dll"
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec-Pipes.0.3.0.0\lib\net45\FParsec-Pipes.dll"
namespace LagDaemon.OGE.FSOpenGL.LinearAlgebra

module Vector3 =

    type T = { X: float; Y: float; Z: float }

    let create x y z = {X = x; Y = y; Z = z}

    let zero = create 0.0 0.0 0.0
    let one = create 1.0 1.0 1.0

    let add (v1:T) (v2:T) =
        create (v1.X + v2.X) (v1.Y + v2.Y) (v1.Z + v2.Z)

    let sub (v1:T) (v2:T) =
        create (v1.X - v2.X) (v1.Y - v2.Y) (v1.Z - v2.Z)

    let mul (v:T) (s:float)  =
        create (v.X * s) (v.Y * s) (v.Z * s)

    let div (v:T) (s:float)  =
        create (v.X / s) (v.Y / s) (v.Z / s)

    let dot (v1:T) (v2:T) =
        (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z)

    let cross (a:T) (b:T) =
        create (a.Y * b.Z - a.Z * b.Y) (a.Z * b.X - a.X * b.Z) (a.X * b.Y - a.Y * b.X)

    let normal (a:T) =
        sqrt (a.X * a.X + a.Y*a.Y + a.Z * a.Z)

    let normalize (a:T) =
        let norm = normal a
        create (a.X/norm) (a.Y/norm) (a.Z/norm)


    /// vector addition
    let (|+|) = add

    /// vector subtraction
    let (|-|) = sub

    /// scalar multiplication
    let (|*|) s v = mul v s

    /// scalar division
    let (|/|) s v = div v s

    /// the dot product of two vectors
    let (|@|) = dot

    /// the cross product of two vectors
    let (|%|) = cross

    let clamp low x high =
        if x < low then low elif x > high then high else x

    /// lerps from vector1 to vecctor2 bases on x from 0-1
    let lerp (v1:T) v2 (easfn: float -> float) (x: float)  =
        let pos = clamp 0.0 (easfn x) 1.0
        pos |*| v2 |+| ((1.0 - pos) |*| v1)


module Spherical =
    open Vector3

    type T = {radius: float; theta:float; phi: float}

    let create r t p = {radius = r; theta = t; phi = p}

    let fromCartesian v = 
        let norm = normal v
        {radius = norm; theta = acos (v.Z / norm ); phi = (atan2 v.Y v.Z)  }
        
    let toCCartesian (s:T) = 
        let sint = sin s.theta
        let cost = cos s.theta
        let sinp = sin s.phi
        let cosp = cos s.phi
        {
            X = s.radius * sint * cosp; 
            Y = s.radius * sint * sinp;
            Z = s.radius * cosp  
        }
                

module Matrix3 =
    open Vector3
    
    type T = {c1: Vector3.T; c2: Vector3.T; c3: Vector3.T}

    let create c11 c12 c13 c21 c22 c23 c31 c32 c33 =
        {
            c1 = create c11 c12 c13;
            c2 = create c21 c22 c23;
            c3 = create c31 c32 c33;
        }

    let fromVectors v1 v2 v3 = {c1 = v1; c2 = v2; c3 = v3}

    let unity = create 1.0 0.0 0.0 0.0 1.0 0.0 0.0 0.0 1.0

    let cell (m:T) r c =
        match (r, c) with
        | 1,1 -> Some m.c1.X
        | 1,2 -> Some m.c1.Y
        | 1,3 -> Some m.c1.Z
        | 2,1 -> Some m.c2.X
        | 2,2 -> Some m.c2.Y
        | 2,3 -> Some m.c2.Z
        | 3,1 -> Some m.c3.X
        | 3,2 -> Some m.c3.Y
        | 3,3 -> Some m.c3.Z
        | _,_ -> None

    let row (m:T) n =
        match n with
        | 1 -> Some {X = m.c1.X; Y = m.c2.X; Z = m.c3.X}
        | 2 -> Some {X = m.c1.Y; Y = m.c2.Y; Z = m.c3.Y}
        | 3 -> Some {X = m.c1.Z; Y = m.c2.Z; Z = m.c3.Z}
        | _ -> None
        
    let mul (a:T) (b:T) = 
        let c1 = Vector3.create (a.c1.X * b.c1.X + a.c1.Y * b.c2.X + a.c1.Z * b.c3.X)
                                (a.c2.X * b.c1.X + a.c2.Y * b.c2.X + a.c2.Z * b.c3.X)
                                (a.c2.X * b.c1.X + a.c3.Y * b.c2.X + a.c3.Z * b.c3.X)
        let c2 = Vector3.create (a.c1.X * b.c1.Y + a.c1.Y * b.c2.Y + a.c1.Z * b.c3.Y)
                                (a.c2.X * b.c1.Y + a.c2.Y * b.c2.Y + a.c2.Z * b.c3.Y)  
                                (a.c3.X * b.c1.Y + a.c3.Y * b.c2.Y + a.c3.Z * b.c3.Y)
        let c3 = Vector3.create (a.c1.X * b.c1.Z + a.c1.Y * b.c2.Z + a.c1.Z * b.c3.Z)
                                (a.c2.X * b.c1.Z + a.c2.Y * b.c2.Z * a.c2.Z * b.c3.Z)
                                (a.c3.X * b.c1.Z + a.c3.Y * b.c2.Z + a.c3.Z * b.c3.Z)
        {c1 = c1; c2 = c2; c3 = c3}


    let (|&&|) m1 m2 = mul m1 m2

    let mulVector (m:T) (v:Vector3.T) =
        let row1 = row m 1
        let row2 = row m 2
        let row3 = row m 3
        match (row1, row2, row3) with
        | Some x, Some y, Some z ->
                                    let dot1 = v |@| x   
                                    let dot2 = v |@| y
                                    let dot3 = v |@| z
                                    Some {X = dot1; Y = dot2; Z = dot3}
        | _,_,_ -> None

    let (|&|) = mulVector

    let rotateX theta =
        {
            c1 = Vector3.create 1.0 0.0 0.0;
            c2 = Vector3.create 0.0 (cos theta) (-sin theta);
            c3 = Vector3.create 0.0 (-sin theta) (cos theta);
        }

    let rotateY theta =
        {
            c1 = Vector3.create (cos theta) 0.0 (-sin theta)
            c2 = Vector3.create 0.0 1.0 0.0
            c3 = Vector3.create (-sin theta) 0.0 (cos theta)
        }

    let rotateZ theta =
        {
            c1 = Vector3.create (cos theta) (sin theta) 0.0
            c2 = Vector3.create (-sin theta) (cos theta) 0.0
            c3 = Vector3.create 0.0 0.0 1.0
        }


    let rotXY thetaX thetaY =
        (rotateX thetaX) |&&| (rotateY thetaY)

    let rotXZ thetaX thetaZ =
        (rotateX thetaX) |&&| (rotateZ thetaZ)

    let rotYZ thetaY thetaZ =
        (rotateY thetaY) |&&| (rotateZ thetaZ)

    let rotXYZ thetaX thetaY thetaZ =
        (rotateX thetaX) |&&| (rotateY thetaY) |&&| (rotateZ thetaZ)


    
