using System;
using System.Configuration;
using System.Linq;
using Biblethon.Controller;

public partial class Biblethon_BillingAddress : System.Web.UI.Page
{
    private readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            DataBind();   
    }

    public void RadGrid1_DataBinding(object sender, EventArgs e)
    {
        DataBind();
    }

    private void DataBind()
    {
        string name;
        string telephone;
        try
        {
            name =    Request.QueryString["name"];
        }catch (Exception )
        {
            name = string.Empty;
        }

        try
        {
            telephone = Request.QueryString["telephone"];
        }
        catch (Exception)
        {
            telephone = string.Empty;
        }

        var customerAddress = new BillingAddress().GetCustomerDetails(_connString);
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(telephone))
        {
            RadGrid1.DataSource = (from c in customerAddress
                                   where (c.CustomerName.StartsWith(name) || c.CustomerName.StartsWith(name) || c.CustomerName.StartsWith(name) ) &&
                                        (c.Telephone1.Equals(telephone) || c.Telephone2.Equals(telephone) || c.Telephone3.Equals(telephone))
                                   select new
                                              {
                                                  CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                                  Name = c.CustomerName,
                                                  Address1 = c.Address1,
                                                  Address2 = c.Address2,
                                                  Address3 = c.Address3,
                                                  City = c.City,
                                                  AddressCode = c.AddressCode,
                                                  State = c.State,
                                                  Country = c.City,
                                                  Zipcode = c.Zipcode,
                                                  Telephone1 = c.Telephone1,
                                                  Telephone2 = c.Telephone2,
                                                  Email = c.Email
                                              });
        }
        else if(!string.IsNullOrEmpty(telephone))
        {
            RadGrid1.DataSource = (from c in customerAddress
                                   where c.Telephone1.Equals(telephone) || c.Telephone2.Equals(telephone) || c.Telephone3.Equals(telephone)
                                   select new
                                   {
                                       CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                       Name = c.CustomerName,
                                       Address1 = c.Address1,
                                       Address2 = c.Address2,
                                       Address3 = c.Address3,
                                       City = c.City,
                                       AddressCode = c.AddressCode,
                                       State = c.State,
                                       Country = c.City,
                                       Zipcode = c.Zipcode,
                                       Telephone1 = c.Telephone1,
                                       Telephone2 = c.Telephone2,
                                       Email = c.Email
                                   });
        }
        else if(!string.IsNullOrEmpty(name))
        {
            RadGrid1.DataSource = (from c in customerAddress
                                   where c.CustomerName.StartsWith(name) || c.CustomerName.StartsWith(name) || c.CustomerName.StartsWith(name)
                                   select new
                                   {
                                       CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                       Name = c.CustomerName,
                                       Address1 = c.Address1,
                                       Address2 = c.Address2,
                                       Address3 = c.Address3,
                                       City = c.City,
                                       AddressCode = c.AddressCode,
                                       State = c.State,
                                       Country = c.City,
                                       Zipcode = c.Zipcode,
                                       Telephone1 = c.Telephone1,
                                       Telephone2 = c.Telephone2,
                                       Email = c.Email
                                   });
        }
        else
        {
            RadGrid1.DataSource = (from c in customerAddress
                                   select new
                                   {
                                       CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                       Name = c.CustomerName,
                                       Address1 = c.Address1,
                                       Address2 = c.Address2,
                                       Address3 = c.Address3,
                                       City = c.City,
                                       AddressCode = c.AddressCode,
                                       State = c.State,
                                       Country = c.City,
                                       Zipcode = c.Zipcode,
                                       Telephone1 = c.Telephone1,
                                       Telephone2 = c.Telephone2,
                                       Email = c.Email
                                   });
        }
        RadGrid1.Rebind();
    }
}