using System;
using System.Collections.Generic;

namespace Aura
{
    public class Color4 : ICloneable, IPoolable<float>
    {
        public static readonly float MAX_VALUE = 255;

        float[] values;

        public Color4() 
        {
            values = new float[4];
        }
        public Color4(float r, float g, float b)
        {
            values = new float[4];

            R = r;
            G = g;
            B = b;
            A = MAX_VALUE;
        }
        public Color4(float r, float g, float b, float a)
        {
            values = new float[4];

            R = r;
            G = g;
            B = b;
            A = a;
        }
        public Color4(Color4 copy)
        {
            values = new float[4];

            R = copy.R;
            G = copy.G;
            B = copy.B;
            A = copy.A;
        }

        public static explicit operator float[](Color4 rhs)
        {
            return rhs.values;
        }

        public static Color4 White { get { return new Color4(MAX_VALUE, MAX_VALUE, MAX_VALUE, MAX_VALUE); } }

        /// <summary>
        /// Multiply the RGB values of the color by the right hand side.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Color4 operator* (Color4 lhs, float rhs)
        {
            Color4 result = new Color4(lhs);
            result.R *= rhs;
            result.G *= rhs;
            result.B *= rhs;
            return result;
        }
        public static Color4 operator +(Color4 lhs, Color4 rhs)
        {
            Color4 result = new Color4(lhs);  
            result.A += rhs.A;
            result.R += rhs.R;
            result.G += rhs.G;
            result.B += rhs.B;
            return result;
        }
        public static Color4 operator -(Color4 lhs, Color4 rhs)
        {
            Color4 result = new Color4(lhs);
            result.A -= rhs.A;
            result.R -= rhs.R;
            result.G -= rhs.G;
            result.B -= rhs.B;
            return result;
        }


        public float R
        {
            get { return values[0]; }
            set { values[0] = value; }
        }
        public float G
        {
            get { return values[1]; }
            set { values[1] = value; }
        }
        public float B
        {
            get { return values[2]; }
            set { values[2] = value; }
        }
        public float A
        {
            get { return values[3]; }
            set { values[3] = value; }
        }

        public object Clone()
        {
            return (object)(new Color4(this));
        }

        #region IPoolable Members
        public void Build(params float[] parameters)
        {
            values[0] = parameters[0];
            values[1] = parameters[1];
            values[2] = parameters[2];
            values[3] = parameters[3];
        }

        public void Clean()
        {
            values[0] = 0;
            values[1] = 0;
            values[2] = 0;
            values[3] = 0;
        }

        public void Build(params object[] parameters)
        {
            values[0] = (float)parameters[0];
            values[1] = (float)parameters[1];
            values[2] = (float)parameters[2];
            values[3] = (float)parameters[3];
        }
        #endregion
    }

    /// <summary>
    /// Simple static pool to avoid realocation of Color4s
    /// </summary>
    public class Color4Pool : Pool<Color4>
    {
        public static readonly Color4Pool Instance = new Color4Pool();
        private Color4Pool() : base(10) { }
    }
}
