using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class frmAlereVOBDocument : System.Web.UI.Page
{
    private CacheItemRemovedCallback callBack;
    private string pathToCreate;
    private string searchString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["PatientID"] = string.Empty;

            List<string> shipMethods = Helper.RetrieveShippingMethods();

            foreach (string shipMethod in shipMethods)
            {
                cboShipMethod.Items.Add(new RadComboBoxItem(shipMethod));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["DocumentKey"]))
            {
                Session["OrderID"] = Request.QueryString["DocumentKey"];
                txtOrderID.Text = Request.QueryString["DocumentKey"];

                LoadDocument(txtOrderID.Text);
            }
            else
            {
                WorkflowDataContext wfDataContext = new WorkflowDataContext();

                string outString = string.Empty;

                //Retrieve next available number
                //wfDataContext.AS_WF_spGetNextInSequence("Alere Home Monitoring Document", ref outString);
                outString = Helper.GetNextGpNumber();

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

    private void LoadDynamicScripts()
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM AS_Alere_DynamicScript";

        string scriptBasicInfo = string.Empty;
        string scriptInsuranceInfo = string.Empty;
        string scriptDoctorInfo = string.Empty;
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
                    case "BasicInformation":
                        scriptBasicInfo = reader["LinkHTML"].ToString();
                        break;

                    case "InsuranceInformation":
                        scriptInsuranceInfo = reader["LinkHTML"].ToString();
                        break;

                    case "DoctorInformation":
                        scriptDoctorInfo = reader["LinkHTML"].ToString();
                        break;

                    case "PatientInformation":
                        scriptPatientInfo = reader["LinkHTML"].ToString();
                        break;

                    case "OrderInformation":
                        scriptOrderInfo = reader["LinkHTML"].ToString();
                        break;
                }
            }

            pnlBasic.Controls.Clear();
            pnlInsurance.Controls.Clear();
            pnlDoctor.Controls.Clear();
            pnlPatient.Controls.Clear();
            pnlOrder.Controls.Clear();

            pnlBasic.Controls.Add(new LiteralControl(scriptBasicInfo));
            pnlInsurance.Controls.Add(new LiteralControl(scriptInsuranceInfo));
            pnlDoctor.Controls.Add(new LiteralControl(scriptDoctorInfo));
            pnlPatient.Controls.Add(new LiteralControl(scriptPatientInfo));
            pnlOrder.Controls.Add(new LiteralControl(scriptOrderInfo));
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }

    #region Methods

    private void LoadDocument()
    {
        LoadDocument(string.Empty);
    }

    //private void LoadDocument(string pOrderID)
    //{
    //    using (HomeMonitoringDataDataContext context = new HomeMonitoringDataDataContext())
    //    {
    //        // Check if existing record first
    //        if (!string.IsNullOrEmpty(pOrderID))
    //        {
    //            //Load from Orders Table
    //            var order =  (from s in context.AS_Alere_Orders
    //                                where s.OrderID == pOrderID
    //                                select s);

    //            foreach (var c in order)
    //            {
    //                txtPatientID.Text = c.PatientID;
    //                Session["PatientID"] = txtPatientID.Text.Trim();
    //                chkSuppliesNotNeeded.Checked = (bool)c.SuppliesNotNeeded;
    //                RadComboBoxItem item = cboShipMethod.FindItemByText(c.ShippingMethodID);
    //                item.Selected = true;

    //                grdOrders.Rebind();
    //            }
    //        }

    //        // Verify if record already exists
    //        var infoCount = (from s in context.AS_WF_VW_AlereBasicInformations
    //                         where s.PatientID.Trim() == txtPatientID.Text.Trim()
    //                         select s).Count();

    //        // If no records, load from GP
    //        if (infoCount == 0)
    //        {
    //            // Patient Address
    //            CompanyDataDataContext cd = new CompanyDataDataContext();

    //            var customer = (from s in cd.RM00101s
    //                             where s.CUSTNMBR == txtPatientID.Text
    //                             select s).First();

    //            if (customer != null)
    //            {
    //                var customerAddress = (from s in cd.RM00102s
    //                                where s.CUSTNMBR == txtPatientID.Text && s.ADRSCODE == customer.ADRSCODE
    //                                select s).First();

    //                if (customerAddress != null)
    //                {
    //                    txtPatientAddressID.Text = customerAddress.ADRSCODE;
    //                    txtPatientAddressLine1.Text = customerAddress.ADDRESS1;
    //                    txtPatientCity.Text = customerAddress.CITY;
    //                    txtPatientState.Text = customer.STATE;
    //                    txtPatientZip.Text = customer.ZIP;
    //                    txtPatientPhone.Text = customer.PHONE1;

    //                    txtCustomerClass.Text = customer.CUSTCLAS;
    //                }
    //            }

    //            var SOPDoc = (from s in cd.SOP10100s
    //                          where s.CUSTNMBR == txtPatientID.Text orderby s.ACTLSHIP descending
    //                          select s).First();

    //            if (SOPDoc != null)
    //            {
    //                dtLastShipDate.SelectedDate = SOPDoc.ACTLSHIP;
    //            }
    //        }
    //        else
    //        {
    //            // Load basicinfo
    //            var basicInfo = (from s in context.AS_WF_VW_AlereBasicInformations
    //                             where s.PatientID.Trim() == txtPatientID.Text.Trim()
    //                             select s);

    //            foreach (var basic in basicInfo)
    //            {
    //                txtFirstName.Text = basic.FirstName;
    //                txtLastName.Text = basic.LastName;
    //                dtDOB.SelectedDate = ((DateTime)basic.DOB).Date;
    //                txtCustomerClass.Text = basic.CustomerClass;
    //                dtLastShipDate.SelectedDate = basic.LastShipDate;
    //                txtMeterType.Text = basic.MeterType;
    //                txtLancetType.Text = basic.LancetType;
    //            }

    //            // Load insurance
    //            var insuranceInfo = (from s in context.AS_WF_VW_AlereInsuranceInformations
    //                                 where s.PatientID.Trim() == txtPatientID.Text.Trim()
    //                                 select s);

    //            foreach (var insurance in insuranceInfo)
    //            {
    //                txtPrimaryPlan.Text = insurance.PrimaryPlan;
    //                txtPrimaryPolicyNumber.Text = insurance.PrimaryPolicyNumber;
    //                txtPrimaryGroupNumber.Text = insurance.PrimaryGroupNumber;
    //                txtPrimaryPhone.Text = insurance.PrimaryClaimPhoneNumber;
    //                txtPrimaryAddress.Text = insurance.PrimaryAddress;
    //                txtPrimaryCity.Text = insurance.PrimaryCity;
    //                txtPrimaryState.Text = insurance.PrimaryState;
    //                txtPrimaryZip.Text = insurance.PrimaryZip;
    //                dtPrimaryEffectivityDate.SelectedDate = (DateTime)(insurance.PrimaryEffectivityDate);
    //                txtSecondaryPlan.Text = insurance.SecondaryPlan;
    //                txtSecondaryPolicyNumber.Text = insurance.SecondaryPolicyNumber;
    //                txtSecondaryGroupNumber.Text = insurance.SecondaryGroupNumber;
    //                txtSecondaryPhone.Text = insurance.SecondaryClaimPhoneNumber;
    //                txtSecondaryAddress.Text = insurance.SecondaryAddress;
    //                txtSecondaryCity.Text = insurance.SecondaryCity;
    //                txtSecondaryState.Text = insurance.SecondaryState;
    //                txtSecondaryZip.Text = insurance.SecondaryZip;
    //                dtSecondaryEffectivityDate.SelectedDate = (DateTime)(insurance.SecondaryEffectivityDate);
    //            }

    //            // Load doctor
    //            var doctorInfo = (from s in context.AS_WF_VW_AlereDoctorInformations
    //                              where s.PatientID.Trim() == txtPatientID.Text.Trim()
    //                              select s);

    //            foreach (var doctor in doctorInfo)
    //            {
    //                txtDoctorCity.Text = doctor.City;
    //                txtDoctorName.Text = doctor.Name;
    //                txtDoctorPhone.Text = doctor.PhoneNumber;
    //                txtDoctorState.Text = doctor.State;
    //                txtDoctorZip.Text = doctor.Zip;
    //                txtClinic.Text = doctor.Clinic;
    //                txtClinicCode.Text = doctor.ClinicCode;
    //            }

    //            // Load patient info
    //            var patientInfo = (from s in context.AS_WF_VW_AlerePatientInformations
    //                               where s.PatientID.Trim() == txtPatientID.Text.Trim()
    //                               select s);

    //            foreach (var patient in patientInfo)
    //            {
    //                txtPatientAddressID.Text = patient.AddressID;
    //                txtPatientCity.Text = patient.City;
    //                txtPatientEmail.Text = patient.Email;
    //                txtPatientAddressLine1.Text = patient.PatientAddress;
    //                txtPatientPhone.Text = patient.Phone;
    //                txtPatientState.Text = patient.State;
    //                txtPatientZip.Text = patient.Zip;
    //            }

    //        }
    //        //Disable Controls if not saved or for revision
    //        //if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision")
    //        //    DisableControls();
    //    }
    //}

    private void LoadDocument(string pOrderID)
    {
        //using (AlereHomeMonitoringDataDataContext context = new AlereHomeMonitoringDataDataContext())
        //{
        //    // Check if existing record first
        //    if (!string.IsNullOrEmpty(pOrderID))
        //    {
        //        //Load from Orders Table
        //        var orderVar = (from s in context.AS_Alere_Orders
        //                        where s.OrderID == pOrderID
        //                        select s);

        //        foreach (AS_Alere_Order order in orderVar)
        //        {
        //            txtPatientID.Text = order.PatientID;
        //            Session["PatientID"] = txtPatientID.Text.Trim();
        //            chkSuppliesNotNeeded.Checked = (bool)order.SuppliesNotNeeded;
        //            RadComboBoxItem item = cboShipMethod.FindItemByText(order.ShippingMethodID);
        //            item.Selected = true;
        //            txtOrderNote.Text = order.NoteColumn;
        //            txtStatus.Text = order.WorkflowState;

        //            txtDoctorNote.Text = order.DoctorInfoNoteColumn;
        //            txtInsuranceNote.Text = order.InsuranceInfoNoteColumn;
        //            txtPatientNote.Text = order.PatientInfoNoteColumn;
        //            txtBasicInfoNote.Text = order.BasicInfoNoteColumn;

        //            grdOrders.Rebind();
        //        }
        //    }

        //    // Verify if record already exists
        //    var basicInfoVarAlba = (from s in context.AS_Alere_BasicInformations
        //                            where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                            select s);

        //    // If no records, load from View
        //    // Revised so that it would always load from View
        //    //if (infoCount == 0)
        //    //{
        //    var basicInfoVar = (from s in context.AS_WF_VW_AlereBasicInformations
        //                        where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                        orderby s.LastShipDate descending
        //                        select s);

        //    foreach (AS_WF_VW_AlereBasicInformation basicInfo in basicInfoVar)
        //    {
        //        txtFirstName.Text = basicInfo.FirstName;
        //        txtLastName.Text = basicInfo.LastName;
        //        dtDOB.SelectedDate = ((DateTime)basicInfo.DOB).Date;
        //        txtPrimaryPhone.Text = basicInfo.CustomerClass;
        //        dtLastShipDate.SelectedDate = Convert.ToDateTime(basicInfo.LastShipDate);
        //        txtPrimaryDx.Text = basicInfo.MeterType;
        //        txtSecondaryDx.Text = basicInfo.LancetType;
        //    }

        //    // Add default due date
        //    dtDueDate.SelectedDate = DateTime.Now.AddDays(3);
        //    //}
        //    //else
        //    //{
        //    //    // Load basicinfo
        //    //    var basicInfoVar = (from s in context.AS_Alere_BasicInformations
        //    //                     where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //    //                     select s);

        //    //    foreach (AS_Alere_BasicInformation basicInfo in basicInfoVar)
        //    //    {
        //    //        txtFirstName.Text = basicInfo.FirstName;
        //    //        txtLastName.Text = basicInfo.LastName;
        //    //        dtDOB.SelectedDate = ((DateTime)basicInfo.DOB).Date;
        //    //        txtCustomerClass.Text = basicInfo.CustomerClass;
        //    //        dtLastShipDate.SelectedDate = basicInfo.LastShipDate;
        //    //        txtMeterType.Text = basicInfo.MeterType;
        //    //        txtLancetType.Text = basicInfo.LancetType;
        //    //        dtDueDate.SelectedDate = basicInfo.DueDate;
        //    //    }
        //    //}

        //    // Load insurance
        //    var insuranceCount = (from s in context.AS_Alere_InsuranceInformations
        //                          where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                          select s).Count();

        //    // If no records, load from View
        //    //if (insuranceCount == 0)
        //    //{
        //    var insuranceInfoVar = (from s in context.AS_WF_VW_AlereInsuranceInformations
        //                            where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                            select s);

        //    foreach (AS_WF_VW_AlereInsuranceInformation insuranceInfo in insuranceInfoVar)
        //    {
        //        txtPrimaryPlan.Text = insuranceInfo.PrimaryPlan;
        //        txtPrimaryPolicyNumber.Text = insuranceInfo.PrimaryPolicyNumber;
        //        //txtPrimaryGroupNumber.Text = insuranceInfo.PrimaryGroupNumber;
        //        //txtPrimaryPhone.Text = insuranceInfo.PrimaryClaimPhoneNumber;
        //        //txtPrimaryAddress.Text = insuranceInfo.PrimaryAddress;
        //        //txtPrimaryCity.Text = insuranceInfo.PrimaryCity;
        //        //txtPrimaryState.Text = insuranceInfo.PrimaryState;
        //        //txtPrimaryZip.Text = insuranceInfo.PrimaryZip;
        //        //dtPrimaryEffectivityDate.SelectedDate = (DateTime)(insuranceInfo.PrimaryEffectivityDate);
        //        txtSecondaryPlan.Text = insuranceInfo.SecondaryPlan;
        //        txtSecondaryPolicyNumber.Text = insuranceInfo.SecondaryPolicyNumber;
        //        //txtSecondaryGroupNumber.Text = insuranceInfo.SecondaryGroupNumber;
        //        //txtSecondaryPhone.Text = insuranceInfo.SecondaryClaimPhoneNumber;
        //        //txtSecondaryAddress.Text = insuranceInfo.SecondaryAddress;
        //        //txtSecondaryCity.Text = insuranceInfo.SecondaryCity;
        //        //txtSecondaryState.Text = insuranceInfo.SecondaryState;
        //        //txtSecondaryZip.Text = insuranceInfo.SecondaryZip;
        //        //dtSecondaryEffectivityDate.SelectedDate = (DateTime)(insuranceInfo.SecondaryEffectivityDate);
        //    }
        //    //}
        //    //else
        //    //{
        //    //var insuranceInfoVar = (from s in context.AS_Alere_InsuranceInformations
        //    //                      where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //    //                     select s);

        //    //foreach (AS_Alere_InsuranceInformation insuranceInfo in insuranceInfoVar)
        //    //{
        //    //    txtPrimaryPlan.Text = insuranceInfo.PrimaryPlan;
        //    //   txtPrimaryPolicyNumber.Text = insuranceInfo.PrimaryPolicyNumber;
        //    //txtPrimaryGroupNumber.Text = insuranceInfo.PrimaryGroupNumber;
        //    //txtPrimaryPhone.Text = insuranceInfo.PrimaryClaimPhoneNumber;
        //    //txtPrimaryAddress.Text = insuranceInfo.PrimaryAddress;
        //    //txtPrimaryCity.Text = insuranceInfo.PrimaryCity;
        //    //txtPrimaryState.Text = insuranceInfo.PrimaryState;
        //    //txtPrimaryZip.Text = insuranceInfo.PrimaryZip;
        //    //dtPrimaryEffectivityDate.SelectedDate = (DateTime)(insuranceInfo.PrimaryEffectivityDate);
        //    //   txtSecondaryPlan.Text = insuranceInfo.SecondaryPlan;
        //    //    txtSecondaryPolicyNumber.Text = insuranceInfo.SecondaryPolicyNumber;
        //    //txtSecondaryGroupNumber.Text = insuranceInfo.SecondaryGroupNumber;
        //    //txtSecondaryPhone.Text = insuranceInfo.SecondaryClaimPhoneNumber;
        //    //txtSecondaryAddress.Text = insuranceInfo.SecondaryAddress;
        //    //txtSecondaryCity.Text = insuranceInfo.SecondaryCity;
        //    //txtSecondaryState.Text = insuranceInfo.SecondaryState;
        //    //txtSecondaryZip.Text = insuranceInfo.SecondaryZip;
        //    //dtSecondaryEffectivityDate.SelectedDate = (DateTime)(insuranceInfo.SecondaryEffectivityDate);
        //    //}
        //    //}

        //    var doctorCount = (from s in context.AS_Alere_DoctorInformations
        //                       where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                       select s).Count();

        //    // If no records, load from View
        //    //if (doctorCount == 0)
        //    //{
        //    // Load doctor
        //    var doctorInfoVar = (from s in context.AS_WF_VW_AlereDoctorInformations
        //                         where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                         select s);

        //    foreach (AS_WF_VW_AlereDoctorInformation doctorInfo in doctorInfoVar)
        //    {
        //        txtDoctorCity.Text = doctorInfo.City;
        //        txtDoctorName.Text = doctorInfo.Name;
        //        txtDoctorPhone.Text = doctorInfo.PhoneNumber;
        //        txtDoctorState.Text = doctorInfo.State;
        //        txtDoctorZip.Text = doctorInfo.Zip;
        //        txtClinic.Text = doctorInfo.Clinic;
        //        txtClinicCode.Text = doctorInfo.ClinicCode;
        //    }

        //    //}
        //    //else
        //    //{
        //    // Load doctor
        //    //var doctorInfoVar = (from s in context.AS_Alere_DoctorInformations
        //    //                  where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //    //                   select s);

        //    //foreach (AS_Alere_DoctorInformation doctorInfo in doctorInfoVar)
        //    //    {
        //    //        txtDoctorCity.Text = doctorInfo.City;
        //    //        txtDoctorName.Text = doctorInfo.DoctorName;
        //    //        txtDoctorPhone.Text = doctorInfo.PhoneNumber;
        //    //        txtDoctorState.Text = doctorInfo.State;
        //    //        txtDoctorZip.Text = doctorInfo.Zip;
        //    //        txtClinic.Text = doctorInfo.Clinic;
        //    //        txtClinicCode.Text = doctorInfo.ClinicCode;
        //    //    }
        //    //}

        //    var patientCount = (from s in context.AS_Alere_PatientInformations
        //                        where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                        select s).Count();
        //    // If no records, load from View
        //    //if (patientCount == 0)
        //    //{
        //    // Load patient info
        //    var patientInfoVar = (from s in context.AS_WF_VW_AlerePatientInformations
        //                          where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //                          select s);

        //    foreach (AS_WF_VW_AlerePatientInformation patientInfo in patientInfoVar)
        //    {
        //        txtPatientAddressID.Text = patientInfo.AddressID;
        //        txtPatientCity.Text = patientInfo.City;
        //        txtPatientEmail.Text = patientInfo.Email;
        //        txtPatientAddressLine1.Text = patientInfo.PatientAddress;
        //        txtPatientPhone.Text = patientInfo.Phone;
        //        txtPatientState.Text = patientInfo.State;
        //        txtPatientZip.Text = patientInfo.Zip;
        //    }
        //    //}
        //    //else
        //    //{
        //    // Load patient info
        //    //    var patientInfoVar = (from s in context.AS_Alere_PatientInformations
        //    //                       where s.PatientID.Trim() == txtPatientID.Text.Trim()
        //    //                       select s);

        //    //    foreach (AS_Alere_PatientInformation patientInfo in patientInfoVar)
        //    //    {
        //    //        txtPatientAddressID.Text = patientInfo.AddressID;
        //    //        txtPatientCity.Text = patientInfo.City;
        //    //        txtPatientEmail.Text = patientInfo.EMail;
        //    //        txtPatientAddressLine1.Text = patientInfo.PatientAddress;
        //    //        txtPatientPhone.Text = patientInfo.Phone;
        //    //        txtPatientState.Text = patientInfo.State;
        //    //        txtPatientZip.Text = patientInfo.Zip;
        //    //    }
        //    //}
        //}

        ////Disable Controls if not saved or for revision
        //if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision" && txtStatus.Text != "New")
        //    EnableRecords(false);
        //else
        //    EnableRecords(true);
    }

    /// <summary>
    /// Shows a <see cref="RadWindow"/> alert if an error occurs
    /// </summary>
    private void ShowErrorMessage()
    {
        //RadAjaxManager1.ResponseScripts.Add("window.radalert(" + pMessage + ")");
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

    private void UpdatePreview()
    {
        //// Basic Info
        //Label orderIDPreview = (Label)previewControl.FindControl("lblOrderNum");
        //orderIDPreview.Text = string.IsNullOrEmpty(txtOrderID.Text) ? "[Order ID]" : txtOrderID.Text;
        //Label patientIDPreview = (Label)previewControl.FindControl("lblPatientID");
        //patientIDPreview.Text = string.IsNullOrEmpty(txtPatientID.Text) ? "[Patient ID]" : txtPatientID.Text;
        //Label firstNamePreview = (Label)previewControl.FindControl("lblFirstName");
        //firstNamePreview.Text = string.IsNullOrEmpty(txtFirstName.Text) ? "[First Name]" : txtFirstName.Text;
        //Label lastNamePreview = (Label)previewControl.FindControl("lblLastName");
        //lastNamePreview.Text = string.IsNullOrEmpty(txtLastName.Text) ? "[Last Name]" : txtLastName.Text;
        //Label meterPreview = (Label)previewControl.FindControl("lblMeterType");
        //meterPreview.Text = string.IsNullOrEmpty(txtPrimaryDx.Text) ? "[Meter Type]" : txtPrimaryDx.Text;
        //Label lancetPreview = (Label)previewControl.FindControl("lblLancetType");
        //lancetPreview.Text = string.IsNullOrEmpty(txtSecondaryDx.Text) ? "[Lancet Type]" : txtSecondaryDx.Text;

        //// Insurance
        //Label primaryPlanPreview = (Label)previewControl.FindControl("lblPrimaryPlan");
        //primaryPlanPreview.Text = string.IsNullOrEmpty(txtPrimaryPlan.Text) ? "[Primary Plan]" : txtPrimaryPlan.Text;
        //Label primaryGroupPreview = (Label)previewControl.FindControl("lblPrimaryGroupNumber");
        //primaryGroupPreview.Text = string.IsNullOrEmpty(txtPrimaryGroupNumber.Text) ? "[Primary Group Number]" : txtPrimaryGroupNumber.Text;
        //Label secondaryPlanPreview = (Label)previewControl.FindControl("lblSecondaryPlan");
        //secondaryPlanPreview.Text = string.IsNullOrEmpty(txtSecondaryPlan.Text) ? "[Secondary Plan]" : txtSecondaryPlan.Text;
        //Label secondaryGroupPreview = (Label)previewControl.FindControl("lblSecondaryGroupNumber");
        //secondaryGroupPreview.Text = string.IsNullOrEmpty(txtSecondaryGroupNumber.Text) ? "[Secondary Group Number]" : txtSecondaryGroupNumber.Text;

        //// Doctor
        //Label doctorReview = (Label)previewControl.FindControl("lblDoctor");
        //doctorReview.Text = string.IsNullOrEmpty(txtDoctorName.Text) ? "[Doctor]" : txtDoctorName.Text;
        //Label doctorPhonePreview = (Label)previewControl.FindControl("lblDoctorPhoneNumber");
        //doctorPhonePreview.Text = string.IsNullOrEmpty(txtDoctorPhone.Text) ? "[DoctorPhoneNumber]" : txtDoctorPhone.Text;
        //Label doctorCityPreview = (Label)previewControl.FindControl("lblDoctorCity");
        //doctorCityPreview.Text = string.IsNullOrEmpty(txtDoctorCity.Text) ? "[DoctorCity]" : txtDoctorCity.Text;
        //Label doctorStatePreview = (Label)previewControl.FindControl("lblDoctorState");
        //doctorStatePreview.Text = string.IsNullOrEmpty(txtDoctorState.Text) ? "[DoctorState]" : txtDoctorState.Text;
        //Label doctorZipPreview = (Label)previewControl.FindControl("lblDoctorZip");
        //doctorZipPreview.Text = string.IsNullOrEmpty(txtDoctorZip.Text) ? "[DoctorZip]" : txtDoctorZip.Text;

        //// Basic Info
        //Label phonePreview = (Label)previewControl.FindControl("lblPhone");
        //phonePreview.Text = string.IsNullOrEmpty(txtPatientPhone.Text) ? "[Phone]" : txtPatientPhone.Text;
        //Label addressIDPreview = (Label)previewControl.FindControl("lblAddressID");
        //addressIDPreview.Text = string.IsNullOrEmpty(txtPatientAddressID.Text) ? "[Address ID]" : txtPatientAddressID.Text;
        //Label address1Preview = (Label)previewControl.FindControl("lblPatientAddress");
        //address1Preview.Text = string.IsNullOrEmpty(txtPatientAddressLine1.Text) ? "[Address 1]" : txtPatientAddressLine1.Text;
        //Label cityPreview = (Label)previewControl.FindControl("lblCity");
        //cityPreview.Text = string.IsNullOrEmpty(txtPatientCity.Text) ? "[City]" : txtPatientCity.Text;
        //Label statePreview = (Label)previewControl.FindControl("lblState");
        //statePreview.Text = string.IsNullOrEmpty(txtPatientState.Text) ? "[State]" : txtPatientState.Text;
        //Label zipPreview = (Label)previewControl.FindControl("lblZip");
        //zipPreview.Text = string.IsNullOrEmpty(txtPatientZip.Text) ? "[Zip]" : txtPatientZip.Text;
        //Label eMailPreview = (Label)previewControl.FindControl("lblEmail");
        //eMailPreview.Text = string.IsNullOrEmpty(txtPatientEmail.Text) ? "[E-Mail]" : txtPatientEmail.Text;
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
                AlereHomeMonitoringDataDataContext hm = new AlereHomeMonitoringDataDataContext();

                var header = (from c in hm.AS_Alere_Orders
                              where c.OrderID == Session["OrderID"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                hm.SubmitChanges();

                rtn = true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
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

                cmd.Parameters.Add(new SqlParameter("@pWorkflowName", "AlereHomeMonitoring"));
                cmd.Parameters.Add(new SqlParameter("@pCompanyID", Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"])));
                cmd.Parameters.Add(new SqlParameter("@pDocType", "Alere Home Monitoring Document"));
                cmd.Parameters.Add(new SqlParameter("@pDescription", "New Home Monitoring approval document"));
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
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField2", dtPrimaryEffectivityDate.SelectedDate == null ? dtPrimaryEffectivityDate.MinDate.ToShortDateString() : dtPrimaryEffectivityDate.SelectedDate.ToString()));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField3", dtSecondaryEffectivityDate.SelectedDate == null ? dtSecondaryEffectivityDate.MinDate.ToShortDateString() : dtSecondaryEffectivityDate.SelectedDate.ToString()));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                AlereHomeMonitoringDataDataContext hm = new AlereHomeMonitoringDataDataContext();

                var header = (from c in hm.AS_Alere_Orders
                              where c.OrderID == Session["OrderID"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                hm.SubmitChanges();

                rtn = true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
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

        ////Validate the item count first
        //if (!chkSuppliesNotNeeded.Checked && grdOrders.Items.Count == 0)
        //{
        //    return rtn;
        //}

        //bool bInsuranceChanged = false;
        //bool bDoctorChanged = false;
        //bool bPatientChanged = false;

        //AlereHomeMonitoringDataDataContext hm = new AlereHomeMonitoringDataDataContext();

        //// Basic Info
        //int basicInfo = (from c in hm.AS_WF_VW_AlereBasicInformations
        //                 where c.PatientID.Trim() == txtPatientID.Text
        //                 select c).Count();

        //if (basicInfo == 0)
        //{
        //    SaveSprocs("BasicInfo", true);
        //}
        //else
        //{
        //    SaveSprocs("BasicInfo", false);
        //}

        //// Insurance Info
        //int insuranceInfo = (from c in hm.AS_WF_VW_AlereInsuranceInformations
        //                     where c.PatientID.Trim() == txtPatientID.Text
        //                     select c).Count();

        //if (insuranceInfo == 0)
        //{
        //    if (!string.IsNullOrEmpty(txtPatientID.Text))
        //        bInsuranceChanged = true;

        //    SaveSprocs("InsuranceInfo", true);
        //}
        //else
        //{
        //    var insurance = (from c in hm.AS_WF_VW_AlereInsuranceInformations
        //                     where c.PatientID.Trim() == txtPatientID.Text
        //                     select c).First();

        //    // Have to verify if the record needs approval
        //    if (insurance.PrimaryPlan != txtPrimaryPlan.Text ||
        //        insurance.PrimaryPolicyNumber != txtPrimaryPolicyNumber.Text ||
        //        //insurance.PrimaryGroupNumber != txtPrimaryGroupNumber.Text ||
        //        //insurance.PrimaryClaimPhoneNumber != txtPrimaryPhone.Text ||
        //        //insurance.PrimaryAddress != txtPrimaryAddress.Text ||
        //        //insurance.PrimaryCity != txtPrimaryCity.Text ||
        //        //insurance.PrimaryState != txtPrimaryState.Text ||
        //        //insurance.PrimaryZip != txtPrimaryZip.Text ||
        //    insurance.SecondaryPlan != txtSecondaryPlan.Text ||
        //    insurance.SecondaryPolicyNumber != txtSecondaryPolicyNumber.Text)
        //        //insurance.SecondaryGroupNumber != txtSecondaryGroupNumber.Text ||
        //        //insurance.SecondaryClaimPhoneNumber != txtSecondaryPhone.Text ||
        //        //insurance.SecondaryAddress != txtSecondaryAddress.Text ||
        //        //insurance.SecondaryCity != txtSecondaryCity.Text ||
        //        //insurance.SecondaryState != txtSecondaryState.Text ||
        //        //insurance.SecondaryZip != txtSecondaryZip.Text)
        //        bInsuranceChanged = true;

        //    SaveSprocs("InsuranceInfo", false);
        //}

        //// Doctor Info
        //int doctorInfo = (from c in hm.AS_WF_VW_AlereDoctorInformations
        //                  where c.PatientID.Trim() == txtPatientID.Text
        //                  select c).Count();

        //if (doctorInfo == 0)
        //{
        //    SaveSprocs("DoctorInfo", true);

        //    if (!string.IsNullOrEmpty(txtDoctorName.Text))
        //        bDoctorChanged = true;
        //}
        //else
        //{
        //    var doctor = (from c in hm.AS_WF_VW_AlereDoctorInformations
        //                  where c.PatientID.Trim() == txtPatientID.Text
        //                  select c).First();

        //    if (doctor.City != txtDoctorCity.Text ||
        //    doctor.Name != txtDoctorName.Text ||
        //    doctor.PhoneNumber != txtDoctorPhone.Text ||
        //    doctor.State != txtDoctorState.Text ||
        //    doctor.Zip != txtDoctorZip.Text)
        //        bDoctorChanged = true;

        //    SaveSprocs("DoctorInfo", false);
        //}

        //// Patient Info
        //int patientInfo = (from c in hm.AS_WF_VW_AlerePatientInformations
        //                   where c.PatientID.Trim() == txtPatientID.Text
        //                   select c).Count();

        //if (patientInfo == 0)
        //{
        //    SaveSprocs("PatientInfo", true);

        //    if (!string.IsNullOrEmpty(txtPatientAddressID.Text))
        //        bPatientChanged = true;
        //}
        //else
        //{
        //    var patient = (from c in hm.AS_WF_VW_AlerePatientInformations
        //                   where c.PatientID.Trim() == txtPatientID.Text
        //                   select c).First();

        //    if (patient.AddressID != txtPatientAddressID.Text ||
        //    patient.City != txtPatientCity.Text ||
        //    patient.Email != txtPatientEmail.Text ||
        //    patient.PatientAddress != txtPatientAddressLine1.Text ||
        //    patient.Phone != txtPatientPhone.Text ||
        //    patient.State != txtPatientState.Text ||
        //    patient.Zip != txtPatientZip.Text)
        //        bPatientChanged = true;

        //    SaveSprocs("PatientInfo", false);
        //}

        //// order Summary
        //// Patient Info
        //// Retrieve Total Amount
        //decimal orderTotal = 0;

        //// Put code here
        //foreach (GridDataItem gi in grdOrders.Items)
        //{
        //    //Update the Extended Price Field in
        //    var orderDetail = (from c in hm.AS_Alere_OrderDetails
        //                       where c.OrderID.Trim() == txtOrderID.Text.Trim() && c.ItemSKU.Trim() == gi["ItemSKU"].Text
        //                       select c).First();

        //    orderDetail.ExtendedPrice = Convert.ToDecimal(gi["ExtendedPrice"].Text);

        //    orderTotal += Convert.ToDecimal(gi["ExtendedPrice"].Text);
        //}

        //int orderSummaryInfo = (from c in hm.AS_Alere_Orders
        //                        where c.OrderID.Trim() == txtOrderID.Text.Trim()
        //                        select c).Count();

        //if (orderSummaryInfo == 0)
        //{
        //    AS_Alere_Order order = new AS_Alere_Order
        //    {
        //        PatientID = txtPatientID.Text,
        //        OrderID = txtOrderID.Text,
        //        TotalAmount = orderTotal,
        //        WorkflowState = "Saved",
        //        ShippingMethodID = cboShipMethod.SelectedItem.Text,
        //        SuppliesNotNeeded = chkSuppliesNotNeeded.Checked,
        //        CreatedBy = Session["LoggedInUser"].ToString(),
        //        DoctorChanged = bDoctorChanged,
        //        InsuranceChanged = bInsuranceChanged,
        //        PatientChanged = bPatientChanged,
        //        NoteColumn = txtOrderNote.Text,
        //        BasicInfoNoteColumn = txtBasicInfoNote.Text,
        //        InsuranceInfoNoteColumn = txtInsuranceNote.Text,
        //        DoctorInfoNoteColumn = txtDoctorNote.Text,
        //        PatientInfoNoteColumn = txtPatientNote.Text
        //    };

        //    hm.AS_Alere_Orders.InsertOnSubmit(order);
        //}
        //else
        //{
        //    var order = (from c in hm.AS_Alere_Orders
        //                 where c.OrderID == txtOrderID.Text
        //                 select c).First();

        //    order.PatientID = txtPatientID.Text;
        //    order.TotalAmount = orderTotal;
        //    order.WorkflowState = "Saved";
        //    order.CreatedBy = Session["LoggedInUser"].ToString();
        //    order.ShippingMethodID = cboShipMethod.SelectedItem.Text;
        //    order.SuppliesNotNeeded = chkSuppliesNotNeeded.Checked;
        //    order.DoctorChanged = bDoctorChanged;
        //    order.InsuranceChanged = bInsuranceChanged;
        //    order.PatientChanged = bInsuranceChanged;
        //    order.NoteColumn = txtOrderNote.Text;
        //    order.BasicInfoNoteColumn = txtBasicInfoNote.Text;
        //    order.InsuranceInfoNoteColumn = txtInsuranceNote.Text;
        //    order.DoctorInfoNoteColumn = txtDoctorNote.Text;
        //    order.PatientInfoNoteColumn = txtPatientNote.Text;
        //}

        //try
        //{
        //    hm.SubmitChanges();

        //    rtn = true;
        //}
        //catch (Exception ex)
        //{
        //    LogError(ex.Message);
        //}

        return rtn;
    }

    private void SaveSprocs(string pAccountType, bool pIsNewRecord)
    {
        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        //SqlCommand cmd = new SqlCommand();
        //try
        //{
        //    switch (pAccountType)
        //    {
        //        case "BasicInfo":
        //            cmd = new SqlCommand("AS_WF_SP_AlereUpdateBasicInfo", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
        //            cmd.Parameters.AddWithValue("@pFirstName", txtFirstName.Text);
        //            cmd.Parameters.AddWithValue("@pLastName", txtLastName.Text);
        //            cmd.Parameters.AddWithValue("@pDOB", dtDOB.SelectedDate);
        //            cmd.Parameters.AddWithValue("@pCustomerClass", txtPrimaryPhone.Text);
        //            cmd.Parameters.AddWithValue("@pLastShipDate", dtLastShipDate.SelectedDate == null ? dtLastShipDate.MinDate : dtLastShipDate.SelectedDate);
        //            cmd.Parameters.AddWithValue("@pMeterType", txtPrimaryDx.Text);
        //            cmd.Parameters.AddWithValue("@pLancetType", txtSecondaryDx.Text);
        //            cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);
        //            cmd.Parameters.AddWithValue("@pDueDate", dtDueDate.SelectedDate);

        //            connection.Open();

        //            cmd.ExecuteNonQuery();
        //            break;

        //        case "DoctorInfo":
        //            cmd = new SqlCommand("AS_WF_SP_AlereUpdateDoctorInfo", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
        //            cmd.Parameters.AddWithValue("@pDoctorName", txtDoctorName.Text);
        //            cmd.Parameters.AddWithValue("@pPhoneNumber", txtDoctorPhone.Text);
        //            cmd.Parameters.AddWithValue("@pCity", txtDoctorCity.Text);
        //            cmd.Parameters.AddWithValue("@pState", txtDoctorState.Text);
        //            cmd.Parameters.AddWithValue("@pZip", txtDoctorZip.Text);
        //            cmd.Parameters.AddWithValue("@pClinic", txtClinic.Text);
        //            cmd.Parameters.AddWithValue("@pClinicCode", txtClinicCode.Text);
        //            cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);

        //            connection.Open();

        //            cmd.ExecuteNonQuery();
        //            break;

        //        case "InsuranceInfo":
        //            cmd = new SqlCommand("AS_WF_SP_AlereUpdateInsuranceInfo", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryPlan", txtPrimaryPlan.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryPolicyNumber", txtPrimaryPolicyNumber.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryGroupNumber", txtPrimaryGroupNumber.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryClaimPhoneNumber", txtPrimaryPhone.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryAddress", txtPrimaryAddress.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryCity", txtPrimaryCity.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryState", txtPrimaryState.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryZip", txtPrimaryZip.Text);
        //            cmd.Parameters.AddWithValue("@pPrimaryEffectivityDate", dtPrimaryEffectivityDate.SelectedDate == null ? dtPrimaryEffectivityDate.MinDate : dtPrimaryEffectivityDate.SelectedDate);
        //            cmd.Parameters.AddWithValue("@pSecondaryPlan", txtSecondaryPlan.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryPolicyNumber", txtSecondaryPolicyNumber.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryGroupNumber", txtSecondaryGroupNumber.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryClaimPhoneNumber", txtSecondaryPhone.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryAddress", txtSecondaryAddress.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryCity", txtSecondaryCity.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryState", txtSecondaryState.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryZip", txtSecondaryZip.Text);
        //            cmd.Parameters.AddWithValue("@pSecondaryEffectivityDate", dtSecondaryEffectivityDate.SelectedDate == null ? dtSecondaryEffectivityDate.MinDate : dtSecondaryEffectivityDate.SelectedDate);
        //            cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);
        //            cmd.Parameters.AddWithValue("@pSupplyOrder", btnSupplyOrderYes.Checked);

        //            connection.Open();

        //            cmd.ExecuteNonQuery();
        //            break;

        //        case "PatientInfo":
        //            cmd = new SqlCommand("AS_WF_SP_AlereUpdatePatientInfo", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@pPatientID", txtPatientID.Text);
        //            cmd.Parameters.AddWithValue("@pAddressID", txtPatientAddressID.Text);
        //            cmd.Parameters.AddWithValue("@pPatientAddress", txtPatientAddressLine1.Text);
        //            cmd.Parameters.AddWithValue("@pCity", txtPatientCity.Text);
        //            cmd.Parameters.AddWithValue("@pState", txtPatientState.Text);
        //            cmd.Parameters.AddWithValue("@pZip", txtPatientZip.Text);
        //            cmd.Parameters.AddWithValue("@pPhone", txtPatientPhone.Text);
        //            cmd.Parameters.AddWithValue("@pEmail", txtPatientEmail.Text);
        //            cmd.Parameters.AddWithValue("@pIsNewRecord", pIsNewRecord);

        //            connection.Open();

        //            cmd.ExecuteNonQuery();
        //            break;

        //    }

        //}

        //catch (Exception ex)
        //{
        //}
        //finally
        //{
        //    connection.Close();
        //}
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
        //// Basic Info
        //txtPatientID.Enabled = pEnable;
        //btnPatientIDLookup.Enabled = pEnable;
        //txtFirstName.Enabled = pEnable;
        //txtLastName.Enabled = pEnable;
        //dtDOB.Enabled = pEnable;
        ////txtCustomerClass.Enabled = pEnable;
        //dtLastShipDate.Enabled = pEnable;
        ////txtMeterType.Enabled = pEnable;
        ////txtLancetType.Enabled = pEnable;
        //txtBasicInfoNote.Enabled = pEnable;

        //// Insurance Info
        //txtPrimaryPlan.Enabled = pEnable;
        //txtPrimaryPolicyNumber.Enabled = pEnable;
        //txtPrimaryGroupNumber.Enabled = pEnable;
        //txtPrimaryAddress.Enabled = pEnable;
        //txtPrimaryPhone.Enabled = pEnable;
        //txtPrimaryCity.Enabled = pEnable;
        //txtPrimaryState.Enabled = pEnable;
        //txtPrimaryZip.Enabled = pEnable;
        //dtPrimaryEffectivityDate.Enabled = pEnable;
        //txtSecondaryPlan.Enabled = pEnable;
        //txtSecondaryPolicyNumber.Enabled = pEnable;
        //txtSecondaryGroupNumber.Enabled = pEnable;
        //txtSecondaryAddress.Enabled = pEnable;
        //txtSecondaryPhone.Enabled = pEnable;
        //txtSecondaryCity.Enabled = pEnable;
        //txtSecondaryState.Enabled = pEnable;
        //txtSecondaryZip.Enabled = pEnable;
        //dtSecondaryEffectivityDate.Enabled = pEnable;
        //txtInsuranceNote.Enabled = pEnable;

        //// Doctor Info
        //txtDoctorName.Enabled = pEnable;
        //txtDoctorPhone.Enabled = pEnable;
        //txtDoctorCity.Enabled = pEnable;
        //txtDoctorState.Enabled = pEnable;
        //txtDoctorZip.Enabled = pEnable;
        //txtClinic.Enabled = pEnable;
        //txtClinicCode.Enabled = pEnable;
        //txtDoctorNote.Enabled = pEnable;

        //// Patient Info
        //txtPatientAddressID.Enabled = pEnable;
        //txtPatientAddressLine1.Enabled = pEnable;
        //txtPatientCity.Enabled = pEnable;
        //txtPatientState.Enabled = pEnable;
        //txtPatientZip.Enabled = pEnable;
        //txtPatientPhone.Enabled = pEnable;
        //txtPatientEmail.Enabled = pEnable;
        //txtPatientNote.Enabled = pEnable;

        //// Order Items
        //chkSuppliesNotNeeded.Enabled = pEnable;
        //grdOrders.Enabled = pEnable;
        //cboShipMethod.Enabled = pEnable;
        //txtOrderNote.Enabled = pEnable;

        //btnSave.Enabled = pEnable;
        //btnProcess.Enabled = pEnable;
    }

    private DataSet RetrieveAlereData(string pPatientID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"EXEC [S-LVM-DB01\CV_TRAIN].[AHM].[dbo].[usp_AHM_Alba_Patient_Detail]";
                cmd.Parameters.AddWithValue("@Patient_Number", pPatientID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    // Fill the DataSet using default values for DataTable names, etc
                    DataSet dataset = new DataSet();
                    da.Fill(dataset);

                    return dataset;
                }
            }
        }
        catch (Exception ex)
        {
        }
        return null;
    }

    #endregion Methods

    #region Events

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
                if (!chkBasicInfoAllCorrectInsurance.Checked)
                {
                    RadWindowManager1.RadAlert("You cannot continue until you verify that the information is correct.", 330, 100, "Notification", "");
                    return;
                }
                break;

            case 2:
                if (!chkBasicInfoAllCorrectDoctor.Checked)
                {
                    RadWindowManager1.RadAlert("You cannot continue until you verify that the information is correct.", 330, 100, "Notification", "");
                    return;
                }
                break;

            case 3:
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
            string closescript = "<script>ErrorPage('There are no items that were added. Make sure to check the NO SUPPLIES NEEDED checkbox if you will not add items')</" + "script>";
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
            string closescript = "<script>ErrorPage('There are no items that were added. Make sure to check the NO SUPPLIES NEEDED checkbox if you will not add items')</" + "script>";
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

    protected void OnSelectedIndexChangedHandler(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["ItemSKU"] = e.Value;

        string sql = "SELECT IV00101.ITEMDESC, IV00108.UOMPRICE FROM IV00101 LEFT OUTER JOIN IV00108 ON IV00101.ITEMNMBR = IV00108.ITEMNMBR " +
            "AND IV00101.PRCLEVEL = IV00108.PRCLEVEL AND IV00101.PRCLEVEL = IV00108.PRCLEVEL AND IV00101.SELNGUOM = IV00108.UOFM WHERE IV00101.ITEMNMBR = @ITEMNMBR";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ITEMNMBR", e.Value.Trim());

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            GridEditableItem editedItem = (sender as RadComboBox).NamingContainer as GridEditableItem;

            TextBox txtItemSKU = editedItem["ItemSKU"].Controls[0] as TextBox;
            txtItemSKU.Text = e.Value.ToString();

            while (reader.Read())
            {
                TextBox txtItemDesc = editedItem["ITEMDESC"].Controls[0] as TextBox;
                txtItemDesc.Text = reader["ITEMDESC"].ToString();

                RadNumericTextBox txtUnitPrice = editedItem["UnitPrice"].Controls[0] as RadNumericTextBox;
                txtUnitPrice.Text = reader["UOMPRICE"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
    }

    #endregion Events
}