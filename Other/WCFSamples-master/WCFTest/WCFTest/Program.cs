using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost s = new ServiceHost(typeof(MyService),
                new Uri("http://localhost:8080/MyService"));
            s.AddServiceEndpoint("WCFTest.IMyService",
                new BasicHttpBinding(), "");
            s.Open();
            Console.ReadLine();
            s.Close();
        }
    }


    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string GetString(string s);
        
    }

    public class MyService : IMyService
    {
        #region IMyService Members

        public string GetString(string s)
        {
            return "Hello " + s;
        }

        #endregion
    }

}
