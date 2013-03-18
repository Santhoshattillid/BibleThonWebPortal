using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using Microsoft.Dynamics.GP.eConnect;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Dynamics.GP.eConnect.Serialization;
using Telerik.Web.UI.GridExcelBuilder;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public partial class frmPresidioApprovalDocument : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //Initialize
                InitializeControls();
                if (!string.IsNullOrEmpty(Request.QueryString["DocumentKey"]))
                {
                    log.Debug(Request.QueryString["DocumentKey"]);
                    txtRecordID.Text = Request.QueryString["DocumentKey"];
                    Session["IsNewRecord"] = false;
                    LoadDocument();                    
                }
                else
                {
                    string outString = Helper.GetNextSequenceNumber("Presidio Approval Document");

                    Session["RecordID"] = outString;
                    Session["IsNewRecord"] = true;

                    txtRecordID.Text = outString;                    
                }
                
                if (!string.IsNullOrEmpty(Request.QueryString["LoggedInUser"]))
                    Session["LoggedInUser"] = Request.QueryString["LoggedInUser"];

                lblUser.Text += Session["LoggedInUser"].ToString();
            }
            else
            {
                if (Session["RecordID"] == null)
                {                    
                    Response.Redirect("Logout.aspx");
                }

                if (!string.IsNullOrEmpty(txtRecordID.Text))
                {
                    //if ((bool)Session["IsNewRecord"] != true && Session["AuthorizationNumber"].ToString() != txtAuthorizationNumber.Text)
                    if (Session["RecordID"].ToString() != txtRecordID.Text)
                    {
                        Session["IsNewRecord"] = false;
                        Session["RecordID"] = txtRecordID.Text;
                        LoadDocument();
                    }
                }
            }                        
        }
        catch (Exception ex)
        {
            log.Error("frmPresidioApprovalDocument Error encountered in Page_Load: ", ex);
        }
    }    

    #region Events

    protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
    {

    }    

    protected void RadToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
    {
       if ((e.Item).Text == "Submit")
        {            
            //Save first
            SaveDocument();

            //Then submit
            SubmitDocument();

            Session["RecordID"] = null;

            string closescript = "<script>returnToParent('Record has been submitted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }        
    }    

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {
        BindValidResults();
        BindInvalidResults();
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {        
     
    }
    

    #endregion

    #region Methods    

    private void InitializeControls()
    {       
        
    }

    private void LoadDocument()
    {
        try
        {
            using (PresidioApprovalsDataDataContext context = new PresidioApprovalsDataDataContext())
            {
                //Load Header
                var approval = (from s in context.AS_Presidio_Approvals
                                        where s.RecordID == txtRecordID.Text
                                 select s).First();                

                if (approval != null)
                {
                    txtRequestor.Text = approval.ApproverID;
                    txtArea.Text = approval.ApprovalArea;
                    txtLevel.Text = approval.LastApprovalLevel.ToString();
                    txtAmount.Text = approval.Amount.ToString();
                    txtWorkflow.Text = approval.WorkflowID;
                    txtApprovalType.Text = approval.ApprovalType;
                }                
            }
        }
        catch (Exception ex)
        {
            log.Error("LoadDocument Error: ", ex);
        }
    }    

    private void BindValidResults()
    {
     
    }

    private void BindInvalidResults()
    {
        
    }

    

    /// <summary>
    /// Shows a <see cref="RadWindow"/> alert if an error occurs
    /// </summary>
    private void ShowErrorMessage()
    {
        RadAjaxManager1.ResponseScripts.Add(string.Format("window.radalert(\"Please enter valid data!\")"));
    }

    private bool SubmitDocument()
    {
        bool rtn = false;

        Dictionary<string, string> keyValue = new Dictionary<string, string>();

        keyValue.Add("DocKey_Str1", Session["RecordID"].ToString());

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        try
        {

            // Submit To Workflow
            SqlCommand cmd = new SqlCommand("", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AS_WF_spInsertIntoWorkflowInstance";

            cmd.Parameters.Add(new SqlParameter("@pWorkflowName", "PresidioWestITDocument"));
            cmd.Parameters.Add(new SqlParameter("@pCompanyID", Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"])));
            cmd.Parameters.Add(new SqlParameter("@pDocType", "Presidio Approval Document"));
            cmd.Parameters.Add(new SqlParameter("@pDescription", "New Presidio approval document"));
            cmd.Parameters.Add(new SqlParameter("@pCreatedBy", Session["LoggedInUser"].ToString()));
            cmd.Parameters.Add(new SqlParameter("@pRequestType", "New"));
            cmd.Parameters.Add(new SqlParameter("@pDocKey_Str1", Session["RecordID"]));
            cmd.Parameters.Add(new SqlParameter("@pCreationDate", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@pCreationTime", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@pAssignedTo", "System"));
            cmd.Parameters.Add(new SqlParameter("@pComment", string.Empty));
            cmd.Parameters.Add(new SqlParameter("@pImportance", string.Empty));
            cmd.Parameters.Add(new SqlParameter("@pDueDate", DateTime.Now.AddDays(10)));            

            connection.Open();

            cmd.ExecuteNonQuery();

            // Update Document Status
            PresidioApprovalsDataDataContext pad = new PresidioApprovalsDataDataContext();

            var header = (from c in pad.AS_Presidio_Approvals
                          where c.RecordID == Session["RecordID"].ToString()
                          select c).First();

            header.WorkflowState = "Submitted";
            pad.SubmitChanges();
            rtn = true;
        }
        catch (Exception ex)
        {
            log.Error("SubmitDocument", ex);
        }
        finally
        {
            connection.Close();
        }
        return rtn;
    }

    private void SaveDocument()
    {
        PresidioApprovalsDataDataContext pad = new PresidioApprovalsDataDataContext();

        if (Convert.ToBoolean(Session["IsNewRecord"]))
        {
            AS_Presidio_Approval header = new AS_Presidio_Approval
            {
                RecordID = txtRecordID.Text,
                Amount = Convert.ToDecimal(txtAmount.Value),
                ApprovalArea = txtArea.Text,
                ApprovalType = txtApprovalType.Text,
                ApproverID = txtRequestor.Text,
                LastApprovalLevel = 0,
                WorkflowID = "PresidioWestITDocument"
            };

            pad.AS_Presidio_Approvals.InsertOnSubmit(header);
        }
        else
        {
            var header = (from c in pad.AS_Presidio_Approvals
                          where c.RecordID == Session["RecordID"].ToString()
                          select c).First();
            
                header.Amount = Convert.ToDecimal(txtAmount.Value);
                header.ApprovalArea = txtArea.Text;
                header.ApprovalType = txtApprovalType.Text;
                header.ApproverID = txtRequestor.Text;
                header.LastApprovalLevel = Convert.ToInt32(txtLevel.Text);
                header.WorkflowID = "PresidioWestITDocument";
        }
        try
        {
            pad.SubmitChanges();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    

    public static void LogError(string pErrorMessage)
    {
        // create a writer and open the file
        //TextWriter tw = new StreamWriter(@"C:\DunnLumberOAAppErrorLog.txt");            
        if (!File.Exists(HttpContext.Current.Server.MapPath("~") + "/WebPortalErrors.txt"))
            File.Create(HttpContext.Current.Server.MapPath("~") + "/WebPortalErrors.txt");

        StreamWriter SW = File.AppendText(HttpContext.Current.Server.MapPath("~") + "/WebPortalErrors.txt");

        // write a line of text to the file
        SW.WriteLine("Error has been encountered at " + DateTime.Now.ToString() + ": " + pErrorMessage);

        // Close file                        
        SW.Close();
    }
    
    #endregion


}

