using System;
using System.Configuration;
using System.Linq;

public partial class Biblethon_BillingAddress : System.Web.UI.Page
{
    private readonly string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id;
            try
            {
                id = Request.QueryString["Id"];
            }
            catch (Exception)
            {
                id = string.Empty;
            }
            if (string.IsNullOrEmpty(id))
            {
                DataBind();
                RadPanelAddress.Visible = false;

            }
            else
            {
                // loading list of billing addresses 
                RadPanelCustomerIds.Visible = false;
                var customerAddress = new BillingAddress().GetCustomerBillingAddresses(_connString);
                RadGridAddress.DataSource = (from c in customerAddress
                                                 where c.CustomerNo.ToLower().Contains(id.ToLower())
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
                RadGridAddress.DataBind();
            }
        }
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
            RadGridCustomerIds.DataSource = (from c in customerAddress
                                   where c.CustomerName.ToLower().Contains(name.ToLower())  &&
                                        c.Telephone1.ToLower().Contains(telephone.ToLower())
                                   select new
                                              {
                                                  CustomerNo = "<a href='BillingAddress.aspx?Id=" + c.CustomerNo + "' class=''>" + c.CustomerNo + "</a>",
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
            RadGridCustomerIds.DataSource = (from c in customerAddress
                                   where c.Telephone1.ToLower().Contains(telephone.ToLower())
                                   select new
                                   {
                                       CustomerNo = "<a href='BillingAddress.aspx?Id=" + c.CustomerNo + "' class=''>" + c.CustomerNo + "</a>",
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
            RadGridCustomerIds.DataSource = (from c in customerAddress
                                   where c.CustomerName.ToLower().Contains(name.ToLower())
                                   select new
                                   {
                                       CustomerNo = "<a href='BillingAddress.aspx?Id=" + c.CustomerNo + "' class=''>" + c.CustomerNo + "</a>",
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
            RadGridCustomerIds.DataSource = (from c in customerAddress
                                   select new
                                   {
                                       CustomerNo = "<a href='BillingAddress.aspx?Id=" + c.CustomerNo + "' class=''>" + c.CustomerNo + "</a>",
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
        RadGridCustomerIds.Rebind();
    }
}