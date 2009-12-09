using System;
using System.Collections.Generic;

namespace Aura.Content
{
    /// <summary>
    /// The base class for a class descriptor
    /// </summary>
    /// <typeparam name="T">The type of class described</typeparam>
    public abstract class Descriptor <T> { }

    /// <summary>
    /// Provides a unified interface for creating instances from descriptors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDescriptorFactory<T>
    {
        T Build(Descriptor<T> descriptor);
    }

    /// <summary>
    /// An abstract class that most descriptor factories should inherit.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseFactory<T> : IDescriptorFactory<T> where T : IBuildable<T>, new()
    {
        public abstract T Build(Descriptor<T> descriptor);
    }

    /// <summary>
    /// Interface that defines a class that can be built by a BaseFactory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBuildable<T>
    {
        void Build(Descriptor<T> descriptor);
        Descriptor<T> GetDescriptor { get; }
        Type DescriptorType { get; }
    }

    
}
