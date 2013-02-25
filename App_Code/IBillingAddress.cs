using System.Collections.Generic;

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