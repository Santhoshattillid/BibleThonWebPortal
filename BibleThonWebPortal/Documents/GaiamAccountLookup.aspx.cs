using System;
using System.Linq;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class GaiamAccountLookup : System.Web.UI.Page
{
    DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            dt = GetDataTable("SELECT TOP 100 CUSTNMBR, CUSTNAME FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ORDER BY CUSTNAME");
            this.RadGrid1.MasterTableView.Columns.Clear();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                GaiamAccountLookupCustomFilterCol gridColumn = new GaiamAccountLookupCustomFilterCol();                
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn);
                gridColumn.DataField = dataColumn.ColumnName;
                if(dataColumn.ColumnName == "CUSTNMBR")
                    gridColumn.HeaderText = "Account Number";
                else
                    gridColumn.HeaderText = "Account Name";
                gridColumn.HeaderStyle.Width = Unit.Pixel(200); 
            }
        }
    }

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        string sqlString = "SELECT TOP 500 CUSTNMBR, CUSTNAME FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ";

        if (RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem).Count() > 0)
        {
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1CUSTNMBR"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1CUSTNMBR")));
                sqlString += " AND CUSTNMBR = '" + cb.Text + "' ";
            }
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1CUSTNAME"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1CUSTNAME")));
                sqlString += " AND CUSTNAME = '" + cb.Text + "' ";
            }
        }
        sqlString += "  ORDER BY CUSTNAME";

            dt = GetDataTable(sqlString);
        this.RadGrid1.DataSource = dt;
    }

    protected void RadGrid1_ColumnCreating(object sender, GridColumnCreatingEventArgs e)
    {
        if ((e.ColumnType == typeof(GaiamAccountLookupCustomFilterCol).Name))
        {
            e.Column = new GaiamAccountLookupCustomFilterCol();
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