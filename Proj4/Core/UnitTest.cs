using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

namespace Aura.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class UnitTest : Attribute
    {
        protected int testNumber;

        public int TestOrder 
        { 
            get { return testNumber; } 
        }

        public UnitTest(int testNmbr)
        {
            testNumber = testNmbr;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class UnitTestMethod : Attribute { }

    public static class UnitTester
    {
        private static TestResults current;
        private static string last_string;

        public static TestResults doUnitTest(String path)
        {
            Assembly a = Assembly.GetCallingAssembly();
            Type[] types = a.GetTypes();
            SortedList<int, Type> unitTestTypes = new SortedList<int, Type>();
            TestResults results;
            if (current != null && last_string == path)
            {
                results = current;
            }
            else
                results = new TestResults(path);

            foreach (Type t in types)
            {
                object[] atrs = t.GetCustomAttributes(typeof(UnitTest), false);
                foreach (UnitTest atr in atrs)
                {
                    unitTestTypes.Add(atr.TestOrder, t);
                }
            }
            foreach (Type t in unitTestTypes.Values)
            {
                results.ReportMessage("");
                testClass(t, results);
            }

            /*
            if (!results.success)
            {
                results.WriteToFile();
                throw new UnitTestException("Tests failed!", results, null);
            }
            */

            current = results;
            last_string = path;

            return results;
        }
        private static void testClass(Type t, TestResults results)
        {
            MethodInfo info = null;
            try
            {
                foreach (MethodInfo method in t.GetMethods())
                {
                    info = method;
                    object[] check = method.GetCustomAttributes(typeof(UnitTestMethod), false);
                    

                    if (check.Length > 0)
                    {
                        UnitTestHandler test = Delegate.CreateDelegate(typeof(UnitTestHandler), method) as UnitTestHandler;
                        test(results);
                    }
                }
            }
            catch (Exception e)
            {
                 results.ReportError("Exception occured in method " + info.Name + ", in class " + t.Name + "." + "\n" + e.Message);
            }
        }
    }

    public delegate void UnitTestHandler(TestResults results);

    public class TestResults
    {
        public bool success { get; protected set; }
        public int numberErrors { get; protected set; }
        public int numberWarnings { get; protected set; }
        public StringBuilder ResultBuffer
        {
            get { return resultBuffer; }
        }
        public StreamWriter LogFile
        {
            get { return logFile; }
        }

        private StringBuilder resultBuffer;
        private StreamWriter logFile;

        public TestResults() 
        {
            success = true;
            numberErrors = 0;
            numberWarnings = 0;
            resultBuffer = new StringBuilder();
            logFile = null;
        }
        public TestResults(string path)
        {
            success = true;
            numberErrors = 0;
            numberWarnings = 0;
            resultBuffer = new StringBuilder();
            logFile = new StreamWriter(path);
        }

        public void ReportWarning(string warning)
        {
            ++numberWarnings;
            resultBuffer.AppendLine(warning); 
        }
        public void ReportError(string error)
        {
            success = false;
            ++numberErrors;
            resultBuffer.AppendLine(error);
        }
        public void ReportMessage(string message)
        {
            resultBuffer.AppendLine(message);
        }

        public void WriteToFile()
        {
            logFile.Write("Test ");
            if (success)
            {
                logFile.WriteLine("success!");
            }
            else
            {
                logFile.WriteLine("failure!!!");
            }
            logFile.WriteLine("There are " + numberErrors + " errors.");
            logFile.WriteLine("There are " + numberWarnings + " warnings.");
            logFile.WriteLine();

            logFile.Write(resultBuffer);
            logFile.Flush();
        }
    }

    public class UnitTestException : Exception
    {
        public TestResults testResults;

        internal UnitTestException() : base() { }
        internal UnitTestException(string message, TestResults results, Exception innerException)
            : base(message, innerException)
        {
            testResults = results;
        }
    }
}
