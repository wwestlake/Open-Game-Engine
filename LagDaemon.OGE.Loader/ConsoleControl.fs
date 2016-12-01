namespace LagDaemon.OGE.Loader

open System
open System.Runtime.InteropServices

module ConsoleControl =

    let SW_HIDE = 0
    let SW_SHOW = 5

    [<DllImport( "kernel32.dll" )>]
    extern IntPtr GetConsoleWindow();

    [<DllImport("user32.dll")>]
    extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [<DllImport("user32.dll")>]
    extern bool SetForegroundWindow(IntPtr hWnd);

    let BringConsoleToFront() =
        SetForegroundWindow(GetConsoleWindow());

    let showConsole () =
        let handle = GetConsoleWindow()
        ShowWindow(handle, SW_SHOW)

    let hideConsole () =
        let handle = GetConsoleWindow()
        ShowWindow(handle, SW_HIDE)

