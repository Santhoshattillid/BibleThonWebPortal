using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using Telerik.Web.UI;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;


public partial class frmAlerePatientMailingDocument : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

    private string searchString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["PatientID"] = string.Empty;

            List<string> shipMethods = Helper.RetrieveShippingMethods();

            if (!string.IsNullOrEmpty(Request.QueryString["DocumentKey"]))
            {
                Session["OrderID"] = Request.QueryString["DocumentKey"];
                txtOrderID.Text = Request.QueryString["DocumentKey"];

                LoadDocument(txtOrderID.Text);
            }
            else
            {
                string outString = Helper.GetNextSequenceNumber("Alere Patient Mailing Document");

                Session["OrderID"] = outString;
                Session["IsNewRecord"] = true;
                txtOrderID.Text = outString;

                txtStatus.Text = "New";
            }

            if (!string.IsNullOrEmpty(Request.QueryString["LoggedInUser"]))
                Session["LoggedInUser"] = Request.QueryString["LoggedInUser"];

        }
        else
        {
            // Resulting from changed patient ID
            if (Session["PatientID"] == null && !string.IsNullOrEmpty(txtPatientID.Text.Trim()))
                LoadDocument();

            // Resulting from changed patient ID (there is already a loaded patient id)
            if (Session["PatientID"] != null && Session["PatientID"].ToString() != txtPatientID.Text.Trim())
                LoadDocument();

            // Resulting from changed Order ID
            if (Session["OrderID"] != null && Session["OrderID"].ToString() != txtOrderID.Text.Trim())
                LoadDocument(txtOrderID.Text);

            Session["OrderID"] = txtOrderID.Text.Trim();
            Session["PatientID"] = txtPatientID.Text.Trim();
        }

        UpdatePreview();

        // Load Dynamic Scripts
        LoadDynamicScripts();

        if (txtStatus.Text == "New")
            dtDueDate.SelectedDate = DateTime.Now.AddDays(3);
    }

    #region Methods

    private void LoadDynamicScripts()
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM AS_Alere_DynamicScript";

        string scriptBasicInfo = string.Empty;
        string scriptPatientInfo = string.Empty;
        string scriptOrderInfo = string.Empty;


        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                switch (reader["LinkIdentifier"].ToString())
                {
                    case "PatientMailingBasicInformation":
                        scriptBasicInfo = reader["LinkHTML"].ToString();
                        break;
                    case "PatientMailingPatientInformation":
                        scriptPatientInfo = reader["LinkHTML"].ToString();
                        break;
                    case "PatientMailingOrderInformation":
                        scriptOrderInfo = reader["LinkHTML"].ToString();
                        break;
                }

            }

            pnlBasic.Controls.Clear();
            pnlPatient.Controls.Clear();
            pnlOrder.Controls.Clear();

            pnlBasic.Controls.Add(new LiteralControl(scriptBasicInfo));
            pnlPatient.Controls.Add(new LiteralControl(scriptPatientInfo));
            pnlOrder.Controls.Add(new LiteralControl(scriptOrderInfo));
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }

    private void LoadDocument()
    {
        LoadDocument(string.Empty);
    }

    private void LoadDocument(string pOrderID)
    {
        using (AlerePatientMailingDataDataContext context = new AlerePatientMailingDataDataContext())
        {
            // Check if existing record first
            if (!string.IsNullOrEmpty(pOrderID))
            {
                //Load from Orders Table
                var orderVar = (from s in context.AS_Alere_PMWF_Orders
                                where s.OrderID == pOrderID
                                select s);

                foreach (AS_Alere_PMWF_Order order in orderVar)
                {
                    txtPatientID.Text = order.PatientID;
                    Session["PatientID"] = txtPatientID.Text.Trim();
                    txtOrderNote.Text = order.NoteColumn;
                    txtStatus.Text = order.WorkflowState;

                    txtPatientNote.Text = order.PatientInfoNoteColumn;
                    txtBasicInfoNote.Text = order.BasicInfoNoteColumn;

                    grdOrders.Rebind();
                }
            }

            // Verify if record already exists
            var basicInfoVarAlba = (from s in context.AS_Alere_PMWF_BasicInformations
                                    where s.PatientID.Trim() == txtPatientID.Text.Trim() && s.OrderID.Trim() == txtOrderID.Text.Trim()
                                    select s);

            // If no records, load from View
            if (basicInfoVarAlba.Count() == 0)
            {
                var basicInfoVar = (from s in context.AS_WF_VW_PMWF_AlereBasicInformations
                                      where s.PatientID.Trim() == txtPatientID.Text.Trim() 
                                      select s);

                foreach (AS_WF_VW_PMWF_AlereBasicInformation basicInfo in basicInfoVar)
                {
                    txtFirstName.Text = basicInfo.FirstName;
                    txtLastName.Text = basicInfo.LastName;
                    dtDOB.SelectedDate = ((DateTime)basicInfo.DOB).Date;
                    txtCustomerClass.Text = basicInfo.CustomerClass;
                    dtLastShipDate.SelectedDate = Convert.ToDateTime(basicInfo.LastShipDate);
                    txtMeterType.Text = basicInfo.MeterType;
                    txtLancetType.Text = basicInfo.LancetType;
                }

                // Add default due date
                dtDueDate.SelectedDate = DateTime.Now.AddDays(3);
            }
            else
            {
                foreach (AS_Alere_PMWF_BasicInformation basicInfo in basicInfoVarAlba)
                {
                    txtFirstName.Text = basicInfo.FirstName;
                    txtLastName.Text = basicInfo.LastName;
                    dtDOB.SelectedDate = ((DateTime)basicInfo.DOB).Date;
                    txtCustomerClass.Text = basicInfo.CustomerClass;
                    dtLastShipDate.SelectedDate = Convert.ToDateTime(basicInfo.LastShipDate);
                    txtMeterType.Text = basicInfo.MeterType;
                    txtLancetType.Text = basicInfo.LancetType;
                    dtDueDate.SelectedDate = basicInfo.DueDate;
                }
            }


            var patientVar = (from s in context.AS_Alere_PMWF_PatientInformations
                                where s.PatientID.Trim() == txtPatientID.Text.Trim() && s.OrderID.Trim() == txtOrderID.Text.Trim()
                                select s);
            // If no records, load from View
            if (patientVar.Count() == 0)
            {
                // Load patient info
                var patientInfoVar = (from s in context.AS_WF_VW_PMWF_AlerePatientInformations
                                      where s.PatientID.Trim() == txtPatientID.Text.Trim()
                                      select s);

                foreach (AS_WF_VW_PMWF_AlerePatientInformation patientInfo in patientInfoVar)
                {
                    txtPatientAddressID.Text = patientInfo.AddressID;
                    txtPatientCity.Text = patientInfo.City;
                    txtPatientEmail.Text = patientInfo.Email;
                    txtPatientAddressLine1.Text = patientInfo.PatientAddress;
                    txtPatientPhone.Text = patientInfo.Phone;
                    txtPatientState.Text = patientInfo.State;
                    txtPatientZip.Text = patientInfo.Zip;
                }
            }
            else
            {
                foreach (AS_Alere_PMWF_PatientInformation patientInfo in patientVar)
                {
                    txtPatientAddressID.Text = patientInfo.AddressID;
                    txtPatientCity.Text = patientInfo.City;
                    txtPatientEmail.Text = patientInfo.EMail;
                    txtPatientAddressLine1.Text = patientInfo.PatientAddress;
                    txtPatientPhone.Text = patientInfo.Phone;
                    txtPatientState.Text = patientInfo.State;
                    txtPatientZip.Text = patientInfo.Zip;
                }
            }
        }

        //Disable Controls if not saved or for revision
        if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision" && txtStatus.Text != "New")
            EnableRecords(false);
        else
            EnableRecords(true);
    }

    /// <summary>
    /// Shows a <see cref="RadWindow"/> alert if an error occurs
    /// </summary>
    private void ShowErrorMessage()
    {
        //RadAjaxManager1.ResponseScripts.Add("window.radalert(" + pMessage + ")");
    }

    private void UpdatePreview()
    {
        // Basic Info
        Label orderIDPreview = (Label)previewControl.FindControl("lblOrderNum");
        orderIDPreview.Text = string.IsNullOrEmpty(txtOrderID.Text) ? "[Order ID]" : txtOrderID.Text;
        Label patientIDPreview = (Label)previewControl.FindControl("lblPatientID");
        patientIDPreview.Text = string.IsNullOrEmpty(txtPatientID.Text) ? "[Patient ID]" : txtPatientID.Text;
        Label firstNamePreview = (Label)previewControl.FindControl("lblFirstName");
        firstNamePreview.Text = string.IsNullOrEmpty(txtFirstName.Text) ? "[First Name]" : txtFirstName.Text;
        Label lastNamePreview = (Label)previewControl.FindControl("lblLastName");
        lastNamePreview.Text = string.IsNullOrEmpty(txtLastName.Text) ? "[Last Name]" : txtLastName.Text;
        Label meterPreview = (Label)previewControl.FindControl("lblMeterType");
        meterPreview.Text = string.IsNullOrEmpty(txtMeterType.Text) ? "[Meter Type]" : txtMeterType.Text;
        Label lancetPreview = (Label)previewControl.FindControl("lblLancetType");
        lancetPreview.Text = string.IsNullOrEmpty(txtLancetType.Text) ? "[Lancet Type]" : txtLancetType.Text;

        // Patient Info
        Label phonePreview = (Label)previewControl.FindControl("lblPhone");
        phonePreview.Text = string.IsNullOrEmpty(txtPatientPhone.Text) ? "[Phone]" : txtPatientPhone.Text;
        Label addressIDPreview = (Label)previewControl.FindControl("lblAddressID");
        addressIDPreview.Text = string.IsNullOrEmpty(txtPatientAddressID.Text) ? "[Address ID]" : txtPatientAddressID.Text;
        Label address1Preview = (Label)previewControl.FindControl("lblPatientAddress");
        address1Preview.Text = string.IsNullOrEmpty(txtPatientAddressLine1.Text) ? "[Address 1]" : txtPatientAddressLine1.Text;
        Label cityPreview = (Label)previewControl.FindControl("lblCity");
        cityPreview.Text = string.IsNullOrEmpty(txtPatientCity.Text) ? "[City]" : txtPatientCity.Text;
        Label statePreview = (Label)previewControl.FindControl("lblState");
        statePreview.Text = string.IsNullOrEmpty(txtPatientState.Text) ? "[State]" : txtPatientState.Text;
        Label zipPreview = (Label)previewControl.FindControl("lblZip");
        zipPreview.Text = string.IsNullOrEmpty(txtPatientZip.Text) ? "[Zip]" : txtPatientZip.Text;
        Label eMailPreview = (Label)previewControl.FindControl("lblEmail");
        eMailPreview.Text = string.IsNullOrEmpty(txtPatientEmail.Text) ? "[E-Mail]" : txtPatientEmail.Text;

    }

    private void GoToNextItem()
    {
        int selectedIndex = RadPanelBar1.SelectedItem.Index;

        RadPanelBar1.Items[selectedIndex + 1].Selected = true;
        RadPanelBar1.Items[selectedIndex + 1].Expanded = true;
        RadPanelBar1.Items[selectedIndex + 1].Enabled = true;
        RadPanelBar1.Items[selectedIndex].Expanded = false;

    }

    private void GoToPreviousItem()
    {
        int selectedIndex = RadPanelBar1.SelectedItem.Index;

        RadPanelBar1.Items[selectedIndex - 1].Selected = true;
        RadPanelBar1.Items[selectedIndex - 1].Expanded = true;
        RadPanelBar1.Items[selectedIndex].Expanded = false;
    }

    private bool SubmitDocument()
    {
        bool rtn = false;

        Dictionary<string, string> keyValue = new Dictionary<string, string>();

        keyValue.Add("DocKey_Str1", Session["OrderID"].ToString());


        if (Helper.IsDocumentForRevision(keyValue))
        {
            WorkflowDataContext wdc = new WorkflowDataContext();

            var wfInstance = (from c in wdc.AS_WF_WorkflowInstances
                              where c.DocKey_Str1 == Session["OrderID"].ToString()
                              select c).First();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
            try
            {
                // Submit To Workflow
                SqlCommand cmd = new SqlCommand("", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AS_WF_spSaveWorkflowInstance";

                cmd.Parameters.Add(new SqlParameter("@pWFInstanceID", wfInstance.WFInstanceID));
                cmd.Parameters.Add(new SqlParameter("@pWFContextID", wfInstance.WFContextID));
                cmd.Parameters.Add(new SqlParameter("@pDescription", wfInstance.Description));
                cmd.Parameters.Add(new SqlParameter("@pCreatedBy", Session["LoggedInUser"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pAssignedTo", "System"));
                cmd.Parameters.Add(new SqlParameter("@pComment", "Revised Document"));
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "Revision"));
                cmd.Parameters.Add(new SqlParameter("@pStatus", "Revised"));
                cmd.Parameters.Add(new SqlParameter("@pActionDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pActionTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pImportance", wfInstance.Importance));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", wfInstance.DueDate));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                AlerePatientMailingDataDataContext hm = new AlerePatientMailingDataDataContext();

                var header = (from c in hm.AS_Alere_PMWF_Orders
                              where c.OrderID == Session["OrderID"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                hm.SubmitChanges();

                rtn = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
        else
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
            try
            {

                // Submit To Workflow
                SqlCommand cmd = new SqlCommand("", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AS_WF_spInsertIntoWorkflowInstance";

                cmd.Parameters.Add(new SqlParameter("@pWorkflowName", "AlerePatientMailingWorkflow"));
                cmd.Parameters.Add(new SqlParameter("@pCompanyID", Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"])));
                cmd.Parameters.Add(new SqlParameter("@pDocType", "Alere Patient Mailing Document"));
                cmd.Parameters.Add(new SqlParameter("@pDescription", "New Patient Mailing document"));
                cmd.Parameters.Add(new SqlParameter("@pCreatedBy", Session["LoggedInUser"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "New"));
                cmd.Parameters.Add(new SqlParameter("@pDocKey_Str1", Session["OrderID"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pCreationDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pCreationTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pAssignedTo", "System"));
                cmd.Parameters.Add(new SqlParameter("@pComment", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pImportance", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", dtDueDate.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField1", txtLastName.Text + ", " + txtFirstName.Text + " : " + txtPatientID.Text));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                AlerePatientMailingDataDataContext hm = new AlerePatientMailingDataDataContext();

                var header = (from c in hm.AS_Alere_PMWF_Orders
                              where c.OrderID == Session["OrderID"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                hm.SubmitChanges();

                rtn = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    private bool SaveDocument()
    {
        bool rtn = false;

        //Validate the item count first
        if (grdOrders.Items.Count == 0)
        {
            return rtn;
        }

        AlerePatientMailingDataDataContext hm = new AlerePatientMailingDataDataContext();

        // Basic Info
        int basicInfo = (from c in hm.AS_Alere_PMWF_BasicInformations
                         where c.PatientID.Trim() == txtPatientID.Text && c.OrderID.Trim() == txtOrderID.Text.Trim()
                         select c).Count();

        if (basicInfo == 0)
        {
            SaveSprocs("BasicInfo", true);
        }
        else
        {
            SaveSprocs("BasicInfo", false);
        }



        // Patient Info
        int patientInfo = (from c in hm.AS_Alere_PMWF_PatientInformations
                           where c.PatientID.Trim() == txtPatientID.Text && c.OrderID.Trim() == txtOrderID.Text.Trim()
                           select c).Count();

        if (patientInfo == 0)
        {
            SaveSprocs("PatientInfo", true);
        }
        else
        {
            SaveSprocs("PatientInfo", false);
        }

        // order Summary
        // Patient Info

        int orderSummaryInfo = (from c in hm.AS_Alere_PMWF_Orders
                                where c.OrderID.Trim() == txtOrderID.Text.Trim()
                                select c).Count();


        if (orderSummaryInfo == 0)
        {
            AS_Alere_PMWF_Order order = new AS_Alere_PMWF_Order
            {
                PatientID = txtPatientID.Text,
                OrderID = txtOrderID.Text,
                WorkflowState = "Saved",
                CreatedBy = Session["LoggedInUser"].ToString(),
                NoteColumn = txtOrderNote.Text,
                BasicInfoNoteColumn = txtBasicInfoNote.Text,
                PatientInfoNoteColumn = txtPatientNote.Text
            };

            hm.AS_Alere_PMWF_Orders.InsertOnSubmit(order);
        }
        else
        {
            var order = (from c in hm.AS_Alere_PMWF_Orders
                         where c.OrderID == txtOrderID.Text
                         select c).First();

            order.PatientID = txtPatientID.Text;
            order.WorkflowState = "Saved";
            order.CreatedBy = Session["LoggedInUser"].ToString();
            order.NoteColumn = txtOrderNote.Text;
            order.BasicInfoNoteColumn = txtBasicInfoNote.Text;
            order.PatientInfoNoteColumn = txtPatientNote.Text;
        }

        try
        {
            hm.SubmitChanges();

            rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        return rtn;
    }

    private void SaveSprocs(string pAccountType, bool pIsNewRecord)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            switch (pAccountType)
            {
                case "BasicInfo":
                    cmd = new SqlCommand("AS_WF_SP_PMWF_AlereUpdateBasicInfo", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pOrderID", txtOrderID.Text);
                    cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
                    cmd.Parameters.AddWithValue("@pFirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@pLastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@pDOB", dtDOB.SelectedDate);
                    cmd.Parameters.AddWithValue("@pCustomerClass", txtCustomerClass.Text);
                    cmd.Parameters.AddWithValue("@pLastShipDate", dtLastShipDate.SelectedDate == null ? dtLastShipDate.MinDate : dtLastShipDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@pMeterType", txtMeterType.Text);
                    cmd.Parameters.AddWithValue("@pLancetType", txtLancetType.Text);
                    cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);
                    cmd.Parameters.AddWithValue("@pDueDate", dtDueDate.SelectedDate);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                    break;

                case "PatientInfo":
                    cmd = new SqlCommand("AS_WF_SP_PMWF_AlereUpdatePatientInfo", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pOrderID", txtOrderID.Text);
                    cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
                    cmd.Parameters.AddWithValue("@pAddressID", txtPatientAddressID.Text);
                    cmd.Parameters.AddWithValue("@pPatientAddress", txtPatientAddressLine1.Text);
                    cmd.Parameters.AddWithValue("@pCity", txtPatientCity.Text);
                    cmd.Parameters.AddWithValue("@pState", txtPatientState.Text);
                    cmd.Parameters.AddWithValue("@pZip", txtPatientZip.Text);
                    cmd.Parameters.AddWithValue("@pPhone", txtPatientPhone.Text);
                    cmd.Parameters.AddWithValue("@pEmail", txtPatientEmail.Text);
                    cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                    break;

            }


        }

        catch (Exception ex)
        {
            log.Error(ex);
        }
        finally
        {
            connection.Close();
        }
    }

    public static DataTable GetDataTable(string query)
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(query, conn);

        DataTable myDataTable = new DataTable();

        conn.Open();
        try
        {
            adapter.Fill(myDataTable);
        }
        finally
        {
            conn.Close();
        }
        return myDataTable;
    }

    private void EnableRecords(bool pEnable)
    {
        // Basic Info
        txtPatientID.Enabled = pEnable;
        btnPatientIDLookup.Enabled = pEnable;
        txtFirstName.Enabled = pEnable;
        txtLastName.Enabled = pEnable;
        dtDOB.Enabled = pEnable;
        dtLastShipDate.Enabled = pEnable;
        txtBasicInfoNote.Enabled = pEnable;

        // Patient Info
        txtPatientAddressID.Enabled = pEnable;
        txtPatientAddressLine1.Enabled = pEnable;
        txtPatientCity.Enabled = pEnable;
        txtPatientState.Enabled = pEnable;
        txtPatientZip.Enabled = pEnable;
        txtPatientPhone.Enabled = pEnable;
        txtPatientEmail.Enabled = pEnable;
        txtPatientNote.Enabled = pEnable;

        // Order Items
        grdOrders.Enabled = pEnable;
        txtOrderNote.Enabled = pEnable;

        btnSave.Enabled = pEnable;
        btnProcess.Enabled = pEnable;
    }

    #endregion

    #region Events

    protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }
    }

    protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }
    }

    protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }
    }

    protected void NextButtonClick(object sender, EventArgs e)
    {
        switch (RadPanelBar1.SelectedItem.Index)
        {
            case 0:
                if (!chkBasicInfoAllCorrectBasic.Checked)
                {
                    RadWindowManager1.RadAlert("You cannot continue until you verify that the information is correct.", 330, 100, "Notification", "");
                    return;
                }
                break;
            case 1:
                if (!chkBasicInfoAllCorrectPatient.Checked)
                {
                    RadWindowManager1.RadAlert("You cannot continue until you verify that the information is correct.", 330, 100, "Notification", "");
                    return;
                }
                break;
            default:
                break;
        }

        GoToNextItem();

        UpdatePreview();
    }

    protected void SaveButtonClick(object sender, EventArgs e)
    {
        if (SaveDocument() == false)
        {
            string closescript = "<script>ErrorPage('There are no items that were added.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation Error", closescript, false);

        }
        else
        {
            string closescript = "<script>returnToParent('Record has been saved')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
    }

    protected void RegisterButtonClick(object sender, EventArgs e)
    {
        // Save the document first
        if (SaveDocument() == false)
        {
            string closescript = "<script>ErrorPage('There are no items that were added.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation Error", closescript, false);

        }
        else
        {
            SubmitDocument();

            string closescript = "<script>returnToParent('Record has been submitted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
    }

    protected void BackButtonClick(object sender, EventArgs e)
    {
        GoToPreviousItem();
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        searchString = e.Argument.ToLower();
        //RadGrid1.Rebind();
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            e.Canceled = true;
            System.Collections.Specialized.ListDictionary newValues = new System.Collections.Specialized.ListDictionary();
            newValues["PatientID"] = Session["PatientID"].ToString();
            newValues["OrderID"] = Session["OrderID"].ToString();
            newValues["ItemSKU"] = string.Empty;
            newValues["Description"] = string.Empty;
            newValues["Quantity"] = 1;
            //Insert the item and rebind
            e.Item.OwnerTableView.InsertItem(newValues);
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            editform["PatientID"].Parent.Visible = false;
            editform["OrderID"].Parent.Visible = false;
            editform["ItemSKU"].Parent.Visible = false;
            editform["Quantity"].Parent.Visible = false;
        }

        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem edititem = (GridEditableItem)e.Item;
        //    TextBox txtItemDesc = (TextBox)edititem["Description"].Controls[0];
        //    txtItemDesc.Width = Unit.Pixel(350);
        //    txtItemDesc.Enabled = false;

        //}
    }

    protected void cboItemSKU_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        string sql = "SELECT ItemSKU, Description FROM [AS_Alere_PMWF_ItemList]";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        adapter.Fill(dt);

        RadComboBox comboBox = (RadComboBox)sender;
        // Clear the default Item that has been re-created from ViewState at this point.
        comboBox.Items.Clear();

        foreach (DataRow row in dt.Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["ItemSKU"].ToString();
            item.Value = row["ItemSKU"].ToString();
            item.Attributes.Add("Description", row["Description"].ToString());

            comboBox.Items.Add(item);

            item.DataBind();
        }
    }

    protected void OnSelectedIndexChangedHandler(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["ItemSKU"] = e.Value;
        try
        {
            AlerePatientMailingDataDataContext context = new AlerePatientMailingDataDataContext();

            var itemListVar = (from s in context.AS_Alere_PMWF_ItemLists
                            where s.ItemSKU == e.Value.Trim()
                            select s).First();

            GridEditableItem editedItem = (sender as RadComboBox).NamingContainer as GridEditableItem;

            TextBox txtItemSKU = editedItem["ItemSKU"].Controls[0] as TextBox;
            txtItemSKU.Text = e.Value.ToString();

            if (itemListVar != null)
            {
                TextBox txtItemDesc = editedItem["Description"].Controls[0] as TextBox;
                txtItemDesc.Text = itemListVar.Description;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

    }

    #endregion


}