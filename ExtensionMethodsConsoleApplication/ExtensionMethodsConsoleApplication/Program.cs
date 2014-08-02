using System;

namespace ImagineCupConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            int integer = 21;
            Console.WriteLine(integer.Negate());
            Console.ReadLine();
        }
    }

    static class Extension
    {
        public static int Negate(this int integer)
        {
            return -integer;
        }
    }

}
