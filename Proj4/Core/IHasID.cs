using System;

namespace Aura.Core
{
    interface IHasID
    {
        int ID { get; }
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
