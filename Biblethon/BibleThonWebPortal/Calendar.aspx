<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Calendar.aspx.cs" Inherits="Alba.Workflow.WebPortal.Calendar" Title="Telerik Web Mail Demo - Calendar" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<telerik:RadAjaxManager runat="Server" ID="RadAjaxManager1">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="RadCalendar1">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadCalendar2" />
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="RadCalendar2">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadCalendar1" />
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="chkDevelopment">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="chkMarketing">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="chkQ1">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="chkQ2">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1"/>
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="RadScheduler1">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1" />
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManager>
	<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
	<div class="calendar-container">
		<telerik:RadCalendar runat="server" ID="RadCalendar1" AutoPostBack="true" EnableMultiSelect="false"
			DayNameFormat="FirstTwoLetters" EnableNavigation="true" EnableMonthYearFastNavigation="false"
			OnSelectionChanged="RadCalendar1_SelectionChanged" OnDefaultViewChanged="RadCalendar1_DefaultViewChanged">
		</telerik:RadCalendar>
		<telerik:RadCalendar runat="server" ID="RadCalendar2" AutoPostBack="true" EnableMultiSelect="false"
			DayNameFormat="FirstTwoLetters" EnableNavigation="false" EnableMonthYearFastNavigation="false"
			OnSelectionChanged="RadCalendar2_SelectionChanged">
		</telerik:RadCalendar>
	</div>
	<telerik:RadPanelBar runat="server" ID="PanelBar" Width="100%">
		<Items>
			<telerik:RadPanelItem runat="server" Text="My Team Calendars" Expanded="true">
				<Items>
					<telerik:RadPanelItem runat="server">
						<ItemTemplate>
							<div class="rpCheckBoxPanel">
								<div>
									<asp:CheckBox ID="chkDevelopment" runat="server" Text="Development" Checked="true"
										AutoPostBack="true" OnCheckedChanged="CheckBoxes_CheckedChanged" />
								</div>
								<div>
									<asp:CheckBox ID="chkMarketing" runat="server" Text="Marketing" Checked="true" AutoPostBack="true"
										OnCheckedChanged="CheckBoxes_CheckedChanged" />
								</div>
							</div>
						</ItemTemplate>
					</telerik:RadPanelItem>
				</Items>
			</telerik:RadPanelItem>
			<telerik:RadPanelItem runat="server" Text="My Calendar" Expanded="true">
				<Items>
					<telerik:RadPanelItem runat="server">
						<ItemTemplate>
							<div class="rpCheckBoxPanel">
								<div>
									<asp:CheckBox ID="chkQ1" runat="server" Text="Personal" Checked="true" AutoPostBack="true"
										OnCheckedChanged="CheckBoxes_CheckedChanged" />
								</div>
								<div>
									<asp:CheckBox ID="chkQ2" runat="server" Text="Work" Checked="true" AutoPostBack="true"
										OnCheckedChanged="CheckBoxes_CheckedChanged" />
								</div>
							</div>
						</ItemTemplate>
					</telerik:RadPanelItem>
				</Items>
			</telerik:RadPanelItem>
		</Items>
	</telerik:RadPanelBar>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
	<telerik:RadScheduler runat="server" ID="RadScheduler1" EnableEmbeddedSkins="True"
		ShowFooter="false" SelectedDate="2009-02-02" DayStartTime="08:00:00" DayEndTime="21:00:00"
		TimeZoneOffset="03:00:00" SelectedView="DayView" Height="100%" Width="100%" TimelineView-UserSelectable="false"
		OnNavigationComplete="RadScheduler1_NavigationComplete" OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
		OnAppointmentDelete="RadScheduler1_AppointmentDelete" OnAppointmentUpdate="RadScheduler1_AppointmentUpdate"
		OnAppointmentInsert="RadScheduler1_AppointmentInsert" FirstDayOfWeek="Monday"
		LastDayOfWeek="Friday" AdvancedForm-Modal="true">
		<ResourceStyles>
			<telerik:ResourceStyleMapping Type="Calendar" Text="Development" ApplyCssClass="rsCategoryGreen" />
			<telerik:ResourceStyleMapping Type="Calendar" Text="Marketing" ApplyCssClass="rsCategoryRed" />
			<telerik:ResourceStyleMapping Type="Calendar" Text="Work" ApplyCssClass="rsCategoryOrange" />
		</ResourceStyles>
	</telerik:RadScheduler>
</asp:Content>
