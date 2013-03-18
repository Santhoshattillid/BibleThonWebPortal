<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Contacts.aspx.cs" Inherits="Alba.Workflow.WebPortal.Contacts" Title="Telerik Web Mail Demo - Contacts" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="PhoneList">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="City" />
					<telerik:AjaxUpdatedControl ControlID="Country" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="City">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="PhoneList" />
					<telerik:AjaxUpdatedControl ControlID="Country" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="Country">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="PhoneList" />
					<telerik:AjaxUpdatedControl ControlID="City" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="RadComboBox2">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManager>
	<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
	</telerik:RadAjaxLoadingPanel>
	<telerik:RadPanelBar runat="server" ID="RadPanelBar1" Width="100%" >
		<Items>
			<telerik:RadPanelItem Text="Current View" Expanded="True" runat="server">
				<Items>
					<telerik:RadPanelItem runat="server">
						<ItemTemplate>
							<ul style="list-style: none; margin: 0; padding: 0">
								<li>
									<asp:RadioButton GroupName="groupBy" runat="server" ID="PhoneList" Text="Phone List"
										OnCheckedChanged="PhoneList_CheckedChanged" AutoPostBack="True" Checked="true" /></li>
								<li>
									<asp:RadioButton GroupName="groupBy" runat="server" ID="City" Text="By City" OnCheckedChanged="City_CheckedChanged"
										AutoPostBack="True" /></li>
								<li>
									<asp:RadioButton GroupName="groupBy" runat="server" ID="Country" Text="By Country"
										OnCheckedChanged="Country_CheckedChanged" AutoPostBack="True" /></li>
							</ul>
						</ItemTemplate>
					</telerik:RadPanelItem>
				</Items>
			</telerik:RadPanelItem>
		</Items>
	</telerik:RadPanelBar>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
	<telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">

		<script type="text/javascript">
			var showDropDown = false;
			function onSelectedIndexChanged(sender, args) {
				var parentId = sender.get_id().replace("RadComboBox1", "");
				var valueComboBox = $find(parentId + "RadComboBox2");
				valueComboBox.set_text("");
				valueComboBox.get_items().clear();
				valueComboBox.requestItems("");
				showDropDown = true;
			}
			function onItemsRequested(sender, args) {
				if (sender.get_items().get_count() > 0 && showDropDown) {
					sender.showDropDown();
					showDropDown = false;
				}
			}
			function onItemsRequesting(sender, args) {
				var parentId = sender.get_id().replace("RadComboBox2", "");
				var filterComboBox = $find(parentId + "RadComboBox1");
				args.get_context()["filter"] = filterComboBox.get_value();
			}

			function onKeyPressing(sender, args) {
				var toolbar = $find("<%= RadToolBar1.ClientID %>");
				var searchButton = toolbar.get_items().getItem(1);
				searchButton.set_imageUrl("images/search.gif");
				searchButton.set_value("search");
			}

			function updateToolBar(sender, args) {
				var toolbar = $find("<%= RadToolBar1.ClientID %>");
				var searchButton = toolbar.get_items().getItem(1);
				searchButton.set_imageUrl("Images/clear.gif");
				searchButton.set_value("clear");
			}

			function onButtonClicked(sender, args) {
				var command = args.get_item().get_value();
				
				if (command != "search" && command != "clear")
					return;
					
				var valueComboBox = sender.get_items().getItem(0).findControl("RadComboBox2");
				
				var searchButton = args.get_item();

				if (command == "clear") {
					valueComboBox.set_text("");
					searchButton.set_imageUrl("images/search.gif");
					searchButton.set_value("search");
				} else {
					valueComboBox.set_value(valueComboBox.get_text());
				}

				if (valueComboBox.get_text()) {
					searchButton.set_imageUrl("images/clear.gif");
					searchButton.set_value("clear");
				}
				
				var uniqueID = valueComboBox.get_id().replace(/_/ig, "$");
				var ajaxManager = $find("<%=RadAjaxManager1.ClientID %>");
				ajaxManager.ajaxRequestWithTarget(uniqueID);
			}

			function onGridRowClicked(sender, args) {
				args.get_tableView().fireCommand("ExpandCollapse", args.get_itemIndexHierarchical());
			}
		</script>

	</telerik:RadScriptBlock>
	<telerik:RadSplitter runat="server" ID="RadSplitter1" Width="100%" BorderSize="0"
		BorderStyle="None" PanesBorderSize="0" Height="100%" Orientation="Horizontal">
		<telerik:RadPane runat="server" ID="filterPane" Height="32px">
			<telerik:RadToolBar runat="server" ID="RadToolBar1" Width="100%" CssClass="contacts-filter-toolbar"
				OnClientButtonClicked="onButtonClicked" EnableViewState="false">
				<Items>
					<telerik:RadToolBarButton>
						<ItemTemplate>
							<div style="float: right">
								<label for="ctl00_ContentPlaceHolder2_RadToolBar1_i0_RadComboBox1_Input">
									Filter:</label>
								<telerik:RadComboBox runat="server" ID="RadComboBox1" OnClientSelectedIndexChanged="onSelectedIndexChanged">
									<Items>
										<telerik:RadComboBoxItem Text="Country" Value="Country" />
										<telerik:RadComboBoxItem Text="City" Value="City" />
										<telerik:RadComboBoxItem Text="Name" Value="ContactName" />
									</Items>
								</telerik:RadComboBox>
								<telerik:RadComboBox runat="server" ID="RadComboBox2" EnableLoadOnDemand="true" OnItemsRequested="RadComboBox2_ItemsRequested"
									Height="200px" OnClientItemsRequesting="onItemsRequesting" AutoPostBack="true"
									OnSelectedIndexChanged="RadComboBox2_SelectedIndexChanged" Filter="Contains"
									OnClientItemsRequested="onItemsRequested" OnClientSelectedIndexChanged="updateToolBar"
									OnClientKeyPressing="onKeyPressing">
								</telerik:RadComboBox>
							</div>
						</ItemTemplate>
					</telerik:RadToolBarButton>
					<telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search">
					</telerik:RadToolBarButton>
				</Items>
			</telerik:RadToolBar>
		</telerik:RadPane>
		<telerik:RadPane runat="server" ID="gridPane">
			<telerik:RadGrid runat="Server" ID="RadGrid1" DataSourceID="LinqDataSource1" GridLines="None"
				Height="100%" BorderWidth="0" AllowSorting="true" Style="outline: none">
				<ClientSettings Scrolling-AllowScroll="True" Scrolling-UseStaticHeaders="true" EnableRowHoverStyle="true" ClientEvents-OnRowClick="onGridRowClicked" Selecting-AllowRowSelect="true" >
				</ClientSettings>
				<MasterTableView AutoGenerateColumns="False" Width="100%">
					<Columns>
						<telerik:GridBoundColumn DataField="ContactName" HeaderText="Name" SortExpression="ContactName"
							UniqueName="ContactName">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn DataField="City" HeaderText="City" SortExpression="City"
							UniqueName="City">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn DataField="Country" HeaderText="Country" SortExpression="Country"
							UniqueName="Country">
						</telerik:GridBoundColumn>
					</Columns>
					<NestedViewTemplate>
						<div class="contact-details">
							<asp:Image runat="server" ID="Image1" ImageUrl='<%# "~/Images/Contacts/" + Eval("CustomerID") + ".jpg" %>' />
							<ul>
								<li><label>Name:</label><%# Eval("ContactName") %></li>
								<li><label>Country:</label><%# Eval("Country") %></li>
								<li><label>City:</label><%# Eval("City") %></li>
								<li><label>Address:</label><%# Eval("Address") %></li>
								<li><label>Company:</label><%# Eval("CompanyName") %></li>
								<li><label>Phone:</label><%# Eval("Phone") %></li>
								<li><label>Fax:</label><%# Eval("Fax") %></li>
							</ul>
						</div>
					</NestedViewTemplate>
				</MasterTableView>
				
			</telerik:RadGrid>
			<asp:LinqDataSource runat="server" ID="LinqDataSource1" ContextTypeName="HelpDeskDataContext"
				TableName="Customers">
				<WhereParameters>
					<asp:ControlParameter ControlID="ctl00$ContentPlaceHolder2$RadToolBar1$i0$RadComboBox2"
						Name="Value" DefaultValue="true" />
				</WhereParameters>
			</asp:LinqDataSource>
		</telerik:RadPane>
	</telerik:RadSplitter>
</asp:Content>
