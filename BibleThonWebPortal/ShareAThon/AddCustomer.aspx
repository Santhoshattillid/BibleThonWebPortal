<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomer.aspx.cs" Inherits="ShareAThon.ShareAThonAddCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Add New Customer</title>
    <link href="Style/NewCustomer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="Form1" runat="server">
    <div class="main">
        <div class="errorContainer">
            <asp:Label ID="LblErrorInfo" CssClass="error errorinfo" runat="server"></asp:Label></div>
        <div class="accordion">
            <div class="LeftBox">
                <ul>
                    <li>
                        <div class="LeftBox">
                            Customer Name *</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtCustomerName" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Customer Number *</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtCustomerNumber" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Address Code *</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtAddressCode" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Address *</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtAddress1" for="LblSAAddress1" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                        </div>
                        <div class="RightBox empty">
                            <asp:TextBox ID="txtAddress2" for="LblSAAddress2" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                        </div>
                        <div class="RightBox empty">
                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            City</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtCity" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            State</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtState" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Country</div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtCountry" for="LblSACountry" runat="server" CssClass="required txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                </ul>
            </div>
            <div class="RightBox">
                <ul>
                    <li>
                        <div class="LeftBox">
                            Telephone
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtTelephone" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Fax
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtFax" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Email
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtEmail" for="LblSAEmail" runat="server" CssClass="required email txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Zip Code
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtZipCode" runat="server" CssClass="required zipUS txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Credit Card Name</div>
                        <div class="RightBox">
                            <asp:DropDownList ID="drpCreditCardName" runat="server">
                                <asp:ListItem>AmericaCharge</asp:ListItem>
                                <asp:ListItem>Bankcard</asp:ListItem>
                                <asp:ListItem>Gold Credit</asp:ListItem>
                                <asp:ListItem>Platinum Credit</asp:ListItem>
                                <asp:ListItem>Cable</asp:ListItem>
                                <asp:ListItem>Retail Credit</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Credit Card Number
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtCreditCardNo" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li>
                        <div class="LeftBox">
                            Credit Card Expiry Date
                        </div>
                        <div class="RightBox">
                            <asp:TextBox ID="txtExpirationDate" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                </ul>
            </div>
            <div class="clear">
            </div>
            <div class="navigations">
                <asp:Label ID="lblError" CssClass="error" runat="server" Text=""></asp:Label>
                <asp:Button ID="BtnAddCustomer" runat="server" Text="Create Customer" OnClick="BtnSaveOrderClick"
                    CssClass="btn" />
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="Scripts/AddCustomer.js" type="text/javascript"></script>
    <asp:Literal runat="server" ID="HdnScriptSource">
    </asp:Literal>
    </form>
</body>
</html>