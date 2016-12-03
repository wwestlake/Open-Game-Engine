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

namespace LagDaemon.OGE.InterfaceTypes


module TimeKeeper =

        open System.Runtime.InteropServices

        type FrameData = {
            frame: int;
            deltaTime: float;
            fps: float;
            start: int64;
            current: int64;
            frequency: int64;
        }

        let initFrameData = {
            frame = 0;
            deltaTime = 0.0;
            fps = 0.0;
            start = 0L;
            current = 0L;
            frequency = 0L;
        }

        [<DllImport("KERNEL32")>]
        extern bool QueryPerformanceCounter(int64 & lpPerformanceCount);

        [<DllImport("Kernel32.dll")>]
        extern bool QueryPerformanceFrequency(int64 & lpFrequency);

        type TimeOfDay = {
            day: int64;
            hours: int64;
            minutes: int64;
            seconds: int64;
        }

        let createTimeOFDay d h m s = {day = d; hours = h; minutes = m; seconds = s}

        type GameTime = {
            deltaTime: float;
            gameRunTime: TimeOfDay;
            gameScaleTime: TimeOfDay;
            timeScale: float;
        }

        let createRunTime frameData =
            let {start = s; current = c; frequency = f } = frameData
            let time = int64 (((float c) - (float s)) /  (float f))
            let days = time / (60L * 60L * 24L)
            let time2 = time - days * (60L * 60L * 24L)
            let hours = time2 / (60L * 60L)
            let time3 = time2 - hours * (60L * 60L)
            let minutes = time3 / 60L
            let time4 = time3 - minutes * 60L
            {
                day = days;
                hours = hours;
                minutes = minutes;
                seconds = time4;
            }

        type MasterTime() as this =
            let mutable _frequency = 0L
            let mutable _start = 0L
            let mutable _stop = 0L
            let mutable _startCount = 0L
            let mutable _currentCount = 0L
            
            let freqOk = QueryPerformanceFrequency(&_frequency)
            do QueryPerformanceCounter(&_startCount) |> ignore

            do this.start() |> ignore

            member this.start () = 
                QueryPerformanceCounter(&_start)

            member this.stop () =
                let result = QueryPerformanceCounter(&_stop)
                do _currentCount <- _stop
                result

            member this.elapsed () =
                _stop - _start

            member this.elapsedTime () =
                do this.stop() |> ignore
                let time = this.elapsed ()
                do this.start() |> ignore
                (float time) / (float _frequency)

            member this.getFrameData { frame = frame } =
                let time = this.elapsedTime()
                {
                    frame = frame + 1;
                    deltaTime = time;
                    fps = 1.0 / time;
                    start = _startCount;
                    current = _currentCount;
                    frequency = _frequency;
                }

        let masterTime = new MasterTime()

