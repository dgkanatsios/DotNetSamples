using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly ab = Assembly.GetAssembly(typeof(System.Windows.Forms.Form));
            foreach (Type t in ab.GetExportedTypes())
            {
                foreach (MethodInfo mi in t.GetMethods())
                {
                    Console.WriteLine(mi.Name);
                }
            }
            Console.WriteLine();
            
            
        }
    }
}
