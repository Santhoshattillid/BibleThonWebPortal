﻿using System.Collections.Generic;
using System.Xml;

namespace AlbaDL
{
    /// <summary>
    /// Summary description for ShippingAddress
    /// </summary>
    public class ShippingAddress
    {
        public string CustomerNo { get; set; }

        public string AddressCode { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Telephone1 { get; set; }

        public string Telephone2 { get; set; }

        public string Telephone3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// To get list of shipping addresses from GP
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="customerNo"> </param>
        /// <returns></returns>
        public IEnumerable<ShippingAddress> GetCustomerShipAddress(string connectionString, string customerNo)
        {
            var customeAddress = new List<ShippingAddress>();
            var customerDetails = new EConnectModel().GetCustomerDetails(connectionString);
            var customerDoc = new XmlDocument();
            customerDoc.LoadXml(customerDetails);
            XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer/Address");
            if (xmlList != null)
                foreach (XmlNode xn in xmlList)
                {
                    if (!string.IsNullOrEmpty(xn.InnerText))
                    {
                        if (xn["CUSTNMBR"] != null && xn["CUSTNMBR"].InnerText.Trim().ToLower().Equals(customerNo.ToLower().Trim()) && xn["ADRSCODE"] != null && xn["ADDRESS1"] != null && xn["ADDRESS2"] != null && xn["ADDRESS3"] != null && xn["PHONE1"] != null && xn["PHONE2"] != null)
                        {
                            var shippingAddr = new ShippingAddress
                                                   {
                                                       CustomerNo = xn["CUSTNMBR"].InnerText,
                                                       AddressCode = xn["ADRSCODE"].InnerText,
                                                       Address1 = xn["ADDRESS1"].InnerText,
                                                       Address2 = xn["ADDRESS2"].InnerText,
                                                       Address3 = xn["ADDRESS3"].InnerText,
                                                       Telephone3 = xn["PHONE3"].InnerText,
                                                       City = xn["CITY"].InnerText,
                                                       State = xn["STATE"].InnerText,
                                                       Zipcode = xn["ZIP"].InnerText,
                                                       Country = xn["COUNTRY"].InnerText,
                                                       Email = "",
                                                       Telephone1 =
                                                           FormatUtilities.FormatToTelephone(xn["PHONE1"].InnerText),
                                                       Telephone2 =
                                                           FormatUtilities.FormatToTelephone(xn["PHONE2"].InnerText)
                                                   };
                            customeAddress.Add(shippingAddr);
                        }
                    }
                }
            return customeAddress;
        }
    }
}