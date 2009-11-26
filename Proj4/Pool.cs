using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Provides a generic class that pools memory resources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : class,new()
    {
        protected Stack<T> pool = new Stack<T>();

        public Pool() { }
        public Pool(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                pool.Push(new T());
            }
        }

        public T Get()
        {
            if (pool.Count < 1)
            {
                return new T();
            }
            else
            {
                return pool.Pop();
            }
        }
        public void Return(T item)
        {
            pool.Push(item);
        }
    }
}
