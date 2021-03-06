﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillingAddress.aspx.cs" Inherits="Share_a_Thon_BillingAddress" %>

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
                <a href="#" onclick="goBack()">Back</a></div>
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
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.CustomerNoLink').live('click', function () {
                var row = $(this).parents('tr:first');
                var customerDetails = {
                    'no': $(this).html(),
                    'name': row.find('td').eq(1).text(),
                    'address1': row.find('td').eq(2).text(),
                    'address2': row.find('td').eq(3).text(),
                    //'address3': row.find('td').eq(4).text(),
                    'city': row.find('td').eq(4).text(),
                    'state': row.find('td').eq(5).text(),
                    'country': row.find('td').eq(6).text(),
                    'zipCode': row.find('td').eq(7).text(),
                    'telephone1': row.find('td').eq(8).text(),
                    //'telephone2': row.find('td').eq(11).text(),
                    'email': $.trim(row.find('td').eq(9).text()),
                    'cardno': $.trim($('[id$=HdnCreditCardNumber]').val()),
                    'expireDate': $.trim($('[id$=HdnExpireDate]').val()),
                };
                getRadWindow().close(customerDetails);
            });
            function getRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
        });
        function goBack() {
                window.history.back();
            }
    </script>
</body>
</html>
