<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAlereVerbalAuthExistingPatientDocument.aspx.cs"
    Inherits="frmAlereVerbalAuthExistingPatientDocument" %>

<%@ Register Src="~/UserControls/Alere_VerbalAuthExistingPatient_Summary.ascx" TagName="AlereVerbalAuthExistingPatientSummaryCtrl"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alba360 Advanced Workflow - Verbal Auth Existing Patient Document</title>
    <link href="../Styles/styleAlereDocument.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="changeScreenSize(1200,1200)">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager1"
        EnableShadow="true" ShowOnTopWhenMaximized="false">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close, Resize, Move"
                OnClientClose="OnOrderClientClose" NavigateUrl="AlereHomeMonitoringOrderIDLookup.aspx"
                AutoSize="true">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close, Resize, Move"
                OnClientClose="OnPatientClientClose" Width="750px" Height="450px" NavigateUrl="AlerePatientIDLookup.aspx"
                AutoSize="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Width="1200px">
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
                        var lblOrder = $find("<%= txtRecordID.ClientID %>");
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
            <tr>
                <td rowspan="2" class="previewWrapper" valign="top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblRecordID" runat="server" Text="Record ID:" Font-Names="Segoe UI"
                                    Font-Size="14px" ForeColor="#904C23"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRecordID" runat="server" Width="150px" ReadOnly="true"
                                    AutoPostBack="True">
                                </telerik:RadTextBox>
                            </td>
                            <%-- <td>
                                <button id="btnRecordIDLookup" onclick="openOrderWin(); return false;" runat="server">
                                    ...</button>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Status:" Font-Names="Segoe UI" Font-Size="14px"
                                    ForeColor="#904C23"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtStatus" runat="server" Width="150px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <uc1:AlereVerbalAuthExistingPatientSummaryCtrl ID="previewControl" runat="server" />
                </td>
            </tr>
            <tr style="height: 800px;">
                <td class="tabWrapper" valign="top" style="margin-left: 80px">
                    <table width="800px">
                        <tr>
                            <td>
                                <asp:Label ID="lblPatientID" runat="server" AssociatedControlID="txtPatientID" Text="* Patient ID"
                                    Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="* First Name"
                                    Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="* Last Name"
                                    Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDOB" runat="server" AssociatedControlID="txtLastName" Text="* DOB"
                                    Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
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
                                <telerik:RadTextBox ID="txtFirstName" runat="server" Width="170px" ReadOnly="True">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="firstNameValidator" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtFirstName" ErrorMessage="First Name is required" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" Width="170px" ReadOnly="True">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator runat="server" ID="lastNameValidator" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="txtLastName" ErrorMessage="Last Name is required" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtDOB" runat="server" Calendar-RangeMinDate="1/1/1900 12:00:00 AM"
                                    Calendar-RangeSelectionStartDate="1/1/1900 12:00:00 AM" Calendar-RangeSelectionMode="None"
                                    DateInput-MinDate="1/1/1900 12:00:00 AM" FocusedDate="1900-01-01" MinDate="1900-01-01"
                                    Enabled="False" Culture="en-US">
                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                    </Calendar>
                                    <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width="160px">
                                    </DateInput>
                                    <DatePopupButton CssClass="rcCalPopup rcDisabled" HoverImageUrl="" ImageUrl="" Visible="False" />
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator runat="server" ID="dtDOBValidator" ValidationGroup="BasicInfoValidationGroup"
                                    ControlToValidate="dtDOB" ErrorMessage="Date Of Birth is required" Text="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblDoctorName" runat="server" AssociatedControlID="txtDoctorName"
                                    Text="Doctor's Name" Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDoctorPhone" runat="server" AssociatedControlID="txtDoctorPhone"
                                    Text="Phone Number" Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtDoctorName" runat="server" Width="527px" ReadOnly="True">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDoctorPhone" runat="server" Width="170px" ReadOnly="True">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblClinicHeader" runat="server" AssociatedControlID="txtClinicHeader"
                                    Text="Clinic" Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblClinicCodeHeader" runat="server" AssociatedControlID="txtClinicCodeHeader"
                                    Text="Clinic Code" Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDueDate" runat="server" AssociatedControlID="dtDueDate" Text="Due Date"
                                    Font-Names="Segoe UI" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtClinicHeader" runat="server" Width="170px" ReadOnly="True">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtClinicCodeHeader" runat="server" Width="170px" ReadOnly="True">
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
                    </table>
                    <telerik:RadPanelBar runat="server" ID="RadPanelBar1" ExpandMode="SingleExpandedItem"
                        Width="100%">
                        <Items>
                            <telerik:RadPanelItem Expanded="True" Text="Basic Information" runat="server" Selected="true">
                                <ContentTemplate>
                                    <div id="Div1" class="text" style="background-color: #edf9fe" runat="server">
                                        <table id="tblBasicInformation" class="tblMain" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblGender" runat="server" AssociatedControlID="chkGender" Text="Gender"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkGender" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCaregiver" runat="server" AssociatedControlID="chkCareGiver" Text="Caregiver"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCareGiver" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPrimaryPhone" runat="server" AssociatedControlID="chkPrimaryPhone"
                                                        Text="Primary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPrimaryPhone" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCaregiverRelationship" runat="server" AssociatedControlID="chkCaregiverRelationship"
                                                        Text="Relationship"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCaregiverRelationship" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSecondaryPhone" runat="server" AssociatedControlID="chkSecondaryPhone"
                                                        Text="Secondary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkSecondaryPhone" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCaregiverPrimaryPhone" runat="server" AssociatedControlID="chkCaregiverPrimaryPhone"
                                                        Text="Caregiver Primary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCaregiverPrimaryPhone" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="chkEmail" Text="Email"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkEmail" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCaregiverSecondaryPhone" runat="server" AssociatedControlID="chkCaregiverSecondaryPhone"
                                                        Text="Caregiver Secondary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCaregiverSecondaryPhone" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPOA" runat="server" AssociatedControlID="chkCaregiverPOA" Text="POA"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCaregiverPOA" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReferringPhysician" runat="server" AssociatedControlID="chkPhysician"
                                                        Text="Referring Physician"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPhysician" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClinic" runat="server" AssociatedControlID="chkClinic" Text="Clinic"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkClinic" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPhysicianPhone" runat="server" AssociatedControlID="chkPhysicianPhone"
                                                        Text="Physician Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPhysicianPhone" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClinicAddress" runat="server" AssociatedControlID="chkClinicAddress"
                                                        Text="Clinic Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkClinicAddress" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPhysicianFax" runat="server" AssociatedControlID="chkPhysicianFax"
                                                        Text="Physician Fax"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPhysicianFax" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClinicCityStateZip" runat="server" AssociatedControlID="chkClinicCityStateZip"
                                                        Text="Clinic City, State, Zip"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkClinicCityStateZip" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReportResultsTo" runat="server" AssociatedControlID="chkReportResultsTo"
                                                        Text="Report Results To"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkReportResultsTo" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClinicPhone" runat="server" AssociatedControlID="chkClinicPhone"
                                                        Text="Clinic Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkClinicPhone" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReportResultsToPHone" runat="server" AssociatedControlID="chkReportsResultsToPhone"
                                                        Text="Report Results To Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkReportsResultsToPhone" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReportResultsToFax" runat="server" AssociatedControlID="chkReportResultsToFax"
                                                        Text="Report Results To Fax"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkReportResultsToFax" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAfterHoursInstruction" runat="server" AssociatedControlID="chkAfterHoursInstruction"
                                                        Text="After Hours Instruction"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAfterHoursInstruction" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMeterType" runat="server" AssociatedControlID="chkMeterType" Text="Meter Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkMeterType" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTestReportingInstructions" runat="server" AssociatedControlID="chkTestReportingInstructions"
                                                        Text="Test Reporting Instructions"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkTestReportingInstructions" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTestFrequency" runat="server" AssociatedControlID="chkTestFrequency"
                                                        Text="Test Frequency"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkTestFrequency" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFaxAllResults" runat="server" AssociatedControlID="chkFaxAllResults"
                                                        Text="Fax All Results"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkFaxAllResults" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAPLSDiscussion" runat="server" AssociatedControlID="chkAPLSDiscussion"
                                                        Text="APLS Discussion"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAPLSDiscussion" runat="server" TextAlign="Left" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMBE" runat="server" AssociatedControlID="chkMBE" Text="MBE"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkMBE" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblChkMBEDate" runat="server" AssociatedControlID="chkMBEDate" Text="MBE Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkMBEDate" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSelectOptions" runat="server" AssociatedControlID="chkSelectOptions"
                                                        Text="Select Options"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkSelectOptions" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRetestIn" runat="server" AssociatedControlID="chkRetestIn" Text="Retest In"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRetestIn" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOtherOptions" runat="server" AssociatedControlID="chkOtherOptions"
                                                        Text="Other Options"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkOtherOptions" runat="server" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblBasicInfoNote" runat="server" AssociatedControlID="txtBasicInfoNote"
                                                        Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <telerik:RadTextBox Width="100%" ID="txtBasicInfoNote" runat="server" TextMode="MultiLine"
                                                        Height="50px" EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <asp:Button runat="server" ID="btnProcess" Text="> Process" CssClass="classname"
                                                        ValidationGroup="BasicInfoValidationGroup" OnClick="RegisterButtonClick" />
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
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Expanded="false" Text="Details Data" runat="server" Selected="false"
                                Enabled="false">
                                <ContentTemplate>
                                    <div class="text" style="background-color: #edf9fe">
                                        <table id="Table2" class="tblMain" runat="server" width="100%">
                                            <tr>
                                                <td style="width: 20%;">
                                                    <asp:Label ID="lblDetailsGender" runat="server" Text="Gender"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadButton ID="btnMale" runat="server" ToggleType="Radio" Checked="true"
                                                        GroupName="StandardButton" ButtonType="StandardButton" AutoPostBack="false" Text="Male"
                                                        Enabled="true">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnFemale" runat="server" ToggleType="Radio" GroupName="StandardButton"
                                                        ButtonType="StandardButton" AutoPostBack="false" Text="Female" Enabled="true">
                                                    </telerik:RadButton>
                                                </td>
                                                <td style="width: 25%;">
                                                    <asp:Label ID="lblDetailsCaregiver" runat="server" AssociatedControlID="txtCaregiver"
                                                        Text="Caregiver"></asp:Label>
                                                </td>
                                                <td style="width: 30%;">
                                                    <telerik:RadTextBox ID="txtCaregiver" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCaregiver" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtCaregiver" ErrorMessage="Caregiver is required" Text="*"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsPrimaryPhone" runat="server" AssociatedControlID="txtPrimaryPhone"
                                                        Text="Primary Phone"></asp:Label>
                                                </td>
                                                <td id="tdPrimaryPhone">
                                                    <telerik:RadTextBox ID="txtPrimaryPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPrimaryPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtPrimaryPhone" ErrorMessage="Primary Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsCaregiverRelationship" runat="server" AssociatedControlID="txtCaregiverRelationship"
                                                        Text="Relationship"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtCaregiverRelationship" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCaregiverRelationship" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtCaregiverRelationship" ErrorMessage="Caregiver Relationship is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsSecondaryPhone" runat="server" AssociatedControlID="txtSecondaryPhone"
                                                        Text="Secondary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtSecondaryPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSecondaryPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtSecondaryPhone" ErrorMessage="Secondary Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsCaregiverPrimaryPhone" runat="server" AssociatedControlID="txtCaregiverPrimaryPhone"
                                                        Text="Caregiver Primary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtCaregiverPrimaryPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCaregiverPrimaryPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtCaregiverPrimaryPhone" ErrorMessage="Caregiver Primary Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsEmail" runat="server" AssociatedControlID="txtEmail" Text="Email"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtEmail" runat="server" Width="170px" ReadOnly="false" Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtEmail" ErrorMessage="Email is required" Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsCaregiverSecondaryPhone" runat="server" AssociatedControlID="txtCaregiverSecondaryPhone"
                                                        Text="Caregiver Secondary Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtCaregiverSecondaryPhone" runat="server" Width="170px"
                                                        ReadOnly="false" Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCaregiverSecondaryPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtCaregiverSecondaryPhone" ErrorMessage="Caregiver Secondary Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsPOA" runat="server" Text="POA"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnPOAYes" runat="server" ToggleType="Radio" Checked="true"
                                                        GroupName="StandardButtonPOA" ButtonType="StandardButton" AutoPostBack="false"
                                                        Text="Yes" Enabled="true">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnPOANo" runat="server" ToggleType="Radio" GroupName="StandardButtonPOA"
                                                        ButtonType="StandardButton" AutoPostBack="false" Text="No" Enabled="true">
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsReferringPhysician" runat="server" AssociatedControlID="txtReferringPhysician"
                                                        Text="Referring Physician"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtReferringPhysician" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvReferringPhysician" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtReferringPhysician" ErrorMessage="Referring Physician is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsClinic" runat="server" AssociatedControlID="txtClinic" Text="Clinic"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinic" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvClinic" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtClinic" ErrorMessage="Clinic is required" Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsPhysicianPhone" runat="server" AssociatedControlID="txtPhysicianPhone"
                                                        Text="Physician Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPhysicianPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPhysicianPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtPhysicianPhone" ErrorMessage="Physician Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsClinicAddress" runat="server" AssociatedControlID="txtClinicAddress"
                                                        Text="Clinic Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinicAddress" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvClinicAddress" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtClinicAddress" ErrorMessage="Clinic Address is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsPhysicianFax" runat="server" AssociatedControlID="txtPhysicianFax"
                                                        Text="Physician Fax"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPhysicianFax" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPhysicianFax" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtPhysicianFax" ErrorMessage="Physician Fax is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsClinicCityStateZip" runat="server" AssociatedControlID="txtClinicCityStateZip"
                                                        Text="Clinic City, State, Zip"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinicCityStateZip" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvClinicCityStateZip" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtClinicCityStateZip" ErrorMessage="Clinic CityStateZip is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsReportResultsTo" runat="server" AssociatedControlID="txtReportResultsTo"
                                                        Text="Report Results To"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtReportResultsTo" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvReportResultsTo" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtReportResultsTo" ErrorMessage="Report Results To is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbldetailsClinicPhone" runat="server" AssociatedControlID="txtClinicPhone"
                                                        Text="Clinic Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtClinicPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvClinicPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtClinicPhone" ErrorMessage="Clinic Phone is required" Text="*"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsReportResultsToPhone" runat="server" AssociatedControlID="txtReportResultsToPhone"
                                                        Text="Report Results To Phone"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtReportResultsToPhone" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvReportResultsToPhone" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtReportResultsToPhone" ErrorMessage="Report Results To Phone is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsReportResultsToFax" runat="server" AssociatedControlID="txtReportResultsToFax"
                                                        Text="Report Results To Fax"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtReportResultsToFax" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvReportResultsToFax" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtReportResultsToFax" ErrorMessage="Report Results To Fax is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsAfterHoursInstruction" runat="server" AssociatedControlID="txtAfterHoursInstruction"
                                                        Text="After Hours Instruction"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtAfterHoursInstruction" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAfterHoursInstruction" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtAfterHoursInstruction" ErrorMessage="After Hours Instruction is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsMeterType" runat="server" AssociatedControlID="cboMeterType"
                                                        Text="Meter Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboMeterType" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMeterType" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboMeterType" ErrorMessage="Meter Type is required" Text="*"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsTestReportingInstructions" runat="server" AssociatedControlID="cboTestReportingInstructions"
                                                        Text="Test Reporting Instructions"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboTestReportingInstructions" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTestReportingInstructions" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboTestReportingInstructions" ErrorMessage="Test Reporting Instructions is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsTestFrequency" runat="server" AssociatedControlID="cboTestFrequency"
                                                        Text="Test Frequency"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboTestFrequency" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTestFrequency" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboTestFrequency" ErrorMessage="Test Frequency is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsFaxAllResults" runat="server" Text="Fax All Results"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadButton ID="btnFaxAllYes" runat="server" ToggleType="Radio" Checked="true"
                                                        GroupName="StandardButtonFaxAll" ButtonType="StandardButton" AutoPostBack="false"
                                                        Text="Yes" Enabled="true">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnFaxAllNo" runat="server" ToggleType="Radio" GroupName="StandardButtonFaxAll"
                                                        ButtonType="StandardButton" AutoPostBack="false" Text="No" Enabled="true">
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsAPLSDiscussion" runat="server" AssociatedControlID="cboAPLSDiscussion"
                                                        Text="APLS Discussion"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboAPLSDiscussion" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAPLSDiscussion" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboAPLSDiscussion" ErrorMessage="APLS Discussion is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsMBE" runat="server" Text="MBE"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadButton ID="btnMBEYes" runat="server" ToggleType="Radio" Checked="true"
                                                        GroupName="StandardButtonMBE" ButtonType="StandardButton" AutoPostBack="false"
                                                        Text="Yes" Enabled="true">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnMBENo" runat="server" ToggleType="Radio" GroupName="StandardButtonMBE"
                                                        ButtonType="StandardButton" AutoPostBack="false" Text="No" Enabled="true">
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsMBEDate" runat="server" AssociatedControlID="dtMBEDate"
                                                        Text="MBE Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtMBEDate" runat="server" MinDate="1900-01-01" AutoPostBack="true"
                                                        Enabled="true">
                                                        <Calendar ID="Calendar2" RangeMinDate="1900-01-01" runat="server">
                                                        </Calendar>
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMBEDate" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="dtMBEDate" ErrorMessage="MBE Date is required" Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsSelectOption" runat="server" AssociatedControlID="cboSelectOptions"
                                                        Text="Select Options"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboSelectOptions" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSelectOptions" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboSelectOptions" ErrorMessage="Select Options is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsRetestIn" runat="server" AssociatedControlID="txtRetestIn"
                                                        Text="Retest In"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="txtRetestIn" runat="server" Width="300px" Type="Number"
                                                        Value="0" MinValue="0" NumberFormat-DecimalDigits="0" NumberFormat-AllowRounding="True"
                                                        EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Enabled="true">
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvRetestIn" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtRetestIn" ErrorMessage="Retest In is required" Text="*"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDetailsOtherOptions" runat="server" AssociatedControlID="txtOtherOptions"
                                                        Text="Other Options"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtOtherOptions" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvOtherOptions" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtOtherOptions" ErrorMessage="Other Options is required"
                                                        Text="*" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsAuthorizedBy" runat="server" AssociatedControlID="txtAuthorizedBy"
                                                        Text="Authorized By"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtAuthorizedBy" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtAuthorizedBy" ErrorMessage="Authorized By is required"
                                                        Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="cboAttempt"
                                                        Text="Attempt:"></asp:Label>
                                                </td>
                                                <td>
                                                     <telerik:RadComboBox ID="cboAttempt" runat="server" Enabled="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="cboAttempt" ErrorMessage="Attempt is required" Text="*"
                                                        Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsAuthorizedByTitle" runat="server" AssociatedControlID="txtAuthorizedByTitle"
                                                        Text="Title"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtAuthorizedByTitle" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAuthorizedByTitle" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtAuthorizedByTitle" ErrorMessage="Authorized By Title is required"
                                                        Text="*" Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="dtAttemptDate"
                                                        Text="Last Attempt Date:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtAttemptDate" runat="server" MinDate="1900-01-01" AutoPostBack="true"
                                                        Enabled="true">
                                                        <Calendar ID="Calendar4" RangeMinDate="1900-01-01" runat="server">
                                                        </Calendar>
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="dtAttemptDate" ErrorMessage="Attempt Date is required" Text="*"
                                                        Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsTakenBy" runat="server" AssociatedControlID="txtTakenBy"
                                                        Text="Taken By"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtTakenBy" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTakenBy" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtTakenBy" ErrorMessage="Taken By is required" Text="*" Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="dtAttemptTime"
                                                        Text="Last Attempt Time:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTimePicker ID="dtAttemptTime" runat="server" AutoPostBack="true" Enabled="true">
                                                    </telerik:RadTimePicker>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="dtAttemptTime" ErrorMessage="Attempt Time is required" Text="*"
                                                        Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsTakenByTitle" runat="server" AssociatedControlID="txtTakenByTitle"
                                                        Text="Title"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtTakenByTitle" runat="server" Width="170px" ReadOnly="false"
                                                        Enabled="true">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="txtTakenByTitle" ErrorMessage="Taken By Title is required"
                                                        Text="*" Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td rowspan="3" colspan="3" valign="top">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" BorderColor="Red" BorderWidth="1px"
                                                        HeaderText="List of errors:" ValidationGroup="DetailsValidationGroup" Width="100%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsDate" runat="server" AssociatedControlID="dtDate" Text="Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDate" runat="server" MinDate="1900-01-01" AutoPostBack="true"
                                                        Enabled="true">
                                                        <Calendar ID="Calendar3" RangeMinDate="1900-01-01" runat="server">
                                                        </Calendar>
                                                        <DateInput DateFormat="MM/dd/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="dtDate" ErrorMessage="Date is required" Text="*" Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="lblDetailsTime" runat="server" AssociatedControlID="dtTime" Text="Time"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTimePicker ID="dtTime" runat="server" AutoPostBack="true" Enabled="true">
                                                    </telerik:RadTimePicker>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTime" ValidationGroup="DetailsValidationGroup"
                                                        ControlToValidate="dtTime" ErrorMessage="Time is required" Text="*" Enabled="true"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblDetailsBasicInfoNote" runat="server" AssociatedControlID="txtDetailsNote"
                                                        Text="Notes"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <telerik:RadTextBox Width="100%" ID="txtDetailsNote" runat="server" TextMode="MultiLine"
                                                        Height="50px" EmptyMessage="Enter Note Here">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="right">
                                                    <asp:Button runat="server" ID="btnSaveDetails" Text="Save" CssClass="classname" ValidationGroup="DetailsValidationGroup"
                                                        OnClick="SaveDetailsClick" />
                                                        <asp:Button runat="server" ID="btnSubmitDetails" Text="> Process" CssClass="classname"
                                                        ValidationGroup="DetailsValidationGroup" OnClick="SubmitDetailsClick" Visible="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
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
                                                    <panel runat="server" id="pnlDetails" width="100%">
                                                    
                                                    </panel>
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
        <asp:CheckBox ID="chkAuthorizedBy" runat="server" TextAlign="Left" Checked="true"
            Visible="false" />
        <asp:CheckBox ID="chkAuthorizedByTitle" runat="server" TextAlign="Left" Checked="true"
            Visible="false" />
        <asp:CheckBox ID="chkTakenBy" runat="server" TextAlign="Left" Checked="true" Visible="false" />
        <asp:CheckBox ID="chkTakenByTitle" runat="server" TextAlign="Left" Checked="true"
            Visible="false" />
        <asp:CheckBox ID="chkDate" runat="server" TextAlign="Left" Checked="true" Visible="false" />
        <asp:CheckBox ID="chkTime" runat="server" TextAlign="Left" Checked="true" Visible="false" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
