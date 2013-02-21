using System;
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
            Session["orderNumber"] = new EConnectModel().GetNextSalseDocNumber(_connString);
            lblOrderNo.Text = Session["orderNumber"].ToString();
            lblOrderNum.Text = Session["orderNumber"].ToString();
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
    }

    protected void btnBillContinue_Click(object sender, EventArgs e)
    {
        var customerAddress = new BillingAddress().GetCustomerDetails(_connString);
        var address = (from c in customerAddress
                       where
                           (c.CustomerName.StartsWith(txtCustName.Text) || c.CustomerName.StartsWith(txtCustName.Text) ||
                            c.CustomerName.StartsWith(txtCustName.Text)) &&
                           (c.Telephone1.Equals(txtTelephone.Text) || c.Telephone2.Equals(txtTelephone.Text) ||
                            c.Telephone3.Equals(txtTelephone.Text))
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

            hidAccordionIndex.Value = "1";
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
        }
        else
            lblError.Text = "The given customer details are not found. Please select the customer for billing address.";
    }

    protected void btnShipContinue_Click(object sender, EventArgs e)
    {
        var offerLines = new OfferLines().GetOfferLines(_connString);
        gdvOfferLine.DataSource = offerLines;
        gdvOfferLine.DataBind();
        hidAccordionIndex.Value = "2";

        // assigning left panel shipping address
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
        lblOrderNum.Text = "0";
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
            foreach (DataRow row in dtSearch.Rows)
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
                break;
            }
        }
    }

    public OrderProcess GetCustomerHeader()
    {
        var orderProcess = new OrderProcess
        {
            SOPTYPE = 2,
            SOPNUMBE = Session["orderNumber"].ToString().Trim(),
            BACHNUMB = "BIBLETHON",
            DOCID = "STDORD",
            CUSTNMBR = hidCustId.Value,
            CUSTNAME = txtCustName.Text,
            SUBTOTAL = Convert.ToDecimal(lblTotal.Text.Substring(1)),
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
            FREIGHT = string.IsNullOrEmpty(txtShipping.Text) ? 0 : Convert.ToDecimal(txtShipping.Text),
            FRTTXAMT = 0,
            MISCAMNT = string.IsNullOrEmpty(txtADonation.Text) ? 0 : Convert.ToDecimal(txtADonation.Text),
            MSCTXAMT = 0,
            TRDISAMT = 0,
            TAXAMNT = 0
        };
        orderProcess.DOCAMNT = Convert.ToDecimal(orderProcess.SUBTOTAL + orderProcess.FREIGHT + orderProcess.MISCAMNT + orderProcess.MSCTXAMT + orderProcess.TAXAMNT + orderProcess.FRTTXAMT) - Convert.ToDecimal(orderProcess.TRDISAMT);
        return orderProcess;
    }

    public List<OrderItems> GetOrderedItems()
    {
        return (from GridViewRow row in gdvOfferLine.Rows
                let itemQty = (TextBox)row.FindControl("TXTQty")
                where Convert.ToDecimal(itemQty.Text) > 0
                let itemNumber = (LinkButton)row.FindControl("OfferId")
                let itemDescription = gdvOfferLine.Rows[row.RowIndex].Cells[1].Text
                let itemPrice = (Label)row.FindControl("lblPrice")
                let itemXTNDPRCE = (TextBox)row.FindControl("LBLSubTotal")
                select new OrderItems
                {
                    SOPTYPE = 2,
                    SOPNUMBE = Session["orderNumber"].ToString().Trim(),
                    CUSTNMBR = hidCustId.Value,
                    DOCDATE = DateTime.Now.ToShortDateString(),
                    ITEMNMBR = itemNumber.Text,
                    ITEMDESC = itemDescription,
                    UNITPRCE = Convert.ToDecimal(itemPrice.Text.Substring(1)),
                    XTNDPRCE = Convert.ToDecimal(itemXTNDPRCE.Text.Substring(1)),
                    QUANTITY = Convert.ToDecimal(itemQty.Text),
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
}