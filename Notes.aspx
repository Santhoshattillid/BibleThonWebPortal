<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Notes.aspx.cs" Inherits="Alba.Workflow.WebPortal.Notes" Title="Telerik Web Mail Demo - Notes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<telerik:RadAjaxManager runat="Server" ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="List">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="ByCategory" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="ByCategory">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="List" />
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManager>
	<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
	</telerik:RadAjaxLoadingPanel>
	<telerik:RadPanelBar runat="server" ID="RadPanelBar1" Width="100%">
		<Items>
			<telerik:RadPanelItem Text="Current View" Expanded="True" runat="server">
				<Items>
					<telerik:RadPanelItem runat="server">
						<ItemTemplate>
							<ul style="list-style: none; margin: 0; padding: 0">
								<li>
									<asp:RadioButton GroupName="groupBy" runat="server" ID="List" Text="List" OnCheckedChanged="List_CheckedChanged"
										AutoPostBack="True" Checked="true" /></li>
								<li>
									<asp:RadioButton GroupName="groupBy" runat="server" ID="ByCategory" Text="Group by category"
										OnCheckedChanged="ByCategory_CheckedChanged" AutoPostBack="True" />
								</li>
							</ul>
						</ItemTemplate>
					</telerik:RadPanelItem>
				</Items>
			</telerik:RadPanelItem>
		</Items>
	</telerik:RadPanelBar>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
	<telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

		<script type="text/javascript">

			var text = null;
			var id = null;

			function onRowDoubleClick(sender, args) {
				args.get_domEvent().stopPropagation();
				text = args.getDataKeyValue("Subject");
				id = args.getDataKeyValue("Id")
				window.radopen(null, "Edit");
			}


			function onRowContextMenu(sender, args) {
				var contextMenu = $find("<%= RadContextMenu1.ClientID %>");
				text = args.getDataKeyValue("Subject");
				id = args.getDataKeyValue("Id");
				contextMenu.show(args.get_domEvent());
			}

			function onWindowLoad(sender, args) {
				sender.get_contentFrame().contentWindow.setEditorContent(text);
			}

			function insertNote(id, content) {
				var commandData = { Id: id, Content: content, Command: 0 };
				executeNoteCommand(commandData);
			}

			function deleteNote(id) {
				var commandData = { Id: id, Command: 2 };
				executeNoteCommand(commandData);
			}

			function updateNote(id, content) {
				var commandData = { Id: id, Content: content, Command: 1 };
				executeNoteCommand(commandData);
			}

			function categorizeNote(id, category) {
				var commandData = { Id: id, Category: category, Command: 3 };
				executeNoteCommand(commandData);
			}

			function executeNoteCommand(commandData) {
				var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
				ajaxManager.ajaxRequest(Sys.Serialization.JavaScriptSerializer.serialize(commandData));
			}

			function onWindowClose(sender, args) {
				var content = args.get_argument();
				if (content !== null) {
					if (id < 0)
						insertNote(id, content);
					else
						updateNote(id, content);
				}
			}

			function onContextMenuItemClicked(sender, args) {
				var item = args.get_item();
				sender.close();

				if (item.get_text() == "Open") {
					window.radopen(null, "Edit");
				}
				else if (item.get_text() == "Delete") {
					deleteNote(id);
				}
				else if (item.get_text() == "Personal" || item.get_text() == "Work") {
					categorizeNote(id, item.get_text());
				}
			}

			$telerik.$(document).ready(function() {
				$telerik.$("div.grid-placeholder").bind("dblclick", function() {
					id = -1;
					text = "Note Text ...";
					window.radopen(null, "Edit");
				}
				);
			}
		);
		
		</script>

	</telerik:RadCodeBlock>
	<telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="false">
		<Windows>
			<telerik:RadWindow runat="server" ID="Edit" NavigateUrl="~/Compose.aspx" OnClientPageLoad="onWindowLoad"
				Width="870px" Height="550px" OnClientClose="onWindowClose" ReloadOnShow="true"
				ShowContentDuringLoad="false" Modal="True" Behaviors="Close,Move" VisibleStatusbar="false" >
			</telerik:RadWindow>
		</Windows>
	</telerik:RadWindowManager>
	<telerik:RadContextMenu runat="server" ID="RadContextMenu1" OnClientItemClicked="onContextMenuItemClicked">
		<Items>
			<telerik:RadMenuItem Text="Open" />
			<telerik:RadMenuItem Text="Categorize">
				<Items>
					<telerik:RadMenuItem Text="Work" />
					<telerik:RadMenuItem Text="Personal" />
				</Items>
			</telerik:RadMenuItem>
			<telerik:RadMenuItem IsSeparator="true" />
			<telerik:RadMenuItem Text="Delete" ImageUrl="~/Images/delete.gif" />
		</Items>
	</telerik:RadContextMenu>
	<div style="height: 100%;" class="grid-placeholder">
		<telerik:RadGrid runat="server" ID="RadGrid1" OnNeedDataSource="RadGrid1_NeedDataSource"
			AutoGenerateColumns="false" AllowSorting="True" OnItemDataBound="RadGrid1_ItemDataBound"
			EnableViewState="false" Height="100%" CssClass="notes-list">
			<ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
				<ClientEvents OnRowDblClick="onRowDoubleClick" OnRowContextMenu="onRowContextMenu" />
			</ClientSettings>
			<MasterTableView ClientDataKeyNames="Subject, Id" GroupLoadMode="Client">
				<Columns>
					<telerik:GridTemplateColumn DataField="Subject" HeaderText="Subject" SortExpression="Subject">
						<ItemTemplate>
							<img src="Images/notes_icon.gif" alt="" /><%# Eval("Subject")%>
						</ItemTemplate>
					</telerik:GridTemplateColumn>
					<telerik:GridBoundColumn DataField="Created" HeaderText="Created On" HeaderStyle-Width="200px" />
					<telerik:GridBoundColumn DataField="Category" HeaderText="Category" HeaderStyle-Width="200px" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
</asp:Content>
