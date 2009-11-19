using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura
{
    public struct Material
    {
        Color4 Ambient;
        Color4 Diffuse;
        Color4 Specular;
        float SpecularExponent;

        public void ApplyMaterial()
        {
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ((float[])Ambient));
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, ((float[])Diffuse));
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, ((float[])Specular));
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, SpecularExponent);
        }
    }
}
