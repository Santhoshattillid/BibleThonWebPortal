using System;
using System.Linq;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class AlerePatientMailingDocumentOrderIDLookup : System.Web.UI.Page
{
    DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            dt = GetDataTable("SELECT TOP 100 ORDERID, PATIENTID FROM AS_Alere_PMWF_Orders ORDER BY ORDERID");
            this.RadGrid1.MasterTableView.Columns.Clear();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                AlerePatientMailingDocumentOrderIDLookupCustomFilterCol gridColumn = new AlerePatientMailingDocumentOrderIDLookupCustomFilterCol();                
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn);
                gridColumn.DataField = dataColumn.ColumnName;
                gridColumn.HeaderText = dataColumn.ColumnName;
                gridColumn.HeaderStyle.Width = Unit.Pixel(200); 
            }
        }
    }

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        string sqlString = "SELECT TOP 500 ORDERID, PATIENTID FROM AS_Alere_PMWF_Orders WHERE ORDERID IS NOT NULL ";

        if (RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem).Count() > 0)
        {
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1PatientID"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1PatientID")));
                sqlString += " AND PATIENTID = '" + cb.Text + "' ";
            }
            if (((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1ORDERID"))).Text != string.Empty)
            {
                RadComboBox cb = ((Telerik.Web.UI.RadComboBox)(RadGrid1.MasterTableView.GetItems(GridItemType.FilteringItem)[0].FindControl("RadComboBox1ORDERID")));
                sqlString += " AND ORDERID = '" + cb.Text + "' ";
            }
        }
        sqlString += "  ORDER BY ORDERID";

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

        SqlCommand cmd = new SqlCommand(query, conn);
        DataTable myDataTable = new DataTable();

        conn.Open();
        try
        {
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