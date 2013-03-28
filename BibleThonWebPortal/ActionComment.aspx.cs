using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActionComment : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        // Handle Session Timeouts
        if (Session["LoggedInUser"] == null)
            Response.Redirect("Logout.aspx");

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["WFInstanceID"]))
            {
                lblWFInstanceID.Text = Request.QueryString["WFInstanceID"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))
            {
                lblAction.Text = Request.QueryString["Action"];
            }
        }
    }

    #region Methods

    #endregion Methods

    #region Events

    protected void Btn_OnCommand(Object sender, CommandEventArgs e)
    {
        string[] wfInstanceIDList = lblWFInstanceID.Text.Split(';');

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);

        try
        {
            foreach (string strWFInstanceID in wfInstanceIDList)
            {
                if (!string.IsNullOrEmpty(strWFInstanceID))
                {
                    string wfInstanceID = string.Empty;
                    string wfContextID = string.Empty;
                    string wfDescription = string.Empty;
                    string wfImportance = string.Empty;
                    string wfStep = string.Empty;

                    DateTime wfDueDate = new DateTime(); ;

                    string sql = "SELECT WFContextID, Description, WFRequestType, Importance, DueDate FROM [AS_WF_WorkflowInstance] WHERE WFInstanceID = @pWFInstanceID";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@pWFInstanceID", strWFInstanceID);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        wfInstanceID = strWFInstanceID;
                        wfContextID = reader["WFContextID"].ToString();
                        wfDescription = reader["Description"].ToString();
                        wfImportance = reader["Importance"].ToString();
                        wfStep = reader["WFRequestType"].ToString();
                        if (!DateTime.TryParse(reader["DueDate"].ToString(), out wfDueDate))
                            wfDueDate = DateTime.MaxValue;
                    }

                    reader.Close();

                    cmd = new SqlCommand("AS_WF_spSaveWorkflowInstance", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pWFInstanceID", wfInstanceID);
                    cmd.Parameters.AddWithValue("@pWFContextID", wfContextID);
                    cmd.Parameters.AddWithValue("@pDescription", wfDescription);
                    cmd.Parameters.AddWithValue("@pCreatedBy", Session["LoggedInUser"].ToString());
                    cmd.Parameters.AddWithValue("@pAssignedTo", "System");
                    cmd.Parameters.AddWithValue("@pComment", txtComment.Text);
                    cmd.Parameters.AddWithValue("@pRequestType", wfStep);
                    cmd.Parameters.AddWithValue("@pStatus", lblAction.Text + "d"); //Added "d" for past tense
                    cmd.Parameters.AddWithValue("@pActionDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@pActionTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@pImportance", wfImportance);
                    cmd.Parameters.AddWithValue("@pDueDate", wfDueDate);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cmd.ExecuteNonQuery();

                    // Remove the lock
                    string sqlString = "UPDATE AS_WF_WorkflowInstance SET LockedBy = '' WHERE WFInstanceID = @WFInstanceID";
                    cmd = new SqlCommand(sqlString, conn);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cmd.Parameters.AddWithValue("@WFInstanceID", wfInstanceID);

                    cmd.ExecuteNonQuery();

                    string closescript = "<script>returnToParent(100)</" + "script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Submitted", closescript, false);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }

    #endregion Events
}