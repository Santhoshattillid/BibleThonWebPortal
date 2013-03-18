using System;
using System.Collections.Generic;
using System.Configuration;
using AlbaDL;

public partial class ShareAThonAddCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        var econnectModel = new EConnectModel();
        try
        {
            lblError.Text = string.Empty;

            // validation for billing address elements
            if (string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
            {
                lblError.Text = "Customer name could not be empty";
                return;
            }

            // validation for shipping address elements
            if (string.IsNullOrEmpty(txtCustomerNumber.Text.Trim()))
            {
                lblError.Text = "Customer Number  could not be empty";
                return;
            }

            if (string.IsNullOrEmpty(txtAddressCode.Text.Trim()))
            {
                lblError.Text = "AddressCode could not be empty";
                return;
            }

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
                CreditExpiryDate = txtExpirationDate.Text.Trim(),
                UpdateIfExists = 2 // updating if it already exists
            };
            var customerInetInfo = new CustomerInetInfo
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
            };

            var listcustomerAddressInet = new List<CustomerInetInfo> { customerInetInfo };

            string fileName = Server.MapPath("~/Customer.xml");
            var connectionString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();
            if (econnectModel.SerilizeCustomerObject(fileName, connectionString, customer, listcustomerAddressInet))
                lblError.Text = "Customer created successfully.";
            else
                lblError.Text = "Ooops something went wrong in our server. We are not able to process your request.";
        }
        catch
        {
            lblError.Text = "Ooops something went wrong in our server. We are not able to process your request.";
        }
    }
}