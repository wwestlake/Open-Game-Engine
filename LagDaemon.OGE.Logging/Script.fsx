



open System
open System.Threading
open System.Diagnostics

// a utility function
type Utility() = 
    static let rand = new Random()
    
    static member RandomSleep() = 
        let ms = rand.Next(1,10)
        Thread.Sleep ms

// an implementation of a shared counter using locks
type LockedCounter () = 

    static let _lock = new Object()

    static let mutable count = 0
    static let mutable sum = 0

    static let updateState i = 
        // increment the counters and...
        sum <- sum + i
        count <- count + 1
        printfn "Count is: %i. Sum is: %i" count sum 

        // ...emulate a short delay
        Utility.RandomSleep()


    // public interface to hide the state
    static member Add i = 
        // see how long a client has to wait
        let stopwatch = new Stopwatch()
        stopwatch.Start() 

        // start lock. Same as C# lock{...}
        lock _lock (fun () ->
        
            // see how long the wait was
            stopwatch.Stop()
            printfn "Client waited %i" stopwatch.ElapsedMilliseconds

            // do the core logic
            updateState i 
            )
        // release lock

let makeCountingTask addFunction taskId  = async {
    let name = sprintf "Task%i" taskId
    for i in [1..3] do 
        addFunction i
    }


let lockedExample5 = 
    [1..100]
        |> List.map (fun i -> makeCountingTask LockedCounter.Add i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore


type MessageBasedCounter () = 

    static let updateState (count,sum) msg = 

        // increment the counters and...
        let newSum = sum + msg
        let newCount = count + 1
        printfn "Count is: %i. Sum is: %i" newCount newSum 

        // ...emulate a short delay
        Utility.RandomSleep()

        // return the new state
        (newCount,newSum)

    // create the agent
    static let agent = MailboxProcessor.Start(fun inbox -> 

        // the message processing function
        let rec messageLoop oldState = async{

            // read a message
            let! msg = inbox.Receive()

            // do the core logic
            let newState = updateState oldState msg

            // loop to top
            return! messageLoop newState 
            }

        // start the loop 
        messageLoop (0,0)
        )

    // public interface to hide the implementation
    static member Add i = agent.Post i



MessageBasedCounter.Add 4
MessageBasedCounter.Add 5

let task = makeCountingTask MessageBasedCounter.Add 1
Async.RunSynchronously task


let messageExample5 = 
    [1..100]
        |> List.map (fun i -> makeCountingTask MessageBasedCounter.Add i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore


let slowConsoleWrite msg = 
    msg |> String.iter (fun ch->
        System.Threading.Thread.Sleep(1)
        System.Console.Write ch
        )


slowConsoleWrite "abc\n"

let makeTask logger taskId = async {
    let name = sprintf "Task%i" taskId
    for i in [1..3] do 
        let msg = sprintf "-%s:Loop%i-" name i
        logger msg 
    }

// test in isolation
let task1 = makeTask slowConsoleWrite 1
Async.RunSynchronously task

type UnserializedLogger() = 
    // interface
    member this.Log msg = slowConsoleWrite msg

// test in isolation
let unserializedLogger = UnserializedLogger()
unserializedLogger.Log "hello"

let unserializedExample = 
    let logger = new UnserializedLogger()
    [1..5]
        |> List.map (fun i -> makeTask logger.Log i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore


type SerializedLogger() = 

    // create the mailbox processor
    let agent = MailboxProcessor.Start(fun inbox -> 

        // the message processing function
        let rec messageLoop () = async{

            // read a message
            let! msg = inbox.Receive()

            // write it to the log
            slowConsoleWrite msg

            // loop to top
            return! messageLoop ()
            }

        // start the loop
        messageLoop ()
        )

    // public interface
    member this.Log msg = agent.Post msg

// test in isolation
let serializedLogger = SerializedLogger()
serializedLogger.Log "hello"

let serializedExample = 
    let logger = new SerializedLogger()
    [1..5]
        |> List.map (fun i -> makeTask logger.Log i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

let createTimerAndObservable timerInterval =
    // setup a timer
    let timer = new System.Timers.Timer(float timerInterval)
    timer.AutoReset <- true

    // events are automatically IObservable
    let observable = timer.Elapsed  

    // return an async task
    let task = async {
        timer.Start()
        do! Async.Sleep 10000
        timer.Stop()
        }

    // return a async task and the observable
    (task,observable)

// create the timer and the corresponding observable
let basicTimer2 , timerEventStream = createTimerAndObservable 1000

// register that everytime something happens on the 
// event stream, print the time.
timerEventStream 
|> Observable.subscribe (fun _ -> printfn "tick %A" DateTime.Now)

// run the task now
Async.RunSynchronously basicTimer2










