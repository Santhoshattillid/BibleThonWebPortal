using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Biblethon.Controller;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.Serialization;

namespace AlbaDL
{
    public class EConnectModel
    {
        public string GetCustomerDetails(string connString)
        {
            var econ = new eConnectMethods();

            var myRequest = new eConnectOut { DOCTYPE = "Customer", OUTPUTTYPE = 2, FORLIST = 1 };

            RQeConnectOutType[] econnectOutType = { new RQeConnectOutType() };
            econnectOutType[0].eConnectOut = myRequest;

            var eConnectDoc = new eConnectType { RQeConnectOutType = econnectOutType };

            var memStream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(eConnectType));
            serializer.Serialize(memStream, eConnectDoc);

            memStream.Position = 0;
            var xmlCustomerdoc = new XmlDocument();
            xmlCustomerdoc.Load(memStream);

            // version 11
            var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            // version 10
            //var customerDoc = econ.eConnect_Requester(connString,EnumTypes.ConnectionStringType.SqlClient ,xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        public string GetOfferDetails(string connString)
        {
            var econ = new eConnectMethods();
            var myRequest = new eConnectOut { DOCTYPE = "Item_ListPrice", OUTPUTTYPE = 2, FORLIST = 1 };
            RQeConnectOutType[] econnectOutType = { new RQeConnectOutType() };
            econnectOutType[0].eConnectOut = myRequest;
            var eConnectDoc = new eConnectType { RQeConnectOutType = econnectOutType };
            var memStream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(eConnectType));
            serializer.Serialize(memStream, eConnectDoc);
            memStream.Position = 0;
            var xmlCustomerdoc = new XmlDocument();
            xmlCustomerdoc.Load(memStream);

            // version 11
            var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            //var customerDoc = econ.eConnect_Requester(connString, EnumTypes.ConnectionStringType.SqlClient, xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        public string GetNextSalseDocNumber(string connString)
        {
            var sopTransNumber = new GetSopNumber();
            return sopTransNumber.GetNextSopNumber(2, "STDORD", connString);
        }

        public bool RollbackSalseDocNumber(string connString, string orderNumber)
        {
            var sopTransNumber = new GetSopNumber();

            //string sopNumKey = HttpContext.Current.Session["orderNumber"].ToString();
            return sopTransNumber.RollBackSopNumber(orderNumber, 2, "STDORD", connString);
        }

        public taUpdateCreateCustomerRcd GetCustomerDetails(NewCustomer customer)
        {
            var taCreateCustomerRcd = new taUpdateCreateCustomerRcd
                                          {
                                              CUSTNMBR = customer.CustomerNumber,
                                              CUSTNAME = customer.CustomerName,
                                              ADRSCODE = customer.AddressCode,
                                              ADDRESS1 = customer.Address1,
                                              ADDRESS2 = customer.Address2,
                                              CITY = customer.City,
                                              STATE = customer.State,
                                              ZIPCODE = customer.ZipCode,
                                              COUNTRY = customer.Country,
                                              PHNUMBR1 = customer.PhoneNumber1,
                                              PHNUMBR2 = customer.PhoneNumber2,
                                              FAX = customer.Fax,
                                              CRCARDID = customer.CreditCardId,
                                              CRCRDNUM = customer.CreditCardNumber,
                                              CCRDXPDT = customer.CreditExpiryDate,
                                              UpdateIfExists = customer.UpdateIfExists
                                          };
            return taCreateCustomerRcd;
        }

        public taCreateInternetAddresses_ItemsTaCreateInternetAddresses[] GetCustomerInetInfo(List<CustomerInetInfo> customerInetInfo)
        {
            var addressCount = customerInetInfo.Count;
            var custInternetAddress = new List<taCreateInternetAddresses_ItemsTaCreateInternetAddresses>();
            for (int i = 0; i < addressCount; i++)
            {
                var taCreateCustomerInetaddr = new taCreateInternetAddresses_ItemsTaCreateInternetAddresses
                                                   {
                                                       Master_ID = customerInetInfo[i].MasterId,
                                                       Master_Type = customerInetInfo[i].MasterType,
                                                       ADRSCODE = customerInetInfo[i].AddressCode,
                                                       INET1 = customerInetInfo[i].IntenetInfo1,
                                                       INET2 = customerInetInfo[i].IntenetInfo2,
                                                       INET3 = customerInetInfo[i].IntenetInfo3,
                                                       INET4 = customerInetInfo[i].IntenetInfo4,
                                                       EmailToAddress = customerInetInfo[i].EmailToAddress,
                                                       EmailBccAddress = customerInetInfo[i].EmailBccAddress,
                                                       EmailCcAddress = customerInetInfo[i].EmailCcAddress
                                                   };
                custInternetAddress.Add(taCreateCustomerInetaddr);
            }
            return custInternetAddress.ToArray();
        }

        public taSopHdrIvcInsert GetHeaderItems(OrderProcess order)
        {
            var salesHdr = new taSopHdrIvcInsert();
            try
            {
                salesHdr.SOPTYPE = order.SOPTYPE;
                salesHdr.SOPNUMBE = order.SOPNUMBE;
                salesHdr.BACHNUMB = order.BACHNUMB;
                salesHdr.DOCID = order.DOCID;
                salesHdr.CUSTNMBR = order.CUSTNMBR;
                salesHdr.CUSTNAME = order.CUSTNAME;
                salesHdr.FREIGHT = order.FREIGHT;
                salesHdr.FRTTXAMT = order.FRTTXAMT;
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
                salesHdr.MISCAMNT = order.MISCAMNT;
                salesHdr.MSCTXAMT = order.MSCTXAMT;
                salesHdr.TAXAMNT = order.TAXAMNT;
                salesHdr.TRDISAMT = order.TRDISAMT;
            }
            catch (Exception exp)
            {
                throw new Exception(Environment.NewLine + Environment.NewLine + "Date :" + DateTime.Now + " " + exp);
            }
            return salesHdr;
        }

        public taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] GetLineItems(List<OrderItems> order)
        {
            int lineCount = order.Count;
            var lineItems = new List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert>();
            try
            {
                for (int i = 0; i < lineCount; i++)
                {
                    var salesLine = new taSopLineIvcInsert_ItemsTaSopLineIvcInsert
                                        {
                                            SOPTYPE = order[i].SOPTYPE,
                                            SOPNUMBE = order[i].SOPNUMBE,
                                            CUSTNMBR = order[i].CUSTNMBR,
                                            QUANTITY = order[i].QUANTITY,
                                            ITEMNMBR = order[i].ITEMNMBR,
                                            ITEMDESC = order[i].ITEMDESC,
                                            UNITPRCE = order[i].UNITPRCE,
                                            XTNDPRCE = order[i].XTNDPRCE,
                                            DOCDATE = order[i].DOCDATE,
                                            TOTALQTY = order[i].TOTALQTY,
                                            ShipToName = order[i].ShipToName,
                                            ADDRESS1 = order[i].ADDRESS1,
                                            ADDRESS2 = order[i].ADDRESS2,
                                            ADDRESS3 = order[i].ADDRESS3,
                                            CITY = order[i].CITY,
                                            STATE = order[i].STATE,
                                            ZIPCODE = order[i].ZIPCODE,
                                            COUNTRY = order[i].COUNTRY,
                                            PHONE1 = order[i].PHNUMBR1
                                        };
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
            bool status;
            var serializer = new XmlSerializer(typeof(eConnectType));
            var eConnect = new eConnectType();
            var sopTrnType = new SOPTransactionType();
            taSopHdrIvcInsert sopHdrInvInsert = GetHeaderItems(order);
            taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] sopLineInvInsert = GetLineItems(orderItems);
            sopTrnType.taSopLineIvcInsert_Items = sopLineInvInsert;
            sopTrnType.taSopHdrIvcInsert = sopHdrInvInsert;
            Array.Resize(ref eConnect.SOPTransactionType, 1);
            eConnect.SOPTransactionType[0] = sopTrnType;
            var fs = new FileStream(filename, FileMode.Create);
            var writer = new XmlTextWriter(fs, new UTF8Encoding());
            var eConCall = new eConnectMethods();
            var xmldoc = new XmlDocument();
            serializer.Serialize(writer, eConnect);
            writer.Close();
            xmldoc.Load(filename);
            string sopTransactionDoc = xmldoc.OuterXml;
            try
            {
                //version 10
                //status = eConCall.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient,sopTransactionDoc, EnumTypes.SchemaValidationType.None);

                //version 11
                status = eConCall.CreateEntity(sConnectionString, sopTransactionDoc);
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

        public bool SerilizeCustomerObject(string filename, string sConnectionString, NewCustomer customer, List<CustomerInetInfo> customerInetInfo)
        {
            bool status;
            var serializer = new XmlSerializer(typeof(eConnectType));
            var eConnect = new eConnectType();
            var customerMasterType = new RMCustomerMasterType();
            taUpdateCreateCustomerRcd createCustomerRcd = GetCustomerDetails(customer);
            var createCustomerAddress = new taCreateCustomerAddress_ItemsTaCreateCustomerAddress[0];
            taCreateInternetAddresses_ItemsTaCreateInternetAddresses[] createInternetAddresses = GetCustomerInetInfo(customerInetInfo);
            customerMasterType.taUpdateCreateCustomerRcd = createCustomerRcd;
            customerMasterType.taCreateCustomerAddress_Items = createCustomerAddress;
            customerMasterType.taCreateInternetAddresses_Items = createInternetAddresses;
            Array.Resize(ref eConnect.RMCustomerMasterType, 1);
            eConnect.RMCustomerMasterType[0] = customerMasterType;
            var fs = new FileStream(filename, FileMode.Create);
            var writer = new XmlTextWriter(fs, new UTF8Encoding());
            var eConCall = new eConnectMethods();
            var xmldoc = new XmlDocument();
            serializer.Serialize(writer, eConnect);
            writer.Close();
            xmldoc.Load(filename);
            string customerDocument = xmldoc.OuterXml;
            try
            {
                //version 10
                //status = eConCall.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient,sopTransactionDoc, EnumTypes.SchemaValidationType.None);

                //version 11
                status = eConCall.CreateEntity(sConnectionString, customerDocument);
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