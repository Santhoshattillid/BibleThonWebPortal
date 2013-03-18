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

public partial class Gaiam_Chargeback_ExpenseTypes_PickList : System.Web.UI.Page
{
    DataTable dt;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        
    }

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

    /// <summary>
    /// Shows a <see cref="RadWindow"/> alert if an error occurs
    /// </summary>
    private void ShowErrorMessage()
    {
        //RadAjaxManager1.ResponseScripts.Add(string.Format("window.radalert(\"Please enter valid data!\")"));
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack && RadGrid1.MasterTableView.Items.Count > 0)
        {
            RadGrid1.MasterTableView.Items[1].Expanded = true;
        }
    }
}