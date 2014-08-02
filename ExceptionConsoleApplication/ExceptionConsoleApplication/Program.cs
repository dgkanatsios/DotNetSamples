using System;
using System.Threading;

namespace ExceptionConsoleApplication
{
    class Program
    {
        public static void Main()
        {
            ThreadClass instance = new ThreadClass();
            Thread thread = new Thread(new ParameterizedThreadStart(instance.ThreadMethod));

            thread.Start();

            while (thread.IsAlive == false) ;

            Thread.Sleep(100);

            thread.Abort();
            thread.Join();

            Console.ReadLine();
        }

    }

    class ThreadClass
    {
        public void ThreadMethod(object o)
        {
            while (true)
            {
                Console.WriteLine("From thread method xD");
            }
        }
    }
}
