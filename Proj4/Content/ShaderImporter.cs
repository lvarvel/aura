using System;
using System.Collections.Generic;
using Aura.Graphics;
using Tao.Cg;
using Tao.OpenGl;
using System.IO;

namespace Aura.Content
{
    public class ShaderImporter : IContentImporter<Shader>
    {
        #region Singleton
        public static readonly ShaderImporter Instance = new ShaderImporter();
        private ShaderImporter() { }
        #endregion

        #region IContentImporter<Shader> Members

        public Shader ImportContent(string path)
        {
            string prog;
            StreamReader reader = new StreamReader(path);
            prog = reader.ReadToEnd();

            Gl.glEnable(Gl.GL_FRAGMENT_PROGRAM_ARB);
            int handle;
            Gl.glGenProgramsARB(1, out handle);
            Gl.glBindProgramARB(Gl.GL_FRAGMENT_PROGRAM_ARB, handle);

            Gl.glProgramStringARB(Gl.GL_FRAGMENT_PROGRAM_ARB, Gl.GL_PROGRAM_FORMAT_ASCII_ARB,
                prog.Length, (object)prog);
            Gl.glDisable(Gl.GL_FRAGMENT_PROGRAM_ARB);
            Shader result = new Shader(handle);

            return null;
        }

        #endregion
    }
}
