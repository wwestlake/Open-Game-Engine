let isOdd num =
    ((num % 2) <> 0)

let mean (list: float list) =
    let len = List.length list
    if len = 0 then None 
    else Some ((List.sum list) / (float (List.length list)))

let sampleMean (list: float list) =
    let len = (List.length list - 1)
    if len = 0 then None
    else Some ((List.sum list) / (float (List.length list - 1)))
    
        
let variance (list: float list) =
    match mean list with
    | Some mean' -> mean(List.map(fun x -> (x - mean') * (x - mean')) list)
    | None -> None

let standardDeviation (list: float list) =
    match variance list with
    | Some x -> Some (sqrt x)
    | None -> None

let sampleVariance (list: float list) =
    match mean list with
    | Some mean' -> sampleMean(List.map(fun x -> (x - mean') * (x - mean')) list)
    | None -> None

let sampleStandardDeviation (list: float list) =
    match sampleVariance list with
    | Some var' -> Some (sqrt (var'))
    | None -> None

let median (list: float list) =
    let list' = list |> List.sort
    let len = List.length list
    if len = 0 then None else
        if isOdd len then Some (list'.[(len / 2)])
        else Some ((list'.[len / 2 - 1] + list'.[(len / 2)]) / 2.0)

　
    
[1.;1.;2.;3.;4.;3.;3.;2.;] |> standardDeviation
[1.;1.;2.;3.;4.;3.;3.;2.;] |> sampleStandardDeviation
[3.;4.;9.;7.;15.;12.] |> median
