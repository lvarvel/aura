using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Aura.Core;

namespace Aura.Graphics
{
    public class Light : Object3D, IDisposable
    {
        #region Fields
        public Material lightMaterial;
        public bool directionalEnabled { get; set; }
        public int lightIndex { get; protected set; }
        #endregion

        #region Constants
        private static SortedList<int, bool> lightIndicies = new SortedList<int, bool>();
        private static bool initialized;
        private static readonly bool INDEX_USED = true;
        private static readonly bool INDEX_FREE = false;
        #endregion

        #region Constructors
        public Light()
        {
            AssignIndex();
        }
        public Light(Material material, bool directional = false)
        {
            directionalEnabled = directional;
            lightMaterial = material;
            AssignIndex();
        }
        public void Dispose()
        {
            lightIndicies[lightIndex] = INDEX_FREE;
            Gl.glDisable(Gl.GL_LIGHT0 + lightIndex);
        }
        #endregion

        #region Methods
        public void ApplyLight()
	    {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glEnable(Gl.GL_LIGHT1 + lightIndex);
            float[] pos = { position.X, position.Y, position.Z, (directionalEnabled) ? 0 : 1 };
            Gl.glLightfv(Gl.GL_LIGHT1 + lightIndex, Gl.GL_POSITION, pos);
            Gl.glLightfv(Gl.GL_LIGHT1 + lightIndex, Gl.GL_AMBIENT, (float[])lightMaterial.Ambient);
		    Gl.glLightfv(Gl.GL_LIGHT1 + lightIndex, Gl.GL_DIFFUSE, (float[])lightMaterial.Diffuse);
            Gl.glLightfv(Gl.GL_LIGHT1 + lightIndex, Gl.GL_SPECULAR, (float[])lightMaterial.Specular);
		    
	    }

        private void AssignIndex()
        {
            if (!initialized) Initialize();
            foreach (int i in lightIndicies.Keys)
            {
                if (lightIndicies[i] == INDEX_FREE)
                {
                    lightIndex = i;
                    lightIndicies[i] = INDEX_USED;
                    return;
                }
            }
            throw new InvalidOperationException("There are already 7 lights. OpenGL cannot handle more");
        }

        internal static void Initialize()
        {
            for (int i = 1; i < 8; ++i)
            {
                lightIndicies.Add(i, INDEX_FREE);
            }
            initialized = true;
        }
        #endregion

    }

    public static class LightManager
    {
        public static List<Light> Lights
        {
            get { return _lights; }
        }
        private static List<Light> _lights = new List<Light>();
        private static bool isLightingEnabled = false;
        public static bool LightingEnabled
        {
            get { return isLightingEnabled; }
            set
            {
                if (value)
                {
                    Gl.glEnable(Gl.GL_LIGHTING);
                }
                else
                {
                    Gl.glDisable(Gl.GL_LIGHTING);
                }
                isLightingEnabled = value;
            }
        }

        public static void ApplyLighting()
        {
            foreach (Light l in _lights)
            {
                l.ApplyLight();
            }
        }
    }
}
