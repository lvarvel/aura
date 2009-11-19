using System;
using System.Collections.Generic;
using System.IO;

namespace Aura
{
    /// <summary>
    /// Provides a standard interface for importing assets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentImporter<T> where T : IImportable
    {
        T ImportContent(string path);
    }

    /// <summary>
    /// Interface standard interface for a class that can be loaded
    /// </summary>
    public interface IImportable
    {
        void ReadValues(ref StreamReader reader);
    }
}
