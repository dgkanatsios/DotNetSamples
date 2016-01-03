using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService1
{
    // NOTE: If you change the interface name "INorthwindService" here, you must also update the reference to "INorthwindService" in Web.config.
    [ServiceContract]
    public interface INorthwindService
    {
        [OperationContract]
        List<WSCustomer> GetAllCustomers();
    }

    [DataContract]
    public class WSCustomer
    {
        [DataMember]
        public string CustomerID { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string ContactName { get; set; }
    }
}
