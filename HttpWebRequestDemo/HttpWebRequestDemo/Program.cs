using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpWebRequestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            using(StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                while(!sr.EndOfStream)
                    Console.WriteLine(sr.ReadLine());
            }
            Console.ReadLine();
        }
    }
}
