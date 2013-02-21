<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillingAddress.aspx.cs" Inherits="Biblethon_BillingAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadGrid ID="RadGrid1" runat="server" OnPageIndexChanged="RadGrid1_DataBinding"
        PageSize="10" AllowPaging="True" OnPageSizeChanged="RadGrid1_DataBinding" AllowSorting="True"
        OnSortCommand="RadGrid1_DataBinding">
    </telerik:RadGrid>
    </form>
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.CustomerNoLink').live('click', function () {
                var row = $(this).parents('tr:first');
                var customerDetails = {
                    'no': $(this).html(),
                    'name': row.find('td').eq(1).html(),
                    'address1':    row.find('td').eq(2).html(),
                    'address2': row.find('td').eq(3).html(),
                    'address3': row.find('td').eq(4).html(),
                    'city': row.find('td').eq(5).html(),
                    'state' : row.find('td').eq(7).html(),
                    'country':row.find('td').eq(8).html(),
                    'zipCode' : row.find('td').eq(9).html(),
                    'telephone1' : row.find('td').eq(10).html(),
                    'telephone2' : row.find('td').eq(11).html(),
                    'email' : $.trim(row.find('td').eq(12).text())
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
