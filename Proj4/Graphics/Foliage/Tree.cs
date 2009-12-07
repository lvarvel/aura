using System;
using System.Collections.Generic;
using Aura.Core;
using Tao.OpenGl;

namespace Aura.Graphics.Foliage
{
    public class Tree : IDrawable
    {
        public TreeBranch Root;
        public Billboard billBoard;
        internal DrawArgs args;
        public Vector3 Position;

        public Tree(Vector3 position, float treeHeight, float angleClamp, float dropOff, int number, 
            int depth, Billboard b)
        {
            args = new DrawArgs(null, new Color4(0, 1, 0));
            Root = new TreeBranch();
            Root.Position = position;
            var subr = new TreeBranch(treeHeight * dropOff, angleClamp, dropOff, number, new Vector3(0,1,0),
                position + new Vector3(0, treeHeight, 0), depth, b, Root, this);
            Root.Children.Add(subr);
            Position = position;
        }



        #region IDrawable Members

        public void Draw()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(Position.X, Position.Y, Position.Z);
            Gl.glScalef(1, 1, 1);
            Gl.glDisable(Gl.GL_LIGHTING);
            Root.Draw();

            if(LightManager.LightingEnabled)
                Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glPopMatrix();
        }

        #endregion
    }

    public abstract class TreePart : IDrawable
    {
        public abstract void Draw();
        public TreeBranch Parent { get; set; }
        public Tree Root { get; set; }
        public Vector3 Position;

        protected Vector3 getNext(float length, float angleClamp, Vector3 normal)
        {
            Vector3 rand = new Vector3((float)Util.r.NextDouble() * Util.r.NextSign(),
                (float)Util.r.NextDouble() * Util.r.NextSign(),
                (float)Util.r.NextDouble() * Util.r.NextSign());
            rand = rand * length;
            /*
            Vector3 result;
            Vector3 axis = rand.cross(normal);
            float theta = (float)Math.Acos(rand.dot(normal));
            theta = (theta > angleClamp) ? theta - (angleClamp * 2f) : theta;
            Quaternion rot = new Quaternion(theta, axis.X, axis.Y, axis.Z);
            result = rot.transformVector(rand);
            theta = (float)(Util.r.NextDouble() * Math.PI * 2);
            rot = new Quaternion(theta, normal.X, normal.Y, normal.Z);
            result = rot.transformVector(result);
            return result;
            */
            Quaternion rot = new Quaternion(angleClamp, rand.X, rand.Y, rand.Z);
            Vector3 result = rot.transformVector(normal);
            result = result;
            return result;
        }
    }

    public class TreeBranch : TreePart
    {
        internal List<TreePart> Children = new List<TreePart>();

        internal TreeBranch() { }
        internal TreeBranch(float c_length, float angleClamp, float dropOff, int number,
            Vector3 normal, Vector3 position, int depthCounter, Billboard b, TreeBranch parent, Tree root)
        {
            List<Vector3> points = new List<Vector3>();
            Position = position;
            Parent = parent;
            Root = root;
            for (int i = 0; i < number; ++i)
            {
                points.Add(getNext(c_length, angleClamp, normal) + position);
            }

            if (depthCounter > 0)
            {
                foreach(Vector3 point in points)
                {
                    Children.Add(new TreeBranch(c_length * dropOff, angleClamp, dropOff, number,
                        (point - position).normalize(), point, depthCounter-1, b, this, root));
                }
            }
            else
            {
                foreach(Vector3 leaf in points)
                {
                    Children.Add(new TreeLeaf(leaf, b, this, root));
                }
            }
        }

        public override void Draw()
        {
            if (Parent != null)
            {
                Gl.glLineWidth(3);
                //Gl.glColor3f(.522f, .152f, 0.0f); //brown
                Gl.glColor3f(1,0,0);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex3fv((float[])Position);
                Gl.glVertex3fv((float[])Parent.Position);
                Gl.glEnd();
            }
            foreach (TreePart p in Children)
            {
                p.Draw();
            }
        }
    }

    public class TreeLeaf : TreePart
    {
        internal Billboard leaf;

        internal TreeLeaf(Vector3 position, Billboard b, TreeBranch parent, Tree root)
        {
            Position = position;
            leaf = b;
            Parent = parent;
            Root = root;
        }

        public override void Draw()
        {
            Root.args.Position = Position;
            //leaf.Draw(Root.args);
        }
    }
}
