using System;
using System.Collections;
using System.Collections.Generic;
using Aura.Core;
using Aura.Content;
using Aura.Graphics;
using Tao.OpenGl;

namespace Aura.Graphics.Assets
{
    public class Explosion : Object3D, IDrawable
    {
        EmitterBase core;
        EmitterBase fireBall;
        EmitterBase smoke;
        EmitterBase dust;
        EmitterBase rays;

        Billboard cloud;
        Billboard flash;
        Billboard circle;
        float Radius;
        Vector3 explosionPos;

        ParticleSystem debug;

        public Explosion(Vector3 position, float radius, float velocity)
        {
            Random random = new Random();
            explosionPos = position;

            Billboard particleBillboard = new Billboard(TextureImporter.Instance.ImportContent("Data/particle.png"));
            particleBillboard.Dimention = new Vector2(.1f, .1f);
             
            /* Create the particle systems for the core of the explosion */
            List<ParticleSystem> coreParticleSystems = new List<ParticleSystem>();
            ParticleSystem test = new ParticleSystem(
                3.0f,  // 3 second lifetime
                particleBillboard,   //Using particle.png as the texture
                new Color4InterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for color
                new ColorRange(new Color4(1, 1, 0, 1), new Color4(1, 0, 0, 1)),   // Yellow to Red
                new FloatInterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for speed
                new Range(1.0f, 0.0f));
            test.Count = 500;
            coreParticleSystems.Add(test);         //Speed from 1.0 to 0.0
            
            /* Build the core of the explosion */
            core = new EmitterBase(5.0, //Five seconds long
                coreParticleSystems, //This should be self-explanatory
                new DirectionalClamp(ClampState.None, ClampState.Positive, ClampState.None), //Nothing in the Negative Y
                random.Next(), //Seed the RNG
                true);  //Repeat!
            core.Emit();
        }

        public void Draw()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(explosionPos.X, explosionPos.Y, explosionPos.Z);
            Gl.glScalef(1, 1, 1);
            Gl.glDisable(Gl.GL_LIGHTING);
            foreach (ParticleSystem p in core.Systems)
            {
                p.Update();
                p.Draw();
            }


            BatchManager.Current.Draw();
            BatchManager.Current.Clear();

            if (LightManager.LightingEnabled)
            {
                Gl.glEnable(Gl.GL_LIGHTING);
            }
            Gl.glPopMatrix();
        }

        public void Update()
        {
            foreach (ParticleSystem p in core.Systems)
            {
                p.Update();
            }
        }
    }
}
