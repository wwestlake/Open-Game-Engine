
#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\MathNet.Numerics.3.13.1\lib\net40\MathNet.Numerics.dll"

open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra


let a = vector [1.0;2.0;3.0]
let b = vector [2.0;3.0;4.0]

let c = b - a

c


