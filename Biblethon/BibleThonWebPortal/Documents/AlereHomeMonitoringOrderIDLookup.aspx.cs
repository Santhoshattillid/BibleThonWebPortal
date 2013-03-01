using System;
using System.Linq;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class AlereHomeMonitoringOrderIDLookup : System.Web.UI.Page
{
    DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            dt = GetDataTable("SELECT [OrderID],[WorkflowState],[PatientID],[FirstName],[LastName],[DOB],[TotalAmount] FROM [TWO].[dbo].[AS_WF_VW_AlereOrderLookup]");

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
        dt = GetDataTable("SELECT [OrderID],[WorkflowState],[PatientID],[FirstName],[LastName],[DOB],[TotalAmount] FROM [TWO].[dbo].[AS_WF_VW_AlereOrderLookup]");
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
}