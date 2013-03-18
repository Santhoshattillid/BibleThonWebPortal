using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using Biblethon.Controller;

public partial class Biblethon_OrderDetails : System.Web.UI.Page
{
    private readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();
    public List<BillingAddress> Billing;
    public List<ShippingAddress> ShippingAddress;
    DataTable _dtCustomer;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
           // DataBind();
    }


    public void RadGrid1_DataBinding(object sender, EventArgs e)
    {
        DataBind();
    }

    private void DataBind()
    {
        Billing = new BillingAddress().GetCustomerDetails(_connString);
        ShippingAddress = new ShippingAddress().GetCustometShipAddress(_connString);
        _dtCustomer = ToDataTable(Billing);
        RadGrid1.DataSource = _dtCustomer;
        RadGrid1.Rebind();
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
}