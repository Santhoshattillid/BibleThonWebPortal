using System;
using System.Collections.Generic;
using System.Globalization;
using AlbaBL;
using AlbaDL;
using Resources;

namespace ShareAThon
{
    public partial class ShareAThonAddCustomer : System.Web.UI.Page
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
        }

        /// <summary>
        /// Handles the Click event of the btnSaveOrder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void BtnSaveOrderClick(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                // validation for billing address
                if (string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
                {
                    lblError.Text = Resource.CustomerNameEmptyErrorString;
                    return;
                }

                // validation for shipping address
                if (string.IsNullOrEmpty(txtCustomerNumber.Text.Trim()))
                {
                    lblError.Text = Resource.CustomerNumberEmptyErrorString;
                    return;
                }

                // validation for address code
                if (string.IsNullOrEmpty(txtAddressCode.Text.Trim()))
                {
                    lblError.Text = Resource.AddressCodeEmptyErrorString;
                    return;
                }

                // validation for address 1
                if (string.IsNullOrEmpty(txtAddress1.Text.Trim()))
                {
                    lblError.Text = Resource.Address1EmptyErrorString;
                    return;
                }

                // validation for address 2
                if (string.IsNullOrEmpty(txtAddress2.Text.Trim()))
                {
                    lblError.Text = Resource.Address2EmptyErrorString;
                    return;
                }

                // try parsing credit card expire date
                DateTime creditCardExpire;

                DateTime.TryParse(txtExpirationDate.Text.Substring(0, 2) + "/01/" + txtExpirationDate.Text.Substring(2, 2), out creditCardExpire);

                // initiating customer process instance
                var customerProcess = new CustomerProcess();

                // verifying whether the customer exists with the same name
                if (customerProcess.UserExists(txtCustomerNumber.Text.Trim()))
                {
                    lblError.Text = Resource.CUSTOMER_EXIST;
                }
                else
                {
                    // instantiating new customer for creation
                    var customer = new NewCustomer
                                       {
                                           CustomerNumber = txtCustomerNumber.Text.Trim(),
                                           CustomerName = txtCustomerName.Text.Trim(),
                                           AddressCode = txtAddressCode.Text.Trim(),
                                           Address1 = txtAddress1.Text.Trim(),
                                           Address2 = txtAddress2.Text.Trim(),
                                           Address3 = txtAddress3.Text.Trim(),
                                           City = txtCity.Text.Trim(),
                                           State = txtState.Text.Trim(),
                                           ZipCode = txtZipCode.Text.Trim(),
                                           Country = txtCountry.Text.Trim(),
                                           PhoneNumber1 = txtTelephone.Text.Trim(),
                                           PhoneNumber2 = string.Empty,
                                           Fax = txtFax.Text.Trim(),
                                           CreditCardId = drpCreditCardName.SelectedValue,
                                           CreditCardNumber = txtCreditCardNo.Text.Trim(),
                                           CreditExpiryDate = creditCardExpire.ToString(CultureInfo.InvariantCulture),
                                           UpdateIfExists = 2 // updating if it already exists
                                       };

                    // customer internet information includes email...
                    var listcustomerAddressInet = new List<CustomerInetInfo> { new CustomerInetInfo
                                                                                   {
                                                                                       MasterId = customer.CustomerNumber,
                                                                                       MasterType = "CUS",
                                                                                       AddressCode = customer.AddressCode,
                                                                                       IntenetInfo1 = txtEmail.Text.Trim(),
                                                                                       IntenetInfo2 = string.Empty,
                                                                                       IntenetInfo3 = string.Empty,
                                                                                       IntenetInfo4 = string.Empty,
                                                                                       EmailToAddress = string.Empty,
                                                                                       EmailBccAddress = string.Empty,
                                                                                       EmailCcAddress = string.Empty
                                                                                   } };

                    // xml file path
                    string fileName = Server.MapPath("~/Customer.xml");

                    bool isCustomerCreated;

                    // executing customer creation process
                    customerProcess.CreateCustomer(fileName, customer, listcustomerAddressInet, true, out isCustomerCreated);

                    // validating the success of the process
                    if (isCustomerCreated)
                    {
                        lblError.Text = Resource.CUSTOMER_CREATION_SUCCESS;

                        // script to move the customer values to parent window
                        HdnScriptSource.Text = @"<script src='Scripts/SelectCustomer.js' type='text/javascript'></script>";
                    }
                    else
                        lblError.Text = Resource.CUSTOMER_CREATION_ERROR;
                }
            }
            catch
            {
                lblError.Text = Resource.CUSTOMER_CREATION_ERROR;
            }
        }
    }
}