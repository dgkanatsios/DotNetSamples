using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace ObjectSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            SerializePerson();
            DeserializePerson();
            Console.ReadLine();
        }

        private static void DeserializePerson()
        {
            using (FileStream fs = new FileStream(@"C:\a.txt", FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Person));
                Person p = ser.Deserialize(fs) as Person;
                if (p == null) throw new ApplicationException();

                Console.WriteLine("{0} {1} {2}", p.Name, p.Salary, p.Age);
            }
        }

        private static void SerializePerson()
        {
            Person p = new Person() { Age = 50, Name = "Dimitris", Salary = 1500 };

            using (FileStream fs = new FileStream(@"C:\a.txt", FileMode.Create))
            {

                XmlSerializer xml = new XmlSerializer(typeof(Person));
                xml.Serialize(fs, p);
            }
        }
    }

    [Serializable()]
    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }

    }
}
