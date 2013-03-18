<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserProfile.aspx.cs" Inherits="Alba.Workflow.WebPortal.UserProfile" Title="Alba360 Advanced Workflow - Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadTreeView runat="Server" ID="RadTreeView1" EnableViewState="false">
        <Nodes>
            <telerik:RadTreeNode Text="Dashboard" Expanded="true" ImageUrl="~/Images/office_clip.gif"
                PostBack="false">
                <Nodes>
                    <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Text="Workflow Forms"
                        NavigateUrl="WorkflowDocuments.aspx" Expanded="True">
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode Text="Tasks" ImageUrl="~/Images/mailbox.gif" Expanded="True">
                        <Nodes>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Open">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Past Due">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Completed">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Owner="" Text="Started By Me">
                            </telerik:RadTreeNode>
                        </Nodes>
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode runat="server" ImageUrl="~/Images/notes.gif" Text="Profile"
                        NavigateUrl="UserProfile.aspx" Expanded="True" Selected="true">
                    </telerik:RadTreeNode>
                    <telerik:RadTreeNode Text="Alerts &amp; Notifications" ImageUrl="~/Images/notes_icon.gif"
                        NavigateUrl="Notes.aspx" Visible="false">
                    </telerik:RadTreeNode>
                </Nodes>
            </telerik:RadTreeNode>
        </Nodes>
    </telerik:RadTreeView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <style type="text/css">
        td.NewDocumentFirstColumn { padding-left: 10px; padding-right: 20px;}
    </style>
    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">        
    </telerik:RadScriptBlock>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
    <telerik:RadSplitter runat="server" ID="RadSplitter1" Width="100%" BorderSize="0"
        BorderStyle="None" PanesBorderSize="0" Height="100%" Orientation="Horizontal">
        <telerik:RadPane runat="server" ID="RadPane1" Height="32px" EnableViewState="false"
            Scrollable="false">
            <telerik:RadToolBar runat="server" ID="RadToolBar1" CssClass="home-search-toolbar"
                EnableViewState="false" OnButtonClick="RadToolBar1_ButtonClick">
                <Items>
                    <telerik:RadToolBarButton Text="Save" ImageUrl="~/Images/saveas.gif" Enabled="true" />                    
                </Items>
            </telerik:RadToolBar>
        </telerik:RadPane>
        <telerik:RadPane runat="server" ID="RadPane2">
<table>
                        <tr>
                            <td colspan="8"  class="NewDocumentFirstColumn">
                                <b>User Profile</b>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label17" runat="server" Text="User ID"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtUserID" runat="server" Width="370px" ReadOnly="true"
                                    AutoPostBack="True">
                                </telerik:RadTextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Password:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtPassword" runat="server" Width="370px" 
                                    TextMode="Password">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Confirm Password:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtConfirmPassword" runat="server" Width="370px" 
                                    TextMode="Password">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label1" runat="server" Text="ERP User ID:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtERPUserID" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDepartment" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label2" runat="server" Text="First Name:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtFirstName" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Last Name:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label7" runat="server" Text="Company"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtCompany" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Phone:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPhone" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="NewDocumentFirstColumn">
                                <asp:Label ID="Label13" runat="server" Text="Email Address:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtEmailAddress" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="User Role:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtUserRole" runat="server" Width="370px" ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
