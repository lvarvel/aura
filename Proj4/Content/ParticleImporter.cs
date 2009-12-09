using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace Aura.Content
{
    public class ParticleImporter : XMLContentImporter<ParticleSystem>
    {
        public ParticleImporter(string directory) : base(directory)
        {
            
        }

        public override ParticleSystem ImportContent(string path)
        {
            var d = (ParticleSystemDescriptor)serializer.Deserialize(new StreamReader(path));
            
            ParticleSystem result = new ParticleSystem();
            result.Build(d);

            return result;
        }

        public override void ExportContent(string path, ParticleSystem content)
        {
            var tmp = content.GetDescriptor;
            serializer.Serialize(new StreamWriter(path), (object)tmp);
        }
    }
}
