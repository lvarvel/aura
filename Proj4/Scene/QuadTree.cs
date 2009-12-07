using System;
using System.Collections.Generic;
using Aura.Core;

namespace Aura.Scene
{
    public class QuadTree : QuadTreePart
    {
        private QuadTreePart root;

        public override bool IsContained(Object3D obj)
        {
            return root.IsContained(obj);
        }

        public override void Build(List<Object3D> objects)
        {
            root.Build(objects);
        }

        public override void Update(Object3D obj)
        {
            root.Update(obj);
        }
    }

    public abstract class QuadTreePart : IHasID
    {
        public abstract bool IsContained(Object3D obj);
        public abstract void Build(List<Object3D> objects);
        public abstract void Update(Object3D obj);

        public QuadTree Root { get; protected set; }
        public QuadTreePart Parent { get; protected set; }

        protected QuadTreePart()
        {
            ID = IDManager.NewID;
        }
        protected QuadTreePart(QuadTreePart parent)
        {
            ID = IDManager.NewID;
            Root = parent.Root;
            Parent = parent;
        }

        #region IHasID Members

        public int ID { get; protected set; }

        #endregion
    }

    public class QuadTreeLeaf : QuadTreePart
    {
        List<QuadTreePart> Children;

        public override bool IsContained(Object3D obj)
        {
            throw new NotImplementedException();
        }

        public override void Build(List<Object3D> objects)
        {
            throw new NotImplementedException();
        }

        public override void Update(Object3D obj)
        {
            throw new NotImplementedException();
        }
    }
}
