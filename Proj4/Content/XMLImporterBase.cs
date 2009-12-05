using System;
using System.Collections.Generic;
using System.IO;
using Aura.Core;

namespace Aura.Content
{
    /// <summary>
    /// Default implementation for the XMLContentImporter
    /// </summary>
    [UnitTest(10)]
    public class XMLImporterBase<T> : XMLContentImporter<T> where T : IBuildable<T>, new()
    {
        XMLImporterBase(string defaultPath)
            : base(defaultPath)
        {

        }

        public override T ImportContent(string path)
        {
            //Note: Wouldn't work in C# 3.5. dynamic type lets the compiler figure out what was
            //deserialized at runtime, so d doesn't need to be cast into the right descriptor type
            StreamReader r = new StreamReader(path);
            dynamic d = serializer.Deserialize(r);
            r.Close();
            Type t = typeof(T);
            
            T result = new T();
            result.Build(d);
            return result;
        }

        public override void ExportContent(string path, T content)
        {
            var tmp = content.GetDescriptor;
            var w = new StreamWriter(path);
            serializer.Serialize(w, (object)tmp);
            w.Close();
        }

        [UnitTestMethod]
        public static void Test(TestResults results)
        {
            results.ReportMessage("Begining XMLImporterBase test.");
            Tester d = new Tester(results);
            XMLImporterBase<Tester> writer = new XMLImporterBase<Tester>("");

            results.ReportMessage("Begining xml content export test.");
            writer.ExportContent("AXtest.xml", d);
            results.ReportMessage("Content has been exported.");
            var r = writer.ImportContent("AXtest.xml");
            if (r == null || r.val == null || r.val.Length < 1) results.ReportError("Error: Returned test string is empty");
            else results.ReportMessage("Test complete.");
            results.ReportMessage("String contents are: " + r.val);
            File.Delete("AXtest.xml");
        }
    }


    public class Tester : IBuildable<Tester>
    {
        TestResults t;
        public string val = "This is a test. Boop, boop, boooooooooooop!";
        public class TDescriptor : Descriptor<Tester>
        {
            public string val;
            public TDescriptor() { }
            public TDescriptor(string s) { val = s; }
        }

        public Tester() { }
        internal Tester(TestResults r) { t = r; }

        public void Build(Descriptor<Tester> descriptor)
        {
            if (descriptor as TDescriptor == null) t.ReportError("Descriptor incompatable.");
            val = (descriptor as TDescriptor).val;
        }

        public Descriptor<Tester> GetDescriptor
        {
            get { return new TDescriptor(val); }
        }

        public Type DescriptorType
        {
            get { return typeof(TDescriptor); }
        }
    }
}
