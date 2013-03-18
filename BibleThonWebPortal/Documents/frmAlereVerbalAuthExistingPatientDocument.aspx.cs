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


public partial class frmAlereVerbalAuthExistingPatientDocument : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPickLists();

            Session["PatientID"] = string.Empty;

            List<string> shipMethods = Helper.RetrieveShippingMethods();

            foreach (string shipMethod in shipMethods)
            {
                //cboShipMethod.Items.Add(new RadComboBoxItem(shipMethod));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["DocumentKey"]))
            {
                Session["RecordID"] = Request.QueryString["DocumentKey"];
                txtRecordID.Text = Request.QueryString["DocumentKey"];

                LoadDocument(txtRecordID.Text);
            }
            else
            {
                WorkflowDataContext wfDataContext = new WorkflowDataContext();

                string outString = string.Empty;

                //Retrieve next available number
                outString = Helper.GetNextSequenceNumber("Alere Verbal Auth Existing Patient Document");

                Session["RecordID"] = outString;
                Session["IsNewRecord"] = true;
                txtRecordID.Text = outString;

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
            if (Session["RecordID"] != null && Session["RecordID"].ToString() != txtRecordID.Text.Trim())
                LoadDocument(txtRecordID.Text);

            Session["RecordID"] = txtRecordID.Text.Trim();
            Session["PatientID"] = txtPatientID.Text.Trim();
        }

        UpdatePreview();

        // Load Dynamic Scripts
        LoadDynamicScripts();

        if (txtStatus.Text == "New")
            dtDueDate.SelectedDate = DateTime.Now.AddDays(3);
    }

    #region Methods

    private void LoadDocument()
    {
        LoadDocument(string.Empty);
    }

    private void LoadDocument(string pRecordID)
    {
        using (AlereVerbalAuthExistingPatientDataDataContext context = new AlereVerbalAuthExistingPatientDataDataContext())
        {
            // Check if existing record first
            if (!string.IsNullOrEmpty(pRecordID))
            {
                //Load from Orders Table
                var orderVar = (from s in context.AS_Alere_VAEP_Records
                                where s.RecordID == pRecordID
                                select s);

                foreach (AS_Alere_VAEP_Record order in orderVar)
                {
                    txtPatientID.Text = order.PatientID;
                    Session["PatientID"] = txtPatientID.Text.Trim();
                    txtStatus.Text = order.WorkflowState;

                    string DetailNote = order.DetailApproverNoteColumn.Contains(order.NoteColumn) ? order.DetailApproverNoteColumn : order.NoteColumn + order.DetailApproverNoteColumn;

                    txtDetailsNote.Text = DetailNote;
                    txtBasicInfoNote.Text = order.NoteColumn;

                }
            }

            // Verify if record already exists
            var patientInfo = (from s in context.uvw_AHM_Patient_Details
                               where s.Patient_Number.Trim() == txtPatientID.Text.Trim()
                               select s);


            foreach (uvw_AHM_Patient_Detail item in patientInfo)
            {
                txtFirstName.Text = item.Patient_First_Name;
                txtLastName.Text = item.Patient_Last_Name;
                dtDOB.SelectedDate = item.Birth_Date;
                txtDoctorName.Text = item.Primary_Physician_Name;
                txtDoctorPhone.Text = item.Primary_Physician_Phone_Number;
                txtClinicHeader.Text = item.Referring_Source_Name;
                txtClinicCodeHeader.Text = item.Referring_Source_Code;
            }

            dtDueDate.SelectedDate = DateTime.Now.AddDays(3);

            // Verify if record already exists
            var cbInfo = (from s in context.AS_Alere_VAEP_CBs
                          where s.RecordID.Trim() == pRecordID.Trim()
                          select s);

            foreach (AS_Alere_VAEP_CB item in cbInfo)
            {
                chkGender.Checked = (bool)item.Gender;
                chkCareGiver.Checked = (bool)item.Caregiver;
                chkPrimaryPhone.Checked = (bool)item.PrimaryPhone;
                chkCaregiverRelationship.Checked = (bool)item.CaregiverRelationship;
                chkSecondaryPhone.Checked = (bool)item.SecondaryPhone;
                chkCaregiverPrimaryPhone.Checked = (bool)item.CaregiverPrimaryPhone;
                chkEmail.Checked = (bool)item.Email;
                chkCaregiverSecondaryPhone.Checked = (bool)item.CaregiverSecondaryPhone;
                chkCaregiverPOA.Checked = (bool)item.CaregiverPOA;
                chkPhysician.Checked = (bool)item.Physician;
                chkClinic.Checked = (bool)item.Clinic;
                chkPhysicianPhone.Checked = (bool)item.PhysicianPhone;
                chkClinicAddress.Checked = (bool)item.ClinicAddress;
                chkPhysicianFax.Checked = (bool)item.PhysicianFax;
                chkClinicCityStateZip.Checked = (bool)item.ClinicCityStateZip;
                chkReportResultsTo.Checked = (bool)item.ReportResultsTo;
                chkClinicPhone.Checked = (bool)item.ClinicPhone;
                chkReportsResultsToPhone.Checked = (bool)item.ReportResultsToPhone;
                chkReportResultsToFax.Checked = (bool)item.ReportResultsToFax;
                chkAfterHoursInstruction.Checked = (bool)item.AfterHoursInstruction;
                chkMeterType.Checked = (bool)item.MeterType;
                chkTestReportingInstructions.Checked = (bool)item.TestReportingInstruction;
                chkTestFrequency.Checked = (bool)item.TestFrequency;
                chkFaxAllResults.Checked = (bool)item.FaxAllResults;
                chkAPLSDiscussion.Checked = (bool)item.APLSDiscussion;
                chkMBE.Checked = (bool)item.MBE;
                chkMBEDate.Checked = (bool)item.MBEDate;
                chkSelectOptions.Checked = (bool)item.SelectOptions;
                chkRetestIn.Checked = (bool)item.RetestIn;
                chkOtherOptions.Checked = (bool)item.OtherOptions;
                chkAuthorizedBy.Checked = (bool)item.AuthorizedBy;
                chkAuthorizedByTitle.Checked = (bool)item.AuthorizedByTitle;
                chkTakenBy.Checked = (bool)item.TakenBy;
                chkTakenByTitle.Checked = (bool)item.TakenByTitle;
                chkDate.Checked = (bool)item.Date;
                chkTime.Checked = (bool)item.Time;
            }

            // Verify if record already exists
            var detailInfo = (from s in context.AS_Alere_VAEP_Details
                              where s.RecordID.Trim() == pRecordID.Trim()
                              select s);

            foreach (AS_Alere_VAEP_Detail item in detailInfo)
            {
                btnMale.Checked = (bool)item.Gender;
                txtCaregiver.Text = item.Caregiver;
                txtPrimaryPhone.Text = item.PrimaryPhone;
                txtCaregiverRelationship.Text = item.CaregiverRelationship;
                txtSecondaryPhone.Text = item.SecondaryPhone;
                txtCaregiverPrimaryPhone.Text = item.CaregiverPrimaryPhone;
                txtEmail.Text = item.Email;
                txtCaregiverSecondaryPhone.Text = item.CaregiverSecondaryPhone;
                btnPOAYes.Checked = (bool)item.CaregiverPOA;
                txtReferringPhysician.Text = item.Physician;
                txtClinic.Text = item.Clinic;
                txtPhysicianPhone.Text = item.PhysicianPhone;
                txtClinicAddress.Text = item.ClinicAddress;
                txtPhysicianFax.Text = item.PhysicianFax;
                txtClinicCityStateZip.Text = item.ClinicCityStateZip;
                txtReportResultsTo.Text = item.ReportResultsTo;
                txtClinicPhone.Text = item.ClinicPhone;
                txtReportResultsToPhone.Text = item.ReportResultsToPhone;
                txtReportResultsToFax.Text = item.ReportResultsToFax;
                txtAfterHoursInstruction.Text = item.AfterHoursInstruction;
                cboMeterType.SelectedIndex = cboMeterType.FindItemByText(item.MeterType).Index;
                cboTestReportingInstructions.SelectedIndex = cboTestReportingInstructions.FindItemByText(item.TestReportingInstruction).Index;
                cboTestFrequency.SelectedIndex = cboTestFrequency.FindItemByText(item.TestFrequency).Index;
                btnFaxAllYes.Checked = (bool)item.FaxAllResults;
                cboAPLSDiscussion.SelectedIndex = cboAPLSDiscussion.FindItemByText(item.APLSDiscussion).Index;
                btnMBEYes.Checked = (bool)item.MBE;
                dtMBEDate.SelectedDate = item.MBEDate;
                cboSelectOptions.SelectedIndex = cboSelectOptions.FindItemByText(item.SelectOptions).Index;
                txtRetestIn.Text = item.RetestIn.ToString();
                txtOtherOptions.Text = item.OtherOptions;
                txtAuthorizedBy.Text = item.AuthorizedBy;
                txtAuthorizedByTitle.Text = item.AuthorizedByTitle;
                txtTakenBy.Text = item.TakenBy;
                txtTakenByTitle.Text = item.TakenByTitle;
                dtDate.SelectedDate = item.Date;
                dtTime.SelectedDate = DateTime.Parse(item.Time.ToString());
                cboAttempt.SelectedIndex = cboAttempt.FindItemByText(item.Attempt).Index;
                dtAttemptDate.SelectedDate = item.AttemptDate;
                dtAttemptTime.SelectedDate = DateTime.Parse(item.AttemptTime.ToString());
            }

        }

        //Disable Controls if not saved or for revision
        //if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision" && txtStatus.Text != "New")
        if (txtStatus.Text == "Submitted" || txtStatus.Text == "Completed" || txtStatus.Text == "For Details Approval")
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
        Label RecordIDPreview = (Label)previewControl.FindControl("lblOrderNum");
        RecordIDPreview.Text = string.IsNullOrEmpty(txtRecordID.Text) ? "[Order ID]" : txtRecordID.Text;
        Label patientIDPreview = (Label)previewControl.FindControl("lblPatientID");
        patientIDPreview.Text = string.IsNullOrEmpty(txtPatientID.Text) ? "[Patient ID]" : txtPatientID.Text;
        Label firstNamePreview = (Label)previewControl.FindControl("lblFirstName");
        firstNamePreview.Text = string.IsNullOrEmpty(txtFirstName.Text) ? "[First Name]" : txtFirstName.Text;
        Label lastNamePreview = (Label)previewControl.FindControl("lblLastName");
        lastNamePreview.Text = string.IsNullOrEmpty(txtLastName.Text) ? "[Last Name]" : txtLastName.Text;


    }

    private bool SubmitDocument()
    {
        bool rtn = false;

        Dictionary<string, string> keyValue = new Dictionary<string, string>();

        keyValue.Add("DocKey_Str1", Session["RecordID"].ToString());


        if (txtStatus.Text == "For Details Approval")
        {
            WorkflowDataContext wdc = new WorkflowDataContext();

            var wfInstance = (from c in wdc.AS_WF_WorkflowInstances
                              where c.DocKey_Str1 == Session["RecordID"].ToString()
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
                cmd.Parameters.Add(new SqlParameter("@pComment", "Reviewed"));
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "DetailsApproval"));
                cmd.Parameters.Add(new SqlParameter("@pStatus", "Pending Action"));
                cmd.Parameters.Add(new SqlParameter("@pActionDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pActionTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pImportance", wfInstance.Importance));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", wfInstance.DueDate));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField3", cboAttempt.SelectedItem.Text));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                AlereVerbalAuthExistingPatientDataDataContext hm = new AlereVerbalAuthExistingPatientDataDataContext();

                var header = (from c in hm.AS_Alere_VAEP_Records
                              where c.RecordID == Session["RecordID"].ToString()
                              select c).First();

                header.WorkflowState = "For Details Approval";
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
        else
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
            try
            {

                // Submit To Workflow
                SqlCommand cmd = new SqlCommand("", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AS_WF_spInsertIntoWorkflowInstance";

                cmd.Parameters.Add(new SqlParameter("@pWorkflowName", "AlereVerbalAuthExistingPatient"));
                cmd.Parameters.Add(new SqlParameter("@pCompanyID", Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"])));
                cmd.Parameters.Add(new SqlParameter("@pDocType", "Alere Verbal Auth Existing Patient Document"));
                cmd.Parameters.Add(new SqlParameter("@pDescription", "New Verbal Auth Existing Patient Document"));
                cmd.Parameters.Add(new SqlParameter("@pCreatedBy", Session["LoggedInUser"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "New"));
                cmd.Parameters.Add(new SqlParameter("@pDocKey_Str1", Session["RecordID"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pCreationDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pCreationTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pAssignedTo", "System"));
                cmd.Parameters.Add(new SqlParameter("@pComment", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pImportance", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", dtDueDate.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField1", txtLastName.Text + ", " + txtFirstName.Text + " : " + txtPatientID.Text));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField2", txtClinicCodeHeader.Text));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                AlereVerbalAuthExistingPatientDataDataContext hm = new AlereVerbalAuthExistingPatientDataDataContext();

                var header = (from c in hm.AS_Alere_VAEP_Records
                              where c.RecordID == Session["RecordID"].ToString()
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

        if (txtStatus.Text == "New")
            SaveSprocs("Main");
        else
            SaveSprocs("Details");

        AlereVerbalAuthExistingPatientDataDataContext hm = new AlereVerbalAuthExistingPatientDataDataContext();

        var recordInfo = (from c in hm.AS_Alere_VAEP_Records
                          where c.RecordID.Trim() == txtRecordID.Text.Trim()
                          select c);


        if (recordInfo.Count() == 0)
        {
            AS_Alere_VAEP_Record vaepRecord = new AS_Alere_VAEP_Record
            {
                RecordID = txtRecordID.Text,
                PatientID = txtPatientID.Text,
                CreatedBy = Session["LoggedInUser"].ToString(),
                DetailApproverNoteColumn = txtDetailsNote.Text,
                NoteColumn = txtBasicInfoNote.Text
            };

            hm.AS_Alere_VAEP_Records.InsertOnSubmit(vaepRecord);
        }
        else
        {
            var order = (from c in hm.AS_Alere_VAEP_Records
                         where c.RecordID == txtRecordID.Text
                         select c).First();

            order.PatientID = txtPatientID.Text;
            //order.CreatedBy = Session["LoggedInUser"].ToString();
            order.NoteColumn = txtBasicInfoNote.Text;
            order.DetailApproverNoteColumn = txtDetailsNote.Text;
            order.WorkflowState = "For Details Approval";

            AS_Alere_VAEP_Records_AuditTrail vaepRecord = new AS_Alere_VAEP_Records_AuditTrail
            {
                RecordID = txtRecordID.Text,
                PatientID = txtPatientID.Text,
                CreatedBy = Session["LoggedInUser"].ToString(),
                DetailApproverNoteColumn = txtDetailsNote.Text,
                NoteColumn = txtBasicInfoNote.Text
            };

            hm.AS_Alere_VAEP_Records_AuditTrails.InsertOnSubmit(vaepRecord);
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

    private void SaveSprocs(string pAccountType)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            switch (pAccountType)
            {
                case "Main":
                    cmd = new SqlCommand("[AS_WF_SP_VAEP_AlereCBs]", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pRecordID", txtRecordID.Text);
                    cmd.Parameters.AddWithValue("@pGender", chkGender.Checked);
                    cmd.Parameters.AddWithValue("@pPrimaryPhone", chkPrimaryPhone.Checked);
                    cmd.Parameters.AddWithValue("@pSecondaryPhone", chkSecondaryPhone.Checked);
                    cmd.Parameters.AddWithValue("@pEmail", chkEmail.Checked);
                    cmd.Parameters.AddWithValue("@pCaregiver", chkCareGiver.Checked);
                    cmd.Parameters.AddWithValue("@pCaregiverRelationship", chkCaregiverRelationship.Checked);
                    cmd.Parameters.AddWithValue("@pCaregiverPrimaryPhone", chkCaregiverPrimaryPhone.Checked);
                    cmd.Parameters.AddWithValue("@pCaregiverSecondaryPhone", chkCaregiverSecondaryPhone.Checked);
                    cmd.Parameters.AddWithValue("@pCaregiverPOA", chkCaregiverPOA.Checked);
                    cmd.Parameters.AddWithValue("@pPhysician", chkPhysician.Checked);
                    cmd.Parameters.AddWithValue("@pPhysicianFax", chkPhysicianFax.Checked);
                    cmd.Parameters.AddWithValue("@pPhysicianPhone", chkPhysicianPhone.Checked);
                    cmd.Parameters.AddWithValue("@pReportResultsTo", chkReportResultsTo.Checked);
                    cmd.Parameters.AddWithValue("@pReportResultsToPhone", chkReportsResultsToPhone.Checked);
                    cmd.Parameters.AddWithValue("@pReportResultsToFax", chkPhysicianPhone.Checked);
                    cmd.Parameters.AddWithValue("@pAfterHoursInstruction", chkAfterHoursInstruction.Checked);
                    cmd.Parameters.AddWithValue("@pClinic", chkClinic.Checked);
                    cmd.Parameters.AddWithValue("@pClinicAddress", chkClinicAddress.Checked);
                    cmd.Parameters.AddWithValue("@pClinicCityStateZip", chkClinicCityStateZip.Checked);
                    cmd.Parameters.AddWithValue("@pClinicPhone", chkClinicPhone.Checked);
                    cmd.Parameters.AddWithValue("@pMeterType", chkMeterType.Checked);
                    cmd.Parameters.AddWithValue("@pTestFrequency", chkTestFrequency.Checked);
                    cmd.Parameters.AddWithValue("@pAPLSDiscussion", chkAPLSDiscussion.Checked);
                    cmd.Parameters.AddWithValue("@pTestReportingInstruction", chkTestReportingInstructions.Checked);
                    cmd.Parameters.AddWithValue("@pFaxAllResults", chkFaxAllResults.Checked);
                    cmd.Parameters.AddWithValue("@pMBE", chkMBE.Checked);
                    cmd.Parameters.AddWithValue("@pMBEDate", chkMBEDate.Checked);
                    cmd.Parameters.AddWithValue("@pSelectOptions", chkSelectOptions.Checked);
                    cmd.Parameters.AddWithValue("@pRetestIn", chkRetestIn.Checked);
                    cmd.Parameters.AddWithValue("@pOtherOptions", chkOtherOptions.Checked);
                    cmd.Parameters.AddWithValue("@pAuthorizedBy", chkAuthorizedBy.Checked);
                    cmd.Parameters.AddWithValue("@pAuthorizedByTitle", chkAuthorizedByTitle.Checked);
                    cmd.Parameters.AddWithValue("@pTakenBy", chkTakenBy.Checked);
                    cmd.Parameters.AddWithValue("@pTakenByTitle", chkTakenByTitle.Checked);
                    cmd.Parameters.AddWithValue("@pDate", chkDate.Checked);
                    cmd.Parameters.AddWithValue("@pTime", chkTime.Checked);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                    break;
                case "Details":
                    cmd = new SqlCommand("[AS_WF_SP_VAEP_AlereDetails]", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pRecordID", txtRecordID.Text);
                    cmd.Parameters.AddWithValue("@pGender", btnMale.Checked);
                    cmd.Parameters.AddWithValue("@pPrimaryPhone", txtPrimaryPhone.Text);
                    cmd.Parameters.AddWithValue("@pSecondaryPhone", txtSecondaryPhone.Text);
                    cmd.Parameters.AddWithValue("@pEmail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@pCaregiver", txtCaregiver.Text);
                    cmd.Parameters.AddWithValue("@pCaregiverRelationship", txtCaregiverRelationship.Text);
                    cmd.Parameters.AddWithValue("@pCaregiverPrimaryPhone", txtCaregiverPrimaryPhone.Text);
                    cmd.Parameters.AddWithValue("@pCaregiverSecondaryPhone", txtCaregiverSecondaryPhone.Text);
                    cmd.Parameters.AddWithValue("@pCaregiverPOA", btnPOAYes.Checked);
                    cmd.Parameters.AddWithValue("@pPhysician", txtReferringPhysician.Text);
                    cmd.Parameters.AddWithValue("@pPhysicianFax", txtPhysicianFax.Text);
                    cmd.Parameters.AddWithValue("@pPhysicianPhone", txtPhysicianPhone.Text);
                    cmd.Parameters.AddWithValue("@pReportResultsTo", txtReportResultsTo.Text);
                    cmd.Parameters.AddWithValue("@pReportResultsToPhone", txtReportResultsToPhone.Text);
                    cmd.Parameters.AddWithValue("@pReportResultsToFax", txtPhysicianPhone.Text);
                    cmd.Parameters.AddWithValue("@pAfterHoursInstruction", txtAfterHoursInstruction.Text);
                    cmd.Parameters.AddWithValue("@pClinic", txtClinic.Text);
                    cmd.Parameters.AddWithValue("@pClinicAddress", txtClinicAddress.Text);
                    cmd.Parameters.AddWithValue("@pClinicCityStateZip", txtClinicCityStateZip.Text);
                    cmd.Parameters.AddWithValue("@pClinicPhone", txtClinicPhone.Text);
                    cmd.Parameters.AddWithValue("@pMeterType", cboMeterType.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pTestFrequency", cboTestFrequency.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pAPLSDiscussion", cboAPLSDiscussion.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pTestReportingInstruction", cboTestReportingInstructions.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pFaxAllResults", btnFaxAllYes.Checked);
                    cmd.Parameters.AddWithValue("@pMBE", btnMBEYes.Checked);
                    cmd.Parameters.AddWithValue("@pMBEDate", dtMBEDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@pSelectOptions", cboSelectOptions.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pRetestIn", txtRetestIn.Text);
                    cmd.Parameters.AddWithValue("@pOtherOptions", txtOtherOptions.Text);
                    cmd.Parameters.AddWithValue("@pAuthorizedBy", txtAuthorizedBy.Text);
                    cmd.Parameters.AddWithValue("@pAuthorizedByTitle", txtAuthorizedByTitle.Text);
                    cmd.Parameters.AddWithValue("@pTakenBy", txtTakenBy.Text);
                    cmd.Parameters.AddWithValue("@pTakenByTitle", txtTakenByTitle.Text);
                    cmd.Parameters.AddWithValue("@pDate", dtDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@pTime", dtTime.SelectedDate.Value.TimeOfDay.ToString());
                    cmd.Parameters.AddWithValue("@pAttempt", cboAttempt.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@pAttemptDate", dtAttemptDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@pAttemptTime", dtAttemptTime.SelectedDate.Value.TimeOfDay.ToString());

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
        dtDueDate.Enabled = pEnable;
        btnProcess.Enabled = pEnable;

        if (!pEnable)
            DisableCheckboxes();

        if (txtStatus.Text == "For Details Approval" && RetrieveDocumentCreator() != Session["LoggedInUser"].ToString())
        {
            btnPatientIDLookup.Enabled = false;
            RadPanelBar1.Items[1].Enabled = true;
            RadPanelBar1.Items[1].Expanded = true;
            RadPanelBar1.Items[0].Expanded = false;

            DisableCheckboxes();

            // Load checked boxes
            EnableControls();

            btnSubmitDetails.Enabled = true;
        }
        else if (txtStatus.Text == "For Final Approval" || txtStatus.Text == "Completed")
        {
            RadPanelBar1.Items[1].Enabled = false;
            RadPanelBar1.Items[1].Expanded = true;
        }        
    }

    private void EnableControls()
    {        
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT [Gender],[PrimaryPhone],[SecondaryPhone],[Email],[Caregiver],[CaregiverRelationship],[CaregiverPrimaryPhone],[CaregiverSecondaryPhone] " +
                                ",[CaregiverPOA],[Physician],[PhysicianFax],[PhysicianPhone],[ReportResultsTo],[ReportResultsToPhone],[ReportResultsToFax],[AfterHoursInstruction] " +
                                ",[Clinic],[ClinicAddress],[ClinicCityStateZip],[ClinicPhone],[MeterType],[TestFrequency],[APLSDiscussion],[TestReportingInstruction],[FaxAllResults] " +
                                ",[MBE],[MBEDate],[SelectOptions],[RetestIn],[OtherOptions],[AuthorizedBy],[AuthorizedByTitle],[TakenBy],[TakenByTitle],[Date],[Time] FROM AS_Alere_VAEP_CBs " +
                                "WHERE RecordID = @pRecordID ";
        cmd.Parameters.AddWithValue("pRecordID", txtRecordID.Text);

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (Convert.ToBoolean(reader["Gender"].ToString()) == true)
                {
                    btnMale.Enabled = true;
                    btnFemale.Enabled = true;
                    lblDetailsGender.Text = "*" + lblDetailsGender.Text;                                       
                }
                if (Convert.ToBoolean(reader["PrimaryPhone"].ToString()) == true)
                {
                    txtPrimaryPhone.Enabled = true;
                    lblDetailsPrimaryPhone.Text = "*" + lblDetailsPrimaryPhone.Text;
                    rfvPrimaryPhone.Enabled = true;                    
                }

                if (Convert.ToBoolean(reader["SecondaryPhone"].ToString()) == true)
                {
                    txtSecondaryPhone.Enabled = true;
                    lblDetailsSecondaryPhone.Text = "*" + lblDetailsSecondaryPhone.Text;
                    rfvSecondaryPhone.Enabled = true;       
                }
                if (Convert.ToBoolean(reader["Email"].ToString()) == true)
                {
                    txtEmail.Enabled = true;
                    lblDetailsEmail.Text = "*" + lblDetailsEmail.Text;
                    rfvEmail.Enabled = true;
                }
                if (Convert.ToBoolean(reader["Caregiver"].ToString()) == true)
                {
                    txtCaregiver.Enabled = true;
                    lblDetailsCaregiver.Text = "*" + lblDetailsCaregiver.Text;
                    rfvCaregiver.Enabled = true;
                }
                if (Convert.ToBoolean(reader["CaregiverRelationship"].ToString()) == true)
                {
                    txtCaregiverRelationship.Enabled = true;
                    lblDetailsCaregiverRelationship.Text = "*" + lblDetailsCaregiverRelationship.Text;
                    rfvCaregiverRelationship.Enabled = true;
                }
                if (Convert.ToBoolean(reader["CaregiverPrimaryPhone"].ToString()) == true)
                {
                    txtCaregiverPrimaryPhone.Enabled = true;
                    lblDetailsCaregiverPrimaryPhone.Text = "*" + lblDetailsCaregiverPrimaryPhone.Text;
                    rfvCaregiverPrimaryPhone.Enabled = true;
                }
                if (Convert.ToBoolean(reader["CaregiverSecondaryPhone"].ToString()) == true)
                {
                    txtCaregiverSecondaryPhone.Enabled = true;
                    lblDetailsCaregiverSecondaryPhone.Text = "*" + lblDetailsCaregiverSecondaryPhone.Text;
                    rfvCaregiverSecondaryPhone.Enabled = true;
                }
                if (Convert.ToBoolean(reader["CaregiverPOA"].ToString()) == true)
                {
                    btnPOANo.Enabled = true;
                    btnPOAYes.Enabled = true;
                    lblDetailsPOA.Text = "*" + lblDetailsPOA.Text;
                }
                if (Convert.ToBoolean(reader["Physician"].ToString()) == true)
                {
                    txtReferringPhysician.Enabled = true;
                    lblDetailsReferringPhysician.Text = "*" + lblDetailsReferringPhysician.Text;
                    rfvReferringPhysician.Enabled = true;
                }
                if (Convert.ToBoolean(reader["PhysicianFax"].ToString()) == true)
                {
                    txtPhysicianFax.Enabled = true;
                    lblDetailsPhysicianFax.Text = "*" + lblDetailsPhysicianFax.Text;
                    rfvPhysicianFax.Enabled = true;
                }
                if (Convert.ToBoolean(reader["PhysicianPhone"].ToString()) == true)
                {
                    txtPhysicianPhone.Enabled = true;
                    lblDetailsPhysicianPhone.Text = "*" + lblDetailsPhysicianPhone.Text;
                    rfvPhysicianPhone.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ReportResultsTo"].ToString()) == true)
                {
                    txtReportResultsTo.Enabled = true;
                    lblDetailsReportResultsTo.Text = "*" + lblDetailsReportResultsTo.Text;
                    rfvReportResultsTo.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ReportResultsToPhone"].ToString()) == true)
                {
                    txtReportResultsToPhone.Enabled = true;
                    lblDetailsReportResultsToPhone.Text = "*" + lblDetailsReportResultsToPhone.Text;
                    rfvReportResultsToPhone.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ReportResultsToFax"].ToString()) == true)
                {
                    txtReportResultsToFax.Enabled = true;
                    lblDetailsReportResultsToFax.Text = "*" + lblDetailsReportResultsToFax.Text;
                    rfvReportResultsToFax.Enabled = true;
                }
                if (Convert.ToBoolean(reader["AfterHoursInstruction"].ToString()) == true)
                {
                    txtAfterHoursInstruction.Enabled = true;
                    lblDetailsAfterHoursInstruction.Text = "*" + lblDetailsAfterHoursInstruction.Text;
                    rfvAfterHoursInstruction.Enabled = true;
                }
                if (Convert.ToBoolean(reader["Clinic"].ToString()) == true)
                {
                    txtClinic.Enabled = true;
                    lblDetailsClinic.Text = "*" + lblDetailsClinic.Text;
                    rfvClinic.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ClinicAddress"].ToString()) == true)
                {
                    txtClinicAddress.Enabled = true;
                    lblDetailsClinicAddress.Text = "*" + lblDetailsClinicAddress.Text;
                    rfvClinicAddress.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ClinicCityStateZip"].ToString()) == true)
                {
                    txtClinicCityStateZip.Enabled = true;
                    lblDetailsClinicCityStateZip.Text = "*" + lblDetailsClinicCityStateZip.Text;
                    rfvClinicCityStateZip.Enabled = true;
                }
                if (Convert.ToBoolean(reader["ClinicPhone"].ToString()) == true)
                {
                    txtClinicPhone.Enabled = true;
                    lbldetailsClinicPhone.Text = "*" + lbldetailsClinicPhone.Text;
                    rfvClinicPhone.Enabled = true;
                }
                if (Convert.ToBoolean(reader["MeterType"].ToString()) == true)
                {
                    cboMeterType.Enabled = true;
                    lblDetailsMeterType.Text = "*" + lblDetailsMeterType.Text;
                    rfvMeterType.Enabled = true;
                }
                if (Convert.ToBoolean(reader["TestFrequency"].ToString()) == true)
                {
                    cboTestFrequency.Enabled = true;
                    lblDetailsTestFrequency.Text = "*" + lblDetailsTestFrequency.Text;
                    rfvTestFrequency.Enabled = true;
                }
                if (Convert.ToBoolean(reader["APLSDiscussion"].ToString()) == true)
                {
                    cboAPLSDiscussion.Enabled = true;
                    lblDetailsAPLSDiscussion.Text = "*" + lblDetailsAPLSDiscussion.Text;
                    rfvAPLSDiscussion.Enabled = true;
                }
                if (Convert.ToBoolean(reader["TestReportingInstruction"].ToString()) == true)
                {
                    cboTestReportingInstructions.Enabled = true;
                    lblDetailsTestReportingInstructions.Text = "*" + lblDetailsTestReportingInstructions.Text;
                    rfvTestReportingInstructions.Enabled = true;
                }
                if (Convert.ToBoolean(reader["FaxAllResults"].ToString()) == true)
                {
                    btnFaxAllNo.Enabled = true;
                    btnFaxAllYes.Enabled = true;
                    lblDetailsFaxAllResults.Text = "*" + lblDetailsFaxAllResults.Text;
                }
                if (Convert.ToBoolean(reader["MBE"].ToString()) == true)
                {
                    btnMBENo.Enabled = true;
                    btnMBEYes.Enabled = true;
                    lblDetailsMBE.Text = "*" + lblDetailsMBE.Text;
                }
                if (Convert.ToBoolean(reader["MBEDate"].ToString()) == true)
                {
                    dtMBEDate.Enabled = true;
                    lblDetailsMBEDate.Text = "*" + lblDetailsMBEDate.Text;
                    rfvMBEDate.Enabled = true;
                }
                if (Convert.ToBoolean(reader["SelectOptions"].ToString()) == true)
                {
                    cboSelectOptions.Enabled = true;
                    lblDetailsSelectOption.Text = "*" + lblDetailsSelectOption.Text;
                    rfvSelectOptions.Enabled = true;
                }
                if (Convert.ToBoolean(reader["RetestIn"].ToString()) == true)
                {
                    txtRetestIn.Enabled = true;
                    lblDetailsRetestIn.Text = "*" + lblDetailsRetestIn.Text;
                    rfvRetestIn.Enabled = true;
                }
                if (Convert.ToBoolean(reader["OtherOptions"].ToString()) == true)
                {
                    txtOtherOptions.Enabled = true;
                    lblDetailsOtherOptions.Text = "*" + lblDetailsOtherOptions.Text;
                    rfvOtherOptions.Enabled = true;
                }
                if (Convert.ToBoolean(reader["AuthorizedBy"].ToString()) == true)
                {
                    txtAuthorizedBy.Enabled = true;
                    lblDetailsAuthorizedBy.Text = "*" + lblDetailsAuthorizedBy.Text;
                }
                if (Convert.ToBoolean(reader["AuthorizedByTitle"].ToString()) == true)
                {
                    txtAuthorizedByTitle.Enabled = true;
                    lblDetailsAuthorizedByTitle.Text = "*" + lblDetailsAuthorizedByTitle.Text;
                }
                if (Convert.ToBoolean(reader["TakenBy"].ToString()) == true)
                {
                    txtTakenBy.Enabled = true;
                    lblDetailsTakenBy.Text = "*" + lblDetailsTakenBy.Text;
                }
                if (Convert.ToBoolean(reader["TakenByTitle"].ToString()) == true)
                {
                    txtTakenByTitle.Enabled = true;
                    lblDetailsTakenByTitle.Text = "*" + lblDetailsTakenByTitle.Text;
                }
                if (Convert.ToBoolean(reader["Date"].ToString()) == true)
                {
                    dtDate.Enabled = true;
                    lblDetailsDate.Text = "*" + lblDetailsDate.Text;
                }
                if (Convert.ToBoolean(reader["Time"].ToString()) == true)
                {
                    dtTime.Enabled = true;
                    lblDetailsTime.Text = "*" + lblDetailsTime.Text;
                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

    }

    private void DisableCheckboxes()
    {
        // Disable the checkboxes
        foreach (Control ctrl in tblBasicInformation.Controls)
        {
            if (ctrl.GetType() == typeof(System.Web.UI.HtmlControls.HtmlTableRow))
            {
                foreach (Control ctrlChild in ((System.Web.UI.HtmlControls.HtmlTableRow)ctrl).Controls)
                {
                    if (ctrlChild.GetType() == typeof(System.Web.UI.HtmlControls.HtmlTableCell))
                    {
                        foreach (Control ctrlChildMain in ((System.Web.UI.HtmlControls.HtmlTableCell)ctrlChild).Controls)
                        {
                            if (ctrlChildMain.GetType() == typeof(CheckBox))
                            {
                                ((CheckBox)ctrlChildMain).Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        txtBasicInfoNote.ReadOnly = true;
    }

    private void LoadPickLists()
    {
        RetrievePickListData("MeterType", cboMeterType);
        RetrievePickListData("TestFrequency", cboTestFrequency);
        RetrievePickListData("APLSDiscussion", cboAPLSDiscussion);
        RetrievePickListData("TestReportingInstruction", cboTestReportingInstructions);
        RetrievePickListData("SelectOptions", cboSelectOptions);
        RetrievePickListData("Attempt", cboAttempt);
    }

    private void RetrievePickListData(string pPicklistKey, RadComboBox pCboBox)
    {
        string sql = "SELECT PicklistItem FROM [AS_Alere_VAEP_PickLists] WHERE ([PicklistKey] = @pPicklistKey)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pPicklistKey", pPicklistKey);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            pCboBox.Items.Clear();

            while (reader.Read())
            {
                pCboBox.Items.Add(new RadComboBoxItem(reader["PicklistItem"].ToString()));
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
    }

    private void LoadDynamicScripts()
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM AS_Alere_DynamicScript";

        string scriptBasicInfo = string.Empty;
        string scriptDetailInfo = string.Empty;


        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                switch (reader["LinkIdentifier"].ToString())
                {
                    case "VerbalAuthExistingPatientBasicInformation":
                        scriptBasicInfo = reader["LinkHTML"].ToString();
                        break;
                    case "VerbalAuthExistingPatientDetailsInformation":
                        scriptDetailInfo = reader["LinkHTML"].ToString();
                        break;

                }

            }

            pnlBasic.Controls.Clear();
            pnlDetails.Controls.Clear();

            pnlBasic.Controls.Add(new LiteralControl(scriptBasicInfo));
            pnlDetails.Controls.Add(new LiteralControl(scriptDetailInfo));
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }

    private string RetrieveDocumentCreator()
    {
        string rtn = string.Empty;

        string sql = "SELECT CreatedBy FROM [AS_Alere_VAEP_Records] WHERE ([RecordID] = @pRecordID)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pRecordID", txtRecordID.Text);

        try
        {
            conn.Open();
            object obj = cmd.ExecuteScalar();

            if (obj != null)
                rtn = obj.ToString();

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    #endregion

    #region Events


    protected void RegisterButtonClick(object sender, EventArgs e)
    {
        // Save the document first
        if (SaveDocument() == false)
        {
            string closescript = "<script>ErrorPage('An error was encountered during processing. Please contact your administrator.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation Error", closescript, false);

        }
        else
        {
            SubmitDocument();

            string closescript = "<script>returnToParent('Record has been submitted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
    }

    protected void SaveDetailsClick(object sender, EventArgs e)
    {
        // Save the document first
        if (SaveDocument() == false)
        {
            string closescript = "<script>ErrorPage('An error was encountered during processing. Please contact your administrator.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation Error", closescript, false);
        }
        else
        {
                string closescript = "<script>returnToParent('Record has been saved')</" + "script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
    }

    protected void SubmitDetailsClick(object sender, EventArgs e)
    {
        // Save the document first
        if (SaveDocument() == false)
        {
            string closescript = "<script>ErrorPage('An error was encountered during processing. Please contact your administrator.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation Error", closescript, false);
        }
        else
        {
            SubmitDocument();

            string closescript = "<script>returnToParent('Record has been submitted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
    }
    #endregion


}