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

                foreach (var record in (from c in customerAddress
                                              where c.CustomerNo.ToLower().Equals(id.ToLower())
                                              select c))
                {
                    HdnCreditCardNumber.Value = record.CreditCardNumber;
                    HdnExpireDate.Value = record.CreditCardExpireDate;
                    break;
                }

                var result = (from c in customerAddress
                              where c.CustomerNo.ToLower().Equals(id.ToLower())
                              select new
                              {
                                  CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                  Name = c.CustomerName,
                                  Address1 = c.Address1,
                                  Address2 = c.Address2,
                                  City = c.City,
                                  State = c.State,
                                  Country = c.Country,
                                  Zipcode = c.Zipcode,
                                  Telephone1 = c.Telephone1,
                                  Email = c.Email,
                              });

                RadGridAddress.DataSource = result;
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
            name =    Request.QueryString["name"].Trim();
        }catch (Exception )
        {
            name = string.Empty;
        }

        try
        {
            telephone = Request.QueryString["telephone"].Trim().ToLower().Replace("�","").Replace("(","").Replace(")","").Replace("ext.","").Replace(" ","").Replace("-","");
        }
        catch (Exception)
        {
            telephone = string.Empty;
        }

        var customerAddress = new BillingAddress().GetCustomerDetails(_connString,name,telephone);

        RadGridCustomerIds.DataSource = (from c in customerAddress
                                         select new
                                                    {
                                                        CustomerNo = "<a href='BillingAddress.aspx?Id=" + c.CustomerNo + "' class=''>" + c.CustomerNo + "</a>",
                                                        Name = c.CustomerName,
                                                        Address1 = c.Address1,
                                                        Address2 = c.Address2,
                                                        City = c.City,
                                                        State = c.State,
                                                        Country = c.Country,
                                                        //Zipcode = c.Zipcode,
                                                        Telephone1 = c.Telephone1,
                                                        //Email = c.Email
                                                    });

        RadGridCustomerIds.Rebind();
    }
}