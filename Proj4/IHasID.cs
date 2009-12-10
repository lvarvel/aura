using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura
{
    interface IHasID
    {
        int GetID { get; }
    }
    public static class IDManager
    {
        public static int BadID { get { return 0; } }
        private static int IDCounter = 1;
        public static int NewID
        {
            get { return IDCounter++; }
        }
    
    }
}
