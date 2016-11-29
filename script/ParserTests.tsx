#load "TextInput.fs"
#load "ParserLib.fs"

open System
open LagDaemon.Parser.TextInput
open LagDaemon.Parser.ParserLib

　
type Identifier = Identifier of string
type StringValue = StringValue of string
type IntValue = IntValue of int
type FloatValue = FloatValue of float
type KeyWord = KeyWord of string

type Number =
    | Int of IntValue
    | Float of FloatValue

type Value =
    | Number 
    | StringValue
