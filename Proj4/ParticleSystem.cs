using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Represents an instance of a single effect particle system.  To combine
    /// particle system effects, create a class that encapsulates more than one
    /// ParticleSystem.
    /// </summary>
    public class ParticleSystem : IDrawable
    {
        /// <summary>
        /// An internal struct.  Particles could be represented using just an
        /// integer (or float, for that matter) life, but the complexities of
        /// physics, especially when rigged with collision, make it a lot easier
        /// if we just encapsulate the useful physics data here.
        /// </summary>
        protected struct Particle
        {
            float3 position;
            float3 velocity;
            float life;
        }

        protected struct float3
        {
            float x;
            float y;
            float z;
        }

        protected List<Particle> particles = new List<Particle>();
        public Color4InterpolationHandler colorHandler;
        public FloatInterpolationHandler speedHandler;
        public Vector3InterpolationHandler emissionHandler;

        public Vector3 constanceForce;

        public ParticleSystem()
        {
            constanceForce = new Vector3(0,0,0);
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            
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
    public delegate Color4 Color4InterpolationHandler(Color4 first, Color4 second, float p);

    /// <summary>
    /// Represents a simple F(x) mathematical function.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate float FunctionHandler(float x);
}
