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
open LagDaemon.OGE.InterfaceTypes.ErrorHandling

module FileIO =

    /// calls the RoP fail method with an exception message as string list
    let failex (ex: Exception) = fail [ex.ToString()]
    
    module Directory =


        /// checks the existance of a directory
        let exists path = succeed (Directory.Exists(path))

        /// creates a directory at the specified path (and any parent direcoties required)
        let create path = 
            try
                succeed (Directory.CreateDirectory(path))
            with 
                | ex -> failex ex

        let info path = 
            try
                succeed (DirectoryInfo(path))
            with
                | ex -> failex ex

        /// gets a listing of all directories at the specified path
        let getDirectories path = 
            try
                succeed (Directory.EnumerateDirectories(path))
            with
                | ex -> failex ex

        /// gets a listing of all files at the specified path
        let getFiles path = 
            try
                succeed (Directory.EnumerateFiles(path))
            with
                | ex -> failex ex

    module File =
        
        /// checks the existance of a file
        let exists filepath = succeed (File.Exists(filepath))

        /// creates or overwrites a random access file
        let create filepath = 
            try
                succeed (File.Create(filepath, 4096, FileOptions.RandomAccess))
            with
                | ex -> failex ex
            
        /// creates or overwrites a text file
        let createText filepath = 
            try
                succeed (File.CreateText(filepath))
            with
                | ex -> failex ex

        /// opens a file for access (used for binary random access files)
        let fopen filepath = 
            try
                succeed (File.Open(filepath, FileMode.Open))
            with
                | ex -> failex ex


        /// opens a text file for read access
        /// returns a StreamReader
        let fopenText filepath = 
            try
                succeed (File.OpenText(filepath))
            with
                | ex -> failex ex


        /// opens an existing file or creates a new file for writing
        // returns a StreamWriter
        let fopenWrite filepath = 
            try
                succeed (new StreamWriter(File.OpenWrite(filepath)))
            with
                | ex -> failex ex


        /// opens an existing file for read write access in append mode 
        let fopenAppend filepath = 
            try
                succeed (new StreamWriter( File.Open(filepath, FileMode.Append)))
            with
                | ex -> failex ex

        let writeline (writer: StreamWriter) (text: string) =
            try 
                writer.WriteLine(text) |> succeed
            with
                | ex -> failex ex

        let fclose (stream : Stream) = stream.Close()


        type IOBuilder() =
            member this.Bind(x,f) =
                match x with 
                | Success (e,_) -> f e
                | Failure msg   -> Failure msg

            
            member this.Return(x) = 
                match x with
                | Success (x,msg) -> Success (x,msg)
                | Failure msg     -> Failure msg
    
            member this.ReturnFrom(x) = x

            member this.Zero(x) = () |> succeed

            member this.TryFinally(body, compensation) =
                try 
                    this.ReturnFrom(body())
                finally 
                    compensation() 

            member this.Combine (a,b) = 
                match a with
                | Success _ -> a
                | Failure _ -> b

            member this.Delay(f) = f()

            member this.Using(disposable:#System.IDisposable, body) =
                let body' = fun () -> body disposable
                this.TryFinally(body', fun () -> 
                    match disposable with 
                        | null -> () 
                        | disp -> disp.Dispose())

        let iorunner = new IOBuilder()

    [<AutoOpen>]
    module Serialize =


        let serialize (t: 'T) (stream: Stream) =
            let ser = new DataContractJsonSerializer(t.GetType())
            let fmtStr = JsonReaderWriterFactory.CreateJsonWriter(stream, Text.Encoding.UTF8, true, true, "  " )
            ser.WriteObject(fmtStr, t)
            fmtStr.Flush()
            
        
        let deserialize<'T> (stream: Stream) =
            let result : 'T =  
                (new System.Runtime.Serialization.Json.DataContractJsonSerializer(typedefof<'T>)).ReadObject(stream) :?> 'T
            result
            


