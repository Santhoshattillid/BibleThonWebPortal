<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Alere_Home_Monitoring_Script_Template.aspx.cs"
    Inherits="Alere_Home_Monitoring_Script_Template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alere Home Monitoring Dynamic Scripting</title>    
</head>
<body onload="AdjustRadWidow();">
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function closeWindow() {                

                self.close();        
            }            

        </script>
    </telerik:RadCodeBlock>
    <form id="Form2" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            

        </script>
    </telerik:RadCodeBlock>
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
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lbl1" Text="Link Identifier:" Font-Names="Segoe UI,Arial,sans-serif" Font-Size="12px"></asp:Label>
                </td>
                <td>                
                    <telerik:RadComboBox ID="CboLinkIdentifier" Runat="server" 
                        DataSourceID="SqlDataSource1" DataTextField="LinkIdentifier" 
                        DataValueField="LinkIdentifier" Width="358px" 
                        onselectedindexchanged="CboLinkIdentifier_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </telerik:RadComboBox>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:TWOConnectionString %>" 
                        SelectCommand="SELECT [LinkIdentifier], [LinkHTML] FROM [AS_Alere_DynamicScript]">
                    </asp:SqlDataSource>                
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadEditor ID="ScriptEditor" runat="server" SkinID="DefaultSetOfTools">
                        <Modules>
                            <telerik:EditorModule Name="RadEditorStatistics" Visible="false" Enabled="true" />
                            <telerik:EditorModule Name="RadEditorDomInspector" Visible="false" Enabled="true" />
                            <telerik:EditorModule Name="RadEditorNodeInspector" Visible="false" Enabled="true" />
                            <telerik:EditorModule Name="RadEditorHtmlInspector" Visible="false" Enabled="true" />
                        </Modules>
                        
                    </telerik:RadEditor>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadButton ID="BtnSave" runat="server" Text="Save" 
                        style="top: 0px; left: 0px" Width="100px" onclick="BtnSave_Click" 
                        UseSubmitBehavior="False">
                    </telerik:RadButton>
                    <telerik:RadButton ID="BtnExit" runat="server" Text="Exit" 
                        style="top: 0px; left: 0px" Width="100px" OnClientClicked="closeWindow">
                    </telerik:RadButton>
                    <asp:Label ID="Label1" runat="server"></asp:Label> 
                </td>                
            </tr>
        </table>
    </fieldset>
    </form>
</body>
</html>
