using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Tao.Sdl;

namespace Aura
{
    /// <summary>
    /// Class that encapsulates the current program status.
    /// </summary>
    public partial class Engine : IDisposable
    {
        internal int x;
        internal int y;
        private IntPtr screen;
        internal static Engine Instance;
        internal string windowName;

        public Engine(int screenWidth = 800, int screenHeight = 600, string window_name = "Aura Particle Simulator")
        {
            x = screenWidth;
            y = screenHeight;
            Instance = this;
            windowName = window_name;
        }
        
        /// <summary>
        /// Runs the engine
        /// </summary>
        public void Run()
        {
            Prime();
            Running = true;
            Sdl.SDL_Event sdlEvent;
            while (Running)
            {
                Sdl.SDL_PollEvent(out sdlEvent);
                this.HandleInput(sdlEvent);

                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            }
        }

        /// <summary>
        /// Initializes the engine
        /// </summary>
        internal void Prime()
        {
            if (Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO) != 0) throw new AuraEngineException("SDL failure: " + Sdl.SDL_GetError());
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);

            screen = Sdl.SDL_SetVideoMode(x, y, 16, Sdl.SDL_OPENGL | Sdl.SDL_DOUBLEBUF);
            if (screen == IntPtr.Zero) throw new AuraEngineException("Unable to set video mode: " + Sdl.SDL_GetError());
            Sdl.SDL_WM_SetCaption(windowName,windowName);

            Gl.glViewport(100, 100, x, y);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
        }

        public string WindowName
        {
            get { return windowName; }
            set 
            {
                windowName = value;
                Sdl.SDL_WM_SetCaption(windowName, windowName);
            }
        }
        public bool Running { get; set; }

        public void Dispose()
        {
            Sdl.SDL_Quit();
            screen = IntPtr.Zero;
            Instance = null;
        }
    }

    public class AuraEngineException : Exception
    {
        internal AuraEngineException() : base() { }
        internal AuraEngineException(string errorMessage, Exception innerException = null) : base(errorMessage, innerException) { }
    }
}
