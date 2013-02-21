<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="Biblethon_OrderDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadGrid ID="RadGrid1" runat="server" OnPageIndexChanged="RadGrid1_DataBinding" PageSize="20" AllowPaging="True" OnPageSizeChanged="RadGrid1_DataBinding"  ></telerik:RadGrid>
    </form>
</body>
</html>
