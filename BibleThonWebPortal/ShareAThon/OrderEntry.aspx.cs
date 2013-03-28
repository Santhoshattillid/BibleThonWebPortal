using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlbaBL;
using AlbaDL;
using Telerik.Web.UI;
using Resource = Resources.Resource;

namespace ShareAThon
{
    public partial class ShareAThonOrderEntry : Page
    {
        private OrderEntry _orderEntry;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] == null && !Utilities.DevelopmentMode)
                Response.Redirect("Logout.aspx");

            if (!IsPostBack)
                Reset();
            else
                UpdateSessionVariables();
        }

        /// <summary>
        /// Save the order entry
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OrderEntrySave(object sender, EventArgs e)
        {
            try
            {
                // Creating new instance
                _orderEntry = new OrderEntry();

                // validation for billing address elements
                if (string.IsNullOrEmpty(txtCustName.Text.Trim()))
                {
                    lblError.Text = Resource.CustomerNameEmptyErrorString;
                    hidAccordionIndex.Value = "0";
                    return;
                }

                if (string.IsNullOrEmpty(HdnCustomerNo.Value))
                {
                    lblError.Text = Resource.CUSTOMER_ID_NOTFOUND_INFO;
                    hidAccordionIndex.Value = "0";
                    return;
                }

                //validating donation panel
                if (string.IsNullOrEmpty(drpFrequency.SelectedValue) || string.IsNullOrEmpty(drpMonths.SelectedValue) || string.IsNullOrEmpty(drpMethdDonation.SelectedValue) || string.IsNullOrEmpty(txtInitialCharge.Text.Trim()) || string.IsNullOrEmpty(txtDonationAmt.Text.Trim().Replace("$", "")) || txtDonationAmt.Text.Trim().Replace("$", "") == "0" || string.IsNullOrEmpty(drpchargeMontly.SelectedValue.Trim()))
                {
                    lblError.Text = Resource.DONATION_NOT_FOUND_INFO;
                    hidAccordionIndex.Value = "1";
                    return;
                }

                DateTime initialChargeDate;
                if (!DateTime.TryParse(txtInitialCharge.Text.Trim(), out initialChargeDate))
                {
                    lblError.Text = Resource.DONATION_NOT_FOUND_INFO;
                    hidAccordionIndex.Value = "1";
                    return;
                }

                // the initialChargeDate should start from tomorrow
                if (initialChargeDate <= DateTime.Now)
                {
                    lblError.Text = Resource.INITIAL_CHARGE_DATE_ERROR;
                    hidAccordionIndex.Value = "1";
                    return;
                }

                // declaring credit card expire date
                var expireDate = new DateTime();

                // validation for credit card information tab
                if (drpMethdDonation.SelectedValue.Equals("Credit Card"))
                {
                    if (string.IsNullOrEmpty(txtCreditCardNo.Text.Trim()))
                    {
                        lblError.Text = Resource.CREDIT_CARD_NO_NOT_FOUND_INFO;
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (string.IsNullOrEmpty(txtExpirationDate.Text.Trim()))
                    {
                        lblError.Text = Resource.CREDIT_CARD_EXPIRY_NOT_FOUND_INFO;
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (txtExpirationDate.Text.Trim().Length < 4)
                    {
                        lblError.Text = Resource.CREDIT_CARD_EXPIRY_NOT_FOUND_INFO;
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (!DateTime.TryParse(txtExpirationDate.Text.Substring(0, 2) + "/01/" + txtExpirationDate.Text.Substring(2, 2), out expireDate))
                    {
                        lblError.Text = Resource.CREDIT_CARD_EXPIRY_INVALID_FORMAT_INFO;
                        hidAccordionIndex.Value = "3";
                        return;
                    }

                    if (string.IsNullOrEmpty(txtCVN.Text.Trim()))
                    {
                        lblError.Text = Resource.CREDIT_CARD_CVV_NOT_FOUND_INFO;
                        hidAccordionIndex.Value = "3";
                        return;
                    }
                }

                // saving in local database
                //using (var context = new ShareAThonDLContainer())
                //{
                // flag to set Authorize.Net payment now
                var processOnlinePayment = false;
                var processGp = false;

                // Generating order number
                var orderNo = new GPEconnect().GetSalesOrderNumber();

                // mapping collection of data to ShareAThonOfferLine class
                var offerlines = GetOfferedLines(orderNo);

                //TxtOrderNumber.Text = orderNo;

                // mapping data to ShareAThonDonation class
                var donation = new ShareAThonDonation
                                   {
                                       OrderId = orderNo,
                                       CustomerId = HdnCustomerNo.Value,
                                       CurrentlyDonorOf = FormatUtilities.ConvertToDecimal(txtCurDonor.Text),
                                       InitialChargeOn = DateTime.Parse(txtInitialCharge.Text),
                                       DonationAmount = FormatUtilities.ConvertToDecimal(txtDonationAmt.Text),
                                       IncreasingTo = FormatUtilities.ConvertToDecimal(txtIncreasing.Text),
                                       DayToChargeMonthly = int.Parse(drpchargeMontly.SelectedValue)
                                   };

                var shareAThonOfferLines = offerlines as ShareAThonOfferLine[] ?? offerlines.ToArray();

                // verifying the frequency
                if (drpFrequency.SelectedValue.Equals("One Time") || drpFrequency.SelectedValue.Equals("Other"))
                {
                    var frequency = new ShareAThonDonationFrequency
                    {
                        OrderId = orderNo,
                        DueDate = DateTime.Now,
                        Amount = decimal.Parse(txtDonationAmt.Text),
                        ModeOfDonation = drpMethdDonation.SelectedValue
                    };

                    // only one time or Other if it is other than credit card
                    if (drpMethdDonation.SelectedValue != "Credit Card")
                    {
                        // Set processed if it is cheque / money order / cash
                        frequency.Status = "Process";
                        //// Saving in GP
                        processGp = true;
                    }
                    else // Credit Card Transaction Only OneTime
                    {
                        // Need to Go for Authorize.Net for processing.
                        frequency.Status = "Pending";

                        // setting true to flag
                        processOnlinePayment = true;
                    }

                    // saving frequency details to db
                    donation.ShareAThonDonationFrequencies.Add(frequency);
                }
                else
                {
                    // recurring payment need to handle
                    var noOfReccurentPayments = int.Parse(drpMonths.SelectedValue);

                    if (DateTime.Now.Subtract(initialChargeDate).Days == 0)
                    {
                        // Adding current day frequency to database
                        var frequency = new ShareAThonDonationFrequency
                        {
                            OrderId = orderNo,
                            DueDate = DateTime.Now,
                            Amount = decimal.Parse(txtDonationAmt.Text),
                            ModeOfDonation = drpMethdDonation.SelectedValue
                        };

                        // only one time or Other if it is other than credit card, it may be cash / money order / cheque
                        if (drpMethdDonation.SelectedValue != "Credit Card")
                        {
                            // Set processed if it is cheque / money order / cash
                            frequency.Status = "Process";

                            //// Saving in GP
                            processGp = true;
                        }
                        else // Credit Card Transaction, Monthly starts from today
                        {
                            // Need to Go for Authorize.Net for processing.
                            frequency.Status = "Pending";

                            // setting true to flag
                            processOnlinePayment = true;
                        }

                        donation.ShareAThonDonationFrequencies.Add(frequency);
                        noOfReccurentPayments -= 1;
                    }

                    for (int i = 0; i < noOfReccurentPayments; i++)
                    {
                        // generating frequency records for db
                        var frequency = new ShareAThonDonationFrequency
                                            {
                                                OrderId = (i.Equals(0)) ? orderNo : "",
                                                DueDate =
                                                    (i.Equals(0) &&
                                                     DateTime.Now.Subtract(initialChargeDate).Days != 0)
                                                        ? DateTime.Parse(txtInitialCharge.Text)
                                                        : DateTime.Parse((DateTime.Today.Month + i) + "/" +
                                                                         drpchargeMontly.SelectedValue + "/" +
                                                                         DateTime.Now.Year),
                                                Amount = FormatUtilities.ConvertToDecimal(txtDonationAmt.Text),
                                                Status = "Pending",
                                                ModeOfDonation = drpMethdDonation.SelectedValue
                                            };

                        // adding into frequencies table
                        donation.ShareAThonDonationFrequencies.Add(frequency);
                    }
                }

                // iterating all offerlines and adding to datatable
                foreach (var offer in shareAThonOfferLines)
                {
                    donation.ShareAThonOfferLines.Add(offer);
                }

                // checking the online payment flag
                if (!processOnlinePayment)
                {
                    if (processGp)
                        ProcessOrderToGp(orderNo, shareAThonOfferLines, expireDate);
                    else
                    {
                        Reset();
                        lblError.Text = @"The Order [" + orderNo + @"]  has been saved successfully.";
                        lblError.CssClass = "Success";
                    }
                }
                else
                {
                    var authorizeNetSubscription = new AuthorizeNetSubscription();
                    var subscriptionId = authorizeNetSubscription.CreateSubscription(new CardDetails
                                                                    {
                                                                        CardCode = txtCVN.Text.Trim(),
                                                                        CardNo = txtCreditCardNo.Text.Trim(),
                                                                        ExpireDate = expireDate
                                                                    },
                                                                    DateTime.Parse(txtInitialCharge.Text),
                                                                    short.Parse(drpMonths.SelectedValue),
                                                                    decimal.Parse(txtDonationAmt.Text),
                                                                    short.Parse(drpchargeMontly.SelectedValue),
                                                                    HdnCustomerNo.Value,
                                                                    txtCustName.Text,
                                                                    txtPhone.Text.Trim()
                                                                );
                    donation.AuthorizeNetSubscriptionId = subscriptionId;

                    // Saving into database
                    _orderEntry.CreateDonation(donation);

                    Reset();
                    lblError.Text = Resource.SHAREATHON_ORDER_SUCCESS;
                    lblError.CssClass = "Success";
                }

                // Saving into database
                _orderEntry.CreateDonation(donation);

                //}
            }
            catch (Exception exception)
            {
                lblError.Text = Resource.ORDER_ENTRY_ERROR + exception;
            }
        }

        /// <summary>
        /// Resets all the controls
        /// </summary>
        private void Reset()
        {
            ResetControls(Controls);

            lblOrderNo.Text = @"####";

            txtInitialCharge.Text = DateTime.Now.AddDays(1).ToShortDateString();
            TxtCCOrderDate.Text = DateTime.Now.ToShortDateString();
            LblDate.Text = DateTime.Now.ToShortDateString();

            LoadChargeMonthly();
            LoadRadGridData();

            txtCurDonor.Text = "0";
            txtIncreasing.Text = "0";

            hidAccordionIndex.Value = "0";
        }

        /// <summary>
        /// Resets the controls.
        /// </summary>
        /// <param name="controlCollection">The control collection.</param>
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

        /// <summary>
        /// Gets the offered lines.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        private IEnumerable<ShareAThonOfferLine> GetOfferedLines(string orderNumber)
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

        /// <summary>
        /// RADs the grid offer lines data binding.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void RadGridOfferLinesDataBinding(object sender, EventArgs e)
        {
            LoadRadGridData();
        }

        /// <summary>
        /// Loads the RADgrid data.
        /// </summary>
        private void LoadRadGridData()
        {
            var offerLines = new GPEconnect().GetOfferLines();
            RadGridOfferLines.DataSource = offerLines;
            RadGridOfferLines.DataBind();
        }

        /// <summary>
        /// Updates the session variables.
        /// </summary>
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

        /// <summary>
        /// Loads the charge monthly.
        /// </summary>
        private void LoadChargeMonthly()
        {
            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                drpchargeMontly.Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Processes the order to gp.
        /// </summary>
        /// <param name="orderNo">The order no.</param>
        /// <param name="offerlines">The offerlines.</param>
        /// <param name="expireDate"> </param>
        private void ProcessOrderToGp(string orderNo, IEnumerable<ShareAThonOfferLine> offerlines, DateTime expireDate)
        {
            // initiating ecoonect model
            //var econnectModel = new EConnectModel();

            // initiating order entry model
            _orderEntry = new OrderEntry();
            bool isOrderCreated;

            Session["orderNumber"] = orderNo;

            OrderProcess orderProcess = GetCustomerHeader(orderNo, offerlines.Sum(o => o.Qty * GetUnitPrice(o.OfferNo)));

            List<OrderItems> listOrders = GetOrderedItems(orderNo);

            string fileName = Server.MapPath("~/SalesOrder.xml");

            _orderEntry.CreateOrderInGP(fileName, Session["orderNumber"].ToString(), orderProcess, listOrders, new CardDetails(ddlCreditCardType.SelectedValue.ToString(CultureInfo.InvariantCulture), txtCreditCardNo.Text.Trim(), expireDate, string.Empty), out isOrderCreated);
            if (isOrderCreated)
            {
                Reset();
                lblError.Text = @"The Order [" + orderNo + @"]  has been processed successfully.";
                lblError.CssClass = "Success";
            }
            else
            {
                lblError.Text = Resource.ORDER_ENTRY_ERROR;
                lblError.CssClass = "error errorinfo";

                //econnectModel.RollbackSalseDocNumber(Utilities._connString, HttpContext.Current.Session["orderNumber"].ToString());
            }
        }

        /// <summary>
        /// Gets the ordered items.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        private List<OrderItems> GetOrderedItems(string orderNumber)
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

        /// <summary>
        /// Gets the unit price.
        /// </summary>
        /// <param name="offerNo">The offer no.</param>
        /// <returns></returns>
        private decimal GetUnitPrice(string offerNo)
        {
            decimal unitPrice = 0;
            var sourceOfferLines = new GPEconnect().GetOfferLines();
            foreach (var sourceOfferLine in sourceOfferLines.Where(sourceOfferLine => sourceOfferLine.OfferId.Equals(offerNo)))
            {
                unitPrice = sourceOfferLine.Price;
            }
            return unitPrice;
        }

        /// <summary>
        /// Gets the customer header.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="subTotal">The sub total.</param>
        /// <returns></returns>
        private OrderProcess GetCustomerHeader(string orderNumber, decimal subTotal)
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
                PHNUMBR1 = FormatUtilities.FormatToText(txtPhone.Text),
                FREIGHT = 0,
                FRTTXAMT = 0,
                MISCAMNT = 0,
                MSCTXAMT = 0,
                TRDISAMT = 0,
                TAXAMNT = 0,
            };

            var donationAmount = Convert.ToDecimal(string.IsNullOrEmpty(txtDonationAmt.Text) ? "0" : txtDonationAmt.Text);
            if (donationAmount > subTotal)
            {
                orderProcess.MISCAMNT = donationAmount - subTotal;
            }

            orderProcess.DOCAMNT = Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT + orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) - Convert.ToDecimal(orderProcess.TRDISAMT);
            return orderProcess;
        }

        #region Un-Used Codes

        #endregion Un-Used Codes
    }
}