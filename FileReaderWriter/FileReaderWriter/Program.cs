using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileReaderWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteFile();
            ReadFile();
        }


        static void WriteFile()
        {
            bool flag = true;


            using (FileStream fs = new FileStream(@"C:\\a.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    while (flag)
                    {

                        string y = Console.ReadLine();
                        if (y.ToLower() == "quit")
                            flag = false;
                        else
                        {
                            sw.WriteLine(y);
                        }

                    }
                }
            }

        }

        static void ReadFile()
        {
            using (FileStream fs = new FileStream(@"C:\\a.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
