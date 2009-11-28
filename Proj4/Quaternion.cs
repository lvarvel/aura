using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// WARNING: Quaternion might not work quite right yet, but it should
    /// </summary>
    public class Quaternion
    {
        //W, X, Y, Z
        float[] data;

        public Quaternion()
        {
            data = new float[4];
            data[0] = 0;
            data[1] = 1;
            data[2] = 0;
            data[3] = 0;
        }
        public Quaternion(float theta, float x, float y, float z)
        {
            data = new float[4];
            float sinAngle;
            theta = theta * 0.5f;
            Vector3 vn = new Vector3(x, y, z);
            vn.normalize();

            sinAngle = (float)Math.Sin(theta);

            data[1] = (vn.X * sinAngle);
            data[2] = (vn.Y * sinAngle);
            data[3] = (vn.Z * sinAngle);
            data[0] = (float)Math.Cos(theta);
        }
        public Quaternion(float w, Vector3 i)
        {
            data[0] = w;
            data[1] = i.X;
            data[2] = i.Y;
            data[3] = i.Z;
        }

        Quaternion(ref Quaternion copy)
        {
            data = new float[4];
            data[0] = copy.data[0];
            data[1] = copy.data[1];
            data[2] = copy.data[2];
            data[3] = copy.data[3];
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
                float w = lhs.data[0] * rhs.data[0] - lhs.data[1] * rhs.data[1] - lhs.data[2] * rhs.data[2] - lhs.data[3] * rhs.data[3];
                float x = lhs.data[0] * rhs.data[1] + lhs.data[1] * rhs.data[0] + lhs.data[2] * rhs.data[3] - lhs.data[3] * rhs.data[2];
                float y = lhs.data[0] * rhs.data[2] + lhs.data[2] * rhs.data[0] + lhs.data[3] * rhs.data[1] - lhs.data[1] * rhs.data[3];
                float z = lhs.data[0] * rhs.data[3] + lhs.data[3] * rhs.data[0] + lhs.data[1] * rhs.data[2] - lhs.data[2] * rhs.data[1];
                return new Quaternion(w, new Vector3(x, y, z));
        }

        public void normalise()
        {
            float mag2 = data[0] * data[0] + data[1] * data[1] + data[2] * data[2] + data[3] * data[3];
            if (mag2 != 0.0f && ((float)Math.Abs(mag2 - 1.0f) > 0.000001f))
            {
                float mag = (float)Math.Sqrt(mag2);
                data[0] /= mag;
                data[1] /= mag;
                data[2] /= mag;
                data[3] /= mag;
            }
        }
        public void getAxisAngle(out Vector3 axis, out float angle)
        {
            float scale = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            axis = new Vector3();
            axis.X = X / scale;
            axis.Y = Y / scale;
            axis.Z = Z / scale;
            angle = (float)Math.Acos(W) * 2.0f;
        }
        Quaternion conjugate()
        {
            return new Quaternion(W, new Vector3(-X, -Y, -Z));
        }
	    
	    Vector3 transformVector(Vector3 vec)
	    {
		    Vector3 vn = new Vector3(ref vec);
		    vn.normalize();

            Quaternion vecQuat = new Quaternion();
            Quaternion resQuat = new Quaternion();
		    vecQuat.X = vn.X;
		    vecQuat.Y = vn.Y;
		    vecQuat.Z = vn.Z;
		    vecQuat.W = 0.0f;
 
		    resQuat = vecQuat * conjugate();
		    resQuat = this * resQuat;
 
		    return new Vector3(resQuat.X, resQuat.Y, resQuat.Z);
	    }

        public float W
        {
            get { return data[0]; }
            set { data[0] = value; }
        }
        public float X
        {
            get { return data[1]; }
            set { data[1] = value; }
        }
        public float Y
        {
            get { return data[2]; }
            set { data[2] = value; }
        }
        public float Z
        {
            get { return data[3]; }
            set { data[3] = value; }
        }
    }
}
