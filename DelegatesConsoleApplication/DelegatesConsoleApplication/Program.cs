using System;

delegate void MyMethods(string s);

namespace MicConsoleApplication
{
    class Program
    {
        public static void Hello(string s)
        {
            Console.WriteLine("Hello " + s);
        }

        public static void Goodbye(string s)
        {
            Console.WriteLine("Goodbye " + s);
        }

        public static void Main()
        {
            MyMethods a = new MyMethods(Hello);
            MyMethods b = new MyMethods(Goodbye);

            MyMethods add, sub;

            add = a + b;

            sub = add - b;
            sub("World!");

            Console.Read();
        }
    }
}
