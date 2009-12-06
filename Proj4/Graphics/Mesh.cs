using System;
using System.Collections.Generic;
using System.IO;
using Tao.OpenGl;
using Aura.Content;
using Aura.Core;

namespace Aura.Graphics
{
    public class ArrayMesh : IDrawable, ICloneable
    {
        public List<MeshPart> MeshParts = new List<MeshPart>();
        public float[] vertexes;
        public float[] vectorNormals;
        public float[] uvCoords;

        public static ArrayMesh Cube
        {
            get
            {
                ArrayMesh result = new ArrayMesh();
                MeshPart quads = new MeshPart(result);

                float[] verts = { 1,1,1 , 1,1,-1 , 1,-1,-1 , 1,-1,1, 
                                 -1,1,1 ,-1,1,-1 ,-1,-1,-1 ,-1,-1,1 };
                float[] normals = { .57735f,.57735f,.57735f , .57735f,.57735f,-.57735f , .57735f,-.57735f,-.57735f , .57735f,-.57735f,.57735f,
                                 -.57735f,.57735f,.57735f ,-.57735f,.57735f,-.57735f ,-.57735f,-.57735f,-.57735f ,-.57735f,-.57735f,.57735f };
                //A cube
                int[] indicies = { 0, 1, 2, 3, 0, 3, 7, 4, 5, 6, 7, 4, 5, 1, 2, 6, 5, 1, 0, 4, 6, 2, 3, 7 };
                //int[] indicies = { 0,1,2, 0,2,3, 0,1,5, 0,5,4, 0,3,7, 0,7,4, 1,2,5, 2,6,5, 2,3,7, 2,7,6, 4,5,6, 4,6,7 };

                //result.vertexes = new Vector3Array(verts);
                //result.vectorNormals = new Vector3Array(normals);
                result.vertexes = (verts);
                result.vectorNormals = (normals);
                result.MeshParts.Add(quads);
                
                quads.indicies = indicies;
                quads.PrimitiveType = PrimitiveType.Quads;
                quads.material = new Material(new Color4(1, 1, 1, 1.0f), new Color4(.5f, .5f, .5f), new Color4(1, 1, 1), .01f);
                

                return result;
            }
        }

        #region IDrawable Members

        public void Draw()
        {
            foreach (MeshPart m in MeshParts)
            {
                m.Draw();

            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            ArrayMesh result = new ArrayMesh();


            result.vertexes = (float[])vertexes.Clone();
            result.vectorNormals = (float[])vectorNormals.Clone();
            result.uvCoords = (float[])uvCoords.Clone();

            foreach (MeshPart m in MeshParts)
            {
                MeshPart r = (MeshPart)m.Clone();
                r.Parent = this;
                result.MeshParts.Add(r);
            }
            return (object)result;
        }

        #endregion
    }

    public class MeshPart : IDrawable, ICloneable
    {
        //public Vector3Array vertexes;
        //public Vector3Array vectorNormals;
        
        public int[] indicies;
        public PrimitiveType PrimitiveType { get; set;}
        public ShadingType ShadingType { get; set; }
        public ArrayMesh Parent { get; set; }

        public Material material = new Material();

        public MeshPart(ArrayMesh parent) 
        {
            Parent = parent;
        }

        

        public void Draw()
        {
            if(LightManager.LightingEnabled)
                material.ApplyMaterial();
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_POLYGON);

            
            if (Parent.vectorNormals != null)
            {
                Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
                Gl.glNormalPointer(Gl.GL_FLOAT, 0, (object)(Parent.vectorNormals));
            }
            if (Parent.uvCoords != null)
            {
                Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
                Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, (object)(Parent.uvCoords));
            }
            else
            {
                Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            }
            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, (object)(Parent.vertexes));
            Gl.glDrawElements((int)PrimitiveType, indicies.Length, Gl.GL_UNSIGNED_INT, (object)indicies);
        }

        public object Clone()
        {
            MeshPart result = new MeshPart(Parent);
            result.indicies = (int[])indicies.Clone();
            result.PrimitiveType = PrimitiveType;
            result.material = material;

            return (object)result;
        }
    }

    /// <summary>
    /// Defines a geometric primitive type
    /// </summary>
    public enum PrimitiveType { Points = Gl.GL_POINTS, Lines = Gl.GL_LINES, LineStrip = Gl.GL_LINE_STRIP, 
        LineLoop = Gl.GL_LINE_LOOP, Triangles = Gl.GL_TRIANGLES, TriangleStrip = Gl.GL_TRIANGLE_STRIP,
        TriangleFan = Gl.GL_TRIANGLE_FAN, Quads = Gl.GL_QUADS, QuadStrip = Gl.GL_QUAD_STRIP, Polygon = Gl.GL_POLYGON}
    public enum ShadingType { Flat = 0, Smooth }


    
}
