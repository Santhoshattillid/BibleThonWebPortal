<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GaiamChargeBackAuthorizationLookup.aspx.cs"
    Inherits="GaiamChargeBackAuthorizationLookup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gaiam ChargeBack Authorization Lookup</title>
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
                var oWindow = GetRadWindow();
                setTimeout(function () { oWindow.autoSize(true); if ($telerik.isChrome || $telerik.isSafari) ChromeSafariFix(oWindow); }, 500);
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

                var grid = $find("<%=RadGrid1.ClientID %>");
                var MasterTable = grid.get_masterTableView();
                var selectedRows = MasterTable.get_selectedItems();
                for (var i = 0; i < selectedRows.length; i++) {
                    var row = selectedRows[i];
                    var cell = MasterTable.getCellByColumnUniqueName(row, "AuthorizationNumber")
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
                
//                document.getElementById("<%= lblAuthorizationNumber.ClientID %>").innerHTML = args.getDataKeyValue("AuthorizationNumber");
            }

        </script>
    </telerik:RadCodeBlock>
    <div style="width: 600px; height: 193px;">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <fieldset id="fld1">
            <div style="margin: 20px 0 0 0;">
                <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" DataSourceID="LinqDataSource1"  AllowPaging="true"
                    GridLines="None">
                    <MasterTableView AutoGenerateColumns="False" DataSourceID="LinqDataSource1" ClientDataKeyNames="AuthorizationNumber">
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AuthorizationNumber" FilterControlAltText="Filter AuthorizationNumber column"
                                HeaderText="Authorization Number" ReadOnly="True" SortExpression="AuthorizationNumber"
                                UniqueName="AuthorizationNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AccountName" FilterControlAltText="Filter AccountName column"
                                HeaderText="Account Name" ReadOnly="True" SortExpression="AccountName" UniqueName="AccountName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="GAIAMAccountNumber" FilterControlAltText="Filter GAIAMAccountNumber column"
                                HeaderText="Gaiam Account Number" ReadOnly="True" SortExpression="GAIAMAccountNumber"
                                UniqueName="GAIAMAccountNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateStartEvent" DataType="System.DateTime" FilterControlAltText="Filter DateStartEvent column"
                                HeaderText="Date Start Event" ReadOnly="True" SortExpression="DateStartEvent"
                                UniqueName="DateStartEvent" DataFormatString="{0:MM/dd/yy}">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnRowSelected="RowSelected" />
                    </ClientSettings>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
                <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="ChargebackDataDataContext"
                    EntityTypeName="" Select="new (AuthorizationNumber, AccountName, GAIAMAccountNumber, DateStartEvent)"
                    TableName="AS_GAIAM_Chargeback_Headers" Where='WorkflowState = "Saved" || WorkflowState = "Revision"'>
                </asp:LinqDataSource>
            </div>
        </fieldset>
        <div style="margin-top: 4px; text-align: right;">
        <asp:Label ID="lblAuthorizationNumber" runat="server" Width="300px" Visible="false"></asp:Label>
            <button title="Submit" id="close" onclick="returnToParent(); return false;">
                Select</button>

        </div>
    </div>
    </form>
</body>
</html>
