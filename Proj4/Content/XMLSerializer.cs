using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aura.Content
{
    internal abstract class XMLContentImporter<T> : IContentImporter<T>
    {
        protected XmlSerializer serializer;

        public abstract T ImportContent(string path);
        public abstract T ExportContent(string path);
    }
}
