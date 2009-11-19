﻿using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura
{
    public class Camera : Object3D
    {
        public void ApplyCameraTransforms()
        {
            //Todo: Camera
            //DO EET!
        }
    }

    public static class CameraManager
    {
        private static Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();
        private static Camera current;
        private static int defaultCount;

        public static Camera Current
        {
            get { return current; }
        }
        public static void SetCamera(string name, Camera camera)
        {
            cameras.Add(name, camera);
        }
    }
}