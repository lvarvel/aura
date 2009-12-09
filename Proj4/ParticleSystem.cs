using System;
using System.Collections.Generic;
using Aura.Content;

namespace Aura
{
    /// <summary>
    /// Represents an instance of a single effect particle system.  To combine
    /// particle system effects, create a class that encapsulates more than one
    /// ParticleSystem.
    /// </summary>
    public class ParticleSystem : IDrawable, IDisposable, IBuildable<ParticleSystem>
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
            public float x;
            public float y;
            public float z;
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

        #region Constructors
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
        #endregion

        #region Methods
        public virtual void Update()
        {
            //TODO
        }

        public virtual void Draw()
        {
            DrawArgs args = new DrawArgs(new Vector3(), Color4.White);
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
        #endregion

        #region IBuildable Members
        public void Build(Descriptor<ParticleSystem> descriptor)
        {
            var d = descriptor as ParticleSystemDescriptor;

            colorRange = d.colorRange;
            speedRange = d.speedRange;
            constantForce = d.constantForce;
            visualization = d.visualization;
            maxLife = d.maxLife;
            lightingEnabled = d.lightingEnabled;

            colorHandler = DelegateConverter.GetDelegate(d.colorHandler, typeof(Color4InterpolationHandler)) as Color4InterpolationHandler;
            speedHandler = DelegateConverter.GetDelegate(d.speedHandler, typeof(FloatInterpolationHandler)) as FloatInterpolationHandler;
        }
        public Type DescriptorType
        {
            get { return typeof(ParticleSystemDescriptor); }
        }
        public Descriptor<ParticleSystem> GetDescriptor
        {
            get 
            {
                var result = new ParticleSystemDescriptor();
                result.colorHandler = DelegateConverter.GetName(colorHandler);
                result.speedHandler = DelegateConverter.GetName(speedHandler);
                result.constantForce = constantForce;
                result.visualization = visualization;
                result.speedRange = speedRange;
                result.colorRange = colorRange;
                result.lightingEnabled = lightingEnabled;
                return result;
            }
        }
        #endregion
    }

    public class ParticleSystemDescriptor : Descriptor<ParticleSystem>
    {
        public string colorHandler;
        public ColorRange colorRange;
        public string speedHandler;
        public Range speedRange;

        public Vector3 constantForce;
        public IVisualization visualization;
        public bool lightingEnabled = false;
        public float maxLife;
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
