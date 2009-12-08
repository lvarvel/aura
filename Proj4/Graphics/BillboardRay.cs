using System;
using System.Collections.Generic;
using Aura.Core;
using Tao.OpenGl;

namespace Aura.Graphics
{
    public class ExplosionBillboard : Billboard
    {
        public Vector3 Center;
        
        internal new void Draw(Vector3 Position, Vector2 Scale, Color4 Color)
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(Position.X, Position.Y, Position.Z);
            configBillboard(Position);


            //Draw the billboard with texture coordinates
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)Image);

            Gl.glColor4fv((float[])Color);
            

            Gl.glScalef(.2f,.2f,.2f);

            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glTexCoord2f(1, 1); Gl.glVertex3d(Scale.X, Scale.Y, 0);
            Gl.glTexCoord2f(0, 1); Gl.glVertex3d(-Scale.X, Scale.Y, 0);
            Gl.glTexCoord2f(0, 0); Gl.glVertex3d(-Scale.X, -Scale.Y, 0);
            Gl.glTexCoord2f(1, 0); Gl.glVertex3d(Scale.X, -Scale.Y, 0);
            Gl.glEnd();

            if (LockType == BillboardLockType.Cylindrical || LockType == BillboardLockType.Spherical)
            {
                Gl.glPopMatrix();
            }
            Gl.glPopMatrix();
        }
        }
    }
}
