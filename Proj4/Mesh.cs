using System;
using System.Collections.Generic;
using System.IO;
using Tao.OpenGl;

namespace Aura
{
    public class Mesh : IDrawable, ICloneable
    {
        public Vector3Array vertexes;
        public Vector3Array vectorNormals;
        public float[] uvCoords;
        public uint[] indicies;
        PrimitiveType primitiveType;

        Material material;

        public Mesh() { }

        public void Draw()
        {
            material.ApplyMaterial();

            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, (object)(vertexes.GetArray));
            if (vectorNormals != null)
                Gl.glNormalPointer(Gl.GL_FLOAT, 0, (object)(vectorNormals.GetArray));
            if (uvCoords != null)
                Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, (object)(uvCoords));
            Gl.glDrawElements((int)primitiveType, indicies.Length, Gl.GL_UNSIGNED_INT, (object)indicies);
        }

        public object Clone()
        {
            Mesh result = new Mesh();
            result.vertexes = new Vector3Array(vertexes.GetArray);
            result.vectorNormals = new Vector3Array(vectorNormals.GetArray);
            result.uvCoords = (float[])uvCoords.Clone();
            result.indicies = (uint[])indicies.Clone();
            result.primitiveType = primitiveType;
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

    /// <summary>
    /// Supports reading in obj files
    /// </summary>
    public class ObjImporter : IContentImporter<Mesh>
    {
        protected ObjImporter() { }
        public static readonly ObjImporter Instance = new ObjImporter();

        public Mesh ImportContent(string path)
        {
            StreamReader reader = new StreamReader(path);
            Mesh result = new Mesh();
            //Import lists. Will be transfered over to the mesh arrays
            //once their sizes are known
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<float> uvcoords = new List<float>();
            List<uint> indicies = new List<uint>();

            int lineR = 0;
            while (!reader.EndOfStream)
            {
                ++lineR;
                string read = reader.ReadLine();
                string[] parsed = read.Split(' ');
                switch (parsed[0])
                {
                    case "v":
                        //The line defines a vertex
                        //Convert the split string data into a Vector3
                        //and add it to the running list of verts
                        Vector3 v = new Vector3(float.Parse(parsed[1]),
                                                float.Parse(parsed[1]),
                                                float.Parse(parsed[2]));
                        verts.Add(v);
                        break;
                    case "vn":
                        //The line defines a vert normal
                        //Convert the split string data into a Vector3
                        //and add it to the running list of normals
                        Vector3 vn = new Vector3(float.Parse(parsed[1]),
                                                float.Parse(parsed[1]),
                                                float.Parse(parsed[2]));
                        normals.Add(vn);
                        break;
                    case "vt":
                        //The line defines texture coordinates
                        //Convert the split string into floats,
                        //and push them onto the list of coordinates
                        uvcoords.Add(float.Parse(parsed[1]));
                        uvcoords.Add(float.Parse(parsed[2]));
                        break;
                    case "f":
                        foreach (string s in parsed)
                        {
                            if (s == "f") continue;
                            string[] face = s.Split('/');
                            uint? vert;
                            uint? tex = null;
                            uint? norm = null;
                            if (face[0].Length == 0) 
                                throw new InvalidOperationException("Error, line " + lineR.ToString() + ": Face vert must have a vertex index");
                            vert = uint.Parse(face[0]);
                            if(face[1].Length != 0)
                                tex = uint.Parse(face[1]);
                            if(face[2].Length != 0)
                                norm = uint.Parse(face[2]);
                            if (!(tex == norm && vert == tex))
                                throw new InvalidOperationException("Error, line " + lineR.ToString() + ": Aura does not support faces with inconsistant index numbers");

                            //OBJ coordinates are 1 based, C# is 0 based
                            indicies.Add(vert.Value-1);
                        }
                        break;
                    case "#":
                        //It's a comment
                        break;
                    default:
                        //There may be groups or material presets that this importer doesn't support
                        break;
                }
            }

            return result;
        }
    }
}
