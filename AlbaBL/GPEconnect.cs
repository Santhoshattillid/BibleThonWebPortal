using System.Collections.Generic;
using System.Linq;
using AlbaDL;

namespace AlbaBL
{
    public class GPEconnect
    {
        /// <summary>
        /// Gets the sales order number.
        /// </summary>
        /// <returns></returns>
        public string GetSalesOrderNumber()
        {
            return new EConnectModel().GetNextSalseDocNumber(Utilities._connString).Trim();
        }

        /// <summary>
        /// Gets the offer lines.
        /// </summary>
        /// <returns></returns>
        public List<OfferLines> GetOfferLines()
        {
            return new OfferLines().GetOfferLines(Utilities._connString);
        }

        /// <summary>
        /// Gets the customer details.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="telephone">The telephone.</param>
        /// <returns></returns>
        public IEnumerable<BillingAddress> GetCustomerDetails(string name, string telephone)
        {
            return new BillingAddress().GetCustomerDetails(Utilities._connString, name, telephone);
        }

        /// <summary>
        /// Gets the customer billing addresses.
        /// </summary>
        /// <returns></returns>
        public List<BillingAddress> GetCustomerBillingAddresses()
        {
            return new BillingAddress().GetCustomerBillingAddresses(Utilities._connString);
        }

        /// <summary>
        /// Gets the customet ship address.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShippingAddress> GetCustomerShipAddress(string customerNo)
        {
            return new ShippingAddress().GetCustomerShipAddress(Utilities._connString, customerNo);
        }

        /// <summary>
        /// Gets the unit price.
        /// </summary>
        /// <param name="offerNo">The offer no.</param>
        /// <returns></returns>
        public decimal GetUnitPrice(string offerNo)
        {
            decimal unitPrice = 0;
            var sourceOfferLines = new OfferLines().GetOfferLines(Utilities._connString);

            foreach (var sourceOfferLine in sourceOfferLines.Where(sourceOfferLine => sourceOfferLine.OfferId.Equals(offerNo)))
            {
                unitPrice = sourceOfferLine.Price;
            }

            return unitPrice;
        }

        /// <summary>
        /// Getting customer details into order process
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="orderNumber"></param>
        /// <param name="subTotal"></param>
        /// <returns></returns>
        public OrderProcess GetOrderProcess(string customerNo, string orderNumber, decimal subTotal)
        {
            return new EConnectModel().GetOrderProcess(Utilities._connString, customerNo, orderNumber, subTotal);
        }

        /// <summary>
        /// Getting list of order items.
        /// </summary>
        /// <param name="donation"></param>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public List<OrderItems> GetOrderedItems(ShareAThonDonation donation, string orderNumber)
        {
            return new EConnectModel().GetOrderedItems(Utilities._connString, donation, orderNumber);
        }
    }
}