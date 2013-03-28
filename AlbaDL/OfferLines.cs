using System;
using System.Collections.Generic;
using System.Xml;

namespace AlbaDL
{
    public class OfferLines
    {
        public string OfferId { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// Getting list of Offerlines from GP
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
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
            return offerLines;
        }
    }
}