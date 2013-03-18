<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPresidioApprovalDocument.aspx.cs"
    Inherits="frmPresidioApprovalDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alba360 Advanced Workflow - New Presidio Approval Document</title>
    <style type="text/css">
        td.NewDocumentFirstColumn
        {
            padding-left: 10px;
        }
    </style>
</head>
<body onunload="refreshParent();" onload="changeScreenSize(1050,1100)">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
        //<![CDATA[


            function changeScreenSize(w, h) {
                var oWindow = GetRadWindow();                
            }            
            
            
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function Close() {
                GetRadWindow().close();
            }

            function refreshParent() {

            }

            function pageLoad() {

            }

            function alertCallBackFn(arg) {
                window.close();
            }

            function CloseEditPage(args) {
                alert(args);
                self.close();
            }

            
            function ShowWindow() {
                
            }

            function windowLoad(oWindow, args) {
                oWindow.set_title("Import Data");
            }

            function returnToParent(args) {
                //get a reference to the current RadWindow
                alert(args);
                var oWnd = GetRadWindow();
                oWnd.close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) {
                    oWindow = window.radWindow;
                }
                else if (window.frameElement.radWindow) {
                    oWindow = window.frameElement.radWindow;
                }
                return oWindow;
            }

            function CustomRadWindowConfirm(sender, args) {
                if (args.get_item().get_text() == "Delete") {
                    //radconfirm('<h3 style=\'color: #333399;\'>Are you sure?</h3>', confirmCallBackFn, 330, 100, null, 'RadConfirm custom title');                    
                    //Cancel the postback
                    args.set_cancel(!confirm('Are you sure you want to delete this document?'));
                }
            }

            function confirmCallBackFn(arg) {
                radalert("<strong>radconfirm</strong> returned the following result: <h3 style='color: #ff0000;'>" + arg + "</h3>", null, null, "Result");
            }            

        //]]>
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadPanelBar runat="server" ID="RadPanelBar1" Width="1100px" Height="100%">
        <Items>
            <telerik:RadPanelItem runat="server" ID="RadPane1" Expanded="true">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadToolBar runat="server" ID="RadToolBar1" CssClass="home-search-toolbar" OnClientButtonClicking="CustomRadWindowConfirm"
                                    EnableViewState="false" OnButtonClick="RadToolBar1_ButtonClick">
                                    <Items>
                                        <telerik:RadToolBarButton Text="Submit" ImageUrl="~/Images/delegate.gif" Enabled="true"/>                                        
                                    </Items>
                                </telerik:RadToolBar>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblUser" runat="server" Text="Logged-In User: " Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadPanelItem>
            <telerik:RadPanelItem runat="server" ID="RadPane3" Expanded="true" Text="Header">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="8" class="NewDocumentFirstColumn">
                                <b>Presidio Approval Document</b>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label17" runat="server" Text="Workflow:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtWorkflow" runat="server" Width="300px" ReadOnly="true" Text="PresidioWestITDocument"
                                    AutoPostBack="True">
                                </telerik:RadTextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Record ID:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtRecordID" runat="server" Width="170px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>                            
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Approval Type:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtApprovalType" runat="server" Width="300px" ReadOnly="false">
                                </telerik:RadTextBox>                                
                            </td>   
                        </tr>        
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label3" runat="server" Text="Area/Territory:"></asp:Label>
                            </td>
                            <td colspan="5">
                                 <telerik:RadTextBox ID="txtArea" runat="server" Width="300px" ReadOnly="false"
                                                        AutoPostBack="false">
                                                    </telerik:RadTextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Requestor:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtRequestor" runat="server" Width="300px" ReadOnly="false">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label4" runat="server" Text="Amount:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadNumericTextBox ID="txtAmount" runat="server" Width="300px" Type="Currency"
                                    Value="0"  NumberFormat-DecimalDigits="2" NumberFormat-AllowRounding="false"
                                    EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" Visible="true">
                                </telerik:RadNumericTextBox>                             
                            </td>                         
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Last Approval Level:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtLevel" runat="server" Width="300px" ReadOnly="true">
                                </telerik:RadTextBox>                                
                            </td>   
                        </tr>                       
                    </table>
                </ContentTemplate>
            </telerik:RadPanelItem>
        </Items>
    </telerik:RadPanelBar>    
    </form>   
</body>
</html>
