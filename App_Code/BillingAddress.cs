using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;

namespace Biblethon.Controller
{
    public class BillingAddress : IBillingAddress
    {
        public string CustomerNo { get; set; }

        public string CustomerName { get; set; }

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

        public List<BillingAddress> GetCustomerDetails(string connString)
        {
            List<BillingAddress> customeDetails = new List<BillingAddress>();
            var customerDetails = new EConnectModel().GetCustomerDetails(connString);
            XmlDocument customerDoc = new XmlDocument();
            customerDoc.LoadXml(customerDetails);
            XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer");
            foreach (XmlNode xn in xmlList)
            {
                BillingAddress BillingAddr = new BillingAddress();

                BillingAddr.CustomerNo = xn["CUSTNMBR"].InnerText;
                BillingAddr.CustomerName = xn["CUSTNAME"].InnerText;
                BillingAddr.Address1 = xn["ADDRESS1"].InnerText;
                BillingAddr.Address2 = xn["ADDRESS2"].InnerText;
                BillingAddr.Address3 = xn["ADDRESS3"].InnerText;
                BillingAddr.Telephone1 = xn["PHONE1"].InnerText;
                BillingAddr.Telephone2 = xn["PHONE2"].InnerText;
                BillingAddr.Telephone3 = xn["PHONE3"].InnerText;
                BillingAddr.City = xn["CITY"].InnerText;
                BillingAddr.State = xn["STATE"].InnerText;
                BillingAddr.Zipcode = xn["ZIP"].InnerText;
                BillingAddr.Country = xn["COUNTRY"].InnerText;
                //if (xn.SelectNodes("INET1").Count>0)
                //    BillingAddr.Email = xn.["INET1"].InnerText;
                //else
                BillingAddr.Email = "";
                BillingAddr.AddressCode = xn["ADRSCODE"].InnerText;
                customeDetails.Add(BillingAddr);
            }
            return customeDetails;
        }

        public BillingAddress GetBillingAddress()
        {
            BillingAddress billing = new BillingAddress();
            //Do Somthing
            return billing;
        }

        public BillingAddress GetBillingAddressByName(string name, string phoneNumber)
        {
            BillingAddress billing = new BillingAddress();
            //Do Somthing
            return billing;
        }

        public BillingAddress GetBillingAddressByAddress(string name, string addressFirstLine)
        {
            BillingAddress billing = new BillingAddress();
            //Do Somthing
            return billing;
        }

    }
}