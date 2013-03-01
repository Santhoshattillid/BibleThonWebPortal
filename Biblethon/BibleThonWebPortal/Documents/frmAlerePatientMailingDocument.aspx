<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAlerePatientMailingDocument.aspx.cs"
    Inherits="frmAlerePatientMailingDocument" %>

<%@ Register Src="~/UserControls/Alere_PatientMailing_Summary.ascx" TagName="Alere_PatientMailing_Summary_CTRL"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alba360 Advanced Workflow - New Alere Patient Mailing Document</title>
    <link href="../Styles/styleAlereDocument.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="changeScreenSize(1200,1200)" style="width: 1200px">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager1"
        EnableShadow="true" ShowOnTopWhenMaximized="false">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close, Resize, Move" OnClientClose="OnOrderClientClose"
                NavigateUrl="AlerePatientMailingDocumentOrderIDLookup.aspx" AutoSize="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close, Resize, Move" OnClientClose="OnPatientClientClose"
                Width="750px" Height="450px" NavigateUrl="AlerePatientIDLookup.aspx" AutoSize="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
        //<![CDATA[

                function changeScreenSize(w, h) {
                    window.resizeTo(w, h)
                }

                function openOrderWin() {
                    var wnd = window.radopen("AlerePatientMailingDocumentOrderIDLookup.aspx", "RadWindow1");
                    return false;
                }

                function OnOrderClientClose(oWnd, args) {
                    //get the transferred arguments                
                    var arg = args.get_argument();
                    if (arg) {
                        var documentKey = arg.documentKey;
                        var lblOrder = $find("<%= txtOrderID.ClientID %>");
                        lblOrder.set_value(documentKey);
                    }
                }

                function GetRadOrderWindow() {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    return oWindow;
                }

                function openPatientWin() {
                    var wnd = window.radopen("AlerePatientIDLookup.aspx?", "RadWindow2");
                    return false;
                }

                function OnPatientClientClose(oWnd, args) {
                    //get the transferred arguments                
                    var arg = args.get_argument();
                    if (arg) {
                        var documentKey = arg.documentKey;
                        var lblOrder = $find("<%= txtPatientID.ClientID %>");
                        lblOrder.set_value(documentKey);
                    }
                }


                function Close() {
                    GetRadOrderWindow().close();
                }

                function CloseEditPage(args) {
                    alert(args);
                    self.close();
                }

                function ErrorPage(args) {
                    alert(args);
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
        //]]>
            </script>
        </telerik:RadCodeBlock>
        <table width="100%" runat="server">
            <tr style="height: 20px;">
                <td rowspan="2" class="previewWrapper" valign="top">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderID" runat="server" Text="Order ID:" Font-Names="Calibri" Font-Size="21px"
                                    ForeColor="#904C23"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOrderID" runat="server" Width="250px" ReadOnly="true"
                                    AutoPostBack="True">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <button id="btnOrderIDLookup" onclick="openOrderWin(); return false;" runat="server">
                                    ...</button>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Status:" Font-Names="Calibri" Font-Size="21px"
                                    ForeColor="#904C23"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtStatus" runat="server" Width="250px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <uc1:Alere_PatientMailing_Summary_CTRL ID="previewControl" runat="server" />
                </td>
            </tr>
            <tr style="height: 600px;">
                <td class="tabWrapper" valign="top">
                    <telerik:RadPanelBar runat="server" ID="RadPanelBar1" ExpandMode="SingleExpandedItem"
                        Width="800px">
                        <Items>
                            <telerik:RadPanelItem Expanded="True" Text="Basic Information" runat="server" Selected="true">
                                <ContentTemplate>
                                    <div id="Div1" class="text" style="background-color: #edf9fe" runat="server">
                                        <table id="Table1" class="tblMain" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPatientID" runat="server" AssociatedControlID="txtPatientID" Text="* Patient ID"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="* First Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="* Last Name"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientID" runat="server" Width="130px" ReadOnly="false"
                                                        AutoPostBack="True" ValidationGroup="BasicInfoValidationGroup">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadButton ID="btnPatientIDLookup" runat="server" OnClientClicked="openPatientWin"
                                                        Text="...">
                                                    </telerik:RadButton>
                                                    <asp:RequiredFieldValidator runat="server" ID="patientValidator" ValidationGroup="BasicInfoValidationGroup"
                                                        ControlToValidate="txtPatientID" ErrorMessage="Patient ID is required" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtFirstName" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="firstNameValidator" ValidationGroup="BasicInfoValidationGroup"
                                                        ControlToValidate="txtFirstName" ErrorMessage="First Name is required" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtLastName" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="lastNameValidator" ValidationGroup="BasicInfoValidationGroup"
                                                        ControlToValidate="txtLastName" ErrorMessage="Last Name is required" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDOB" runat="server" AssociatedControlID="dtDOB" Text="* Date of Birth"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustomerClass" runat="server" AssociatedControlID="txtCustomerClass"
                                                        Text="Customer Class"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLastShipDate" runat="server" AssociatedControlID="dtLastShipDate"
                                                        Text="Last Ship Date"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDOB" runat="server" Calendar-RangeMinDate="1/1/1900 12:00:00 AM"
                                                        Calendar-RangeSelectionStartDate="1/1/1900 12:00:00 AM" Calendar-RangeSelectionMode="None"
                                                        DateInput-MinDate="1/1/1900 12:00:00 AM" FocusedDate="1/1/1900 12:00:00 AM" MinDate="1/1/1900 12:00:00 AM">
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator runat="server" ID="dtDOBValidator" ValidationGroup="BasicInfoValidationGroup"
                                                        ControlToValidate="dtDOB" ErrorMessage="Date Of Birth is required" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtCustomerClass" runat="server" Width="170px" ReadOnly="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtLastShipDate" runat="server" MinDate="1900-01-01" AutoPostBack="true">
                                                        <Calendar ID="Calendar2" RangeMinDate="1900-01-01" runat="server">
                                                        </Calendar>
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMeterType" runat="server" AssociatedControlID="txtMeterType" Text="Meter Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLancetType" runat="server" AssociatedControlID="txtLancetType"
                                                        Text="Lancet Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="dtDueDate" Text="Due Date"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtMeterType" runat="server" Width="170px" ReadOnly="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtLancetType" runat="server" Width="170px" ReadOnly="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDueDate" runat="server" MinDate="1900-01-01" AutoPostBack="true">
                                                        <Calendar ID="Calendar1" RangeMinDate="1900-01-01" runat="server">
                                                        </Calendar>
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="BasicInfoValidationGroup"
                                                        ControlToValidate="dtDueDate" ErrorMessage="Due Date is required" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="txtBasicInfoNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadTextBox Width="100%" ID="txtBasicInfoNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <%--<ul>
                                                        <li>
                                                            <asp:Label ID="lbl1" runat="server" Text="Hello this is(TS Associate) with Alere Home Monitoring - is this (patient name)?"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="lbl2" runat="server" Text="Great, for security purposes can you please verify your date of birth?"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="Label1" runat="server" Text="Thanks! I understand you need to order supplies today, is that correct?"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="Label2" runat="server" Text="(yes) I can help you with that"></asp:Label></li>
                                                    </ul>--%>
                                                    <panel runat="server" id="pnlBasic" width="100%">
                                                    
                                                    </panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <asp:Label ID="Label3" runat="server" Text="CLICK +Next to advance to Patient Details"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="valSum" DisplayMode="BulletList" runat="server" ShowMessageBox="true"
                                                        HeaderText="You must enter a value in the following fields:" Font-Name="verdana"
                                                        Font-Size="12" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <asp:CheckBox ID="chkBasicInfoAllCorrectBasic" runat="server" TextAlign="Right" Text="All information entered is correct" />&nbsp;
                                                    <asp:Button runat="server" ID="nxtButtonBasic" OnClick="NextButtonClick" Text="+ Next"
                                                        CssClass="classname" ValidationGroup="BasicInfoValidationGroup" /><br />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Expanded="False" Text="Patient" runat="server" Selected="false"
                                Enabled="false">
                                <ContentTemplate>
                                    <div class="text" style="background-color: #edf9fe">
                                        <table class="tblMain">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPatientAddressID" runat="server" AssociatedControlID="txtPatientAddressID"
                                                        Text="Address ID"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblPatientAddressLine1" runat="server" AssociatedControlID="txtPatientAddressLine1"
                                                        Text="Address Line 1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientAddressID" runat="server" Width="170px" ReadOnly="false"
                                                        ValidationGroup="BasicInfoValidationGroup">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadTextBox ID="txtPatientAddressLine1" runat="server" Width="400px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPatientCity" runat="server" AssociatedControlID="txtPatientCity"
                                                        Text="City"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientState" runat="server" AssociatedControlID="txtPatientState"
                                                        Text="State"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientZip" runat="server" AssociatedControlID="txtPatientZip"
                                                        Text="Zip"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientCity" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientState" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientZip" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPatientPhone" runat="server" AssociatedControlID="txtPatientPhone"
                                                        Text="Phone" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientEmail" runat="server" AssociatedControlID="txtPatientEmail"
                                                        Text="E-Mail" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                           <tr style="visibility= hidden;">
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientPhone" runat="server" Width="170px" ReadOnly="false" Visible="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientEmail" runat="server" Width="170px" ReadOnly="false" Visible="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label8" runat="server" AssociatedControlID="txtPatientNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3"style="width:500px;">
                                                    <telerik:RadTextBox Width="100%" ID="txtPatientNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            <tr>
                                            <tr>
                                                <td colspan="3" style="width:500px;">
                                                    <br />
                                                    <panel runat="server" id="pnlPatient">
                                                    
                                                    </panel>
                                                    <%--<asp:Label ID="lblPatient1" runat="server" Text="And what is your address?" Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblPatient5" runat="server" Text="(match) - Thank you!"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="lblPatient6" runat="server" Text="(no match) - What is the address where you would like us to send your order?"></asp:Label></li>
                                                    </ul>
                                                    <br />
                                                    <asp:Label ID="lblPatient2" runat="server" Text="And your primary phone number?"
                                                        Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblPatient7" runat="server" Text="(match) - Thank you!" Font-Bold="true"></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="lblPatient8" runat="server" Text="(no match) - (enter on form)" Font-Bold="true"></asp:Label>
                                                        </li>
                                                    </ul>
                                                    <br />
                                                    <asp:Label ID="lblPatient3" runat="server" Text="Can I get your email address?(enter on form)"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <asp:Label ID="lblPatient4" runat="server" Text="CLICK +Next to advance to Order"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="valSumPatient" DisplayMode="BulletList" runat="server"
                                                        ShowMessageBox="true" HeaderText="You must enter a value in the following fields:"
                                                        Font-Name="verdana" Font-Size="12" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnBackPatient" Text="- Back" OnClick="BackButtonClick"
                                                        CssClass="classname" />
                                                </td>
                                                <td align="right" colspan="2">
                                                    <asp:CheckBox ID="chkBasicInfoAllCorrectPatient" runat="server" TextAlign="Right"
                                                        Text="All information entered is correct" />&nbsp;
                                                    <asp:Button runat="server" ID="nxtButtonPatient" OnClick="NextButtonClick" Text="+ Next"
                                                        CssClass="classname" ValidationGroup="BasicInfoValidationGroup" /><br />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Expanded="False" Text="Order" runat="server" Selected="false"
                                Enabled="false">
                                <ContentTemplate>
                                    <div class="text" style="background-color: #edf9fe">
                                        <table class="tblMain">
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadGrid runat="server" ID="grdOrders" DataSourceID="LinqDataSource1" AllowAutomaticUpdates="true"
                                                        AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AutoGenerateColumns="false"
                                                        AllowPaging="true" OnItemUpdated="RadGrid1_ItemUpdated" OnItemInserted="RadGrid1_ItemInserted"
                                                        OnItemDeleted="RadGrid1_ItemDeleted" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound"
                                                        Height="350px" Enabled="true">
                                                        <MasterTableView DataKeyNames="PatientID, OrderID, ItemSKU" CommandItemDisplay="Top"
                                                            InsertItemPageIndexAction="ShowItemOnCurrentPage" AllowPaging="false">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="PatientID" HeaderText="Patient ID" Visible="false"
                                                                    UniqueName="PatientID" Display="False" />
                                                                <telerik:GridBoundColumn DataField="OrderID" HeaderText="Order ID" Visible="false"
                                                                    UniqueName="OrderID" Display="False" />
                                                                <telerik:GridBoundColumn DataField="ItemSKU" HeaderText="Item" UniqueName="ItemSKU"
                                                                    Visible="false" />
                                                                <telerik:GridTemplateColumn UniqueName="cboItemSKU" HeaderText="Item" SortExpression="ItemSKU"
                                                                    ItemStyle-Width="150px">
                                                                    <FooterTemplate>
                                                                        Template footer</FooterTemplate>
                                                                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%#DataBinder.Eval(Container.DataItem, "ItemSKU")%>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <telerik:RadComboBox runat="server" ID="cboItemSKU" EnableLoadOnDemand="True" DataTextField="ItemSKU"
                                                                            OnItemsRequested="cboItemSKU_ItemsRequested" DataValueField="ItemSKU" AutoPostBack="true"
                                                                            HighlightTemplatedItems="true" Height="140px" Width="220px" DropDownWidth="420px"
                                                                            OnSelectedIndexChanged="OnSelectedIndexChangedHandler">
                                                                            <HeaderTemplate>
                                                                                <ul>
                                                                                    <li class="col1">Item</li>
                                                                                </ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# DataBinder.Eval(Container, "Text")%>
                                                                                    </li>
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridNumericColumn DataField="Quantity" HeaderText="Quantity" ColumnEditorID="grdNumericColumnEditor" />
                                                                <telerik:GridEditCommandColumn ButtonType="ImageButton"/>
                                                                <telerik:GridButtonColumn ConfirmText="Delete this item?" ConfirmDialogType="RadWindow"
                                                                    ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" />
                                                            </Columns>
                                                            <EditFormSettings>
                                                                <EditColumn ButtonType="ImageButton" />
                                                                <PopUpSettings Modal="true" />
                                                            </EditFormSettings>
                                                        </MasterTableView>
                                                        <PagerStyle AlwaysVisible="true" />
                                                    </telerik:RadGrid>
                                                    <telerik:GridNumericColumnEditor ID="grdNumericColumnEditor" runat="server">
                                                        <NumericTextBox ID="NumericTextBox1" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0"
                                                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                                                            MinValue="0">
                                                            <NumberFormat GroupSeparator="" />
                                                        </NumericTextBox>
                                                    </telerik:GridNumericColumnEditor>
                                                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="AlerePatientMailingDataDataContext"
                                                        EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="AS_Alere_PMWF_OrderDetails"
                                                        Where="PatientID == @PatientID AND OrderID == @OrderID">
                                                        <WhereParameters>
                                                            <asp:SessionParameter Name="PatientID" SessionField="PatientID" Type="String" />
                                                            <asp:SessionParameter Name="OrderID" SessionField="OrderID" Type="String" />
                                                        </WhereParameters>
                                                    </asp:LinqDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label9" runat="server" AssociatedControlID="txtOrderNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadTextBox Width="100%" ID="txtOrderNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            <tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                    <panel runat="server" id="pnlOrder">
                                                    
                                                    </panel>
                                                    <%--<asp:Label ID="lblOrder1" runat="server" Text="Do you need strips and lancets?" Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblOrder2" runat="server" Text="(select items from pick list and verify with patient)"></asp:Label></li>
                                                    </ul>
                                                    <br />
                                                    <asp:Label ID="lblOrder3" runat="server" Text="Thank you! Your order should arrive in"
                                                        Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblOrder4" runat="server" Text="3-5 days (Medicare primary)" Font-Bold="true"></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="lblOrder5" runat="server" Text="7-10 days (Private insurance)" Font-Bold="true"></asp:Label>
                                                        </li>
                                                    </ul>
                                                    <br />
                                                    <asp:Label ID="lblOrder6" runat="server" Text="Is there anything else i can help you with today?"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                    <asp:Label ID="lblOrder7" runat="server" Text="CLICK +Save to finish later or >Process to complete order"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumOrder" DisplayMode="BulletList" runat="server" ShowMessageBox="true"
                                                        HeaderText="You must enter a value in the following fields:" Font-Name="verdana"
                                                        Font-Size="12" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnBackOrder" Text="- Back" OnClick="BackButtonClick"
                                                        CssClass="classname" />
                                                </td>
                                                <td align="right">
                                                    <asp:Button runat="server" ID="btnSave" OnClick="SaveButtonClick" Text="+ Save" CssClass="classname"
                                                        ValidationGroup="BasicInfoValidationGroup" />
                                                    <asp:Button runat="server" ID="btnProcess" Text="> Process" CssClass="classname"
                                                        ValidationGroup="BasicInfoValidationGroup" OnClick="RegisterButtonClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                        </Items>
                        <CollapseAnimation Duration="100" Type="None" />
                        <ExpandAnimation Duration="100" Type="None" />
                    </telerik:RadPanelBar>
                </td>
            </tr>
        </table>
        <asp:Button runat="server" ID="backButton" Visible="false" CssClass="qsfButton" Text="Back"
            OnClick="BackButtonClick" Style="margin: 10px 0 30px 25px" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
