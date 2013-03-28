using System;
using System.Linq;
using AlbaBL;
using AlbaDL;

namespace Biblethon
{
    public partial class BiblethonPaymentReceiving : System.Web.UI.Page
    {
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] == null && !Utilities.DevelopmentMode)
                Response.Redirect("Logout.aspx");

            var response = new AuthorizeNet.SIMResponse(Request.Form);

            var isValid = response.Validate(Utilities.MerchantHash, Utilities.ApiLoginID);

            if (!isValid) // && !Utilities.DevelopmentMode)
            {
                Session["Status"] = "Failed";
                Session["Message"] = "An un-authorized transactions has been detected and declined.";
            }
            else if (response.ResponseCode == "1") // || Utilities.DevelopmentMode)
            {
                // for success transactions, redirecting to order details page
                Session["Status"] = "Success";
                Session["Message"] = String.Empty;

                // authorization code needs to save into GP against to the order
                Session["AuthorizeCode"] = response.AuthorizationCode;

                // saving the entire transaction into custome Table
                var twoEntities = new TWOEntities();
                var authorizeNetTransaction = new AuthorizeNetTransaction
                                                  {
                                                      Amount = response.Amount,
                                                      AuthorizationCode = response.AuthorizationCode,
                                                      Approved = response.Approved,
                                                      CardNumber = response.CardNumber,
                                                      InvoiceNumber = response.InvoiceNumber,
                                                      Message = response.Message,
                                                      TransactionID = response.TransactionID,
                                                      Id = twoEntities.AuthorizeNetTransactions.Max(ant => ant.Id) + 1
                                                  };

                // adding record
                twoEntities.AddToAuthorizeNetTransactions(authorizeNetTransaction);

                // updating database
                twoEntities.SaveChanges();
            }
            else
            {
                // for other than success, we will need to show messsage to the page
                Session["Status"] = "Failed";
                Session["Message"] = response.Message;
            }
            Response.Redirect("OrderDetails.aspx");
        }
    }
}