using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RefConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 2, b = 20;
            Console.WriteLine("a before Swap: " + a + " b before Swap: " + b);
            Swap(ref a, ref b);
            Console.WriteLine("a after Swap: " + a.ToString() + " b after Swap: " + b.ToString());
            Console.ReadLine();
        }

        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
