using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Aura.Core;

namespace Aura.Graphics
{
    /// <summary>
    /// Class that describes an emitter
    /// </summary>
    public abstract class Emitter : Object3D, ITimedEntity, IDisposable
    {
        #region Fields
        public Range VelocityFactor;
        public Timer Timer;
        public List<ParticleSystem> Systems;
        

        private Random random;
        #endregion

        #region Constructors
        public Emitter() : base()
        {
            random = new Random();
        }
        protected Emitter(double period, List<ParticleSystem> particleSystems, int? rSeed = null, bool repeat = false) 
            : base()
        {
            random = (!rSeed.HasValue) ? new Random() : new Random(rSeed.Value);
            Systems = particleSystems;
            foreach (ParticleSystem p in particleSystems)
            {
                p.Emitters.Add(this);
            }
            Timer = new Timer(period, repeat);
            Timer.TimerEvent += HandleTimer;
        }
        #endregion

        #region Methods
        public abstract void Emit();

        public void HandleTimer(object sender, EventArgs e)
        {
            Emit();
        }

        public virtual void Dispose()
        {
            Timer.Dispose();
            Timer = null;
            Systems = null;
        }

        public bool isRunning
        {
            get { return Timer.isRunning; }
        }
        #endregion
    }

    /// <summary>
    /// A simple, point implementation of an emitter
    /// </summary>
    public class EmitterBase : Emitter
    {
        #region Fields
        public DirectionalClamp Clamp;
        #endregion

        #region Constructors
        public EmitterBase() : base() { }
        public EmitterBase(double period, List<ParticleSystem> particleSystems, DirectionalClamp clamp, int? rSeed = null, bool repeat = false)
            : base(period, particleSystems, rSeed, repeat)
        {
            Clamp = clamp;
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Methods
        public override void Emit()
        {
            Vector3 l_position = this.position;
            foreach (ParticleSystem p in Systems)
            {
                for (int i = 0; i < p.Count; ++i)
                {
                    Vector3 l_velocity = Util.GetRandomEmissionNormal(Clamp); //Grab a random velocity based on the clamp
                    p.AddParticle(l_position, l_velocity);
                    Vector3Pool.Instance.Return(l_velocity); //Give the pool back its particle
                }
            }
        }
        #endregion
    }

    //TODO: Make a mesh emitter class
}
