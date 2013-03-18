using System;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Alere_Home_Monitoring_Script_Template : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["UserRole"].ToString().ToUpper() != "WORKFLOW ADMIN")
            //{
            //    string scriptstring = "radalert('Welcome to Rad<strong>Window</strong>!', 330, 210);";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "radalert", scriptstring, true);  

            //    ShowMessage("You do not have the necessary access rights to access this window");
            //}

            LoadContent("BasicInformation");
        }
    }

    private void LoadContent(string pLinkIdentifier)
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT LinkHTML FROM AS_Alere_DynamicScript WHERE LinkIdentifier = @LinkIdentifier";

        cmd.Parameters.AddWithValue("@LinkIdentifier", pLinkIdentifier);

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            object tmp = cmd.ExecuteScalar();

            ScriptEditor.Content = tmp == null ? string.Empty: tmp.ToString();
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string ConnString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "UPDATE AS_Alere_DynamicScript SET LinkHTML = @LinkHTML WHERE LinkIdentifier = @LinkIdentifier";

        cmd.Parameters.AddWithValue("@LinkHTML", ScriptEditor.Content);
        cmd.Parameters.AddWithValue("@LinkIdentifier", CboLinkIdentifier.SelectedValue);

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            cmd.ExecuteNonQuery();

            ShowMessage("Record Saved"); 
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        

    }

    private void ShowMessage(string pMessage)
    {
        RadWindowManager1.RadAlert(pMessage, 330, 100, "Alert", "", "");
    }
    protected void CboLinkIdentifier_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadContent(CboLinkIdentifier.SelectedValue);
    }
}