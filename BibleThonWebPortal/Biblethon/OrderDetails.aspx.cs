using System;
using System.Linq;
using AlbaBL;
using AlbaDL;
using Resources;

namespace Biblethon
{
    public partial class BiblethonOrderDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] == null && !Utilities.DevelopmentMode)
                Response.Redirect("Logout.aspx");

            if (Session["Status"] != null)
            {
                if (Session["Status"].ToString() == "Success")
                {
                    string orderNo = string.Empty;
                    var econnectModel = new EConnectModel();
                    try
                    {
                        // pushing order into GP
                        orderNo = econnectModel.GetNextSalseDocNumber(Utilities._connString).Trim();

                        var sessionOrderDetails = (SessionOrderDetails)Session["OrderDetails"];

                        // setting the transaction code to move into GP
                        sessionOrderDetails.CardDetails.AuthorizeCode = Session["AuthorizeCode"] != null ? Session["AuthorizeCode"].ToString() : string.Empty;

                        // updating the new orderno for order items
                        foreach (OrderItems orderItems in sessionOrderDetails.ListOrders)
                        {
                            orderItems.SOPNUMBE = orderNo;
                        }

                        // updating the new orderno for OrderProcess item
                        sessionOrderDetails.OrderProcess.SOPNUMBE = orderNo;

                        string fileName = Server.MapPath("~/SalesOrder.xml");

                        if (new EConnectModel().SerializeSalesOrderObject(fileName,
                                                                          Utilities._connString,
                                                                          sessionOrderDetails.OrderProcess,
                                                                          sessionOrderDetails.ListOrders,
                                                                          sessionOrderDetails.CardDetails))
                        {
                            // updating our customer database
                            var twoEntities = new TWOEntities();
                            var orderDetail = new OrderDetail
                                                  {
                                                      OrdNo = orderNo,
                                                      OrdDate = DateTime.Now,
                                                      Status = "Work",
                                                      CustomerName = sessionOrderDetails.CustomerName,
                                                      Operator =
                                                          Session["LoggedInUser"] != null
                                                              ? Session["LoggedInUser"].ToString()
                                                              : "",
                                                      OrdTotal = sessionOrderDetails.OrderGrandTotal,
                                                      Id = twoEntities.OrderDetails.Max(od => od.Id) + 1
                                                  };

                            twoEntities.AddToOrderDetails(orderDetail);
                            twoEntities.SaveChanges();

                            LblError.Text = @"The Order [" + orderNo + @"]  has been processed successfully.";
                            LblError.CssClass = "Success";
                        }
                    }
                    catch (Exception)
                    {
                        // roll back of order number here
                        if (!string.IsNullOrEmpty(orderNo))
                            econnectModel.RollbackSalseDocNumber(Utilities._connString, orderNo);

                        LblError.Text = Resource.UN_EXPECTED_ERROR + " , but the payment transaction was made successfully.";
                        LblError.CssClass = "error errorinfo";
                    }
                }
                else
                {
                    LblError.Text = Session["Message"] != null ? Session["Message"].ToString() : Resource.ORDER_ENTRY_ERROR;
                    LblError.CssClass = "error errorinfo";
                }
            }
            else
                Response.Redirect("OrderEntry.aspx");
        }
    }
}