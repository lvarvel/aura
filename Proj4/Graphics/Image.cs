using System;
using System.Collections.Generic;
using Aura.Content;
using Tao.DevIl;
using Tao.OpenGl;

namespace Aura.Graphics
{
    public class Texture : IDisposable
    {
        internal int glHandle;

        /// <summary>
        /// Creates an empty image
        /// </summary>
        internal Texture() { }

        public static explicit operator int (Texture rhs)
        {
            return rhs.glHandle;
        }

        /// <summary>
        /// Release graphics card memory
        /// </summary>
        public void Dispose()
        {
            //Note: Double check with someone on this one
            Gl.glDeleteTextures(1, ref glHandle);
        }
    }

    public class ImageImporter : IContentImporter<Texture>
    {
        private Dictionary<string, Texture> buffer = new Dictionary<string, Texture>();

        private ImageImporter() { }

        /// <summary>
        /// Processes an image at specified path directly into openGL
        /// If you need a new instance of the same image object, use GetImage
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Image object instance</returns>
        public Texture ImportContent(string path)
        {
            Texture result = new Texture();
            //Suddenly, DevIl makes importing images directly into openGL so easy its silly
            
            result.glHandle = Ilut.ilutGLLoadImage(path);
            if(!buffer.ContainsKey(path))
                buffer.Add(path, result);

            return result;
        }

        /// <summary>
        /// Returns the first image instance created from a specified path name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Texture GetImage(string name)
        {
            return buffer[name];
        }

        public static readonly ImageImporter Instance = new ImageImporter();
    }
}
