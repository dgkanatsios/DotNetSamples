using System;
using System.Collections.Generic;
using System.IO;

namespace GenericsAndIOConsoleApplication
{
    class Program
    {
        static void Main()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            StreamReader streamReader = new StreamReader("input.txt");

            while (streamReader.EndOfStream == false)
            {
                string line = streamReader.ReadLine();
                int index = line.IndexOf(' ');
                dictionary.Add(line.Substring(0, index), Int32.Parse(line.Substring(++index)));
            }

            foreach (KeyValuePair<string, int> key in dictionary)
            {
                Console.WriteLine("{0}: {1}", key.Key, key.Value);
            }

            Console.ReadLine();
        }
    }
}
