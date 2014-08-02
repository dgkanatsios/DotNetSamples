using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD5Hashing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MD5Transform("Hello world"));
            Console.ReadLine();
        }

        public static string MD5Transform(string input)
        {
            if (input == null) return string.Empty;
            if (input.Length == 0) return String.Empty;
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();

        }
    }
}
