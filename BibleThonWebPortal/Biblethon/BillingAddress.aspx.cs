﻿using System;
using System.Linq;
using AlbaBL;

namespace Biblethon
{
    public partial class BiblethonBillingAddress : System.Web.UI.Page
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
            {
                var id = Request.QueryString["Id"] ?? string.Empty;

                if (string.IsNullOrEmpty(id))
                {
                    LoadCustomerDetails();
                    RadPanelAddress.Visible = false;
                }
                else
                {
                    // loading list of billing addresses
                    RadPanelCustomerIds.Visible = false;
                    var customerAddress = new GPEconnect().GetCustomerBillingAddresses();

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

        /// <summary>
        /// Handles the DataBinding event of the RadGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void RadGridCustomerIdsDataBinding(object sender, EventArgs e)
        {
            LoadCustomerDetails();
        }

        /// <summary>
        /// Loads the customer details.
        /// </summary>
        private void LoadCustomerDetails()
        {
            var name = Request.QueryString["name"] ?? string.Empty;
            var telephone = RemoveFormat(Request.QueryString["telephone"] ?? string.Empty) ?? string.Empty;

            var customerAddress = new GPEconnect().GetCustomerDetails(name.Trim(), telephone);

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

        /// <summary>
        /// Removes the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string RemoveFormat(string value)
        {
            return value.Trim().ToLower().Replace("�", "").Replace("(", "").Replace(")", "").Replace("ext.", "").Replace(" ", "").Replace("-", "");
        }
    }
}