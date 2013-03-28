using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.MiscRoutines;
using Microsoft.Dynamics.GP.eConnect.Serialization;

namespace AlbaDL
{
    public class EConnectModel
    {
        #region Public Methods

        /// <summary>
        /// Get list of customer from GP
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
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
            //var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            // version 10
            var customerDoc = econ.eConnect_Requester(connString, EnumTypes.ConnectionStringType.SqlClient, xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        /// <summary>
        /// Get particular customer from GP based on customer id
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string GetCustomerDetails(string connString, string customerId)
        {
            var econ = new eConnectMethods();

            var myRequest = new eConnectOut { DOCTYPE = "Customer", OUTPUTTYPE = 2, FORLIST = 1, WhereClause = "custnmbr='" + customerId + "'" };

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
            //var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            // version 10
            var customerDoc = econ.eConnect_Requester(connString, EnumTypes.ConnectionStringType.SqlClient, xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        /// <summary>
        /// Getting Offer Details XML from GP
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
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
            //var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            var customerDoc = econ.eConnect_Requester(connString, EnumTypes.ConnectionStringType.SqlClient, xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        /// <summary>
        /// Generating next sales doc number in GP for order processing
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public string GetNextSalseDocNumber(string connString)
        {
            var sopTransNumber = new GetSopNumber();
            return sopTransNumber.GetNextSopNumber(2, "STDORD", connString);
        }

        /// <summary>
        /// Method to roll back the generated order number
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="orderNumber"></param>
        public void RollbackSalseDocNumber(string connString, string orderNumber)
        {
            var sopTransNumber = new GetSopNumber();
            sopTransNumber.RollBackSopNumber(orderNumber, 2, "STDORD", connString);
        }

        /// <summary>
        /// Method to create customer into GP
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sConnectionString"></param>
        /// <param name="customer"></param>
        /// <param name="customerInetInfo"></param>
        /// <returns></returns>
        public bool SerilizeCustomerObject(string filename, string sConnectionString, CustomerDetails customer, List<CustomerInetInfo> customerInetInfo)
        {
            bool status;
            var serializer = new XmlSerializer(typeof(eConnectType));
            var eConnect = new eConnectType();
            var customerMasterType = new RMCustomerMasterType();
            taUpdateCreateCustomerRcd createCustomerRcd = GetCustomerDetails(customer);
            var createCustomerAddress = new taCreateCustomerAddress_ItemsTaCreateCustomerAddress[0];

            //taCreateInternetAddresses_ItemsTaCreateInternetAddresses[] createInternetAddresses = GetCustomerInetInfo(customerInetInfo);
            customerMasterType.taUpdateCreateCustomerRcd = createCustomerRcd;
            customerMasterType.taCreateCustomerAddress_Items = createCustomerAddress;

            //customerMasterType.taCreateInternetAddresses_Items = createInternetAddresses;
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
                status = eConCall.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient, customerDocument, EnumTypes.SchemaValidationType.None);

                //version 11
                //status = eConCall.CreateEntity(sConnectionString, customerDocument);
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

        /// <summary>
        /// Method to process order into GP
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sConnectionString"></param>
        /// <param name="order"></param>
        /// <param name="orderItems"></param>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        public bool SerializeSalesOrderObject(string filename, string sConnectionString, OrderProcess order, List<OrderItems> orderItems, CardDetails cardDetails)
        {
            var sopOrderPayment = StoreSopOrderPayment(order, sConnectionString, cardDetails);

            bool status;
            var serializer = new XmlSerializer(typeof(eConnectType));
            var eConnect = new eConnectType();
            var sopTrnType = new SOPTransactionType();
            taSopHdrIvcInsert sopHdrInvInsert = GetHeaderItems(order);
            taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] sopLineInvInsert = GetLineItems(orderItems);
            taCreateSopPaymentInsertRecord_ItemsTaCreateSopPaymentInsertRecord[] sopPaymentInsert = GetSopPaymentInsertRecord(sopOrderPayment);
            sopTrnType.taSopLineIvcInsert_Items = sopLineInvInsert;
            sopTrnType.taSopHdrIvcInsert = sopHdrInvInsert;
            sopTrnType.taCreateSopPaymentInsertRecord_Items = sopPaymentInsert;
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
                status = eConCall.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient, sopTransactionDoc, EnumTypes.SchemaValidationType.None);

                //version 11
                //status = eConCall.CreateEntity(sConnectionString, sopTransactionDoc);
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

        /// <summary>
        /// Getting list of address for particular customer
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        public List<CustomerAddress> GtCustomerAddress(string connString, string customerNumber)
        {
            var customerAddressList = new List<CustomerAddress>();
            var customerAddresses = GetCustomerDetails(connString, customerNumber);
            var customerDoc = new XmlDocument();
            customerDoc.LoadXml(customerAddresses);
            XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer/Address");
            if (xmlList != null)
                customerAddressList.AddRange(from XmlNode xn in xmlList
                                             select new CustomerAddress
                                                        {
                                                            CustomerNumber = xn["CUSTNMBR"].InnerText,
                                                            AddressCode = xn["ADRSCODE"].InnerText,
                                                            SalesPersonId = xn["SLPRSNID"].InnerText,
                                                            UpsZone = xn["UPSZONE"].InnerText,
                                                            ShippingMethod = xn["SHIPMTHD"].InnerText,
                                                            TaxScheduleId = xn["TAXSCHID"].InnerText,
                                                            ContactPerson = xn["CNTCPRSN"].InnerText,
                                                            Address1 = xn["ADDRESS1"].InnerText,
                                                            Address2 = xn["ADDRESS2"].InnerText,
                                                            Address3 = xn["ADDRESS3"].InnerText,
                                                            Country = xn["COUNTRY"].InnerText,
                                                            City = xn["CITY"].InnerText,
                                                            State = xn["STATE"].InnerText,
                                                            ZipCode = xn["ZIP"].InnerText,
                                                            PhoneNumber1 = xn["PHONE1"].InnerText,
                                                            PhoneNumber2 = xn["PHONE2"].InnerText,
                                                            Fax = xn["FAX"].InnerText,
                                                            UpdateIfExists = 1
                                                        });
            return customerAddressList;
        }

        /// <summary>
        /// Getting customer account details
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="customerNumber"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public NewCustomer GetCustomerAccounts(string connString, string customerNumber, NewCustomer customer)
        {
            var customerAccounts = GetAccountValue(connString, customerNumber);
            var customerAccount = customerAccounts.AccountReceivable + "," + customerAccounts.CashAccount + "," +
                                  customerAccounts.CostofSalesAccount + "," + customerAccounts.FinanceChargesAccount +
                                  "," + customerAccounts.InventoryAccount + "," + customerAccounts.SalesAccount +
                                  "," +
                                  customerAccounts.SalesOrderReturnAccount + "," +
                                  customerAccounts.TermDiscountTackenAccount + "," +
                                  customerAccounts.TermDiscountAvailableAccount + "," +
                                  customerAccounts.WriteOffAccount;
            var customerAccDetails = GetCustomerAccountDetails(connString, customerAccount);
            var customerDoc = new XmlDocument();
            customerDoc.LoadXml(customerAccDetails);
            XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/GL_Accounts");
            if (xmlList != null)
                foreach (XmlNode xn in xmlList)
                {
                    //if (xn.ParentNode.Attributes["ACTINDX"].Value=="6")
                    customer.DefaultCashAccount = 0;
                    if (xn.ParentNode != null)
                    {
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.CashAccount)
                            customer.CashAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.AccountReceivable)
                            customer.ReceivableAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.SalesAccount)
                            customer.SalesAccnt = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.CostofSalesAccount)
                            customer.CostofSalesAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.InventoryAccount)
                            customer.InventoryAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.TermDiscountTackenAccount)
                            customer.TermsDicountTakeAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.TermDiscountAvailableAccount)
                            customer.TermsDicountAvailAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.FinanceChargesAccount)
                            customer.FinanceChargesAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.WriteOffAccount)
                            customer.WriteOffsAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.SalesOrderReturnAccount)
                            customer.SalesOrderRetAcct = xn["ACTNUMST"].InnerText;
                        if (xn.ParentNode.Attributes["ACTINDX"].Value == customerAccounts.OverPaymentWriteOffAccount)
                            customer.OverPaymentWriteOffsAcct = xn["ACTNUMST"].InnerText;
                    }
                }
            return customer;
        }

        /// <summary>
        /// Method to create new customer to GP
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="connString"></param>
        /// <param name="customer"></param>
        /// <param name="customerAddress"></param>
        /// <param name="customerNumber"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public bool SerilizeCustomerObject(string filename, string connString, NewCustomer customer, List<CustomerAddress> customerAddress, string customerNumber, string customerName)
        {
            bool status;
            var serializer = new XmlSerializer(typeof(eConnectType));
            var eConnect = new eConnectType();
            var customerMasterType = new RMCustomerMasterType();
            taUpdateCreateCustomerRcd createCustomerRcd = GetCustomerDetails(customer, customerNumber, customerName);
            taCreateCustomerAddress_ItemsTaCreateCustomerAddress[] createCustomerAddress = GetCustomerAddress(customerAddress);
            customerMasterType.taUpdateCreateCustomerRcd = createCustomerRcd;
            customerMasterType.taCreateCustomerAddress_Items = createCustomerAddress;
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
                status = eConCall.eConnect_EntryPoint(connString, EnumTypes.ConnectionStringType.SqlClient, customerDocument, EnumTypes.SchemaValidationType.None);

                //version 11
                //status = eConCall.CreateEntity(connectionString, customerDocument);
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

        /// <summary>
        /// Method to checks whether the customer is exists in GP or not with customer number
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        public bool CheckCustomer(string connString, string customerNumber)
        {
            using (var sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                var custNumber = "SELECT CUSTNMBR FROM RM00101 WHERE CUSTNMBR ='" + customerNumber + "'";
                var command = new SqlCommand(custNumber, sqlConn);
                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// Gets the customer header.
        /// </summary>
        /// <param name="customerNo"> </param>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="subTotal">The sub total.</param>
        /// <param name="connString"> </param>
        /// <returns></returns>
        public OrderProcess GetOrderProcess(string connString, string customerNo, string orderNumber, decimal subTotal)
        {
            // getting customer info
            string cutomerInfo = new EConnectModel().GetCustomerDetails(connString, customerNo);
            var customerDoc = new XmlDocument();
            if (cutomerInfo != null) customerDoc.LoadXml(cutomerInfo);
            var nodeList = customerDoc.SelectNodes("root/eConnect/Customer");
            if (nodeList != null)
            {
                var orderProcess = new OrderProcess
                                       {
                                           SOPNUMBE = orderNumber,
                                           SOPTYPE = 2,
                                           BACHNUMB = "SHAREATHON  ",
                                           DOCID = "STDORD",
                                           CUSTNMBR = customerNo,
                                           CUSTNAME =
                                                (nodeList[0]["CUSTNMBR"] != null)
                                                   ? nodeList[0]["CUSTNMBR"].InnerText
                                                   : string.Empty,
                                           SUBTOTAL = subTotal,
                                           DOCDATE = DateTime.Now.ToShortDateString(),
                                           ORDRDATE = DateTime.Now.ToShortDateString(),
                                           ShipToName = "",
                                           ADDRESS1 =
                                                (nodeList[0]["ADDRESS1"] != null)
                                                   ? nodeList[0]["ADDRESS1"].InnerText
                                                   : string.Empty,
                                           ADDRESS2 =
                                                (nodeList[0]["ADDRESS2"] != null)
                                                   ? nodeList[0]["ADDRESS2"].InnerText
                                                   : string.Empty,
                                           CITY =
                                                (nodeList[0]["CITY"] != null)
                                                   ? nodeList[0]["CITY"].InnerText
                                                   : string.Empty,
                                           STATE =
                                               (nodeList[0]["STATE"] != null)
                                                   ? nodeList[0]["STATE"].InnerText
                                                   : string.Empty,
                                           ZIPCODE =
                                               (nodeList[0]["ZIP"] != null)
                                                   ? nodeList[0]["ZIP"].InnerText
                                                   : string.Empty,
                                           COUNTRY =
                                               (nodeList[0]["COUNTRY"] != null)
                                                   ? nodeList[0]["COUNTRY"].InnerText
                                                   : string.Empty,
                                           PHNUMBR1 =
                                               (nodeList[0]["PHONE1"] != null)
                                                   ? nodeList[0]["PHONE1"].InnerText
                                                   : string.Empty,
                                           FREIGHT = 0,
                                           FRTTXAMT = 0,
                                           MISCAMNT = 0,
                                           MSCTXAMT = 0,
                                           TRDISAMT = 0,
                                           TAXAMNT = 0,
                                       };
                orderProcess.DOCAMNT =
                    Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT +
                                      orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) -
                    Convert.ToDecimal(orderProcess.TRDISAMT);
                return orderProcess;
            }
            return null;
        }

        /// <summary>
        /// Gets the ordered items.
        /// </summary>
        /// <param name="connString"> </param>
        /// <param name="donation"> </param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        public List<OrderItems> GetOrderedItems(string connString, ShareAThonDonation donation, string orderNumber)
        {
            var outPut = new List<OrderItems>();

            string cutomerInfo = new EConnectModel().GetCustomerDetails(connString, donation.CustomerId);
            var customerDoc = new XmlDocument();
            if (cutomerInfo != null) customerDoc.LoadXml(cutomerInfo);
            var nodeList = customerDoc.SelectNodes("root/eConnect/Customer");
            if (nodeList != null)
            {
                foreach (ShareAThonOfferLine shareAThonOfferLine in donation.ShareAThonOfferLines)
                {
                    outPut.Add(new OrderItems
                                   {
                                       SOPTYPE = 2,
                                       SOPNUMBE = orderNumber,
                                       CUSTNMBR =
                                           (nodeList[0]["CUSTNMBR"] != null)
                                               ? nodeList[0]["CUSTNMBR"].InnerText
                                               : string.Empty,
                                       DOCDATE = DateTime.Now.ToShortDateString(),
                                       ITEMNMBR = shareAThonOfferLine.OfferNo,
                                       ITEMDESC = shareAThonOfferLine.Description,
                                       UNITPRCE = GetUnitPrice(shareAThonOfferLine.OfferNo, connString),
                                       QUANTITY = shareAThonOfferLine.Qty,
                                       XTNDPRCE = shareAThonOfferLine.Qty * GetUnitPrice(shareAThonOfferLine.OfferNo, connString),
                                       TOTALQTY = 0,
                                       CURNCYID = "",
                                       UOFM = "",
                                       NONINVEN = 0,
                                       ShipToName = "",
                                       ADDRESS1 =
                                           (nodeList[0]["ADDRESS1"] != null)
                                               ? nodeList[0]["ADDRESS1"].InnerText
                                               : string.Empty,
                                       ADDRESS2 =
                                           (nodeList[0]["ADDRESS2"] != null)
                                               ? nodeList[0]["ADDRESS2"].InnerText
                                               : string.Empty,
                                       CITY =
                                           (nodeList[0]["CITY"] != null) ? nodeList[0]["CITY"].InnerText : string.Empty,
                                       STATE =
                                           (nodeList[0]["STATE"] != null)
                                               ? nodeList[0]["STATE"].InnerText
                                               : string.Empty,
                                       ZIPCODE =
                                           (nodeList[0]["ZIP"] != null) ? nodeList[0]["ZIP"].InnerText : string.Empty,
                                       COUNTRY =
                                           (nodeList[0]["COUNTRY"] != null)
                                               ? nodeList[0]["COUNTRY"].InnerText
                                               : string.Empty,
                                       PHNUMBR1 =
                                           (nodeList[0]["PHONE1"] != null)
                                               ? nodeList[0]["PHONE1"].InnerText
                                               : string.Empty,
                                   }
                        );
                }
            }
            return outPut;
        }

        #endregion Public Methods

        #region Private Methods

        // Customer Creation process methods
        /// <summary>
        /// Getting customer account details XML from GP
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        private string GetCustomerAccountDetails(string connString, string customerAccount)
        {
            var econ = new eConnectMethods();
            var myRequest = new eConnectOut
            {
                DOCTYPE = "GL_Accounts",
                OUTPUTTYPE = 2,
                FORLIST = 1,
                WhereClause = "ACTINDX in(" + customerAccount + ")"
            };
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
            // var customerDoc = econ.GetEntity(connString, xmlCustomerdoc.OuterXml);

            // version 10
            var customerDoc = econ.eConnect_Requester(connString, EnumTypes.ConnectionStringType.SqlClient, xmlCustomerdoc.OuterXml);
            return customerDoc;
        }

        /// <summary>
        /// Method to get taUpdateCreateCustomerRcd for customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerName"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        private taUpdateCreateCustomerRcd GetCustomerDetails(NewCustomer customer, string customerName, string customerNumber)
        {
            var taCreateCustomerRcd = new taUpdateCreateCustomerRcd
            {
                CUSTNMBR = customerNumber,
                CUSTNAME = customerName,
                HOLD = customer.Hold,
                INACTIVE = customer.InActive,
                SHRTNAME = customer.CustShortName,
                STMTNAME = customer.StatementName,
                CUSTCLAS = customer.CustomerClass,
                ADRSCODE = customer.AddressCode,
                CNTCPRSN = customer.ContactPerson,
                ADDRESS1 = customer.Address1,
                ADDRESS2 = customer.Address2,
                CITY = customer.City,
                STATE = customer.State,
                ZIPCODE = customer.ZipCode,
                COUNTRY = customer.Country,
                CCode = customer.CountryCode,
                PHNUMBR1 = customer.PhoneNumber1,
                PHNUMBR2 = customer.PhoneNumber2,
                FAX = customer.Fax,
                UPSZONE = customer.UpsZone,
                SHIPMTHD = customer.ShippingMethod,
                TAXSCHID = customer.TaxScheduleId,
                SHIPCOMPLETE = customer.ShipComplete,
                PRSTADCD = customer.PrimaryShipToAddressCode,
                PRBTADCD = customer.PrimaryBillToAddressCode,
                STADDRCD = customer.StatementToAddressCode,
                SLPRSNID = customer.SalesPersonId,
                SALSTERR = customer.SalesTerritorry,
                USERDEF1 = customer.UserDefined1,
                USERDEF2 = customer.UserDefined2,
                COMMENT1 = customer.Comment1,
                COMMENT2 = customer.Comment2,
                CUSTDISC = customer.TradeDiscount,
                PYMTRMID = customer.PaymentTermId,
                DISGRPER = customer.DiscountGracePeriod,
                DUEGRPER = customer.DuedateGracePeriod,
                PRCLEVEL = customer.PriceLevel,
                NOTETEXT = customer.NoteText,
                BALNCTYP = customer.BalanceType,
                FNCHATYP = customer.FinanceChargeType,
                FNCHPCNT = customer.FinanceChargePercent,
                FINCHDLR = customer.FinanceChargeDollar,
                MINPYTYP = customer.MinPaymentType,
                MINPYPCT = customer.MinPymntPercent,
                MINPYDLR = customer.MinPymntDollarAmt,
                CRLMTTYP = customer.CreditLimitType,
                CRLMTAMT = customer.CreditLimitAmt,
                CRLMTPER = customer.CreditLimitPeriod,
                CRLMTPAM = customer.CreditLimitPeriodAmat,
                MXWOFTYP = customer.MaximumWriteOffType,
                MXWROFAM = customer.MaximumWriteOffAmt,
                Revalue_Customer = customer.RevalueCustomer,
                Post_Results_To = customer.PostResultTo,
                CRCARDID = customer.CreditCardId,
                CRCRDNUM = customer.CreditCardNumber,
                CCRDXPDT = Convert.ToDateTime(customer.CreditExpiryDate).ToShortDateString(),
                BANKNAME = customer.BankName,
                BNKBRNCH = customer.BankBranch,
                TAXEXMT1 = customer.TaxExcempt1,
                TAXEXMT2 = customer.TaxExcempt2,
                TXRGNNUM = customer.TaxRegNumber,
                CURNCYID = customer.CurrencyId,
                RATETPID = customer.RateTypeId,
                STMTCYCL = customer.StatementCycleId,
                KPCALHST = customer.MaintainHistoryCalendarYear,
                KPERHIST = customer.MaintainHistoryFiscalYear,
                KPTRXHST = customer.MaintainHistoryTrans,
                KPDSTHST = customer.MaintainHistoryDist,
                Send_Email_Statements = customer.SendEmailStatment,
                ToEmail_Recipient = customer.ToReceipient,
                CcEmail_Recipient = customer.CcReceiptient,
                BccEmail_Recipient = customer.BcCReceipient,
                CHEKBKID = customer.CheckBookId,
                DEFCACTY = customer.DefaultCashAccount,
                RMCSHACTNUMST = customer.CashAcct,
                RMARACTNUMST = customer.ReceivableAcct,
                RMSLSACTNUMST = customer.SalesAccnt,
                RMCOSACTNUMST = customer.CostofSalesAcct,
                RMIVACTNUMST = customer.InventoryAcct,
                RMTAKACTNUMST = customer.TermsDicountTakeAcct,
                RMAVACTNUMST = customer.TermsDicountAvailAcct,
                RMFCGACTNUMST = customer.FinanceChargesAcct,
                RMWRACTNUMST = customer.WriteOffsAcct,
                RMSORACTNUMST = customer.SalesOrderRetAcct,
                RMOvrpymtWrtoffACTNUMST = customer.OverPaymentWriteOffsAcct,
                UseCustomerClass = 0,
                CreateAddress = 1,
                UpdateIfExists = 0
            };
            return taCreateCustomerRcd;
        }

        /// <summary>
        /// Method to get list of taCreateCustomerAddress_ItemsTaCreateCustomerAddress for a customer
        /// </summary>
        /// <param name="customerAddress"></param>
        /// <returns></returns>
        private taCreateCustomerAddress_ItemsTaCreateCustomerAddress[] GetCustomerAddress(List<CustomerAddress> customerAddress)
        {
            var addressCount = customerAddress.Count;
            var custAddress = new List<taCreateCustomerAddress_ItemsTaCreateCustomerAddress>();
            try
            {
                for (int i = 0; i < addressCount; i++)
                {
                    var taCreateCustomerAddress = new taCreateCustomerAddress_ItemsTaCreateCustomerAddress
                    {
                        CUSTNMBR = customerAddress[i].CustomerNumber,
                        ADRSCODE = customerAddress[i].AddressCode,
                        SLPRSNID = customerAddress[i].SalesPersonId,
                        UPSZONE = customerAddress[i].UpsZone,
                        SHIPMTHD = customerAddress[i].ShippingMethod,
                        TAXSCHID = customerAddress[i].TaxScheduleId,
                        CNTCPRSN = customerAddress[i].ContactPerson,
                        ADDRESS1 = customerAddress[i].Address1,
                        ADDRESS2 = customerAddress[i].Address2,
                        CITY = customerAddress[i].City,
                        STATE = customerAddress[i].State,
                        ZIPCODE = customerAddress[i].ZipCode,
                        COUNTRY = customerAddress[i].Country,
                        CCode = customerAddress[i].CountryCode,
                        PHNUMBR1 = customerAddress[i].PhoneNumber1,
                        PHNUMBR2 = customerAddress[i].PhoneNumber2,
                        FAX = customerAddress[i].Fax,
                        LOCNCODE = customerAddress[i].LocationCode,
                        UpdateIfExists = 1
                    };
                    custAddress.Add(taCreateCustomerAddress);
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
            return custAddress.ToArray();
        }

        /// <summary>
        /// Method to get list of taCreateSopPaymentInsertRecord_ItemsTaCreateSopPaymentInsertRecord for a customer
        /// </summary>
        /// <param name="sopOrderPayment"></param>
        /// <returns></returns>
        private taCreateSopPaymentInsertRecord_ItemsTaCreateSopPaymentInsertRecord[] GetSopPaymentInsertRecord(SopOrderPayment sopOrderPayment)
        {
            var sopPaymentInsertRecordList =
                new List<taCreateSopPaymentInsertRecord_ItemsTaCreateSopPaymentInsertRecord>();
            var sopPaymentInsertRecord =
                new taCreateSopPaymentInsertRecord_ItemsTaCreateSopPaymentInsertRecord
                {
                    SOPTYPE = sopOrderPayment.SOPTYPE,
                    SOPNUMBE = sopOrderPayment.SOPNUMBE,
                    CUSTNMBR = sopOrderPayment.CUSTNMBR,
                    DOCDATE = sopOrderPayment.DOCDATE,
                    DOCAMNT = sopOrderPayment.DOCAMNT,
                    CHEKBKID = sopOrderPayment.CHEKBKID,
                    CARDNAME = sopOrderPayment.CARDNAME,
                    CHEKNMBR = sopOrderPayment.CHEKNMBR,
                    RCTNCCRD = sopOrderPayment.RCTNCCRD,
                    AUTHCODE = sopOrderPayment.AUTHCODE,
                    EXPNDATE = sopOrderPayment.EXPNDATE,
                    PYMTTYPE = sopOrderPayment.PYMTTYPE,
                    DOCNUMBR = sopOrderPayment.DOCNUMBR,
                    Action = 1,
                    SEQNUMBR = 0,
                    SEQNUMBRSpecified = true,
                    ActionSpecified = true,
                    MDFUSRID = sopOrderPayment.MDFUSRID
                };
            sopPaymentInsertRecordList.Add(sopPaymentInsertRecord);

            return sopPaymentInsertRecordList.ToArray();
        }

        /// <summary>
        /// To get next payment number
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        private string GetPaymentNumber(string connString)
        {
            var getNextDocNumber = new GetNextDocNumbers();
            return getNextDocNumber.GetNextRMNumber(GetNextDocNumbers.IncrementDecrement.Increment, GetNextDocNumbers.RMPaymentType.RMPayments, connString);
        }

        /// <summary>
        /// Method to get SOP Order Payment
        /// </summary>
        /// <param name="orderProcess"></param>
        /// <param name="connString"></param>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        private SopOrderPayment StoreSopOrderPayment(OrderProcess orderProcess, string connString, CardDetails cardDetails)
        {
            var sopOrderPayment = new SopOrderPayment
            {
                SOPTYPE = 2,
                SOPNUMBE = orderProcess.SOPNUMBE,
                CUSTNMBR = orderProcess.CUSTNMBR,
                DOCDATE = DateTime.Now.ToShortDateString(),
                DOCAMNT = orderProcess.DOCAMNT,
                CHEKBKID = "",
                CARDNAME = cardDetails.CardName,
                CHEKNMBR = "",
                RCTNCCRD = cardDetails.CardNo,
                AUTHCODE = cardDetails.AuthorizeCode,
                EXPNDATE = cardDetails.ExpireDate.ToString(CultureInfo.InvariantCulture),
                PYMTTYPE = 3,
                DOCNUMBR = GetPaymentNumber(connString),
                Action = 1, //default form setup
                SEQNUMBR = 0, //default form setup
            };
            return sopOrderPayment;
        }

        /// <summary>
        /// Method to get Customer Account
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        private CustomerAccounts GetAccountValue(string connectionString, string customerNumber)
        {
            var customerAccounts = new CustomerAccounts();
            using (var sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                var command = new SqlCommand("select DEFCACTY,RMCSHACC,RMARACC,RMSLSACC,RMIVACC,RMCOSACC,RMTAKACC,RMAVACC,RMFCGACC,RMWRACC,RMSORACC from RM00101 where CUSTNMBR='" + customerNumber + "'", sqlConn);
                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        customerAccounts.CashAccount = reader["RMCSHACC"].ToString();
                        customerAccounts.AccountReceivable = reader["RMARACC"].ToString();
                        customerAccounts.SalesAccount = reader["RMSLSACC"].ToString();
                        customerAccounts.CostofSalesAccount = reader["RMCOSACC"].ToString();
                        customerAccounts.InventoryAccount = reader["RMIVACC"].ToString();
                        customerAccounts.TermDiscountTackenAccount = reader["RMTAKACC"].ToString();
                        customerAccounts.TermDiscountAvailableAccount = reader["RMAVACC"].ToString();
                        customerAccounts.FinanceChargesAccount = reader["RMFCGACC"].ToString();
                        customerAccounts.WriteOffAccount = reader["RMWRACC"].ToString();
                        customerAccounts.SalesOrderReturnAccount = reader["RMSORACC"].ToString();
                    }
                }
            }
            return customerAccounts;
        }

        /// <summary>
        /// Method to get taUpdateCreateCustomerRcd for customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private taUpdateCreateCustomerRcd GetCustomerDetails(CustomerDetails customer)
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

        /// <summary>
        /// Method to get taSopHdrIvcInsert
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private taSopHdrIvcInsert GetHeaderItems(OrderProcess order)
        {
            return new taSopHdrIvcInsert
                               {
                                   SOPTYPE = order.SOPTYPE,
                                   SOPNUMBE = order.SOPNUMBE,
                                   BACHNUMB = order.BACHNUMB,
                                   DOCID = order.DOCID,
                                   CUSTNMBR = order.CUSTNMBR,
                                   CUSTNAME = order.CUSTNAME,
                                   FREIGHT = order.FREIGHT,
                                   FRTTXAMT = order.FRTTXAMT,
                                   SUBTOTAL = order.SUBTOTAL,
                                   DOCAMNT = order.DOCAMNT,
                                   DOCDATE = order.DOCDATE,
                                   ORDRDATE = order.ORDRDATE,
                                   ShipToName = order.ShipToName,
                                   ADDRESS1 = order.ADDRESS1,
                                   ADDRESS2 = order.ADDRESS2,
                                   ADDRESS3 = order.ADDRESS3,
                                   CITY = order.CITY,
                                   STATE = order.STATE,
                                   ZIPCODE = order.ZIPCODE,
                                   COUNTRY = order.COUNTRY,
                                   PHNUMBR1 = order.PHNUMBR1,
                                   MISCAMNT = order.MISCAMNT,
                                   MSCTXAMT = order.MSCTXAMT,
                                   TAXAMNT = order.TAXAMNT,
                                   TRDISAMT = order.TRDISAMT,
                                   PYMTRCVD = order.DOCAMNT
                               };
        }

        /// <summary>
        /// Method to get list of taSopLineIvcInsert_ItemsTaSopLineIvcInsert
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] GetLineItems(List<OrderItems> order)
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

        /// <summary>
        /// Gets the unit price.
        /// </summary>
        /// <param name="offerNo">The offer no.</param>
        /// <param name="connString"> </param>
        /// <returns></returns>
        private decimal GetUnitPrice(string offerNo, string connString)
        {
            decimal unitPrice = 0;
            var sourceOfferLines = new OfferLines().GetOfferLines(connString);

            foreach (var sourceOfferLine in sourceOfferLines.Where(sourceOfferLine => sourceOfferLine.OfferId.Equals(offerNo)))
            {
                unitPrice = sourceOfferLine.Price;
            }

            return unitPrice;
        }

        #endregion Private Methods

        #region Un-Used Code

        //public taCreateInternetAddresses_ItemsTaCreateInternetAddresses[] GetCustomerInetInfo(List<CustomerInetInfo> customerInetInfo)
        //{
        //    var addressCount = customerInetInfo.Count;
        //    var custInternetAddress = new List<taCreateInternetAddresses_ItemsTaCreateInternetAddresses>();
        //    for (int i = 0; i < addressCount; i++)
        //    {
        //        var taCreateCustomerInetaddr = new taCreateInternetAddresses_ItemsTaCreateInternetAddresses
        //                                           {
        //                                               Master_ID = customerInetInfo[i].MasterId,
        //                                               Master_Type = customerInetInfo[i].MasterType,
        //                                               ADRSCODE = customerInetInfo[i].AddressCode,
        //                                               INET1 = customerInetInfo[i].IntenetInfo1,
        //                                               INET2 = customerInetInfo[i].IntenetInfo2,
        //                                               INET3 = customerInetInfo[i].IntenetInfo3,
        //                                               INET4 = customerInetInfo[i].IntenetInfo4,
        //                                               EmailToAddress = customerInetInfo[i].EmailToAddress,
        //                                               EmailBccAddress = customerInetInfo[i].EmailBccAddress,
        //                                               EmailCcAddress = customerInetInfo[i].EmailCcAddress
        //                                           };
        //        custInternetAddress.Add(taCreateCustomerInetaddr);
        //    }
        //    return custInternetAddress.ToArray();
        //}

        //public bool SerializeSalesOrderObject(string filename, string sConnectionString, OrderProcess order, List<OrderItems> orderItems)
        //{
        //    bool status;
        //    var serializer = new XmlSerializer(typeof(eConnectType));
        //    var eConnect = new eConnectType();
        //    var sopTrnType = new SOPTransactionType();
        //    taSopHdrIvcInsert sopHdrInvInsert = GetHeaderItems(order);
        //    taSopLineIvcInsert_ItemsTaSopLineIvcInsert[] sopLineInvInsert = GetLineItems(orderItems);
        //    sopTrnType.taSopLineIvcInsert_Items = sopLineInvInsert;
        //    sopTrnType.taSopHdrIvcInsert = sopHdrInvInsert;
        //    Array.Resize(ref eConnect.SOPTransactionType, 1);
        //    eConnect.SOPTransactionType[0] = sopTrnType;
        //    var fs = new FileStream(filename, FileMode.Create);
        //    var writer = new XmlTextWriter(fs, new UTF8Encoding());
        //    var eConCall = new eConnectMethods();
        //    var xmldoc = new XmlDocument();
        //    serializer.Serialize(writer, eConnect);
        //    writer.Close();
        //    xmldoc.Load(filename);
        //    string sopTransactionDoc = xmldoc.OuterXml;
        //    try
        //    {
        //        //version 10
        //        status = eConCall.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient, sopTransactionDoc, EnumTypes.SchemaValidationType.None);

        //        //version 11
        //        //status = eConCall.CreateEntity(sConnectionString, sopTransactionDoc);
        //    }
        //    catch (eConnectException exp)
        //    {
        //        throw new Exception(exp.ToString());
        //    }
        //    finally
        //    {
        //        eConCall.Dispose();
        //    }
        //    return status;
        //}

        //public NewCustomer GetCustomerInformation(string connString, string customerNumber)
        //{
        //    var customer = new NewCustomer();
        //    var customerDetails = GetCustomerDetails(connString, customerNumber);
        //    var customerDoc = new XmlDocument();
        //    customerDoc.LoadXml(customerDetails);
        //    XmlNodeList xmlList = customerDoc.SelectNodes("root/eConnect/Customer");
        //    if (xmlList != null)
        //        foreach (XmlNode xn in xmlList)
        //        {
        //            customer.CustomerNumber = customerNumber;
        //            customer.CustomerName = "Aaron Fitz Electrical";
        //            customer.Hold = short.Parse(xn["HOLD"].InnerText);
        //            customer.InActive = short.Parse(xn["INACTIVE"].InnerText);
        //            customer.CustShortName = xn["SHRTNAME"].InnerText;
        //            customer.StatementName = xn["STMTNAME"].InnerText;
        //            customer.CustomerClass = xn["CUSTCLAS"].InnerText;
        //            customer.AddressCode = xn["ADRSCODE"].InnerText;
        //            customer.ContactPerson = xn["CNTCPRSN"].InnerText;
        //            customer.Address1 = xn["ADDRESS1"].InnerText;
        //            customer.Address2 = xn["ADDRESS1"].InnerText;
        //            customer.City = xn["CITY"].InnerText;
        //            customer.State = xn["STATE"].InnerText;
        //            customer.ZipCode = xn["ZIP"].InnerText;
        //            customer.Country = xn["COUNTRY"].InnerText;
        //            customer.PhoneNumber1 = xn["PHONE1"].InnerText;
        //            customer.PhoneNumber2 = xn["PHONE2"].InnerText;
        //            customer.Fax = xn["FAX"].InnerText;
        //            customer.UpsZone = xn["UPSZONE"].InnerText;
        //            customer.ShippingMethod = xn["SHIPMTHD"].InnerText;
        //            customer.TaxScheduleId = xn["TAXSCHID"].InnerText;
        //            customer.PrimaryShipToAddressCode = xn["PRSTADCD"].InnerText;
        //            customer.PrimaryBillToAddressCode = xn["PRBTADCD"].InnerText;
        //            customer.StatementToAddressCode = xn["STADDRCD"].InnerText;
        //            customer.SalesPersonId = xn["SLPRSNID"].InnerText;
        //            customer.SalesTerritorry = xn["SALSTERR"].InnerText;
        //            customer.UserDefined1 = xn["USERDEF1"].InnerText;
        //            customer.UserDefined2 = xn["USERDEF2"].InnerText;
        //            customer.TradeDiscount = decimal.Parse(xn["CUSTDISC"].InnerText);
        //            customer.PaymentTermId = xn["PYMTRMID"].InnerText;
        //            customer.DiscountGracePeriod = short.Parse(xn["DISGRPER"].InnerText);
        //            customer.DuedateGracePeriod = short.Parse(xn["DUEGRPER"].InnerText);
        //            customer.PriceLevel = xn["PRCLEVEL"].InnerText;
        //            customer.BalanceType = short.Parse(xn["BALNCTYP"].InnerText);
        //            customer.FinanceChargeType = short.Parse(xn["FNCHATYP"].InnerText);
        //            customer.FinanceChargePercent = int.Parse(xn["FNCHPCNT"].InnerText);
        //            customer.MinPaymentType = short.Parse(xn["MINPYTYP"].InnerText);
        //            customer.MinPymntPercent = short.Parse(xn["MINPYPCT"].InnerText);
        //            customer.MinPymntDollarAmt = decimal.Parse(xn["MINPYDLR"].InnerText);
        //            customer.CreditLimitType = short.Parse(xn["CRLMTTYP"].InnerText);
        //            customer.CreditLimitAmt = decimal.Parse(xn["CRLMTAMT"].InnerText);
        //            customer.CreditLimitPeriod = short.Parse(xn["CRLMTPER"].InnerText);
        //            customer.CreditLimitPeriodAmat = Decimal.Parse(xn["CRLMTPAM"].InnerText);
        //            customer.MaximumWriteOffType = short.Parse(xn["MXWOFTYP"].InnerText);
        //            customer.MaximumWriteOffAmt = decimal.Parse(xn["MXWROFAM"].InnerText);
        //            customer.RevalueCustomer = short.Parse(xn["Revalue_Customer"].InnerText);
        //            customer.PostResultTo = short.Parse(xn["Post_Results_To"].InnerText);
        //            customer.CreditCardId = xn["CRCARDID"].InnerText;
        //            customer.CreditCardNumber = xn["CRCRDNUM"].InnerText;
        //            customer.CreditExpiryDate = xn["CCRDXPDT"].InnerText; //DateTime.Parse(xn["CCRDXPDT"].InnerText);
        //            customer.BankName = xn["BANKNAME"].InnerText;
        //            customer.BankBranch = xn["BNKBRNCH"].InnerText;
        //            customer.TaxExcempt1 = xn["TAXEXMT1"].InnerText;
        //            customer.TaxExcempt2 = xn["TAXEXMT2"].InnerText;
        //            customer.TaxRegNumber = xn["TXRGNNUM"].InnerText;
        //            customer.CurrencyId = xn["CURNCYID"].InnerText;
        //            customer.RateTypeId = xn["RATETPID"].InnerText;
        //            customer.StatementCycleId = short.Parse(xn["STMTCYCL"].InnerText);
        //            customer.MaintainHistoryCalendarYear = short.Parse(xn["KPCALHST"].InnerText);
        //            customer.MaintainHistoryFiscalYear = short.Parse(xn["KPERHIST"].InnerText);
        //            customer.MaintainHistoryTrans = short.Parse(xn["KPTRXHST"].InnerText);
        //            customer.MaintainHistoryDist = short.Parse(xn["KPDSTHST"].InnerText);
        //            customer.SendEmailStatment = short.Parse(xn["Send_Email_Statements"].InnerText);
        //            customer.CheckBookId = xn["CHEKBKID"].InnerText;
        //            customer.FrontOfficeItegrationId = string.Empty;
        //            customer.IntegrationSource = 0;
        //            customer.IntegrationId = string.Empty;
        //            customer.UseCustomerClass = 0;
        //            customer.CreateAddressCode = 1;
        //            customer.UpdateIfExists = 2;
        //        }
        //    return customer;
        //}

        #endregion Un-Used Code
    }
}