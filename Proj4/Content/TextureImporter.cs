using System;
using System.Collections.Generic;
using Aura.Graphics;
using Tao.DevIl;

namespace Aura.Content
{
    public class TextureImporter : IContentImporter<Texture>
    {
        private Dictionary<string, Texture> buffer = new Dictionary<string, Texture>();

        public TextureImporter() { }

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
            if (!buffer.ContainsKey(path))
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

        public static readonly TextureImporter Instance = new TextureImporter();
    }
}
