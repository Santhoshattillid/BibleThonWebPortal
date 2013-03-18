using System;
using System.Collections.Generic;
using System.Xml;
using AlbaDL;

public class BillingAddress
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
                }
                catch (Exception)
                {
                }

                XmlNodeList emailAddressNodes = xn.SelectNodes("Address/Internet_Address");

                if (emailAddressNodes != null)
                    foreach (XmlNode emailAddressNode in emailAddressNodes)
                    {
                        XmlElement xmlElement = emailAddressNode["ADRSCODE"];
                        if (xmlElement != null && String.CompareOrdinal(billingAddr.AddressCode, xmlElement.InnerText) == 0)
                        {
                            XmlElement element = emailAddressNode["INET1"];
                            if (element != null)
                                billingAddr.Email = element.InnerText;
                            break;
                        }
                    }

                customeDetails.Add(billingAddr);
            }
        return customeDetails;
    }

    public List<BillingAddress> GetCustomerBillingAddresses(string connString)
    {
        var customeDetails = new List<BillingAddress>();
        var customerDetails = new EConnectModel().GetCustomerDetails(connString);
        var customerDoc = new XmlDocument();
        customerDoc.LoadXml(customerDetails);
        XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer");
        if (xmlList != null)
            foreach (XmlNode xnParent in xmlList)
            {
                string customerName = xnParent["CUSTNAME"].InnerText;
                string creditCardNumber = xnParent["CRCRDNUM"].InnerText;
                string creditCardExpireDate = string.Empty;
                try
                {
                    creditCardExpireDate = Convert.ToDateTime(xnParent["CCRDXPDT"].InnerText).ToString("MMyy");
                }
                catch (Exception)
                {
                }

                XmlNodeList xmlNodeList = xnParent.SelectNodes("Address");
                if (xmlNodeList != null)
                    foreach (XmlNode xn in xmlNodeList)
                    {
                        if (xn["CUSTNMBR"] != null && xn["ADDRESS1"] != null && xn["ADDRESS2"] != null && xn["ADDRESS3"] != null)
                        {
                            var billingAddr = new BillingAddress
                                                  {
                                                      CustomerNo = xn["CUSTNMBR"].InnerText,
                                                      CustomerName = customerName,
                                                      Address1 = xn["ADDRESS1"].InnerText,
                                                      Address2 = xn["ADDRESS2"].InnerText,
                                                      Address3 = xn["ADDRESS3"].InnerText
                                                  };

                            if (!string.IsNullOrEmpty(xn["PHONE1"].InnerText) &&
                                xn["PHONE1"].InnerText.Trim() != "00000000000000")
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

                            if (!string.IsNullOrEmpty(xn["PHONE2"].InnerText) &&
                                xn["PHONE2"].InnerText.Trim() != "00000000000000")
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
                            billingAddr.AddressCode = xn["ADRSCODE"].InnerText;

                            billingAddr.CreditCardNumber = creditCardNumber;
                            billingAddr.CreditCardExpireDate = creditCardExpireDate;

                            XmlNodeList emailAddressNodes = xn.SelectNodes("Internet_Address");
                            if (emailAddressNodes != null)
                                foreach (XmlNode emailAddressNode in emailAddressNodes)
                                {
                                    XmlElement xmlElement = emailAddressNode["ADRSCODE"];
                                    if (xmlElement != null &&
                                        String.CompareOrdinal(billingAddr.AddressCode, xmlElement.InnerText) == 0)
                                    {
                                        XmlElement element = emailAddressNode["INET1"];
                                        if (element != null)
                                            billingAddr.Email = element.InnerText;
                                        break;
                                    }
                                }
                            customeDetails.Add(billingAddr);
                        }
                    }
            }
        return customeDetails;
    }

    public List<BillingAddress> GetCustomerDetails(string connString, string customerName, string telephone)
    {
        var customeDetails = new List<BillingAddress>();
        var customerDetails = new EConnectModel().GetCustomerDetails(connString);
        var customerDoc = new XmlDocument();
        customerDoc.LoadXml(customerDetails);
        XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer");
        if (xmlList != null)
            foreach (XmlNode xn in xmlList)
            {
                bool isRecordFound = xn["CUSTNAME"] != null && xn["CUSTNAME"].InnerText.ToLower().StartsWith(customerName.ToLower());
                if (!string.IsNullOrEmpty(telephone))
                {
                    if (!isRecordFound)
                        isRecordFound = xn["PHONE1"] != null && xn["PHONE1"].InnerText.ToLower().Trim().StartsWith(telephone.ToLower().Trim());
                    if (!isRecordFound)
                    {
                        // if the search is not found in the primary address  then start search on billing addresses
                        XmlNodeList xmlNodeList = xn.SelectNodes("Address");
                        if (xmlNodeList != null)
                            foreach (XmlNode xnBillingAddress in xmlNodeList)
                            {
                                isRecordFound = xnBillingAddress["PHONE1"] != null && xnBillingAddress["PHONE1"].InnerText.ToLower().Trim().StartsWith(telephone.ToLower().Trim());

                                // checking untill any record matches in billing addresses
                                if (isRecordFound)
                                    break;
                            }
                    }
                }
                if (isRecordFound)
                {
                    if (xn["CUSTNMBR"] != null && !string.IsNullOrEmpty(xn["CUSTNMBR"].InnerText))
                    {
                        var billingAddr = new BillingAddress
                                              {
                                                  CustomerNo = xn["CUSTNMBR"].InnerText,
                                                  CustomerName = xn["CUSTNAME"].InnerText,
                                                  Address1 = xn["ADDRESS1"].InnerText,
                                                  Address2 = xn["ADDRESS2"].InnerText,
                                                  Address3 = xn["ADDRESS3"].InnerText
                                              };

                        if (!string.IsNullOrEmpty(xn["PHONE1"].InnerText) &&
                            xn["PHONE1"].InnerText.Trim() != "00000000000000")
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

                        if (!string.IsNullOrEmpty(xn["PHONE2"].InnerText) &&
                            xn["PHONE2"].InnerText.Trim() != "00000000000000")
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
                            billingAddr.CreditCardExpireDate =
                                Convert.ToDateTime(xn["CCRDXPDT"].InnerText).ToString("MMyy");
                        }
                        catch (Exception)
                        {
                        }

                        XmlNodeList emailAddressNodes = xn.SelectNodes("Address/Internet_Address");

                        if (emailAddressNodes != null)
                            foreach (XmlNode emailAddressNode in emailAddressNodes)
                            {
                                XmlElement xmlElement = emailAddressNode["ADRSCODE"];
                                if (xmlElement != null &&
                                    String.CompareOrdinal(billingAddr.AddressCode, xmlElement.InnerText) == 0)
                                {
                                    XmlElement element = emailAddressNode["INET1"];
                                    if (element != null)
                                        billingAddr.Email = element.InnerText;
                                    break;
                                }
                            }

                        customeDetails.Add(billingAddr);
                    }
                }
            }

        return customeDetails;
    }
}