using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Timers
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                Console.WriteLine("xaxa");
            };
            t.Start();
            Console.ReadLine();
        }
    }
}
