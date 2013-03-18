using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;

namespace Alba.Workflow.WebPortal
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadPanelItem item = RadPanelBar1.FindItemByUrl(Request.Url.PathAndQuery);
            if (item != null)
            {
                item.Selected = true;
            }

            // Disabled Google Analytics for everything but telerik.com
            AnalyticsPlaceholder.Visible = Request.Url.Host.ToLowerInvariant().Contains("telerik.com");

            lblLoggedInUser.Text = String.Format("Welcome, {0} | {1}", Session["SessionLoggedInUser"], Session["LoggedInUserRole"]);
            lblUserRole.InnerText = Session["UserRole"].ToString().ToUpper();
            lblCompanyName.InnerText = ConfigurationManager.AppSettings["CompanyName"];

            if (!IsPostBack)
            {

                foreach (RadMenuItem menuItem in RadMenu1.Items)
                {
                    if (menuItem.Text == "Edit")
                    {
                        string pathToCreate = "~/Documents/EditForms/" + ConfigurationManager.AppSettings["CompanyName"];
                        if (Directory.Exists(Server.MapPath(pathToCreate)))
                        {
                            string[] filelist = Directory.GetFiles(Server.MapPath(pathToCreate));

                            foreach (string str in filelist)
                            {
                                //string fileName = pathToCreate + "/" + Path.GetFileName(str);
                                string filePath = Path.GetFileName(str);
                                string[] filePathArray = filePath.Split('_');
                                string newFilePath = string.Empty;
                                foreach (string strFilePath in filePathArray)
                                {
                                    if (newFilePath == string.Empty)
                                        newFilePath = strFilePath;
                                    else
                                        newFilePath += " " + strFilePath;
                                }

                                newFilePath = newFilePath.Replace(".aspx", string.Empty);
                                RadMenuItem newRadMenuItem = new RadMenuItem(newFilePath);
                                newRadMenuItem.ImageUrl = "~/Images/mark.gif";
                                menuItem.Items.Add(newRadMenuItem);
                            }
                        }
                    }
                }

            }
        }
    }
}
