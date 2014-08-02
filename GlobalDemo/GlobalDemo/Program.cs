using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GlobalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Cultures();
            Currencies();
            Console.ReadLine();
        }

        private static void Currencies()
        {
            CultureInfo c = new CultureInfo("en-US");
            Console.WriteLine(string.Format(c, "{0:c}", 50));
            CultureInfo c2 = new CultureInfo("el-GR");
            Console.WriteLine(string.Format(c2, "{0:c}", 50));
            CultureInfo c3 = new CultureInfo("fr-FR");
            Console.WriteLine(string.Format(c3, "{0:c}", 50));
        }

        private static void Cultures()
        {
            CultureInfo c = new CultureInfo("en-US");
            Console.WriteLine(string.Format(c, "{0}", DateTime.Now));
            CultureInfo c2 = new CultureInfo("el-GR");
            Console.WriteLine(string.Format(c2, "{0}", DateTime.Now));
            CultureInfo c3 = new CultureInfo("fr-FR");
            Console.WriteLine(string.Format(c3, "{0}", DateTime.Now));
        }
    }
}
