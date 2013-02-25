using System;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

public partial class Login : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);   

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //Retrieve the decrypted password first
        string decryptedPassword = clsCrypto.EncryptString(txtPassword.Text);
        bool shouldUserChangePassword = false;

        var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        try
        {
            var cmd = new SqlCommand("SELECT UserID, FirstName, CompanyPosition, ERPUserID, UserRole, IsDisabled, ShouldChangePassword FROM AS_WF_Users WHERE UserID = @UserID AND UserPassword = @UserPassword", connection);
            cmd.Parameters.AddWithValue("@UserID", txtUserName.Text);
            cmd.Parameters.AddWithValue("@UserPassword", decryptedPassword);
            
            connection.Open();

            SqlDataReader reader =  cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (!DBNull.Value.Equals(reader["IsDisabled"]) && Convert.ToBoolean(reader["IsDisabled"]) == true)
                    {
                        ShowDisabledMessage();
                        return;
                    }

                    Session["LoggedInUser"] = reader["UserID"].ToString();
                    Session["SessionLoggedInUser"] = reader["UserID"].ToString();
                    Session["LoggedInUserRole"] = reader["CompanyPosition"].ToString();
                    Session["UserRole"] = reader["UserRole"].ToString();

                    shouldUserChangePassword = DBNull.Value.Equals(reader["ShouldChangePassword"]) ? false: Convert.ToBoolean(reader["ShouldChangePassword"]);
                }

                if (shouldUserChangePassword)
                {
                    Session["ShouldUserChangePassword"] = true;                             

                    Response.Redirect("ForceChangePassword.aspx");
                }
                else
                {
                    Session["ShouldUserChangePassword"] = false;
                    Response.Redirect("Default.aspx");
                }
            }
            else
            {
                ShowErrorMessage();
            }

             
        }

        catch (Exception ex)
        {
            log.Error("Login", ex);
        }
        finally
        {
            connection.Close();
        }
    }

    private void ShowErrorMessage()
    {
        RadWindowManager1.RadAlert("User Name and Password combination was not found. Please try again.", 330, 100, "Login Failure", "", "");
    }

    private void ShowDisabledMessage()
    {
        RadWindowManager1.RadAlert("User is disabled. Please contact your systems administrator.", 330, 100, "Login Failure", "", "");
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        //searchString = e.Argument.ToLower();
        //RadGrid1.Rebind();
        //RadGrid1.Rebind();
    }
}