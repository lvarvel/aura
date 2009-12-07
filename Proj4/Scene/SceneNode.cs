using System;
using System.Collections.Generic;
using Aura.Core;

namespace Aura.Scene
{
    public class SceneNode : IDisposable
    {
        public Object3D SceneObject;
        public List<SceneNode> Children { get; protected set; }
        public List<SceneNode> BackEdges { get; protected set; }
        public SceneNode Parent { get; set; }

        public Vector3 Position 
        {
            get
            {
                return SceneObject.position;
                Reset();
            }
            set
            {

                SceneObject.position = value;
            }
        }
        public Quaternion Rotation 
        {
            get
            {
                return SceneObject.rotation;
            }
            set
            {
                SceneObject.rotation = value;
                Reset();
            }
        }
        public float Scale 
        {
            get
            {
                Reset();
                return SceneObject.scale;
            }
            set
            {
                SceneObject.scale = value;
            }
        }

        public bool Hidden = false;

        private Transform s_transform = null;

        public SceneNode()
        {
            Children = new List<SceneNode>();
            BackEdges = new List<SceneNode>();
        }
        public SceneNode(Object3D item)
            : this()
        {
            SceneObject = item;
        }

        public Matrix MatrixTransform
        {
            get
            {
                return new Matrix(Position, Rotation, Scale);
            }
        }

        public Transform WorldTransform
        {
            get
            {
                if (s_transform != null) return s_transform;

                Matrix current = this.MatrixTransform;
                for (SceneNode itr = this; itr != null; itr = itr.Parent)
                {
                    current = itr.MatrixTransform * current;
                }
                return current.getTransform();
            }
        }

        internal void Reset()
        {
            s_transform = null;
            foreach (SceneNode n in Children)
            {
                n.Reset();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            SceneObject = null;
            Children = null;
            BackEdges = null;
            Parent = null;
        }

        #endregion
    }

    public class Empty : Object3D
    {
        public Empty() : base() { }
    }
}
