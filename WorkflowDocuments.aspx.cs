using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Telerik.Web.UI;

namespace Alba.Workflow.WebPortal
{
    public partial class WorkflowDocuments : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string searchString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle Session Timeouts
            if (Session["LoggedInUser"] == null)
                Response.Redirect("Logout.aspx");

            RefreshData();

            if (!IsPostBack)
            {
                RadGrid2.Rebind();
                RadGrid3.Rebind();
                SelectFirstGridRow();

                //RadPane3.Collapsed = true;
            }



            if (!IsPostBack)
                LoadWorkflowDocumentTypes();
        }

        #region Methods

        private void SelectFirstGridRow()
        {
            GridDataItem firstDataItem = RadGrid2.Items.OfType<GridDataItem>().FirstOrDefault();
            //GridDataItem firstDataItem = RadGrid1.Items.OfType<GridDataItem>().FirstOrDefault();
            if (firstDataItem != null)
            {
                firstDataItem.Selected = true;

                string DocumentKey = firstDataItem["DocumentKey"].Text;
                Session["SelectedDocumentKey"] = DocumentKey;
            }
            else
                Session.Remove("SelectedDocumentKey");
        }

        private void ApplyWFInstanceGridFilter(string pSelectedNode)
        {

            switch (pSelectedNode)
            {
                case "Open":
                    Session["SelectedNode"] = pSelectedNode;
                    break;
                case "Past Due":
                    Session["SelectedNode"] = pSelectedNode;
                    break;
                case "Completed":
                    Session["SelectedNode"] = pSelectedNode;
                    break;
                case "Started By Me":
                    Session["SelectedNode"] = pSelectedNode;
                    break;
                default:
                    Session.Remove("SelectedNode");
                    break;
            }

            if (Session["SelectedNode"] != null)
                Response.Redirect("Default.aspx");

            //LinqDataSource3.Where = "Status == @Status AND Assigned_To == @Assigned_To";
        }

        private void DeleteGridRow()
        {
            #region OLD CODE -- Removed Deleting of workflow documents
            //GridDataItem item = RadGrid2.SelectedItems[0] as GridDataItem;

            //string authorizationNumber = item["DocumentKey"].Text;

            //ChargebackDataDataContext cb = new ChargebackDataDataContext();

            //var header = (from c in cb.AS_GAIAM_Chargeback_Headers
            //              where c.AuthorizationNumber == authorizationNumber
            //              select c).First();

            //IEnumerable<AS_GAIAM_Chargeback_Detail> cbDetails = (from c in cb.AS_GAIAM_Chargeback_Details
            //                                                     where c.AuthorizationNumber == authorizationNumber
            //                                                     select c).ToList();
            //try
            //{
            //    cb.AS_GAIAM_Chargeback_Headers.DeleteOnSubmit(header);
            //    cb.AS_GAIAM_Chargeback_Details.DeleteAllOnSubmit(cbDetails);
            //    cb.SubmitChanges();

            //    RadGrid2.Rebind();
            //    SelectFirstGridRow();
            //    RadGrid3.Rebind();
            //    RadGrid2.RaisePostBackEvent(null);
            //    if (RadGrid2.SelectedIndexes.Count > 0)
            //    {
            //        int selectedIndex = int.Parse(RadGrid2.SelectedIndexes[0]);

            //        if (selectedIndex < RadGrid2.Items.Count)
            //        {
            //            GridDataItem selectedRow = RadGrid2.Items[selectedIndex];
            //            string DocumentKey = selectedRow["DocumentKey"].Text;
            //            Session["SelectedDocumentKey"] = DocumentKey;
            //        }
            //    }

            //    RadGrid2.Rebind();
            //    RadGrid3.Rebind();
            //    SelectFirstGridRow();
            //}

            //catch (Exception ex)
            //{

            //}
            #endregion

        }

        private DataTable RetrieveWorkflowData(bool pIsAdmin, string pUserName)
        {
            DataTable dt = new DataTable();
            int wfCtr = 0;
            string sqlString = string.Empty;
            SqlConnection dbCompConn = OpenCompanyDBConnection();
            WorkflowDataContext wd = new WorkflowDataContext();

            // Retrieve viewnames from Workflow List
            var workflows = (from c in wd.AS_WF_WorkflowDocumentTypes
                             where c.IsWebBased == true
                             select c).ToList();

            if (workflows != null)
            {
                foreach (AS_WF_WorkflowDocumentType workflow in workflows)
                {
                    if (wfCtr == 0)
                    {
                        if (pIsAdmin || Helper.IsUserEditor(Session["LoggedInUser"].ToString(), workflow.DocType))
                            sqlString = "SELECT DISTINCT * FROM " + workflow.WorkflowDocViewName;
                        else
                            sqlString = "SELECT DISTINCT * FROM " + workflow.WorkflowDocViewName + " WHERE CreatedBy = '" + pUserName + "'";
                    }
                    else
                    {
                        if (pIsAdmin || Helper.IsUserEditor(Session["LoggedInUser"].ToString(), workflow.DocType))
                            sqlString = sqlString + " UNION SELECT DISTINCT * FROM " + workflow.WorkflowDocViewName;
                        else
                            sqlString = sqlString + " UNION SELECT DISTINCT * FROM " + workflow.WorkflowDocViewName + " WHERE CreatedBy = '" + pUserName + "'";
                    }

                    wfCtr += 1;
                }
            }

            // Retrieve Records from generated Query
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlString;
                cmd.Connection = dbCompConn;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dbCompConn.Open();

                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("RetrieveKey1Name", ex);
            }
            finally
            {
                if (dbCompConn.State == ConnectionState.Open)
                    dbCompConn.Close();
            }
            return dt;
        }

        private SqlConnection OpenCompanyDBConnection()
        {
            //string connString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
            string connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ConnectionString;
            SqlConnection dbConn = new SqlConnection(connString);

            return dbConn;
        }

        private void RefreshData()
        {
            if (Session["UserRole"].ToString().ToUpper() != "WORKFLOW ADMIN")
            {
                RadGrid2.DataSource = RetrieveWorkflowData(false, Session["LoggedInUser"].ToString());
            }
            else
            {
                RadGrid2.DataSource = RetrieveWorkflowData(true, Session["LoggedInUser"].ToString());
            }

        }

        private void LoadWorkflowDocumentTypes()
        {
            foreach (RadToolBarItem item in RadToolBar1.Items)
            {
                if (item.Text == "New")
                {
                    var rtDropDownNew = (RadToolBarDropDown)item;

                    var wd = new WorkflowDataContext();

                    var workflows = (from c in wd.AS_WF_WorkflowDocumentTypes
                                     where c.IsWebBased == true
                                     select c).ToList();

                    foreach (AS_WF_WorkflowDocumentType workflow in workflows)
                    {
                        var btn = new RadToolBarButton { Text = workflow.DocType };
                        rtDropDownNew.Buttons.Add(btn);
                        var btnSeparator = new RadToolBarButton { IsSeparator = true };
                        rtDropDownNew.Buttons.Add(btnSeparator);
                    }


                    var btnNew = new RadToolBarButton { Text = "Purchase Order Entry" };
                    rtDropDownNew.Buttons.Add(btnNew);
                    btnNew.NavigateUrl = "Biblethon/OrderEntry.aspx";
                    var btnSeparatorNew = new RadToolBarButton { IsSeparator = true };
                    rtDropDownNew.Buttons.Add(btnSeparatorNew);


                    rtDropDownNew.Buttons.RemoveAt(rtDropDownNew.Buttons.Count - 1);
                }
            }

        }

        #endregion

        #region Events

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            searchString = e.Argument.ToLower();
            RadGrid2.Rebind();
            RadGrid3.Rebind();
        }
        /*
        protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var context = new HelpDeskDataContext();
                e.Result = from message in context.Messages
                           where	message.Subject.ToLower().Contains(searchString) || 
                                    message.Body.ToLower().Contains(searchString) ||
                                    message.From.ToLower().Contains(searchString)
                           select message;
            }
        }
        */
        protected void RadGrid2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("SelectedDocumentKey");
            if (RadGrid2.SelectedIndexes.Count > 0)
            {
                int selectedIndex = int.Parse(RadGrid2.SelectedIndexes[0]);

                if (selectedIndex < RadGrid2.Items.Count)
                {
                    GridDataItem selectedRow = RadGrid2.Items[selectedIndex];
                    string DocumentKey = selectedRow["DocumentKey"].Text;
                    Session["SelectedDocumentKey"] = DocumentKey;
                }
            }
        }

        protected void RadGrid3_ItemDataBound(object sender, GridItemEventArgs e)
        {
            #region OLD CODE
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;

            //    Image img = (Image)item["Priority_Image"].FindControl("Image1");
            //    if (img != null)
            //    {
            //        img.AlternateText = "Priority: " + item["Importance"].Text.Trim();
            //        img.ImageUrl = "~/Images/" + item["Importance"].Text.Trim() + ".png";
            //    }
            //}
            #endregion
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            ApplyWFInstanceGridFilter(e.Node.Text);

            RadGrid2.Rebind();
            RadGrid3.Rebind();
            SelectFirstGridRow();
        }

        protected void RadToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            //Removed code. Everything will be handled thru client-side for faster access
        }

        protected void RadGrid2_DeleteCommand(object source, GridCommandEventArgs e)
        {
            #region OLD CODE -- Removed Deleting of workflow documents
            //string docType = (string)((GridDataItem)e.Item).GetDataKeyValue("DocType");
            //string docKey = (string)((GridDataItem)e.Item).GetDataKeyValue("DocumentKey");

            //switch (docType)
            //{
            //    case "GAIAM Chargeback Document":
            //        ChargebackDataDataContext cb = new ChargebackDataDataContext();

            //        var cbheader = (from c in cb.AS_GAIAM_Chargeback_Headers
            //                        where c.AuthorizationNumber == docKey
            //                        select c).First();

            //        IEnumerable<AS_GAIAM_Chargeback_Detail> cbdetails = (from c in cb.AS_GAIAM_Chargeback_Details
            //                                                           where c.AuthorizationNumber == docKey
            //                                                                select c).ToList();

            //        if (cbheader != null)
            //        {
            //            try
            //            {
            //                cb.AS_GAIAM_Chargeback_Headers.DeleteOnSubmit(cbheader);
            //                cb.AS_GAIAM_Chargeback_Details.DeleteAllOnSubmit(cbdetails);
            //                cb.SubmitChanges();

            //                RadWindowManager1.RadAlert("Record has been deleted.", 330, 100, "Notification", "");

            //                Response.Redirect("WorkflowDocuments.aspx");
            //            }
            //            catch (System.Exception)
            //            {
            //                //RadWindowManager1.RadAlert("There are no records to edit.", 330, 100, "Error", "");
            //            }
            //        }            
            //        break;
            //    case "Alere Home Monitoring Document":
            //        AlereHomeMonitoringDataDataContext hm = new AlereHomeMonitoringDataDataContext();

            //        var orderHeader = (from c in hm.AS_Alere_Orders
            //                        where c.OrderID == docKey
            //                        select c).First();

            //        IEnumerable<AS_Alere_OrderDetail> orderdetails = (from c in hm.AS_Alere_OrderDetails
            //                                                           where c.OrderID == docKey
            //                                                                select c).ToList();

            //        if (orderHeader != null)
            //        {
            //            try
            //            {
            //                hm.AS_Alere_Orders.DeleteOnSubmit(orderHeader);
            //                hm.AS_Alere_OrderDetails.DeleteAllOnSubmit(orderdetails);
            //                hm.SubmitChanges();

            //                RadWindowManager1.RadAlert("Record has been deleted.", 330, 100, "Notification", "");

            //                Response.Redirect("WorkflowDocuments.aspx");
            //            }
            //            catch (System.Exception)
            //            {
            //                //RadWindowManager1.RadAlert("There are no records to edit.", 330, 100, "Error", "");
            //            }
            //        }            
            //        break;
            //}
            #endregion
        }

        protected void RadGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RefreshData();
        }


        #endregion
    }
}
