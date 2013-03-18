<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmGaiamChargebackDocument.aspx.cs"
    Inherits="frmGaiamChargebackDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alba360 Advanced Workflow - New Gaiam Chargeback Document</title>
    <style type="text/css">
        td.NewDocumentFirstColumn
        {
            padding-left: 10px;
        }
    </style>
</head>
<body onunload="refreshParent();" onload="changeScreenSize(1050,1100)">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
        //<![CDATA[


            function changeScreenSize(w, h) {
                var oWindow = GetRadWindow();                
            }

            function openWin() {
                var oWnd = radopen("GaiamChargeBackAuthorizationLookup.aspx", "RadWindow1");
            }

            function OnClientClose(oWnd, args) {
                //get the transferred arguments
                var window2Client = $find("<%= RadWindow2.ClientID %>");
                var arg = args.get_argument();
                if (oWnd == window2Client) {
                    var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                    masterTable.rebind();

                    var docTotal = arg.documentTotal;
                    var txtTot = $find("<%= txtTotalAmount.ClientID %>");
                    txtTot.set_value(docTotal);
                }
                else {
                    if (arg) {
                        var documentKey = arg.documentKey;
                        //                $get("order").innerHTML = "AuthorizationNumber is <strong>" + authorizationNumber + "</strong>";
                        var radTextBox1 = $find("<%= txtAuthorizationNumber.ClientID %>");
                        radTextBox1.set_value(documentKey);
                    }
                }
            }

            function openAccountWin() {
                var wnd = window.radopen("GaiamAccountLookup.aspx", "RadWindow3");
                return false;
            }

            function OnAccountClientClose(oWnd, args) {
                //get the transferred arguments                
                var arg = args.get_argument();
                if (arg) {
                    var documentKey = arg.documentKey;
                    var lblOrder = $find("<%= txtAccountNumber.ClientID %>");
                    lblOrder.set_value(documentKey);
                }
            }

            function openVendorWin() {
                var wnd = window.radopen("GaiamVendorLookup.aspx", "RadWindow4");
                return false;
            }

            function OnVendorClientClose(oWnd, args) {
                //get the transferred arguments                
                var arg = args.get_argument();
                if (arg) {
                    var documentKey = arg.documentKey;
                    var lblOrder = $find("<%= txtVendorID.ClientID %>");
                    lblOrder.set_value(documentKey);
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

            function refreshParent() {

            }

            function pageLoad() {

            }

            function alertCallBackFn(arg) {
                window.close();
            }

            function CloseEditPage(args) {
                alert(args);
                self.close();
            }

            function printWin() {
                window.print();

            }

            function onButtonClicked(sender, args) {
                if (args.get_item().get_text() == "Print") {
                    window.print();
                }                
            }

            function ShowWindow() {
                var docKey = $find("<%= txtAuthorizationNumber.ClientID %>");

                var windowToOpen = "GaiamChargeBackImportData.aspx?AuthorizationNumber=" + docKey.get_value();
                var oWnd = radopen(windowToOpen, "RadWindow2");
                oWnd.remove_pageLoad(windowLoad);
                oWnd.add_pageLoad(windowLoad);
            }

            function windowLoad(oWindow, args) {
                oWindow.set_title("Import Data");
            }

            function returnToParent(args) {
                //get a reference to the current RadWindow
                alert(args);
                var oWnd = GetRadWindow();
                oWnd.close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) {
                    oWindow = window.radWindow;
                }
                else if (window.frameElement.radWindow) {
                    oWindow = window.frameElement.radWindow;
                }
                return oWindow;
            }

            function CustomRadWindowConfirm(sender, args) {
                if (args.get_item().get_text() == "Delete") {
                    //radconfirm('<h3 style=\'color: #333399;\'>Are you sure?</h3>', confirmCallBackFn, 330, 100, null, 'RadConfirm custom title');                    
                    //Cancel the postback
                    args.set_cancel(!confirm('Are you sure you want to delete this document?'));
                }
            }

            function confirmCallBackFn(arg) {
                radalert("<strong>radconfirm</strong> returned the following result: <h3 style='color: #ff0000;'>" + arg + "</h3>", null, null, "Result");
            }            

        //]]>
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadPanelBar runat="server" ID="RadPanelBar1" Width="1100px" Height="100%">
        <Items>
            <telerik:RadPanelItem runat="server" ID="RadPane1" Expanded="true">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadToolBar runat="server" ID="RadToolBar1" CssClass="home-search-toolbar" OnClientButtonClicking="CustomRadWindowConfirm"
                                    OnClientButtonClicked="onButtonClicked" EnableViewState="false" OnButtonClick="RadToolBar1_ButtonClick">
                                    <Items>
                                        <telerik:RadToolBarButton Text="Save" ImageUrl="~/Images/approval.png" Enabled="true"
                                            ValidationGroup="BasicInfoValidationGroup" />
                                        <telerik:RadToolBarButton Text="Submit" ImageUrl="~/Images/delegate.gif" Enabled="true"
                                            ValidationGroup="BasicInfoValidationGroup" />
                                        <telerik:RadToolBarButton Text="Print" ImageUrl="~/Images/print.gif" Enabled="true" />                                        
                                        <telerik:RadToolBarButton Text="Delete" ImageUrl="~/Images/move.gif" Enabled="true" Visible="true"/>
                                        <telerik:RadToolBarButton Text="Export Items to Excel" ImageUrl="~/Images/move.gif" Enabled="true" Visible="true"/>                                        
                                    </Items>
                                </telerik:RadToolBar>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblUser" runat="server" Text="Logged-In User: " Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem runat="server" ID="RadPane3" Expanded="true" Text="Header">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="8" class="NewDocumentFirstColumn">
                                <b>Gaiam Chargeback Document</b>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label17" runat="server" Text="Authorization Number:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtAuthorizationNumber" runat="server" Width="300px" ReadOnly="true"
                                    AutoPostBack="True">
                                </telerik:RadTextBox>
                                <button id="btnLookup" onclick="openWin(); return false;" runat="server" visible="false">
                                    ...</button>
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtStatus" runat="server" Width="170px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td rowspan="11" valign="top">
                                <asp:Panel ID="Panel3" runat="server" Width="100%">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" BorderColor="Red" BorderWidth="1px"
                                        HeaderText="List of errors:" ValidationGroup="BasicInfoValidationGroup" Width="100%" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label3" runat="server" Text="Gaiam Account Number:"></asp:Label>
                            </td>
                            <td colspan="5">
                                 <telerik:RadTextBox ID="txtAccountNumber" runat="server" Width="270px" ReadOnly="true"
                                                        AutoPostBack="True">
                                                    </telerik:RadTextBox>
                                <asp:Button ID="btnAccountLookup" runat="server" Text="..." OnClientClick="openAccountWin(); return false; " />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtAccountNumber" ErrorMessage="Gaiam Account is required"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Account Name:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtAccountName" runat="server" Width="300px" ReadOnly="true">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="accountNameValidator" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtAccountName" ErrorMessage="Account Name is required" Text="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label4" runat="server" Text="Event Name:"></asp:Label>
                            </td>
                            <td colspan="9">
                                <telerik:RadTextBox ID="txtEventName" runat="server" Width="715px" MaxLength="30">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtEventName" ErrorMessage="Event Name is required" Text="*"></asp:RequiredFieldValidator>
                            </td>                            
                        </tr>
                        <tr>                            
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label12" runat="server" Text="Sales Person:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtSalesPerson" runat="server" Width="300px" ReadOnly="true">
                                </telerik:RadTextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtSalesPerson" ErrorMessage="Sales Person is required" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Expense Type:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadComboBox ID="cboExpenseType" runat="server" Width="300px" OnSelectedIndexChanged="cboExpenseType_SelectedIndexChanged"
                                    AutoPostBack="True">                                   
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                        <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label13" runat="server" Text="Indirect Account Name:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtIndirectAccountName" runat="server" Width="300px">
                                </telerik:RadTextBox>
                            </td>                            
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Expense SubType:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadComboBox ID="cboExpenseSubType" runat="server" Width="300px">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr style="visibility:hidden;">                  
                            <td class="NewDocumentFirstColumn">                                
                            </td>          
                            <td colspan="3">
                                <asp:CheckBox ID="chkExpensePaidByInvoice" runat="server" Text="Expense Paid By Invoice?"
                                    OnCheckedChanged="chkExpensePaidByInvoice_CheckedChanged" TextAlign="Right" AutoPostBack="true" Visible="false"/>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsExpenseStudioApproved" runat="server" Text="Is Expense Studio Approved?"
                                    TextAlign="Right" Visible="false"/>
                            </td>
                        </tr>                        
                        <tr>                            
                            <td>
                                <asp:Label ID="lblVendorID" runat="server" Text="Vendor ID:" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVendorID" runat="server" Width="75px" Visible="false" ReadOnly="true" AutoPostBack="true">
                                </telerik:RadTextBox>
                                <asp:Button ID="btnVendorLookup" runat="server" Text="..." OnClientClick="openVendorWin(); return false; " Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name:" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVendorName" runat="server" Width="150px" Visible="false" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn" rowspan="2">
                                <asp:Label ID="Label14" runat="server" Text="Description:"></asp:Label>
                            </td>
                            <td colspan="5" rowspan="2">
                                <telerik:RadTextBox ID="txtSpecialInstructions" runat="server" Width="300px" TextMode="MultiLine" Height="50px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Event Start Date:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadDatePicker ID="dtStart" runat="server" MinDate="1900-01-01" AutoPostBack="true">
                                    <Calendar ID="Calendar1" RangeMinDate="1900-01-01" runat="server">
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>                            
                        </tr>
                        <tr>                                                 
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Event End Date:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadDatePicker ID="dtEnd" runat="server" MinDate="1900-01-01" AutoPostBack="true">
                                    <Calendar ID="Calendar2" RangeMinDate="1900-01-01" runat="server">
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="lblTotalAmount" runat="server" Text="Item Total Amount:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadNumericTextBox ID="txtTotalAmount" runat="server" Width="300px" Type="Currency"
                                    Value="0" NumberFormat-DecimalDigits="2" NumberFormat-AllowRounding="false"
                                    EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" ReadOnly="true">
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblClaimedAmount" runat="server" Text="Claimed Amount:" Visible="false"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadNumericTextBox ID="txtClaimedAmount" runat="server" Width="300px" Type="Currency"
                                    Value="0"  NumberFormat-DecimalDigits="2" NumberFormat-AllowRounding="false"
                                    EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Visible="false">
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem runat="server" ID="RadPane4" Expanded="true" Text="Attachments">
                <ContentTemplate>
                    <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" />
                    <table>
                        <tr>
                            <td>
                                Attachments:
                            </td>
                        </tr>
                        <tr>
                            <td id="controlContainer">
                                <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="5" OverwriteExistingFiles="false" />
                                <telerik:RadProgressArea ID="progressArea1" runat="server" />
                                <asp:Button ID="buttonSubmit" runat="server" CssClass="RadUploadSubmit" OnClick="buttonSubmit_Click"
                                    Text="Upload" />
                            </td>
                            <td>
                                <div class="smallModule">
                                    <div class="rc1">
                                        <div class="rc2">
                                            <div class="rc3" style="width: 240px">
                                                <asp:Label ID="labelNoResults" runat="server" Visible="True">No uploaded files yet</asp:Label>
                                                <asp:Repeater ID="repeaterResults" runat="server" Visible="False" OnItemDataBound="repeaterResults_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <div class="title">
                                                            Attachments:</div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                            <asp:HyperLink runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FileName")%>'
                                                                ID="HyperLink1">
                                                            </asp:HyperLink>
                                                        </telerik:RadCodeBlock>
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem runat="server" ID="RadPane2" Expanded="true" Text="Details">
                <ContentTemplate>
                    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
                    <telerik:RadGrid runat="server" ID="RadGrid1" DataSourceID="LinqDataSource1" AllowAutomaticUpdates="True"
                        AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AutoGenerateColumns="False"
                        AllowPaging="True" OnItemUpdated="RadGrid1_ItemUpdated" OnItemInserted="RadGrid1_ItemInserted"
                        OnItemDeleted="RadGrid1_ItemDeleted" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound"
                        Height="400px" ExportSettings-HideStructureColumns="True" OnExcelMLWorkBookCreated="RadGrid1_ExcelMLWorkBookCreated"
                        OnExcelMLExportRowCreated="RadGrid1_ExcelMLExportRowCreated" ExportSettings-ExportOnlyData="True"
                        OnGridExporting="RadGrid1_GridExporting" PagerStyle-AlwaysVisible="True" CellSpacing="0"
                        GridLines="None" PageSize="8" PagerStyle-PageButtonCount="8" OnDataBound="RadGrid1_DataBound" ExportSettings-IgnorePaging="True" OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand">
                        <ExportSettings ExportOnlyData="True" HideStructureColumns="true">
                            <Excel Format="ExcelML" />
                        </ExportSettings>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="RecordID" CommandItemDisplay="Top"
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" AllowPaging="true" UseAllDataFields="true"
                            ShowFooter="True" ShowGroupFooter="true" >
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="RecordID" HeaderText="RecordID"
                                    Visible="false" UniqueName="RecordID" />
                                <telerik:GridBoundColumn DataField="AuthorizationNumber" HeaderText="AuthorizationNumber"
                                    Visible="false" UniqueName="AuthorizationNumber" />
                                <%--<telerik:GridBoundColumn DataField="GAIAMItemNumber" HeaderText="GAIAM Item Number" />--%>
                                <telerik:GridBoundColumn DataField="GAIAMItemNumber" HeaderText="GAIAMItemNumber"
                                    UniqueName="GAIAMItemNumber" Visible="false" />
                                <telerik:GridTemplateColumn UniqueName="cboItemSKU" HeaderText="GAIAM Item Number"
                                    SortExpression="ItemSKU" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                    <FooterTemplate>
                                        Template footer</FooterTemplate>
                                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "GAIAMItemNumber")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox runat="server" ID="cboItemSKU" EnableLoadOnDemand="True" DataTextField="ItemSKU"
                                            OnItemsRequested="cboItemSKU_ItemsRequested" DataValueField="ItemSKU" AutoPostBack="true"
                                            HighlightTemplatedItems="true" Height="140px" Width="200px" DropDownWidth="420px"
                                            OnSelectedIndexChanged="OnSelectedIndexChangedHandler">
                                            <HeaderTemplate>
                                                GAIAM Item Number : Description
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "Text")%>
                                                :
                                                <%# DataBinder.Eval(Container, "Attributes['ITEMDESC']")%>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="UPCCode" HeaderText="UPC Code" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                <telerik:GridBoundColumn DataField="TitlePromotion" HeaderText="Title" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                                <telerik:GridBoundColumn DataField="Studio" HeaderText="Studio" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                                <telerik:GridNumericColumn DataField="AmountPerUnit" HeaderText="Amount Per Unit"
                                    ColumnEditorID="GridNumericColumnEditor2" ItemStyle-Width="100px" HeaderStyle-Width="100px" DataFormatString="{0:C}" />
                                <telerik:GridNumericColumn DataField="Quantity" HeaderText="Quantity" ColumnEditorID="GridNumericColumnEditor1" ItemStyle-Width="100px" HeaderStyle-Width="100px" DataType="System.Int32" DataFormatString="{0:###,##0}" />
                                <telerik:GridNumericColumn Aggregate="Sum" DataField="TotalAmount" HeaderText="Total Amount"
                                    ColumnEditorID="GridNumericColumnEditor3" FooterText="Current Total Amount:" ItemStyle-Width="100px" HeaderStyle-Width="100px" DataFormatString="{0:C}" FooterAggregateFormatString="Current Total Amount: {0:C}" />
                                    <telerik:GridBoundColumn DataField="InvalidText" HeaderText="Invalid SKU"></telerik:GridBoundColumn>
                                <%--<telerik:GridNumericColumn DataField="AuthorizedAmount" HeaderText="Authorized Amount"
                                    ColumnEditorID="GridNumericColumnEditor4" />--%>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" />
                                <telerik:GridButtonColumn ConfirmText="Delete this item?" ConfirmDialogType="RadWindow"
                                    UniqueName="DeleteButtonCommand" ConfirmTitle="Delete" ButtonType="ImageButton"
                                    CommandName="Delete" />
                            </Columns>
                            <EditFormSettings>
                                <EditColumn ButtonType="ImageButton" />
                                <PopUpSettings Modal="true" />
                            </EditFormSettings>
                            <CommandItemSettings ShowExportToExcelButton="false" />
                            <CommandItemTemplate>
                                <%--<asp:LinkButton ID="LinkButton4" runat="server" CommandName="InitInsert"><img style="border:0px" alt="" src="../Images/AddRecord.gif" />Add New Record</asp:LinkButton>--%>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="ExportDocToExcel"><img style="border:0px" alt="" src="../Images/move.gif" /> Export Items to XLS</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="ImportDocFromExcel"><img style="border:0px" alt="" src="../Images/move.gif" /> Import Items from XLS</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="DownloadTemplate"><img style="border:0px" alt="" src="../Images/move.gif" /> Download Template</asp:LinkButton>
                            </CommandItemTemplate>
                            <PagerStyle AlwaysVisible="True"></PagerStyle>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Studio" FieldName="Studio" FormatString="{0:D}">
                                        </telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="Studio" SortOrder="Descending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                        </MasterTableView>
                        <%--<ExportSettings>
                                <Excel Format="Biff" />
                            </ExportSettings>--%>
                        <PagerStyle AlwaysVisible="true" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                    <telerik:GridNumericColumnEditor ID="GridNumericColumnEditor1" runat="server">
                        <NumericTextBox ID="NumericTextBox1" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                            MinValue="0" Type="Number">
                            <NumberFormat GroupSeparator="," />
                        </NumericTextBox>
                    </telerik:GridNumericColumnEditor>
                    <telerik:GridNumericColumnEditor ID="GridNumericColumnEditor2" runat="server">
                        <NumericTextBox ID="NumericTextBox2" runat="server" MaxLength="0" NumberFormat-DecimalDigits="2"
                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                            MinValue="0" Type="Currency">
                            <NumberFormat GroupSeparator="" />
                        </NumericTextBox>
                    </telerik:GridNumericColumnEditor>
                    <telerik:GridNumericColumnEditor ID="GridNumericColumnEditor3" runat="server">
                        <NumericTextBox ID="NumericTextBox3" runat="server" MaxLength="0" NumberFormat-DecimalDigits="2"
                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                            MinValue="0" Type="Currency">
                            <NumberFormat GroupSeparator="" />
                        </NumericTextBox>
                    </telerik:GridNumericColumnEditor>
                    <telerik:GridNumericColumnEditor ID="GridNumericColumnEditor4" runat="server">
                        <NumericTextBox ID="NumericTextBox4" runat="server" MaxLength="0" NumberFormat-DecimalDigits="2"
                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                            MinValue="0" Type="Currency">
                            <NumberFormat GroupSeparator="" />
                        </NumericTextBox>
                    </telerik:GridNumericColumnEditor>
                    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
                        ReloadOnShow="true" runat="server" Skin="Sunset" EnableShadow="true">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" OnClientClose="OnClientClose"
                                NavigateUrl="GaiamChargeBackAuthorizationLookup.aspx">
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" OnClientClose="OnClientClose"
                                Width="780px" Height="630px" Modal="true" >
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close" OnClientClose="OnAccountClientClose"
                                Width="750px" Height="450px" NavigateUrl="GaiamAccountLookup.aspx">
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Close" OnClientClose="OnVendorClientClose"
                                Width="750px" Height="450px" NavigateUrl="GaiamVendorLookup.aspx">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="ChargebackDataDataContext"
                        EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="AS_GAIAM_Chargeback_Details"
                        Where="AuthorizationNumber == @AuthorizationNumber">
                        <WhereParameters>
                            <asp:SessionParameter Name="AuthorizationNumber" SessionField="AuthorizationNumber"
                                Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </ContentTemplate>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>
    <telerik:RadTextBox ID="txtStreet" runat="server" Width="300px" ReadOnly="true" Visible="false"></telerik:RadTextBox>                                            
    <telerik:RadTextBox ID="txtPhone" runat="server" Width="300px" Visible="false"></telerik:RadTextBox>
    <telerik:RadTextBox ID="txtCity" runat="server" Width="100px" ReadOnly="true" Visible="false"></telerik:RadTextBox>                                                            
    <telerik:RadTextBox ID="txtState" runat="server" Width="35px" ReadOnly="true" Visible="false"></telerik:RadTextBox>                                                            
    <telerik:RadTextBox ID="txtZip" runat="server" Width="80px" ReadOnly="true" Visible="false"></telerik:RadTextBox>    
    <telerik:RadTextBox ID="txtCountry" runat="server" Width="300px" ReadOnly="true" Visible="false"></telerik:RadTextBox>
    <asp:CheckBox ID="chkEstimate" runat="server" Text="Estimate?" TextAlign="Right" OnCheckedChanged="chkEstimate_CheckedChanged" AutoPostBack="true" Visible="false"/>
    </form>   
</body>
</html>
