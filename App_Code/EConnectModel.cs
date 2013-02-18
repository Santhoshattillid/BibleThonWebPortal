using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using Biblethon.Controller;
using System.Text;

namespace Biblethon.Controller
{
    public class EConnectModel
    {
        public string GetCustomerDetails(string connString)
        {
            using (eConnectMethods eConnectMethods = new eConnectMethods())
            {
                eConnectMethods econ = new eConnectMethods();
                eConnectOut myRequest = new eConnectOut();
                myRequest.DOCTYPE = "Customer";
                myRequest.OUTPUTTYPE = 2;
                myRequest.FORLIST = 1;

                //Create the requestor schema document type
                //Since the eConnect document requires an array, create an 
                //array of RQeConnectOutType
                RQeConnectOutType[] econnectOutType = { new RQeConnectOutType() };
                econnectOutType[0].eConnectOut = myRequest;
                //Create the eConnect document type
                eConnectType eConnectDoc = new eConnectType();
                eConnectDoc.RQeConnectOutType = econnectOutType;
                //**Serialize the eConnect document**
                //Create a memory stream for the serialized eConnect document
                MemoryStream memStream = new MemoryStream();
                //Create an Xml Serializer and serialize the eConnect document 
                //to the memory stream
                XmlSerializer serializer = new XmlSerializer(typeof(eConnectType));
                serializer.Serialize(memStream, eConnectDoc);
                //Reset the position property to the start of the buffer
                memStream.Position = 0;
                //**Load the serialized Xml into an Xml document**
                XmlDocument xmlCustomerdoc = new XmlDocument();
                xmlCustomerdoc.Load(memStream);
                //Retrieve the specified document
                var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);
                return customerDoc;
            }
        }

        public string GetOfferDetails(string connString)
        {
            using (eConnectMethods eConnectMethods = new eConnectMethods())
            {
                eConnectMethods econ = new eConnectMethods();
                eConnectOut myRequest = new eConnectOut();
                myRequest.DOCTYPE = "Item_ListPrice";
                myRequest.OUTPUTTYPE = 2;
                myRequest.FORLIST = 1;

                //Create the requestor schema document type
                //Since the eConnect document requires an array, create an 
                //array of RQeConnectOutType
                RQeConnectOutType[] econnectOutType = { new RQeConnectOutType() };
                econnectOutType[0].eConnectOut = myRequest;
                //Create the eConnect document type
                eConnectType eConnectDoc = new eConnectType();
                eConnectDoc.RQeConnectOutType = econnectOutType;
                //**Serialize the eConnect document**
                //Create a memory stream for the serialized eConnect document
                MemoryStream memStream = new MemoryStream();
                //Create an Xml Serializer and serialize the eConnect document 
                //to the memory stream
                XmlSerializer serializer = new XmlSerializer(typeof(eConnectType));
                serializer.Serialize(memStream, eConnectDoc);
                //Reset the position property to the start of the buffer
                memStream.Position = 0;
                //**Load the serialized Xml into an Xml document**
                XmlDocument xmlCustomerdoc = new XmlDocument();
                xmlCustomerdoc.Load(memStream);
                //Retrieve the specified document
                var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);
                return customerDoc;
            }
        }

        public string GetNextSalseDocNumber(string connString)
        {

            //GetNextDocNumbers sopTransNumber = new GetNextDocNumbers();

            //return sopTransNumber.GetNextSOPNumber(IncrementDecrement.Increment, "2", SopType.SOPOrder, connString);
            GetSopNumber sopTransNumber = new GetSopNumber();

            return sopTransNumber.GetNextSopNumber(2, "STDORD", connString);
        }

        public bool RollbackSalseDocNumber(string connString)
        {
            GetSopNumber sopTransNumber = new GetSopNumber();
            string sopNumKey=HttpContext.Current.Session["orderNumber"].ToString();
            return sopTransNumber.RollBackSopNumber(sopNumKey, 2, "STDORD", connString);
        }

        public taSopHdrIvcInsert GetHeaderItems(OrderProcess order)
        {
            taSopHdrIvcInsert salesHdr = new taSopHdrIvcInsert();
            try
            {
                //string PaymentType = order.Payment.PaymentMethod.ToString();

                //salesHdr.SHIPMTHD = order.Shipping.Name;
                //salesHdr.USINGHEADERLEVELTAXES = 1;
                salesHdr.SOPTYPE = order.SOPTYPE;
                salesHdr.SOPNUMBE = order.SOPNUMBE;
                salesHdr.BACHNUMB = order.BACHNUMB;
                //salesHdr.PYMTRCVD = order.Invoice.Total.Value + GiftCertificateAmount;
                salesHdr.DOCID = order.DOCID;
                // salesHdr.DOCID = "STDINV";
                salesHdr.CUSTNMBR = order.CUSTNMBR;
                salesHdr.CUSTNAME = order.CUSTNAME;
                //salesHdr.TXRGNNUM = 
                salesHdr.SUBTOTAL = order.SUBTOTAL;
                salesHdr.DOCAMNT = order.DOCAMNT;
                salesHdr.DOCDATE = order.DOCDATE;
                salesHdr.ORDRDATE = order.ORDRDATE;
                salesHdr.ShipToName = order.ShipToName;
                salesHdr.ADDRESS1 = order.ADDRESS1;
                salesHdr.ADDRESS2 = order.ADDRESS2;
                salesHdr.ADDRESS3 = order.ADDRESS3;
                salesHdr.CITY = order.CITY;
                salesHdr.STATE = order.STATE;
                salesHdr.ZIPCODE = order.ZIPCODE;
                salesHdr.COUNTRY = order.COUNTRY;
                salesHdr.PHNUMBR1 = order.PHNUMBR1;



            }
            catch (Exception exp)
            {
                throw new Exception(Environment.NewLine + Environment.NewLine + "Date :" + DateTime.Now.ToString() + " " + exp.ToString());


            }
            return salesHdr;
        }

        public taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] GetLineItems(List<OrderItems> order)
        {
            string UofM = string.Empty;

            int LineSeq = 0;
            string PartNO = string.Empty;
            string OrderQTY = string.Empty;
            string ProdUnitPrice = string.Empty;
            int lineCount = order.Count;
            List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert> lineItems = new List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert>();
            try
            {
                for (int i = 0; i < lineCount; i++)
                {
                    taSopLineIvcInsert_ItemsTaSopLineIvcInsert salesLine = new taSopLineIvcInsert_ItemsTaSopLineIvcInsert();

                    salesLine.SOPTYPE = order[i].SOPTYPE;
                    salesLine.SOPNUMBE = order[i].SOPNUMBE;
                    salesLine.CUSTNMBR = order[i].CUSTNMBR;


                    //salesLine.DOCID = order[i].DOCID;
                    //salesLine.DOCID = "STDINV";

                    salesLine.QUANTITY = order[i].QUANTITY;
                    salesLine.ITEMNMBR = order[i].ITEMNMBR;
                    salesLine.ITEMDESC = order[i].ITEMDESC;
                    salesLine.UNITPRCE = order[i].UNITPRCE;
                    //salesLine.MRKDNAMT = 0;
                    //salesLine.UNITCOST = order[i].UNITCOST;
                    salesLine.XTNDPRCE = order[i].XTNDPRCE;
                    //salesLine.SLPRSNID = order[i].SLPRSNID;
                    //salesLine.CURNCYID = order[i].CURNCYID;
                    // salesLine.LOCNCODE = entityOrder.LocationCode;
                    //salesLine.LOCNCODE = "WAREHOUSE";

                    //salesLine.NONINVEN = order[i].NONINVEN;
                    salesLine.DOCDATE = order[i].DOCDATE;
                    salesLine.TOTALQTY = order[i].TOTALQTY;
                    //salesLine.CURNCYID = order[i].CURNCYID;
                    salesLine.ShipToName = order[i].ShipToName;
                    salesLine.ADDRESS1 = order[i].ADDRESS1;
                    salesLine.ADDRESS2 = order[i].ADDRESS2;
                    salesLine.ADDRESS3 = order[i].ADDRESS3;
                    salesLine.CITY = order[i].CITY;
                    salesLine.STATE = order[i].STATE;
                    salesLine.ZIPCODE = order[i].ZIPCODE;
                    salesLine.COUNTRY = order[i].COUNTRY;
                    salesLine.PHONE1 = order[i].PHNUMBR1;

                    //LineSeq = LineSeq + 16384;

                    //salesLine.LNITMSEQ = LineSeq;
                    lineItems.Add(salesLine);
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());


            }
            return lineItems.ToArray();
        }

        public bool SerializeSalesOrderObject(string filename, string sConnectionString, OrderProcess order, List<OrderItems> orderItems)
        {
            bool status=false;
            XmlSerializer serializer = new XmlSerializer(typeof(eConnectType));
            eConnectType eConnect = new eConnectType();
            SOPTransactionType SOPTrnType = new SOPTransactionType();
            taSopHdrIvcInsert SOPHdrInvInsert = GetHeaderItems(order);
            taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] SOPLineInvInsert = GetLineItems(orderItems);

            SOPTrnType.taSopLineIvcInsert_Items = SOPLineInvInsert;
            SOPTrnType.taSopHdrIvcInsert = SOPHdrInvInsert;
            Array.Resize(ref eConnect.SOPTransactionType, 1);
            eConnect.SOPTransactionType[0] = SOPTrnType;
            FileStream fs = new FileStream(filename, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, new UTF8Encoding());
            string SOPTransactionDoc = string.Empty;
            eConnectMethods eConCall = new eConnectMethods();
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            serializer.Serialize(writer, eConnect);
            writer.Close();
            //xmldoc.Load("SOPTransaction.xml");
            xmldoc.Load(filename);
            SOPTransactionDoc = xmldoc.OuterXml;
            try
            {
                status = eConCall.CreateEntity(sConnectionString, SOPTransactionDoc);
            }
            catch (eConnectException exp)
            {
                throw new Exception(exp.ToString());
            }
            finally
            {
                eConCall.Dispose();
            }
            return status;
        }


    }
}