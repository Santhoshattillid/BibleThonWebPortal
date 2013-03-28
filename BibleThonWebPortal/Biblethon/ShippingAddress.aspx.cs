using System;
using System.Linq;
using AlbaBL;

namespace Biblethon
{
    public partial class ShippingAddress : System.Web.UI.Page
    {
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
                GridDataBind();
        }

        /// <summary>
        /// Grids the shipping address data binding.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void GridShippingAddressDataBinding(object sender, EventArgs e)
        {
            GridDataBind();
        }

        /// <summary>
        /// Grids the data bind.
        /// </summary>
        private void GridDataBind()
        {
            string customerNo = Request.QueryString["CustomerNo"] ?? string.Empty;

            // assigning shipping address from econnect
            var custometShipAddress = new GPEconnect().GetCustomerShipAddress(customerNo);
            if (!string.IsNullOrEmpty(customerNo))
            {
                GridShippingAddress.DataSource = (from c in custometShipAddress
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
            GridShippingAddress.Rebind();
        }
    }
}