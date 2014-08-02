using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            UseThreadPool();
            UseNonPoolThreads();
            UseDelegates();
            Console.ReadLine();
        }

        private delegate void MyEventHandler();
        private delegate void MyEventHandlerParams(object obj);
        private static void UseDelegates()
        {
            Console.WriteLine("Inside UseDelegates. Thread ID = {0}", Thread.CurrentThread.ManagedThreadId);
            new MyEventHandler(DoWork).BeginInvoke(null, null);
            new MyEventHandlerParams(DoWorkParams).BeginInvoke(8,null,null);
        }

        private static void UseNonPoolThreads()
        {
            Console.WriteLine("Inside UseNonPoolThreads. Thread ID = {0}", Thread.CurrentThread.ManagedThreadId);
            Thread t = new Thread(new ThreadStart(DoWork));
            t.Start();
            Thread t2 = new Thread(new ParameterizedThreadStart(DoWorkParams));
            t2.Start(6);
        }

        private static void UseThreadPool()
        {
            Console.WriteLine("Inside UseThreadPool. Thread ID = {0}", Thread.CurrentThread.ManagedThreadId);
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWorkParams));
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWorkParams), 5);
        }

        public static void DoWork()
        {
            Console.WriteLine("Inside DoWork. Thread ID = {0}", Thread.CurrentThread.ManagedThreadId);
        }

        public static void DoWorkParams(object obj)
        {
            Console.WriteLine("Inside DoWorkParams(obj). Thread ID = {0}", Thread.CurrentThread.ManagedThreadId);
            if (obj != null) Console.WriteLine("Inside DoWorkParams(obj). Parameter = {0}", obj.ToString());
        }
    }
}
