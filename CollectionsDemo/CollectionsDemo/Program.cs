using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionaries();
            //Lists();
            //Arrays();
            Ranges();
            Console.ReadLine();
        }

        private static void Ranges()
        {
            int[] x = Enumerable.Range(500, 2000).ToArray<int>();
            foreach (int a in x)
                Console.WriteLine(a);
        }

        private static void Arrays()
        {
            int[] x = new int[200];
            Random r = new Random();
            for (int i = 0; i < 200; i++)
            {
                x[i] = r.Next();
            }
            Array.Sort(x);
            foreach (int j in x)
                Console.WriteLine(j);
        }

        private static void Lists()
        {
            List<DateTime> l = new List<DateTime>();
            for (DateTime dt = DateTime.Now; dt < new DateTime(2009, 4, 5); dt = dt.AddDays(1))
                l.Add(dt);
            foreach (DateTime d in l)
                Console.WriteLine(d);
        }

        private static void Dictionaries()
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            Random r = new Random(System.DateTime.Now.Millisecond);
            for (int i = 0; i < 100; i++)
            {
                d.Add(i, r.Next(1000, 2000));
            }
            foreach (KeyValuePair<int, int> kvp in d)
            {
                Console.WriteLine("key = {0}, value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
