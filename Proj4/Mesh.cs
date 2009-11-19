using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura
{
    public class Mesh : IDrawable
    {
        public float[] verts;

        Vector3Array vertexes;
        Vector3Array vectorNormals;
        uint[] indicies;
        PrimitiveType primitiveType;

        Material material;

        public void Draw()
        {
            material.ApplyMaterial();

            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, (object)(vertexes.GetArray));
            if (vectorNormals != null)
                Gl.glNormalPointer(Gl.GL_FLOAT, 0, (object)(vectorNormals.GetArray));
            Gl.glDrawElements((int)primitiveType, indicies.Length, Gl.GL_UNSIGNED_INT, (object)indicies);
        }
    }

    /// <summary>
    /// Defines a geometric primitive type
    /// </summary>
    public enum PrimitiveType { Points = Gl.GL_POINTS, Lines = Gl.GL_LINES, LineStrip = Gl.GL_LINE_STRIP, 
        LineLoop = Gl.GL_LINE_LOOP, Triangles = Gl.GL_TRIANGLES, TriangleStrip = Gl.GL_TRIANGLE_STRIP,
        TriangleFan = Gl.GL_TRIANGLE_FAN, Quads = Gl.GL_QUADS, QuadStrip = Gl.GL_QUAD_STRIP, Polygon = Gl.GL_POLYGON}
}
