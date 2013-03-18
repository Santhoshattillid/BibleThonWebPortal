using System;
using System.Collections.Generic;
using System.Xml;
using AlbaDL;

public class OfferLines
{
    public string OfferId { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Total { get; set; }

    public List<OfferLines> GetOfferLines(string connectionString)
    {
        var offerLines = new List<OfferLines>();
        var offerDetails = new EConnectModel().GetOfferDetails(connectionString);
        var customerDoc = new XmlDocument();
        customerDoc.LoadXml(offerDetails);
        XmlNodeList xmlOfferList = customerDoc.SelectNodes("root/eConnect/Item/ListPrice");
        if (xmlOfferList != null)
            foreach (XmlNode of in xmlOfferList)
            {
                if (of["CURNCYID"] != null && of["LISTPRCE"] != null && of.ParentNode != null && of.ParentNode["ITEMNMBR"] != null && of.ParentNode["ITEMDESC"] != null)
                {
                    var offlinr = new OfferLines();
                    if (of["CURNCYID"].InnerText == "Z-US$")
                    {
                        var price = Convert.ToDecimal(of["LISTPRCE"].ChildNodes[0].InnerText);
                        if (price > 0)
                        {
                            offlinr.Price = Convert.ToDecimal(of["LISTPRCE"].ChildNodes[0].InnerText);
                            offlinr.OfferId = of.ParentNode["ITEMNMBR"].InnerText;
                            offlinr.Description = of.ParentNode["ITEMDESC"].InnerText;
                            offerLines.Add(offlinr);
                        }
                    }
                }
            }

        //Do somthing
        return offerLines;
    }
}

public class OrderItems
{
    public short SOPTYPE { get; set; }

    public string SOPNUMBE { get; set; }

    public string CUSTNMBR { get; set; }

    public string DOCDATE { get; set; }

    public string ITEMNMBR { get; set; }

    public decimal UNITPRCE { get; set; }

    public decimal XTNDPRCE { get; set; }

    public decimal QUANTITY { get; set; }

    public decimal UNITCOST { get; set; }

    public string ITEMDESC { get; set; }

    public short NONINVEN { get; set; }

    public string SLPRSNID { get; set; }

    public decimal TOTALQTY { get; set; }

    public string CURNCYID { get; set; }

    public string UOFM { get; set; }

    public string ShipToName { get; set; }

    public string ADDRESS1 { get; set; }

    public string ADDRESS2 { get; set; }

    public string ADDRESS3 { get; set; }

    public string CITY { get; set; }

    public string STATE { get; set; }

    public string ZIPCODE { get; set; }

    public string COUNTRY { get; set; }

    public string PHNUMBR1 { get; set; }

    public short SOPSTATUS { get; set; }
}