using System;
using System.Linq;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class AlerePatientIDLookup : System.Web.UI.Page
{
    DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            dt = GetDataTable("SELECT TOP 100 PATIENTID, LASTNAME, FIRSTNAME, DOB FROM AS_WF_VW_AlerePatientLookup ORDER BY PATIENTID");
            this.RadGrid1.MasterTableView.Columns.Clear();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                AlerePatientLookupCustomFilterCol gridColumn = new AlerePatientLookupCustomFilterCol();                
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn);
                gridColumn.DataField = dataColumn.ColumnName;
                gridColumn.HeaderText = dataColumn.ColumnName;
                gridColumn.HeaderStyle.Width = Unit.Pixel(200); 
            }
        }
    }

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        string sqlString = "SELECT TOP 500 PATIENTID, LASTNAME, FIRSTNAME, DOB FROM AS_WF_VW_AlerePatientLookup WHERE PATIENTID IS NOT NULL ";

        if (RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem).Count() > 0)
        {
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1PatientID"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1PatientID")));
                sqlString += " AND PATIENTID = '" + cb.Text + "' ";
            }
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1LASTNAME"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1LASTNAME")));
                sqlString += " AND LASTNAME = '" + cb.Text + "' ";
            }
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1FIRSTNAME"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1FIRSTNAME")));
                sqlString += " AND FIRSTNAME = '" + cb.Text + "' ";
            }
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1DOB"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1DOB")));
                sqlString += " AND DOB = '" + cb.Text + "' ";
            }

        }
            sqlString += "  ORDER BY PATIENTID";

            dt = GetDataTable(sqlString);
        this.RadGrid1.DataSource = dt;
    }

    protected void RadGrid1_ColumnCreating(object sender, GridColumnCreatingEventArgs e)
    {
        if ((e.ColumnType == typeof(AlerePatientLookupCustomFilterCol).Name))
        {
            e.Column = new AlerePatientLookupCustomFilterCol();
        }
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if ((e.CommandName == "Filter"))
        {
            foreach (GridColumn column in e.Item.OwnerTableView.Columns)
            {
                column.CurrentFilterValue = string.Empty;
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            }
        }
    }

    protected void clrFilters_Click(object sender, EventArgs e)
    {
        foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid1.MasterTableView.FilterExpression = string.Empty;
        RadGrid1.MasterTableView.Rebind();
    }

    public static DataTable GetDataTable(string query)
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlDataAdapter adapter = new SqlDataAdapter();

        //string sqlQuery = @"[S-LVM-DB01\CV_TRAIN].[AHM].[dbo].[usp_AHM_Alba_Patient_List]";

        //adapter.SelectCommand = new SqlCommand(query, conn);
        SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.CommandType = CommandType.StoredProcedure;
        //adapter.SelectCommand = cmd;        
        DataTable myDataTable = new DataTable();

        conn.Open();
        try
        {
            //adapter.Fill(myDataTable);
            SqlDataReader reader = cmd.ExecuteReader();            

            myDataTable.Load(reader);
        }
        finally
        {
            conn.Close();
        }
        return myDataTable;
    }
}