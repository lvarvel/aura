using System;
using System.Collections.Generic;

namespace Aura.Core
{
    /// <summary>
    /// Represents a 3 dimentional vector
    /// </summary>
    [UnitTest(1)]
    public class Vector3 : IPoolable<float>
    {
        private float[] data;

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
            data[0] = x;
            data[1] = y;
            data[2] = z;
        }
        public Vector3(ref Vector3 rhs)
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
        public void AssignValues(float x, float y, float z)
        {
            data[0] = x;
            data[1] = y;
            data[2] = z;
        }

        [UnitTestMethod]
        public static void TestVector(TestResults results)
        {
            results.ReportMessage("Begining Test 1: Vector3 addition test");
            Vector3 v1 = new Vector3(0,0,0);
            Vector3 v2 = new Vector3(1, 1, 1);
            Vector3 v3 = v1 + v2;

            if (v3.X != 1 || v3.Y != 1 || v3.Z != 1)
            {
                results.ReportError("Test 1 failed, V1+v2 != 1");
            }
            else
            {
                results.ReportMessage("Test 1 success.");
            }

            results.ReportMessage("Begining Test 2: Vector cross product");
            v1 = new Vector3(1, 0, 0);
            v2 = new Vector3(0, 1, 0);
            v3 = v1.cross(v2);

            if (v3.cross(v1) != v2)
            {
                results.ReportError("Test 2 failed, cross product equivelency rule violated");
            }
            else
            {
                results.ReportMessage("Test 2 success.");
            }
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

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }

        #region IPoolable Members
        public void Build(params float[] parameters)
        {
            data[0] = parameters[0];
            data[1] = parameters[1];
            data[2] = parameters[2];
        }

        public void Clean()
        {
            data[0] = 0;
            data[1] = 0;
            data[2] = 0;
        }

        public void Build(params object[] parameters)
        {
            data[0] = (float)parameters[0];
            data[1] = (float)parameters[1];
            data[2] = (float)parameters[2];
        }
        #endregion
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


    /// <summary>
    /// Simple static pool to avoid rapid alocation/realocation of vector3s
    /// </summary>
    public class Vector3Pool : Pool<Vector3>
    {
        public static readonly Vector3Pool Instance = new Vector3Pool();
        private Vector3Pool() : base(10) { }
    }
}
