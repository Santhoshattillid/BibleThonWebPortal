using System;
using System.Collections.Generic;
using System.Linq;
using AlbaBL;
using AlbaDL;

namespace ShareAThon
{
    public partial class ShareAThonSubscriptionProcess : System.Web.UI.Page
    {
        /// <summary>
        /// Event for Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // verfying the success transaction
            if (Request.Form["x_response_code"] != null && Request.Form["x_response_code"] == "1")
            {
                ShareAThonDonation donation = null;

                // initiating orderEntry
                var entry = new OrderEntry();

                // verifying subscription id for recurring payment transaction
                if (Request.Form["x_subscription_id"] != null)
                {
                    // get donation from database based on subscription id
                    donation = entry.GetDonation(Request.Form["x_subscription_id"]);
                }

                // checked whether donation record founds
                if (donation != null)
                {
                    // Generating order number
                    var gpEConnect = new GPEconnect();

                    // generating next sales order
                    var orderNo = gpEConnect.GetSalesOrderNumber();

                    // calculating sum for offerlines
                    var sum = donation.ShareAThonOfferLines.AsEnumerable().Select(o => o.Qty * gpEConnect.GetUnitPrice(o.OfferNo)).Sum();

                    // getting customer details in order process
                    OrderProcess orderProcess = gpEConnect.GetOrderProcess(donation.CustomerId, orderNo, sum);

                    // get list of orders
                    List<OrderItems> listOrders = gpEConnect.GetOrderedItems(donation, orderNo);

                    // filename to update order into GP
                    string fileName = Server.MapPath("~/SalesOrder.xml");

                    bool isOrderCreated;

                    // processing order into GP
                    entry.CreateOrderInGP(fileName, orderNo, orderProcess, listOrders, new CardDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty), out isOrderCreated);
                }
            }
        }
    }
}