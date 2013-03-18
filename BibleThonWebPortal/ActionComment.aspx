<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionComment.aspx.cs" Inherits="ActionComment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comments Window</title>
    <style type="text/css">
        html, body, form
        {
            padding: 0;
            margin: 0;
            height: 100%;
            background: #f2f2de;
        }
        
        body
        {
            font: normal 11px Arial, Verdana, Sans-serif;
        }
        
        fieldset
        {
            height: 150px;
        }
        
        * + html fieldset
        {
            height: 154px;
            width: 268px;
        }
    </style>
</head>
<body onload="AdjustRadWidow();">
    <form id="Form2" method="post" runat="server">
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        Skin="Sunset" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function returnToParent(arg) {
                //get a reference to the current RadWindow
                var oWnd = GetRadWindow();
                oWnd.close(arg);
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function AdjustRadWidow() {
                var oWindow = GetRadWindow();
                setTimeout(function () { oWindow.autoSize(true); if ($telerik.isChrome || $telerik.isSafari) ChromeSafariFix(oWindow); }, 500);
            }

        </script>
    </telerik:RadCodeBlock>
    <table width="100%">
        <tr>
            <td style="width: 30px;">
                <asp:Label ID="Label1" runat="server" Text="Comment:"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox Width="300px" ID="txtComment" runat="server" TextMode="MultiLine"
                    Height="50px" EmptyMessage="Enter Comment Here">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblWFInstanceID" runat="server" Width="300px" Visible="false"></asp:Label>
                 <asp:Label ID="lblAction" runat="server" Width="300px" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnConfirm" Width="150" runat="server" OnCommand="Btn_OnCommand"
                    Text="Process" CommandArgument="radconfirm" /><br style="clear: both" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
