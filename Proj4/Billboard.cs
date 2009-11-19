using System;
using System.Collections.Generic;
using Aura.Images;
using Tao.OpenGl;

namespace Aura
{
    public struct Billboard : IVisualization
    {
        public Texture Image;
        public Material Color;
        public Vector2 Dimention;
        public BillboardLockType LockType;

        public Billboard(Texture image, Material color, Vector2 dimention, BillboardLockType lockType = BillboardLockType.Spherical)
        {
            Image = image;
            Color = color;
            Dimention = dimention;
            LockType = lockType;
        }

        public void Draw(Vector3 position)
        {
            //Shamelessly stolen from http://www.lighthouse3d.com/opengl/billboarding/index.php?billSphe
            if (LockType == BillboardLockType.Cylindrical)
            {
                float[] modelView = new float[16];
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                Vector3 camera = CameraManager.Current.position;

                Vector3 difference = new Vector3(camera.X - position.X, 0, camera.Z - position.Z);
                Vector3 lookAt = new Vector3(0,0,1);
                difference.normalize();
                Vector3 up = lookAt.cross(difference);
                float angleCosine = lookAt.dot(difference);
                if ((angleCosine < 0.99990) && (angleCosine > -0.9999))
                    Gl.glRotatef((float)Math.Acos(angleCosine), up.X, up.Y, up.Z);

                if (LockType == BillboardLockType.Spherical)
                {
                    Vector3 difference3d = camera - position;
                    difference3d.normalize();
                    angleCosine = difference3d.dot(difference);
                    if ((angleCosine < 0.99990) && (angleCosine > -0.9999))
                    {
                        if (difference3d.Y < 0)
                            Gl.glRotatef((float)Math.Acos(angleCosine), 1, 0, 0);
                        else
                            Gl.glRotatef((float)Math.Acos(angleCosine), -1, 0, 0);
                    }
                }
            }

            //Draw the billboard with texture coordinates
            Gl.glBindTexture(Gl.GL_TEXTURE_2D ,(int)Image);
            Color.ApplyMaterial();
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glTexCoord2f(1, 1); Gl.glVertex3d(Dimention.X, Dimention.Y, 0);
            Gl.glTexCoord2f(0, 1); Gl.glVertex3d(-Dimention.X, Dimention.Y, 0);
            Gl.glTexCoord2f(0, 0); Gl.glVertex3d(-Dimention.X, -Dimention.Y, 0);
            Gl.glTexCoord2f(1, 0); Gl.glVertex3d(Dimention.X, -Dimention.Y, 0);
            Gl.glEnd();

            if (LockType == BillboardLockType.Cylindrical || LockType == BillboardLockType.Spherical)
            {
                Gl.glPopMatrix();
            }
        }
    }

    public enum BillboardLockType : byte { None, Cylindrical, Spherical }
}
