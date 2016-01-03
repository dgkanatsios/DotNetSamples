using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfService1
{
    // NOTE: If you change the class name "NorthwindService" here, you must also update the reference to "NorthwindService" in Web.config.
    public class NorthwindService : INorthwindService
    {

        #region INorthwindService Members

        public List<WSCustomer> GetAllCustomers()
        {
            using (NorthwindDataContext daa = new NorthwindDataContext())
            {

                Thread.Sleep(4000);
                var c = from x in daa.Customers
                        select new WSCustomer
                        {
                            CustomerID =
                                x.CustomerID,
                            ContactName = x.ContactName,
                            CompanyName = x.CompanyName
                        };
                return c.ToList();
            }
        }

        #endregion
    }
}
