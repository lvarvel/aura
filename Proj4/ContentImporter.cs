using System;
using System.Collections.Generic;
using System.IO;

namespace Aura
{
    /// <summary>
    /// Provides a standard interface for importing assets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentImporter<T>
    {
        T ImportContent(string path);
    }

    /// <summary>
    /// Standard interface for a class that can be loaded
    /// </summary>
    public interface IImportable
    {
        void ReadValues(ref StreamReader reader);
    }
}
