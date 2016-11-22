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

    Source code available at: https://github.com/wwestlake/Open-Game-Engine
*)

namespace LagDaemon.OGE.FileManager

open System
open System.IO
open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open System.Xml
open LagDaemon.OGE.InterfaceTypes.MessageTypes

module FileIO =
    
    module Directory =

        /// checks the existance of a directory
        let exists path = Directory.Exists(path)

        /// creates a directory at the specified path (and any parent direcoties required)
        let create path = Directory.CreateDirectory(path)

        let info path = DirectoryInfo(path)

        /// gets a listing of all directories at the specified path
        let getDirectories path = Directory.EnumerateDirectories(path)

        /// gets a listing of all files at the specified path
        let getFiles path = Directory.EnumerateFiles(path)

    module File =
        
        /// checks the existance of a file
        let exists filepath = File.Exists(filepath)

        /// creates or overwrites a random access file
        let create filepath = File.Create(filepath, 4096, FileOptions.RandomAccess)

        /// creates or overwrites a text file
        let createText filepath = File.CreateText(filepath)

        /// opens a file for access (used for binary random access files)
        let fopen filepath = File.Open(filepath, FileMode.Open)

        /// opens a text file for read access
        /// returns a StreamReader
        let fopenText filepath = File.OpenText(filepath)

        /// opens an existing file or creates a new file for writing
        // returns a StreamWriter
        let fopenWrite filepath = new StreamWriter( File.OpenWrite(filepath) )

        /// opens an existing file for read write access in append mode 
        let fopenAppend filepath = new StreamWriter( File.Open(filepath, FileMode.Append))

    
    [<AutoOpen>]
    module Serialize =


        let serialize (t: 'T) (stream: Stream) =
            let ser = new DataContractJsonSerializer(t.GetType())
            ser.WriteObject(stream, t)
        


