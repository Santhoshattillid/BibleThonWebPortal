<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShippingAddress.aspx.cs"
    Inherits="Biblethon.ShippingAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style/AddressPopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
    <telerik:RadScriptManager runat="server">
    </telerik:RadScriptManager>
    <div class="header">
        <h3>
            Select Shipping Address</h3>
    </div>
    <div class="grid">
        <telerik:RadGrid ID="GridShippingAddress" runat="server" OnPageIndexChanged="GridShippingAddressDataBinding"
            PageSize="10" AllowPaging="True" OnPageSizeChanged="GridShippingAddressDataBinding"
            AllowSorting="True" OnSortCommand="GridShippingAddressDataBinding">
        </telerik:RadGrid>
    </div>
    </form>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.CustomerNoLink').live('click', function () {
                var row = $(this).parents('tr:first');
                var customerDetails = {
                    'no': $(this).html(),
                    'address1': row.find('td').eq(1).text(),
                    'address2': row.find('td').eq(2).text(),
                    //'address3': row.find('td').eq(3).text(),
                    'city': row.find('td').eq(3).text(),
                    'state': row.find('td').eq(5).text(),
                    'country': row.find('td').eq(6).text(),
                    'zipCode': row.find('td').eq(7).text(),
                    'telephone1': row.find('td').eq(8).text(),
                    'telephone2': row.find('td').eq(9).text(),
                    'email': $.trim(row.find('td').eq(10).text())
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
    </script>
</body>
</html>