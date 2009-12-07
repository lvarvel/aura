using System;
using System.Collections.Generic;

namespace Aura.Core
{
    public abstract class Object3D : IHasID
    {
        public Vector3 position;
        public float scale;
        public Quaternion rotation;
        int _id;

        public Scene.SceneNode SceneNode;

        protected Object3D() 
        {
            position = new Vector3(0, 0, 0);
            scale = 1;
            rotation = new Quaternion(0, 0, 1, 0);
            _id = IDManager.NewID;
        }

        #region IHasID Members

        public int ID
        {
            get { return _id; }
        }

        #endregion
    }
}
