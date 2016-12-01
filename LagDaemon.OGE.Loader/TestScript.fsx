


let (|Fizz|Buzz|FizzBuzz|Num|) n =
    match n % 3, n % 5 with
    | 0,0 -> FizzBuzz
    | 0,_ -> Fizz
    | _,0 -> Buzz
    | _,_ -> Num n

for i in 1 .. 100 do
    match i with
    | FizzBuzz   -> printfn "FizzBuzz"
    | Fizz       -> printfn "Fizz"
    | Buzz       -> printfn "Buzz"
    | Num n      -> printfn "%i" n


let (|Fizz|_|) value =
    if value % 3 = 0 then Some () else None

let (|Buzz|_|) value =
    if value % 5 = 0 then Some () else None

    
for i in 1..100 do
    match i with
    | Fizz & Buzz -> printfn "FizzBuzz"
    | Fizz        -> printfn "Fizz"
    | Buzz        -> printfn "Buzz"
    | n           -> printfn "%i" n


let (|Groups|_|) pattern value =
    let m = System.Text.RegularExpressions.Regex.Match(value, pattern)
    match m.Success, m.Groups.Count with
    | true, n when n > 0 ->
        [for g in m.Groups -> g.Value] 
        |> List.tail
        |> Some
    | _ -> None

match "3720611231" with
| Groups "(\d{5})([-]\d{4})?" [ zip; ex ] ->
        printfn "Postal Code: %s with %s" zip ex
| _ -> printfn "Invalid postal code"


type Tree<'T> =
    | Leaf of 'T
    | Branch of Tree<'T> * Tree<'T>

let (|Branch|_|) = function
    | Branch (l,r) -> Some (l,r)
    | _            -> None

let collect (|Pred|_|) root =
    let rec loop values node =
        match node with
        | Pred (v,next) -> loop (v::values) next
        | _             -> List.rev values, root
    loop [] root

let (|Branches|) root =
    collect (|Branch|_|) root











