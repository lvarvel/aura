using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Represents a 3 dimentional vector
    /// </summary>
    public class Vector3
    {
        float[] data;

        public Vector3()
        {
            data = new float[3];
            data[0] = 0;
            data[1] = 0;
            data[2] = 0;
        }
        public Vector3(float x, float y, float z)
        {
            data = new float[3];
        }
        Vector3(ref Vector3 rhs)
        {
            data = new float[3];
            data[0] = rhs.data[0];
            data[1] = rhs.data[1];
            data[2] = rhs.data[2];
        }

        public static explicit operator float[](Vector3 v)
        {
            return v.data;
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.data[0] == rhs.data[0]) && (lhs.data[1] == rhs.data[1]) && (lhs.data[2] == rhs.data[2]);
        }
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.data[0] != rhs.data[0]) || (lhs.data[1] != rhs.data[1]) || (lhs.data[2] != rhs.data[2]);
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            Vector3 temp = new Vector3(ref lhs);
            temp.data[0] += rhs.data[0];
            temp.data[1] += rhs.data[1];
            temp.data[2] += rhs.data[2];

            return temp;
        }
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            Vector3 temp = new Vector3(ref lhs);
            temp.data[0] += rhs.data[0];
            temp.data[1] += rhs.data[1];
            temp.data[2] += rhs.data[2];

            return temp;
        }

        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            Vector3 temp = new Vector3(ref lhs);
            temp.data[0] *= rhs;
            temp.data[1] *= rhs;
            temp.data[2] *= rhs;
            return temp;
        }
        public float length()
        {
            return (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }
        public Vector3 normalize()
        {
            float len = length();
            data[0] /= len;
            data[1] /= len;
            data[2] /= len;
            return this;
        }
        public float dot(Vector3 rhs)
        {
            return data[0] * rhs.data[0] + data[1] * rhs.data[1] + data[2] * rhs.data[2];
        }
        public Vector3 cross(Vector3 rhs)
        {
            Vector3 result = new Vector3();
            result.X = Y * rhs.Z - Z * rhs.Y;
            result.Y = Z * rhs.X - X * rhs.Z;
            result.Z = X * rhs.Y - Y * rhs.X;
            return result;
        }

        public float X
        {
            get { return data[0]; }
            set { data[0] = value; }
        }
        public float Y
        {
            get { return data[1]; }
            set { data[1] = value; }
        }
        public float Z
        {
            get { return data[2]; }
            set { data[2] = value; }
        }
    }

    /// <summary>
    /// Represents a 2 dimentional vector
    /// </summary>
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 One { get { return new Vector2(1,1); } }
    }
}
