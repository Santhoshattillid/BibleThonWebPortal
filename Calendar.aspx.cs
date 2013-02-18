using System;
using System.Collections.Generic;
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
using Telerik.Web.UI.Calendar;

namespace Alba.Workflow.WebPortal
{
    public partial class Calendar : System.Web.UI.Page
    {
        private const string ProviderSessionKey = "Telerik.Web.Examples.Scheduler.Outlook2007.DefaultCS";
        private Dictionary<int, string> checkBoxIDs;

        XmlSchedulerProvider Provider
        {
            get
            {
                XmlSchedulerProvider provider = (XmlSchedulerProvider)Session[ProviderSessionKey];
                if (Session[ProviderSessionKey] == null || !IsPostBack)
                {
                    provider = new XmlSchedulerProvider(Server.MapPath("~/App_Data/Appointments_Outlook.xml"), false);
                    Session[ProviderSessionKey] = provider;
                }

                return provider;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            RadScheduler1.Provider = Provider;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            checkBoxIDs = new Dictionary<int, string>();
            checkBoxIDs.Add(1, "chkDevelopment");
            checkBoxIDs.Add(2, "chkMarketing");
            checkBoxIDs.Add(3, "chkQ1");
            checkBoxIDs.Add(4, "chkQ2");

            if (!IsPostBack)
            {
                RadCalendar1.SelectedDate = RadScheduler1.SelectedDate;
                SyncCalendars();
            }
        }

        protected void RadScheduler1_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
        {
            RadCalendar1.FocusedDate = RadScheduler1.SelectedDate;
            SyncCalendars();
        }

        protected void RadCalendar1_DefaultViewChanged(object sender, DefaultViewChangedEventArgs e)
        {
            SyncCalendars();
        }

        private void SyncCalendars()
        {
            RadCalendar2.FocusedDate = RadCalendar1.FocusedDate.AddMonths(1);
        }

        protected void RadCalendar1_SelectionChanged(object sender, SelectedDatesEventArgs e)
        {
            if (RadCalendar1.SelectedDates.Count > 0)
            {
                RadScheduler1.SelectedDate = RadCalendar1.SelectedDate;
                RadCalendar2.SelectedDate = RadCalendar1.SelectedDate;
            }
        }

        protected void RadCalendar2_SelectionChanged(object sender, SelectedDatesEventArgs e)
        {
            if (RadCalendar2.SelectedDates.Count > 0)
            {
                RadScheduler1.SelectedDate = RadCalendar2.SelectedDate;
                RadCalendar1.SelectedDate = RadCalendar2.SelectedDate;
            }
        }

        protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            RadCalendarDay radCalendarDay = new RadCalendarDay(RadCalendar1);
            radCalendarDay.Date = e.Appointment.Start;
            //radCalendarDay.ItemStyle.CssClass = "DayWithAppointments";
            RadCalendar1.SpecialDays.Add(radCalendarDay);
            RadCalendar2.SpecialDays.Add(radCalendarDay);

            e.Appointment.Visible = false;

            foreach (int key in checkBoxIDs.Keys)
            {
                CheckBox chkBox = PanelBar.Items[0].Items[0].FindControl(checkBoxIDs[key]) as CheckBox;
                if (chkBox == null)
                    chkBox = PanelBar.Items[1].Items[0].FindControl(checkBoxIDs[key]) as CheckBox;

                if (chkBox.Checked)
                {
                    Resource userRes = e.Appointment.Resources.GetResource("Calendar", key.ToString());
                    if (userRes != null)
                    {
                        e.Appointment.Visible = true;
                        e.Appointment.CssClass = userRes.Text;
                    }
                }
            }
        }
        protected void RadScheduler1_AppointmentDelete(object sender, SchedulerCancelEventArgs e)
        {
            RadCalendar1.SpecialDays.Clear();
            RadCalendar2.SpecialDays.Clear();
        }

        protected void RadScheduler1_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
        {
            RadCalendar1.SpecialDays.Clear();
            RadCalendar2.SpecialDays.Clear();
        }

        protected void RadScheduler1_AppointmentInsert(object sender, SchedulerCancelEventArgs e)
        {
            if (e.Appointment.Resources.Count < 1)
                e.Appointment.Resources.Add(RadScheduler1.Resources.GetResource("Calendar", "1"));
        }
        protected void CheckBoxes_CheckedChanged(object sender, EventArgs e)
        {
            RadScheduler1.Rebind();
        }
    }
}