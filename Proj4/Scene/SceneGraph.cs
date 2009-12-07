using System;
using System.Collections.Generic;
using Aura.Core;
using System.Collections;
using Aura.Graphics;

namespace Aura.Scene
{
    [UnitTest(3)]
    public class SceneGraph : IEnumerable<SceneNode>
    {
        SceneNode Root { get; protected set; }

        public SceneGraph()
        {
            Root = new SceneNode(new Empty());
        }

        public void DrawSceneGraph()
        {
            DrawWorker(Root);
        }

        private void DrawWorker(SceneNode node)
        {
            IDrawable d = node.SceneObject as IDrawable;
            if (d != null) d.Draw();
            foreach (SceneNode n in node.Children)
            {
                if(!node.Hidden)
                    DrawWorker(node);
            }
        }

        public class SceneGraphIterator : IEnumerator<SceneNode>, IDisposable
        {
            private List<SceneNode> nodes = new List<SceneNode>();
            IEnumerator<SceneNode> enumerator;

            public SceneGraphIterator(SceneGraph graph)
            {
                IteratorWorker(graph.Root);
                enumerator = nodes.GetEnumerator();
            }

            private void IteratorWorker(SceneNode curr)
            {
                foreach (SceneNode node in curr.Children)
                {
                    IteratorWorker(node);
                }
                nodes.Add(curr);
            }

            #region IEnumerator Members

            public object Current
            {
                get { return enumerator.Current; }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                nodes = null;
                enumerator.Dispose();
                enumerator = null;
            }

            #endregion

            #region IEnumerator<SceneNode> Members

            SceneNode IEnumerator<SceneNode>.Current
            {
                get { return enumerator.Current; }
            }

            #endregion
        }

        #region IEnumerable<SceneNode> Members

        public IEnumerator<SceneNode> GetEnumerator()
        {
            return new SceneGraphIterator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(new SceneGraphIterator(this));
        }

        #endregion
    }
}
