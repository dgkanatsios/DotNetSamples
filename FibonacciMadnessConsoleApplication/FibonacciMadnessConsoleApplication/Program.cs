using System;

namespace FibonacciMadnessConsoleApplication
{
    class Program
    {
        static void Main()
        {
            Func<int, int> fibonaci = null;
            fibonaci = x => x <= 1 ? 1 : fibonaci(x - 1) + fibonaci(x - 2);

            Console.WriteLine(Fibonacci(3));

            Console.ReadLine();
        }

        static int Fibonacci(int x)
        {
            return x <= 1 ? x : Fibonacci(x - 1) + Fibonacci(x - 2);
        }
    }
}
