using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringBuilderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            string s = string.Empty;
            for (int i = 0; i < 5000; i++)
                s += Guid.NewGuid().ToString();
            DateTime dt2 = DateTime.Now;
            TimeSpan t = dt2 - dt;
            Console.WriteLine(t.Milliseconds);

            dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5000; i++)
                sb.Append(Guid.NewGuid().ToString());
            dt2 = DateTime.Now;
            t = dt2 - dt;
            Console.WriteLine(t.Milliseconds);
            Console.ReadLine();

        }
    }
}
