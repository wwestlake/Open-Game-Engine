namespace LagDaemon.OGE.RulesEngine

open System
open LagDaemon.OGE.InterfaceTypes.ErrorHandling

module Rules =


    type RuleResult<'Tpass, 'Tfail> =
         | Pass of 'Tpass
         | Fail of 'Tfail

    let pass x = Pass x
    let fail x = Fail x

    let either passf failf rule =
        match rule with
        | Pass p -> passf p
        | Fail f -> failf f


    let bind x f =
        either f fail
        //match x with
        //| Pass d -> f d
        //| Fail d -> Fail d

    let (>>=) x f = bind x f

    let map f = 
        either (f >> pass) fail

    let rule f x = f x |> pass

    let tee f x =
        f x |> ignore
        x

    let (>=>) rule1 rule2 = 
        rule1 >> bind rule2
       //match rule1 x with
       //| Pass s -> rule2 s
       //| Fail f -> Fail f 

    let TryRule x f =
        try
            f x |> Pass 
        with
            | ex -> Fail ex.Message

    let And andSuccess andFailure rule1 rule2 x =
        match (rule1 x), (rule2 x) with
        | Pass p1, Pass p2 -> (andSuccess p1 p2) |> pass
        | Fail f1, Pass _ -> f1 |> fail
        | Pass _, Fail f2 -> f2 |> fail
        | Fail f1, Fail f2 -> (andFailure f1 f2) |> fail

    let (&&&) v1 v2 =
        let andSuccess r1 r2 = r1
        let andFailure s1 s2 = s1 + "; " + s2
        And andSuccess andFailure v1 v2


/////////////////////////////////////////////////
// some rules for testing
////////////////////////////////////////////////

    type Name = Name of string

    let nameLengthLessThan50 name =
        if String.length name > 50 
        then Fail "Name can not be longer than 50 characts"
        else name |> Pass

    let nameContainsNoSpaces (name: string) =
        let result = name.IndexOfAny([|' ';'\t';'\n';'\r'|])
        if result < 0 then Pass name
        else Fail "Names must not contain whitespace"

    let nameIsNotBill (name: string) =
        if (name.ToLower() <> "bill") then Pass name
        else Fail "Name must not be bill"

    
    let logit x = printfn "%A" x
        
    let testName  =  nameLengthLessThan50 
                     &&& rule (tee logit) 
                     &&& nameContainsNoSpaces 
                     &&& rule (tee logit) 
                     &&& nameIsNotBill 
                     &&& rule (tee logit)

    testName "BillW "

    let even n = [0..n] |> List.map (fun x -> 2 * x)
    let odd n = [0..n] |> List.map (fun x -> 2 * x + 1)

    even 10

    odd 10




