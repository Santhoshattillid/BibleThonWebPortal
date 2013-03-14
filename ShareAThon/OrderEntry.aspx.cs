using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using TWOModel;
using Telerik.Web.UI;
using System.Collections;
using Biblethon.Controller;

namespace ShareAThon
{
    public partial class ShareAThonOrderEntry : System.Web.UI.Page
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hdOrderId.Value = new EConnectModel().GetNextSalseDocNumber(_connString).Trim();
                //TxtOrderNumber.Text = "####";
                txtInitialCharge.Text = DateTime.Now.ToShortDateString();
                TxtCCOrderDate.Text = DateTime.Now.ToShortDateString();
                LoadChargeMonthly();
                LoadRadGridData();
            }
            else
            {
                UpdateSessionVariables();
            }
        }

        protected void OrderEntrySave(object sender, EventArgs e)
        {
            try
            {
                var econnectModel = new EConnectModel();

                // validation for billing address elements
                if (string.IsNullOrEmpty(txtCustName.Text.Trim()))
                {
                    lblError.Text = "Customer name could not be empty";
                    hidAccordionIndex.Value = "0";
                    return;
                }

                // validation for credit card information tab
                if (drpMethdDonation.SelectedValue.Equals("Credit Card"))
                {

                    if (string.IsNullOrEmpty(txtCreditCardNo.Text.Trim()))
                    {
                        lblError.Text = "Credit card number could not be empty";
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (string.IsNullOrEmpty(txtExpirationDate.Text.Trim()))
                    {
                        lblError.Text = "Credit card  expire date could not be empty";
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (string.IsNullOrEmpty(txtCVN.Text.Trim()))
                    {
                        lblError.Text = "CVN could not be empty";
                        hidAccordionIndex.Value = "3";
                        return;
                    }
                }

                // Generating order number
                var orderNo = new EConnectModel().GetNextSalseDocNumber(_connString).Trim();
                //TxtOrderNumber.Text = orderNo;

                // mapping data to ShareAThonDonation class
                var donation = new TWOModel.ShareAThonDonation
                                   {
                                       OrderId = orderNo,
                                       CustomerId = HdnCustomerNo.Value,
                                       CurrentlyDonorOf = decimal.Parse(txtCurDonor.Text),
                                       InitialChargeOn = DateTime.Parse(txtInitialCharge.Text),
                                       DonationAmount = decimal.Parse(txtDonationAmt.Text),
                                       IncreasingTo = decimal.Parse(txtIncreasing.Text),
                                       DayToChargeMonthly = int.Parse(drpchargeMontly.SelectedValue)
                                   };

                // mapping collection of data to ShareAThonDonationFrequency class
                var frequencies = new List<TWOModel.ShareAThonDonationFrequency>();
                var frequencyname = new TWOModel.ShareAThonDonationFrequency();

                if (drpFrequency.SelectedValue.Equals("One Time") || drpFrequency.SelectedValue.Equals("Other"))
                {
                    frequencyname.OrderId = orderNo;
                    frequencyname.DueDate = DateTime.Now;
                    frequencyname.Amount = decimal.Parse(txtDonationAmt.Text);
                    frequencyname.Status = "Process";
                    frequencies.Add(frequencyname);
                }
                else
                {

                    for (int i = 0; i <= int.Parse(drpMonths.SelectedValue); i++)
                    {
                        var frequency = new TWOModel.ShareAThonDonationFrequency
                                            {
                                                OrderId = (i.Equals(0)) ? orderNo : "",
                                                DueDate =
                                                    (i.Equals(0)) ? DateTime.Parse(txtInitialCharge.Text) : DateTime.Parse((DateTime.Today.Month + i) + "/" +
                                                                   drpchargeMontly.SelectedValue + "/" + DateTime.Now.Year),
                                                Amount = decimal.Parse(txtDonationAmt.Text),
                                                Status = (i.Equals(0)) ? "Process" : "Pending"
                                            };
                        frequencies.Add(frequency);
                    }
                }

                // mapping collection of data to ShareAThonOfferLine class
                var offerlines = GetOfferedLines(orderNo);

                // saving in local database
                using (var context = new TWOEntities())
                {

                    foreach (var frequency in frequencies)
                    {
                        donation.ShareAThonDonationFrequencies.Add(frequency);

                    }

                    foreach (var offer in offerlines)
                    {
                        donation.ShareAThonOfferLines.Add(offer);
                        //context.AddToShareAThonOfferLines(offer);
                    }

                    context.AddToShareAThonDonations(donation);
                    context.SaveChanges();


                }

                // Saving in GP
                Session["orderNumber"] = orderNo;
                OrderProcess orderProcess = GetCustomerHeader(orderNo, offerlines.Sum(o => o.Qty * GetUnitPrice(o.OfferNo)));
                List<OrderItems> listOrders = GetOrderedItems(orderNo);
                string fileName = Server.MapPath("~/SalesOrder.xml");

                if (new EConnectModel().SerializeSalesOrderObject(fileName, _connString, orderProcess, listOrders))
                {
                    Reset();
                    lblError.Text = "The Order [" + orderNo + "]  has been processed successfully.";
                    lblError.CssClass = "Success";
                }
                else
                {
                    lblError.Text = "Ooops something went wrong in our server. We are not able to process your request.";
                    lblError.CssClass = "error errorinfo";
                    econnectModel.RollbackSalseDocNumber(_connString);
                }

            }
            catch (Exception exception)
            {
                lblError.Text = "Ooops something went wrong in our server. We are not able to process your request." + exception;
            }
        }
        private void Reset()
        {
            LblBName.Text = string.Empty;
            LblBAddress1.Text = string.Empty;
            LblBAddress2.Text = string.Empty;
            LblBCityStateZip.Text = string.Empty;
            LblBCountry.Text = string.Empty;
            LblBTelephone1.Text = string.Empty;
            LblBEmail.Text = string.Empty;

            LblSAName.Text = string.Empty;
            LblSAAddress1.Text = string.Empty;
            LblSAAddress2.Text = string.Empty;
            LblSACityStateZip.Text = string.Empty;
            LblSACountry.Text = string.Empty;
            LblSATelephone1.Text = string.Empty;
            LblSAEmail.Text = string.Empty;


            LblCreditCardNo.Text = string.Empty;
            LblCreditExpired.Text = string.Empty;

            txtCustName.Text = string.Empty;
            lblAddress1.Text = string.Empty;
            lblAddress2.Text = string.Empty;
            lblAddress3.Text = string.Empty;
            lblCity.Text = string.Empty;
            lblState.Text = string.Empty;
            lblCountry.Text = string.Empty;

            txtPhone.Text = string.Empty;
            txtBEmail.Text = string.Empty;
            lblZipCode.Text = string.Empty;

            cbCaller.Checked = false;
            cbMention.Checked = false;



            LoadRadGridData();

            txtCreditCardNo.Text = string.Empty;
            txtExpirationDate.Text = string.Empty;
            txtCVN.Text = string.Empty;

            //TxtCCOrderDate.Text = string.Empty;
            TxtTotalOrder.Text = string.Empty;

            hidAccordionIndex.Value = "0";
        }

        public List<OrderItems> GetOrderedItems(string orderNumber)
        {
            return (from GridDataItem gridDataItem in RadGridOfferLines.Items
                    let itemQty = (TextBox)gridDataItem.FindControl("TxtQuantity")
                    where !string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0
                    select new OrderItems
                    {
                        SOPTYPE = 2,
                        SOPNUMBE = orderNumber,
                        CUSTNMBR = HdnCustomerNo.Value,
                        DOCDATE = DateTime.Now.ToShortDateString(),
                        ITEMNMBR = gridDataItem.Cells[2].Text,
                        ITEMDESC = gridDataItem.Cells[3].Text,
                        UNITPRCE = Convert.ToDecimal(gridDataItem.Cells[5].Text.Replace("$", "0")),
                        QUANTITY = Convert.ToDecimal(itemQty.Text),
                        XTNDPRCE = Convert.ToDecimal(itemQty.Text.Replace("$", "0")) * Convert.ToDecimal(gridDataItem.Cells[5].Text.Replace("$", "0")),
                        TOTALQTY = 0,
                        CURNCYID = "",
                        UOFM = "",
                        NONINVEN = 0,
                        ShipToName = hidAddressCode.Value,
                        ADDRESS1 = lblAddress1.Text,
                        ADDRESS2 = lblAddress2.Text,
                        CITY = lblCity.Text,
                        STATE = lblState.Text,
                        ZIPCODE = lblZipCode.Text,
                        COUNTRY = lblCountry.Text,
                        PHNUMBR1 = txtPhone.Text.Replace("(", "").Replace(")", "").Replace("Ext.", "").Replace(" ", "").Replace("-", "")
                    }).ToList();
        }

        public decimal GetUnitPrice(string offerNo)
        {
            decimal unitPrice = 0;
            var sourceOfferLines = new OfferLines().GetOfferLines(_connString);

            foreach (var sourceOfferLine in sourceOfferLines.Where(sourceOfferLine => sourceOfferLine.OfferId.Equals(offerNo)))
            {
                unitPrice = sourceOfferLine.Price;
            }

            return unitPrice;
        }

        public OrderProcess GetCustomerHeader(string orderNumber, decimal subTotal)
        {
            var orderProcess = new OrderProcess
            {
                SOPTYPE = 2,
                SOPNUMBE = orderNumber,
                BACHNUMB = "SHAREATHON  ",
                DOCID = "STDORD",
                CUSTNMBR = HdnCustomerNo.Value,
                CUSTNAME = txtCustName.Text,
                SUBTOTAL = subTotal,
                DOCDATE = DateTime.Now.ToShortDateString(),
                ORDRDATE = DateTime.Now.ToShortDateString(),
                ShipToName = hidAddressCode.Value,
                ADDRESS1 = lblAddress1.Text,
                ADDRESS2 = lblAddress2.Text,
                CITY = lblCity.Text,
                STATE = lblState.Text,
                ZIPCODE = lblZipCode.Text,
                COUNTRY = lblCountry.Text,
                PHNUMBR1 = txtPhone.Text.Replace("(", "").Replace(")", "").Replace("Ext.", "").Replace(" ", "").Replace("-", ""),
                FREIGHT = 0,
                FRTTXAMT = 0,
                MISCAMNT = 0,
                MSCTXAMT = 0,
                TRDISAMT = 0,
                TAXAMNT = 0,
            };
            orderProcess.DOCAMNT = Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT + orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) - Convert.ToDecimal(orderProcess.TRDISAMT);
            return orderProcess;

        }


        public List<TWOModel.ShareAThonOfferLine> GetOfferedLines(string orderNumber)
        {
            return (from GridDataItem gridDataItem in RadGridOfferLines.Items
                    let itemQty = (TextBox)gridDataItem.FindControl("TxtQuantity")
                    where !string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0
                    select new TWOModel.ShareAThonOfferLine
                               {
                                   OrderId = orderNumber,
                                   OfferNo = gridDataItem.Cells[2].Text,
                                   Description = gridDataItem.Cells[3].Text,
                                   Qty = Convert.ToInt16(itemQty.Text)
                               }).ToList();
        }

        public void RadGridOfferLinesDataBinding(object sender, EventArgs e)
        {
            LoadRadGridData();
        }

        private void LoadRadGridData()
        {
            var offerLines = new OfferLines().GetOfferLines(_connString);
            RadGridOfferLines.DataSource = offerLines;
            RadGridOfferLines.DataBind();
        }
        private void UpdateSessionVariables()
        {
            // keeping the customer name and telephone in session
            Session["CustomerName"] = txtCustName.Text.Trim();
            Session["Telephone"] = txtPhone.Text.Trim();
            //Session["IsShippingAddressIsSame"] = cbShipping.Checked;

            var result = new Hashtable();

            foreach (GridDataItem gridDataItem in RadGridOfferLines.Items)
            {
                var itemQty = (TextBox)gridDataItem.FindControl("TxtQuantity");
                if (!string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0)
                {
                    result.Add(gridDataItem.Cells[2].Text, itemQty.Text);
                }
            }
            Session["userSelectedOfferlines"] = result;
        }
        protected void LoadChargeMonthly()
        {
            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                drpchargeMontly.Items.Add(new ListItem(text: i.ToString(CultureInfo.InvariantCulture), value: i.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}