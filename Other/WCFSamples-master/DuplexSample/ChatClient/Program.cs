using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {

            InstanceContext ic = new InstanceContext(new MyCallBack());

            using (ChatServiceClient.ChatServiceClient cl =
                new ChatClient.ChatServiceClient.ChatServiceClient(ic))
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                cl.Connect(username);
                string answer = string.Empty;
                do
                {
                    string x = Console.ReadLine();

                    cl.SendMessage(username, x);

                } while (answer.ToLower() != "quit");

            }
        }
    }

    public class MyCallBack : ChatServiceClient.IChatServiceCallback
    {

        

        public void SendInfo(string info)
        {
            Console.WriteLine(info);
        }

        
    }
}
