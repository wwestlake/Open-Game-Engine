namespace LagDaemon.OGE.Loader


module SystemInitialization =

    open LagDaemon.OGE.FileManager.DirectoryManager
    open LagDaemon.OGE.Logging.ServerLog
    open LagDaemon.OGE.InterfaceTypes.MessageTypes

    let directoryInit () =  
        directoryCheck |> (List.iter (fun x ->  () ))


    let systemInit () =
        do directoryInit ()


