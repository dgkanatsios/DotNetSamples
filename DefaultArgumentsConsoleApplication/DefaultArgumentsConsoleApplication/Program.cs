using System;
using System.Threading.Tasks;
using System.Threading;

namespace DefaultArgumentsConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] ints = new int[100];

            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = i;
            }
            Console.WriteLine("MainThreadID={0}", Thread.CurrentThread.ManagedThreadId);
            Parallel.ForEach(ints, integer => Process(integer));
            Console.ReadLine();
        }

        static void Process(int integer)
        {
            Console.WriteLine("ProcessThreadID={0}",  Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(integer);
        }
    }
}
