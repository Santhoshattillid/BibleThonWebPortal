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

public partial class frmGaiamChargebackDocument : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

    string pathToCreate;
    private string searchString;
    private GPCustomer customer;
    private string createdByUser;

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
                    txtAuthorizationNumber.Text = Request.QueryString["DocumentKey"];
                    Session["IsNewRecord"] = false;
                    LoadDocument();                    
                }
                else
                {
                    string outString = Helper.GetNextSequenceNumber("GAIAM Chargeback Document");

                    Session["AuthorizationNumber"] = outString;
                    Session["IsNewRecord"] = true;

                    txtAuthorizationNumber.Text = outString;
                    txtStatus.Text = "New";
                }
                
                if (!string.IsNullOrEmpty(Request.QueryString["LoggedInUser"]))
                    Session["LoggedInUser"] = Request.QueryString["LoggedInUser"];

                RadGrid1.Rebind();
                SelectFirstGridRow();

                pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString();
                Session["AttachmentFilePath"] = Server.MapPath(pathToCreate);

                if (!Directory.Exists(Server.MapPath(pathToCreate)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathToCreate));
                }

                lblUser.Text += Session["LoggedInUser"].ToString();

                Session["AccountNumber"] = txtAccountNumber.Text;
                Session["VendorID"] = txtVendorID.Text;
            }
            else
            {
                if (Session["AuthorizationNumber"] == null)
                {
                    RadWindowManager1.RadAlert("Your session has expired. Please login again.", 330, 100, "Notification", "");
                    Response.Redirect("Logout.aspx");
                }

                if (Session["AccountNumber"].ToString() != txtAccountNumber.Text && !string.IsNullOrEmpty(txtAccountNumber.Text))
                {
                    LoadAccountDetails();
                    Session["AccountNumber"] = txtAccountNumber.Text;
                }

                if (Session["VendorID"].ToString() != txtVendorID.Text && !string.IsNullOrEmpty(txtVendorID.Text))
                {
                    LoadVendorDetails();
                    Session["VendorID"] = txtVendorID.Text;
                }

                if (!string.IsNullOrEmpty(txtAuthorizationNumber.Text))
                {
                    //if ((bool)Session["IsNewRecord"] != true && Session["AuthorizationNumber"].ToString() != txtAuthorizationNumber.Text)
                    if (Session["AuthorizationNumber"].ToString() != txtAuthorizationNumber.Text)
                    {
                        Session["IsNewRecord"] = false;
                        Session["AuthorizationNumber"] = txtAuthorizationNumber.Text;
                        LoadDocument();
                    }
                }
            }

            //Moved here because the uploading of the document causes a postback.
            if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision" && txtStatus.Text != "New")
                DisableControls(false);
            else if (txtStatus.Text == "Saved" && Helper.RetrieveWFCommentFromDocKeyString(txtAuthorizationNumber.Text) != string.Empty)
                DisableControls(true);
            else if (txtStatus.Text == "Revision")
                DisableControls(true);
        }
        catch (Exception ex)
        {
            log.Error("frmGaiamChargebackDocument Error encountered in Page_Load: ", ex);
        }
    }    

    #region Events

    protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
    {

        //RadGrid1.Rebind();
        RadGrid1.Rebind();
        SelectFirstGridRow();

    }

    protected void cboExpenseType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //cboExpenseSubType.Items.Clear();

        //switch (e.Value)
        //{
        //    case "Retail Marketing":
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Brand Manager"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Custom Event"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Custom Packaging"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Gift W/ Purchase"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Merch Services"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Signage"));
        //        break;
        //    case "Measurable Media":
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Circular Ad"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Distributor Mailer"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("In-Store TV"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Online Ads"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Print Ad"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Radio Ad"));
        //        break;
        //    case "Performance Incentives":
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Performance Incentives"));
        //        break;
        //    case "Placement Fee":
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Placement Fee"));
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Sales Incentive"));
        //        break;
        //    case "Price Protection":                
        //        cboExpenseSubType.Items.Add(new RadComboBoxItem("Price Protection"));                
        //        break;
        //}

        PopulateExpenseSubTypes(e.Text);
    }

    protected void RadToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
    {
        if ((e.Item).Text == "Save")
        {
            SaveChargebackDocument();

            Session["AuthorizationNumber"] = null;

            string closescript = "<script>returnToParent('Record has been saved')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
        else if ((e.Item).Text == "Submit")
        {
            if (txtStatus.Text == "Revision")
            {
                GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
                string totalGridAmount = (footer["TotalAmount"].Text.Split(':')[1].Trim().Replace("$", string.Empty));

                if (Convert.ToDecimal(totalGridAmount) != Convert.ToDecimal(txtClaimedAmount.Text))
                {
                    //Removed as per Davis' request
                    RadWindowManager1.RadAlert("You cannot continue until the Claimed Amount is equal to the Current Total Amount.", 330, 100, "Notification", "");

                    //return;
                }

                if (txtClaimedAmount.Text == "0")
                {
                    //Removed as per Davis' request
                    //RadWindowManager1.RadAlert("Claimed amount cannot be zero.", 330, 100, "Notification", "");
                    //return;
                }
            }

            //Verify Customer exists in GP
            if (!Helper.IsCustomerNumberValid(txtAccountNumber.Text))
            {
                RadWindowManager1.RadAlert("Account number is not valid/does not exist in GP.", 330, 100, "Notification", "");

                return;
            }

            //Verify SalesPerson exists in GP
            if (!Helper.IsSalesPersonValid(txtSalesPerson.Text))
            {
                RadWindowManager1.RadAlert("Salesperson ID is not valid/does not exist in GP.", 330, 100, "Notification", "");

                return;
            }

            //Verify Items exist
            if (RadGrid1.Items.Count == 0)
            {
                RadWindowManager1.RadAlert("There are no items in the Details section. Please upload items before submitting the document.", 330, 100, "Notification", "");

                return;
            }

            //Verify if all items are valid
            //RadGrid1.AllowPaging = false;
            //RadGrid1.Rebind();

            //foreach (GridDataItem item in RadGrid1.Items)
            //{
            //    if (item["InvalidText"].Text.ToString() == "INVALID" || item["InvalidText"].Text.ToString() == "ZERO")
            //    {
            //        RadWindowManager1.RadAlert("There are items in the Details section that are invalid. Please check the items marked as Invalid or Zero and then try submitting again.", 330, 100, "Notification", "");

            //        RadGrid1.AllowPaging = true;
            //        RadGrid1.Rebind();

            //        return;
            //    }
            //} 
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

            try
            {
                string sql = "SELECT COUNT(*) FROM [AS_GAIAM_Chargeback_Detail] WHERE AuthorizationNumber = @AuthorizationNumber AND (INVALIDTEXT = 'ZERO' OR INVALIDTEXT = 'INVALID')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AuthorizationNumber", txtAuthorizationNumber.Text);

                if(conn.State == ConnectionState.Closed)
                    conn.Open();

                object obj = cmd.ExecuteScalar();

                if (Convert.ToInt32(obj) > 0)
                {
                    RadWindowManager1.RadAlert("There are items in the Details section that are invalid. Please check the items marked as Invalid or Zero and then try submitting again.", 330, 100, "Notification", "");
                    return;
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




            RadGrid1.AllowPaging = true;
            RadGrid1.Rebind();

            //Save first
            SaveChargebackDocument();

            //Then submit
            SubmitChargebackDocument();

            Session["AuthorizationNumber"] = null;

            string closescript = "<script>returnToParent('Record has been submitted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
        }
        else if ((e.Item).Text == "Export Items to Excel")
        {
            DownloadExcelData(false);
        }
        else if ((e.Item).Text == "Delete")
        {
            //RadWindowManager1.RadConfirm("Are you sure you want to delete?", 
            //"There are items in the Details section that are invalid. Please check the items marked as Invalid or Zero and then try submitting again.", 330, 100, "Notification", "");

            DeleteChargebackDocument();

            Session["AuthorizationNumber"] = null;

            string closescript = "<script>returnToParent('Record has been deleted')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Deleted", closescript, false);
        }
    }    

    protected void repeaterResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HyperLink hypl = (HyperLink)e.Item.FindControl("HyperLink1");
        if (hypl != null)
        {
            //hypl.Text = ar.ToString();
            pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString();
            hypl.NavigateUrl = pathToCreate + "/" + hypl.Text;
        }
    }

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {
        BindValidResults();
        BindInvalidResults();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            e.Canceled = true;
            System.Collections.Specialized.ListDictionary newValues = new System.Collections.Specialized.ListDictionary();
            newValues["AuthorizationNumber"] = Session["AuthorizationNumber"].ToString();
            newValues["AmountPerUnit"] = 0;
            newValues["Quantity"] = 0;
            newValues["TotalAmount"] = 0;
            newValues["AuthorizedAmount"] = 0;
            //Insert the item and rebind
            e.Item.OwnerTableView.InsertItem(newValues);
        }
        else if (e.CommandName == "ExportDocToExcel")
        {
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //foreach (GridColumn col in RadGrid1.MasterTableView.Columns)
            //{
            //    col.Visible = true;
            //}
            //RadGrid1.Rebind();

            //RadGrid1.MasterTableView.ExportToExcel();

            DownloadExcelData(false);

        }
        else if (e.CommandName == "ImportDocFromExcel")
        {
            //string closescript = "<script>openImportWin('" + txtAuthorizationNumber.Text + "')</" + "script>";
            //string closescript = "<script>openImportWin(); return false;</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Record Saved", closescript, false);
            RadWindow2.NavigateUrl = "GaiamChargeBackImportData.aspx?AuthorizationNumber=" + txtAuthorizationNumber.Text;

            string script = "<script language='javascript' type='text/javascript'>Sys.Application.add_load(ShowWindow);</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowWindow", script); 
        }
        else if (e.CommandName == "DownloadTemplate")
        {
            DownloadExcelData(true);

        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            editform["AuthorizationNumber"].Parent.Visible = false;
            editform["GAIAMItemNumber"].Parent.Visible = false;
            editform["TitlePromotion"].Parent.Visible = false;
            editform["UPCCode"].Parent.Visible = false;
            editform["Studio"].Parent.Visible = false;
            editform["TotalAmount"].Parent.Visible = false;
            editform["InvalidText"].Parent.Visible = false;
        }

        if (!(e.Item.DataItem is GridInsertionObject) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            editform["cboItemSKU"].Parent.Visible = false;
            editform["UPCCode"].Parent.Visible = false;
        }

        if ((e.Item is GridDataItem))
        {
            string itemNumber = ((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).GAIAMItemNumber;
            string UPCCode = ((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).UPCCode;
            int qty = ((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).Quantity == null ? 0 : (int)((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).Quantity;
            decimal amtPerUnit = ((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).AmountPerUnit == null ? 0 : (decimal)((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).AmountPerUnit;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

            try
            {
                //if (!Helper.IsItemSKUValid(itemNumber, conn) ||
                //    !Helper.IsItemShortNameValid(itemNumber, UPCCode, conn))
                //{
                //    GridDataItem gItem = (GridDataItem)e.Item;
                //    //gItem["InvalidSKU"].Text = "Invalid";
                //}
                //else if (qty == 0 || amtPerUnit == 0)
                //{
                //    GridDataItem gItem = (GridDataItem)e.Item;
                //    //gItem["InvalidSKU"].Text = "Zero";
                //}
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
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        searchString = e.Argument.ToLower();
        RadGrid1.Rebind();
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;

            //Compute the total
            Telerik.Web.UI.RadNumericTextBox tb = editform["TotalAmount"].Controls[0] as Telerik.Web.UI.RadNumericTextBox;
            tb.Text = (Convert.ToDecimal(((Telerik.Web.UI.RadNumericTextBox)(((System.Web.UI.Control)(editform["Quantity"])).Controls[0])).Text) * Convert.ToDecimal(((Telerik.Web.UI.RadNumericTextBox)(((System.Web.UI.Control)(editform["AmountPerUnit"])).Controls[0])).Text)).ToString();
           
        }
        
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        //{
        //    GridEditFormItem editform = (GridEditFormItem)e.Item;

        //    //Compute the Total
        //    Telerik.Web.UI.RadNumericTextBox tb = editform["TotalAmount"].Controls[0] as Telerik.Web.UI.RadNumericTextBox;
        //    tb.Text = (Convert.ToDecimal(((Telerik.Web.UI.RadNumericTextBox)(((System.Web.UI.Control)(editform["Quantity"])).Controls[0])).Text) * Convert.ToDecimal(((Telerik.Web.UI.RadNumericTextBox)(((System.Web.UI.Control)(editform["AmountPerUnit"])).Controls[0])).Text)).ToString();

        //    //Retrieve UPC and Studio and Title
        //    Telerik.Web.UI.RadTextBox tbUPC = editform["UPCCode"].Controls[0] as Telerik.Web.UI.RadTextBox;
        //    tbUPC.Text = Helper.RetrieveItemShortName(((Telerik.Web.UI.RadTextBox)(((System.Web.UI.Control)(editform["GAIAMItemNumber"])).Controls[0])).Text);

        //    Telerik.Web.UI.RadTextBox tbStudio = editform["Studio"].Controls[0] as Telerik.Web.UI.RadTextBox;
        //    tbStudio.Text = Helper.RetrieveExtenderStudioField(((Telerik.Web.UI.RadTextBox)(((System.Web.UI.Control)(editform["GAIAMItemNumber"])).Controls[0])).Text);

        //    Telerik.Web.UI.RadTextBox tbTitle = editform["TitlePromotion"].Controls[0] as Telerik.Web.UI.RadTextBox;
        //    tbTitle.Text = Helper.RetrieveItemDescription(((Telerik.Web.UI.RadTextBox)(((System.Web.UI.Control)(editform["GAIAMItemNumber"])).Controls[0])).Text);
        //}
    }

    protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }       


        if ((e.Item is GridEditFormItem))
        {
            GridEditFormItem editItem = e.Item as GridEditFormItem;             

            string itemNumber = ((TextBox)(editItem["GAIAMItemNumber"].Controls[0])).Text;
            string UPCCode = ((TextBox)(editItem["UPCCode"].Controls[0])).Text;
            int qty = Convert.ToInt32(((RadNumericTextBox)(editItem["Quantity"].Controls[0])).Text);
            decimal amtPerUnit = Convert.ToDecimal(((RadNumericTextBox)(editItem["AmountPerUnit"].Controls[0])).Text);

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

            try
            {
                string invalidText = string.Empty;

                if (!Helper.IsItemSKUValid(itemNumber, conn) ||
                    !Helper.IsItemShortNameValid(itemNumber, UPCCode, conn))
                {
                    invalidText = "INVALID";
                }
                else if (qty == 0 || amtPerUnit == 0)
                {
                    invalidText = "ZERO";
                }

                ChargebackDataDataContext cbd = new ChargebackDataDataContext();

                var chargebackDetails = (from s in cbd.AS_GAIAM_Chargeback_Details
                                         where s.AuthorizationNumber == txtAuthorizationNumber.Text && s.GAIAMItemNumber == itemNumber
                                         select s).First();

                chargebackDetails.InvalidText = invalidText;

                cbd.SubmitChanges();
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

        RadGrid1.Rebind();

        GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (!string.IsNullOrEmpty(footer["TotalAmount"].Text.Split(':')[1].Trim()))
        {
            txtTotalAmount.Text = (footer["TotalAmount"].Text.Split(':')[1].Replace("$", string.Empty));
            txtClaimedAmount.Text = txtTotalAmount.Text;
        }
    }

    protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }

        if ((e.Item is GridDataItem))
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
            try
            {
                if (!Helper.IsItemSKUValid(((AS_GAIAM_Chargeback_Detail)(e.Item.DataItem)).GAIAMItemNumber, conn))
                {
                    GridDataItem gItem = (GridDataItem)e.Item;
                    //gItem["InvalidSKU"].Text = "Invalid";
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

        RadGrid1.Rebind();

        GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (!string.IsNullOrEmpty(footer["TotalAmount"].Text.Split(':')[1].Trim()))
        {
            txtTotalAmount.Text = (footer["TotalAmount"].Text.Split(':')[1].Replace("$", string.Empty));
            txtClaimedAmount.Text = txtTotalAmount.Text;
        }
    }

    protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            ShowErrorMessage();
        }

        RadGrid1.Rebind();

        GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (!string.IsNullOrEmpty(footer["TotalAmount"].Text.Split(':')[1].Trim()))
        {
            txtTotalAmount.Text = (footer["TotalAmount"].Text.Split(':')[1].Replace("$", string.Empty));
            txtClaimedAmount.Text = txtTotalAmount.Text;
        }
    }

    protected void chkExpensePaidByInvoice_CheckedChanged(object sender, EventArgs e)
    {
        bool bl = chkExpensePaidByInvoice.Checked;
        lblVendorID.Visible = bl;
        txtVendorID.Visible = bl;
        lblVendorName.Visible = bl;
        txtVendorName.Visible = bl;
        btnVendorLookup.Visible = bl;

        if(!bl)
        {
            txtVendorID.Text = string.Empty;
            txtVendorName.Text = string.Empty;
        }
    }

    protected void chkEstimate_CheckedChanged(object sender, EventArgs e)
    {
        //txtTotalAmount.ReadOnly = !chkEstimate.Checked;
    }    

    protected void cboItemSKU_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        #region OLD CODE

        //try
        //{
        //    // Create an eConnect document type object
        //    eConnectType myEConnectType = new eConnectType();

        //    // Create a RQeConnectOutType schema object
        //    RQeConnectOutType myReqType = new RQeConnectOutType();

        //    // Create an eConnectOut XML node object
        //    eConnectOut myeConnectOut = new eConnectOut();

        //    // Populate the eConnectOut XML node elements            
        //    myeConnectOut.ACTION = 0;
        //    myeConnectOut.DOCTYPE = "Item";
        //    myeConnectOut.OUTPUTTYPE = 2;
        //    myeConnectOut.FORLIST = 1;

        //    // Add the eConnectOut XML node object to the RQeConnectOutType schema object
        //    myReqType.eConnectOut = myeConnectOut;

        //    // Add the RQeConnectOutType schema object to the eConnect document object
        //    RQeConnectOutType[] myReqOutType = { myReqType };
        //    myEConnectType.RQeConnectOutType = myReqOutType;

        //    // Serialize the eConnect document object to a memory stream
        //    MemoryStream myMemStream = new MemoryStream();
        //    XmlSerializer mySerializer = new XmlSerializer(myEConnectType.GetType());
        //    mySerializer.Serialize(myMemStream, myEConnectType);
        //    myMemStream.Position = 0;

        //    // Load the serialized eConnect document object into an XML document object
        //    XmlTextReader xmlreader = new XmlTextReader(myMemStream);
        //    XmlDocument myXmlDocument = new XmlDocument();
        //    myXmlDocument.Load(xmlreader);

        //    // Create a connection string to specify the Microsoft Dynamics GP server and database
        //    // Change the data source and initial catalog to specify your server and database
        //    //string sConnectionString = @"data source=RIC-ASUS\ALBA;initial catalog=TWO;integrated security=SSPI;persist security info=False;packet size=4096";
        //    string sConnectionString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString; 

        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sConnectionString);
        //    string ds = builder.DataSource;
        //    string catalog = builder.InitialCatalog;

        //    string strConnection = "data source=" + ds + ";initial catalog=" + catalog + ";integrated security=SSPI;persist security info=False;packet size=4096;";

        //    // Create an eConnectMethods object
        //    eConnectMethods requester = new eConnectMethods();

        //    // Call the eConnect_Requester method of the eConnectMethods object to retrieve specified XML data
        //    string reqDoc = requester.eConnect_Requester(strConnection, EnumTypes.ConnectionStringType.SqlClient, myXmlDocument.OuterXml);

        //    // Display the result of the eConnect_Requester method call
        //    //Console.Write(reqDoc);       
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(reqDoc);
        //    XmlNodeList nl = doc.SelectNodes("/root/eConnect/Item");

        //    RadComboBox comboBox = (RadComboBox)sender;
        //    // Clear the default Item that has been re-created from ViewState at this point.
        //    comboBox.Items.Clear();

        //    foreach (XmlNode node in nl)
        //    {
        //        RadComboBoxItem item = new RadComboBoxItem();

        //        foreach (XmlNode nodeSub in node.ChildNodes)
        //        {
        //            if (nodeSub.Name == "ITEMNMBR")
        //            {
        //                item.Text = nodeSub.InnerText;
        //                item.Value = nodeSub.InnerText;
        //            }
        //            else if (nodeSub.Name == "ITEMDESC")
        //            {
        //                item.Attributes.Add("ITEMDESC", nodeSub.InnerText);
        //            }                    
        //        }
        //        comboBox.Items.Add(item);
        //        item.DataBind();
        //    }


        //    //string sql = "SELECT [ITEMNMBR] as ItemSKU, ITEMDESC FROM [IV00101] WHERE ([USCATVLS_4] = 'True')";
        //    //SqlDataAdapter adapter = new SqlDataAdapter(sql, ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

        //    //DataTable dt = new DataTable();
        //    //adapter.Fill(dt);


        //    //foreach (DataRow row in dt.Rows)


        //}
        //catch (Exception ex)
        //{// Dislay any errors that occur to the console
        //    //Console.Write(ex.ToString());
        //}

        #endregion

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

        try
        {            
            string sql = "SELECT TOP 10 [ITEMNMBR] as ItemSKU, ITEMDESC FROM [IV00101]";
            SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            RadComboBox comboBox = (RadComboBox)sender;
            comboBox.Items.Clear();

            while (reader.Read())
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = reader["ItemSKU"].ToString();
                item.Value = reader["ItemSKU"].ToString();

                item.Attributes.Add("ITEMDESC", reader["ITEMDESC"].ToString());

                comboBox.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }

    protected void OnSelectedIndexChangedHandler(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {        
        try
        {
            
            GridEditableItem editedItem = (sender as RadComboBox).NamingContainer as GridEditableItem;

            TextBox txtItemSKU = editedItem["GAIAMItemNumber"].Controls[0] as TextBox;
            txtItemSKU.Text = e.Value.ToString();

            TextBox txtTitlePromotion = editedItem["TitlePromotion"].Controls[0] as TextBox;
            txtTitlePromotion.Text = Helper.RetrieveItemDescription(e.Value.ToString());

            TextBox txtUPC = editedItem["UPCCode"].Controls[0] as TextBox;
            txtUPC.Text = Helper.RetrieveItemShortName(e.Value.ToString());

            //TextBox txtStudio = editedItem["Studio"].Controls[0] as TextBox;
            //txtStudio.Text = Helper.RetrieveExtenderStudioField(e.Value.ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }        

    }

    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        if (txtStatus.Text != "Revision" && txtStatus.Text != "Submitted")
        {
            GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (!string.IsNullOrEmpty(footer["TotalAmount"].Text.Split(':')[1].Trim()))
            {
                string strTotalAmount = footer["TotalAmount"].Text.Split(':')[1].Replace("$", string.Empty);

                decimal decTotalAmount = strTotalAmount.Contains("(") ? Convert.ToDecimal(strTotalAmount.Replace("(", string.Empty).Replace(")", string.Empty)) * (-1) : Convert.ToDecimal(strTotalAmount);


                txtTotalAmount.Text = decTotalAmount.ToString();
                txtClaimedAmount.Text = txtTotalAmount.Text;
            }
        }
        else if (txtStatus.Text == "Revision" && !chkEstimate.Checked)
        {
            GridFooterItem footer = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
            //txtClaimedAmount.Text = (footer["TotalAmount"].Text.Split(':')[1]);
        }


    }

    protected void RadGrid1_BiffExporting(object sender, GridBiffExportingEventArgs e)
    {
        RadGrid1.MasterTableView.GetColumn("DeleteButtonCommand").Visible = false;
        RadGrid1.MasterTableView.GetColumn("InvalidText").Visible = true;
    }

    protected void RadGrid1_ExcelMLWorkBookCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExcelMLWorkBookCreatedEventArgs e)
    {
        // e.WorkBook.Worksheets[0].Table.Columns[7].Hidden = true;
        //e.WorkBook.Worksheets[0].Table.Columns[8].Hidden = true;
    }

    protected void RadGrid1_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        if (e.Worksheet.Table.Rows.Count == RadGrid1.Items.Count)
        {
            //foreach(ColumnElement col in ... )
            //{
            //    col.Hidden = true;
            //}
        }

        //e.Row.Cells.GetCellByName("Name").StyleValue = "myCustomStyle";
    }

    protected void RadGrid1_GridExporting(object sender, GridExportingArgs e)
    {
        //RadGrid1.Columns[0].Visible = true;
    }

    protected void LoadAccountDetails()
    {
        customer = Helper.RetrieveCustomerData(txtAccountNumber.Text);

        txtAccountName.Text = customer.CUSTNAME.Trim();
        txtStreet.Text = customer.ADDRESS1;
        txtCity.Text = customer.CITY;
        txtState.Text = customer.STATE;
        txtZip.Text = customer.ZIP;
        txtCountry.Text = customer.COUNTRY;
    }

    protected void LoadVendorDetails()
    {
        txtVendorName.Text = Helper.RetrieveVendorName(txtVendorID.Text);
    }

    protected void cbAccountName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string customerID = Helper.RetrieveCustomerIDFromCustomerName(((Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)(e)).Text);

        customer = Helper.RetrieveCustomerData(customerID);

        txtAccountNumber.Text = customerID;
        txtStreet.Text = customer.ADDRESS1;
        txtCity.Text = customer.CITY;
        txtState.Text = customer.STATE;
        txtZip.Text = customer.ZIP;
        txtCountry.Text = customer.COUNTRY;
    }

    protected void CustomButton_Click(object sender, EventArgs e)
    {
        
    }

    #endregion

    #region Methods

    private void PopulateAccounts()
    {
        //List<string> customerAccounts = Helper.RetrieveCustomerIDs();

        //cbGAIAMAccountNumber.Items.Clear();

        //foreach (string customerAccount in customerAccounts)
        //{
        //    cbGAIAMAccountNumber.Items.Add(new RadComboBoxItem(customerAccount));
        //}
    }

    private void PopulateAccountNames()
    {
        //List<string> customerAccountNames = Helper.RetrieveCustomerNames();

        //cbAccountName.Items.Clear();

        //foreach (string customerAccountName in customerAccountNames)
        //{
        //    cbAccountName.Items.Add(new RadComboBoxItem(customerAccountName.Trim()));
        //}
    }

    private void DisableControls(bool pCanBeEdited)
    {
        try
        {
            string comment = Helper.RetrieveWFCommentFromDocKeyString(txtAuthorizationNumber.Text);

            if (pCanBeEdited)
            {
                if (comment.Contains("LVL1ApproverComment") || comment.Contains("LVL2ApproverComment"))
                {
                    RadToolBar1.Enabled = true;
                    RadUpload1.Enabled = true;

                    if (chkEstimate.Checked)
                        RadGrid1.Enabled = true;

                    txtClaimedAmount.ReadOnly = false;

                    buttonSubmit.Enabled = true;
                    lblTotalAmount.Text = "Authorized Amount";

                    RadPane1.Expanded = true;
                    RadPane1.Enabled = true;
                }
                else if (comment.Contains("FINALLVLApproverComment") || comment.Contains("Please attach chargeback document"))
                {
                    btnLookup.Visible = false;
                    txtAccountNumber.ReadOnly = true;
                    txtAccountName.ReadOnly = true;
                    txtEventName.ReadOnly = true;
                    cboExpenseType.Enabled = false;
                    cboExpenseSubType.Enabled = false;
                    txtPhone.ReadOnly = true;
                    chkIsExpenseStudioApproved.Enabled = false;
                    txtSalesPerson.ReadOnly = true;
                    chkExpensePaidByInvoice.Enabled = false;
                    txtIndirectAccountName.ReadOnly = true;
                    txtVendorID.ReadOnly = true;
                    txtVendorName.ReadOnly = true;
                    btnVendorLookup.Enabled = false;
                    txtStatus.ReadOnly = true;
                    chkExpensePaidByInvoice.Enabled = false;
                    dtStart.Enabled = false;
                    dtEnd.Enabled = false;
                    txtSpecialInstructions.ReadOnly = true;
                    txtTotalAmount.ReadOnly = true;
                    chkEstimate.Enabled = false;
                    btnAccountLookup.Enabled = false;


                    RadToolBar1.Items[0].Enabled = false;
                    RadToolBar1.Enabled = true;
                    RadUpload1.Enabled = true;

                    RadGrid1.Enabled = false;

                    buttonSubmit.Enabled = true;
                    lblTotalAmount.Text = "Authorized Amount";

                    RadPane1.Expanded = true;
                    RadPane1.Enabled = true;
                    if (createdByUser != Session["LoggedInUser"].ToString())
                    {
                        txtClaimedAmount.ReadOnly = false;
                    }
                    else
                    {
                        txtClaimedAmount.ReadOnly = true;
                    }
                }
            }
            else
            {
                RadUpload1.Enabled = false;
                RadToolBar1.Enabled = false;
                RadGrid1.Enabled = false;
                txtClaimedAmount.ReadOnly = true;
                buttonSubmit.Enabled = false;

                RadPane1.Expanded = false;
                RadPane1.Enabled = false;

                btnLookup.Visible = false;
                txtAccountNumber.ReadOnly = true;
                txtAccountName.ReadOnly = true;
                txtEventName.ReadOnly = true;
                cboExpenseType.Enabled = false;
                cboExpenseSubType.Enabled = false;
                txtPhone.ReadOnly = true;
                chkIsExpenseStudioApproved.Enabled = false;
                txtSalesPerson.ReadOnly = true;
                chkExpensePaidByInvoice.Enabled = false;
                txtIndirectAccountName.ReadOnly = true;
                txtVendorID.ReadOnly = true;
                txtVendorName.ReadOnly = true;
                btnVendorLookup.Enabled = false;
                txtStatus.ReadOnly = true;
                chkExpensePaidByInvoice.Enabled = false;
                dtStart.Enabled = false;
                dtEnd.Enabled = false;
                txtSpecialInstructions.ReadOnly = true;
                txtTotalAmount.ReadOnly = true;
                chkEstimate.Enabled = false;
                btnAccountLookup.Enabled = false;                
                
                RadToolBar1.Enabled = true;
                RadPane1.Enabled = true;
                RadToolBar1.Items[0].Enabled = false;
                RadToolBar1.Items[1].Enabled = false;
                RadToolBar1.Items[2].Enabled = true;
                RadToolBar1.Items[3].Enabled = false;
                RadToolBar1.Items[4].Enabled = true;
                RadToolBar1.Items[4].Visible = true;

                if (txtStatus.Text != "Claimed")
                {
                    RadUpload1.Enabled = true;
                    buttonSubmit.Enabled = true;
                }
            }            
        }
        catch (Exception ex)
        {
            log.Error("frmGaiamChargebackDocument Error encountered in DisableControls: ", ex);
        }
        
    }

    private void InitializeControls()
    {
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Brand Manager"));
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Custom Event"));
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Custom Packaging"));
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Gift W/ Purchase"));
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Merch Services"));
        //cboExpenseSubType.Items.Add(new RadComboBoxItem("Signage"));

        PopulateExpenseTypes();
        PopulateAccounts();
        PopulateAccountNames();

        customer = Helper.RetrieveCustomerData(txtAccountNumber.Text);

        txtAccountName.Text = customer.CUSTNAME;
        txtStreet.Text = customer.ADDRESS1;
        txtCity.Text = customer.CITY;
        txtState.Text = customer.STATE;
        txtZip.Text = customer.ZIP;
        txtCountry.Text = customer.COUNTRY;

        WorkflowUser wfUser = Helper.RetrieveUserData(Session["LoggedInUser"].ToString());
        txtSalesPerson.Text = wfUser.FirstName;
    }

    private void LoadDocument()
    {
        try
        {
            using (ChargebackDataDataContext context = new ChargebackDataDataContext())
            {
                //Load Header
                var chargebackHeaders = from s in context.AS_GAIAM_Chargeback_Headers
                                        where
                                            s.AuthorizationNumber == txtAuthorizationNumber.Text
                                        select s;
                log.Debug("1.1");

                foreach (var c in chargebackHeaders)
                {
                    log.Debug("1.1.0");
                    Session["AuthorizationNumber"] = c.AuthorizationNumber;
                    log.Debug("1.1.1");
                    txtAccountName.Text = c.AccountName;
                    log.Debug("1.1.2");
                    //RadComboBoxItem item = cbGAIAMAccountNumber.FindItemByText(c.GAIAMAccountNumber.Trim());
                    //log.Debug("1.1.3");
                    //item.Selected = true;
                    txtAccountNumber.Text = c.GAIAMAccountNumber.Trim();
                    txtEventName.Text = c.EventName;
                    log.Debug("1.1.4");
                    txtStreet.Text = c.AddressStreet;
                    log.Debug("1.1.5");
                    txtCity.Text = c.AddressCity;
                    txtState.Text = c.AddressState;
                    txtZip.Text = c.AddressZip;
                    txtCountry.Text = c.AddressCountry;
                    txtPhone.Text = c.Phone;
                    txtSalesPerson.Text = c.SalesPerson;
                    txtIndirectAccountName.Text = c.IndirectAccountName;
                    txtVendorID.Text = c.VendorID;
                    txtVendorName.Text = c.VendorName;
                    dtStart.SelectedDate = c.DateStartEvent;
                    dtEnd.SelectedDate = c.DateEndEvent;
                    cboExpenseType.SelectedValue = c.ExpenseType;
                    cboExpenseSubType.SelectedValue = c.ExpenseSubType;
                    chkIsExpenseStudioApproved.Checked = (bool)c.IsExpenseStudioApproved;
                    chkExpensePaidByInvoice.Checked = (bool)c.IsExpensePaidByInvoice;
                    chkEstimate.Checked = (bool)c.Estimate;
                    txtTotalAmount.Value = Convert.ToDouble(c.TotalAmount);
                    txtSpecialInstructions.Text = c.SpecialInstructions;
                    txtStatus.Text = c.WorkflowState;
                    txtClaimedAmount.Value = Convert.ToDouble(c.ClaimedAmount);
                    createdByUser = c.CreatedBy;
                }

                log.Debug("1.2");

                if (chkExpensePaidByInvoice.Checked)
                {
                    txtVendorID.Visible = true;
                    txtVendorName.Visible = true;
                    btnVendorLookup.Visible = true;
                }
                else
                {
                    txtVendorID.Visible = false;
                    txtVendorName.Visible = false;
                    btnVendorLookup.Visible = false;
                }


                //Load Detail
                //Detail Automatically loads because of the LINQTOSQL data source. Just needs to set the Session variable authorizationnumber

                log.Debug("1.3");

                //Load attachments
                LoadAttachments();

                //Disable Controls if not saved or for revision
                if (txtStatus.Text != "Saved" && txtStatus.Text != "Revision" && txtStatus.Text != "New")
                    DisableControls(false);
                else if (txtStatus.Text == "Revision")
                    DisableControls(true);
            }
        }
        catch (Exception ex)
        {
            log.Error("frmGAIAMChargebackDocument Error encountered in LoadDocument: ", ex);
        }
    }

    private void LoadAttachments()
    {
        pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString();
        if (Directory.Exists(Server.MapPath(pathToCreate)))
        {
            string[] filelist = Directory.GetFiles(Server.MapPath(pathToCreate));

            DataTable dt = new DataTable();
            dt.Columns.Add("FileName");
            dt.Columns.Add("FilePath");

            foreach (string str in filelist)
            {

                DataRow dr = dt.NewRow();
                string filepath = Path.GetFileName(str);
                dr["FileName"] = filepath;
                dt.Rows.Add(dr);
            }

            repeaterResults.DataSource = dt;
            repeaterResults.DataBind();

            if (dt.Rows.Count > 0)
            {
                labelNoResults.Visible = false;
                repeaterResults.Visible = true;
            }
            else
            {
                labelNoResults.Visible = true;
                repeaterResults.Visible = false;
            }
        }
    }

    private void BindValidResults()
    {
        if (RadUpload1.UploadedFiles.Count > 0)
        {
            foreach (UploadedFile validFile in RadUpload1.UploadedFiles)
            {
                pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString();

                string targetFolder = Server.MapPath(pathToCreate);
                validFile.SaveAs(Path.Combine(targetFolder, validFile.GetName()), true);
            }
        }
        LoadAttachments();
    }

    private void BindInvalidResults()
    {
        if (RadUpload1.InvalidFiles.Count > 0)
        {
            //labelNoInvalidResults.Visible = false;
            //repeaterInvalidResults.Visible = true;
            //repeaterInvalidResults.DataSource = RadUpload1.InvalidFiles;
            //repeaterInvalidResults.DataBind();
        }
        else
        {
            //labelNoInvalidResults.Visible = true;
            //repeaterInvalidResults.Visible = false;
        }
    }

    /// <summary>
    /// For the purpose of this demo we delete the files collection in the targetfolder
    /// to prevent it from growing infinitely 
    /// </summary>
    private void DeleteFiles()
    {
        string targetFolder = Server.MapPath("~/Upload/Examples/ManipulatingTheUploadedFiles/MyFiles");

        DirectoryInfo targetDir = new DirectoryInfo(targetFolder);

        try
        {
            foreach (FileInfo file in targetDir.GetFiles())
            {
                if ((file.Attributes & FileAttributes.ReadOnly) == 0) file.Delete();
            }
        }
        catch (IOException)
        {
        }
    }

    private void SelectFirstGridRow()
    {
        GridDataItem firstDataItem = RadGrid1.Items.OfType<GridDataItem>().FirstOrDefault();
        //GridDataItem firstDataItem = RadGrid1.Items.OfType<GridDataItem>().FirstOrDefault();
        if (firstDataItem != null)
            firstDataItem.Selected = true;
    }

    /// <summary>
    /// Shows a <see cref="RadWindow"/> alert if an error occurs
    /// </summary>
    private void ShowErrorMessage()
    {
        RadAjaxManager1.ResponseScripts.Add(string.Format("window.radalert(\"Please enter valid data!\")"));
    }

    private bool SubmitChargebackDocument()
    {
        bool rtn = false;

        Dictionary<string, string> keyValue = new Dictionary<string, string>();

        keyValue.Add("DocKey_Str1", Session["AuthorizationNumber"].ToString());

        if (Helper.IsDocumentForRevision(keyValue))
        {
            WorkflowDataContext wdc = new WorkflowDataContext();

            var wfInstance = (from c in wdc.AS_WF_WorkflowInstances
                              where c.DocKey_Str1 == Session["AuthorizationNumber"].ToString()
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

                if (wfInstance.Comment.Contains("attach"))
                {
                    cmd.Parameters.Add(new SqlParameter("@pComment", "Revised Document - Attachment"));
                }
                else if (wfInstance.Comment == "Sent back to AR to enter Claimed Amount")
                {
                    cmd.Parameters.Add(new SqlParameter("@pComment", "Revised Document - AR"));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@pComment", "Revised Document"));
                }
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "Revision"));
                cmd.Parameters.Add(new SqlParameter("@pStatus", "Revised"));
                cmd.Parameters.Add(new SqlParameter("@pActionDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pActionTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pImportance", wfInstance.Importance));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", wfInstance.DueDate));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                ChargebackDataDataContext cbd = new ChargebackDataDataContext();

                var header = (from c in cbd.AS_GAIAM_Chargeback_Headers
                              where c.AuthorizationNumber == Session["AuthorizationNumber"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                header.Estimate = false;
                cbd.SubmitChanges();

                rtn = true;
            }
            catch (Exception ex)
            {
                log.Error("SubmitChargebackDocument Revision:", ex);
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

                cmd.Parameters.Add(new SqlParameter("@pWorkflowName", "GAIAMChargebackApprovalWorkflowV2"));
                cmd.Parameters.Add(new SqlParameter("@pCompanyID", Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"])));
                cmd.Parameters.Add(new SqlParameter("@pDocType", "GAIAM Chargeback Document"));
                cmd.Parameters.Add(new SqlParameter("@pDescription", "New Chargeback approval document"));
                cmd.Parameters.Add(new SqlParameter("@pCreatedBy", Session["LoggedInUser"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@pRequestType", "New"));
                cmd.Parameters.Add(new SqlParameter("@pDocKey_Str1", Session["AuthorizationNumber"]));
                cmd.Parameters.Add(new SqlParameter("@pCreationDate", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pCreationTime", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("@pAssignedTo", "System"));
                cmd.Parameters.Add(new SqlParameter("@pComment", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pImportance", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@pDueDate", DateTime.Now.AddDays(10)));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField1", txtAccountName.Text));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField2", txtEventName.Text));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField3", dtStart.SelectedDate == null ? dtStart.MinDate.ToShortDateString() : dtStart.SelectedDate.ToString()));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField4", dtEnd.SelectedDate == null ? dtEnd.MinDate.ToShortDateString() : dtEnd.SelectedDate.ToString()));
                cmd.Parameters.Add(new SqlParameter("@pUserDefinedField5", txtTotalAmount.Text));

                connection.Open();

                cmd.ExecuteNonQuery();

                // Update Document Status
                ChargebackDataDataContext cbd = new ChargebackDataDataContext();

                var header = (from c in cbd.AS_GAIAM_Chargeback_Headers
                              where c.AuthorizationNumber == Session["AuthorizationNumber"].ToString()
                              select c).First();

                header.WorkflowState = "Submitted";
                cbd.SubmitChanges();
                rtn = true;
            }
            catch (Exception ex)
            {
                log.Error("SubmitChargebackDocument", ex);
            }
            finally
            {
                connection.Close();
            }
            return rtn;
        }        

        //Close the form
        //RadWindowManager1.RadAlert("Document has been submitted.", 330, 100, "Notification", "alertCallBackFn");
        //ScriptManager.RegisterStartupScript(this, typeof(string), "CLOSE_WINDOW", "self.close();", true);        
    }

    private void SaveChargebackDocument()
    {
        ChargebackDataDataContext cbd = new ChargebackDataDataContext();

        if (Convert.ToBoolean(Session["IsNewRecord"]))
        {
            AS_GAIAM_Chargeback_Header header = new AS_GAIAM_Chargeback_Header
            {
                AuthorizationNumber = Session["AuthorizationNumber"].ToString(),
                AccountName = txtAccountName.Text,
                GAIAMAccountNumber = txtAccountNumber.Text,
                EventName = txtEventName.Text,
                AddressStreet = txtStreet.Text,
                AddressCity = txtCity.Text,

                AddressState = txtState.Text,
                AddressZip = txtZip.Text,
                AddressCountry = txtCountry.Text,
                Phone = txtPhone.Text,
                SalesPerson = txtSalesPerson.Text,
                IndirectAccountName = txtIndirectAccountName.Text,
                VendorID = txtVendorID.Text,
                VendorName = txtVendorName.Text,
                DateStartEvent = dtStart.SelectedDate,
                DateEndEvent = dtEnd.SelectedDate,
                ExpenseType = cboExpenseType.SelectedItem.Text,
                ExpenseSubType = cboExpenseSubType.SelectedItem.Text,
                IsExpenseStudioApproved = chkIsExpenseStudioApproved.Checked,
                IsExpensePaidByInvoice = chkExpensePaidByInvoice.Checked,
                Estimate = chkEstimate.Checked,
                TotalAmount = Convert.ToDecimal(txtTotalAmount.Value),
                SpecialInstructions = txtSpecialInstructions.Text,
                WorkflowState = "Saved",
                CreatedBy = Session["LoggedInUser"].ToString(),
                AttachedFilePath = Session["AttachmentFilePath"].ToString(),
                ClaimedAmount =Convert.ToDecimal(txtClaimedAmount.Value)
            };

            cbd.AS_GAIAM_Chargeback_Headers.InsertOnSubmit(header);
        }
        else
        {
            var header = (from c in cbd.AS_GAIAM_Chargeback_Headers
                          where c.AuthorizationNumber == Session["AuthorizationNumber"].ToString()
                          select c).First();

            header.AccountName = txtAccountName.Text;
            header.GAIAMAccountNumber = txtAccountNumber.Text;
            header.EventName = txtEventName.Text;
            header.AddressStreet = txtStreet.Text;
            header.AddressCity = txtCity.Text;
            header.AddressState = txtState.Text;
            header.AddressZip = txtZip.Text;
            header.AddressCountry = txtCountry.Text;
            header.Phone = txtPhone.Text;
            header.SalesPerson = txtSalesPerson.Text;
            header.IndirectAccountName = txtIndirectAccountName.Text;
            header.VendorID = txtVendorID.Text;
            header.VendorName = txtVendorName.Text;
            header.DateStartEvent = dtStart.SelectedDate;
            header.DateEndEvent = dtEnd.SelectedDate;
            header.ExpenseType = cboExpenseType.SelectedItem.Text;
            header.ExpenseSubType = cboExpenseSubType.SelectedItem.Text;
            header.IsExpenseStudioApproved = chkIsExpenseStudioApproved.Checked;
            header.IsExpensePaidByInvoice = chkExpensePaidByInvoice.Checked;
            header.Estimate = chkEstimate.Checked;
            header.TotalAmount = Convert.ToDecimal(txtTotalAmount.Value);
            header.ClaimedAmount = Convert.ToDecimal(txtClaimedAmount.Value);
            header.SpecialInstructions = txtSpecialInstructions.Text;
            header.WorkflowState = "Saved";
        }
        try
        {
            cbd.SubmitChanges();
        }
        catch (Exception ex)
        {

        }
    }

    private void DeleteChargebackDocument()
    {
        ChargebackDataDataContext cbd = new ChargebackDataDataContext();

        try
        {
            //Delete Header
            var chargebackHeaders = from s in cbd.AS_GAIAM_Chargeback_Headers
                                    where s.AuthorizationNumber == txtAuthorizationNumber.Text
                                    select s;

            if (chargebackHeaders.Count() > 0)
            {
                foreach (var header in chargebackHeaders)
                {
                    cbd.AS_GAIAM_Chargeback_Headers.DeleteOnSubmit(header);
                }

                //Delete Details
                var chargebackDetails = from s in cbd.AS_GAIAM_Chargeback_Details
                                        where s.AuthorizationNumber == txtAuthorizationNumber.Text
                                        select s;

                foreach (var detail in chargebackDetails)
                {
                    cbd.AS_GAIAM_Chargeback_Details.DeleteOnSubmit(detail);
                }

                cbd.SubmitChanges();

                //Delete WFInstance
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);

                string sql = "DELETE FROM [AS_WF_WorkflowInstance] WHERE DocKey_Str1 = @AuthorizationNumber AND DocType = 'GAIAM Chargeback Document'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@AuthorizationNumber", txtAuthorizationNumber.Text);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();

                //Create Reverse GL
                Helper.GAIAM_ReverseGLEntriesForDeleted(txtAuthorizationNumber.Text);
            }

        }
        catch (Exception ex)
        {

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

    private void DownloadExcelData(bool pIsTemplateOnly)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

        try
        {
            // Opening the Excel template...
            FileStream fs =
                new FileStream(Server.MapPath(@"GAIAMTemplates\TradeSpendExpenseDocument.xlsx"), FileMode.Open, FileAccess.Read);

            // Getting the complete workbook...
            IWorkbook templateWorkbook = new XSSFWorkbook(fs);

            if (!pIsTemplateOnly)
            {
                // Getting the worksheet by its name...
                ISheet sheet = templateWorkbook.GetSheet("Sheet1");

                // Getting the row... 0 is the first row.
                // Start at the 5th row
                int ctr = 4;

                ChargebackDataDataContext cbd = new ChargebackDataDataContext();

                //Delete Details
                var chargebackDetails = from s in cbd.AS_GAIAM_Chargeback_Details
                                        where s.AuthorizationNumber == txtAuthorizationNumber.Text
                                        select s;

                foreach (var item in chargebackDetails)
                {
                    log.Debug("Starting DownloadExcelData Start Item: " + item.GAIAMItemNumber + " - " + DateTime.Now.ToLongTimeString());
                    //foreach (GridDataItem item in RadGrid1.Items)

                    XSSFFormulaEvaluator evaluator = new XSSFFormulaEvaluator(templateWorkbook);

                    IRow dataRow = sheet.GetRow(ctr);

                    if (dataRow == null)
                        dataRow = sheet.CreateRow(ctr);

                    // Set the Data
                    dataRow.CreateCell(0).SetCellValue(txtAuthorizationNumber.Text);
                    dataRow.CreateCell(1).SetCellValue(item.GAIAMItemNumber == null || item.GAIAMItemNumber.ToString() == "&nbsp;" ? string.Empty : item.GAIAMItemNumber.ToString());
                    dataRow.CreateCell(2).SetCellValue(item.UPCCode == null || item.UPCCode.ToString() == "&nbsp;" ? string.Empty : item.UPCCode.ToString());
                    dataRow.CreateCell(3).SetCellValue(item.TitlePromotion == null || item.TitlePromotion.ToString() == "&nbsp;" ? string.Empty : item.TitlePromotion.ToString());
                    dataRow.CreateCell(4).SetCellValue(item.Studio == null || item.Studio.ToString() == "&nbsp;" ? string.Empty : item.Studio.ToString());
                    dataRow.CreateCell(5).SetCellValue(Convert.ToDouble(item.AmountPerUnit.ToString().Replace("$", string.Empty)));
                    dataRow.CreateCell(6).SetCellValue(Convert.ToDouble(item.Quantity.ToString().Replace("$", string.Empty)));
                    //dataRow.CreateCell(7).SetCellFormula("F" + (ctr + 1).ToString() + "*G" + (ctr + 1).ToString());
                    dataRow.CreateCell(7).SetCellValue(Convert.ToDouble(item.TotalAmount.ToString().Replace("$", string.Empty)));
                    dataRow.CreateCell(8).SetCellValue(item.InvalidText == null || item.InvalidText.ToString() == "&nbsp;" ? string.Empty : item.InvalidText.ToString());                    

                    //dataRow.CreateCell(1).SetCellValue(item["GAIAMItemNumber"].Text.ToString() == "&nbsp;" ? string.Empty : item["GAIAMItemNumber"].Text.ToString());
                    //dataRow.CreateCell(2).SetCellValue(item["UPCCode"].Text.ToString() == "&nbsp;" ? string.Empty : item["UPCCode"].Text.ToString());
                    //dataRow.CreateCell(3).SetCellValue(item["TitlePromotion"].Text.ToString() == "&nbsp;" ? string.Empty : item["TitlePromotion"].Text.ToString());
                    //dataRow.CreateCell(4).SetCellValue(item["Studio"].Text.ToString() == "&nbsp;" ? string.Empty : item["Studio"].Text.ToString());
                    //dataRow.CreateCell(5).SetCellValue(Convert.ToDouble(item["AmountPerUnit"].Text.ToString().Replace("$", string.Empty)));
                    //dataRow.CreateCell(6).SetCellValue(Convert.ToDouble(item["Quantity"].Text.ToString().Replace("$", string.Empty)));
                    //dataRow.CreateCell(7).SetCellFormula("F" + (ctr + 1).ToString() + "*G" + (ctr + 1).ToString());
                    //dataRow.CreateCell(8).SetCellValue(item["InvalidSKU"].Text.ToString() == "&nbsp;" ? string.Empty : item["InvalidSKU"].Text.ToString());

                    //if (item["InvalidSKU"].Text.ToString() == "Invalid" || item["InvalidSKU"].Text.ToString() == "Zero")
                    //{
                    //    ICellStyle style1 = templateWorkbook.CreateCellStyle();
                    //    style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index2;
                    //    style1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                    //    for (int i = 0; i < 9; i++)
                    //    {
                    //        dataRow.Cells[i].CellStyle = style1;
                    //    }

                    //}

                    if (!string.IsNullOrEmpty(item.InvalidText))
                    {
                        ICellStyle style1 = templateWorkbook.CreateCellStyle();
                        style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index2;
                        style1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        //for (int i = 0; i < 9; i++)
                        //{
                        dataRow.Cells[0].CellStyle = style1;
                        //}



                    }


                    //evaluator.EvaluateAll();

                    ctr++;

                    log.Debug("Starting DownloadExcelData End Item: " + item.GAIAMItemNumber + " - " + DateTime.Now.ToLongTimeString());
                }


                // Forcing formula recalculation...
                sheet.ForceFormulaRecalculation = true;
            }

            MemoryStream ms = new MemoryStream();

            // Writing the workbook content to the FileStream...
            templateWorkbook.Write(ms);

            //TempData["Message"] = "Excel report created successfully!";

            // Sending the server processed data back to the user computer...
            //return File(ms.ToArray(), "application/vnd.ms-excel", "NPOINewFile.xls");

            string filename = "TradeSpendExpenseDocument.xlsx";
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
        catch (Exception ex)
        {
            //TempData["Message"] = "Oops! Something went wrong.";

            //return RedirectToAction("NPOI");
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }

    private void PopulateExpenseTypes()
    {

        cboExpenseType.Items.Clear();

        // Update Document Status
        ChargebackDataDataContext cbd = new ChargebackDataDataContext();

        var header = (from c in cbd.AS_GAIAM_Chargeback_ExpenseTypes
                      select c.ExpenseType).Distinct();

        foreach (string expenseType in header)
        {
            cboExpenseType.Items.Add(new RadComboBoxItem(expenseType));
        }

        //Initialize the SubTypes for first loading
        PopulateExpenseSubTypes(cboExpenseType.SelectedItem.Text);
    }

    private void PopulateExpenseSubTypes(string pExpenseType)
    {
        cboExpenseSubType.Items.Clear();

        // Update Document Status
        ChargebackDataDataContext cbd = new ChargebackDataDataContext();

        var header = (from c in cbd.AS_GAIAM_Chargeback_ExpenseTypes
                      where c.ExpenseType == pExpenseType
                      select c);

        foreach (AS_GAIAM_Chargeback_ExpenseType expenseType in header)
        {
            cboExpenseSubType.Items.Add(new RadComboBoxItem(expenseType.ExpenseSubType));
        }
    }

    #endregion


}

