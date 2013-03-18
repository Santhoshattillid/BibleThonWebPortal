using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

/// <summary>
/// Summary description for MyCustomFilteringColumnCS
/// </summary>
public class GaiamVendorLookupCustomFilterCol : GridBoundColumn
{    
    public static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString; }
    }

    //RadGrid will call this method when it initializes the controls inside the filtering item cells
    protected override void SetupFilterControls(TableCell cell)
    {
        base.SetupFilterControls(cell);
        cell.Controls.RemoveAt(0);
        RadComboBox combo = new RadComboBox();
        combo.ID = ("RadComboBox1" + this.UniqueName);
        combo.ShowToggleImage = false;
        combo.Skin = "Office2007";
        combo.EnableLoadOnDemand = true;
        combo.AutoPostBack = true;
        combo.MarkFirstMatch = true;
        combo.Height = Unit.Pixel(100);
        combo.ItemsRequested += this.list_ItemsRequested;
        combo.SelectedIndexChanged += this.list_SelectedIndexChanged;
        cell.Controls.AddAt(0, combo);
        cell.Controls.RemoveAt(1);
    }

    //RadGrid will cal this method when the value should be set to the filtering input control(s)
    protected override void SetCurrentFilterValueToControl(TableCell cell)
    {
        base.SetCurrentFilterValueToControl(cell);
        RadComboBox combo = (RadComboBox)cell.Controls[0];
        if ((this.CurrentFilterValue != string.Empty))
        {
            combo.Text = this.CurrentFilterValue;
        }
    }

    //RadGrid will cal this method when the filtering value should be extracted from the filtering input control(s)
    protected override string GetCurrentFilterValueFromControl(TableCell cell)
    {
        RadComboBox combo = (RadComboBox)cell.Controls[0];
        return combo.Text;
    }

    private void list_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        
        ((RadComboBox)o).DataTextField = this.DataField;
        ((RadComboBox)o).DataValueField = this.DataField;
        ((RadComboBox)o).DataSource = GetDataTable("SELECT DISTINCT TOP 1000  " + this.UniqueName + " FROM PM00200 WHERE " + this.UniqueName + " LIKE '" + e.Text + "%'");
        ((RadComboBox)o).DataBind();
        
    }

    private void list_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (GridFilteringItem)((RadComboBox)o).NamingContainer;
        if ((this.UniqueName == "Index"))
        {
            //this is filtering for integer column type 
            filterItem.FireCommandEvent("Filter", new Pair("EqualTo", this.UniqueName));
        }
        //filtering for string column type
        filterItem.FireCommandEvent("Filter", new Pair("Contains", this.UniqueName));
    }

    public static DataTable GetDataTable(string query)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
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