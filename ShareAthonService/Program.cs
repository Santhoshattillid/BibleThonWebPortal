using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using AlbaBL;
using AlbaDL;

namespace ShareAThonService
{
    internal static class Program
    {
        private static void Main()
        {
            // Get The current data tO update GP
            DataTable currentData = GetcurrentData();

            // verify current data table is empty or not
            if (currentData != null)
            {
                // Creating new order
                foreach (DataRow row in currentData.Rows)
                {
                    // create new order number
                    var orderNumber = new EConnectModel().GetNextSalseDocNumber(Utilities._connString).Trim();

                    // getting customer info
                    string cutomerInfo = new EConnectModel().GetCustomerDetails(Utilities._connString, GetCustomerId(int.Parse(row["ShareAThonDonationId"].ToString())));
                    var customerDoc = new XmlDocument();
                    if (cutomerInfo != null) customerDoc.LoadXml(cutomerInfo);
                    var xmlList = customerDoc.SelectNodes("root/eConnect/Customer");

                    var sum = GetOfferedLines(int.Parse(row["ShareAThonDonationId"].ToString())).AsEnumerable().Select(o => int.Parse(o["Qty"].ToString()) * GetUnitPrice(o["OfferNo"].ToString())).Sum();

                    var orderProcess = GetCustomerHeader(xmlList, int.Parse(row["ShareAThonDonationId"].ToString()), orderNumber, sum);

                    var listOrders = GetOrderedItems(xmlList, int.Parse(row["ShareAThonDonationId"].ToString()), orderNumber);

                    var fileName = System.IO.Path.Combine(Environment.CurrentDirectory, "SalesOrder.xml");

                    if (new EConnectModel().SerializeSalesOrderObject(fileName, Utilities._connString, orderProcess, listOrders, new CardDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty)))
                    {
                        // upating order id and status field
                        UpdateOrderStatus(orderNumber, int.Parse(row["id"].ToString()));

                        //Console.WriteLine("The Order [" + orderNumber + "]  has been processed successfully.");
                        //Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="id">The id.</param>
        private static void UpdateOrderStatus(string orderNumber, int id)
        {
            using (var con = new SqlConnection(Utilities._connString))
            {
                using (var command = new SqlCommand("update ShareAThonDonationFrequency set OrderId='" + orderNumber + "', status='Process' where id=" + id, con))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Getcurrents the data.
        /// </summary>
        /// <returns></returns>
        private static DataTable GetcurrentData()
        {
            using (var con = new SqlConnection(Utilities._connString))
            {
                using (var adapter = new SqlDataAdapter("select * from shareathondonationFrequency where duedate='" + DateTime.Now + "' and status='Pending'", con))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    return dataSet.Tables[0];
                }
            }
        }

        /// <summary>
        /// Gets the unit price.
        /// </summary>
        /// <param name="offerNo">The offer no.</param>
        /// <returns></returns>
        private static decimal GetUnitPrice(string offerNo)
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
        /// Gets the offered lines.
        /// </summary>
        /// <param name="donId">The don id.</param>
        /// <returns></returns>
        private static DataTable GetOfferedLines(int donId)
        {
            using (var con = new SqlConnection(Utilities._connString))
            {
                using (var adapter = new SqlDataAdapter("select * from shareathonofferlines where ShareAThonDonationId=" + donId, con))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    return dataSet.Tables[0];
                }
            }
        }

        /// <summary>
        /// Gets the customer id.
        /// </summary>
        /// <param name="donId">The don id.</param>
        /// <returns></returns>
        private static string GetCustomerId(int donId)
        {
            using (var con = new SqlConnection(Utilities._connString))
            {
                using (var command = new SqlCommand("select CustomerId from ShareAThonDonation where id=" + donId, con))
                {
                    command.Connection.Open();
                    return (command.ExecuteScalar() != null) ? command.ExecuteScalar().ToString() : string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the ordered items.
        /// </summary>
        /// <param name="nodeList">The node list.</param>
        /// <param name="donId">The don id.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        private static List<OrderItems> GetOrderedItems(XmlNodeList nodeList, int donId, string orderNumber)
        {
            return (from DataRow dr in GetOfferedLines(donId).Rows
                    where !string.IsNullOrEmpty(dr["qty"].ToString()) && Convert.ToInt32(dr["qty"].ToString()) > 0
                    select new OrderItems
                    {
                        SOPTYPE = 2,
                        SOPNUMBE = orderNumber,
                        CUSTNMBR = (nodeList[0]["CUSTNMBR"] != null) ? nodeList[0]["CUSTNMBR"].InnerText : string.Empty,
                        DOCDATE = DateTime.Now.ToShortDateString(),
                        ITEMNMBR = (dr["OfferNo"] != null) ? dr["OfferNo"].ToString() : string.Empty,
                        ITEMDESC = (dr["description"] != null) ? dr["description"].ToString() : string.Empty,
                        UNITPRCE = GetUnitPrice((dr["OfferNo"] != null) ? dr["OfferNo"].ToString() : string.Empty),
                        QUANTITY = (dr["qty"] != null) ? decimal.Parse(dr["qty"].ToString()) : 0,
                        XTNDPRCE = ((dr["qty"] != null) ? decimal.Parse(dr["qty"].ToString()) : 0) * (GetUnitPrice((dr["OfferNo"] != null) ? dr["OfferNo"].ToString() : string.Empty)),
                        TOTALQTY = 0,
                        CURNCYID = "",
                        UOFM = "",
                        NONINVEN = 0,
                        ShipToName = "",
                        ADDRESS1 = (nodeList[0]["ADDRESS1"] != null) ? nodeList[0]["ADDRESS1"].InnerText : string.Empty,
                        ADDRESS2 = (nodeList[0]["ADDRESS2"] != null) ? nodeList[0]["ADDRESS2"].InnerText : string.Empty,
                        CITY = (nodeList[0]["CITY"] != null) ? nodeList[0]["CITY"].InnerText : string.Empty,
                        STATE = (nodeList[0]["STATE"] != null) ? nodeList[0]["STATE"].InnerText : string.Empty,
                        ZIPCODE = (nodeList[0]["ZIP"] != null) ? nodeList[0]["ZIP"].InnerText : string.Empty,
                        COUNTRY = (nodeList[0]["COUNTRY"] != null) ? nodeList[0]["COUNTRY"].InnerText : string.Empty,
                        PHNUMBR1 = (nodeList[0]["PHONE1"] != null) ? nodeList[0]["PHONE1"].InnerText : string.Empty,
                    }).ToList();
        }

        /// <summary>
        /// Gets the customer header.
        /// </summary>
        /// <param name="nodeList">The node list.</param>
        /// <param name="id">The id.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="subTotal">The sub total.</param>
        /// <returns></returns>
        private static OrderProcess GetCustomerHeader(XmlNodeList nodeList, int id, string orderNumber, decimal subTotal)
        {
            OrderProcess orderProcess = null;

            using (var con = new SqlConnection(Utilities._connString))
            {
                using (var adapter = new SqlDataAdapter("select * from ShareAThonDonation where id=" + id, con))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        orderProcess = new OrderProcess
                        {
                            SOPNUMBE = orderNumber,
                            SOPTYPE = 2,
                            BACHNUMB = "SHAREATHON  ",
                            DOCID = "STDORD",
                            CUSTNMBR = dr["CustomerId"].ToString(),
                            CUSTNAME = (nodeList[0]["CUSTNMBR"] != null) ? nodeList[0]["CUSTNMBR"].InnerText : string.Empty,
                            SUBTOTAL = subTotal,
                            DOCDATE = DateTime.Now.ToShortDateString(),
                            ORDRDATE = DateTime.Now.ToShortDateString(),
                            ShipToName = "",
                            ADDRESS1 = (nodeList[0]["ADDRESS1"] != null) ? nodeList[0]["ADDRESS1"].InnerText : string.Empty,
                            ADDRESS2 = (nodeList[0]["ADDRESS2"] != null) ? nodeList[0]["ADDRESS2"].InnerText : string.Empty,
                            CITY = (nodeList[0]["CITY"] != null) ? nodeList[0]["CITY"].InnerText : string.Empty,
                            STATE = (nodeList[0]["STATE"] != null) ? nodeList[0]["STATE"].InnerText : string.Empty,
                            ZIPCODE = (nodeList[0]["ZIP"] != null) ? nodeList[0]["ZIP"].InnerText : string.Empty,
                            COUNTRY = (nodeList[0]["COUNTRY"] != null) ? nodeList[0]["COUNTRY"].InnerText : string.Empty,
                            PHNUMBR1 = (nodeList[0]["PHONE1"] != null) ? nodeList[0]["PHONE1"].InnerText : string.Empty,
                            FREIGHT = 0,
                            FRTTXAMT = 0,
                            MISCAMNT = 0,
                            MSCTXAMT = 0,
                            TRDISAMT = 0,
                            TAXAMNT = 0,
                        };
                        orderProcess.DOCAMNT = Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT + orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) - Convert.ToDecimal(orderProcess.TRDISAMT);
                    }
                }
            }
            return orderProcess;
        }
    }
}