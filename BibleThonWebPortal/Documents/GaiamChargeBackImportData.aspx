<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GaiamChargeBackImportData.aspx.cs" Inherits="GaiamChargeBackImportData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import Data</title>
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
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" Skin="Sunset" EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        Skin="Sunset" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function confirmCallBackFn(arg) {
                if (arg == true) {
                    var ajaxManager = $find("<%= pnl1.ClientID %>");
                    ajaxManager.ajaxRequest("client");
                }
                else
                    return false;
            }

            function ErrorMsg(arg) {
                alert(arg);
                return false;
            }

            function returnToParent(arg) {
                //get a reference to the current RadWindow
                var oArg = new Object();

                oArg.documentTotal = 100;

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function OnClientClicked(sender, eventArgs) {
                var upload = $find("<%= RadUpload1.ClientID %>");
                var inputs = upload.getUploadedFiles();
                if (inputs.length > 0) {
                    alert(inputs[0] + " will be processed. Please refrain from navigating away from this page until processing has completed.");
                    myShow();
                }
                else {
                    alert("Please upload a file first before processing.");
                }
            }         

            function myShow() {
                currentLoadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID%>");
                currentUpdatedControl = "<%= Panel1.ClientID %>";
                //show the loading panel over the updated control 
                currentLoadingPanel.show(currentUpdatedControl);
            }

            function MyClientShowing(sender, args) {
                args.get_loadingElement().style.border = "2px solid black";
                args.set_cancelNativeDisplay(true);
                $telerik.$(args.get_loadingElement()).show("slow");
            }

            function MyClientHiding(sender, args) {
                args.get_loadingElement().style.border = "2px solid blue";
                args.set_cancelNativeDisplay(true);
                $telerik.$(args.get_loadingElement()).hide("slow");
            }

        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Height="275px">
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Choose file to upload:"></asp:Label>
            </td>
            <td>
                <telerik:RadAsyncUpload ID="RadUpload1" runat="server" MaxFileInputsCount="1" InputSize="30" Width="350px">
                </telerik:RadAsyncUpload>                
            </td>
            <td align="left">                
                <telerik:RadButton ID="RadButton1" runat="server" Text="Upload" 
                    onclick="RadButton1_Click" OnClientClicked="OnClientClicked">
                </telerik:RadButton>
            </td>
        </tr>
        <tr>
            <td colspan="3">                
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" 
                    Transparency="10" BackColor="#CCCCCC" BackImageUrl="~/Images/loading.gif" 
                    style="position:absolute;" BackgroundPosition="None" 
                    onclienthiding="MyClientHiding" onclientshowing="MyClientShowing">                    
                </telerik:RadAjaxLoadingPanel>
            </td>
        </tr>
        <tr>
            <td colspan="3">                
                <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" Width="100%" AllowPaging="true"
                    OnNeedDataSource="RadGrid1_NeedDataSource" Height="100%" 
                    OnItemCreated="RadGrid1_ItemCreated" AllowSorting="True">
                    <MasterTableView AutoGenerateColumns="false">
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="GAIAMItemNumber" HeaderText="Item Number" Visible="true"
                                UniqueName="ItemNumber" HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UPCCode" HeaderText="UPC" Visible="true" UniqueName="UPC"
                                HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TitlePromotion" HeaderText="Title" Visible="true" UniqueName="Title"
                                HeaderStyle-Width="150px">
                                <HeaderStyle Width="150px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Studio" HeaderText="Studio" Visible="true" UniqueName="Studio"
                                HeaderStyle-Width="150px">
                                <HeaderStyle Width="150px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="Quantity" HeaderText="Quantity" Visible="true"
                                UniqueName="Quantity" HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridNumericColumn>
                            <telerik:GridNumericColumn DataField="AmountPerUnit" HeaderText="Amount Per Unit"
                                Visible="true" UniqueName="AmountPerUnit" HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridNumericColumn>
                            <telerik:GridNumericColumn DataField="TotalAmount" HeaderText="Total" Visible="true" UniqueName="Total"
                                HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridNumericColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle AlwaysVisible="True" />
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnRowSelected="RowSelected" />
                    </ClientSettings>
                    <PagerStyle AlwaysVisible="True" VerticalAlign="Bottom" />
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblFilePathName" runat="server" Width="300px" Visible="false"></asp:Label>
                <asp:Label ID="lblFilePathExtension" runat="server" Width="300px" Visible="false"></asp:Label>
                <asp:Label ID="lblAuthorizationNumber" runat="server" Width="300px" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>            
            <td colspan="3" align="right">                
                <telerik:RadAjaxPanel ID="pnl1" runat="server" OnAjaxRequest="pnl1_AjaxRequest" LoadingPanelID="RadAjaxLoadingPanel1">
                    <telerik:RadButton ID="btnConfirm" runat="server" Text="Process" Width="150px" 
                        onclick="btnConfirm_Click">
                    </telerik:RadButton>                    
                </telerik:RadAjaxPanel>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </form>
</body>
</html>
