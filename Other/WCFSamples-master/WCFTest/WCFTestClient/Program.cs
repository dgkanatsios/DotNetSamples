using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IMyService> cf =
                new ChannelFactory<IMyService>(new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:8080/MyService"));

            IMyService s = cf.CreateChannel();

            string x = s.GetString("Dimitris");
            Console.WriteLine(x);
            Console.ReadLine();
            
        }
    }


    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string GetString(string s);

    }
}
