using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AlbaDL;

namespace AlbaBL
{
    public class OrderEntry
    {
        /// <summary>
        /// Creates the donation.
        /// </summary>
        /// <param name="donation">The donation.</param>
        public void CreateDonation(ShareAThonDonation donation)
        {
            // saving in local database
            using (var context = new ShareAThonDLContainer())
            {
                // adding donatio to context to save into db
                context.AddToShareAThonDonations(donation);

                // save changes into database
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates the order in GP.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="orderProcess">The order process.</param>
        /// <param name="listOrders">The list orders.</param>
        /// <param name="cardDetails"> </param>
        /// <param name="isOrderCreated">if set to <c>true</c> [is order created].</param>
        public void CreateOrderInGP(string fileName, string orderNumber, OrderProcess orderProcess, List<OrderItems> listOrders, CardDetails cardDetails, out bool isOrderCreated)
        {
            // initiating ecoonect model
            var econnectModel = new EConnectModel();
            if (new EConnectModel().SerializeSalesOrderObject(fileName, Utilities._connString, orderProcess, listOrders, cardDetails))
            {
                isOrderCreated = true;
            }
            else
            {
                econnectModel.RollbackSalseDocNumber(Utilities._connString, orderNumber);
                isOrderCreated = false;
            }
        }

        /// <summary>
        /// Gets the ShareAThon based on subscriptionID
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public ShareAThonDonation GetDonation(string subscriptionId)
        {
            using (var context = new ShareAThonDLContainer())
            {
                return (from c in context.ShareAThonDonations
                        where c.AuthorizeNetSubscriptionId.ToString(CultureInfo.InvariantCulture).ToLower().Equals(subscriptionId)
                        select c).FirstOrDefault();
            }
        }
    }
}