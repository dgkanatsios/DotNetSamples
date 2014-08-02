using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Processes
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (Process p in Process.GetProcesses())
            {
                Console.WriteLine(p.ProcessName);
            }
            Console.ReadLine();
        }
    }
}
