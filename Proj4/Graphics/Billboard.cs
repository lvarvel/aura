﻿using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Aura.Core;

namespace Aura.Graphics
{
    /// <summary>
    /// Class handles the visualization of billboarded particles.
    /// Can either be used explicitly with Draw(Vector3).  Can
    /// also be passed parameters with Draw(DrawArgs)
    /// </summary>
    public class Billboard : IVisualization
    {
        public Texture Image;
        public Material Color;
        public Vector2 Dimention;
        public BillboardLockType LockType;
        public float scale;

        public Billboard(Texture image, BillboardLockType lockType = BillboardLockType.Spherical, float scalefactor = 1.0f)
        {
            Image = image;
            Color = null;
            Dimention = new Vector2(1,1);
            LockType = lockType;
            scale = scalefactor;
        }
        public Billboard(Texture image, Material color, Vector2 dimention, BillboardLockType lockType = BillboardLockType.Spherical)
        {
            Image = image;
            Color = color;
            Dimention = dimention;
            LockType = lockType;
        }

        public void Draw(DrawArgs args)
        {
            
            Vector3 c_pos = CameraManager.Current.position;
            float c_dist = (float)(Math.Pow(args.Position.X - c_pos.X, 2) + Math.Pow(args.Position.Y - c_pos.Y, 2) + Math.Pow(args.Position.Y - c_pos.Y, 2));
            
            BatchEntry r = new BatchEntry(this, c_dist, (DrawArgs)args.Clone());
            BatchManager.Current.Add(r);
        }
        public void Draw(Vector3 position)
        {
            configBillboard(position);

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

        internal virtual void DepthDraw(DrawArgs args)
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(args.Position.X, args.Position.Y, args.Position.Z);
            configBillboard(args.Position);


            //Draw the billboard with texture coordinates
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, (int)Image);


            if (args.LightingEnabled)
            {
                Gl.glEnable(Gl.GL_LIGHTING);
                if (args._Material == null) throw new ArgumentNullException();
                args._Material.ApplyMaterial();
            }
            else
            {
                if (args.Color == null) throw new ArgumentNullException();
                Gl.glColor4fv((float[])args.Color);
            }

            Gl.glScalef(scale, scale, scale);

            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glTexCoord2f(1, 1); Gl.glVertex3d(args.Scale.X, args.Scale.Y, 0);
            Gl.glTexCoord2f(0, 1); Gl.glVertex3d(-args.Scale.X, args.Scale.Y, 0);
            Gl.glTexCoord2f(0, 0); Gl.glVertex3d(-args.Scale.X, -args.Scale.Y, 0);
            Gl.glTexCoord2f(1, 0); Gl.glVertex3d(args.Scale.X, -args.Scale.Y, 0);
            Gl.glEnd();

            if (LockType == BillboardLockType.Cylindrical || LockType == BillboardLockType.Spherical)
            {
                Gl.glPopMatrix();
            }
            Gl.glPopMatrix();
        }

        protected void configBillboard(Vector3 position)
        {
            //Shamelessly stolen from http://www.lighthouse3d.com/opengl/billboarding/index.php?billSphe
            if (LockType == BillboardLockType.Cylindrical || LockType == BillboardLockType.Spherical)
            {
                float[] modelView = new float[16];
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                //Gl.glLoadIdentity();
                Vector3 camera = CameraManager.Current.position;

                Vector3 difference = new Vector3(camera.X - position.X, 0, camera.Z - position.Z);
                Vector3 lookAt = new Vector3(0,0,1);
                difference.normalize();
                Vector3 up = lookAt.cross(difference);
                float angleCosine = lookAt.dot(difference);
                if ((angleCosine < 0.99990) && (angleCosine > -0.9999))
                    Gl.glRotatef((float)(Math.Acos(angleCosine) * 180/Math.PI), up.X, up.Y, up.Z);

                if (LockType == BillboardLockType.Spherical)
                {
                    Vector3 difference3d = camera - position;
                    difference3d.normalize();
                    angleCosine = difference3d.dot(difference);
                    if ((angleCosine < 0.99990) && (angleCosine > -0.9999))
                    {
                        if (difference3d.Y < 0)
                            Gl.glRotatef((float)(Math.Acos(angleCosine) * 180 / Math.PI), 1, 0, 0);
                        else
                            Gl.glRotatef((float)(Math.Acos(angleCosine) * 180 / Math.PI), -1, 0, 0);
                    }
                }
            }
        }
    }

    public enum BillboardLockType : byte { None, Cylindrical, Spherical }
}
