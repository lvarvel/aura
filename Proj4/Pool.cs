using System;
using System.Collections.Generic;

namespace Aura
{
    /// <summary>
    /// Provides a generic class that pools memory resources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : class,IPoolable,new()
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
        public T New(params object[] parameters)
        {
            T t = Get();
            t.Build(parameters);
            return t;
        }
        public T New<B>(params B[] parameters)
        {
            IPoolable<B> t = (IPoolable<B>)Get();
            t.Build(parameters);
            return (T)t;
        }
        public void Return(T item)
        {
            pool.Push(item);
            item.Clean();
        }
    }

    /// <summary>
    /// An item that can be pooled
    /// </summary>
    public interface IPoolable
    {
        void Clean();
        void Build(params object[] parameters);
    }
    /// <summary>
    /// A convenient extension to IPoolable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoolable<T> : IPoolable
    {
        void Build(params T[] parameters);
    }
}
