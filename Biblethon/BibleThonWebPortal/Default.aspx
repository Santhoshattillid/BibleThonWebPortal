<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Alba.Workflow.WebPortal.Home" Title="Alba360 Advanced Workflow - Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest"
        DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        NavigateUrl="WorkflowDocuments.aspx" Expanded="True">
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode Text="Tasks" ImageUrl="~/Images/mailbox.gif" Expanded="True">
                        <Nodes>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Open"
                                Selected="True">
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
                if (args.get_item().get_text() == "Lock")
                {                          
				    var grid = $find("<%=RadGrid2.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var selectedRows = MasterTable.get_selectedItems();                    
                    for (var i = 0; i < selectedRows.length; i++) {
                      var row = selectedRows[i];
                      var cell = MasterTable.getCellByColumnUniqueName(row, "LockedBy")
                      //here cell.innerHTML holds the value of the cell    
                        if (document.getElementById("<%=lblUser.ClientID %>").value != "&nbsp;" && document.getElementById("<%=lblUser.ClientID %>").value != cell.innerHTML && cell.innerHTML != "&nbsp;")
                        {
                            //alert("Record is already locked by " + cell.innerHTML);
                            //return false;
                        }
                        if (document.getElementById("<%=lblUser.ClientID %>").value != "&nbsp;" && document.getElementById("<%=lblUser.ClientID %>").value == cell.innerHTML && cell.innerHTML != "&nbsp;")
                        {
                            //alert("You have already locked the record. If you want to unlock it, just click on the Unlock button");
                            //return false;
                        }
                    }
                }
                if (args.get_item().get_text() == "Unlock")
                {                          
				    var grid = $find("<%=RadGrid2.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var selectedRows = MasterTable.get_selectedItems();
                    for (var i = 0; i < selectedRows.length; i++) {
                      var row = selectedRows[i];
                      var cell = MasterTable.getCellByColumnUniqueName(row, "LockedBy")
                      //here cell.innerHTML holds the value of the cell    
                        if (document.getElementById("<%=lblUser.ClientID %>").value != "&nbsp;" && document.getElementById("<%=lblUser.ClientID %>").value != cell.innerHTML && cell.innerHTML != "&nbsp;")
                        {
                            //alert("You do not have permission to unlock this record. Record is locked by " + cell.innerHTML);
                            //return false;
                        }                        
                    }
                }
                else if(args.get_item().get_text() == "View Document")
                {
                    var grid = $find("<%=RadGrid2.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var selectedRows = MasterTable.get_selectedItems();
                    for (var i = 0; i < selectedRows.length; i++) {
                        var row = selectedRows[i];
                        var cell = MasterTable.getCellByColumnUniqueName(row, "WFInstanceID")
                        var cell2 = MasterTable.getCellByColumnUniqueName(row, "DocType");  
                        var value = cell.innerHTML  ; 
                        var value2 = cell2.innerHTML ;              
                        var newWin = window.open("DocumentViewer.aspx?WFInstanceID=" +value + "&DocumentType=" + value2, 'EditDocument', 'height=850,width=900,status=yes,toolbar=no,menubar=no,location=no' ); newWin.focus();
                    }
                }
                else if(args.get_item().get_text() == "Approve" || args.get_item().get_text() == "Decline")
                {                
                    var grid = $find("<%=RadGrid2.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var selectedRows = MasterTable.get_selectedItems();
                    var selectedWFInstanceIDs = "";
                    for (var i = 0; i < selectedRows.length; i++) 
                    {
                        var row = selectedRows[i];
                        var cell = MasterTable.getCellByColumnUniqueName(row, "LockedBy")
                        //here cell.innerHTML holds the value of the cell                          
                        if (cell.innerHTML == "&nbsp;")
                        {
                            //alert("Record should be locked first before it can be approved/rejected.");
                            //return false;
                        }
                        else if (document.getElementById("<%=lblUser.ClientID %>").value != cell.innerHTML)
                        {
                            //alert("Record is already locked by " + cell.innerHTML);
                            //    return false;
                        }
                        else
                        {
                            var wfInstanceIDCell = MasterTable.getCellByColumnUniqueName(row, "WFInstanceID")
                            selectedWFInstanceIDs = selectedWFInstanceIDs + wfInstanceIDCell.innerHTML + ";";                        
                        }
                    }
                    
                    if (selectedWFInstanceIDs != "")
                    {
                        var oWnd = $find("<%= RadWindow1.ClientID %>")
                        oWnd.setUrl("ActionComment.aspx?WFInstanceID=" + selectedWFInstanceIDs + "&Action=" + args.get_item().get_text());
                        oWnd.show();
                    }                    

                    //var oWnd = radopen("ActionComment.aspx?WFInstanceID=" + wfInstanceIDCell.innerHTML + "&Action=" + args.get_item().get_text(), "RadWindow1");
                }
			}			

            var doc_Key = "";
			
            function RowDblClick(sender, eventArgs)  
            { 
                var dataItem = $get(eventArgs.get_id());  
                var grid = sender;  
                var MasterTable = grid.get_masterTableView();  
                var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];  
                var cell = MasterTable.getCellByColumnUniqueName(row, "WFInstanceID");  
                var cell2 = MasterTable.getCellByColumnUniqueName(row, "DocType");  
                var value = cell.innerHTML  ; 
                var value2 = cell2.innerHTML ;              
                var newWin = window.open("DocumentViewer.aspx?WFInstanceID=" +value + "&DocumentType=" + value2, 'EditDocument', 'height=850,width=900,status=yes,toolbar=no,menubar=no,location=no' ); newWin.focus();                
              } 
			            
            function OnClientClose(oWnd, args) 
            {               
                var window2Client = $find("<%= RadWindow1.ClientID %>");

                if (oWnd == window2Client) {
                    var masterTable = $find("<%= RadGrid2.ClientID %>").get_masterTableView();                    
                    masterTable.rebind();                    
                }
            }

			/* ]]> */

            
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="false">
        <Windows>
            <telerik:RadWindow runat="server" ID="Edit" NavigateUrl="~/Reply.aspx" 
                Width="870px" Height="550px" ReloadOnShow="true" ShowContentDuringLoad="false"
                Modal="True" Behaviors="Close,Move" VisibleStatusbar="false">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="PromptWIn" NavigateUrl="Calendar.aspx">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" OnClientClose="OnClientClose"
                Width="750px" Height="470px">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadSplitter runat="server" ID="RadSplitter1" Width="100%" BorderSize="0"
        BorderStyle="None" PanesBorderSize="0" Height="100%" Orientation="Horizontal">
        <telerik:RadPane runat="server" ID="RadPane1" Height="32px" EnableViewState="false"
            Scrollable="false">
            <telerik:RadToolBar runat="server" ID="RadToolBar1" CssClass="home-search-toolbar"
                OnClientButtonClicked="onButtonClicked" EnableViewState="false" OnButtonClick="RadToolBar1_ButtonClick">
                <Items>
                    <telerik:RadToolBarButton Text="Approve" ImageUrl="~/Images/approval.png" Enabled="true" PostBack="False" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <telerik:RadToolBarButton Text="Decline" ImageUrl="~/Images/clear.gif" Enabled="true"
                        PostBack="False" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <telerik:RadToolBarButton Text="Lock" ImageUrl="~/Images/delegate.gif" Enabled="true"
                        Value="Lock" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <telerik:RadToolBarButton Text="Unlock" ImageUrl="~/Images/request_change.png" Enabled="true"
                        Value="Unlock" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <telerik:RadToolBarButton Text="View Document" ImageUrl="~/Images/notes_icon.gif"
                        Enabled="true" Value="View Document" />
                    <telerik:RadToolBarButton IsSeparator="true" />
                    <%--<telerik:RadToolBarButton Text="Delegate" ImageUrl="~/Images/delegate.gif" Enabled="true" />--%>
                    <%--<telerik:RadToolBarButton Text="Revise" ImageUrl="~/Images/request_change.png" CommandName="reply" />--%>                                        
                </Items>
            </telerik:RadToolBar>
            <telerik:RadToolBar runat="server" ID="RadToolBar2" CssClass="home-search-toolbar"
                OnClientButtonClicked="onButtonClicked" EnableViewState="false" Visible="false">
                <Items>
                    <telerik:RadToolBarButton Text="View Document" ImageUrl="~/Images/notes_icon.gif"
                        Enabled="true" Value="View Document" />
                    <%--<telerik:RadToolBarButton Text="Delegate" ImageUrl="~/Images/delegate.gif" Enabled="true" />--%>
                    <%--<telerik:RadToolBarButton Text="Revise" ImageUrl="~/Images/request_change.png" CommandName="reply" />--%>
                    <telerik:RadToolBarButton Value="searchTextBoxButton" CommandName="searchText">
                        <ItemTemplate>
                            <telerik:RadTextBox runat="server" ID="RadTextBox1" EmptyMessage="Search Tasks" CssClass="home-search-textbox"
                                Width="300px" ClientEvents-OnKeyPress="onKeyPress" />
                        </ItemTemplate>
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton ImageUrl="~/Images/search.gif" Value="search" CommandName="doSearch" />
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        <telerik:RadPane runat="server" ID="RadPane2" Height="100%">
            <asp:HiddenField ID="lblUser" runat="server" />
            <telerik:RadGrid ID="RadGrid2" runat="server" CellSpacing="0" GridLines="Horizontal"
                Height="530px" AllowSorting="True" Style="outline: none" ShowGroupPanel="True"
                AllowFilteringByColumn="True" OnItemDataBound="RadGrid2_ItemDataBound" OnSelectedIndexChanged="RadGrid2_SelectedIndexChanged"
                OnNeedDataSource="RadGrid2_NeedDataSource" AllowPaging="True" 
                PageSize="15" ClientSettings-AllowColumnsReorder="True" 
                AllowMultiRowSelection="True">
                <GroupingSettings CaseSensitive="false" />
                <ClientSettings Scrolling-AllowScroll="True" Scrolling-UseStaticHeaders="true" Selecting-AllowRowSelect="true"
                    AllowDragToGroup="true" EnableRowHoverStyle="false" EnablePostBackOnRowClick="true"
                    AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                    <Selecting AllowRowSelect="True" />                                        
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="False" TableLayout="Auto">
                    <CommandItemSettings ExportToPdfText="Export to PDF" />
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
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="DocType" SortOrder="Descending"></telerik:GridGroupByField>
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Department" HeaderStyle-Width="150px" FilterControlAltText="Filter Department column"
                            HeaderText="Department" ReadOnly="True" SortExpression="Department" UniqueName="Department"
                            Visible="false" GroupByExpression="Department">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WorkflowName" HeaderStyle-Width="150px" FilterControlAltText="Filter Workflow Name column"
                            HeaderText="Workflow Name" ReadOnly="True" SortExpression="WorkflowName" UniqueName="WorkflowName"
                            Visible="false" GroupByExpression="WorkflowName">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Importance" HeaderStyle-Width="150px" FilterControlAltText="Filter Importance column"
                            HeaderText="Importance" ReadOnly="True" SortExpression="Importance" UniqueName="Importance"
                            Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="Priority_Image" HeaderText="Priority" HeaderStyle-Width="50px"
                            ReadOnly="True" Visible="true" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="50px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="LockedBy" HeaderStyle-Width="250px" FilterControlAltText="Filter LockedBy column"
                            HeaderText="Locked By" ReadOnly="True" SortExpression="LockedBy" UniqueName="LockedBy">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Status" HeaderStyle-Width="150px" FilterControlAltText="Filter Status column"
                            HeaderText="Status" ReadOnly="True" SortExpression="Status" UniqueName="Status">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocType" HeaderStyle-Width="250px" FilterControlAltText="Filter DocType column"
                            HeaderText="Document Type" ReadOnly="True" SortExpression="DocType" UniqueName="DocType">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentKey" HeaderStyle-Width="250px" FilterControlAltText="Filter Document Key column"
                            HeaderText="Document Key" ReadOnly="True" SortExpression="DocumentKey" UniqueName="DocumentKey">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Key2" HeaderStyle-Width="250px" FilterControlAltText="Filter Document Key2 column"
                            HeaderText="Document Key2" ReadOnly="True" SortExpression="DocumentKey2" UniqueName="DocumentKey2">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Key3" HeaderStyle-Width="250px" FilterControlAltText="Filter Document Key3 column"
                            HeaderText="Document Key3" ReadOnly="True" SortExpression="DocumentKey3" UniqueName="DocumentKey3">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField1" HeaderStyle-Width="250px"
                            FilterControlAltText="Filter Customer column" HeaderText="Customer" ReadOnly="True"
                            SortExpression="UserDefinedField1" UniqueName="UserDefinedField1">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField2" HeaderStyle-Width="250px"
                            FilterControlAltText="Filter UserDefinedField2 column" HeaderText="UserDefinedField2"
                            ReadOnly="True" SortExpression="UserDefinedField2" UniqueName="UserDefinedField2"
                            Visible="true">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField3" HeaderStyle-Width="250px"
                            FilterControlAltText="Filter UserDefinedField3 column" HeaderText="UserDefinedField3"
                            ReadOnly="True" SortExpression="UserDefinedField3" UniqueName="UserDefinedField3"
                            Visible="true">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField4" HeaderStyle-Width="250px"
                            FilterControlAltText="Filter UserDefinedField4 column" HeaderText="UserDefinedField4"
                            ReadOnly="True" SortExpression="UserDefinedField4" UniqueName="UserDefinedField4"
                            Visible="true">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserDefinedField5" HeaderStyle-Width="250px"
                            FilterControlAltText="Filter UserDefinedField5 column" HeaderText="UserDefinedField5"
                            ReadOnly="True" SortExpression="UserDefinedField5" UniqueName="UserDefinedField5"
                            Visible="true">
                            <HeaderStyle Width="250px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderStyle-Width="350px" FilterControlAltText="Filter Description column"
                            HeaderText="Task Description" ReadOnly="True" SortExpression="Description" UniqueName="Description">
                            <HeaderStyle Width="350px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedBy" HeaderStyle-Width="150px" FilterControlAltText="Filter Created By column"
                            HeaderText="Created By" ReadOnly="True" SortExpression="CreatedBy" UniqueName="CreatedBy">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationDate" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Date column" HeaderText="Creation Date"
                            ReadOnly="True" SortExpression="CreationDate" UniqueName="CreationDate" DataFormatString="{0:d}">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationTime" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Time column" HeaderText="Creation Time"
                            ReadOnly="True" SortExpression="CreationTime" UniqueName="CreationTime" DataFormatString="{0:t}"
                            Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedTo" HeaderStyle-Width="150px" FilterControlAltText="Filter Assigned To column"
                            HeaderText="Assigned To" ReadOnly="True" SortExpression="AssignedTo" UniqueName="AssignedTo">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DueDate" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Due Date column" HeaderText="Due Date" ReadOnly="True"
                            SortExpression="DueDate" UniqueName="DueDate" DataFormatString="{0:d}">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WorkflowStep" HeaderStyle-Width="150px" FilterControlAltText="Filter Workflow Step column"
                            HeaderText="Workflow Step" ReadOnly="True" SortExpression="WorkflowStep" UniqueName="WorkflowStep"
                            Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionDate" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Date column" HeaderText="Completion Date"
                            ReadOnly="True" SortExpression="CompletionDate" UniqueName="CompletionDate" DataFormatString="{0:d}">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionTime" HeaderStyle-Width="150px" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Time column" HeaderText="Completion Time"
                            ReadOnly="True" SortExpression="CompletionTime" UniqueName="CompletionTime" DataFormatString="{0:T}"
                            Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Comment" HeaderStyle-Width="450px" FilterControlAltText="Filter Comment column"
                            HeaderText="Comment" ReadOnly="True" SortExpression="Comment" UniqueName="Comment">
                            <HeaderStyle Width="450px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFID" DataType="System.String" FilterControlAltText="Filter WFID column"
                            HeaderText="WFID" ReadOnly="True" SortExpression="WFID" UniqueName="WFID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFContextID" DataType="System.String" FilterControlAltText="Filter WFContextID column"
                            HeaderText="WFID" ReadOnly="True" SortExpression="WFContextID" UniqueName="WFContextID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WFInstanceID" DataType="System.String" FilterControlAltText="Filter WFInstanceID column"
                            HeaderText="WFInstanceID" ReadOnly="True" SortExpression="WFInstanceID" UniqueName="WFInstanceID"
                            Visible="true">
                            <HeaderStyle Width="350px" />
                        </telerik:GridBoundColumn>
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
            <asp:LinqDataSource ID="LinqDataSource3" runat="server" ContextTypeName="WorkflowDataContext"
                EntityTypeName="" Select="new (Department, WorkflowName, DocType, Description, CreatedBy, CreationDate, CreationTime, AssignedTo, Comment, WorkflowStep, CompletionDate, CompletionTime, Importance, DueDate, Status, DocumentKey, Key2, Key3, WFInstanceID, WFID, WFContextID)"
                TableName="AS_WF_VW_WorkflowTasks">
                <WhereParameters>
                    <asp:SessionParameter DefaultValue="" Name="Status" SessionField="SelectedWFInstanceStatus"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="" Name="AssignedTo" SessionField="SelectedWFInstanceAssignedTo"
                        Type="String" />
                    <asp:SessionParameter Name="DueDateStart" SessionField="SelectedWFInstanceDueDateStart"
                        Type="DateTime" />
                    <asp:SessionParameter Name="DueDateEnd" SessionField="SelectedWFInstanceDueDateEnd"
                        Type="DateTime" />
                    <asp:SessionParameter DefaultValue="" Name="WorkflowStep" SessionField="SelectedWFInstanceWorkflowStep"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="" Name="CreatedBy" SessionField="SelectedWFInstanceCreatedBy"
                        Type="String" />
                </WhereParameters>
            </asp:LinqDataSource>
            <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="WorkflowDataContext"
                EntityTypeName="" TableName="AS_WF_VW_WorkflowHistories">
                <WhereParameters>
                    <asp:SessionParameter DefaultValue="" Name="Status" SessionField="SelectedWFInstanceStatus"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="" Name="Created_By" SessionField="SelectedWFInstanceCreatedBy"
                        Type="String" />
                </WhereParameters>
            </asp:LinqDataSource>
        </telerik:RadPane>
        <telerik:RadSplitBar runat="server" ID="RadSplitBar1" CollapseMode="Backward" />
        <telerik:RadPane runat="server" ID="RadPane3" Height="250px" Collapsed="true">
            <telerik:RadGrid ID="RadGrid3" runat="server" CellSpacing="0" DataSourceID="LinqDataSource2"
                GridLines="None" BorderWidth="0px" Height="500px" AllowSorting="True" Style="outline: none"
                ShowGroupPanel="True" OnItemDataBound="RadGrid3_ItemDataBound" AllowPaging="True">
                <MasterTableView AutoGenerateColumns="False" DataSourceID="LinqDataSource2" TableLayout="Auto">
                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Importance" FilterControlAltText="Filter Importance column"
                            HeaderStyle-Width="150px" HeaderText="Importance" ReadOnly="True" SortExpression="Importance"
                            UniqueName="Importance" Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Priority" ReadOnly="True"
                            UniqueName="Priority_Image" Visible="true">
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="50px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="LockedBy" FilterControlAltText="Filter LockedBy column"
                            HeaderStyle-Width="250px" HeaderText="Locked By" ReadOnly="True" SortExpression="LockedBy"
                            UniqueName="LockedBy">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                            HeaderStyle-Width="150px" HeaderText="Status" ReadOnly="True" SortExpression="Status"
                            UniqueName="Status">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                            HeaderStyle-Width="450px" HeaderText="Task Description" ReadOnly="True" SortExpression="Description"
                            UniqueName="Description">
                            <HeaderStyle Width="450px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentKey" FilterControlAltText="Filter Document Key column"
                            HeaderStyle-Width="450px" HeaderText="Document Key" ReadOnly="True" SortExpression="DocumentKey"
                            UniqueName="DocumentKey">
                            <HeaderStyle Width="450px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter Created By column"
                            HeaderStyle-Width="150px" HeaderText="Created By" ReadOnly="True" SortExpression="CreatedBy"
                            UniqueName="CreatedBy">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationDate" DataFormatString="{0:d}" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Date column" HeaderStyle-Width="150px"
                            HeaderText="Creation Date" ReadOnly="True" SortExpression="CreationDate" UniqueName="CreationDate">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreationTime" DataFormatString="{0:t}" DataType="System.DateTime"
                            FilterControlAltText="Filter Creation Time column" HeaderStyle-Width="150px"
                            HeaderText="Time" ReadOnly="True" SortExpression="CreationTime" UniqueName="CreationTime"
                            Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedTo" FilterControlAltText="Filter Assigned To column"
                            HeaderStyle-Width="150px" HeaderText="Assigned To" ReadOnly="True" SortExpression="AssignedTo"
                            UniqueName="AssignedTo">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DueDate" DataFormatString="{0:d}" DataType="System.DateTime"
                            FilterControlAltText="Filter Due Date column" HeaderStyle-Width="150px" HeaderText="Due Date"
                            ReadOnly="True" SortExpression="DueDate" UniqueName="DueDate" Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WorkflowStep" FilterControlAltText="Filter Workflow Step column"
                            HeaderStyle-Width="150px" HeaderText="Stage" ReadOnly="True" SortExpression="WorkflowStep"
                            UniqueName="WorkflowStep" Visible="true">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionDate" DataFormatString="{0:d}" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Date column" HeaderStyle-Width="150px"
                            HeaderText="Completion Date" ReadOnly="True" SortExpression="CompletionDate"
                            UniqueName="CompletionDate">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CompletionTime" DataFormatString="{0:T}" DataType="System.DateTime"
                            FilterControlAltText="Filter Completion Time column" HeaderStyle-Width="150px"
                            HeaderText="Time" ReadOnly="True" SortExpression="CompletionTime" UniqueName="CompletionTime"
                            Visible="false">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Comment" FilterControlAltText="Filter Comment column"
                            HeaderStyle-Width="450px" HeaderText="Comment" ReadOnly="True" SortExpression="Comment"
                            UniqueName="Comment">
                            <HeaderStyle Width="450px" />
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
                    </Columns>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
                <PagerStyle AlwaysVisible="True" />
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
                <ClientSettings Scrolling-AllowScroll="True" Scrolling-UseStaticHeaders="True">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:LinqDataSource runat="server" ID="LinqDataSource2" ContextTypeName="WorkflowDataContext"
                TableName="AS_WF_VW_WorkflowHistories" Where="(WFInstanceID == @WFInstanceID)"
                EntityTypeName="" OrderBy="CreationDate, CreationTime">
                <WhereParameters>
                    <asp:SessionParameter DefaultValue="" Name="WFInstanceID" SessionField="SelectedWFInstance"
                        Type="String" />
                </WhereParameters>
            </asp:LinqDataSource>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
