namespace LagDaemon.OGE.FileManager

open System

module FileManager =

    let currentLogFile () =
        sprintf "DailyLog_%i%02i%02i.log" DateTime.Now.Year DateTime.Now.Month DateTime.Now.Day
        |> makefilepath logs


