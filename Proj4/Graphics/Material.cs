using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Aura.Core;

namespace Aura.Graphics
{
    public class Material
    {
        public Color4 Ambient;
        public Color4 Diffuse;
        public Color4 Specular;
        public float SpecularExponent;

        public Material() { }
        public Material(Color4 ambient, Color4 diffuse, Color4 specular, float specularExponent)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            SpecularExponent = specularExponent;
        }

        public void ApplyMaterial()
        {
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ((float[])Ambient));
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, ((float[])Diffuse));
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, ((float[])Specular));
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, SpecularExponent);
        }
    }
}
