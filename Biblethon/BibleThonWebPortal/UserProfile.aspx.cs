using System;
using System.Linq;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

namespace Alba.Workflow.WebPortal
{
    public partial class UserProfile : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string searchString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle Session Timeouts
            if (Session["LoggedInUser"] == null)
                Response.Redirect("Logout.aspx");

            if (!IsPostBack)
            {
                using (WorkflowDataContext context = new WorkflowDataContext())
                {
                    //Load Header
                    var users = from s in context.AS_WF_Users
                                where s.UserID == Session["SessionLoggedInUser"].ToString()
                                select s;


                    foreach (var c in users)
                    {
                        txtUserID.Text = c.UserID;
                        txtPassword.Text = clsCrypto.DecryptString(c.UserPassword);
                        txtERPUserID.Text = c.ERPUserID;
                        txtFirstName.Text = c.FirstName;
                        txtLastName.Text = c.LastName;
                        txtDepartment.Text = c.Department;
                        txtCompany.Text = c.Company;
                        txtPhone.Text = c.Phone;
                        txtEmailAddress.Text = c.EmailAdd;
                        txtUserRole.Text = c.UserRole;
                    }

                }

                if (Session["UserRole"].ToString().ToUpper() == "WORKFLOW ADMIN")
                    txtUserRole.ReadOnly = false;
                else
                    txtUserRole.ReadOnly = true;
            }
        }


        #region Methods

        private void ShowMessage(string pMessageAlert)
        {
            RadWindowManager1.RadAlert(pMessageAlert, 330, 100, "Password Request", "", "");
        }

        #endregion

        #region Events


        protected void RadToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
        {

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);

            string messageAlert = string.Empty;
            string firstName = string.Empty;
            bool containsDigits = false;
            bool containsUpperCase = false;
            bool containsLowerCase = false;
            bool containsSpecialChar = false;
            int pwMaxLength = 0;
            int pwMinimumLength = 0;


            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ShowMessage("Passwords do not match. Please make sure the password and the confirm password textboxes are identical.");
                    return;
                }

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
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.Digit, null);
                else if (!containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.Digit, null);
                else if (containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperCase, null);
                else if (!containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.LowerCase, null);
                else if (containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndLowerCase, null);
                else if (!containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.UpperAndLowerCase, null);
                else if (containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperAndLowerCase, null);
                else if (!containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.SpecialChar, null);
                else if (containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndSpecialChar, null);
                else if (!containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.UpperCaseAndSpecialChar, null);
                else if (containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndUpperCaseAndSpecialChar, null);
                else if (!containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.LowerCaseAndSpecialChar, null);
                else if (containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.DigitAndLowerCaseAndSpecialChar, null);
                else if (!containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar)
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.UpperAndLowerCaseAndSpecialChar, null);
                else if ((containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar))
                    isPasswordValid = clsPasswordGenerator.IsPasswordValid(txtPassword.Text, clsPasswordGenerator.PasswordRules.All, null);
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
                    if (pwMaxLength > 0 && txtPassword.Text.Length > pwMaxLength)
                    {
                        ShowMessage("The maximum number of characters in the password is " + pwMaxLength);
                        return;
                    }
                    if (pwMinimumLength > 0 && txtPassword.Text.Length < pwMinimumLength)
                    {
                        ShowMessage("The minimum number of characters in the password is " + pwMaxLength);
                        return;
                    }
                }


                //Save the new password to the database (Encrypt First)
                string encryptedNewPassword = clsCrypto.EncryptString(txtPassword.Text);

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
            else
            {
                ShowMessage("Please enter a password.");
               return;

            }
            ShowMessage(messageAlert);


            WorkflowDataContext context = new WorkflowDataContext();

            var user = (from c in context.AS_WF_Users
                        where c.UserID == Session["SessionLoggedInUser"].ToString()
                        select c).First();

            user.UserPassword = clsCrypto.EncryptString(txtPassword.Text);
            user.ERPUserID = txtERPUserID.Text;
            user.Department = txtDepartment.Text;
            user.FirstName = txtFirstName.Text;
            user.LastName = txtLastName.Text;
            user.Company = txtCompany.Text;
            user.Phone = txtPhone.Text;
            user.EmailAdd = txtEmailAddress.Text;
            user.UserRole = txtUserRole.Text;

            try
            {
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error("Error in Saving User Profile:", ex);
            }
        }

        #endregion

    }
}
