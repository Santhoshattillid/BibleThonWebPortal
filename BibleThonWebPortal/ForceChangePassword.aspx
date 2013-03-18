<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForceChangePassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" href="Styles/styles.css" />
    <script type="text/javascript">
                        //<![CDATA[
        var imgUrl = null;
        function alertCallBackFn(arg) {
            radalert("<strong>radalert</strong> returned the following result: <h3 style='color: #ff0000;'>" + arg + "</h3>", null, null, "Result");
        }

        function pageLoad() {
            //attach a handler to readion buttons to update global variable holding image url
            var $ = $telerik.$;
            $('input:radio').bind('click', function () {
                imgUrl = $(this).val();
            });
        }

                        //]]>                                                                        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" EnableCdn="true" ID="RadScriptManager1">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
    <div>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/alba_logo.jpg" Width="235px" />
        <ul class="login-form">
            <li>
                <h3 style="text-align:left;">
                    Alba Advanced Workflow Solution New Password</h3>
            </li>
            <li>
                <table>
                    <tr>
                        <td align="left">
                            <label style="width:150px; text-align:left;">
                                New Password:</label>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtNewPassword" Width="160px" TextMode="Password"/>
                        </td>
                    </tr>         
                     <tr>
                        <td align="left">
                            <label style="width:150px; text-align:left;">
                                Confirm New Password:</label>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtConfirmNewPassword" Width="160px" TextMode="Password"/>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="2" align="right">
                            <telerik:RadAjaxPanel ID="pnl1" runat="server">
                                <telerik:RadButton ID="RadButton1" runat="server" Text="Update Password" OnClick="Button1_Click">
                                </telerik:RadButton>
                            </telerik:RadAjaxPanel>
                        </td>
                    </tr>
                </table>
            </li>
            <li class="button-item"></li>
        </ul>
    </div>
    </form>
</body>
</html>
