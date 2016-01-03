using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToTwitter;
using System.Data;
using System.Reflection;
using BLinq;
using System.Configuration;
using System.Xml.Linq;
using System.IO;

namespace LINQ2Anything
{


#error Update app.config with BING API Key, northwind connection string and twitter user/pass for the samples to work
    class Program
    {
        static void Main(string[] args)
        {
            #region LINQ2Objects

            Console.WriteLine("Using Linq to Objects");

            List<Person> persons = GetListOfPersons();
            ObjectDumper.Write(persons);
            Console.ReadLine();

            //Get List of persons in Athens//
            Console.WriteLine("Get List of persons in Athens");
            var personsInAthens = from x in persons
                                  where x.City == "Athens"
                                  select x;
            ObjectDumper.Write(personsInAthens);
            Console.ReadLine();
            //----------------------------------//


            //Get persons names + age older than 30 years old//
            var personsOlderThan30 = from x in persons
                                     where DateTime.Now.Year - x.DOB.Year > 30
                                     select new { Name = x.LastName, Age = DateTime.Now.Year - x.DOB.Year };
            ObjectDumper.Write(personsOlderThan30);
            Console.ReadLine();
            //----------------------------------//


            //Get persons that firstname begins with 'j', sorted by DOB desc//
            var personsFirstnameJ = from x in persons
                                    where x.FirstName.StartsWith("J")
                                    orderby x.DOB descending
                                    select x;
            ObjectDumper.Write(personsFirstnameJ);
            Console.ReadLine();
            //----------------------------------//


            //Get Persons that Lastname contains 'o' Grouped by city//
            var personsGroupedCity = from x in persons
                                     where x.LastName.Contains('o')
                                     group x by x.City into g
                                     select g;
            ObjectDumper.Write(personsGroupedCity);
            Console.ReadLine();
            //----------------------------------//

            #endregion

            #region LINQ2SQL

            Console.WriteLine("Using Linq to SQL");

            using (NorthwindDataContext ctx = new NorthwindDataContext())
            {
                Console.WriteLine("Customers in US or in UK");
                var customersinUsorUK = from x in ctx.Customers
                                        where x.Country == "US"
                                        || x.Country == "UK"
                                        select x;
                //the query is NOT executed right now...


                //but NOW (deferred execution)
                ObjectDumper.Write(customersinUsorUK);

                Console.WriteLine("Customers with more than 3 orders");
                var customersWithMoreThan3Orders = from x in ctx.Customers
                                                   where x.Orders.Count() > 3
                                                   select x;
                ObjectDumper.Write(customersWithMoreThan3Orders);

                Console.WriteLine("Customer names with orders in 2007");
                var customerNamesWithOrdersin2007 = from x in ctx.Customers
                                                    join y in ctx.Orders
                                                    on x.CustomerID equals y.CustomerID
                                                    where y.OrderDate.Value.Year == 2007
                                                    select x.ContactName;
                ObjectDumper.Write(customerNamesWithOrdersin2007);


                Console.WriteLine("To add a new category");
                Category cat = new Category() { CategoryName = "Meat" };
                ctx.Categories.InsertOnSubmit(cat);

                //persist it on database
                ctx.SubmitChanges();

                //let's find now the category with the biggest CategoryID
                //and delete it
                Category categoryWithBiggestID = (from x in ctx.Categories
                                                  orderby x.CategoryID descending
                                                  select x).First();

                ctx.Categories.DeleteOnSubmit(categoryWithBiggestID);
                ctx.SubmitChanges();

            }
            #endregion

            #region LINQ2XML

            Console.WriteLine("Using Linq to XML");

            XDocument xdocument = XDocument.Parse(File.ReadAllText("Books.xml"));

            var booksPriceLessThan30 = from x in xdocument.Element("catalog").Elements()
                                       where float.Parse(x.Element("price").Value) < 30.0
                                       select x;
            ObjectDumper.Write(booksPriceLessThan30);

            var booksGroupedByAuthor = from x in xdocument.Element("catalog").Elements()
                                       group x by x.Element("author").Value into g
                                       select g;
            ObjectDumper.Write(booksGroupedByAuthor);
            #endregion

            #region LINQ2Reflection

            Console.WriteLine("Using Linq to Reflection");

            var virtualMethodsInDataSet =
                from x in typeof(string).GetMethods(BindingFlags.Public
                    | BindingFlags.Instance)
                where x.IsVirtual
                select x;
            ObjectDumper.Write(virtualMethodsInDataSet);
            #endregion

            #region LINQ2Twitter

            Console.WriteLine("Using Linq to Twitter");

            TwitterContext twitterContext = new TwitterContext
                (ConfigurationManager.AppSettings["TwitterUsername"],
                ConfigurationManager.AppSettings["TwitterPassword"]);


            var trends = from x in twitterContext.Trends
                         select new { x.Name };
            ObjectDumper.Write(twitterContext);

            var dgkanatsiosTimelineLatestTweets = from x in twitterContext.User
                                                  where x.UserID == "dgkanatsios"
                                                  select new { x.Name, x.Status.Text };
            ObjectDumper.Write(dgkanatsiosTimelineLatestTweets);


            #endregion

            #region LINQ2Bing

            Console.WriteLine("Using Linq to Bing");

            BingContext bing = new BingContext
                (ConfigurationManager.AppSettings["BingKey"]);

            IQueryable<PageSearchResult> pages1 = from p in bing.Pages
                                                  where p.Query == "hdms 2009"
                                                  select p;
            pages1 = pages1.Take(2);
            WriteBingPages("hdms 2009", pages1);

            var q2 = from p in bing.Pages.SafeResults().LocalResults("Ελλάδα")
                     where p.Query == "βάσεις δεδομένων"
                     select p;
            WriteBingPages("Βάσεις δεδομένων", q2);

            #endregion

            Console.ReadLine();
        }


        #region Helper Methods
        private static List<Person> GetListOfPersons()
        {
            Person p = new Person() { DOB = new DateTime(1985, 4, 5), FirstName = "John", LastName = "Doe", City = "Athens" };
            Person p2 = new Person() { DOB = new DateTime(1960, 6, 30), FirstName = "Patrick", LastName = "Stone", City = "London" };
            Person p3 = new Person() { DOB = new DateTime(1987, 12, 22), FirstName = "Victoria", LastName = "Douglas", City = "Athens" };
            Person p4 = new Person() { DOB = new DateTime(1976, 11, 30), FirstName = "Jack", LastName = "Mars", City = "Rome" };
            Person p5 = new Person() { DOB = new DateTime(1965, 4, 21), FirstName = "James", LastName = "Bridger", City = "Rome" };
            Person p6 = new Person() { DOB = new DateTime(1999, 10, 15), FirstName = "Nick", LastName = "Cage", City = "Athens" };
            Person p7 = new Person() { DOB = new DateTime(1975, 4, 9), FirstName = "Caroline", LastName = "Daniels", City = "London" };
            Person p8 = new Person() { DOB = new DateTime(1963, 1, 12), FirstName = "Andreas", LastName = "Douncan", City = "Barcelona" };
            Person p9 = new Person() { DOB = new DateTime(1991, 10, 17), FirstName = "Johnny", LastName = "Hunnighton", City = "Barcelona" };
            Person p10 = new Person() { DOB = new DateTime(1990, 3, 19), FirstName = "Ted", LastName = "Molta", City = "Athens" };

            return new List<Person> { p, p2, p3, p4, p5, p6, p7, p8, p9, p10 };

        }

        private static void WriteBingPages(string query, IEnumerable<PageSearchResult> pages)
        {
            Console.WriteLine("Page Query: " + query);
            Console.WriteLine(new String('=', 80));
            foreach (PageSearchResult page in pages)
            {
                Console.WriteLine("Title:   " + page.Title);
                Console.WriteLine("Uri:     " + page.Uri.AbsoluteUri);
                Console.WriteLine("Display: " + page.DisplayUrl);
                Console.WriteLine("Description");
                Console.WriteLine(page.Description);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        #endregion
    }
}
