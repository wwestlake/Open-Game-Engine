namespace LagDaemon.OGE.Loader

open LagDaemon.OGE.FileManager.DirectoryManager
open LagDaemon.OGE.MessageService.Router
open LagDaemon.OGE.InterfaceTypes.MessageTypes
open LagDaemon.OGE.InterfaceTypes.ErrorHandling
open LagDaemon.OGE.InterfaceTypes.SystemStateTypes


module SystemInitialization =

    let directoryInit () =  
        directoryCheck |> (List.iter (fun x ->  () ))

    let systemInit () =
        do directoryInit ()



