<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="Biblethon_OrderDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <asp:GridView ID="gdvOrders" AutoGenerateColumns="false" runat="server" 
            Width="786px">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="cbAll" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbCheck" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:BoundField DataField=""  />--%>
                <asp:TemplateField HeaderText="Order No.">
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="lnkOrderId" runat="server" Text='<%# Bind("OrderNo") %>'></asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkOrderId" runat="server" Text=''></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="" HeaderText="Date" />
                <asp:BoundField DataField="" HeaderText="Operator" />
                <asp:BoundField DataField="" HeaderText="OrderName" />
                <asp:BoundField DataField="" HeaderText="OrderTotal" />
                <asp:BoundField DataField="" HeaderText="Status" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView1" runat="server" >  
        </asp:GridView>
    </div>
    </form>
</body>
</html>
