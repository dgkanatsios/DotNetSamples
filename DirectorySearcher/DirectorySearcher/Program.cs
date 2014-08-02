using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Principal;
using System.Security.AccessControl;

namespace DirectorySearcher
{
    class Program
    {
        static string pattern;
        static void Main(string[] args)
        {
            pattern = Console.ReadLine();
            DirectoryInfo di = new DirectoryInfo(@"C:\\");
            Search(di);

        }

        private static void Search(DirectoryInfo di)
        {

            
            foreach (FileInfo f in di.GetFiles())
            {
                if (f.Name.Contains(pattern))
                    Console.WriteLine(f.FullName);
            }
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                Search(d);
            }
        }
    }
}
