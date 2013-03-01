using System;
using System.Configuration;
using System.Linq;

namespace Biblethon
{
    public partial class ShippingAddress : System.Web.UI.Page
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
            string customerNo;
            try
            {
                customerNo = Request.QueryString["CustomerNo"];
            }catch (Exception )
            {
                customerNo = string.Empty;
            }

            // assigning shipping address from econnect
            var custometShipAddress = new global::ShippingAddress().GetCustometShipAddress(_connString);
            if (!string.IsNullOrEmpty(customerNo) )
            {
                RadGrid1.DataSource = (from c in custometShipAddress
                                       where c.CustomerNo.ToLower().Equals(customerNo.ToLower())  
                                       select new
                                                  {
                                                      CustomerNo = "<a href='#' class='CustomerNoLink'>" + c.CustomerNo + "</a>",
                                                      Address1 = c.Address1,
                                                      Address2 = c.Address2,
                                                      City = c.City,
                                                      AddressCode = c.AddressCode,
                                                      State = c.State,
                                                      Country = c.Country,
                                                      Zipcode = c.Zipcode,
                                                      Telephone1 = c.Telephone1,
                                                      Email = c.Email
                                                  });
            }
            
            RadGrid1.Rebind();
        }
    }
}