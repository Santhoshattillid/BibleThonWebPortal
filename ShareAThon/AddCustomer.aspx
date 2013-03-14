<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomer.aspx.cs" Inherits="ShareAThonAddCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Add New Customer</title>
    <link href="Style/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="Form1" runat="server">
    <div class="main">
        <div class="container">
            <div class="errorContainer">
                <asp:Label ID="lblError" CssClass="error errorinfo" runat="server" Text=""></asp:Label></div>
            <div class="accordion">
                <table>
                    <tr>
                        <td class="halftd">
                            <table class="hafdiv">
                                <tr>
                                    <td class="label">
                                        Customer Name
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCustomerName" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Customer Number
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCustomerNumber" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        AddressCode
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAddressCode" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Address
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAddress1" for="LblSAAddress1" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAddress2" for="LblSAAddress2" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAddress3" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        City
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        State
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtState" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Country
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCountry" for="LblSACountry" runat="server" CssClass="required txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="halftd">
                            <table class="hafdiv">
                                <tr>
                                    <td class="label">
                                        Telephone
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelephone" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Fax
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Email
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" for="LblSAEmail" runat="server" CssClass="required email txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        Zip Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="required zipUS txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        CreditCard Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCreditCardName" runat="server" CssClass="required txtarea">
                                            <asp:ListItem>AmericaCharge</asp:ListItem>
                                            <asp:ListItem>Bankcard</asp:ListItem>
                                            <asp:ListItem>Gold Credit</asp:ListItem>
                                            <asp:ListItem>Platinum Credit</asp:ListItem>
                                            <asp:ListItem>Cable</asp:ListItem>
                                            <asp:ListItem>Retail Credit</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        CreditCard Number
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditCardNo" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">
                                        CreditCardExpiry Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExpirationDate" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="buttunback">
                    <tr>
                        <td>
                            <asp:Button ID="btnShipContinue" runat="server" Text="Save" OnClick="btnSaveOrder_Click"
                                CssClass="btn" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="Scripts/AddCustomer.js" type="text/javascript"></script>
    </form>
</body>
</html>
