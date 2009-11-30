using System;
using System.Collections.Generic;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Aura
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Engine e = new Engine())
            {
                e.Run();
            }
        }
    }
}
