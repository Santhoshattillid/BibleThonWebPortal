<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderEntry.aspx.cs" Inherits="Biblethon_OrderEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Purchase Order Entry</title>
    <link href="Style/Style.css" rel="stylesheet" type="text/css" />
    <link href="Style/jquery-ui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Modal="true" Behaviors="Close" KeepInScreenBounds="True"
        VisibleStatusbar="False" runat="server" DestroyOnClose="true">
    </telerik:RadWindowManager>
    <asp:Label ID="lblError" CssClass="error" runat="server" Text=""></asp:Label>
    <div class="main">
        <div class="container">
            <div class="leftcolumn">
                <h2>
                    Order Summary</h2>
                <p>
                    Order No:
                    <asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label></p>
                <p>
                    <asp:Label runat="server" ID="LblDate"></asp:Label></p>
                <p>
                    Operator:
                    <asp:Label ID="LblOperator" runat="server" Text=""></asp:Label></p>
                <h3>
                    Billing Address
                </h3>
                <p>
                    <asp:Label runat="server" ID="LblBName"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBAddress1"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBAddress2"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBCityStateZip"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBCountry"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBTelephone1"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblBEmail"></asp:Label>
                </p>
                <h3>
                    Shipping Address
                </h3>
                <p>
                    <asp:Label runat="server" ID="LblSAName"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSAAddress1"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSAAddress2"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSACityStateZip"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSACountry"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSATelephone1"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblSAEmail"></asp:Label>
                </p>
                <h3>
                    Offer Lines
                </h3>
                <p>
                    Shipping Total:
                    <asp:Label ID="LblShippingTotal" runat="server" Text=""></asp:Label></p>
                <p>
                    Order Subtotal:
                    <asp:Label ID="LblOrderSubtotal" runat="server" Text=""></asp:Label></p>
                <p>
                    Additional Donation:
                    <asp:Label ID="LblAdditionalDonation" runat="server" Text=""></asp:Label></p>
                <p>
                    Total:
                    <asp:Label ID="LblTotalAmount" runat="server" Text=""></asp:Label></p>
                <h3>
                    CC Payment Information
                </h3>
            </div>
            <div id="accordion" style="float: left; width: 750px;">
                <!-- Billing Address Accordation -->
                <a id="0" href="" class="BillingAddressAccordion">
                    <h3>
                        Billing Address</h3>
                </a>
                <div class="divAccordian BillingAddressAccordion">
                    <table style="width: 700px;" class="secondpara" align="center">
                        <tr>
                            <td>
                                <table style="width: 350px;">
                                    <tr>
                                        <td class="label">
                                            Name
                                        </td>
                                        <td style="width: 180px">
                                            <asp:TextBox ID="txtCustName" Width="180px" runat="server" CssClass="txtarea required"></asp:TextBox>
                                        </td>
                                        <td>
                                            <img class="search" id="imgSearch" alt="search" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearchBy" Style="visibility: hidden" Width="0px" runat="server"
                                                Text="" />
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
                                        <td>
                                        </td>
                                        <td colspan="3" style="width: 280px;">
                                            <asp:Label ID="lblAddress2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr3" runat="server">
                                        <td>
                                        </td>
                                        <td colspan="3" style="width: 280px;">
                                            <asp:Label ID="lblAddress3" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr4" runat="server">
                                        <td style="width: 70px; color: Red;">
                                            City
                                        </td>
                                        <td colspan="3" style="width: 280px;">
                                            <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr5" runat="server">
                                        <td style="width: 70px; color: Red;">
                                            State
                                        </td>
                                        <td style="width: 50px;">
                                            <asp:Label ID="lblState" Width="60px" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="width: 100px; color: Red;">
                                            Zip Code
                                        </td>
                                        <td>
                                            <asp:Label ID="lblZipCode" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr6" runat="server">
                                        <td style="width: 70px; color: Red;">
                                            Country
                                        </td>
                                        <td colspan="3" style="width: 280px;">
                                            <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
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
                                            <asp:TextBox ID="txtPhone" Width="240px" runat="server" CssClass="txtarea required"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr id="tr7" runat="server">
                                        <td style="width: 70px; color: red">
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
                                <%--<asp:Button ID="btnBillingBack" runat="server" Text="<< Back" CssClass="btn" />--%>
                                <asp:Button ID="btnBillContinue" runat="server" Text="Continue" OnClick="btnBillContinue_Click"
                                    CssClass="btn" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Shipping Address Accordation -->
                <a id="1" href="#" class="ShippingAddressAccordion">
                    <h3>
                        Shipping Address</h3>
                </a>
                <div class="divAccordian secondpara ShippingAddressAccordion">
                    <table style="width: 700px;" align="center">
                        <tr>
                            <td valign="top">
                                <table style="width: 350px;">
                                    <tr>
                                        <td class="label">
                                            Name
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCustomerName" runat="server" Width="250px" CssClass="required"></asp:TextBox>
                                        </td>
                                        <td>
                                            <img class="search" id="ImgShippingAddressModify" alt="BillingAddressModify" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: Red;">
                                            Address
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtAddress1" runat="server" Width="250px" CssClass="required"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtAddress2" runat="server" Width="250px" CssClass="required"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtAddress3" runat="server" Width="250px" CssClass="required"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: Red;">
                                            City
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtCity" runat="server" Width="250px" CssClass="required"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: Red;">
                                            State
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtState" runat="server" Width="75px" CssClass="required"></asp:TextBox>
                                        </td>
                                        <td style="color: Red;">
                                            Zip Code
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" runat="server" Width="55px" CssClass="required zipUS "></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: Red;">
                                            Country
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtCountry" runat="server" Width="130px" CssClass="required"></asp:TextBox>
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
                                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="required phoneUs"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: red">
                                            Email
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="required email"></asp:TextBox>
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
                                <asp:Button ID="BtnShippingBack" runat="server" Text="<< Back" CssClass="btn" />
                                <asp:Button ID="btnShipContinue" runat="server" Text="Continue" OnClick="btnShipContinue_Click"
                                    CssClass="btn BtnshippingContinue" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Offer Lines Accordation -->
                <a id="2" href="" class="OffLinesAccordion">
                    <h3>
                        Offer Lines</h3>
                </a>
                <div class="divAccordian OffLinesAccordion">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:GridView ID="gdvOfferLine" AutoGenerateColumns="false" CssClass="gridId" runat="server"
                                    PageSize="10" HeaderStyle-BackColor="" AlternatingRowStyle-BackColor="#ecebe7">
                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                    <PagerStyle BackColor="#F0FFFF" />
                                    <HeaderStyle BackColor="#A9A9A9" />
                                    <Columns>
                                        <asp:BoundField DataField="OfferId" HeaderText="Offer ID">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTQty" runat="server" Text="0" Style="text-align: right"></asp:TextBox>
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
                                                    background-color: #edf8f8;" runat="server" ForeColor="Green" Text="$0.00" ReadOnly="True"></asp:TextBox>
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
                                <asp:TextBox ID="lblTotal" runat="server" EnableViewState="True" Style="text-align: right;
                                    border-width: 0px; border-collapse: collapse; background-color: #edf8f8;" ReadOnly="True">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Shipping
                            </td>
                            <td align="right" style="width: 150px">
                                $<asp:TextBox ID="txtShipping" runat="server" Style="margin-left: 0px; text-align: right;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                AdditionalDonation
                            </td>
                            <td align="right" style="width: 150px">
                                $<asp:TextBox ID="txtADonation" runat="server" Style="text-align: right;"></asp:TextBox>
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
                    </table>
                    <table class="buttunback">
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="BtnOfferLineBack" runat="server" Text="<< Back" CssClass="btn" />&nbsp;&nbsp;
                                <asp:Button ID="btnConfirmOffer" runat="server" Text="Continue" OnClick="btnConfirmOffer_Click"
                                    CssClass="btn" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Credit Card Information -->
                <a id="3" href="" class="creditCardAccordion">
                    <h3>
                        Credit Card Payment Information</h3>
                </a>
                <div class="divAccordian creditCardAccordion">
                    <table style="width: 700px;" align="center" class="forthpara" cellpadding="0" cellspacing="4">
                        <tr>
                            <td class="label">
                                Credit Card Type
                            </td>
                            <td style="width: 200px;" class="txtarea">
                                <asp:DropDownList ID="ddlCreditCardType" runat="server" Style="width: 200px;" CssClass="required">
                                    <asp:ListItem>Visa</asp:ListItem>
                                    <asp:ListItem>MasterCard</asp:ListItem>
                                    <asp:ListItem>Discover</asp:ListItem>
                                    <asp:ListItem>AmEx</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 60px">
                            </td>
                            <td class="label">
                                Expiration Date (MMYY)
                            </td>
                            <td style="width: 200px;" class="txtarea">
                                <asp:TextBox ID="txtExpirationDate" runat="server" CssClass="required" />
                            </td>
                        </tr>
                        <tr>
                            <td class="label">
                                Credit Card No.
                            </td>
                            <td class="txtarea">
                                <asp:TextBox ID="txtCreditCardNo" runat="server" Width="152px" CssClass="required" />
                            </td>
                            <td style="width: 60px">
                            </td>
                            <td class="label">
                                CVN
                            </td>
                            <td class="txtarea">
                                <asp:TextBox ID="txtCVN" runat="server" CssClass="required" />
                            </td>
                        </tr>
                    </table>
                    <table class="buttunback">
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <asp:Button ID="BtnCreditCardBack" runat="server" Text="<< Back"
                                    CssClass="btn" />
                                <asp:Button ID="BtnCreditCardProcess" runat="server" Text="Continue" OnClick="BtnCreditCardProcess_Click"
                                    CssClass="btn" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Order Confirmation Accordation -->
                <a id="4" href="">
                    <h3>
                        Total, Confirmation and Order Submission
                    </h3>
                </a>
                <div class="divAccordian fifthpara">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="label">
                                Order No.
                            </td>
                            <td class="inputxt">
                                <asp:Label ID="lblOrderNum" Width="200px" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 50px">
                            </td>
                            <td class="label">
                                Order Total
                            </td>
                            <td class="inputxt">
                                <asp:Label ID="lblOrderTotal" Width="200px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%; padding-top: 10px; border-top: 1px solid #c4bd97">
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <asp:Button ID="BtnOrderConfirmationBack" runat="server"  Text="<< Back"
                                    CssClass="btn" />&nbsp;&nbsp;
                                <asp:Button ID="btnSaveOrder" runat="server" Text="Save" Width="63px" OnClick="btnSaveOrder_Click"
                                    CssClass="btn" />&nbsp;&nbsp;
                                <asp:Button ID="btnProcessOrder" runat="server" Text="Process" Width="74px" CssClass="btn"
                                    OnClick="btnProcessOrder_Click" />
                            </td>
                            <td style="width: 30px">
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="HdnCustomerNo" />
    <input id="hidAccordionIndex" runat="server" type="hidden" value="0" />
    <input id="hidAddressCode" runat="server" type="hidden" value="" />
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="Scripts/contentScript.js" type="text/javascript"></script>
    </form>
</body>
</html>
