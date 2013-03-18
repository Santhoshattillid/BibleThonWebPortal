<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GaiamAccountLookup.aspx.cs"
    Inherits="GaiamAccountLookup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gaiam Account Lookup</title>
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
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        Skin="Sunset" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function AdjustRadWidow() {

            }

            //fix for Chrome/Safari due to absolute positioned popup not counted as part of the content page layout
            function ChromeSafariFix(oWindow) {
                var iframe = oWindow.get_contentFrame();
                var body = iframe.contentWindow.document.body;

                setTimeout(function () {
                    var height = body.scrollHeight;
                    var width = body.scrollWidth;

                    var iframeBounds = $telerik.getBounds(iframe);
                    var heightDelta = height - iframeBounds.height;
                    var widthDelta = width - iframeBounds.width;

                    if (heightDelta > 0) oWindow.set_height(oWindow.get_height() + heightDelta);
                    if (widthDelta > 0) oWindow.set_width(oWindow.get_width() + widthDelta);
                    oWindow.center();

                }, 310);
            }

            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();

                var keyValues;

                var grid = $find("<%=RadGrid1.ClientID %>");
                var MasterTable = grid.get_masterTableView();
                var selectedRows = MasterTable.get_selectedItems();
                for (var i = 0; i < selectedRows.length; i++) {
                    var row = selectedRows[i];
                    var cell = MasterTable.getCellByColumnUniqueName(row, "CUSTNMBR")
                    //here cell.innerHTML holds the value of the cell
                    keyValues = cell.innerHTML;
                }

                oArg.documentKey = keyValues;

                //get a reference to the current RadWindow
                var oWnd = GetRadWindow();

                //Close the RadWindow and send the argument to the parent page
                if (oArg.documentKey) {
                    oWnd.close(oArg);
                }
                else {
                    alert("Please choose a record");
                }
            }

            function RowSelected(sender, args) {

            }


        </script>
    </telerik:RadCodeBlock>
    <div style="height: 193px;">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" GridLines="None" Width="100%" AllowPaging="true" AllowFilteringByColumn="true"  OnColumnCreating="RadGrid1_ColumnCreating"
                        OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource">
                        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="CUSTNMBR">                                                                                    
                        </MasterTableView>
                        <ClientSettings>
                            <Selecting AllowRowSelect="true" />
                            <ClientEvents OnRowSelected="RowSelected" />
                        </ClientSettings>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>                    
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCustomerNumber" runat="server" Width="300px" Visible="true"></asp:Label>
                    <button title="Submit" id="close" onclick="returnToParent(); return false;">
                        Select</button>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
