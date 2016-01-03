using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    // NOTE: If you change the interface name "IChatService" here, you must also update the reference to "IChatService" in App.config.
    [ServiceContract(CallbackContract=typeof(ICallback))]
    public interface IChatService
    {
        [OperationContract]
        void Connect(string username);

        [OperationContract]
        void SendMessage(string username, string message);

        
    }

    public interface ICallback
    {
        [OperationContract]
        void SendInfo(string info);
    }
}
