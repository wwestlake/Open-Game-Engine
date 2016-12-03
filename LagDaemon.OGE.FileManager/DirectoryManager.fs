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


namespace LagDaemon.OGE.FileManager

open System
open System.IO
open FileIO
open LagDaemon.OGE.InterfaceTypes.ErrorHandling

[<AutoOpen>]
module DirectoryManager =

    let companyDir = "LagDaemon"
    let engineDir = "OpenGameEngine"
    let assetsDir = "assets"
    let libraryDir = "lib"
    let pluginDir = "plugins"
    let dataDir = "data"
    let logDir = "logs"

    let appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    let createSubdir baseDir path = Path.Combine(baseDir, path)
    let baseDirectory = createSubdir (createSubdir appData companyDir) engineDir
    let create path = createSubdir baseDirectory path
    let assets = create assetsDir        
    let library = create libraryDir
    let plugins = create pluginDir
    let data = create dataDir
    let logs = create logDir
    let makefilepath path file = Path.Combine(path, file)

    let allDirectories = seq { 
                            yield assets
                            yield library
                            yield plugins
                            yield data
                            yield logs
                            }

    let directoryCheck =
        [
            for d in allDirectories do
                let result = 
                    match FileIO.Directory.exists d with
                    | Success (exist,_) -> if exist 
                                           then FileIO.Directory.create d |> succeed 
                                           else fail "No file" 

                    | Failure msg -> "Exception " |> fail 

                yield result
        ]


    