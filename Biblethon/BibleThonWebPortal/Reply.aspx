<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reply.aspx.cs" Inherits="Alba.Workflow.WebPortal.Reply" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
	<title>Reply</title>
	<link rel="stylesheet" href="Styles/compose.css" type="text/css" />
</head>
<body class="composeWindow">
	<form id="form1" runat="server">
	<telerik:radscriptmanager id="ScriptManager" runat="server" />
	<telerik:radskinmanager runat="Server" id="SkinManager1" skin="Vista" persistencekey="Skin"
		persistencemode="Session">
	</telerik:radskinmanager>
	<telerik:radeditor id="RadEditor1" width="854px" height="480px" autoresizeheight="false"
		runat="server" imagemanager-viewpaths="~/Img" imagemanager-uploadpaths="~/Img"
		imagemanager-deletepaths="~/Img" imagemanager-maxuploadfilesize="512000" documentmanager-deletepaths="~/Documents"
		flashmanager-deletepaths="~/Flash" mediamanager-deletepaths="~/Flash" templatemanager-deletepaths="~/Documents"
		documentmanager-viewpaths="~/Documents" documentmanager-uploadpaths="~/Documents"
		flashmanager-uploadpaths="~/Flash" mediamanager-uploadpaths="~/Flash" templatemanager-uploadpaths="~/Documents"
		flashmanager-viewpaths="~/Flash" mediamanager-viewpaths="~/Media" enableresize="false"
		editmodes="Design">
		<Modules>  
        <telerik:EditorModule Name="fakeModule" />  
    </Modules> 
	</telerik:radeditor>
	<div id="composeActions">
		<asp:Button ID="Button1" runat="server" Style="width: 75px" Text="Send" OnClientClick="return save()" /><asp:Button
			ID="Button2" runat="server" Style="width: 75px" Text="Cancel" OnClientClick="return cancel()" />
	</div>

	<script type="text/javascript">

		function setEditorContent(content) {
			window.setTimeout(function() {

				var editor = $find("<%= RadEditor1.ClientID%>");
				editor.set_html(content);
			}, 0);
		}

		function save() {
			var radWindow = window.radWindow ? window.radWindow : window.frameElement.radWindow;
			var editor = $find("<%= RadEditor1.ClientID%>");
			radWindow.close(editor.get_html());

			return false;
		}

		function cancel() {
			var radWindow = window.radWindow ? window.radWindow : window.frameElement.radWindow;
			radWindow.close();

			return false;
		}
	</script>

	<telerik:radformdecorator runat="server" id="RadFormDecorator1" />
	</form>
</body>
</html>
