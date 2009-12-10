using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura
{
    public class PointVisualization : IVisualization
    {
        public void Draw(DrawArgs args)
        {
            if (args.LightingEnabled)
            {
                Gl.glEnable(Gl.GL_LIGHTING);
                args._Material.ApplyMaterial();
            }
            else
            {
                Gl.glDisable(Gl.GL_LIGHTING);
                Gl.glColor3fv((float[])args.Color);
            }
            Gl.glPointSize(args.Scale.X);
            Gl.glBegin(Gl.GL_POINTS);
            Gl.glVertex3fv((float[])args.Position);
            Gl.glEnd();
        }
    }
}
