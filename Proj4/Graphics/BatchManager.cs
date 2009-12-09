using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura.Graphics
{
    public static class BatchManager
    {
        public static Batch Current = new Batch();
    }

    public class BatchEntry
    {
        public Billboard visualization;
        public float c_dist;
        public DrawArgs args;

        public BatchEntry() { }
        public BatchEntry(Billboard b, float distance, DrawArgs dArgs)
        {
            visualization = b;
            c_dist = distance;
            args = dArgs;
        }
    }

    public class Batch : IDrawable
    {
        SortedSet<BatchEntry> batch = new SortedSet<BatchEntry>(new BComparer());

        public void Add(BatchEntry b)
        {
            batch.Add(b);
        }
        public void Remove(BatchEntry b)
        {
            batch.Remove(b);
        }
        public void Clear()
        {
            batch.Clear();
        }

        private class BComparer : IComparer<BatchEntry>
        {
            public int Compare(BatchEntry x, BatchEntry y)
            {
                if (x.c_dist == y.c_dist) return 0;
                return (x.c_dist > y.c_dist) ? 1 : -1;
            }
        }

        public void Draw()
        {

            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glDepthMask(Gl.GL_FALSE);
            foreach (BatchEntry r in batch)
            {
                r.visualization.DepthDraw(r.args);
            }
            Gl.glDepthMask(Gl.GL_TRUE);
            Gl.glDisable(Gl.GL_BLEND);
            
        }
    }
}
