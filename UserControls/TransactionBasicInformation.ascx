<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransactionBasicInformation.ascx.cs" Inherits="UserControls_TransactionBasicInformation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 122px;
    }
    .style2
    {
        width: 96px;
    }
    .style3
    {
        width: 75px;
    }
    .style4
    {
        width: 130px;
    }
    .style5
    {
        width: 121px;
    }
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function openWin() {
                var oWnd = radopen("AuthorizationLookup.aspx", "RadWindow1");
            }            
        
        </script>
    </telerik:RadCodeBlock>
<table id="Table1" runat="server" width="100%" style="background-color: #F2F2F2">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblPatientID" AssociatedControlID="txtPatientID" Text="Patient ID:"
                ForeColor="Red" />
        </td>
        <td style="width: 250px;">
            <asp:TextBox ID="txtPatientID" runat="server" Width="150px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="reqFieldValidatorPatientID" ControlToValidate="txtPatientID"
                ErrorMessage="Patient ID is required" Text="*"></asp:RequiredFieldValidator>
            <button id="btnLookup" onclick="openWin(); return false;" runat="server">...</button>
        </td>        
        <td>
            <asp:Label runat="server" ID="lblFirstName" AssociatedControlID="txtTelNo" Text="Telephone:"
                ForeColor="Red" />
        </td>
        <td>
            <asp:TextBox ID="txtTelNo" runat="server" Width="250px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="telephonevalidator" ControlToValidate="txtTelNo"
                ErrorMessage="Telephone is required" Text="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtAddress1" Text="Address:"
                ForeColor="Red" />
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtAddress1" runat="server" Width="250px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="requiredfieldvalidator2" ControlToValidate="txtAddress1"
                ErrorMessage="Address is required" Text="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtAddress2" runat="server" Width="250px"></asp:TextBox>
        </td>
        <td class="style5">
            <asp:Label runat="server" ID="Label2" AssociatedControlID="txtTelNo" Text="E-mail:" />
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtAddress3" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>  
    <tr>
        <td class="style1">
            <asp:Label runat="server" ID="Label5" AssociatedControlID="txtState" Text="State:"
                ForeColor="Red" />
        </td>
        <td class="style2">
            <asp:TextBox ID="txtState" runat="server" Width="75px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="requiredfieldvalidator4" ControlToValidate="txtState"
                ErrorMessage="State is required" Text="*"></asp:RequiredFieldValidator>
        </td>
        <td class="style3">
            <asp:Label runat="server" ID="Label6" AssociatedControlID="txtZip" Text="Zip Code:"
                Width="70px" ForeColor="Red" />
        </td>
        <td class="style4">
            <asp:TextBox ID="txtZip" runat="server" Width="75px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="requiredfieldvalidator5" ControlToValidate="txtZip"
                ErrorMessage="Zip Code is required" Text="*"></asp:RequiredFieldValidator>
        </td>
        <td class="style5">
            <asp:Label runat="server" ID="Label7" AssociatedControlID="txtOtherSource" Text="Other Source:" />
        </td>
        <td>
            <asp:TextBox ID="txtOtherSource" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style1" colspan="6">
            <asp:ValidationSummary runat="server" ID="validationsummary" Font-Names="Calibri" />
        </td>
    </tr>
    <tr>
        <td class="style1" colspan="6">
            <div class="buttonseparator">
                <!-- -->
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="6" align="right">
            <asp:Button runat="server" ID="nextbutton" Text="Next" OnClick="nextButton_Click"
                CssClass="nextButton" />
        </td>
    </tr>
</table>
