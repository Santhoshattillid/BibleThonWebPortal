<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="WorkflowDocuments.aspx.cs" Inherits="Alba.Workflow.WebPortal.WorkflowDocuments"
    Title="Alba360 Advanced Workflow - Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadTreeView runat="Server" ID="RadTreeView1" OnNodeClick="RadTreeView1_NodeClick"
        EnableViewState="false">
        <Nodes>
            <telerik:RadTreeNode Text="Dashboard" Expanded="true" ImageUrl="~/Images/office_clip.gif"
                PostBack="false">
                <Nodes>
                    <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Text="Workflow Forms"
                        NavigateUrl="WorkflowDocuments.aspx" Expanded="True" Selected="true">
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode Text="Tasks" ImageUrl="~/Images/mailbox.gif" Expanded="True">
                        <Nodes>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Open">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Past Due">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Completed">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Started By Me">
                            </telerik:RadTreeNode>
                        </Nodes>
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Text="Profile"
                        NavigateUrl="UserProfile.aspx" Expanded="True">
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode Text="Alerts &amp; Notifications" ImageUrl="~/Images/notes_icon.gif"
                        NavigateUrl="Notes.aspx" Visible="false">
                    </telerik:RadTreeNode>
                </Nodes>
            </telerik:RadTreeNode>
        </Nodes>
    </telerik:RadTreeView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">
        <script type="text/javascript">
			/* <![CDATA[ */
			var toolbar;
			var grid;
			var searchButton;
			var ajaxManager;			

			function onButtonClicked(sender, args) {	
                if(args.get_item().get_text() == "Edit")
                {
                    var grid = $find("<%=RadGrid2.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var selectedRows = MasterTable.get_selectedItems();
                    for (var i = 0; i < selectedRows.length; i++) {
                        var row = selectedRows[i];
                        var cell = MasterTable.getCellByColumnUniqueName(row, "DocumentKey")
                        var cell2 = MasterTable.getCellByColumnUniqueName(row, "DocType");  
                        var strToReplace = cell2.innerHTML;
                        strToReplace = strToReplace.replace(/ /g, "");
                        strToReplace = "/WebPortal/Documents/frm" + strToReplace + ".aspx";              
                        var value = cell.innerHTML  ; 
                        var finalValue = strToReplace + "?DocumentKey=" + value;                                   
                        openWin(finalValue);
                    }
                }
                else
                {
                    var wfDocType = args.get_item().get_text();
                    
                    if(wfDocType == "Biblethon") {
                        openWin("Biblethon/OrderEntry.aspx");
                    }
                    else if(wfDocType == "ShareAthon") {
                        openWin("ShareAThon/OrderEntry.aspx");
                    }
                    else {
                        wfDocType = wfDocType.replace(/ /g, "");
                        openWin("Documents/frm" + wfDocType + ".aspx");
                    }
                    
                }
			}

            function onToolBarClientButtonClicking(sender, args)
            {
                var button = args.get_item();
                if (button.get_commandName() == "DeleteSelected")
                {
                    args.set_cancel(!confirm('Delete record?'));
                }
            }            			
			
            function RowDblClick(sender, eventArgs)  
            {                 
                
                var dataItem = $get(eventArgs.get_id());  
                var grid = sender;  
                var MasterTable = grid.get_masterTableView();  
                var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];  
                var cell = MasterTable.getCellByColumnUniqueName(row, "DocumentKey");  
                var cell2 = MasterTable.getCellByColumnUniqueName(row, "DocType");  
                var strToReplace = cell2.innerHTML;
                strToReplace = strToReplace.replace(/ /g, "");
                strToReplace = "/WebPortal/Documents/frm" + strToReplace + ".aspx";                
                var value = cell.innerHTML;  
                var finalValue = strToReplace + "?DocumentKey=" + value                                
                var newWin = window.open( finalValue, 'EditDocument', 'height=650,width=1300,status=yes,toolbar=no,menubar=no,scrollbars=yes,location=no' ); newWin.focus();
                
              } 			            

            function openWin(args) {
                var oWnd = radopen(args, "RadWindow5");
            }

            function OnClientClose(oWnd, args) {                
                var window2Client = $find("<%= RadWindow5.ClientID %>");
                var arg = args.get_argument();
                if (oWnd == window2Client) {
                    var masterTable = $find("<%= RadGrid2.ClientID %>").get_masterTableView();
                    var detailTable = $find("<%= RadGrid3.ClientID %>").get_masterTableView();
                    masterTable.rebind();
                    detailTable.rebind();                    
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest(); 
                }
               
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function Close() {
                GetRadWindow().close();
            }
            
			/* ]]> */

            
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="RadWindow5" runat="server" Behaviors="Close, Move, Maximize, Resize"
                OnClientClose="OnClientClose" Modal="true" AutoSize="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadSplitter runat="server" ID="RadSplitter1" Width="100%" BorderSize="0"
        BorderStyle="None" PanesBorderSize="0" Height="100%" Orientation="Horizontal">
        <telerik:RadPane runat="server" ID="RadPane2">
            <telerik:RadToolBar runat="server" ID="RadToolBar1" CssClass="home-search-toolbar"
                OnClientButtonClicking="onToolBarClientButtonClicking" EnableViewState="true"
                OnClientButtonClicked="onButtonClicked" AutoPostBack="false">
                <Items>
                    <telerik:RadToolBarDropDown Text="New" ImageUrl="~/Images/office_clip.gif" Enabled="true" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <telerik:RadToolBarButton Text="Edit" ImageUrl="~/Images/saveas.gif" Enabled="true"
                        Value="Edit" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                </Items>
            </telerik:RadToolBar>
            <asp:Label ID="Label2" runat="server" Text="Documents List" Font-Size="Large" Font-Bold="true"
                Font-Names="segoe ui, arial, sans-serif" Visible="false"></asp:Label>
            <telerik:RadGrid ID="RadGrid2" runat="server" CellSpacing="0" GridLines="Horizontal"
                BorderWidth="0px" AllowSorting="True" Style="outline: none" ShowGroupPanel="True"
                OnSelectedIndexChanged="RadGrid2_SelectedIndexChanged" OnNeedDataSource="RadGrid2_NeedDataSource"
                AllowAutomaticDeletes="True" OnDeleteCommand="RadGrid2_DeleteCommand" AllowFilteringByColumn="True"
                AllowPaging="True" PageSize="15" Height="530px" ClientSettings-AllowColumnsReorder="True">
                <GroupingSettings CaseSensitive="false" />
                <ClientSettings Scrolling-AllowScroll="True" Scrolling-UseStaticHeaders="true" Selecting-AllowRowSelect="true"
                    EnablePostBackOnRowClick="true" AllowDragToGroup="true" EnableRowHoverStyle="false">
                    <Selecting AllowRowSelect="True" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    <ClientEvents OnRowDblClick="RowDblClick" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="False" TableLayout="Auto" CommandItemDisplay="None"
                    DataKeyNames="DocumentKey, DocType" Caption="Document List">
                    <CommandItemSettings ExportToPdfText="Export to PDF" ShowAddNewRecordButton="false"
                        ShowRefreshButton="false" />
                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="DocType" FieldName="DocType" FormatString="{0:D}">
                                </telerik:GridGroupByField>
                                <telerik:GridGroupByField FieldAlias="Status" FieldName="Status" FormatString="{0:D}">
                                </telerik:GridGroupByField>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="DocType" SortOrder="Descending"></telerik:GridGroupByField>
                                <telerik:GridGroupByField FieldName="Status" SortOrder="Descending"></telerik:GridGroupByField>
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Status" HeaderStyle-Width="150px" FilterControlAltText="Filter Status column"
                            HeaderText="Status" ReadOnly="True" SortExpression="Status" UniqueName="Status">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocType" HeaderStyle-Width="300px" FilterControlAltText="Filter Doc Type column"
                            HeaderText="Document Type" ReadOnly="True" SortExpression="DocType" UniqueName="DocType">
                            <HeaderStyle Width="300px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentKey" HeaderStyle-Width="300px" FilterControlAltText="Filter Document Key column"
                            HeaderText="Document Key" ReadOnly="True" SortExpression="DocumentKey" UniqueName="DocumentKey">
                            <HeaderStyle Width="300px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Customer" HeaderStyle-Width="350px" FilterControlAltText="Filter Customer column"
                            HeaderText="Customer" ReadOnly="True" SortExpression="Customer" UniqueName="Description">
                            <HeaderStyle Width="350px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedBy" HeaderStyle-Width="150px" FilterControlAltText="Filter Created_By column"
                            HeaderText="Created By" ReadOnly="True" SortExpression="CreatedBy" UniqueName="CreatedBy">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField1" HeaderStyle-Width="150px"
                            FilterControlAltText="Filter UserDefinedField1 column" HeaderText="UserDefinedField1"
                            ReadOnly="True" SortExpression="UserDefinedField1" UniqueName="UserDefinedField1"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField2" HeaderStyle-Width="150px"
                            FilterControlAltText="Filter UserDefinedField2 column" HeaderText="UserDefinedField2"
                            ReadOnly="True" SortExpression="UserDefinedField2" UniqueName="UserDefinedField2"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField3" HeaderStyle-Width="150px"
                            FilterControlAltText="Filter UserDefinedField3 column" HeaderText="UserDefinedField3"
                            ReadOnly="True" SortExpression="UserDefinedField3" UniqueName="UserDefinedField3"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField4" HeaderStyle-Width="150px"
                            FilterControlAltText="Filter UserDefinedField4 column" HeaderText="UserDefinedField4"
                            ReadOnly="True" SortExpression="UserDefinedField4" UniqueName="UserDefinedField4"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField5" HeaderStyle-Width="150px"
                            FilterControlAltText="Filter UserDefinedField5 column" HeaderText="UserDefinedField5"
                            ReadOnly="True" SortExpression="UserDefinedField5" UniqueName="UserDefinedField5"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ConfirmText="Delete this record?" ConfirmDialogType="RadWindow"
                            ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" HeaderText="Delete Record"
                            Visible="false" />
                    </Columns>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
                <PagerStyle AlwaysVisible="True" PageButtonCount="15" />
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>
        </telerik:RadPane>
        <telerik:RadSplitBar runat="server" ID="RadSplitBar1" CollapseMode="Backward" />
        <telerik:RadPane runat="server" ID="RadPane3" Collapsed="true" Height="250px">
            <telerik:RadGrid ID="RadGrid3" runat="server" CellSpacing="0" DataSourceID="LinqDataSource2"
                GridLines="None" BorderWidth="0px" AllowSorting="True" Style="outline: none"
                ShowGroupPanel="True" OnItemDataBound="RadGrid3_ItemDataBound" AllowPaging="True">
                <MasterTableView AutoGenerateColumns="False" DataSourceID="LinqDataSource2" TableLayout="Auto"
                    Caption="Document Details History">
                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Importance" HeaderStyle-Width="100px" FilterControlAltText="Filter Importance column"
                            HeaderText="Importance" ReadOnly="True" SortExpression="Importance" UniqueName="Importance"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="Priority_Image" HeaderText="Priority" HeaderStyle-Width="50px"
                            ReadOnly="True" Visible="true">
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Status" HeaderStyle-Width="100px" FilterControlAltText="Filter Status column"
                            HeaderText="Status" ReadOnly="True" SortExpression="Status" UniqueName="Status">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderStyle-Width="350px" FilterControlAltText="Filter Description column"
                            HeaderText="Task Description" ReadOnly="True" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedBy" HeaderStyle-Width="100px" FilterControlAltText="Filter Created By column"
                            HeaderText="Created By" ReadOnly="True" SortExpression="CreatedBy" UniqueName="CreatedBy">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationDate" HeaderStyle-Width="100px" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Date column" HeaderText="Creation Date"
                            ReadOnly="True" SortExpression="CreationDate" UniqueName="CreationDate" DataFormatString="{0:d}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationTime" HeaderStyle-Width="100px" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Time column" HeaderText="Time" ReadOnly="True"
                            SortExpression="CreationTime" UniqueName="CreationTime" DataFormatString="{0:t}"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedTo" HeaderStyle-Width="100px" FilterControlAltText="Filter Assigned To column"
                            HeaderText="Assigned To" ReadOnly="True" SortExpression="AssignedTo" UniqueName="AssignedTo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DueDate" HeaderStyle-Width="100px" DataType="System.DateTime"
                            FilterControlAltText="Filter Due Date column" HeaderText="Due Date" ReadOnly="True"
                            SortExpression="DueDate" UniqueName="DueDate" DataFormatString="{0:d}" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WorkflowStep" HeaderStyle-Width="100px" FilterControlAltText="Filter Workflow Step column"
                            HeaderText="Stage" ReadOnly="True" SortExpression="WorkflowStep" UniqueName="WorkflowStep"
                            Visible="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionDate" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Date column" HeaderText="Completion Date"
                            ReadOnly="True" SortExpression="CompletionDate" UniqueName="CompletionDate" DataFormatString="{0:d}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionTime" HeaderStyle-Width="100px" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Time column" HeaderText="Completion Time"
                            ReadOnly="True" SortExpression="Time" UniqueName="CompletionTime" DataFormatString="{0:T}"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Comment" HeaderStyle-Width="350px" FilterControlAltText="Filter Comment column"
                            HeaderText="Comment" ReadOnly="True" SortExpression="Comment" UniqueName="Comment">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFInstanceHistoryID" DataType="System.String"
                            FilterControlAltText="Filter WFInstanceHistoryID column" HeaderText="WFInstanceHistoryID"
                            ReadOnly="True" SortExpression="WFInstanceHistoryID" UniqueName="WFInstanceHistoryID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFInstanceID" DataType="System.String" FilterControlAltText="Filter WFInstanceID column"
                            HeaderText="WFInstanceID" ReadOnly="True" SortExpression="WFInstanceID" UniqueName="WFInstanceID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFID" DataType="System.String" FilterControlAltText="Filter WFID column"
                            HeaderText="WFID" ReadOnly="True" SortExpression="WFID" UniqueName="WFID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentKey" DataType="System.String" FilterControlAltText="Filter Document Key column"
                            HeaderText="Document Key" ReadOnly="True" SortExpression="DocumentKey" UniqueName="DocumentKey"
                            Visible="false">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
                <ClientSettings Scrolling-AllowScroll="True" Scrolling-UseStaticHeaders="True">
                </ClientSettings>
            </telerik:RadGrid>
            <asp:LinqDataSource runat="server" ID="LinqDataSource2" ContextTypeName="WorkflowDataContext"
                TableName="AS_WF_VW_WorkflowHistories" Where="(DocumentKey == @DocumentKey)"
                EntityTypeName="" OrderBy="CreationDate, CreationTime">
                <WhereParameters>
                    <asp:SessionParameter DefaultValue="" Name="DocumentKey" SessionField="SelectedDocumentKey"
                        Type="String" />
                </WhereParameters>
            </asp:LinqDataSource>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
