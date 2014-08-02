using System;
using System.Windows.Forms;

namespace ImagineCupConsoleApplication
{
    class Program
    {
        class Integer
        {
            int integer;

            public int Integer1
            {
                get { return integer; }
                set { integer = value; }
            }

            public Integer(int integer)
            {
                this.integer = integer;
            }

            public Integer(Integer integer)
            {
                this.integer = integer.integer;
            }
        }

        static void Main(string[] args)
        {
            Integer integer = new Integer(21);
            Method(integer);
            Console.WriteLine("From Integer instance in Method: " + integer.Integer1);

            Console.ReadLine();
        }

        static void Method(Integer integer)
        {
            Integer temp = new Integer(integer);
            temp.Integer1++;
            Console.WriteLine("From Integer instance in Method: " + temp.Integer1);
        }
    }

}
