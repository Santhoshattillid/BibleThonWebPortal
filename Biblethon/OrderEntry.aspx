<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderEntry.aspx.cs" Inherits="Biblethon_OrderEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Order Entry</title>
    <link href="Style/Style.css" rel="stylesheet" type="text/css" />
    <link href="Style/jquery-ui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divMain">
        <asp:Label ID="lblError" Style="color: Red; margin-left: 170px; visibility: hidden;"
            runat="server" Text=""></asp:Label></div>
    <div style="float: left; width: 170px">
        <div style="float: left; width: 170px">
            <p>
                Order Summary<br />
                Order No.
                <asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label></p>
        </div>
        <div style="float: left; width: 170px">
            <p style="font-style: italic">
                Billing Address
            </p>
            <div id="divBilling">
            </div>
        </div>
        <div style="float: left; width: 170px">
            <p style="font-style: italic">
                Shipping Address
            </p>
            <div id="divShipping">
            </div>
        </div>
        <div style="float: left; width: 170px">
            <p style="font-style: italic">
                Offer Lines
            </p>
            <div id="divOfferLines">
            </div>
        </div>
        <div style="float: left; width: 170px">
            <p style="font-style: italic">
                CC Payment Information
            </p>
            <div id="divPaymentInfo">
            </div>
        </div>
    </div>
    <div id="accordion" style="float: left; width: 750px;">
        <a id="0" href="" onclick="AccordianHid(this)">
            <h3>
                Billing Address</h3>
        </a>
        <div class="divAccordian">
            <table style="width: 700px;">
                <tr>
                    <td valign="top">
                        <table style="width: 350px;">
                            <tr>
                                <td style="width: 70px; color: Red;">
                                    Name
                                </td>
                                <td colspan="3" style="width: 280px;">
                                    <asp:TextBox ID="txtCustName" Width="180px" runat="server"></asp:TextBox>
                                    <asp:ImageButton ID="imgSearch" runat="server" OnClientClick="return IsValidateFor()"
                                        OnClick="imgSearch_Click" /><asp:Button ID="btnSearchBy" Style="visibility: hidden"
                                            Width="0px" runat="server" Text="" />
                                    <%-- </td>
                                <td style="width: 150px;">--%>
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td style="width: 70px; color: Red;">
                                    Address
                                </td>
                                <td colspan="3" style="width: 280px;">
                                    <asp:Label ID="lblAddress1" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td rowspan="3" valign="bottom" style="width: 70px; color: Red;">
                                    City
                                </td>
                                <td colspan="3" style="width: 280px;">
                                    <asp:Label ID="lblAddress2" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr3" runat="server">
                                <td colspan="3" style="width: 280px;">
                                    <asp:Label ID="lblAddress3" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr4" runat="server">
                                <td colspan="3" style="width: 280px;">
                                    <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr5" runat="server">
                                <td style="width: 70px; color: Red;">
                                    State
                                </td>
                                <td style="width: 60px;">
                                    <asp:Label ID="lblState" Width="60px" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 70px; color: Red;">
                                    Zip Code
                                </td>
                                <td style="width: 150px;">
                                    <asp:Label ID="lblZipCode" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr6" runat="server">
                                <td style="width: 70px; color: Red;">
                                    Country
                                </td>
                                <td colspan="3" style="width: 280px;">
                                    <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
                                    <%--<asp:DropDownList ID="ddlBCoutry" runat="server" Width="225px">
                                    </asp:DropDownList>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table style="width: 350px;">
                            <tr>
                                <td style="width: 70px; color: Red;">
                                    Telephone
                                </td>
                                <td style="width: 240px;">
                                    <asp:TextBox ID="txtPhone" Width="240px" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="tr7" runat="server">
                                <td style="width: 70px;">
                                    Email
                                </td>
                                <td style="width: 240px;">
                                    <asp:TextBox ID="txtBEmail" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 310px;">
                                    <asp:CheckBox ID="cbMention" Text="Please do not mention name" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 310px;">
                                    <asp:CheckBox ID="cbCaller" Text="First time caller" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 310px;">
                                    <asp:CheckBox ID="cbShipping" Text="Use same information for shipping address" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnBillingBack" runat="server" Text="<< Back" />
                        <asp:Button ID="btnBillContinue" runat="server" Text="Continue" OnClientClick="return getBillingAddress()"
                            OnClick="btnBillContinue_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <a id="1" href="" onclick="AccordianHid(this)">
            <h3>
                Shipping Address</h3>
        </a>
        <div class="divAccordian">
            <table style="width: 700px;">
                <tr>
                    <td valign="top">
                        <table style="width: 350px;">
                            <tr>
                                <td style="color: Red;">
                                    Name
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCustomerName" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: Red;">
                                    Address
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAddress1" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="3" valign="bottom" style="color: Red;">
                                    City
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAddress2" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAddress3" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCity" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: Red;">
                                    State
                                </td>
                                <td>
                                    <asp:TextBox ID="txtState" runat="server" Width="75px"></asp:TextBox>
                                </td>
                                <td style="color: Red;">
                                    Zip Code
                                </td>
                                <td>
                                    <asp:TextBox ID="txtZipCode" runat="server" Width="55px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: Red;">
                                    Country
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCountry" runat="server" Width="130px"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlCountry" runat="server">
                                    </asp:DropDownList>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table style="width: 350px;">
                            <tr>
                                <td style="color: Red;">
                                    Telephone
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelephone" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnshippingBack" OnClientClick="GetBack(this)" runat="server" Text="<< Back" /><asp:Button
                            ID="btnShipContinue" runat="server" OnClientClick="return getShippingAddress()"
                            Text="Continue" OnClick="btnShipContinue_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <a id="2" href="" onclick="AccordianHid(this)">
            <h3>
                Offer Lines</h3>
        </a>
        <div class="divAccordian">
            <%--<table>
                    <tr>
                        <td>
                            <asp:GridView ID="gdvOfferLine" AutoGenerateColumns="false" CssClass="gridId" runat="server"
                                AllowPaging="true" PageSize="4" AlternatingRowStyle-BackColor="#ecebe7">
                                <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                <PagerStyle BackColor="#F0FFFF" />
                                <HeaderStyle BackColor="#A9A9A9" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Offer ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="OfferId" runat="server" Text='<%# Bind("OfferId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>--%>
            <table style="width: 700px;">
                <tr>
                    <td>
                        <asp:GridView ID="gdvOfferLine" Width="700px" AutoGenerateColumns="false" CssClass="gridId"
                            runat="server" PageSize="10" HeaderStyle-BackColor="" AlternatingRowStyle-BackColor="#ecebe7">
                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                            <PagerStyle BackColor="#F0FFFF" />
                            <HeaderStyle BackColor="#A9A9A9" />
                            <Columns>
                                <asp:TemplateField HeaderText="Offer ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="OfferId" runat="server" Text='<%# Bind("OfferId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="Description">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TXTQty" runat="server" Text="0" Style="text-align: right" onkeyup="CalculateTotals();"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>Total Qty:</b>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price","{0:c}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:TextBox ID="LBLSubTotal" Style="text-align: right; border-width: 0px; border-collapse: collapse;
                                            background-color: #edf8f8;" runat="server" ForeColor="Green" Text="$0.00"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <table style="width: 700px;">
                <tr>
                    <td align="right">
                        SubTotal
                    </td>
                    <td align="right" style="width: 150px">
                        <%--<asp:TextBox ID="LBLTotal1" runat="server" ForeColor="Green" Font-Bold="true" style="margin-left: 0px;text-align:right;" ></asp:TextBox>--%>
                        <asp:TextBox ID="lblTotal" runat="server" Text="" Style="text-align: right; border-width: 0px;
                            border-collapse: collapse; background-color: #edf8f8;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Shipping
                    </td>
                    <td align="right" style="width: 150px">
                        $<asp:TextBox ID="txtShipping" runat="server" onblur="calculatGranTotal()" Style="margin-left: 0px;
                            text-align: right;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        AdditionalDonation
                    </td>
                    <td align="right" style="width: 150px">
                        $<asp:TextBox ID="txtADonation" runat="server" onblur="calculatGranTotal()" Style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        GroundTotal
                    </td>
                    <td align="right" style="width: 150px">
                        <asp:TextBox ID="lblGrandTotal" runat="server" Style="text-align: right; border-width: 0px;
                            border-collapse: collapse; background-color: #edf8f8;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Button ID="btnBack3" OnClientClick="GetBack(this)" runat="server" Text="<< Back" />&nbsp;&nbsp;<asp:Button
                            ID="btnConfirmOffer" OnClientClick="return GetOfferLines()" runat="server" Text="Continue"
                            OnClick="btnConfirmOffer_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <a id="3" href="" onclick="AccordianHid(this)">
            <h3>
                Credit Card Payment Information</h3>
        </a>
        <div class="divAccordian">
            <table style="width: 700px;">
                <tr>
                    <td style="width: 150px; color: Red;">
                        CreditCardType
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="ddlCreditCardType" runat="server" Style="width: 160px;">
                            <asp:ListItem>Visa</asp:ListItem>
                            <asp:ListItem>MasterCard</asp:ListItem>
                            <asp:ListItem>Discover</asp:ListItem>
                            <asp:ListItem>AmEx</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 150px; color: Red;">
                        ExpirationDate
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtExpirationDate" runat="server" />
                    </td>
                </tr>
                <tr style="height: 45px; color: Red;">
                    <td>
                        CreditCardNo.
                    </td>
                    <td>
                        <asp:TextBox ID="txtCreditCardNo" runat="server" Width="152px" />
                    </td>
                    <td style="color: Red;">
                        CVN
                    </td>
                    <td>
                        <asp:TextBox ID="txtCVN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="btnBack4" runat="server" OnClientClick="GetBack(this)" Text="<< Back" /><asp:Button
                            ID="btncontinue4" runat="server" Text="Continue" OnClick="btncontinue4_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <a id="4" href="" onclick="AccordianHid(this)">
            <h3>
                Total, Confirmation and Order Submission
            </h3>
        </a>
        <div class="divAccordian">
            <table style="width: 700px;">
                <tr>
                    <td style="width: 150px;">
                        Order No.
                    </td>
                    <td style="width: 200px;">
                        <asp:Label ID="lblOrderNum" Width="200px" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 150px;">
                        Order Total
                    </td>
                    <td style="width: 200px;">
                        <asp:Label ID="lblOrderTotal" Width="200px" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr style="height: 45px">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="btnBack5" runat="server" OnClientClick="GetBack(this)" Text="<< Back" />&nbsp;&nbsp;<asp:Button
                            ID="btnSaveOrder" runat="server" Text="Save" Width="63px" OnClick="btnSaveOrder_Click" />&nbsp;&nbsp;<asp:Button
                                ID="btnProcessOrder" runat="server" Text="Process" Width="74px" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%-- <div id="popUpShow" style="display: none">
        <div id="dialog-confirm" title="Empty the recycle bin?" style="width: 900px; height: 600px">--%>
    <cc1:ModalPopupExtender ID="MPEGridview" runat="server" PopupControlID="pnlPopup"
        TargetControlID="btnSearchBy" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        <table style="margin: 0px 0px 10px 10px; width: 680px; border: 1px solid #b6bb86">
            <tr style="background-color: #b6bb86;">
                <td style="float: right;">
                    <asp:Button ID="btnClose" runat="server" Text="Close" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 650px;">
                        <tr>
                            <td style="width: 150px; text-align: left">
                                Find
                            </td>
                            <td style="width: 400px; text-align: left" colspan="2">
                                <input type="text" id="txtName" name="txtName" />
                            </td>
                            <td style="width: 100px;">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="115px" OnClientClick="return IsValidate()"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px; text-align: left">
                                Refine Search By
                            </td>
                            <td style="width: 200px; text-align: left">
                                <select id="ddlOption" name="ddlOption">
                                    <option value="1">Address</option>
                                    <option value="2">Phone Number</option>
                                </select>
                            </td>
                            <td style="width: 200px; text-align: left; float: left">
                                <input type="text" id="txtAddAndPh" name="txtAddAndPh" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="gvCustomers" class="gridId" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="true" PageSize="10" HeaderStyle-BackColor="" AlternatingRowStyle-BackColor="#ecebe7"
                                    OnPageIndexChanging="gvCustomers_PageIndexChanging">
                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                    <PagerStyle BackColor="#F0FFFF" />
                                    <HeaderStyle BackColor="#A9A9A9" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblCustomerName" runat="server" CssClass="lnkButton" Text='<%# Bind("CustomerName") %>'
                                                    CommandName="Select" OnClientClick="return GetSelectedRow(this)"></asp:LinkButton>
                                                <asp:TextBox ID="customerId" runat="server" Enabled="false" CssClass="txtID" Text='<%# Bind("CustomerNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Telephone1" HeaderText="Phone Number">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Address1" HeaderText="Address">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="City" HeaderText="City">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Zipcode" HeaderText="Postal Code">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="float: right; padding-right: 10px;">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="108px" />
                                <asp:Button ID="btnSelect" runat="server" Text="Select" Width="108px" OnClick="btnSelect_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <input id="hidAccordionIndex" runat="server" type="hidden" value="0" />
    <input id="hidCustId" runat="server" type="hidden" value="0" />
    <input id="hidAddressCode" runat="server" type="hidden" value="" />
    
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/contentScript.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

            $("#accordion").accordion({
                event: "click",
                autoHeight: false,
                active: activeIndex
            });
            if ($('#MainContent_lblZipCode').text() != '') {
                getBillingAddress();
            }
            if ($('#MainContent_txtZipCode').val() != '') {
                getShippingAddress();
            }
            CalculateTotals();
            if ($('#MainContent_lblGrandTotal').val() != '0') {
                //debugger;
                GetOfferLines();
            }
        });

        function AccordianHid(val1) {
            //debugger;
            var i = parseInt(val1.id);
            $("#<%=hidAccordionIndex.ClientID %>").val(i);
        }
        function popModel() {
            $("#popUpShow").css('display', 'block');
            $("#dialog-confirm").dialog({
                resizable: true,
                height: 600,
                width: 800,
                modal: true,
                buttons: {
                    "Delete all items": function () {
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $("#popUpShow").css('display', 'none');
                        $(this).dialog("close");
                    }
                }
            });
        }

        function GetSelectedRow(lnk) {
            // debugger;
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var id = document.getElementById('MainContent_gvCustomers_customerId_' + rowIndex).value;
            $("#<%=hidCustId.ClientID %>").val(id);
            //alert($("#<%=hidCustId.ClientID %>").val())
            return false;
        }
        function IsValidate() {
            var findByName = document.getElementById('txtName').value;
            var findByAddr = document.getElementById('txtAddAndPh').value;
            if (findByName == '') {
                alert("Find is Required");
                return false;
            }
            else if (findByAddr == '') {
                alert("Refine Search By is Required");
                return false;
            }
            else {
                return true;
            }
        }

        function IsValidateFor() {
            var name = $('#MainContent_txtCustName').val();
            var phone = $('#MainContent_txtPhone').val();
            var lblError = '<p style="margin-left:170px">';
            if (name == '') {
                lblError += '* Name is Required<br/>';
            }
            if (phone == '') {
                lblError += '* Telephone Number is Required';
            }
            if (name != '' && phone != '') {
                $('#MainContent_lblError').css('visibility', 'hidden');
                return true;
            }
            else {
                lblError += '</p>';
                $('#MainContent_lblError').html(lblError).css({ visibility: 'visible', 'margin-left': '170px' });
                return false;
            }
        }

        function GetBack(anchor) {
            //debugger;
            var i = parseInt($("#<%=hidAccordionIndex.ClientID %>").val()) - 1;
            $("#<%=hidAccordionIndex.ClientID %>").val(i);
        }

        function getBillingAddress() {
            //debugger;
            var name = $('#MainContent_txtCustName').val();
            var add1 = $('#MainContent_lblAddress1').text();
            var add2 = $('#MainContent_lblAddress2').text();
            var add3 = $('#MainContent_lblAddress3').text();
            var city = $('#MainContent_lblCity').text();
            var state = $('#MainContent_lblState').text();
            var zip = $('#MainContent_lblZipCode').text();
            var country = $('#MainContent_lblCountry').text();
            var phone = $('#MainContent_txtPhone').val();
            var email = $('#MainContent_txtBEmail').val();
            $("#divBilling").html('<p>' + name + '<br/>' + add1 + '<br/>' + city + ',' + state + ',' + zip + '<br/>' + country + '<br/>T:' + phone + '<br/>E:' + email + '</p>');
            if (name != '' && add1 != '' && phone != '' && city != '' && state != '' && zip != '' && country != '')
                return true;
            else
                return false;
        }

        function getShippingAddress() {
            //debugger;
            var isZipCodeValid = /^[0-9]{5}(?:-[0-9]{4})?$/.test($('#MainContent_txtZipCode').val());
            var isphoneNumbervalid = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/.test($('#MainContent_txtTelephone').val());
            var lblError = '<p style="margin-left:170px">';
            var name = $('#MainContent_txtCustomerName').val();
            var add1 = $('#MainContent_txtAddress1').val();
            var add2 = $('#MainContent_txtAddress2').val();
            var add3 = $('#MainContent_txtAddress3').val();
            var city = $('#MainContent_txtCity').val();
            var state = $('#MainContent_txtState').val();
            var zip = $('#MainContent_txtZipCode').val();
            var country = $('#MainContent_txtCountry').val();
            var phone = $('#MainContent_txtTelephone').val();
            var email = $('#MainContent_txtEmail').val();
            if ($('#MainContent_txtCustomerName').val() == '') {
                //alert("Name is Required");
                lblError = '* Name is Required <br/>';
            }
            if ($('#MainContent_txtTelephone').val() == '') {
                //alert("Telephone is Required");
                lblError += '* Telephone is Required <br />';
            }
            if ($('#MainContent_txtAddress1').val() == '') {
                //alert("Adderess is Required");
                lblError += '* Adderess is Required <br/>';
            }
            if ($('#MainContent_txtCity').val() == '') {
                //alert("City is Required");
                lblError += '* City is Required <br/>';
            }
            if ($('#MainContent_txtState').val() == '') {
                //alert("State is Required");
                lblError += '* State is Required <br/>';
            }
            if ($('#MainContent_txtZipCode').val() == '') {
                // alert("ZipCode is Required");
                lblError += '* ZipCode is Required <br/>';
            }
            if ($('#MainContent_txtCountry').val() == '') {
                //alert("Country is Required");
                lblError += '* Country is Required <br/>';
            }
            if (name != '' && add1 != '' && phone != '' && city != '' && state != '' && zip != '' && country != '') {
                $('#MainContent_lblError').css('visibility', 'hidden');
                $("#divShipping").html('<p>' + name + '<br/>' + add1 + '<br/>' + city + ',' + state + ',' + zip + '<br/>' + country + '<br/>T:' + phone + '<br/>E:' + email + '</p>');
                return true;
            }
            else {
                lblError += '</p>';
                $('#divMain').html($('#MainContent_lblError').html(lblError).css({ visibility: 'visible', 'margin-left': '170px' }));
                return false;
            }

        }

        function GetOfferLines() {
            var sub = $('#MainContent_lblTotal').val().substring(1);
            var subtotal = parseFloat(sub);
            var shipping, donation;
            if ($("#MainContent_txtShipping").val() == '')
                shipping = 0;
            else
                shipping = parseFloat($("#MainContent_txtShipping").val());
            if ($("#MainContent_txtADonation").val() == '')
                donation = 0;
            else
                donation = parseFloat($("#MainContent_txtADonation").val());

            var grandtotal = subtotal + shipping + donation;
            //$("#MainContent_lblGrandTotal").text(grandtotal);
            $("#divOfferLines").html('<p>Shipping Total: $' + shipping + '<br/>Order Subtotal: $' + subtotal + '<br/>Additional Donation: $' + donation + '<br/>Total: $' + grandtotal + '</p>');
            return true;
        }

        //mani

        function CalculateTotals() {
            var gv = document.getElementById("<%= gdvOfferLine.ClientID %>");
            var tb = gv.getElementsByTagName("input");
            var lb = gv.getElementsByTagName("span");

            var sub = 0;
            var total = 0;
            var indexQ = 1;
            var indexP = 0;
            var price = 0;
            var qty = 0;
            var totalQty = 0;

            for (var i = 0; i < gv.rows.length - 1; i++) {
                if (tb[indexP].type == "text") {
                    ValidateNumber(tb[indexP].value);

                    price = lb[i].innerHTML.replace("$", "");
                    sub = (parseFloat(price) * parseFloat($('#MainContent_gdvOfferLine_TXTQty_' + i).val()));
                    if (isNaN(sub)) {
                        //lb[i + indexQ].innerHTML = "0.00";
                        $('#MainContent_gdvOfferLine_LBLSubTotal_' + i).val("0.00");
                        sub = 0;
                    }
                    else {
                        $('#MainContent_gdvOfferLine_LBLSubTotal_' + i).val(FormatToMoney(sub, "$", "."));
                        //lb[i + indexQ].innerHTML = ; ;
                    }

                    indexQ++;
                    indexP = indexP + 2;

                    if (isNaN($('#MainContent_gdvOfferLine_TXTQty_' + i).val()) || $('#MainContent_gdvOfferLine_TXTQty_' + i).val() == "") {
                        qty = 0;
                    }
                    else {
                        qty = $('#MainContent_gdvOfferLine_TXTQty_' + i).val();
                    }

                    totalQty += parseInt(qty);
                    total += parseFloat(sub);
                }
            }

            //document.getElementById('MainContent_LBLTotal1').value = FormatToMoney(total, "$", ",", ".");
            $('#MainContent_lblTotal').val(FormatToMoney(total, "$", "."));
            calculatGranTotal();
        }

        //        function CalculateTotals() {
        //            var gv = document.getElementById("<%= gdvOfferLine.ClientID %>");
        //            var tb = gv.getElementsByTagName("input");
        //            var lb = gv.getElementsByTagName("span");

        //            var sub = 0;
        //            var total = 0;
        //            var indexQ = 1;
        //            var indexP = 0;
        //            var price = 0;
        //            var qty = 0;
        //            var totalQty = 0;

        //            for (var i = 0; i < tb.length; i++) {
        //                if (tb[i].type == "text") {
        //                    ValidateNumber(tb[i]);

        //                    price = lb[indexP].innerHTML.replace("$", "").replace(",");
        //                    sub = parseFloat(price) * parseFloat(tb[i].value);
        //                    if (isNaN(sub)) {
        //                        //lb[i + indexQ].innerHTML = "0.00";
        //                        $('#MainContent_gdvOfferLine_LBLSubTotal_' + i).text("0.00");
        //                        sub = 0;
        //                    }
        //                    else {
        //                        $('#MainContent_gdvOfferLine_LBLSubTotal_' + i).text(FormatToMoney(sub, "$", "."));
        //                        //lb[i + indexQ].innerHTML = ; ;
        //                    }

        //                    indexQ++;
        //                    indexP = indexP + 2;

        //                    if (isNaN(tb[i].value) || tb[i].value == "") {
        //                        qty = 0;
        //                    }
        //                    else {
        //                        qty = tb[i].value;
        //                    }

        //                    totalQty += parseInt(qty);
        //                    total += parseFloat(sub);
        //                }
        //            }

        //            //document.getElementById('MainContent_LBLTotal1').value = FormatToMoney(total, "$", ",", ".");
        //            $('#MainContent_lblTotal').val(FormatToMoney(total, "$", "."));
        //            calculatGranTotal();
        //        }

        function ValidateNumber(o) {
            if (o.value > 0) {
                o.value = o.value.replace(/[^\d]+/g, ''); //Allow only whole numbers
            }
        }
        function isThousands(position) {
            if (Math.floor(position / 3) * 3 == position) return true;
            return false;
        };

        function FormatToMoney(theNumber, theCurrency, theDecimal) {
            var theDecimalDigits = Math.round((theNumber * 100) - (Math.floor(theNumber) * 100));
            theDecimalDigits = "" + (theDecimalDigits + "0").substring(0, 2);
            theNumber = "" + Math.floor(theNumber);
            var theOutput = theCurrency;
            for (x = 0; x < theNumber.length; x++) {
                theOutput += theNumber.substring(x, x + 1);
                //                if (isThousands(theNumber.length - x - 1) && (theNumber.length - x - 1 != 0)) {
                //                    theOutput += theThousands;
                //                };
            };
            theOutput += theDecimal + theDecimalDigits;
            return theOutput;
        }

        function calculatGranTotal() {

            var sub = $('#MainContent_lblTotal').val().substring(1);
            var subtotal = parseFloat(sub);
            var shipping, donation;
            if ($("#MainContent_txtShipping").val() == '')
                shipping = 0;
            else
                shipping = parseFloat($("#MainContent_txtShipping").val());
            if ($("#MainContent_txtADonation").val() == '')
                donation = 0;
            else
                donation = parseFloat($("#MainContent_txtADonation").val());

            var grandtotal = subtotal + shipping + donation;
            $("#MainContent_lblGrandTotal").val(grandtotal);
        }
        

    </script>
    </form>
</body>
</html>
