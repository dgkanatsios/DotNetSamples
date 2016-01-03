using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CSharpLanguageEnhancements
{
    class Program
    {
        static void Main(string[] args)
        {

            DemoObjectInitializer();
            DemoCollectionInitializer();
            DemoExtensionMethods();
            DemoLINQ();
            SimpleLambdas();
            ExpressionTrees();
            Console.ReadLine();
        }

        private static void ExpressionTrees()
        {
            //lambdas are stored as data, instead of delegates

            ParameterExpression left = Expression.Parameter(typeof(int), "x");
            ParameterExpression right = Expression.Parameter(typeof(int), "y");
            BinaryExpression bin = Expression.Add(left, right);
            Expression<Func<int, int, int>> expr =
                Expression.Lambda<Func<int, int, int>>
                (bin, new ParameterExpression[] { left, right });

            Func<int, int, int> f = expr.Compile();
            int result = f(4, 5);
            Console.WriteLine(result);

        }

        private static void SimpleLambdas()
        {

            Func<int> f = () => new Random().Next(5, 10); ;
            Console.WriteLine(f()); //displays random from 5 to 10


            Func<int, int, int> f2 = (a, b) => a + b;
            Console.WriteLine(f2(4, 5));//displays 9    


            //well, it seems recursive, but it isn't
            //for more info, check here: http://blogs.msdn.com/wesdyer/archive/2007/02/02/anonymous-recursion-in-c.aspx
            Func<int, int> fibonacci = null;
            fibonacci = x => x > 1 ? fibonacci(x - 1) + fibonacci(x - 2) : x;

            int g = 6;
            int y = fibonacci(g);
            Console.WriteLine("the {0}th fibonacci term is {1}", g, y);


        }

        private static void DemoLINQ()
        {
            //let's get all the available encodings using Array extension methods...
            //don't scare because of the lambda expression, it just projects the DisplayName property
            IEnumerable<string> mycol
                = System.Text.Encoding.GetEncodings().Select(x => x.DisplayName);
            foreach (string s in mycol)
                Console.WriteLine(s);


            //nah, I want some lanquage integrated query
            //is it SQL? nah...
            //is it syntactic sugar?? eh, nooooooo :P
            //it's about the same as above
            //get all the available encodings...
            IEnumerable<string> l =
                from e in System.Text.Encoding.GetEncodings()
                select e.DisplayName;

            foreach (string s in l)
                Console.WriteLine(s);


            //let's filter that a bit...
            IEnumerable<string> l2 =
                from e in System.Text.Encoding.GetEncodings()
                where e.DisplayName.Contains('e')//just a stupid filter
                select e.DisplayName + " " + e.CodePage.ToString();

            foreach (string s in l2)
                Console.WriteLine(s);


            //want some type inference??
            //NO, it's not late binding.....
            var l3 =
                from e in System.Environment.GetLogicalDrives()
                select e;

            //l3's type is INFERRED from the query results

            foreach (var s in l3)
                Console.WriteLine(s);


            //so, you want a 'dynamic' object?? glad you asked:P
            var l4 = from e in System.TimeZoneInfo.GetSystemTimeZones()
                     select new { e.DisplayName, e.StandardName };
            foreach (var s in l4)
            {
                //i have compile time support in s! Great!!!
                Console.WriteLine(s.DisplayName + " " + s.StandardName);
            }


        }

        private static void DemoExtensionMethods()
        {
            ObjectInitialiserClass c =
               new ObjectInitialiserClass()
               {
                   MyDateTime = DateTime.Now,
                   MyInt = 6,
                   MyString = "hello"
               };

            //extension method call
            string returnMessage = c.ObjectInitialiserClassToString();
        }

        private static void DemoCollectionInitializer()
        {
            //you do this in C# prior to 3.0
            ObjectInitialiserClass c = new ObjectInitialiserClass();
            c.MyDateTime = DateTime.Now;
            c.MyInt = 5;
            c.MyString = "hello di";


            ObjectInitialiserClass c2 = new ObjectInitialiserClass();
            c2.MyDateTime = DateTime.Now;
            c2.MyInt = 6;
            c2.MyString = "hello di2";


            ObjectInitialiserClass c3 = new ObjectInitialiserClass();
            c3.MyDateTime = DateTime.Now;
            c3.MyInt = 7;
            c3.MyString = "hello di3";


            List<ObjectInitialiserClass> l = new List<ObjectInitialiserClass>();
            l.Add(c);
            l.Add(c2);
            l.Add(c3);


            //in C# 3.0, you can do this
            List<ObjectInitialiserClass> l2 =
                new List<ObjectInitialiserClass>() { c, c2, c3 };

        }

        private static void DemoObjectInitializer()
        {
            //you do this in C# prior to 3.0
            ObjectInitialiserClass c = new ObjectInitialiserClass();
            c.MyDateTime = DateTime.Now;
            c.MyInt = 5;
            c.MyString = "hello di";

            //in C# 3.0, you can do this
            ObjectInitialiserClass c2 =
                new ObjectInitialiserClass()
                {
                    MyDateTime = DateTime.Now,
                    MyInt = 6,
                    MyString = "hello"
                };

        }
    }


    class AytomaticPropertiesClass
    {
        //shorter syntax... like it??
        public int MyInteger { get; set; }
    }

    //a class for our demo...
    class ObjectInitialiserClass
    {
        private int _MyInt;

        public int MyInt
        {
            get { return _MyInt; }
            set { _MyInt = value; }
        }
        private string _MyString;

        public string MyString
        {
            get { return _MyString; }
            set { _MyString = value; }
        }
        private DateTime _MyDateTime;

        public DateTime MyDateTime
        {
            get { return _MyDateTime; }
            set { _MyDateTime = value; }
        }

    }

    //this method will seem like it belongs to the 
    //ObjectInitialiserClass 
    static class MyExtensionMethods
    {
        public static string ObjectInitialiserClassToString
            (this ObjectInitialiserClass obj)
        {
            return
                obj.MyString + " "
                + obj.MyInt.ToString() + " "
                + obj.MyDateTime.ToLongDateString();
        }

    }
}
