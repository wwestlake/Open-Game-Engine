// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open LagDaemon.Parser.ParserLib

　
type State = {
    Command: string;
    Count: int;
}

type Continue =
    | Yes of State
    | No

　
let initState = {Command = ""; Count = 0} |> Yes

let prompt p = printf "%s" p
               

let readLine () = Console.ReadLine()

let run state =
    let {Command = cmd; Count = cnt} = state
    match cmd with
    | "quit" -> No
    | _ -> state |> Yes
    

let eval state = 
    match state with
    | Yes {Command = cmd; Count = cnt} -> run {Command = cmd; Count = cnt + 1}
    | No -> No

let rec repl state =
    match state with
    | Yes st -> do prompt (sprintf "Ogre %i> " st.Count)
                let text = readLine ()
                { st with Command = text; } |> Yes |> eval |> repl
    | No -> No

　
[<EntryPoint>]
let main argv = 
    
    do repl initState |> ignore

    

    //printfn "Press any key to exit"
    //Console.ReadKey(true) |> ignore
    0 // return an integer exit code
