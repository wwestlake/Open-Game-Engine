
open System.Runtime.InteropServices

[<DllImport("Kernel32.dll")>]
extern int QueryPerformanceCounter(System.Int64& count)

[<DllImport("Kernel32.dll")>]
extern int QueryPerformanceFrequency(System.Int64& Frequency);

let mutable count : int64 = 10L
　
do QueryPerformanceCounter(count)
printfn "%i" count

　
type Result<'S,'F> =
    | Success of 'S
    | Failure of 'F

let succeed x = Success x
let fail x = Failure x

type TimeSpec = TimeSpec of float

type Vector = { X: float; Y: float; Z: float }
type Matrix = { R1: Vector; R2: Vector; R3: Vector }

type Transform = {
    Location: Vector;
    Rotation: Vector;
    Scale: Vector;
}

　
type TimeKeeper() =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let mutable lastTime = stopWatch.ElapsedMilliseconds

    member this.elapsed 
        with get() = 
            let now = stopWatch.ElapsedMilliseconds
            let dt = now - lastTime
            do lastTime <- now
            (float dt) / 1000.0

　
[<AbstractClass>]
type Component() =
    abstract member Update: deltaTime: TimeSpec -> unit
    abstract member Render: deltaTime: TimeSpec -> unit

　
[<AbstractClass>]
type GameObject() =
    let mutable components = Map.empty<string, Component>

    member this.AddComponent (name: string) (comp: Component) =
        if Map.exists (fun k _ -> k = name) components 
        then Failure [(sprintf "Component %s already exists in this object" name)]
        else
            do components <- (Map.add name comp components) 
            name |> succeed

    abstract member Update: deltaTime: TimeSpec -> unit
    abstract member Render: deltaTime: TimeSpec -> unit

　
    // these members are called internally by the game engine
    // the process of calling is this order:
    // In the Update Cycle
    //  1. Update Components
    //  2. Update this object
    // In the render cycle
    //  1. Render all components
    //  2. Render this object

    

    member internal this._UpdateComponents (deltaTime: TimeSpec) =
        components |> Map.toList |> List.iter (fun (_,b) -> b.Update deltaTime)

    member internal this._RenderComponents (deltaTime: TimeSpec) =
        components |> Map.toList |> List.iter (fun (_,b) -> b.Render deltaTime)

    member internal this._Update (deltaTime: TimeSpec) =
        this.Update deltaTime

    member internal this._Render (deltaTime: TimeSpec) =
        this.Render deltaTime

　
　
let test = new TimeKeeper()

[1..100] |> List.map (fun x -> printfn "%i" x; test.elapsed )

　
