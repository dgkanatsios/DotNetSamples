using System;

namespace ImagineCupConsoleApplication
{
    class Program
    {
        static void Main(string[] arguments)
        {
            foreach (string argument in arguments)
            {
                Console.WriteLine(argument + " RULES :D");
            }

            Console.ReadLine();
        }
    }
}
