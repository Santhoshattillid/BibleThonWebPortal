using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlbaDL;
using Biblethon.Controller;
using Telerik.Web.UI;

namespace ShareAThon
{
    public partial class ShareAThonOrderEntry : Page
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Reset();
            else
                UpdateSessionVariables();
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

                if (string.IsNullOrEmpty(HdnCustomerNo.Value))
                {
                    lblError.Text = "Customer id is not found, please select the customer again and then try.";
                    hidAccordionIndex.Value = "0";
                    return;
                }

                //validating donation panel
                if (string.IsNullOrEmpty(drpFrequency.SelectedValue) || string.IsNullOrEmpty(drpMonths.SelectedValue) || string.IsNullOrEmpty(drpMethdDonation.SelectedValue) || string.IsNullOrEmpty(txtInitialCharge.Text.Trim()) || string.IsNullOrEmpty(txtDonationAmt.Text.Trim().Replace("$", "")) || txtDonationAmt.Text.Trim().Replace("$", "") == "0" || string.IsNullOrEmpty(drpchargeMontly.SelectedValue.Trim()))
                {
                    lblError.Text = "Please input donation details and then try.";
                    hidAccordionIndex.Value = "1";
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
                var donation = new ShareAThonDonation
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
                var frequencies = new List<ShareAThonDonationFrequency>();

                var frequencyname = new ShareAThonDonationFrequency();

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
                    for (int i = 0; i < int.Parse(drpMonths.SelectedValue); i++)
                    {
                        var frequency = new ShareAThonDonationFrequency
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
                using (var context = new ShareAThonDLContainer())
                {
                    foreach (var frequency in frequencies)
                    {
                        donation.ShareAThonDonationFrequencies.Add(frequency);
                    }

                    foreach (var offer in offerlines)
                    {
                        donation.ShareAThonOfferLines.Add(offer);
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
                    econnectModel.RollbackSalseDocNumber(_connString, HttpContext.Current.Session["orderNumber"].ToString());
                }
            }
            catch (Exception exception)
            {
                lblError.Text = "Ooops something went wrong in our server. We are not able to process your request." + exception;
            }
        }

        private void Reset()
        {
            ResetControls(Controls);

            lblOrderNo.Text = "####";

            txtInitialCharge.Text = DateTime.Now.ToShortDateString();
            TxtCCOrderDate.Text = DateTime.Now.ToShortDateString();
            LblDate.Text = DateTime.Now.ToShortDateString();

            LoadChargeMonthly();
            LoadRadGridData();

            txtCurDonor.Text = "0";
            txtIncreasing.Text = "0";

            hidAccordionIndex.Value = "0";
        }

        private void ResetControls(ControlCollection controlCollection)
        {
            foreach (Control control in controlCollection)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is Label)
                    ((Label)control).Text = string.Empty;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else
                    ResetControls(control.Controls);
            }
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
                ADDRESS1 = string.Empty,
                ADDRESS2 = string.Empty,
                CITY = string.Empty,
                STATE = string.Empty,
                ZIPCODE = string.Empty,
                COUNTRY = string.Empty,
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

        public List<ShareAThonOfferLine> GetOfferedLines(string orderNumber)
        {
            return (from GridDataItem gridDataItem in RadGridOfferLines.Items
                    let itemQty = (TextBox)gridDataItem.FindControl("TxtQuantity")
                    where !string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0
                    select new ShareAThonOfferLine
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
                drpchargeMontly.Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}