using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WindowsFormsWP7
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMissileService" in both code and config file together.
    [ServiceContract]
    public interface IMissileService
    {
        [OperationContract]
        void Left();
        [OperationContract]
        void Right();
        [OperationContract]
        void Up();
        [OperationContract]
        void Down();
        [OperationContract]
        void Fire();
        [OperationContract]
        void Stop();
    }
}
