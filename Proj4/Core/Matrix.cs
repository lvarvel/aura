using System;

namespace Aura.Core
{
    public class Matrix
    {
        public Matrix(Vector3 position, Quaternion rotation, float scale)
        {
            throw new NotImplementedException();
        }

        public Transform getTransform()
        {
            throw new NotImplementedException();
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            throw new NotImplementedException();
        }
    }

    public class Transform : IDisposable
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float Scale;

        public Transform(Vector3 position, Vector3 rotation, float scale = 1)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Position = null;
            Rotation = null;
        }

        #endregion
    }
}
