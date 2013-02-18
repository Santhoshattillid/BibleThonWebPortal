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
    DataTable _dtCustomer, _dtShip;
    DataRow[] _dtFilterRows;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["orderNumber"] = new EConnectModel().GetNextSalseDocNumber(_connString);
            lblOrderNo.Text = Session["orderNumber"].ToString();
            BindCustomerDetails();
            BindOfferLines();
            BindCustomerShipDetails();
            GetDataVisibleFalse();
        }
    }

    private void BindCustomerDetails()
    {
        CustomerAddress = new BillingAddress().GetCustomerDetails(_connString);
        _dtCustomer = ToDataTable(CustomerAddress);
        Session["customerAddress"] = _dtCustomer;
        gvCustomers.DataSource = _dtCustomer;
        gvCustomers.DataSource = CustomerAddress;
        gvCustomers.DataBind();
    }

    private void BindCustomerShipDetails()
    {
        ShippingAddress = new ShippingAddress().GetCustometShipAddress(_connString);
        _dtShip = ToDataTable(ShippingAddress);
        Session["ShippingAddress"] = _dtShip;
    }

    private void BindOfferLines()
    {
        OfferLines = new OfferLines().GetOfferLines(_connString);
        Session["OfferLines"] = OfferLines;
        gdvOfferLine.DataSource = OfferLines;
        gdvOfferLine.DataBind();
    }

    private void BindGridview()
    {
        gvCustomers.DataSource = Session["customerAddress"];
        gvCustomers.DataBind();
    }

    protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomers.PageIndex = e.NewPageIndex;
        BindGridview();
        MPEGridview.Show();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string customerName = Request.Form["txtName"];
            string address;
            if (Request.Form["ddlOption"] == "1")
            {
                address = "Address1='" + Request.Form["txtAddAndPh"] + "'";
            }
            else
            {
                address = "Telephone1='" + Request.Form["txtAddAndPh"] + "'";
            }
            gvCustomers.DataSource = GetFilterData(customerName, address);
            gvCustomers.DataBind();
            MPEGridview.Show();
        }
        catch (Exception)
        {
        }
    }

    public DataTable GetFilterData(string customerName, string address)
    {
        var dtSession = Session["customerAddress"] as DataTable;
        if (dtSession != null)
        {
            DataView dvSession = dtSession.DefaultView;
            string filterExprssion = "CustomerName like'" + customerName + "%' And " + address;
            //string filterExprssion = "CustomerName like'" + customerName + "%'";
            _dtFilterRows = dvSession.Table.Select(filterExprssion);
            DataTable dtNewFilterData = dtSession.Clone();
            foreach (DataRow drNew in _dtFilterRows)
            {
                //if (dtNewFilterData.Rows[0]["CustomerName"].ToString() != "Service")
                dtNewFilterData.ImportRow(drNew);
            }
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
            //string filterExprssion = "CustomerName like'" + customerName + "%' and " + address;
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

    protected void imgSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string customerName = txtCustName.Text;
            string address = "Telephone1='" + txtPhone.Text + "'";
            DataTable dtSearch = GetFilterData(customerName, address);
            GetCustomerData(dtSearch);
        }
        catch (Exception)
        {
        }
    }

    public void GetCustomerData(DataTable dtSearch)
    {
        if (dtSearch.Rows.Count == 1)
        {
            //foreach (DataRow row in dtSearch.Rows)
            //{
            txtCustName.Text = dtSearch.Rows[0]["CustomerName"].ToString();
            lblAddress1.Text = dtSearch.Rows[0]["Address1"].ToString();
            lblAddress2.Text = dtSearch.Rows[0]["Address2"].ToString();
            lblAddress3.Text = dtSearch.Rows[0]["Address3"].ToString();
            txtPhone.Text = dtSearch.Rows[0]["Telephone1"].ToString();
            lblCity.Text = dtSearch.Rows[0]["City"].ToString();
            lblState.Text = dtSearch.Rows[0]["State"].ToString();
            lblZipCode.Text = dtSearch.Rows[0]["Zipcode"].ToString();
            lblCountry.Text = dtSearch.Rows[0]["Country"].ToString();
            txtBEmail.Text = dtSearch.Rows[0]["Email"].ToString();
            hidAddressCode.Value = dtSearch.Rows[0]["AddressCode"].ToString();
            //}
            GetDataVisibleTrue();
        }
        else
        {
            MPEGridview.Show();
        }
    }

    private void GetDataVisibleFalse()
    {
        tr1.Visible = false;
        tr2.Visible = false;
        tr3.Visible = false;
        tr4.Visible = false;
        tr5.Visible = false;
        tr6.Visible = false;
        tr7.Visible = false;
    }

    private void GetDataVisibleTrue()
    {
        tr1.Visible = true;
        tr2.Visible = true;
        tr3.Visible = true;
        tr4.Visible = true;
        tr5.Visible = true;
        tr6.Visible = true;
        tr7.Visible = true;
    }

    protected void btnBillContinue_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "1";
        if (cbShipping.Checked)
        {
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
        }
        else
        {
            txtCustomerName.Text = txtCustName.Text;
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtTelephone.Text = txtPhone.Text;
            txtCity.Text = "";
            txtState.Text = "";
            txtZipCode.Text = "";
            txtCountry.Text = "";
            txtEmail.Text = "";
        }
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        try
        {
            string customerNo = hidCustId.Value;
            DataTable dtSearch = GetFilterDataBYId(customerNo);
            GetCustomerData(dtSearch);
        }
        catch (Exception)
        { }
    }

    protected void btnShipContinue_Click(object sender, EventArgs e)
    {
        try
        {
            var dtShipping = Session["ShippingAddress"] as DataTable;
            if (dtShipping != null)
            {
                DataView dvSession = dtShipping.DefaultView;
                if (cbShipping.Checked == false)
                {
                    string filterExprssion = "CustomerNo='" + hidCustId.Value + "' And Zipcode='" + txtZipCode.Text + "' and Telephone1='" + txtTelephone.Text + "'";
                    //string filterExprssion = "CustomerName like'" + customerName + "%'";
                    _dtFilterRows = dvSession.Table.Select(filterExprssion);
                    if (_dtFilterRows.Length == 0)
                    {
                        lblError.Visible = true;
                        lblError.Text = "Shipping address doesn't match, Please try again";
                    }
                    else
                    {
                        lblError.Visible = false;
                        DataTable dtNewFilterData = dtShipping.Clone();
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
        }
    }

    protected void btnConfirmOffer_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "3";
        lblOrderNum.Text = Session["orderNumber"].ToString();
        lblOrderNum.Text = "0";
        lblOrderTotal.Text = lblGrandTotal.Text;
    }

    protected void btncontinue4_Click(object sender, EventArgs e)
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
            ADDRESS3 = txtAddress3.Text,
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
                    ADDRESS3 = txtAddress3.Text,
                    CITY = txtCity.Text,
                    STATE = txtState.Text,
                    ZIPCODE = txtZipCode.Text,
                    COUNTRY = txtCountry.Text,
                    PHNUMBR1 = txtTelephone.Text
                }).ToList();
    }
}