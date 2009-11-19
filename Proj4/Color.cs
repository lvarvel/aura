using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura
{
    public class Color4 : ICloneable
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
            //Average the alpha.  
            result.A = (lhs.A + rhs.A) * 0.5f;
            result.R += rhs.R;
            result.G += rhs.G;
            result.B += rhs.B;
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
    }
}
