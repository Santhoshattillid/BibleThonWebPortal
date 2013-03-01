using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;
using System.Xml;
using System.IO;

public partial class ForgotPassword : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {                
        RadWindowManager1.RadAlert("You need to change your password.", 330, 100, "Change Password", "", "");    
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Check of the passwords match
        if (txtNewPassword.Text.Trim() != txtConfirmNewPassword.Text.Trim())
        {
            ShowMessage("The passwords you entered do not match. Please try again.");

            return;
        }

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);

        string messageAlert = string.Empty;
        string firstName = string.Empty;
        bool containsDigits = false;
        bool containsUpperCase = false;
        bool containsLowerCase = false;
        bool containsSpecialChar = false;
        int pwMaxLength = 0;
        int pwMinimumLength = 0;

        if (!string.IsNullOrEmpty(txtNewPassword.Text))
        {            
            //Retrieve Password Rules
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT [ContainsDigit], [ContainsUpperCase], [ContainsLowerCase], [ContainsSpecialChar], [MaxLength], [MininumLength] FROM [AS_WF_PasswordRules]", connection);

                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    containsDigits = DBNull.Value.Equals(reader["ContainsDigit"]) ? false : Convert.ToBoolean(reader["ContainsDigit"]);
                    containsUpperCase = DBNull.Value.Equals(reader["ContainsUpperCase"]) ? false : Convert.ToBoolean(reader["ContainsUpperCase"]);
                    containsLowerCase = DBNull.Value.Equals(reader["ContainsLowerCase"]) ? false : Convert.ToBoolean(reader["ContainsLowerCase"]);
                    containsSpecialChar = DBNull.Value.Equals(reader["ContainsSpecialChar"]) ? false : Convert.ToBoolean(reader["ContainsSpecialChar"]);
                    pwMaxLength = DBNull.Value.Equals(reader["MaxLength"]) ? 0 : Convert.ToInt32(reader["MaxLength"]);
                    pwMinimumLength = DBNull.Value.Equals(reader["MininumLength"]) ? 0 : Convert.ToInt32(reader["MininumLength"]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Force New Password - Retrieve Password Rules:", ex);
                ShowMessage("An error was encountered. Please contact your systems admin.");
                return;
            }
            finally
            {
                connection.Close();                
            }
            
            //Generate New Password
            bool isPasswordValid = false;

            if (containsDigits && !containsUpperCase && !containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.Digit, null);
            else if (!containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.Digit, null);
            else if (containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperCase, null);
            else if (!containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.LowerCase, null);
            else if (containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndLowerCase, null);
            else if (!containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.UpperAndLowerCase, null);
            else if (containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperAndLowerCase, null);
            else if (!containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.SpecialChar, null);
            else if (containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndSpecialChar, null);
            else if (!containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.UpperCaseAndSpecialChar, null);
            else if (containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperCaseAndSpecialChar, null);
            else if (!containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.LowerCaseAndSpecialChar, null);
            else if (containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndLowerCaseAndSpecialChar, null);
            else if (!containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar)
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.UpperAndLowerCaseAndSpecialChar, null);
            else if ((containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar))
                isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtNewPassword.Text, clsPasswordGenerator.PasswordRules.All, null);
            else if ((!containsDigits && !containsUpperCase && !containsLowerCase && !containsSpecialChar)) // No Rules
                isPasswordValid = true;

            if (!isPasswordValid)
            {
                string rulesMessage = "The password you entered does not conform with the password rules set by your admin. Make sure the following rules are followed: ";

                if (containsDigits)
                    rulesMessage += "At least one Digit. ";
                if (containsUpperCase)
                    rulesMessage += "At least one Uppercase Letter. ";
                if (containsLowerCase)
                    rulesMessage += "At least one Lowercase Letter. ";
                if (containsSpecialChar)
                    rulesMessage += "At least one AlphaNumeric Character (examples: *$-+?_&=!%{}/). ";

                ShowMessage(rulesMessage);

                return;
            }
            else
            {
                if (pwMaxLength > 0 && txtNewPassword.Text.Length > pwMaxLength)
                {
                    ShowMessage("The maximum number of characters in the password is " + pwMaxLength);
                    return;
                }
                if (pwMinimumLength > 0 && txtNewPassword.Text.Length < pwMinimumLength)
                {
                    ShowMessage("The minimum number of characters in the password is " + pwMaxLength);
                    return;
                }
            }


            //Save the new password to the database (Encrypt First)
            string encryptedNewPassword = clsCrypto.EncryptString(txtNewPassword.Text);

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE AS_WF_Users SET UserPassword = @UserPassword, ShouldChangePassword = 0 WHERE UserID = @UserID", connection);
                cmd.Parameters.AddWithValue("@UserID", Session["LoggedInUser"].ToString());
                cmd.Parameters.AddWithValue("@UserPassword", encryptedNewPassword);

                connection.Open();
                cmd.ExecuteNonQuery();

                messageAlert = "Successfully changed password.";
            }
            catch (Exception ex)
            {
                log.Error("Saving Forced New Password:", ex);
                ShowMessage("An error was encountered. Please contact your systems admin.");
                return;
            }
            finally
            {
                connection.Close();
            }
        }            

        ShowMessage(messageAlert);

        Session["ShouldUserChangePassword"] = false;

        Response.Redirect("Default.aspx");
    }

    private void ShowMessage(string pMessageAlert)
    {
        RadWindowManager1.RadAlert(pMessageAlert, 330, 100, "Password Request", "", "");
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        //searchString = e.Argument.ToLower();
        //RadGrid1.Rebind();
        //RadGrid1.Rebind();
    }
}