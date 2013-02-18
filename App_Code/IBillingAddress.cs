using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblethon.Controller
{
    public interface IBillingAddress
    {
        List<BillingAddress> GetCustomerDetails(string connString);

        BillingAddress GetBillingAddress();

        BillingAddress GetBillingAddressByName(string name, string phoneNumber);

        BillingAddress GetBillingAddressByAddress(string name, string addressFirstLine);
    }
}