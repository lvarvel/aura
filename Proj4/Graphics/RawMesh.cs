using System;
using System.Collections.Generic;
using Aura.Core;
using Tao.OpenGl;

namespace Aura.Graphics
{
    public class RawMesh : IDrawable
    {
        public class Polygon : IDrawable
        {
            public int size;
            public List<Vector3> Verts;
            public List<Vector3> Normals;
            public List<Vector2> UVCoords;

            public Polygon()
            {
                Verts = new List<Vector3>();
                Normals = new List<Vector3>();
                UVCoords = new List<Vector2>();
                size = 0;
            }

            public void Draw()
            {
                if (size == 3)
                    Gl.glBegin(Gl.GL_TRIANGLES);
                else if (size == 4)
                    Gl.glBegin(Gl.GL_QUADS);
                else throw new Exception();


                for (int i = 0; i < size; ++i)
                {
                    if(Normals.Count > 0)
                        Gl.glNormal3fv((float[])Normals[i]);
                    if (UVCoords.Count > 0)
                        Gl.glTexCoord2f(UVCoords[i].X, UVCoords[i].Y);
                    Gl.glVertex3fv((float[])Verts[i]);
                }

                Gl.glEnd();
            }
        }

        public List<Polygon> Polygons { get; protected set; }
        public Material Material { get; set; }

        public RawMesh()
        {
            Polygons = new List<Polygon>();
            Material = new Material();
        }

        #region IDrawable Members

        public void Draw()
        {
            Material.ApplyMaterial();
            foreach (Polygon p in Polygons)
            {
                p.Draw();
            }
        }

        public Mesh CompileToDL()
        {
            int handle = Gl.glGenLists(IDManager.NewID);
            Mesh result = new Mesh(handle);
            Gl.glNewList(handle, Gl.GL_COMPILE);
            Draw();
            Gl.glEndList();
            return result;
        }

        #endregion
    }

    public struct Mesh : ICloneable, IDrawable
    {
        internal int dlHandle;

        internal Mesh(int handle)
        {
            dlHandle = handle;
        }

        public object Clone()
        {
            return new Mesh(dlHandle);
        }

        #region IDrawable Members

        public void Draw()
        {
            Gl.glCallList(dlHandle);
        }

        #endregion
    }
}
