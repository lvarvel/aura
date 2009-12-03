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
        public bool Disposed = false;

        //Debug
        Model m;

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

                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Update();
                Draw();

                try 
                { 
                    if(!Disposed)
                        Sdl.SDL_GL_SwapBuffers(); 
                }
                catch (AccessViolationException e) {  }
            }
        }

        public void Draw()
        {
            
            CameraManager.Current.ApplyCameraTransforms();
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
            LightManager.ApplyLighting();
            Gl.glPopMatrix();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glPushMatrix();
            //Debug
            
            //Glu.gluSphere(Glu.gluNewQuadric(), 1, 36, 36);

            m.Draw();
            
            Gl.glPopMatrix();
        }
        public void Update()
        {
            m.rotation = m.rotation * new Quaternion(.03f, 0,1,0 );
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

            Gl.glViewport(0, 0, x, y);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DEPTH_SIZE, 16);
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            CameraManager.SetCamera("Default", new Camera(new Vector3(0, 5, 10), new Vector3(0, 0, 0)));

            //DEBUG
            LightManager.LightingEnabled = true;
            Material lmaterial = new Material(new Color4(.1f, .1f, .1f, .1f), new Color4(1,0,0), new Color4(1,1,1), .1f);
            Light l = new Light(lmaterial, false);
            l.position = new Vector3(5,5,5);
            LightManager.Lights.Add(l);
            m = new Model(Mesh.Cube);

            
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
            Disposed = true;
            Environment.Exit(0);
        }
    }

    public class AuraEngineException : Exception
    {
        internal AuraEngineException() : base() { }
        internal AuraEngineException(string errorMessage, Exception innerException = null) : base(errorMessage, innerException) { }
    }
}
