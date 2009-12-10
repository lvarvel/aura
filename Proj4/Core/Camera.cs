using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura.Core
{
    public class Camera : Object3D
    {
        public Vector3 chasePoint;

        public Camera() : base() { }
        public Camera(Vector3 Position, Vector3 LookAt)
        {
            chasePoint = LookAt;
            position = Position;
        }

        public void ApplyCameraTransforms()
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(35.0f, 4 / 3, 1, 10000);
            //Gl.glOrtho(-30, 30, -20, 20, -100, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluLookAt(position.X, position.Y, position.Z, chasePoint.X, chasePoint.Y, chasePoint.Z, 0, 1, 0);
            //Glu.gluLookAt(0,0,5, 0,0,0 , 0, 1, 0);
        }
    }

    public static class CameraManager
    {
        private static Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();
        private static Camera current;

        public static Camera Current
        {
            get { return current; }
        }
        public static void SetCamera(string name, Camera camera)
        {
            cameras.Add(name, camera);
            current = camera;
        }
    }
}
