using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ChatService
{
    // NOTE: If you change the class name "ChatService" here, you must also update the reference to "ChatService" in App.config.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {

        private Random myRandom;
        private Dictionary<string, ICallback> connected;

        public void SendInfoToConnectedUsers(object obj)
        {
            while (true)
            {
                Thread.Sleep(myRandom.Next(3000, 6000));

                int total = 0;
                lock (connected)
                {
                    total = connected.Count;
                }

                string s = String.Format("Total users: {0} DateTime = {1}",
                    total, DateTime.Now.ToString());
                Array.ForEach(connected.Values.ToArray(),
               x => x.SendInfo(s));
            }
        }

        public void Connect(string username)
        {
            connected.Add(username,
                OperationContext.Current.GetCallbackChannel<ICallback>());
        }


        public ChatService()
        {
            connected = new Dictionary<string, ICallback>();
            myRandom = new Random(DateTime.Now.Millisecond);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendInfoToConnectedUsers));
        }

        public void SendMessage(string username, string message)
        {
            Array.ForEach(connected.Values.ToArray(),
                x => x.SendInfo(string.Format("{0} says {1}", username, message)));
        }


    }
}
