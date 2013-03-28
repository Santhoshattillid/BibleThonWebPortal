<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAlereHomeMonitoringDocument.aspx.cs"
    Inherits="frmAlereHomeMonitoringDocument" %>

<%@ Register Src="~/UserControls/TransactionSummary.ascx" TagName="ShareathonSummaryCtrl"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alba360 Advanced Workflow - Ins/MD/Demo/Order Document</title>
    <link href="../Styles/styleAlereDocument.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="changeScreenSize(1200,1200)">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager1"
        EnableShadow="true" ShowOnTopWhenMaximized="false">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close, Resize, Move" OnClientClose="OnOrderClientClose"
                NavigateUrl="AlereHomeMonitoringOrderIDLookup.aspx" AutoSize="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close, Resize, Move" OnClientClose="OnPatientClientClose"
                Width="750px" Height="450px" NavigateUrl="AlerePatientIDLookup.aspx" AutoSize="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
        //<![CDATA[

                function changeScreenSize(w, h) {
                    window.resizeTo(w, h)
                }

                function openOrderWin() {
                    var wnd = window.radopen("AlereHomeMonitoringOrderIDLookup.aspx", "RadWindow1");
                    wnd.setSize(400, 400);
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
                    <table>
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
                    <uc1:ShareathonSummaryCtrl ID="previewControl" runat="server"></uc1:ShareathonSummaryCtrl>
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
                                                    <asp:Label ID="Label3" runat="server" Text="CLICK +Next to advance to Insurance"></asp:Label>
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
                            <telerik:RadPanelItem Enabled="False" Text="Insurance" runat="server">
                                <ContentTemplate>
                                    <div class="text" style="background-color: #edf9fe">
                                        <table class="tblMain">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPrimaryPlan" runat="server" AssociatedControlID="txtPrimaryPlan"
                                                        Text="Primary Plan"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrimaryPolicyNumber" runat="server" AssociatedControlID="txtPrimaryPolicyNumber"
                                                        Text="Policy Number"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrimaryGroupNumber" runat="server" AssociatedControlID="txtPrimaryGroupNumber"
                                                        Text="Group Number"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryPlan" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryPolicyNumber" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryGroupNumber" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblPrimaryAddress" runat="server" AssociatedControlID="txtPrimaryAddress"
                                                        Text="Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrimaryPhone" runat="server" AssociatedControlID="txtPrimaryPhone"
                                                        Text="Claim Phone Number"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadTextBox ID="txtPrimaryAddress" runat="server" Width="527px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryPhone" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPrimaryCity" runat="server" AssociatedControlID="txtPrimaryCity"
                                                        Text="City"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrimaryState" runat="server" AssociatedControlID="txtPrimaryState"
                                                        Text="State"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrimaryZip" runat="server" AssociatedControlID="txtPrimaryZip"
                                                        Text="Zip"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryCity" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryState" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPrimaryZip" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label11" runat="server" AssociatedControlID="dtPrimaryEffectivityDate"
                                                        Text="Effective Date"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtPrimaryEffectivityDate" runat="server" DateInput-MinDate="1/1/1900 12:00:00 AM"
                                                        MinDate="1/1/1900 12:00:00 AM" Calendar-RangeMinDate="1/1/1900 12:00:00 AM" Calendar-RangeSelectionStartDate="1/1/1900 12:00:00 AM">
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="Label2" runat="server" Text="Supply Order?"></asp:Label>
                                                    <telerik:RadButton ID="btnSupplyOrderYes" runat="server" ToggleType="Radio" Checked="true"
                                                        GroupName="StandardButton" ButtonType="StandardButton" AutoPostBack="false" Text="Yes">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnSupplyOrderNo" runat="server" ToggleType="Radio" GroupName="StandardButton"
                                                        ButtonType="StandardButton" AutoPostBack="false" Text="No">
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSecondaryPlan" runat="server" AssociatedControlID="txtSecondaryPlan"
                                                        Text="Secondary Plan"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondaryPolicyNumber" runat="server" AssociatedControlID="txtSecondaryPolicyNumber"
                                                        Text="Policy Number"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondaryGroupNumber" runat="server" AssociatedControlID="txtSecondaryGroupNumber"
                                                        Text="Group Number"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryPlan" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryPolicyNumber" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryGroupNumber" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblSecondaryAddress" runat="server" AssociatedControlID="txtSecondaryAddress"
                                                        Text="Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondaryPhone" runat="server" AssociatedControlID="txtSecondaryPhone"
                                                        Text="Claim Phone Number"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadTextBox ID="txtSecondaryAddress" runat="server" Width="527px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryPhone" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSecondaryCity" runat="server" AssociatedControlID="txtSecondaryCity"
                                                        Text="City"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondaryState" runat="server" AssociatedControlID="txtSecondaryState"
                                                        Text="State"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondaryZip" runat="server" AssociatedControlID="txtSecondaryZip"
                                                        Text="Zip"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryCity" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryState" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryZip" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label15" runat="server" AssociatedControlID="dtSecondaryEffectivityDate"
                                                        Text="Effective Date"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadDatePicker ID="dtSecondaryEffectivityDate" runat="server" DateInput-MinDate="1/1/1900 12:00:00 AM"
                                                        Calendar-MonthLayout="Layout_7columns_x_6rows" Calendar-RangeMinDate="1/1/1900 12:00:00 AM"
                                                        Calendar-RangeSelectionStartDate="1/1/1900 12:00:00 AM" MinDate="1/1/1900 12:00:00 AM">
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="txtInsuranceNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadTextBox Width="100%" ID="txtInsuranceNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <panel runat="server" id="pnlInsurance">
                                                    
                                                    </panel>
                                                    <%--<asp:Label ID="lblPrimary123" runat="server" Text="Who is your primary insurance provider?"
                                                        Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblTY1" runat="server" Text="(If match) - Thank you!"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="lblTY2" runat="server" Text="(If no match) - That appears to be different than what we have. Do you have your insurance card handy?"></asp:Label></li>
                                                        <ul>
                                                            <li>
                                                                <asp:Label ID="Label4" runat="server" Text="(Yes) Can you tell me the name of the new insurance company?(enter on form)"></asp:Label>
                                                                <li>
                                                                    <asp:Label ID="Label5" runat="server" Text="And the policy number?(enter on form)"></asp:Label>
                                                                </li>
                                                                <li>
                                                                    <asp:Label ID="Label6" runat="server" Text="Is there a group number?(enter on form)"></asp:Label>
                                                                </li>
                                                                <li>
                                                                    <asp:Label ID="Label8" runat="server" Text="And on the back of the card can you give me the address and phone number?(enter on form)"></asp:Label>
                                                                </li>
                                                            </li>
                                                            <li>
                                                                <asp:Label ID="Label9" runat="server" Text="(No) Is there someone else in your home who has the information?"></asp:Label>
                                                            </li>
                                                        </ul>
                                                    </ul>                                           
                                                    <asp:Label ID="lblSecondary123" runat="server" Text="Do you have secondary coverage?"
                                                        Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="Label12" runat="server" Text="(If match) - Thank you!" Font-Bold="true"></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="Label13" runat="server" Text="(If no match - repeat steps above)"
                                                                Font-Bold="true"></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="Label14" runat="server" Text="(If secondary no longer active update form)"
                                                                Font-Bold="true"></asp:Label>
                                                        </li>
                                                    </ul>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <asp:Label ID="lblAdvance1" runat="server" Text="CLICK +Next to advance to Doctor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="valSum1" DisplayMode="BulletList" runat="server" ShowMessageBox="true"
                                                        HeaderText="You must enter a value in the following fields:" Font-Name="verdana"
                                                        Font-Size="12" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnBackInsurance" OnClick="BackButtonClick" Text="- Back"
                                                        CssClass="classname" /><br />
                                                </td>
                                                <td align="right" colspan="2">
                                                    <asp:CheckBox ID="chkBasicInfoAllCorrectInsurance" runat="server" TextAlign="Right"
                                                        Text="All information entered is correct" />&nbsp;
                                                    <asp:Button runat="server" ID="nxtButtonInsurance" OnClick="NextButtonClick" Text="+ Next"
                                                        CssClass="classname" ValidationGroup="BasicInfoValidationGroup" /><br />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Enabled="False" Text="Doctor" runat="server">
                                <ContentTemplate>
                                    <div class="text" style="background-color: #edf9fe">
                                        <table class="tblMain">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblDoctorName" runat="server" AssociatedControlID="txtDoctorName"
                                                        Text="Doctor's Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDoctorPhone" runat="server" AssociatedControlID="txtDoctorPhone"
                                                        Text="Phone Number"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadTextBox ID="txtDoctorName" runat="server" Width="527px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDoctorPhone" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDoctorCity" runat="server" AssociatedControlID="txtDoctorCity"
                                                        Text="City"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDoctorState" runat="server" AssociatedControlID="txtDoctorState"
                                                        Text="State"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDoctorZip" runat="server" AssociatedControlID="txtDoctorZip" Text="Zip"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDoctorCity" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDoctorState" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDoctorZip" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" AssociatedControlID="txtClinic" Text="Clinic"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" AssociatedControlID="txtClinicCode" Text="Clinic Code"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinic" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinicCode" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label6" runat="server" AssociatedControlID="txtDoctorNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadTextBox Width="100%" ID="txtDoctorNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <panel runat="server" id="pnlDoctor">
                                                    
                                                    </panel>
                                                    <%--<asp:Label ID="Label10" runat="server" Text="Who is the doctor that manages your Coumadin?"
                                                        Font-Bold="true"></asp:Label>
                                                    <ul>
                                                        <li>
                                                            <asp:Label ID="lblDoctor1" runat="server" Text="(If match) - Thank you!"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="lblDoctor2" runat="server" Text="(If not sure) - Do you have your pill bottle handy or do you know your doctor's phone number?"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="lblDoctor3" runat="server" Text="(If change) - Can you spell the doctor's name for me(check bottle if necessary)"></asp:Label>
                                                            <ul>
                                                                <li>
                                                                    <asp:Label ID="lblDoctor4" runat="server" Text="and what is the doctor's phone number"></asp:Label>
                                                                </li>
                                                                <li>
                                                                    <asp:Label ID="lblDoctor5" runat="server" Text="and what State is (s)he in"></asp:Label>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>--%>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <asp:Label ID="lblDoctor6" runat="server" Text="CLICK +Next to advance to Patient"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="valSumDoctor" DisplayMode="BulletList" runat="server"
                                                        ShowMessageBox="true" HeaderText="You must enter a value in the following fields:"
                                                        Font-Name="verdana" Font-Size="12" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnBackDoctor" OnClick="BackButtonClick" Text="Back"
                                                        CssClass="classname" /><br />
                                                </td>
                                                <td align="right" colspan="2">
                                                    <asp:CheckBox ID="chkBasicInfoAllCorrectDoctor" runat="server" TextAlign="Right"
                                                        Text="All information entered is correct" />&nbsp;
                                                    <asp:Button runat="server" ID="nxtButtonDoctor" OnClick="NextButtonClick" Text="+ Next"
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
                                                    <telerik:RadTextBox ID="txtPatientAddressLine1" runat="server" Width="527px" ReadOnly="false">
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
                                                        Text="Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientEmail" runat="server" AssociatedControlID="txtPatientEmail"
                                                        Text="E-Mail"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientPhone" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientEmail" runat="server" Width="170px" ReadOnly="false">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label8" runat="server" AssociatedControlID="txtPatientNote" Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadTextBox Width="100%" ID="txtPatientNote" runat="server" TextMode="MultiLine" Height="50px"
                                                        EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            <tr>
                                            <tr>
                                                <td colspan="3">
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
                                                    <asp:CheckBox ID="chkSuppliesNotNeeded" runat="server" TextAlign="Right" Text="No Supplies Needed"
                                                        OnCheckedChanged="chkSuppliesNotNeeded_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadGrid runat="server" ID="grdOrders" DataSourceID="LinqDataSource1" AllowAutomaticUpdates="true"
                                                        AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AutoGenerateColumns="false"
                                                        AllowPaging="true" OnItemUpdated="RadGrid1_ItemUpdated" OnItemInserted="RadGrid1_ItemInserted"
                                                        OnItemDeleted="RadGrid1_ItemDeleted" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound"
                                                        Height="350px">
                                                        <MasterTableView DataKeyNames="PatientID, OrderID, ItemSKU" CommandItemDisplay="Top"
                                                            InsertItemPageIndexAction="ShowItemOnCurrentPage" AllowPaging="false">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="PatientID" HeaderText="Patient ID" Visible="false"
                                                                    UniqueName="PatientID" Display="False" />
                                                                <telerik:GridBoundColumn DataField="OrderID" HeaderText="Order ID" Visible="false"
                                                                    UniqueName="OrderID" Display="False" />
                                                                <telerik:GridBoundColumn DataField="ItemSKU" HeaderText="Item SKU" UniqueName="ItemSKU"
                                                                    Visible="false" />
                                                                <telerik:GridTemplateColumn UniqueName="cboItemSKU" HeaderText="Item SKU" SortExpression="ItemSKU"
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
                                                                                    <li class="col1">Item SKU</li>
                                                                                    <li class="col2">Description</li>
                                                                                </ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li class="col1">
                                                                                        <%# DataBinder.Eval(Container, "Text")%>
                                                                                    </li>
                                                                                    <li class="col2">
                                                                                        <%# DataBinder.Eval(Container, "Attributes['ITEMDESC']")%></li>
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                    </EditItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="ITEMDESC"
                                                                    ItemStyle-Width="500px" />
                                                                <telerik:GridNumericColumn DataField="UnitPrice" HeaderText="Unit Price" ColumnEditorID="GridNumericColumnEditor1"
                                                                    Visible="false" UniqueName="UnitPrice" />
                                                                <telerik:GridNumericColumn DataField="Quantity" HeaderText="Quantity" ColumnEditorID="grdNumericColumnEditor" />
                                                                <telerik:GridCalculatedColumn HeaderText="Extended Price" UniqueName="ExtendedPrice"
                                                                    DataType="System.Double" DataFields="UnitPrice, Quantity" Expression="{0}*{1}"
                                                                    Visible="false" />
                                                                <%--<telerik:GridNumericColumn DataField="ExtendedPrice" HeaderText="ExtendedPrice" ColumnEditorID="grdCurrencyColumnEditor2" UniqueName="ExtendedPrice" Visible="false"/>--%>
                                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" />
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
                                                    <telerik:GridNumericColumnEditor ID="GridNumericColumnEditor1" runat="server">
                                                        <NumericTextBox ID="NumericTextBox3" runat="server" MaxLength="0" NumberFormat-DecimalDigits="2"
                                                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                                                            MinValue="0" Type="Currency" NumberFormat-GroupSeparator="," NumberFormat-GroupSizes="3">
                                                            <NumberFormat GroupSeparator="" />
                                                        </NumericTextBox>
                                                    </telerik:GridNumericColumnEditor>
                                                    <telerik:GridNumericColumnEditor ID="grdNumericColumnEditor" runat="server">
                                                        <NumericTextBox ID="NumericTextBox1" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0"
                                                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                                                            MinValue="0">
                                                            <NumberFormat GroupSeparator="" />
                                                        </NumericTextBox>
                                                    </telerik:GridNumericColumnEditor>
                                                    <telerik:GridNumericColumnEditor ID="grdCurrencyColumnEditor2" runat="server">
                                                        <NumericTextBox ID="NumericTextBox2" runat="server" MaxLength="0" NumberFormat-DecimalDigits="2"
                                                            EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Value="0"
                                                            MinValue="0" Type="Currency" NumberFormat-GroupSeparator="," NumberFormat-GroupSizes="3">
                                                            <NumberFormat GroupSeparator="" />
                                                        </NumericTextBox>
                                                    </telerik:GridNumericColumnEditor>
                                                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="AlereHomeMonitoringDataDataContext"
                                                        EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="AS_Alere_OrderDetails"
                                                        Where="PatientID == @PatientID AND OrderID == @OrderID">
                                                        <WhereParameters>
                                                            <asp:SessionParameter Name="PatientID" SessionField="PatientID" Type="String" />
                                                            <asp:SessionParameter Name="OrderID" SessionField="OrderID" Type="String" />
                                                        </WhereParameters>
                                                    </asp:LinqDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label7" runat="server" AssociatedControlID="cboShipMethod" Text="Shipping Method:"></asp:Label>&nbsp;
                                                    <telerik:RadComboBox ID="cboShipMethod" runat="server" Width="370px">
                                                    </telerik:RadComboBox>
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
