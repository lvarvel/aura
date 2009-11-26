using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Represents an instance of a single effect particle system.  To combine
    /// particle system effects, create a class that encapsulates more than one
    /// ParticleSystem.
    /// </summary>
    public class ParticleSystem : IDrawable, IDisposable
    {
        #region Declarations
        /// <summary>
        /// An internal class.  Particles could be represented using just an
        /// integer (or float, for that matter) life, but the complexities of
        /// physics, especially when rigged with collision, make it a lot easier
        /// if we just encapsulate the useful physics data here.
        /// </summary>
        protected class Particle
        {
            public float3 position;
            public float3 velocity;
            public float life;
        }
        protected static Pool<Particle> particlePool = new Pool<Particle>();
        protected struct float3
        {
            float x;
            float y;
            float z;
        }
        #endregion

        #region Fields
        protected List<Particle> particles = new List<Particle>();

        public Color4InterpolationHandler colorHandler;
        public ColorRange colorRange;
        public FloatInterpolationHandler speedHandler;
        public Range speedRange;

        public Vector3 constantForce;
        public IVisualization visualization;
        public bool lightingEnabled = false;
        public float maxLife;
        #endregion

        public ParticleSystem()
        {
            constantForce = new Vector3(0,0,0);
        }
        public ParticleSystem(float max_life, IVisualization _visualization = null, Color4InterpolationHandler color_handler = null, ColorRange color_range = null,
            FloatInterpolationHandler speed_handler = null, Range speed_range = null)
        {
            maxLife = max_life;
            colorHandler = color_handler;
            colorRange = color_range;
            speedHandler = speed_handler;
            speedRange = speed_range;
            visualization = _visualization;
        }

        public virtual void Update()
        {
            //TODO
        }

        public virtual void Draw()
        {
            DrawArgs args = new DrawArgs();
            foreach (Particle p in particles)
            {
                float f = p.life / maxLife;
                
            }
        }

        public virtual void Dispose()
        {
            foreach (Particle p in particles)
            {
                particlePool.Return(p);
            }
            particles.Clear();
        }
    }

    /// <summary>
    /// Represents an interpolation over a range of floats.
    /// </summary>
    /// <param name="range">The input minimum and maximum</param>
    /// <param name="p">The interpolation percentage, in decimal form</param>
    /// <returns></returns>
    public delegate float FloatInterpolationHandler(Range range, float p);
    /// <summary>
    /// Represents an interpolation over a range of Vector3s.
    /// </summary>
    /// <param name="first">The minimum Vector</param>
    /// <param name="second">The maximum Vector</param>
    /// <param name="p">The interpolation percentage, in decimal form</param>
    /// <returns></returns>
    public delegate Vector3 Vector3InterpolationHandler(Vector3 first, Vector3 second, float p);
    /// <summary>
    /// Represents an interpolation over a color
    /// </summary>
    /// <param name="first">The minimum color</param>
    /// <param name="second">The maximum color</param>
    /// <param name="p">The interpolation percentage, in decimal form</param>
    /// <returns></returns>
    public delegate Color4 Color4InterpolationHandler(ColorRange range, float p);

    /// <summary>
    /// Represents a simple F(x) mathematical function.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate float FunctionHandler(float x);
}
