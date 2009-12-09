using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aura.Content
{
    public abstract class XMLContentImporter<T> : IContentImporter<T> where T : IBuildable<T>, new()
    {
        protected XmlSerializer serializer;
        public string WorkingDirectory { get; set; }

        protected XMLContentImporter(string directory)
        {
            WorkingDirectory = directory;
            T t = new T();
            serializer = new System.Xml.Serialization.XmlSerializer(t.DescriptorType);
        }

        public abstract T ImportContent(string path);
        public abstract void ExportContent(string path, T content);
    }
}
