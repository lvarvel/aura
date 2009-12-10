using System;
using System.Collections.Generic;
using Aura.Content;
using Aura.Core;

namespace Aura.Graphics
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
        protected class Particle : IPoolable<float3>
        {
            public float3 position;
            /// <summary>
            /// Note: Be careful of the magnitude of this vector
            /// </summary>
            public float3 velocity;
            public float life;

            public void Build(params float3[] paramters)
            {
                position = paramters[0];
                velocity = paramters[1];
                life = 0;
            }

            public void Clean()
            {
                position.x = 0;
                position.y = 0;
                position.z = 0;
                velocity.x = 0;
                velocity.y = 0;
                velocity.z = 0;
                life = 0;
            }

            public void Build(params object[] parameters)
            {
                position = (float3)parameters[0];
                velocity = (float3)parameters[1];
                life = 0;
            }
        }
        protected static Pool<Particle> particlePool = new Pool<Particle>(100);
        protected struct float3
        {
            public float x;
            public float y;
            public float z;

            public float3(float X, float Y, float Z)
            {
                x = X;
                y = Y;
                z = Z;
            }
            public float3(Vector3 v)
            {
                x = v.X;
                y = v.Y;
                z = v.Z;
            }

            public static float3 operator +(float3 lhs, float3 rhs)
            {
                float3 result = new float3();
                result.x = lhs.x + rhs.x;
                result.y = lhs.y + rhs.y;
                result.z = lhs.z + rhs.z;
                return result;
            }
            public static float3 operator *(float3 lhs, float rhs)
            {
                float3 result = new float3();
                result.x = lhs.x * rhs;
                result.y = lhs.y * rhs;
                result.z = lhs.z * rhs;
                return result;
            }
        }
        #endregion

        #region Fields
        protected List<Particle> particles = new List<Particle>();
        protected List<Emitter> emitters = new List<Emitter>();

        public Color4InterpolationHandler colorHandler;
        public ColorRange colorRange;
        public FloatInterpolationHandler speedHandler;
        public Range speedRange;

        public Vector3 constantForce;
        public IVisualization visualization;
        public bool lightingEnabled = false;
        public float maxLife { get; set; }
        public int Count { get; set; }
        public List<Emitter> Emitters 
        { 
            get { return emitters; } 
        }
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
            foreach (Particle p in particles)
            {
                float f = p.life / maxLife;
                float v = speedHandler(speedRange, f);
                p.position += p.velocity * v;
            }
        }

        public virtual void Draw()
        {
            DrawArgs args = new DrawArgs(new Vector3(), Color4.White);
            Vector3 s = new Vector3();
            Vector3 v = new Vector3();
            args.Position = s;
            args.Vector = v;

            foreach (Particle p in particles)
            {
                s.AssignValues(p.position.x, p.position.y, p.position.z);
                v.AssignValues(p.velocity.x, p.velocity.y, p.velocity.z);
                float f = p.life / maxLife;
                args.Color = colorHandler(colorRange, f);
                args._Material = null;
                args.LightingEnabled = false;

                visualization.Draw(args);
            }
        }

        public virtual void AddParticle(Vector3 position, Vector3 velocity)
        {
            //Grab particle from particle pool
            particles.Add(particlePool.New<float3>(new float3(position), new float3(velocity)));
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
