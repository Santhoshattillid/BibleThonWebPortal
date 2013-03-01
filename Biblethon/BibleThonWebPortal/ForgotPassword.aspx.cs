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
    }

    protected void Button1_Click(object sender, EventArgs e)
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

        if (!string.IsNullOrEmpty(txtEmailAddress.Text))
        {
            //Verify if email address exists
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT EmailAdd FROM AS_WF_Users WHERE EmailAdd = @EmailAdd", connection);
                cmd.Parameters.AddWithValue("@EmailAdd", txtEmailAddress.Text);

                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                object obj = cmd.ExecuteScalar();
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                    firstName = obj.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Sending New Password - Verify Email Address:", ex);
            }
            finally
            {
                if(connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
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
                    containsDigits = reader["ContainsDigit"] is DBNull ? false : Convert.ToBoolean(reader["ContainsDigit"]);
                    containsUpperCase = reader["ContainsUpperCase"] is DBNull ? false : Convert.ToBoolean(reader["ContainsUpperCase"]);
                    containsLowerCase = reader["ContainsLowerCase"] is DBNull ? false : Convert.ToBoolean(reader["ContainsLowerCase"]);
                    containsSpecialChar = reader["ContainsSpecialChar"] is DBNull ? false : Convert.ToBoolean(reader["ContainsSpecialChar"]);
                    pwMaxLength = reader["MaxLength"] is DBNull ? 0 : Convert.ToInt32(reader["MaxLength"]);
                    pwMinimumLength = reader["MininumLength"] is DBNull ? 0 : Convert.ToInt32(reader["MininumLength"]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Sending New Password - Retrieve Password Rules:", ex);
            }
            finally
            {
                connection.Close();
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                //Generate New Password
                string generatedPassword = clsPasswordGenerator.Generate(pwMaxLength, pwMinimumLength);

                bool isPasswordValid = false;

                while (!isPasswordValid)
                {
                    if (containsDigits && !containsUpperCase && !containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.Digit, null);
                    else if (!containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.Digit, null);
                    else if (containsDigits && containsUpperCase && !containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndUpperCase, null);
                    else if (!containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.LowerCase, null);
                    else if (containsDigits && !containsUpperCase && containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndLowerCase, null);
                    else if (!containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.UpperAndLowerCase, null);
                    else if (containsDigits && containsUpperCase && containsLowerCase && !containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndUpperAndLowerCase, null);
                    else if (!containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.SpecialChar, null);
                    else if (containsDigits && !containsUpperCase && !containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndSpecialChar, null);
                    else if (!containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.UpperCaseAndSpecialChar, null);
                    else if (containsDigits && containsUpperCase && !containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndUpperCaseAndSpecialChar, null);
                    else if (!containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.LowerCaseAndSpecialChar, null);
                    else if (containsDigits && !containsUpperCase && containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.DigitAndLowerCaseAndSpecialChar, null);
                    else if (!containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar)
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.UpperAndLowerCaseAndSpecialChar, null);
                    else if ((containsDigits && containsUpperCase && containsLowerCase && containsSpecialChar))
                        isPasswordValid = clsPasswordGenerator.IsPasswordValid(generatedPassword, clsPasswordGenerator.PasswordRules.All, null);
                    else if ((!containsDigits && !containsUpperCase && !containsLowerCase && !containsSpecialChar)) // No Rules
                        isPasswordValid = true;

                    if (!isPasswordValid)
                        generatedPassword = clsPasswordGenerator.Generate(pwMaxLength, pwMinimumLength);
                }

                //Save the new password to the database (Encrypt First)
                string encryptedNewPassword = clsCrypto.EncryptString(generatedPassword);

                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE AS_WF_Users SET UserPassword = @UserPassword, ShouldChangePassword = 1 WHERE EmailAdd = @EmailAdd", connection);
                    cmd.Parameters.AddWithValue("@EmailAdd", txtEmailAddress.Text);
                    cmd.Parameters.AddWithValue("@UserPassword", encryptedNewPassword);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Sending New Password:", ex);
                }
                finally
                {
                    connection.Close();
                }

                //Email the New Password
                try
                {
                    string configFile = string.Format("{0}{1}", Server.MapPath("~"), @"\ASConfig.xml");

                    XmlDocument xmlDoc = new XmlDocument(); //* create an xml document object.
                    xmlDoc.Load(configFile); //* load the XML document from the specified file.

                    //* Get elements.
                    XmlNodeList SMTPUserName = xmlDoc.GetElementsByTagName("SMTP_UserName");
                    XmlNodeList SMTPPassword = xmlDoc.GetElementsByTagName("SMTP_Password");
                    XmlNodeList SMTPServerHostName = xmlDoc.GetElementsByTagName("SMTP_ServerHostName");
                    XmlNodeList SMTPNotificationBodyFilePath = xmlDoc.GetElementsByTagName("SMTP_NotificationBodyFilePath");
                    XmlNodeList SMTPHostPort = xmlDoc.GetElementsByTagName("SMTP_HostPort");
                    XmlNodeList SMTPHostSSL = xmlDoc.GetElementsByTagName("SMTP_IsSSLEnabled");

                    string userName = SMTPUserName[0].InnerText;
                    string passWord = SMTPPassword[0].InnerText;
                    string host = SMTPServerHostName[0].InnerText;
                    string emailHTMLBody = string.Empty;
                    int port = Convert.ToInt32(SMTPHostPort[0].InnerText);

                    //Read the contents of the html and setup the body
                    //using (StreamReader reader = new StreamReader(SMTPNotificationBodyFilePath[0].InnerText))
                    using (StreamReader reader = new StreamReader(Server.MapPath("~") + @"\EmailTemplate.HTML"))
                    {
                        String line = String.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            emailHTMLBody += line;
                        }
                    }

                    emailHTMLBody = emailHTMLBody.Replace("[EmailVariable1]", "Sir/Mam");
                    emailHTMLBody = emailHTMLBody.Replace("[EmailVariable2]", txtEmailAddress.Text);
                    emailHTMLBody = emailHTMLBody.Replace("[EmailVariable3]", generatedPassword);

                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                    //Add the recipient
                    message.To.Add(txtEmailAddress.Text);

                    message.Subject = "Request For New Password";
                    message.From = new System.Net.Mail.MailAddress(userName);
                    message.Body = emailHTMLBody;
                    message.IsBodyHtml = true;

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host, port);
                    smtp.EnableSsl = SMTPHostSSL == null ? true : Convert.ToBoolean(SMTPHostSSL[0].InnerText);
                    smtp.Credentials = new System.Net.NetworkCredential(userName, passWord);

                    smtp.Send(message);

                    messageAlert = "Your new password has been sent to your e-mail account. Please check your e-mail. You would be required to change your password on your next login.";
                }
                catch (Exception ex)
                {
                    messageAlert = ex.Message;
                    log.Error("SendEmail", ex);
                }
            }
            else
                messageAlert = "E-mail address does not exist";
        }
        else
            messageAlert = "Please enter your e-mail address.";

        ShowMessage(messageAlert);
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