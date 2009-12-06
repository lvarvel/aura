using System;
using System.Collections.Generic;
using Tao.OpenGl;

namespace Aura
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //TestResults results = UnitTester.doUnitTest("testLog.txt");
            //results.WriteToFile();

            using (Engine e = new Engine())
            {
                e.Run();
            }
        }
    }
}
