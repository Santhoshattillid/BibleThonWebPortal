<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillingAddress.aspx.cs" Inherits="Share_a_Thon_BillingAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Style/AddressPopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <asp:Panel runat="server" ID="RadPanelCustomerIds">
        <div class="header">
            <h3>
                Select Customer</h3>
        </div>
        <div class="grid">
            <telerik:RadGrid ID="RadGridCustomerIds" runat="server" OnPageIndexChanged="RadGrid1_DataBinding"
                PageSize="10" AllowPaging="True" OnPageSizeChanged="RadGrid1_DataBinding" AllowSorting="True"
                OnSortCommand="RadGrid1_DataBinding" CssClass="radGrid">
            </telerik:RadGrid>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="RadPanelAddress">
        <div class="header">
            <div>
                <h3>
                    Select Billing Address</h3>
            </div>
            <div class="right">
                <a href="#" id="BtnBack">Back</a></div>
        </div>
        <div class="clear">
        </div>
        <div class="grid">
            <telerik:RadGrid ID="RadGridAddress" runat="server">
            </telerik:RadGrid>
        </div>
        <asp:HiddenField runat="server" ID="HdnCreditCardNumber" />
        <asp:HiddenField runat="server" ID="HdnExpireDate" />
    </asp:Panel>
    </form>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/contentScript.js" type="text/javascript"></script>
</body>
</html>