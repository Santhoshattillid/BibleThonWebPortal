using System;
using System.Linq;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;
using System.Web.UI;

public partial class GaiamChargeBackImportData : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

    private static DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["AuthorizationNumber"]))
            {
                lblAuthorizationNumber.Text = Request.QueryString["AuthorizationNumber"];
            }

            string pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString() + "/ImportedItems/";
            if (!Directory.Exists(Server.MapPath(pathToCreate)))
            {
                Directory.CreateDirectory(Server.MapPath(pathToCreate));
            }
        }
    }

    #region Methods

    private void ProcessExcelFile()
    {
        string connectionString = "";

        //Check whether file extension is xls or xslx
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);

        try
        {
            
            if (!string.IsNullOrEmpty(lblFilePathName.Text))
            {
                if (lblFilePathExtension.Text == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lblFilePathName.Text + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=No;" + Convert.ToChar(34).ToString();
                }
                else if (lblFilePathExtension.Text == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + lblFilePathName.Text + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=2\"";
                }

                //Create OleDB Connection and OleDb Command
                OleDbConnection con = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                con.Open();
                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "] WHERE F2 <> '' OR F3 <> ''";
                dAdapter.SelectCommand = cmd;
                log.Debug("Import Before Fill : " + DateTime.Now.ToShortTimeString());
                dAdapter.Fill(dtExcelRecords);
                log.Debug("Import After Fill : " + DateTime.Now.ToShortTimeString());
                con.Close();
                
                // Remove unnecessary rows
                for (int i = 0; i < 1; i++)
                {
                    DataRow dr = dtExcelRecords.Rows[0];
                    dtExcelRecords.Rows.Remove(dr);
                }

                dtExcelRecords.Columns[1].ColumnName = "Item Number";
                dtExcelRecords.Columns[2].ColumnName = "UPC";
                dtExcelRecords.Columns[3].ColumnName = "Title";
                dtExcelRecords.Columns[4].ColumnName = "Studio";
                dtExcelRecords.Columns[5].ColumnName = "Quantity";
                dtExcelRecords.Columns[6].ColumnName = "Amount Per Unit";                

                if (dtExcelRecords.Columns.Count == 7)
                    dtExcelRecords.Columns.Add("Total");

                dtExcelRecords.Columns[7].ColumnName = "Total";

                dtExcelRecords.Columns.Add("AuthorizationNumber");
                dtExcelRecords.Columns.Add("Processed");

                foreach (DataRow dr in dtExcelRecords.Rows)
                {
                    dr["AuthorizationNumber"] = lblAuthorizationNumber.Text;
                    dr["Processed"] = false;
                }

                // Delete existing records first
                Helper.GAIAM_DeleteDetailTemp(lblAuthorizationNumber.Text, conn);

                //foreach (DataRow dr in dtExcelRecords.Rows)
                //{
                //    log.Debug("Import DataRow : " + dr[2].ToString() + " - " + DateTime.Now.ToShortTimeString());
                //    string UPCOrig = string.Empty;                    
                   
                //    string itemNumber = DBNull.Value.Equals(dr[1]) ? string.Empty : dr[1].ToString();                        
                //    string upccode = DBNull.Value.Equals(dr[2]) ? string.Empty : dr[2].ToString();                        
                //    int quantity = DBNull.Value.Equals(dr[5]) ? 0 : Convert.ToInt32(dr[5].ToString());                        
                //    decimal unitAmount = DBNull.Value.Equals(dr[6]) ? 0 : Convert.ToDecimal(dr[6].ToString());
                //    decimal total = quantity * unitAmount;

                //    if (!Helper.GAIAM_InsertDetailTemp(lblAuthorizationNumber.Text, upccode, itemNumber, quantity, unitAmount, total, conn))
                //        throw new Exception("Error inserting data in detail temptable");

                //    log.Debug("Import DataRow : " + dr[2].ToString() + " - " + DateTime.Now.ToShortTimeString());

                //}                

                #region OLD CODE
                ////dt.DefaultView.Sort = "[Item Number] ASC";                

                //int ctr = 0;
                //string itemNmbrCtr = string.Empty;

                //// sorting the datatable basedon salary in descending order 
                //DataRow[] rows = dtExcelRecords.Select(string.Empty, "[Item Number] ASC");

                //dt = new DataTable();
                //dt.Columns.Add("Item Number");
                //dt.Columns.Add("UPC");
                //dt.Columns.Add("Title");
                //dt.Columns.Add("Studio");
                //dt.Columns.Add("Quantity");
                //dt.Columns.Add("Amount Per Unit");
                //dt.Columns.Add("Total");       

                //foreach (DataRow dr in rows)
                //{
                //    if (itemNmbrCtr == string.Empty)
                //    {
                //        itemNmbrCtr = dr["Item Number"].ToString();

                //        DataRow dtnewRow = dt.NewRow();
                //        dtnewRow["Item Number"] = dr["Item Number"];
                //        dtnewRow["UPC"] = dr["UPC"];
                //        dtnewRow["Title"] = dr["Title"];
                //        dtnewRow["Studio"] = dr["Studio"];
                //        dtnewRow["Quantity"] = dr["Quantity"];
                //        dtnewRow["Amount Per Unit"] = dr["Amount Per Unit"];
                //        dtnewRow["Total"] = dr["Total"];

                //        dt.Rows.Add(dtnewRow);                        
                //    }
                //    else
                //    {
                //        if (itemNmbrCtr == dr["Item Number"].ToString())
                //        {
                //            dt.Rows[ctr]["Quantity"] = Convert.ToInt32(dt.Rows[ctr]["Quantity"].ToString()) + Convert.ToInt32(dr["Quantity"].ToString());
                //            dt.Rows[ctr]["Total"] = Convert.ToDecimal(dt.Rows[ctr]["Total"].ToString()) + Convert.ToDecimal(dr["Total"].ToString());
                //            //dt.Rows.Remove(dr);
                //        }
                //        else
                //        {
                //            itemNmbrCtr = dr["Item Number"].ToString();

                //            DataRow dtnewRow = dt.NewRow();
                //            dtnewRow["Item Number"] = dr["Item Number"];
                //            dtnewRow["UPC"] = dr["UPC"];
                //            dtnewRow["Title"] = dr["Title"];
                //            dtnewRow["Studio"] = dr["Studio"];
                //            dtnewRow["Quantity"] = dr["Quantity"];
                //            dtnewRow["Amount Per Unit"] = dr["Amount Per Unit"];
                //            dtnewRow["Total"] = dr["Total"];

                //            dt.Rows.Add(dtnewRow);                      

                //            ctr += 1;
                //        }
                //    }
#endregion                                                               

                //string connectionStrin1g = @"Server=localhost;Database=Northwind;Trusted_Connection=true";                                
                // open the destination data
                log.Debug("Starting DB Processes : " + DateTime.Now.ToLongTimeString());
                using (SqlConnection destinationConnection = conn)
                {
                    // open the connection
                    if (destinationConnection.State == ConnectionState.Closed)
                    {
                        destinationConnection.Open();   
                    }                    

                    using (SqlBulkCopy bulkCopy =
                                new SqlBulkCopy(destinationConnection.ConnectionString))
                    {
                        // column mappings
                        bulkCopy.ColumnMappings.Add("AuthorizationNumber", "AuthorizationNumber");
                        bulkCopy.ColumnMappings.Add("Item Number", "GAIAMItemNumber");
                        bulkCopy.ColumnMappings.Add("UPC", "UPCCode");
                        bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                        bulkCopy.ColumnMappings.Add("Amount Per Unit", "AmountPerUnit");
                        bulkCopy.ColumnMappings.Add("Total", "TotalAmount");
                        bulkCopy.ColumnMappings.Add("Processed", "Processed");
                        bulkCopy.DestinationTableName = "AS_GAIAM_Chargeback_Detail_Temp";
                        bulkCopy.WriteToServer(dtExcelRecords);
                    }

                    Helper.GAIAM_ProcessDetailTemp(lblAuthorizationNumber.Text, destinationConnection);
                    log.Debug("DB Processes Ended at: " + DateTime.Now.ToLongTimeString());
                }                

            }
        }
        catch (Exception ex)
        {
            log.Error(ex);

            string closescript = "<script>ErrorMsg('Error was encountered uploading the file. Please check the log file for details.')</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", closescript, false);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
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

    #endregion

    #region Events

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {        

        //RadGrid1.DataSource = dt;
        //RadGrid1.DataBind();
        dt = GetDataTable("SELECT * FROM [AS_GAIAM_Chargeback_Detail_Buffer] WHERE AuthorizationNumber = '" + lblAuthorizationNumber.Text + "'");
        this.RadGrid1.DataSource = dt;
    }

    protected void RadButton1_Click(object sender, EventArgs e)
    {
        if (RadUpload1.UploadedFiles.Count > 0)
        {
            string fileName = Path.GetFileName(RadUpload1.UploadedFiles[0].FileName);
            string fileExtension = Path.GetExtension(RadUpload1.UploadedFiles[0].FileName);
            //string fileName = Path.GetFileName(FileUpload1.FileName);
            //string fileExtension = Path.GetExtension(FileUpload1.FileName);
            string fileLocation = Server.MapPath("~/App_Data/" + fileName);
            //FileUpload1.SaveAs(fileLocation);

            string pathToCreate = "~/Attachments/" + Session["AuthorizationNumber"].ToString() + "/ImportedItems/";

            string targetFolder = Server.MapPath(pathToCreate);

            string finalPath = Path.Combine(targetFolder, RadUpload1.UploadedFiles[0].GetName());
            RadUpload1.UploadedFiles[0].SaveAs(finalPath, true);

            lblFilePathName.Text = finalPath;
            lblFilePathExtension.Text = fileExtension;
            ProcessExcelFile();
        }

        //RadGrid1.DataSource = dt;
        //RadGrid1.DataBind();
        RadGrid1.Rebind();
    }   

    protected void pnl1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {              
        // Delete records first
        ChargebackDataDataContext cb = new ChargebackDataDataContext();

        IEnumerable<AS_GAIAM_Chargeback_Detail> cbDetails = (from c in cb.AS_GAIAM_Chargeback_Details
                                                             where c.AuthorizationNumber == lblAuthorizationNumber.Text
                                                             select c).ToList();
        try
        {
            cb.AS_GAIAM_Chargeback_Details.DeleteAllOnSubmit(cbDetails);
            cb.SubmitChanges();            

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
            
            string rtn = string.Empty;
            string sql = "INSERT INTO [AS_GAIAM_Chargeback_Detail] ([AuthorizationNumber],[GAIAMItemNumber],[TitlePromotion],[UPCCode],[Studio],[AmountPerUnit],[Quantity],[TotalAmount],[AuthorizedAmount],[InvalidText]) SELECT [AuthorizationNumber],[GAIAMItemNumber],[TitlePromotion],[UPCCode],[Studio],[AmountPerUnit],[Quantity],[TotalAmount],[AuthorizedAmount],[InvalidText] FROM [AS_GAIAM_Chargeback_Detail_Buffer] WHERE AuthorizationNumber = @pAuthorizationNumber; ";
                
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pAuthorizationNumber", lblAuthorizationNumber.Text);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();
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

            string closescript = "<script>returnToParent(100)</" + "script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Records Imported", closescript, false);
        }

        catch (Exception ex)
        {
            RadWindowManager1.RadAlert(ex.Message, 330, 100, "Warning", "");
            log.Error(ex);
        }        
    }   

    protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pagerItem = (GridPagerItem)e.Item;
            RadComboBox PageSizeComboBox = (RadComboBox)pagerItem.FindControl("PageSizeComboBox");
            PageSizeComboBox.Visible = false;
            Label changePageSizelbl = (Label)pagerItem.FindControl("ChangePageSizeLabel");
            changePageSizelbl.Visible = false;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid1.MasterTableView.Items.Count > 0)
            {
                bool areAllRecordsValid = true;

                foreach (GridDataItem item in RadGrid1.Items)
                {
                    if (item["ItemNumber"].Text == string.Empty || item["ItemNumber"].Text == "&nbsp;")
                        areAllRecordsValid = false;
                }
                if (areAllRecordsValid)
                {
                    RadWindowManager1.RadConfirm("Warning: Uploading the items will delete the current items on the grid. Continue?", "confirmCallBackFn", 330, 100, null, "Confirm Processing", null);
                }
                else
                {
                    RadWindowManager1.RadAlert("There are records with missing Item Numbers or UPC Codes with no Item Numbers mapped to them.", 330, 100, "Warning", "");
                }
            }
            else
            {
                RadWindowManager1.RadAlert("There are no items to be imported", 330, 100, "Warning", "confirmCallBackFn");
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion          
}