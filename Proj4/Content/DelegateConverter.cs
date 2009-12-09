using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aura.Content
{
    public static class DelegateConverter
    {
        /// <summary>
        /// Imports a delegate from a string name using reflection.
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        /// <param name="d">The type of delegate that you want to get.</param>
        /// <returns></returns>
        public static Delegate GetDelegate(string methodName, Type d)
        {
            if (methodName == "null") return null;
            Assembly a = Assembly.GetCallingAssembly();
            Type[] types = a.GetTypes();

            foreach (Type t in types)
            {
                MethodInfo[] methods = t.GetMethods();
                foreach (MethodInfo m in methods)
                {
                    object[] atrs = m.GetCustomAttributes(typeof(ContentVisible), false);
                    if (atrs.Length > 0 && m.Name == methodName)
                    {
                        var r = Delegate.CreateDelegate(d, m);
                        return r;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Returns the name of the given delegate
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetName(Delegate d)
        {
            if (d == null) return "null";
            return d.Method.Name;
        }
    }

    /// <summary>
    /// Attribute for methods that you want to expose to the content importer system
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false, Inherited=false)]
    public class ContentVisible : Attribute { }
}
