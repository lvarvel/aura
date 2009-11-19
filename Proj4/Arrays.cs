using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Provides a unified interface for storing data in a simpler form
    /// </summary>
    /// <typeparam name="T">Impostor data type</typeparam>
    /// <typeparam name="U">Underlying data type</typeparam>
    public interface IArrayImpostor<T, U> : IEnumerable<U>
    {
        T this[int i] { get; set; }
        U[] GetArray { get; set; }
    }

    /// <summary>
    /// Provides support for Vector3s stored as an array of floats
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vector3Array : IArrayImpostor<Vector3, float>
    {
        protected float[] array;
        public static readonly int ArrayStep = 3;

        protected Vector3Array() { }
        /// <summary>
        /// Copies the values of the given array into the internal array
        /// </summary>
        /// <param name="values"></param>
        public Vector3Array(float[] values)
        {
            array = (float[])values.Clone();
        }
        /// <summary>
        /// Copies the values of the impostor type into the underlying array in order
        /// </summary>
        /// <param name="values"></param>
        public Vector3Array(Vector3[] values)
        {
            array = new float[values.Length * ArrayStep];
            int index = 0;
            foreach (Vector3 v in values)
            {
                array[index++] = v.X;
                array[index++] = v.Y;
                array[index++] = v.Z;
            }
        }

        public Vector3 this[int i]
        {
            get
            {
                Vector3 result = new Vector3();
                int index = i * ArrayStep;
                result.X = array[i++];
                result.Y = array[i++];
                result.Z = array[i];
                return result;
            }
            set
            {
                int index = i * ArrayStep;
                array[i++] = value.X;
                array[i++] = value.Y;
                array[i] = value.Z;
            }
        }

        public float[] GetArray
        {
            get
            {
                return array;
            }
            set
            {
                array = value;
            }
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return (IEnumerator<Vector3>)array.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }
    }

    /// <summary>
    /// Provides support for Color4s stored as an array of floats
    /// </summary>
    public class Color4Array : IArrayImpostor<Color4, float>
    {
        protected float[] array;
        public static readonly int ArrayStep = 4;

        protected Color4Array() { }
        /// <summary>
        /// Copies the values of the array into the internal array
        /// </summary>
        /// <param name="values"></param>
        public Color4Array(float[] values)
        {
            array = (float[])values.Clone();
        }
        /// <summary>
        /// Copies the values of the impostor type into the underlying array in order
        /// </summary>
        /// <param name="values"></param>
        public Color4Array(Color4[] values)
        {
            array = new float[values.Length * ArrayStep];
            int index = 0;
            foreach (Color4 c in values)
            {
                array[index++] = c.R;
                array[index++] = c.G;
                array[index++] = c.B;
                array[index++] = c.A;
            }
        }

        public Color4 this[int i]
        {
            get
            {
                int index = i * ArrayStep;
                Color4 result = new Color4();
                result.R = array[index++];
                result.G = array[index++];
                result.B = array[index++];
                result.A = array[index];
                return result;
            }
            set
            {
                int index = i * ArrayStep;
                array[index++] = value.R;
                array[index++] = value.G;
                array[index++] = value.B;
                array[index++] = value.A;
            }
        }

        public float[] GetArray
        {
            get
            {
                return array;
            }
            set
            {
                array = value;
            }
        }

        public IEnumerator<float> GetEnumerator()
        {
            return (IEnumerator<float>) array.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }
    }
}
