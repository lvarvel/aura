using System;
using System.Collections.Generic;
using Aura.Graphics;
using Aura.Core;
using System.IO;

namespace Aura.Content
{
    public class RawObjImporter : IContentImporter<RawMesh>
    {
        public static readonly RawObjImporter Instance = new RawObjImporter();
        protected RawObjImporter() { }

        #region IContentImporter<RawMesh> Members

        public RawMesh ImportContent(string path)
        {
            StreamReader reader = new StreamReader(path);
            RawMesh result = new RawMesh();
            //Import lists. Will be transfered over to the mesh arrays
            //once their sizes are known
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvcoords = new List<Vector2>();

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
                                                float.Parse(parsed[2]),
                                                float.Parse(parsed[3]));
                        verts.Add(v);
                        break;
                    case "vn":
                        //The line defines a vert normal
                        //Convert the split string data into a Vector3
                        //and add it to the running list of normals
                        Vector3 vn = new Vector3(float.Parse(parsed[1]),
                                                float.Parse(parsed[2]),
                                                float.Parse(parsed[3]));
                        normals.Add(vn);
                        break;
                    case "vt":
                        //The line defines texture coordinates
                        //Convert the split string into floats,
                        //and push them onto the list of coordinates
                        uvcoords.Add(new Vector2(float.Parse(parsed[1]), float.Parse(parsed[2])));
                        break;
                    case "f":
                        RawMesh.Polygon poly = new RawMesh.Polygon();
                        for (int itr = 1; itr < parsed.Length; ++itr)
                        {
                            string[] indicies = parsed[itr].Split('/');
                            int vertIndex = int.Parse(indicies[0]);
                            int uvIndex = 0;
                            int normalIndex = 0;

                            if (indicies.Length > 1 && indicies[1].Length > 0)
                                uvIndex = int.Parse(indicies[1]);
                            if (indicies.Length > 2 && indicies[2].Length > 0)
                                normalIndex = int.Parse(indicies[2]);

                            ++poly.size;
                            if (poly.size > 4) throw new Exception("Polygons must be quads or tris.");

                            if (normalIndex != 0)
                            {
                                poly.Normals.Add(normals[normalIndex - 1]);
                            }
                            if (uvIndex != 0)
                            {
                                poly.UVCoords.Add(uvcoords[uvIndex - 1]);
                            }
                            poly.Verts.Add(verts[vertIndex - 1]);

                            
                        }
                        result.Polygons.Add(poly);

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

        #endregion
    }

    /// <summary>
    /// Supports reading in obj files
    /// </summary>
    public class ObjImporter : IContentImporter<Mesh>
    {
        protected ObjImporter() { }
        public static readonly ObjImporter Instance = new ObjImporter();



        public Mesh ImportContent(string path)
        {
            RawMesh raw = RawObjImporter.Instance.ImportContent(path);
            Mesh result = raw.CompileToDL();
            return result;
        }

        
    }
}
