using System;
using System.Collections.Generic;
using Aura.Core;

namespace Aura.Graphics.Assets
{
    public class Explosion : Object3D
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

        public Explosion(Vector3 position, float radius, float velocity)
        {

        }
    }
}
