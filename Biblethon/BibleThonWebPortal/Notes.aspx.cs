using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using Telerik.Web.UI;

namespace Alba.Workflow.WebPortal
{
    public partial class Notes : Page
    {
        private List<Note> NotesList
        {
            get
            {
                if (Session["Notes"] == null)
                {
                    List<Note> notes = new List<Note>();

                    notes.Add(new Note("Submit whitepaper proposal", ""));
                    notes.Add(new Note("Buy birthday present for Jill", "Personal"));
                    notes.Add(new Note("Weekly data backup", ""));
                    notes.Add(new Note("Book ski vacation in St. Anton", "Personal"));
                    notes.Add(new Note("Reserve table at ANW Vegetarian Restaurant", "Personal"));
                    notes.Add(new Note("Pick up exam results from the clinic", ""));
                    notes.Add(new Note("Order the new JavaScript book on Amazon", ""));
                    notes.Add(new Note("Call attorneys and schedule a meeting", "Work"));
                    notes.Add(new Note("Send a cancellation for the planning meeting", "Work"));
                    notes.Add(new Note("Buy tickets for Madonna's concert", "Personal"));
                    notes.Add(new Note("Return Simpson's call", "Work"));
                    notes.Add(new Note("Take Samantha to the annual art exhibition", "Personal"));
                    notes.Add(new Note("Check account balance", ""));
                    notes.Add(new Note("Order office materials", "Work"));
                    notes.Add(new Note("Send memo to Carla Anderson", "Work"));

                    Session["Notes"] = notes;
                }
                return (List<Note>)Session["Notes"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadContextMenu1.DataBind();
        }

        protected void ByCategory_CheckedChanged(object sender, EventArgs e)
        {
            GroupBy("Category");
        }

        protected void List_CheckedChanged(object sender, EventArgs e)
        {
            GroupBy("");
        }

        private void GroupBy(string columnName)
        {
            RadGrid1.MasterTableView.GroupByExpressions.Clear();
            if (!string.IsNullOrEmpty(columnName))
                RadGrid1.MasterTableView.GroupByExpressions.Add(
                    new GridGroupByExpression(RadGrid1.MasterTableView.GetColumn(columnName)));
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = NotesList;
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            NoteEditData data = new JavaScriptSerializer().Deserialize<NoteEditData>(e.Argument);
            Note note = NotesList.Find(n => n.Id.ToString() == data.Id);
            switch (data.Command)
            {
                case NoteCommand.Insert:
                    NotesList.Add(new Note(data.Content, data.Category));
                    break;
                case NoteCommand.Update:
                    note.Subject = data.Content;
                    break;
                case NoteCommand.Delete:
                    NotesList.Remove(note);
                    break;
                case NoteCommand.Categorize:
                    note.Category = data.Category;
                    break;
            }

            RadGrid1.Rebind();
        }

        private class NoteEditData
        {
            public String Id { get; set; }
            public string Content { get; set; }
            public string Category { get; set; }
            public NoteCommand Command { get; set; }
        }

        private enum NoteCommand
        {
            Insert,
            Update,
            Delete,
            Categorize
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                if (item.DataCell.Text == "Category: ")
                    item.DataCell.Text += "(none)";
            }
        }
    }
}