using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Biblethon.Controller;

public partial class Biblethon_OrderEntry : Page
{
    private readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();
    public List<BillingAddress> CustomerAddress;
    public List<ShippingAddress> ShippingAddress;
    public List<OfferLines> OfferLines;
    DataRow[] _dtFilterRows;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LblDate.Text = DateTime.Now.ToString("MM / dd / yyyy");
            if (Request.QueryString.AllKeys.Contains("Status") && Request.QueryString.AllKeys.Contains("Message"))
            {
                // here is it considered as request from payment gateway, so loading order number from session
                lblOrderNo.Text = Session["orderNumber"].ToString();
                lblOrderNum.Text = Session["orderNumber"].ToString();
                LoadLineItems((IEnumerable<DictionaryEntry>)Session["userSelectedOfferlines"]);
                hidAccordionIndex.Value = "3";

                txtCustName.Text = Session["CustomerName"].ToString();
                txtTelephone.Text = Session["Telephone"].ToString();
                cbShipping.Checked = Convert.ToBoolean(Session["IsShippingAddressIsSame"]);

                LoadCustomerAddressFromEconnect();

                lblError.Text = Request.QueryString["Message"];

                hidAccordionIndex.Value = "3";
            }
            else
            {
                // here is it considered as new request to generating new sales number from econnect
                Session["orderNumber"] = new EConnectModel().GetNextSalseDocNumber(_connString);
                lblOrderNo.Text = Session["orderNumber"].ToString();
                lblOrderNum.Text = Session["orderNumber"].ToString();
                LoadLineItems(null);
                txtShipping.Text = "0.0";
                txtADonation.Text = "0.0";
            }
        }
        else
        {
            UpdateSessionVariables();
        }

        // setting operator here
        try
        {
            LblOperator.Text = Session["LoggedInUser"].ToString();
        }
        catch (Exception)
        {
            // it might throw error if we run the form individually
            LblOperator.Text = string.Empty;
        }
        

        // making all shipping address fields as read only
        txtCustomerName.ReadOnly = true;
        txtTelephone.ReadOnly = true;
        txtAddress1.ReadOnly = true;
        txtAddress2.ReadOnly = true;
        txtAddress3.ReadOnly = true;
        txtCity.ReadOnly = true;
        txtState.ReadOnly = true;
        txtZipCode.ReadOnly = true;
        txtCountry.ReadOnly = true;
        txtEmail.ReadOnly = true;


        // assigning left hand side panel labels
        LblAdditionalDonation.Text = string.Format("${0}", txtADonation.Text);
        LblShippingTotal.Text = string.Format("${0}", txtShipping.Text);
        LblOrderSubtotal.Text = string.Format("${0}", Convert.ToDecimal(string.IsNullOrEmpty(lblGrandTotal.Text.Replace("$","")) ? "0" : lblGrandTotal.Text.Replace("$","")) - Convert.ToDecimal(string.IsNullOrEmpty(txtADonation.Text.Replace("$","")) ? "0" : txtADonation.Text.Replace("$","")) - Convert.ToDecimal(string.IsNullOrEmpty(txtShipping.Text.Replace("$","")) ? "0" : txtShipping.Text.Replace("$","")));
        LblTotalAmount.Text = lblGrandTotal.Text;
    }

    protected void btnBillContinue_Click(object sender, EventArgs e)
    {
        if (LoadCustomerAddressFromEconnect())
        {
            UpdateSessionVariables();
            hidAccordionIndex.Value = "1";
        }
        else
            lblError.Text = "The given customer details are not found. Please select the customer for billing address.";
    }

    protected void btnShipContinue_Click(object sender, EventArgs e)
    {
        // assigning left panel shipping address

        hidAccordionIndex.Value = "2";

        LblSAName.Text = txtCustName.Text;
        LblSAAddress1.Text = txtAddress1.Text;
        LblSAAddress2.Text = txtAddress2.Text;
        LblSACityStateZip.Text = txtCity.Text + ", " + txtState.Text + ", " + txtZipCode.Text;
        LblSACountry.Text = txtCountry.Text;
        LblSATelephone1.Text = txtTelephone.Text;
        LblSAEmail.Text = txtEmail.Text;

        #region unUsedCode
        /*try
        {
            var custometShipAddress = new ShippingAddress().GetCustometShipAddress(_connString);
            if (custometShipAddress != null)
            {
                _dtShip = ToDataTable(custometShipAddress);
                if (cbShipping.Checked == false)
                {
                    string filterExprssion = "CustomerNo='" + hidCustId.Value + "' And Zipcode='" + txtZipCode.Text + "' and Telephone1='" + txtTelephone.Text + "'";
                    _dtFilterRows = _dtShip.Select(filterExprssion);
                    if (_dtFilterRows.Length == 0)
                    {
                        lblError.Visible = true;
                        lblError.Text = "Shipping address doesn't match, Please try again";
                    }
                    else
                    {
                        lblError.Visible = false;
                        DataTable dtNewFilterData = _dtShip.Clone();
                        foreach (DataRow drNew in _dtFilterRows)
                        {
                            dtNewFilterData.ImportRow(drNew);
                            hidAddressCode.Value = dtNewFilterData.Rows[0]["AddressCode"].ToString();
                        }
                        hidAccordionIndex.Value = "2";
                    }
                }
                else
                {
                    hidAccordionIndex.Value = "2";
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }*/
        #endregion
    }

    protected void btnConfirmOffer_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "3";
        lblOrderNum.Text = Session["orderNumber"].ToString();
        lblOrderTotal.Text = lblGrandTotal.Text;
    }

    protected void BtnCreditCardProcess_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "4";
    }

    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        try
        {
            OrderProcess orderProcess = GetCustomerHeader();
            List<OrderItems> listOrders = GetOrderedItems();
            string fileName = Server.MapPath("~/SalesOrder.xml");
            bool savedStatus = new EConnectModel().SerializeSalesOrderObject(fileName, _connString, orderProcess, listOrders);
            lblError.Text = "Transaction processed successfully.";
            Response.Redirect("OrderDetails.aspx");
        }
        catch (Exception)
        {
            lblError.Text = "Ooops something went wrong in our server. We are not able to process your request.";
        }
    }

    protected void btnProcessOrder_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtExpirationDate.Text.Trim()) && !string.IsNullOrEmpty(txtCreditCardNo.Text.Trim()) && !string.IsNullOrEmpty(txtCVN.Text.Trim()))
        {
            var result = (from GridViewRow row in gdvOfferLine.Rows
                          let itemQty = (TextBox)row.FindControl("TXTQty")
                          where Convert.ToDecimal(itemQty.Text) > 0
                          let itemNumber = gdvOfferLine.Rows[row.RowIndex].Cells[0].Text
                          select new DictionaryEntry
                          {
                              Key = itemNumber,
                              Value = itemQty
                          });
            Session["userSelectedOfferlines"] = result;
            Response.Redirect("PaymentProcessing.aspx?Amount=" + lblGrandTotal.Text.Trim().Replace("$", "") + "&CardNo=" + txtCreditCardNo.Text.Trim() + "&ExpDate=" + txtExpirationDate.Text.Trim() + "&CVV=" + txtCVN.Text.Trim());
        }
        else
        {
            hidAccordionIndex.Value = "3";
            lblError.Text = "Please input required credit card informations and try processing.";
        }
    }

    private void LoadLineItems(IEnumerable<DictionaryEntry> userSelectedOfferlines)
    {
        var offerLines = new OfferLines().GetOfferLines(_connString);
        if (userSelectedOfferlines != null)
        {
            foreach (DictionaryEntry selectedOfferline in userSelectedOfferlines)
            {
                foreach (OfferLines offerLs in offerLines)
                {
                    if (offerLs.OfferId == selectedOfferline.Key.ToString())
                    {
                        offerLs.Quantity = Convert.ToInt32(selectedOfferline.Value);
                        break;
                    }
                }
            }
        }
        gdvOfferLine.DataSource = offerLines;
        gdvOfferLine.DataBind();
    }

    public DataTable ToDataTable<T>(IList<T> listData)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        var table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        var values = new object[props.Count];
        foreach (T item in listData)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }

    public DataTable GetFilterData(string customerName, string address)
    {
        var dtSession = Session["customerAddress"] as DataTable;
        if (dtSession != null)
        {
            DataView dvSession = dtSession.DefaultView;
            string filterExprssion = "CustomerName like'" + customerName + "%' And " + address;
            _dtFilterRows = dvSession.Table.Select(filterExprssion);
            DataTable dtNewFilterData = dtSession.Clone();
            foreach (DataRow drNew in _dtFilterRows)
                dtNewFilterData.ImportRow(drNew);
            return dtNewFilterData;
        }
        return null;
    }

    public DataTable GetFilterDataBYId(string customerID)
    {
        var dtSession = Session["customerAddress"] as DataTable;
        if (dtSession != null)
        {
            DataView dvSession = dtSession.DefaultView;
            string filterExprssion = "CustomerNo='" + customerID + "'";
            _dtFilterRows = dvSession.Table.Select(filterExprssion);
            DataTable dtNewFilterData = dtSession.Clone();
            foreach (DataRow drNew in _dtFilterRows)
            {
                dtNewFilterData.ImportRow(drNew);
            }
            return dtNewFilterData;
        }
        return null;
    }

    public void GetCustomerData(DataTable dtSearch)
    {
        if (dtSearch.Rows.Count == 1)
        {
            if (dtSearch.Rows.Cast<DataRow>().Any())
            {
                txtCustName.Text = dtSearch.Rows[0]["CustomerName"].ToString();
                lblAddress1.Text = dtSearch.Rows[0]["Address1"].ToString();
                lblAddress2.Text = dtSearch.Rows[0]["Address2"].ToString();
                txtPhone.Text = dtSearch.Rows[0]["Telephone1"].ToString();
                lblCity.Text = dtSearch.Rows[0]["City"].ToString();
                lblState.Text = dtSearch.Rows[0]["State"].ToString();
                lblZipCode.Text = dtSearch.Rows[0]["Zipcode"].ToString();
                lblCountry.Text = dtSearch.Rows[0]["Country"].ToString();
                txtBEmail.Text = dtSearch.Rows[0]["Email"].ToString();
                hidAddressCode.Value = dtSearch.Rows[0]["AddressCode"].ToString();
            }
        }
    }

    public OrderProcess GetCustomerHeader()
    {
        var shipping = string.IsNullOrEmpty(txtShipping.Text.Replace("$", "")) ? 0 : Convert.ToDecimal(txtShipping.Text.Replace("$", ""));
        var donation = string.IsNullOrEmpty(txtADonation.Text.Replace("$", "")) ? 0 : Convert.ToDecimal(txtADonation.Text.Replace("$", ""));
        var subTotal = (string.IsNullOrEmpty(lblGrandTotal.Text.Replace("$", "")) ? 0 : Convert.ToDecimal(lblGrandTotal.Text.Replace("$", ""))) - shipping - donation;
        var orderProcess = new OrderProcess
        {
            SOPTYPE = 2,
            SOPNUMBE = Session["orderNumber"].ToString().Trim(),
            BACHNUMB = "BIBLETHON",
            DOCID = "STDORD",
            CUSTNMBR = HdnCustomerNo.Value,
            CUSTNAME = txtCustName.Text,
            SUBTOTAL = subTotal,
            DOCDATE = DateTime.Now.ToShortDateString(),
            ORDRDATE = DateTime.Now.ToShortDateString(),
            ShipToName = hidAddressCode.Value,
            ADDRESS1 = txtAddress1.Text,
            ADDRESS2 = txtAddress2.Text,
            CITY = txtCity.Text,
            STATE = txtState.Text,
            ZIPCODE = txtZipCode.Text,
            COUNTRY = txtCountry.Text,
            PHNUMBR1 = txtTelephone.Text,
            FREIGHT = shipping,
            FRTTXAMT = 0,
            MISCAMNT = donation,
            MSCTXAMT = 0,
            TRDISAMT = 0,
            TAXAMNT = 0,
        };
        orderProcess.DOCAMNT = Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT + orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) - Convert.ToDecimal(orderProcess.TRDISAMT);
        return orderProcess;

    }

    public List<OrderItems> GetOrderedItems()
    {
        return (from GridViewRow row in gdvOfferLine.Rows
                let itemQty = (TextBox)row.FindControl("TXTQty")
                where Convert.ToDecimal(itemQty.Text) > 0
                let itemNumber = gdvOfferLine.Rows[row.RowIndex].Cells[0].Text
                let itemDescription = gdvOfferLine.Rows[row.RowIndex].Cells[1].Text
                let itemPrice = (Label)row.FindControl("lblPrice")
                //let itemXTNDPRCE = (TextBox)row.FindControl("LBLSubTotal")
                select new OrderItems
                {
                    SOPTYPE = 2,
                    SOPNUMBE = Session["orderNumber"].ToString().Trim(),
                    CUSTNMBR = HdnCustomerNo.Value,
                    DOCDATE = DateTime.Now.ToShortDateString(),
                    ITEMNMBR = itemNumber,
                    ITEMDESC = itemDescription,
                    UNITPRCE = Convert.ToDecimal(itemPrice.Text.Replace("$", "0")),
                    QUANTITY = Convert.ToDecimal(itemQty.Text),
                    XTNDPRCE = Convert.ToDecimal(itemQty.Text.Replace("$", "0")) * Convert.ToDecimal(itemPrice.Text.Replace("$", "0")),  //Convert.ToDecimal(itemXTNDPRCE.Text.Substring(1)),
                    TOTALQTY = 0,
                    CURNCYID = "",
                    UOFM = "",
                    NONINVEN = 0,
                    ShipToName = hidAddressCode.Value,
                    ADDRESS1 = txtAddress1.Text,
                    ADDRESS2 = txtAddress2.Text,
                    CITY = txtCity.Text,
                    STATE = txtState.Text,
                    ZIPCODE = txtZipCode.Text,
                    COUNTRY = txtCountry.Text,
                    PHNUMBR1 = txtTelephone.Text
                }).ToList();
    }

    private void UpdateSessionVariables()
    {
        // keeping the customer name and telephone in session
        Session["CustomerName"] = txtCustName.Text.Trim();
        Session["Telephone"] = txtTelephone.Text.Trim();
        Session["IsShippingAddressIsSame"] = cbShipping.Checked;
    }

    private bool LoadCustomerAddressFromEconnect()
    {
        var customerAddress = new BillingAddress().GetCustomerDetails(_connString);
        var address = (from c in customerAddress
                       where c.CustomerName.Contains(txtCustName.Text) && c.Telephone1.Contains(txtTelephone.Text.Replace("(","").Replace(")","").Replace("Ext.","").Replace(" ",""))
                       select c).ToList();
        if (address.Count > 0)
        {
            // assigning billing address values to labels
            lblAddress1.Text = address[0].Address1;
            lblAddress2.Text = address[0].Address2;
            lblAddress3.Text = address[0].Address3;
            lblCity.Text = address[0].City;
            lblCountry.Text = address[0].Country;
            lblState.Text = address[0].State;
            lblZipCode.Text = address[0].Zipcode;

            //assighing credit card details
            txtCreditCardNo.Text = address[0].CreditCardNumber;
            txtExpirationDate.Text = address[0].CreditCardExpireDate;

            if (cbShipping.Checked)
            {
                // assigning the address to shipping fields
                txtCustomerName.Text = txtCustName.Text;
                txtAddress1.Text = lblAddress1.Text;
                txtAddress2.Text = lblAddress2.Text;
                txtAddress3.Text = lblAddress3.Text;
                txtTelephone.Text = txtPhone.Text;
                txtCity.Text = lblCity.Text;
                txtState.Text = lblState.Text;
                txtZipCode.Text = lblZipCode.Text;
                txtCountry.Text = lblCountry.Text;
                txtEmail.Text = txtBEmail.Text;


                // assigning left panel Shipping  address
                LblSAName.Text = txtCustName.Text;
                LblSAAddress1.Text = lblAddress1.Text;
                LblSAAddress2.Text = lblAddress2.Text;
                LblSACityStateZip.Text = lblCity.Text + ", " + lblState.Text + ", " + lblZipCode.Text;
                LblSACountry.Text = lblCountry.Text;
                LblSATelephone1.Text = txtTelephone.Text;
                LblSAEmail.Text = txtEmail.Text;
            }
            else
            {
                txtCustomerName.Text = txtCustName.Text;
                txtTelephone.Text = txtPhone.Text;
                /*
                 // clearing shipping address fields
                txtAddress1.Text = string.Empty;
                txtAddress2.Text = string.Empty;
                txtAddress3.Text = string.Empty;
                txtCity.Text = string.Empty;
                txtState.Text = string.Empty;
                txtZipCode.Text = string.Empty;
                txtCountry.Text = string.Empty;
                txtEmail.Text = string.Empty; */

                // assigning shipping address from econnect
                var custometShipAddress = new ShippingAddress().GetCustometShipAddress(_connString);
                foreach (ShippingAddress shippingAddress in custometShipAddress)
                {
                    txtAddress1.Text = shippingAddress.Address1;
                    txtAddress2.Text = shippingAddress.Address2;
                    txtAddress3.Text = shippingAddress.Address3;
                    txtCity.Text = shippingAddress.City;
                    txtState.Text = shippingAddress.State;
                    txtZipCode.Text = shippingAddress.Zipcode;
                    txtCountry.Text = shippingAddress.Country;
                    txtEmail.Text = shippingAddress.Email;
                    break;
                }
            }

            // assigning left panel billing  address
            LblBName.Text = txtCustName.Text;
            LblBAddress1.Text = lblAddress1.Text;
            LblBAddress2.Text = lblAddress2.Text;
            LblBCityStateZip.Text = lblCity.Text + ", " + lblState.Text + ", " + lblZipCode.Text;
            LblBCountry.Text = lblCountry.Text;
            LblBTelephone1.Text = txtTelephone.Text;
            LblBEmail.Text = txtEmail.Text;

            return true;
        }

        return false;
    }

}