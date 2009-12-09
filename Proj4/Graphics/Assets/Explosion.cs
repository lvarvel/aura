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
        EmitterBase sparks;

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

            #region Dust
            Billboard dustParticleBillboard = new Billboard(TextureImporter.Instance.ImportContent("Data/particle.png"));
            dustParticleBillboard.Dimention = new Vector2(.1f, .1f);
             
            /* Create the particle systems for the dust of the explosion */
            List<ParticleSystem> dustParticleSystems = new List<ParticleSystem>();
            ParticleSystem shockwave = new ParticleSystem(
                20f,  // 10... um.... units.
                dustParticleBillboard,   //Using particle.png as the texture
                1.0f,
                new Color4InterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for color
                new ColorRange(new Color4(0.250f, 0.135f, 0.0372f, 1), new Color4(0.543f, 0.270f, 0.0742f, 0.0f)),   // Brown, removing Alpha
                new FloatInterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for speed
                new Range(0.75f, 0.0f));
            shockwave.Count = 500;
            dustParticleSystems.Add(shockwave);         //Speed from 1.0 to 0.0
            
            /* Build the dust */
            dust = new EmitterBase(75, //75 umm... MS?
                dustParticleSystems, //This should be self-explanatory
                new DirectionalClamp(ClampState.None, ClampState.Positive, ClampState.None), //Nothing in the Negative Y
                random.Next(), //Seed the RNG
                false);  //Repeat!

            dust.Emit();
            #endregion

            #region Core
            Billboard coreParticleBillboard = new Billboard(TextureImporter.Instance.ImportContent("Data/particle.png"), BillboardLockType.Spherical, 1.0f);
            coreParticleBillboard.Dimention = new Vector2(0.5f, .05f);

            /* Create the particle systems for the core of the explosion */
            List<ParticleSystem> coreParticleSystems = new List<ParticleSystem>();
            ParticleSystem hotcore = new ParticleSystem(
                10f,  // 10... um.... units.
                coreParticleBillboard,   //Using particle.png as the texture
                0.1f,
                new Color4InterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for color
                new ColorRange(new Color4(1.0f, 1.0f, 1.0f, 1.0f), new Color4(1.0f, 0.9f, 0.0f, 1.0f)),   // Yellow-orange, going dark
                new FloatInterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for speed
                new Range(0.75f, 0.0f));
            hotcore.Count = 100;
            coreParticleSystems.Add(hotcore);         //Speed from 1.0 to 0.0

            /* Build the core of the explosion */
            core = new EmitterBase(75, //75 umm... MS?
                coreParticleSystems, //This should be self-explanatory
                new DirectionalClamp(ClampState.None, ClampState.Negative, ClampState.None), //Nothing in the Negative Y
                random.Next(), //Seed the RNG
                false);  //Repeat!
            core.Emit();
            #endregion

            #region Sparks
            Billboard sparkParticleBillboard = new Billboard(TextureImporter.Instance.ImportContent("Data/particle.png"), BillboardLockType.Spherical, 0.05f);
            sparkParticleBillboard.Dimention = new Vector2(0.5f, .05f);

            /* Create the particle systems for the sparks */
            List<ParticleSystem> sparkParticleSystems = new List<ParticleSystem>();
            ParticleSystem sparkles = new ParticleSystem(
                10f,  // 10... um.... units.
                sparkParticleBillboard,   //Using particle.png as the texture
                0.1f,
                new Color4InterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for color
                new ColorRange(new Color4(1.0f, 1.0f, 1.0f, 1.0f), new Color4(1.0f, 0.9f, 0.0f, 1.0f)),   // White, going yellow
                new FloatInterpolationHandler(FunctionAssets.LinearInterpolation),  //Linear Interpolation for speed
                new Range(0.75f, 0.0f));
            sparkles.Count = 100;
            coreParticleSystems.Add(sparkles);         //Speed from 1.0 to 0.0

            /* Build the sparks */
            core = new EmitterBase(75, //75 umm... MS?
                coreParticleSystems, //This should be self-explanatory
                new DirectionalClamp(ClampState.None, ClampState.Negative, ClampState.None), //Nothing in the Negative Y
                random.Next(), //Seed the RNG
                false);  //Repeat!
            core.Emit();
            #endregion
        }

        public void Draw()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(explosionPos.X, explosionPos.Y, explosionPos.Z);
            Gl.glDisable(Gl.GL_LIGHTING);
            foreach (ParticleSystem p in dust.Systems)
            {
                p.Update();
                p.Draw();
            }
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
            foreach (ParticleSystem p in dust.Systems)
            {
                p.Update();
            }
            foreach (ParticleSystem p in core.Systems)
            {
                p.Update();
            }
        }
    }
}
