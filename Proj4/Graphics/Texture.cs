using System;
using System.Collections.Generic;
using Aura.Content;
using Tao.DevIl;
using Tao.OpenGl;

namespace Aura.Graphics
{
    public class Texture : IDisposable, IImportable<Texture>
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

        public void ApplyTexture()
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, glHandle);
        }

        /// <summary>
        /// Release graphics card memory
        /// </summary>
        public void Dispose()
        {
            //Note: Double check with someone on this one
            Gl.glDeleteTextures(1, ref glHandle);
        }

        #region IImportable<Texture> Members

        public IContentImporter<Texture> Importer
        {
            get { return TextureImporter.Instance; }
        }

        #endregion
    }
}
