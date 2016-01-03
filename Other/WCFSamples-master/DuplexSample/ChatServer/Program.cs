using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatService;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatService.ChatService cs = new ChatService.ChatService();

            ServiceHost s = new ServiceHost(cs,
                new Uri("http://localhost:8080/ChatServer"));
            s.AddServiceEndpoint("ChatService.IChatService",
                new  WSDualHttpBinding(), "");

            ServiceMetadataBehavior beh = new ServiceMetadataBehavior();
            beh.HttpGetEnabled = true;
            s.Description.Behaviors.Add(beh);

            s.AddServiceEndpoint("IMetadataExchange",
                MetadataExchangeBindings.CreateMexHttpBinding(),
                "mex");
            
            s.Open();

            Console.WriteLine("Chat Server is open...");
            Console.ReadLine();
            
        }
    }
}
