using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Aura.Graphics;

namespace Aura.Core
{
    public class Model : Object3D, IDrawable
    {
        Mesh Mesh { get; set; }

        public Model() : base() { }
        public Model(Mesh mesh)
            : base()
        {
            Mesh = mesh;
        }

        public void Draw()
        {
            Gl.glPushMatrix();
            Vector3 axis;
            float angle;
            rotation.getAxisAngle(out axis, out angle);
            if (angle == float.NaN)
                throw new Exception();

            Gl.glTranslatef(position.X, position.Y, position.Z);
            Gl.glRotatef(angle * (float)(180/Math.PI), axis.X, axis.Y, axis.Z);
            Gl.glScalef(scale, scale, scale);

            Mesh.Draw();

            Gl.glPopMatrix();
        }
    }
}
