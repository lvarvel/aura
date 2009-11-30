using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

namespace Aura
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
        public static TestResults doUnitTest(String path)
        {
            Assembly a = Assembly.GetCallingAssembly();
            Type[] types = a.GetTypes();
            SortedList<int, Type> unitTestTypes = new SortedList<int, Type>();
            TestResults results = new TestResults(path);

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
                testClass(t, results);
            }

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
                    UnitTestHandler test = Delegate.CreateDelegate(typeof(UnitTestHandler), method) as UnitTestHandler;

                    if (test != null) test(results);
                    results.ReportWarning("Method " + method.Name +
                        " was marked to unit test but could not be converted into a UnitTestHandler delegate type.");
                }
            }
            catch (Exception e)
            {
                throw new UnitTestException("Error occured in method " + info.Name + ", in class " + t.Name + ".",
                    results, e);
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
