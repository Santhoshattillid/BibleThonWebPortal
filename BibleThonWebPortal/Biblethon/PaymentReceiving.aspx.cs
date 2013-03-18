using System;
using AlbaDL;

public partial class Biblethon_PaymentReceiving : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var response = new AuthorizeNet.SIMResponse(Request.Form);
        var isValid = response.Validate(Utilities.MerchantHash, Utilities.ApiLoginID);
        if (!isValid)
            Response.Redirect("OrderEntry.aspx?Status=failed&Message=Invalid operation has been done.");
        else if (response.ResponseCode == "1")
        {
            // for success transactions, redirecting to order details page
            Response.Redirect("OrderDetails.aspx");
        }
        else
        {
            var returnUrl = "/OrderEntry.aspx?Status=succeeded&Message" + response.Message;
            Response.Redirect(returnUrl);
        }
    }
}