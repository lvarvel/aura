using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Tao.Sdl;
using Tao.DevIl;
using Aura.Graphics;
using Aura.Content;
using Aura.Graphics.Assets;

namespace Aura.Core
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

        internal TimerManager timerManager = TimerManager.Instance;

        //Debug
        Model m;
        Billboard b;
        Emitter e;
        Explosion mrExplody;
        ParticleSystem ps;
        Tree tree;

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

            #region Game Loop
            while (Running)
            {
                Sdl.SDL_PollEvent(out sdlEvent);
                this.HandleInput(sdlEvent);

                timerManager.Update();
                Update();
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Draw();

                if(!Disposed)
                    Sdl.SDL_GL_SwapBuffers(); 
            }
            #endregion
        }

        public void Draw()
        {
            CameraManager.Current.ApplyCameraTransforms();
            Gl.glPushMatrix();
           
            LightManager.ApplyLighting();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            //Debug
            
            //Glu.gluSphere(Glu.gluNewQuadric(), 1, 36, 36);
            m.Draw();
            tree.Draw();

            mrExplody.Draw();
            //ps.Draw();

            Gl.glPopMatrix();
        }
        public void Update()
        {
            //Debug Lighting
            //m.rotation = m.rotation * new Quaternion((float)(Math.PI / 100), 0,1,0 );

            //Debug particles
            ps.Update();

            mrExplody.Update();
        }

        /// <summary>
        /// Initializes the engine
        /// </summary>
        internal void Prime()
        {
            #region Initialize SDL
            if (Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO) != 0) throw new AuraEngineException("SDL failure: " + Sdl.SDL_GetError());
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);

            screen = Sdl.SDL_SetVideoMode(x, y, 16, Sdl.SDL_OPENGL | Sdl.SDL_DOUBLEBUF);
            if (screen == IntPtr.Zero) throw new AuraEngineException("Unable to set video mode: " + Sdl.SDL_GetError());
            Sdl.SDL_WM_SetCaption(windowName,windowName);
            #endregion

            #region Initialize openGL
            Gl.glViewport(0, 0, x, y);
            Gl.glClearColor(0.8f, 0.8f, 0.8f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DEPTH_SIZE, 16);
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            //Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glAlphaFunc(Gl.GL_GREATER, .016f);
            Gl.glEnable(Gl.GL_ALPHA_TEST);
            #endregion

            #region Initialize ilut
            Il.ilInit();
            Ilu.iluInit();
            Ilut.ilutInit();
            Ilu.iluImageParameter(Ilu.ILU_PLACEMENT, Ilu.ILU_UPPER_LEFT);
            Ilut.ilutRenderer(Ilut.ILUT_OPENGL);
            #endregion

            CameraManager.SetCamera("Default", new Camera(new Vector3(10, 10, 10), new Vector3(0, 0, 0)));

            #region Set up explosion
            mrExplody = new Explosion(new Vector3(1, 1, 1), 5.0f, 5.0f);
            #endregion

            #region DEBUG
            //DEBUG: LIGHTING (BROKEN)
            LightManager.LightingEnabled = true;
            Texture t = TextureImporter.Instance.ImportContent("Data/grass.jpg");
            Texture leaf = TextureImporter.Instance.ImportContent("Data/leaf.png");
            Material lmaterial = new Material(new Color4(.1f, .1f, .1f, .1f), new Color4(.1f,0,0), new Color4(.1f,.1f,.1f), .1f);
            Light l = new Light(lmaterial, false);
            l.position = new Vector3(0,15,5);
            LightManager.Lights.Add(l);
            m = new Model(ObjImporter.Instance.ImportContent("Data/plane.obj"), t);
            m.scale = 1f;

            //Debug: Particles
            
            b = new Billboard(leaf, BillboardLockType.Spherical);
            b.Dimention = new Vector2(.1f, .1f);
            PointVisualization v = new PointVisualization();

            ps = new ParticleSystem(300, b, FunctionAssets.LinearInterpolation, new ColorRange(new Color4(1, 1, 1)), FunctionAssets.LinearInterpolation, new Range(.1f));
            ps.Count = 5;
            tree = new Tree(new Vector3(0,0,0), 3.0f, 1.5f, .3f, 3, 3, b);
            
            ParticleSystem[] tps = { ps };
            //e = new EmitterBase(1, tps, DirectionalClamp.ZeroClamp, null, true);
            #endregion
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
