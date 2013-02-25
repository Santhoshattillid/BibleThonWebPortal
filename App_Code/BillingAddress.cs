﻿using System;
using System.Collections.Generic;
using System.Xml;
using Biblethon.Controller;

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

    public string CreditCardNumber { get; set; }

    public string CreditCardExpireDate { get; set; }

    public List<BillingAddress> GetCustomerDetails(string connString)
    {
        var customeDetails = new List<BillingAddress>();
        var customerDetails = new EConnectModel().GetCustomerDetails(connString);
        var customerDoc = new XmlDocument();
        customerDoc.LoadXml(customerDetails);
        XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer");
        if (xmlList != null)
            foreach (XmlNode xn in xmlList)
            {
                var billingAddr = new BillingAddress
                                      {
                                          CustomerNo = xn["CUSTNMBR"].InnerText,
                                          CustomerName = xn["CUSTNAME"].InnerText,
                                          Address1 = xn["ADDRESS1"].InnerText,
                                          Address2 = xn["ADDRESS2"].InnerText,
                                          Address3 = xn["ADDRESS3"].InnerText
                                      };

                if (!string.IsNullOrEmpty(xn["PHONE1"].InnerText) && xn["PHONE1"].InnerText.Trim() != "00000000000000")
                {
                    if (xn["PHONE1"].InnerText.Length > 3)
                        billingAddr.Telephone1 = "(" + xn["PHONE1"].InnerText.Substring(0, 3) + ") ";
                    if (xn["PHONE1"].InnerText.Length > 6)
                        billingAddr.Telephone1 += xn["PHONE1"].InnerText.Substring(3, 3) + "-";
                    if (xn["PHONE1"].InnerText.Length > 10)
                        billingAddr.Telephone1 += xn["PHONE1"].InnerText.Substring(6, 4) + " Ext. ";
                    if (xn["PHONE1"].InnerText.Length > 13)
                        billingAddr.Telephone1 += xn["PHONE1"].InnerText.Substring(10);
                }
                else
                    billingAddr.Telephone1 = string.Empty;

                if (!string.IsNullOrEmpty(xn["PHONE2"].InnerText) && xn["PHONE2"].InnerText.Trim() != "00000000000000")
                {
                    if (xn["PHONE2"].InnerText.Length > 3)
                        billingAddr.Telephone2 = "(" + xn["PHONE2"].InnerText.Substring(0, 3) + ") ";
                    if (xn["PHONE2"].InnerText.Length > 6)
                        billingAddr.Telephone2 += xn["PHONE2"].InnerText.Substring(3, 3) + "-";
                    if (xn["PHONE2"].InnerText.Length > 10)
                        billingAddr.Telephone2 += xn["PHONE2"].InnerText.Substring(6, 4) + " Ext. ";
                    if (xn["PHONE2"].InnerText.Length > 13)
                        billingAddr.Telephone2 += xn["PHONE2"].InnerText.Substring(10);
                }
                else
                    billingAddr.Telephone2 = string.Empty;
                billingAddr.Telephone3 = xn["PHONE3"].InnerText;
                billingAddr.City = xn["CITY"].InnerText;
                billingAddr.State = xn["STATE"].InnerText;
                billingAddr.Zipcode = xn["ZIP"].InnerText;
                billingAddr.Country = xn["COUNTRY"].InnerText;
                billingAddr.Email = "";
                billingAddr.AddressCode = xn["ADRSCODE"].InnerText;
                billingAddr.CreditCardNumber = xn["CRCRDNUM"].InnerText;
                try
                {
                    billingAddr.CreditCardExpireDate = Convert.ToDateTime(xn["CCRDXPDT"].InnerText).ToString("MMyy");
                }catch(Exception)
                {
                }
                customeDetails.Add(billingAddr);
            }
        return customeDetails;
    }

    public BillingAddress GetBillingAddress()
    {
        var billing = new BillingAddress();
        //Do Somthing
        return billing;
    }

    public BillingAddress GetBillingAddressByName(string name, string phoneNumber)
    {
        var billing = new BillingAddress();
        //Do Somthing
        return billing;
    }

    public BillingAddress GetBillingAddressByAddress(string name, string addressFirstLine)
    {
        var billing = new BillingAddress();
        //Do Somthing
        return billing;
    }

}