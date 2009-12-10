using System;
using System.Collections.Generic;

namespace Aura.Core
{
    public abstract class Object3D
    {
        public Vector3 position;
        public float scale;
        public Quaternion rotation;

        protected Object3D() 
        {
            position = new Vector3(0, 0, 0);
            scale = 1;
            rotation = new Quaternion(0, 0, 1, 0);
        }
    }
}
