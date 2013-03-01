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
using Telerik.Web.UI;

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
                TxtOrderNumber.Text = Session["orderNumber"].ToString().Trim();
                LoadRadGridData(false);
                hidAccordionIndex.Value = "3";

                txtCustName.Text = Session["CustomerName"].ToString();
                txtTelephone.Text = Session["Telephone"].ToString();
                cbShipping.Checked = Convert.ToBoolean(Session["IsShippingAddressIsSame"]);

                //LoadCustomerAddressFromEconnect();

                lblError.Text = Request.QueryString["Message"];

                hidAccordionIndex.Value = "3";
            }
            else
            {
                // here is it considered as new request to generating new sales number from econnect
                Session["orderNumber"] = new EConnectModel().GetNextSalseDocNumber(_connString);
                lblOrderNo.Text = Session["orderNumber"].ToString().Trim();
                TxtOrderNumber.Text = Session["orderNumber"].ToString().Trim();
                LoadRadGridData(true);
                txtShipping.Text = "0.0";
                txtADonation.Text = "0.0";
            }
        }
        else
        {
            UpdateSessionVariables();
            LoadRadGridData(false);
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
        //txtCustomerName.ReadOnly = true;
        //txtTelephone.ReadOnly = true;
        //txtAddress1.ReadOnly = true;
        //txtAddress2.ReadOnly = true;
        //txtAddress3.ReadOnly = true;
        //txtCity.ReadOnly = true;
        //txtState.ReadOnly = true;
        //txtZipCode.ReadOnly = true;
        //txtCountry.ReadOnly = true;
        //txtEmail.ReadOnly = true;


        // assigning left hand side panel labels
        LblAdditionalDonation.Text = string.Format("${0}", txtADonation.Text);
        LblShippingTotal.Text = string.Format("${0}", txtShipping.Text);
        LblOrderSubtotal.Text = string.Format("${0}", Convert.ToDecimal(string.IsNullOrEmpty(lblGrandTotal.Text.Replace("$","")) ? "0" : lblGrandTotal.Text.Replace("$","")) - Convert.ToDecimal(string.IsNullOrEmpty(txtADonation.Text.Replace("$","")) ? "0" : txtADonation.Text.Replace("$","")) - Convert.ToDecimal(string.IsNullOrEmpty(txtShipping.Text.Replace("$","")) ? "0" : txtShipping.Text.Replace("$","")));
        LblTotalAmount.Text = lblGrandTotal.Text;
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
            UpdateSessionVariables();
            Response.Redirect("PaymentProcessing.aspx?Amount=" + lblGrandTotal.Text.Trim().Replace("$", "") + "&CardNo=" + txtCreditCardNo.Text.Trim() + "&ExpDate=" + txtExpirationDate.Text.Trim() + "&CVV=" + txtCVN.Text.Trim());
        }
        else
        {
            hidAccordionIndex.Value = "3";
            lblError.Text = "Please input required credit card informations and try processing.";
        }
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

    public void RadGridOfferLinesDataBinding(object sender, EventArgs e)
    {
        LoadRadGridData(false);
    }

    private void LoadRadGridData(bool forceFreshValues)
    {
        var userSelectedOfferlines = new Hashtable();
        try
        {
            userSelectedOfferlines = (Hashtable)Session["userSelectedOfferlines"];
        }
        catch (Exception)
        {}
        var offerLines = new OfferLines().GetOfferLines(_connString);
        if (!forceFreshValues)
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
        RadGridOfferLines.DataSource = offerLines;
        RadGridOfferLines.DataBind();
    }

    public List<OrderItems> GetOrderedItems()
    {
        return (from GridDataItem gridDataItem in RadGridOfferLines.Items
                let itemQty = (TextBox) gridDataItem.FindControl("TxtQuantity")
                where !string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0
                select new OrderItems()
                           {
                               SOPTYPE = 2, SOPNUMBE = Session["orderNumber"].ToString().Trim(), CUSTNMBR = HdnCustomerNo.Value, DOCDATE = DateTime.Now.ToShortDateString(), ITEMNMBR = gridDataItem.Cells[2].Text, ITEMDESC = gridDataItem.Cells[3].Text, UNITPRCE = Convert.ToDecimal(gridDataItem.Cells[5].Text.Replace("$", "0")), QUANTITY = Convert.ToDecimal(itemQty.Text), XTNDPRCE = Convert.ToDecimal(itemQty.Text.Replace("$", "0"))*Convert.ToDecimal(gridDataItem.Cells[5].Text.Replace("$", "0")), TOTALQTY = 0, CURNCYID = "", UOFM = "", NONINVEN = 0, ShipToName = hidAddressCode.Value, ADDRESS1 = txtAddress1.Text, ADDRESS2 = txtAddress2.Text, CITY = txtCity.Text, STATE = txtState.Text, ZIPCODE = txtZipCode.Text, COUNTRY = txtCountry.Text, PHNUMBR1 = txtTelephone.Text
                           }).ToList();
    }

    private void UpdateSessionVariables()
    {
        // keeping the customer name and telephone in session
        Session["CustomerName"] = txtCustName.Text.Trim();
        Session["Telephone"] = txtTelephone.Text.Trim();
        Session["IsShippingAddressIsSame"] = cbShipping.Checked;

        var result = new Hashtable();

        foreach (GridDataItem gridDataItem in RadGridOfferLines.Items)
        {
            var itemQty = (TextBox) gridDataItem.FindControl("TxtQuantity");
            if(!string.IsNullOrEmpty(itemQty.Text) && Convert.ToInt32(itemQty.Text) > 0)
            {
                result.Add(gridDataItem.Cells[2].Text,itemQty.Text);
            }
        }
        Session["userSelectedOfferlines"] =result;
    }

   #region Un-used code

    //protected void btnShipContinue_Click(object sender, EventArgs e)
    //{
    //    // assigning left panel shipping address

    //    hidAccordionIndex.Value = "2";

    //    LblSAName.Text = txtCustName.Text;
    //    LblSAAddress1.Text = txtAddress1.Text;
    //    LblSAAddress2.Text = txtAddress2.Text;
    //    LblSACityStateZip.Text = txtCity.Text + ", " + txtState.Text + ", " + txtZipCode.Text;
    //    LblSACountry.Text = txtCountry.Text;
    //    LblSATelephone1.Text = txtTelephone.Text;
    //    LblSAEmail.Text = txtEmail.Text;

    //    #region unUsedCode
    //    /*try
    //    {
    //        var custometShipAddress = new ShippingAddress().GetCustometShipAddress(_connString);
    //        if (custometShipAddress != null)
    //        {
    //            _dtShip = ToDataTable(custometShipAddress);
    //            if (cbShipping.Checked == false)
    //            {
    //                string filterExprssion = "CustomerNo='" + hidCustId.Value + "' And Zipcode='" + txtZipCode.Text + "' and Telephone1='" + txtTelephone.Text + "'";
    //                _dtFilterRows = _dtShip.Select(filterExprssion);
    //                if (_dtFilterRows.Length == 0)
    //                {
    //                    lblError.Visible = true;
    //                    lblError.Text = "Shipping address doesn't match, Please try again";
    //                }
    //                else
    //                {
    //                    lblError.Visible = false;
    //                    DataTable dtNewFilterData = _dtShip.Clone();
    //                    foreach (DataRow drNew in _dtFilterRows)
    //                    {
    //                        dtNewFilterData.ImportRow(drNew);
    //                        hidAddressCode.Value = dtNewFilterData.Rows[0]["AddressCode"].ToString();
    //                    }
    //                    hidAccordionIndex.Value = "2";
    //                }
    //            }
    //            else
    //            {
    //                hidAccordionIndex.Value = "2";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //    }*/
    //    #endregion
    //}

    //protected void btnConfirmOffer_Click(object sender, EventArgs e)
    //{
    //    hidAccordionIndex.Value = "3";
    //    lblOrderNum.Text = Session["orderNumber"].ToString();
    //    lblOrderTotal.Text = lblGrandTotal.Text;
    //}


    //private bool LoadCustomerAddressFromEconnect()
    //{
    //    var customerAddress = new BillingAddress().GetCustomerDetails(_connString);
    //    var address = (from c in customerAddress
    //                   where c.CustomerName.Equals(txtCustName.Text) // && c.Telephone1.Equals(txtPhone.Text.Replace("(", "").Replace(")", "").Replace("Ext.", "").Replace(" ", "").Replace("-",""))
    //                   select c).ToList();
    //    if (address.Count > 0)
    //    {
    //        // assigning billing address values to labels
    //        lblAddress1.Text = address[0].Address1;
    //        lblAddress2.Text = address[0].Address2;
    //        lblAddress3.Text = address[0].Address3;
    //        lblCity.Text = address[0].City;
    //        lblCountry.Text = address[0].Country;
    //        lblState.Text = address[0].State;
    //        lblZipCode.Text = address[0].Zipcode;

    //        //assighing credit card details
    //        txtCreditCardNo.Text = address[0].CreditCardNumber;
    //        txtExpirationDate.Text = address[0].CreditCardExpireDate;

    //        if (cbShipping.Checked)
    //        {
    //            // assigning the address to shipping fields
    //            txtCustomerName.Text = txtCustName.Text;
    //            txtAddress1.Text = lblAddress1.Text;
    //            txtAddress2.Text = lblAddress2.Text;
    //            txtAddress3.Text = lblAddress3.Text;
    //            txtTelephone.Text = txtPhone.Text;
    //            txtCity.Text = lblCity.Text;
    //            txtState.Text = lblState.Text;
    //            txtZipCode.Text = lblZipCode.Text;
    //            txtCountry.Text = lblCountry.Text;
    //            txtEmail.Text = txtBEmail.Text;


    //            // assigning left panel Shipping  address
    //            LblSAName.Text = txtCustName.Text;
    //            LblSAAddress1.Text = lblAddress1.Text;
    //            LblSAAddress2.Text = lblAddress2.Text;
    //            LblSACityStateZip.Text = lblCity.Text + ", " + lblState.Text + ", " + lblZipCode.Text;
    //            LblSACountry.Text = lblCountry.Text;
    //            LblSATelephone1.Text = txtTelephone.Text;
    //            LblSAEmail.Text = txtEmail.Text;
    //        }
    //        else
    //        {
    //            txtCustomerName.Text = txtCustName.Text;
    //            txtTelephone.Text = txtPhone.Text;

    //            // assigning shipping address from econnect
    //            var custometShipAddress = new ShippingAddress().GetCustometShipAddress(_connString);
    //            foreach (ShippingAddress shippingAddress in custometShipAddress)
    //            {
    //                txtAddress1.Text = shippingAddress.Address1;
    //                txtAddress2.Text = shippingAddress.Address2;
    //                txtAddress3.Text = shippingAddress.Address3;
    //                txtCity.Text = shippingAddress.City;
    //                txtState.Text = shippingAddress.State;
    //                txtZipCode.Text = shippingAddress.Zipcode;
    //                txtCountry.Text = shippingAddress.Country;
    //                txtEmail.Text = shippingAddress.Email;

    //                // assigning left panel shipping  address
    //                LblSAName.Text = txtCustName.Text;
    //                LblSAAddress1.Text = shippingAddress.Address1;
    //                LblSAAddress2.Text = shippingAddress.Address2;
    //                LblSACityStateZip.Text = shippingAddress.City + ", " + shippingAddress.State + ", " + shippingAddress.Zipcode;
    //                LblSACountry.Text = shippingAddress.Country;
    //                LblSATelephone1.Text = shippingAddress.Telephone1;
    //                LblSAEmail.Text = shippingAddress.Email;

    //                break;
    //            }
    //        }

    //        // assigning left panel billing  address
    //        LblBName.Text = txtCustName.Text;
    //        LblBAddress1.Text = lblAddress1.Text;
    //        LblBAddress2.Text = lblAddress2.Text;
    //        LblBCityStateZip.Text = lblCity.Text + ", " + lblState.Text + ", " + lblZipCode.Text;
    //        LblBCountry.Text = lblCountry.Text;
    //        LblBTelephone1.Text = txtTelephone.Text;
    //        LblBEmail.Text = txtEmail.Text;

    //        return true;
    //    }

    //    return false;
    //}

    //protected void btnBillContinue_Click(object sender, EventArgs e)
    //{
    //    if (LoadCustomerAddressFromEconnect())
    //    {
    //        UpdateSessionVariables();
    //        hidAccordionIndex.Value = "1";
    //        lblError.Text = string.Empty;
    //    }
    //    else
    //    {
    //        lblError.Text = "The given customer details are not found. Please select the customer for billing address.";
    //        hidAccordionIndex.Value = "0";
    //    }
    //}

    #endregion
}