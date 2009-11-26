using System;
using System.Collections.Generic;

namespace Aura
{
    public interface IDrawable
    {
        void Draw();
    }

    public interface IVisualization
    {
        void Draw(DrawArgs args);
    }

    /// <summary>
    /// Contains data about drawing a visualization
    /// </summary>
    public class DrawArgs : IDisposable
    {
        public Vector3 Position;
        public Vector3 Vector;
        public Vector2 Scale;
        public float Rotation;

        public bool LightingEnabled;
        public Color4 Color;
        public Material _Material;

        public DrawArgs(Vector3 position, Color4 color, float rotation = 0)
        {
            Position = position;
            Color = color;
            LightingEnabled = false;
            Scale = new Vector2(1, 1);
            Rotation = rotation;

            _Material = null;
            Vector = null;
        }
        public DrawArgs(Vector3 position, Color4 color, Vector2 scale, float rotation = 0)
        {
            Position = position;
            Color = color;
            LightingEnabled = false;
            Scale = scale;
            Rotation = rotation;

            _Material = null;
            Vector = null;
        }
        public DrawArgs(Vector3 position, Vector3 vector, Material material, Vector2 scale, float rotation = 0)
        {
            Position = position;
            LightingEnabled = true;
            Scale = scale;
            Vector = vector;
            _Material = material;
            Color = null;
            Rotation = rotation;
        }

        public void Dispose()
        {
            Position = null;
            Color = null;
            Vector = null;
            _Material = null;
        }
    }
}
