using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace Alba.Workflow.WebPortal
{
    public partial class Home : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string searchString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle Session Timeouts
            if (Session["LoggedInUser"] == null || Session["ShouldUserChangePassword"] == null || Convert.ToBoolean(Session["ShouldUserChangePassword"]) == true)
                Response.Redirect("Logout.aspx");

            if (!IsPostBack)
            {
                if (Session["SelectedNode"] == null)
                    ApplyWFInstanceGridFilter("Open");
                else
                {
                    RadTreeNode node = RadTreeView1.FindNodeByText(Session["SelectedNode"].ToString());
                    if (node != null)
                    {
                        node.Selected = true;
                    }

                    ApplyWFInstanceGridFilter(Session["SelectedNode"].ToString());
                }

                //RadGrid1.Rebind();
                RadGrid2.Rebind();
                SelectFirstGridRow();

                lblUser.Value = Session["LoggedInUser"].ToString();
            }
            else
            {
                ApplyWFInstanceGridFilter(RadTreeView1.SelectedNode.Text);
            }
        }

        #region Methods

        private void SelectFirstGridRow()
        {
            GridDataItem firstDataItem = RadGrid2.Items.OfType<GridDataItem>().FirstOrDefault();
            //GridDataItem firstDataItem = RadGrid1.Items.OfType<GridDataItem>().FirstOrDefault();
            if (firstDataItem != null)
            {
                firstDataItem.Selected = true;

                string wfInstanceID = firstDataItem["WFInstanceID"].Text;
                Session["SelectedWFInstance"] = wfInstanceID;
            }
            else
                Session.Remove("SelectedWFInstance");
        }

        private void ApplyWFInstanceGridFilter(string pSelectedNode)
        {
            if (Session["LoggedInUser"] == null)
                Response.Redirect("Logout.aspx");

            switch (pSelectedNode)
            {
                case "Open":
                    Session["SelectedWFInstanceStatus"] = "Pending Action";
                    Session["SelectedWFInstanceDueDateStart"] = "01/01/1900";
                    Session["SelectedWFInstanceDueDateEnd"] = DateTime.Now;

                    UpdateGridData(pSelectedNode);

                    RadPane1.Collapsed = false;
                    RadSplitter1.Visible = true;

                    break;
                case "Past Due":
                    Session["SelectedWFInstanceStatus"] = "Pending Action";
                    Session["SelectedWFInstanceDueDateStart"] = "01/01/1900";
                    Session["SelectedWFInstanceDueDateEnd"] = DateTime.Now;

                    UpdateGridData(pSelectedNode);

                    // RadGrid2.DataSourceID = "LinqDataSource3";
                    RadPane1.Collapsed = false;
                    RadSplitter1.Visible = true;                   

                    break;
                case "Completed":
                    Session["SelectedWFInstanceStatus"] = "Completed";
                    Session["SelectedWFInstanceWorkflowStep"] = "End";

                    UpdateGridData(pSelectedNode);

                    RadPane1.Collapsed = false;
                    RadSplitter1.Visible = true;

                    //Disable the other toolbar items
                    foreach (RadToolBarItem tbItem in RadToolBar1.Items)
                    {
                        if (tbItem.Text != "View Document")
                            tbItem.Enabled = false;
                    }
                    

                    break;
                case "Started By Me":
                    Session["SelectedWFInstanceCreatedBy"] = Session["LoggedInUser"].ToString();
                    Session["SelectedWFInstanceStatus"] = "New";

                    UpdateGridData(pSelectedNode);

                    RadPane1.Collapsed = false;
                    RadSplitter1.Visible = true;

                    //Disable the other toolbar items
                    foreach (RadToolBarItem tbItem in RadToolBar1.Items)
                    {
                        if (tbItem.Text != "View Document")
                            tbItem.Enabled = false;
                    }

                    break;
                default:                    
                    RadSplitter1.Visible = false;
                    break;
            }

            //LinqDataSource3.Where = "Status == @Status AND Assigned_To == @Assigned_To";
        }

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString; }
        }

        protected void UpdateGridData(string pSelectedNode)
        {
            DataTable dt = new DataTable();

            string sqlQuery = string.Empty;
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand();

            switch (pSelectedNode)
            {
                case "Open":
                    if (Session["UserRole"].ToString().ToUpper() != "WORKFLOW ADMIN")
                    {
                        sqlQuery = "SELECT Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, " +
                        "CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID, LockedBy, UserDefinedField1, UserDefinedField2, " +
                        "UserDefinedField3, UserDefinedField4, UserDefinedField5 FROM AS_WF_VW_WorkflowTasks " +
                        "WHERE Status = @Status AND WorkflowStep <> 'Revision' AND DueDate >= @Due_DateStart AND DueDate >= @Due_DateEnd AND (AssignedTo = @Assigned_To ";

                        List<string> groupList = Helper.UserGroup(Session["LoggedInUser"].ToString());
                        int ctr = 0;

                        foreach (string s in groupList)
                        {
                            string strCtr = "@" + ctr;
                            sqlQuery += " OR AssignedTo = " + strCtr;
                            cmd.Parameters.AddWithValue(strCtr, s);

                            ctr += 1;
                        }

                        sqlQuery += ")";

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateStart", Session["SelectedWFInstanceDueDateStart"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateEnd", Session["SelectedWFInstanceDueDateEnd"].ToString());
                        cmd.Parameters.AddWithValue("@Assigned_To", Session["LoggedInUser"].ToString());
                    }
                    else
                    {
                        sqlQuery = "SELECT Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, " +
                        "CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID, LockedBy, UserDefinedField1, UserDefinedField2, " +
                        "UserDefinedField3, UserDefinedField4, UserDefinedField5  FROM AS_WF_VW_WorkflowTasks " +
                       "WHERE Status = @Status AND WorkflowStep <> 'Revision'  AND AssignedTo <> 'System' AND DueDate >= @Due_DateStart AND DueDate >= @Due_DateEnd ";

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateStart", Session["SelectedWFInstanceDueDateStart"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateEnd", Session["SelectedWFInstanceDueDateEnd"].ToString());
                    }
                    break;
                case "Past Due":
                    if (Session["UserRole"].ToString().ToUpper() != "WORKFLOW ADMIN")
                    {
                        sqlQuery = "SELECT Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, " +
                        "CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID, LockedBy, UserDefinedField1, UserDefinedField2, " +
                        "UserDefinedField3, UserDefinedField4, UserDefinedField5  FROM AS_WF_VW_WorkflowTasks " +
                        "WHERE Status = @Status AND WorkflowStep <> 'Revision'  AND (AssignedTo = @Assigned_To ";

                        List<string> groupList = Helper.UserGroup(Session["LoggedInUser"].ToString());
                        int ctr = 0;

                        foreach (string s in groupList)
                        {
                            string strCtr = "@" + ctr;
                            sqlQuery += " OR AssignedTo = " + strCtr;
                            cmd.Parameters.AddWithValue(strCtr, s);

                            ctr += 1;
                        }

                        sqlQuery += ") AND DueDate >= @Due_DateStart AND DueDate <  @Due_DateEnd ";

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateStart", Session["SelectedWFInstanceDueDateStart"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateEnd", Session["SelectedWFInstanceDueDateEnd"].ToString());
                        cmd.Parameters.AddWithValue("@Assigned_To", Session["LoggedInUser"].ToString());
                    }
                    else
                    {
                        sqlQuery = "SELECT Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, " +
                        "CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID, LockedBy, UserDefinedField1, UserDefinedField2, " +
                        "UserDefinedField3, UserDefinedField4, UserDefinedField5  FROM AS_WF_VW_WorkflowTasks " +
                       "WHERE Status = @Status  AND WorkflowStep <> 'Revision' AND AssignedTo <> 'System' AND DueDate >= @Due_DateStart AND DueDate <  @Due_DateEnd ";

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateStart", Session["SelectedWFInstanceDueDateStart"].ToString());
                        cmd.Parameters.AddWithValue("@Due_DateEnd", Session["SelectedWFInstanceDueDateEnd"].ToString());
                    }
                    break;
                case "Completed":
                    if (Session["UserRole"].ToString().ToUpper() != "WORKFLOW ADMIN")
                    {
                        //sqlQuery = "SELECT AS_WF_VW_WorkflowTasks.Department, AS_WF_VW_WorkflowTasks.WorkflowName, AS_WF_VW_WorkflowTasks.DocType, AS_WF_VW_WorkflowTasks.Description, AS_WF_VW_WorkflowTasks.CreatedBy, AS_WF_VW_WorkflowTasks.CreationDate, AS_WF_VW_WorkflowTasks.CreationTime, AS_WF_VW_WorkflowTasks.AssignedTo, AS_WF_VW_WorkflowTasks.Comment, AS_WF_VW_WorkflowTasks.WorkflowStep, " +
                        //"AS_WF_VW_WorkflowTasks.CompletionDate, AS_WF_VW_WorkflowTasks.CompletionTime, AS_WF_VW_WorkflowTasks.Importance, AS_WF_VW_WorkflowTasks.DueDate, AS_WF_VW_WorkflowTasks.Status, AS_WF_VW_WorkflowTasks.DocumentKey, AS_WF_VW_WorkflowTasks.Key2, AS_WF_VW_WorkflowTasks.WFInstanceID, AS_WF_VW_WorkflowTasks.WFID, AS_WF_VW_WorkflowTasks.WFContextID, AS_WF_VW_WorkflowTasks.LockedBy, AS_WF_VW_WorkflowTasks.UserDefinedField1, AS_WF_VW_WorkflowTasks.UserDefinedField2, " +
                        //"AS_WF_VW_WorkflowTasks.UserDefinedField3, AS_WF_VW_WorkflowTasks.UserDefinedField4, AS_WF_VW_WorkflowTasks.UserDefinedField5 FROM AS_WF_VW_WorkflowHistory RIGHT OUTER JOIN " +
                        //"AS_WF_VW_WorkflowTasks ON AS_WF_VW_WorkflowHistory.WFID = AS_WF_VW_WorkflowTasks.WFID " +
                        //"WHERE AS_WF_VW_WorkflowTasks.Status = @Status AND AS_WF_VW_WorkflowHistory.CreatedBy = @Assigned_To AND AS_WF_VW_WorkflowTasks.WorkflowStep = @Workflow_Step";

                        sqlQuery = "SELECT AS_WF_VW_WorkflowTasks.Department, AS_WF_VW_WorkflowTasks.WorkflowName, AS_WF_VW_WorkflowTasks.DocType, AS_WF_VW_WorkflowTasks.Description, AS_WF_VW_WorkflowTasks.CreatedBy, AS_WF_VW_WorkflowTasks.CreationDate, AS_WF_VW_WorkflowTasks.CreationTime, AS_WF_VW_WorkflowTasks.AssignedTo, AS_WF_VW_WorkflowTasks.Comment, AS_WF_VW_WorkflowTasks.WorkflowStep, " +
                        "AS_WF_VW_WorkflowTasks.CompletionDate, AS_WF_VW_WorkflowTasks.CompletionTime, AS_WF_VW_WorkflowTasks.Importance, AS_WF_VW_WorkflowTasks.DueDate, AS_WF_VW_WorkflowTasks.Status, AS_WF_VW_WorkflowTasks.DocumentKey, AS_WF_VW_WorkflowTasks.Key2, AS_WF_VW_WorkflowTasks.Key3, AS_WF_VW_WorkflowTasks.WFInstanceID, AS_WF_VW_WorkflowTasks.WFID, AS_WF_VW_WorkflowTasks.WFContextID, AS_WF_VW_WorkflowTasks.LockedBy, AS_WF_VW_WorkflowTasks.UserDefinedField1, AS_WF_VW_WorkflowTasks.UserDefinedField2, " +
                        "AS_WF_VW_WorkflowTasks.UserDefinedField3, AS_WF_VW_WorkflowTasks.UserDefinedField4, AS_WF_VW_WorkflowTasks.UserDefinedField5 FROM AS_WF_VW_WorkflowTasks " +
                        "WHERE AS_WF_VW_WorkflowTasks.Status = @Status AND AS_WF_VW_WorkflowTasks.WorkflowStep = @Workflow_Step AND AS_WF_VW_WorkflowTasks.DocumentKey IN (SELECT DocumentKey FROM AS_WF_VW_WorkflowHistory WHERE AS_WF_VW_WorkflowHistory.CreatedBy = @Assigned_To) ";


                        //List<string> groupList = Helper.UserGroup(Session["LoggedInUser"].ToString());
                        //int ctr = 0;

                        //foreach (string s in groupList)
                        //{
                        //    string strCtr = "@" + ctr;
                        //    sqlQuery += " OR AS_WF_VW_WorkflowTasks.AssignedTo = " + strCtr;
                        //    cmd.Parameters.AddWithValue(strCtr, s);

                        //    ctr += 1;
                        //}

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Assigned_To", Session["LoggedInUser"].ToString());
                        cmd.Parameters.AddWithValue("@Workflow_Step", Session["SelectedWFInstanceWorkflowStep"].ToString());
                    }
                    else
                    {
                        sqlQuery = "SELECT Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, " +
                        "CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID, LockedBy, UserDefinedField1, UserDefinedField2, " +
                        "UserDefinedField3, UserDefinedField4, UserDefinedField5  FROM AS_WF_VW_WorkflowTasks " +
                       "WHERE Status = @Status AND WorkflowStep = @Workflow_Step";

                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                        cmd.Parameters.AddWithValue("@Workflow_Step", Session["SelectedWFInstanceWorkflowStep"].ToString());
                    }
                    break;
                case "Started By Me":
                    sqlQuery = "SELECT AS_WF_VW_WorkflowTasks.Department, AS_WF_VW_WorkflowTasks.WorkflowName, AS_WF_VW_WorkflowTasks.DocType, AS_WF_VW_WorkflowTasks.Description, AS_WF_VW_WorkflowTasks.CreatedBy, AS_WF_VW_WorkflowTasks.CreationDate, AS_WF_VW_WorkflowTasks.CreationTime, AS_WF_VW_WorkflowTasks.AssignedTo, AS_WF_VW_WorkflowTasks.Comment, AS_WF_VW_WorkflowTasks.WorkflowStep, " +
                    "AS_WF_VW_WorkflowTasks.CompletionDate, AS_WF_VW_WorkflowTasks.CompletionTime, AS_WF_VW_WorkflowTasks.Importance, AS_WF_VW_WorkflowTasks.DueDate, AS_WF_VW_WorkflowTasks.Status, AS_WF_VW_WorkflowTasks.DocumentKey, AS_WF_VW_WorkflowTasks.Key2, AS_WF_VW_WorkflowTasks.Key3, AS_WF_VW_WorkflowTasks.WFInstanceID, AS_WF_VW_WorkflowTasks.WFID, AS_WF_VW_WorkflowTasks.WFContextID, AS_WF_VW_WorkflowTasks.LockedBy, AS_WF_VW_WorkflowTasks.UserDefinedField1, AS_WF_VW_WorkflowTasks.UserDefinedField2, " +
                        "AS_WF_VW_WorkflowTasks.UserDefinedField3, AS_WF_VW_WorkflowTasks.UserDefinedField4, AS_WF_VW_WorkflowTasks.UserDefinedField5  FROM AS_WF_VW_WorkflowHistory RIGHT OUTER JOIN " +
                    "AS_WF_VW_WorkflowTasks ON AS_WF_VW_WorkflowHistory.WFID = AS_WF_VW_WorkflowTasks.WFID " +
                    "WHERE AS_WF_VW_WorkflowHistory.CreatedBy = @Created_By AND AS_WF_VW_WorkflowHistory.Status = @Status ";

                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Status", Session["SelectedWFInstanceStatus"].ToString());
                    cmd.Parameters.AddWithValue("@Created_By", Session["SelectedWFInstanceCreatedBy"].ToString());
                    break;

            }

            adapter.SelectCommand = cmd;
            cmd.CommandText = sqlQuery;

            conn.Open();
            try
            {
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                conn.Close();
            }

            this.RadGrid2.DataSource = dt;
        }

        #endregion

        #region Events

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            searchString = e.Argument.ToLower();
            RadGrid2.Rebind();
            //RadGrid1.Rebind();
        }

        protected void RadGrid2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("SelectedWFInstance");
            if (RadGrid2.SelectedIndexes.Count > 0)
            {
                int selectedIndex = int.Parse(RadGrid2.SelectedIndexes[0]);

                if (selectedIndex < RadGrid2.Items.Count)
                {
                    GridDataItem selectedRow = RadGrid2.Items[selectedIndex];
                    string wfInstanceID = selectedRow["WFInstanceID"].Text;
                    Session["SelectedWFInstance"] = wfInstanceID;
                }
            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                Image img = (Image)item["Priority_Image"].FindControl("Image1");
                if (img != null)
                {
                    img.AlternateText = "Priority: " + item["Importance"].Text.Trim();
                    img.ImageUrl = "~/Images/" + item["Importance"].Text.Trim() + ".png";
                }

                //if(item["Document_Key"].Text.Length > 10)
                //    item["Document_Key"].Text = item["Document_Key"].Text.Substring(0, 10) + "...";

            }
        }

        protected void RadGrid3_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                Image img = (Image)item["Priority_Image"].FindControl("Image1");
                if (img != null)
                {
                    img.AlternateText = "Priority: " + item["Importance"].Text.Trim();
                    img.ImageUrl = "~/Images/" + item["Importance"].Text.Trim() + ".png";
                }

                //if (item["Document_Key"].Text.Length > 10)
                //item["Document_Key"].Text = item["Document_Key"].Text.Substring(0, 10) + "...";
            }
        }

        protected void RadToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            //if ((e.Item).Text == "Approve")
            //{
            if (RadGrid2.SelectedIndexes.Count > 0)
            {
                //int selectedIndex = int.Parse(RadGrid2.SelectedIndexes[0]);

                //if (selectedIndex < RadGrid2.Items.Count)
                //{
                    GridItemCollection gridRows = RadGrid2.SelectedItems;
                    foreach (GridDataItem selectedRow in gridRows)
                    {
                        //GridDataItem selectedRow = RadGrid2.Items[selectedIndex];                        
                        if (selectedRow["Status"].Text == "Pending Action")
                        {
                            if ((e.Item).Text == "View Document")
                            {
                                // Do Nothing because this is being handled from the client side
                                return;
                            }
                            #region Lock
                            else if ((e.Item).Text == "Lock")
                            {
                                //Verify first if locked already                            
                                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
                                try
                                {
                                    //GridDataItem selectedRow = RadGrid2.Items[selectedIndex];
                                    string wfInstanceID = selectedRow["WFInstanceID"].Text;

                                    string sqlString = "SELECT LockedBy FROM AS_WF_WorkflowInstance WHERE WFInstanceID = @WFInstanceID";

                                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                                    cmd.Parameters.AddWithValue("@WFInstanceID", wfInstanceID);

                                    if (connection.State == ConnectionState.Closed)
                                        connection.Open();

                                    object objRtn = cmd.ExecuteScalar();

                                    if (objRtn != null && !string.IsNullOrEmpty(objRtn.ToString()))
                                    {
                                        // Already Locked            
                                        // Need to add code here. Javascript does not work
                                    }
                                    else
                                    {
                                        // Create a new record in the historical first
                                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
                                        try
                                        {
                                            string wfContextID = selectedRow["WFContextID"].Text;
                                            string wfDescription = selectedRow["Description"].Text;
                                            string wfImportance = selectedRow["Importance"].Text;
                                            string wfAssignedTo = selectedRow["AssignedTo"].Text;

                                            string wfStep = selectedRow["WorkflowStep"].Text;

                                            DateTime wfDueDate;

                                            if (!DateTime.TryParse(selectedRow["DueDate"].Text, out wfDueDate))
                                                wfDueDate = DateTime.MaxValue;

                                            cmd = new SqlCommand("AS_WF_spSaveWorkflowInstance", conn);
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@pWFInstanceID", wfInstanceID);
                                            cmd.Parameters.AddWithValue("@pWFContextID", wfContextID);
                                            cmd.Parameters.AddWithValue("@pDescription", wfDescription);
                                            cmd.Parameters.AddWithValue("@pCreatedBy", Session["LoggedInUser"].ToString());
                                            cmd.Parameters.AddWithValue("@pAssignedTo", wfAssignedTo);
                                            cmd.Parameters.AddWithValue("@pComment", "Locked by " + Session["LoggedInUser"].ToString());
                                            cmd.Parameters.AddWithValue("@pRequestType", wfStep);
                                            cmd.Parameters.AddWithValue("@pStatus", "Pending Action");
                                            cmd.Parameters.AddWithValue("@pActionDate", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@pActionTime", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@pImportance", wfImportance);
                                            cmd.Parameters.AddWithValue("@pDueDate", wfDueDate);

                                            if (conn.State == ConnectionState.Closed)
                                                conn.Open();

                                            cmd.ExecuteNonQuery();

                                            // Lock the record
                                            sqlString = "UPDATE AS_WF_WorkflowInstance SET LockedBy = @LockedBy WHERE WFInstanceID = @WFInstanceID";
                                            cmd = new SqlCommand(sqlString, conn);

                                            if (conn.State == ConnectionState.Closed)
                                                conn.Open();

                                            cmd.Parameters.AddWithValue("@WFInstanceID", wfInstanceID);
                                            cmd.Parameters.AddWithValue("@LockedBy", Session["LoggedInUser"].ToString());

                                            cmd.ExecuteNonQuery();                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                        }
                                        finally
                                        {
                                            conn.Close();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                }
                            }
                            #endregion
                            #region Unlock
                            else if ((e.Item).Text == "Unlock")
                            {
                                //Verify first if locked by another person
                                string wfInstanceID = selectedRow["WFInstanceID"].Text;

                                string sqlString = "SELECT LockedBy FROM AS_WF_WorkflowInstance WHERE WFInstanceID = @WFInstanceID";
                                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
                                try
                                {
                                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                                    cmd.Parameters.AddWithValue("@WFInstanceID", wfInstanceID);

                                    if (connection.State == ConnectionState.Closed)
                                        connection.Open();

                                    object objRtn = cmd.ExecuteScalar();

                                    if (objRtn != null && !string.IsNullOrEmpty(objRtn.ToString()) && objRtn.ToString() != Session["LoggedInUser"].ToString())
                                    {
                                        // Already Locked            
                                        // Need to add code here. Javascript does not work
                                    }
                                    else
                                    {
                                        // Create a new record in the historical first
                                        string wfContextID = selectedRow["WFContextID"].Text;
                                        string wfDescription = selectedRow["Description"].Text;
                                        string wfImportance = selectedRow["Importance"].Text;
                                        string wfAssignedTo = selectedRow["AssignedTo"].Text;

                                        string wfStep = selectedRow["WorkflowStep"].Text;

                                        DateTime wfDueDate;

                                        if (!DateTime.TryParse(selectedRow["DueDate"].Text, out wfDueDate))
                                            wfDueDate = DateTime.MaxValue;

                                        cmd = new SqlCommand("AS_WF_spSaveWorkflowInstance", connection);
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@pWFInstanceID", wfInstanceID);
                                        cmd.Parameters.AddWithValue("@pWFContextID", wfContextID);
                                        cmd.Parameters.AddWithValue("@pDescription", wfDescription);
                                        cmd.Parameters.AddWithValue("@pCreatedBy", Session["LoggedInUser"].ToString());
                                        cmd.Parameters.AddWithValue("@pAssignedTo", wfAssignedTo);
                                        cmd.Parameters.AddWithValue("@pComment", "Unlocked by " + Session["LoggedInUser"].ToString());
                                        cmd.Parameters.AddWithValue("@pRequestType", wfStep);
                                        cmd.Parameters.AddWithValue("@pStatus", "Pending Action");
                                        cmd.Parameters.AddWithValue("@pActionDate", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@pActionTime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@pImportance", wfImportance);
                                        cmd.Parameters.AddWithValue("@pDueDate", wfDueDate);

                                        if (connection.State == ConnectionState.Closed)
                                            connection.Open();

                                        cmd.ExecuteNonQuery();

                                        // Lock the record
                                        sqlString = "UPDATE AS_WF_WorkflowInstance SET LockedBy = '' WHERE WFInstanceID = @WFInstanceID";
                                        cmd = new SqlCommand(sqlString, connection);

                                        if (connection.State == ConnectionState.Closed)
                                            connection.Open();

                                        cmd.Parameters.AddWithValue("@WFInstanceID", wfInstanceID);

                                        cmd.ExecuteNonQuery();                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                }
                            }
                            #endregion
                            else
                            {
                                //Transferred code to ActionComment
                            }
                        }
                    }
                //}

                Response.Redirect("Default.aspx");

            }
            //}
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler source, string eventArgument)
        {
            base.RaisePostBackEvent(source, eventArgument);

            if (source == this.RadGrid2 && eventArgument.IndexOf("RowDblClicked") != -1)
            {
                GridDataItem item = RadGrid2.Items[int.Parse(eventArgument.Split(':')[1])];
                Response.Write(String.Format("CustomerID:{0}", item.GetDataKeyValue("CustomerID")));
            }
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            ApplyWFInstanceGridFilter(e.Node.Text);

            RadGrid2.Rebind();
            SelectFirstGridRow();

            switch (e.Node.Text)
            {
                case "Open":
                    Session["SelectedNode"] = e.Node.Text;
                    break;
                case "Past Due":
                    Session["SelectedNode"] = e.Node.Text;
                    break;
                case "Completed":
                    Session["SelectedNode"] = e.Node.Text;
                    break;
                case "Started By Me":
                    Session["SelectedNode"] = e.Node.Text;
                    break;
                default:
                    Session.Remove("e.Node.Text");
                    break;
            }
        }

        protected void RadGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            ApplyWFInstanceGridFilter(RadTreeView1.SelectedNode.Text);

            SelectFirstGridRow();
        }

        #endregion
    }
}
