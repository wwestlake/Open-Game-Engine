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
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec.1.0.2\lib\net40-client\FParsecCS.dll"
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec.1.0.2\lib\net40-client\FParsec.dll"
//#r @"c:\users\wwestlake\documents\visual studio 2015\Projects\Open Game Engine\packages\FParsec-Pipes.0.3.0.0\lib\net45\FParsec-Pipes.dll"
namespace LagDaemon.OGE.FSOpenGL

module ObjFileParser =

    open FParsec

    type UserState = unit // doesn't have to be unit, of course
    type Parser<'t> = Parser<'t, UserState>

    type OValue = OValue of float
    type OOptionValue = OOptionValue of float option

    type ObjLine =
        | VLine of OValue * OValue * OValue
        | VTLine of OValue * OValue * OOptionValue
        | VNLine of OValue * OValue * OValue
        | FLine of OValue list
        | Comment





    

    /// test helper function
    let test p str =
        match run p str with
        | Success(result,_,_)   -> printfn "Success: %A" result
        | Failure(errorMsg,_,_) -> printfn "Failure: %s" errorMsg


    let pComment : Parser<_> = 
        (spaces >>. pstring "#" >>. skipMany anyChar .>> opt newline) >>% Comment


    let pVLine : Parser<_> = 
        let point = pipe3 (spaces >>. pfloat) (spaces >>. pfloat) (spaces >>. pfloat) (fun x y z -> VLine (OValue x,OValue y,OValue z)) 
        (spaces .>> pstring "v" >>. point .>> opt newline)
        
    let pVTLine : Parser<_> = 
        let point = 
            pipe3 (spaces >>. pfloat) (spaces >>. pfloat) (spaces >>. (opt pfloat)) (fun x y z -> VTLine (OValue x,OValue y, OOptionValue z)) 
        (pstring "vt" >>. point .>> opt newline)

    let pVNLine : Parser<_> = 
        let point = pipe3 (spaces >>. pfloat) (spaces >>. pfloat) (spaces >>. pfloat) (fun x y z -> VNLine (OValue x,OValue y,OValue z)) 
        (pstring "vn" >>. point .>> opt newline)

    //let pFLine : Parser<_> =
    //    let pflist = many1 (spaces >>. pint32 .>> opt (pstring "/"))
    //    pstring "f" >>. 
        

    let pFile  : Parser<_> = choice [pVTLine; pVNLine; pVLine; pComment]


    let parseLines (strList: string list) =
        let rec inner list result = 
            match list with
            | [] -> result
            | head :: tail -> inner tail ((run pFile head) :: result)
        inner strList [] |> List.rev                                     
        

(* testing only
    let lines = ["v 1 2 3";
                     "v 3 4 5";
                     "v 1 3 4";
                     "v 1 2 3";
                     "v 1 2 3";
                     "# vt lines";
                     "vt 1 2 3";
                     "vt 1 2 3";
                     "vt 1 2 3";
                     "# vn lines";
                     "vn 1 2 3";
                     "vn 1 2 3";
                     "vn 1 2 3"]

    let p = parseLines lines

    // results
val p : ParserResult<ObjLine,unit> list =
  [Success: VLine (OValue 1.0,OValue 2.0,OValue 3.0);
   Success: VLine (OValue 3.0,OValue 4.0,OValue 5.0);
   Success: VLine (OValue 1.0,OValue 3.0,OValue 4.0);
   Success: VLine (OValue 1.0,OValue 2.0,OValue 3.0);
   Success: VLine (OValue 1.0,OValue 2.0,OValue 3.0); 
   Success: Comment;
   Success: VTLine (OValue 1.0,OValue 2.0,OOptionValue (Some 3.0));
   Success: VTLine (OValue 1.0,OValue 2.0,OOptionValue (Some 3.0));
   Success: VTLine (OValue 1.0,OValue 2.0,OOptionValue (Some 3.0));
   Success: Comment; 
   Success: VNLine (OValue 1.0,OValue 2.0,OValue 3.0);
   Success: VNLine (OValue 1.0,OValue 2.0,OValue 3.0);
   Success: VNLine (OValue 1.0,OValue 2.0,OValue 3.0)]

*)

