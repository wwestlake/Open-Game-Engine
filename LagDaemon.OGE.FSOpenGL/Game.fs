
namespace LagDaemon.OGE.FSOpenGL

module Game =

    open System
    open System.Drawing
    open OpenTK
    open OpenTK.Graphics.OpenGL



    type Game(gameWindow: GameWindow) as x =
        let window = gameWindow

        do window.Load.Add(fun e -> x.load(e))
        do window.Resize.Add(fun e -> x.resize(e))
        do window.UpdateFrame.Add(fun e -> x.updateFrame(e))
        do window.RenderFrame.Add(fun e -> x.renderFrame(e))
        do window.Closing.Add(fun e -> x.closing(e))

        member x.load(e) =
            do ()
        member x.resize(e) =
            do ()
        member x.updateFrame(e) =
            do ()
        member x.renderFrame(e) =
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);
            window.SwapBuffers();

        member x.closing(e) =
            do ()
        member x.run() =
            window.Run()

        
