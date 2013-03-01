using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

namespace Alba.Workflow.WebPortal
{
    public partial class Contacts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PhoneList_CheckedChanged(object sender, EventArgs e)
        {
            GroupBy("");
        }

        protected void City_CheckedChanged(object sender, EventArgs e)
        {
            GroupBy("City");
        }

        protected void Country_CheckedChanged(object sender, EventArgs e)
        {
            GroupBy("Country");
        }

        private void GroupBy(string columnName)
        {
            RadGrid1.MasterTableView.GroupByExpressions.Clear();
            if (!string.IsNullOrEmpty(columnName))
                RadGrid1.MasterTableView.GroupByExpressions.Add(
                    new GridGroupByExpression(RadGrid1.MasterTableView.GetColumn(columnName)));
            RadGrid1.Rebind();
        }

        protected void RadComboBox2_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox RadComboBox2 = (RadComboBox)sender;
            string filterBy = (string)e.Context["filter"];

            HelpDeskDataContext context = new HelpDeskDataContext();

            string criteria = e.Text.ToLower();
            if (filterBy == "Country")
            {

                RadComboBox2.DataSource = (from customer in context.Customers
                                           where customer.Country.ToLower().Contains(criteria)
                                           orderby customer.Country
                                           select new { Column = customer.Country }).Distinct();
            }
            else if (filterBy == "ContactName")
            {
                RadComboBox2.DataSource = (from customer in context.Customers
                                           where customer.ContactName.ToLower().Contains(criteria)
                                           orderby customer.ContactName
                                           select new { Column = customer.ContactName }).Distinct();
            }
            else if (filterBy == "City")
            {
                RadComboBox2.DataSource = (from customer in context.Customers
                                           where customer.City.ToLower().Contains(criteria)
                                           orderby customer.City
                                           select new { Column = customer.City }).Distinct();
            }

            RadComboBox2.DataValueField = "Column";
            RadComboBox2.DataTextField = "Column";
            RadComboBox2.DataBind();
        }

        protected void RadComboBox2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox RadComboBox2 = (RadComboBox)sender;
            RadComboBox RadComboBox1 = (RadComboBox)RadComboBox2.NamingContainer.FindControl("RadComboBox1");
            if (String.IsNullOrEmpty(RadComboBox2.SelectedValue))
            {
                LinqDataSource1.Where = "";
            }
            else
            {
                LinqDataSource1.Where = RadComboBox1.SelectedValue + ".Contains(@Value)";
            }
        }
    }
}