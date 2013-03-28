<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderEntry.aspx.cs" Inherits="Biblethon.BiblethonOrderEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>BibleThon Order Entry</title>
    <link href="Style/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <telerik:RadWindowManager Modal="true" Behaviors="Close" KeepInScreenBounds="True"
        VisibleStatusbar="False" runat="server" DestroyOnClose="true">
    </telerik:RadWindowManager>
    <div class="main">
        <div class="container">
            <div class="leftcolumn">
                <h2>
                    Order Summary</h2>
                <p>
                    Order No:
                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label></p>
                <p>
                    <asp:Label runat="server" ID="LblDate"></asp:Label></p>
                <p>
                    Operator:
                    <asp:Label ID="LblOperator" runat="server"></asp:Label></p>
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
                    <br />
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
                    <asp:Label ID="LblShippingTotal" runat="server"></asp:Label></p>
                <p>
                    Order Subtotal:
                    <asp:Label ID="LblOrderSubtotal" runat="server"></asp:Label></p>
                <p>
                    Additional Donation:
                    <asp:Label ID="LblAdditionalDonation" runat="server"></asp:Label></p>
                <p>
                    Total:
                    <asp:Label ID="LblTotalAmount" runat="server"></asp:Label></p>
                <h3>
                    CC Payment Information
                </h3>
                <p>
                    Credit Card No:
                    <asp:Label ID="LblCreditCardNo" runat="server"></asp:Label>
                </p>
                <p>
                    Credit Card Expired:
                    <asp:Label ID="LblCreditExpired" runat="server"></asp:Label>
                </p>
            </div>
            <div class="accordionarea">
                <div class="errorContainer">
                    <asp:Label ID="lblError" CssClass="error errorinfo" runat="server"></asp:Label></div>
                <div class="accordion">
                    <!-- Billing Address Accordation -->
                    <h3>
                        <a id="0" href="#" class="BillingAddressAccordion">Billing Address </a>
                    </h3>
                    <div class="divAccordian BillingAddressAccordion">
                        <table>
                            <tr>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustName" for="LblBName,TxtOCCustomerName" runat="server" CssClass="txtarea required"></asp:TextBox>
                                            </td>
                                            <td>
                                                <img class="search" id="imgSearch" alt="search" src="../Images/search_button.png" />
                                            </td>
                                            <td>
                                                <img class="search" id="ImgAddCustomer" alt="Add Customer" src="../Images/AddRecord.gif" />
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td class="label">
                                                Address
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblAddress1" for="LblBAddress1" runat="server"></asp:Label>
                                                <input type="text" for="LblBAddress1,lblAddress1" name="TxtBillingAddress1" class="hidden txtarea"
                                                    id="TxtBillingAddress1" />
                                            </td>
                                        </tr>
                                        <tr id="tr2" runat="server">
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblAddress2" for="LblBAddress2" runat="server"></asp:Label>
                                                <input type="text" for="LblBAddress2,lblAddress2" name="TxtBillingAddress2" class="hidden txtarea"
                                                    id="TxtBillingAddress2" />
                                            </td>
                                        </tr>
                                        <tr id="tr3" runat="server">
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblAddress3" runat="server"></asp:Label>
                                                <input type="text" for="LblBAddress3,lblAddress3" name="TxtBillingAddress3" class="hidden txtarea"
                                                    id="TxtBillingAddress3" />
                                            </td>
                                        </tr>
                                        <tr id="tr4" runat="server">
                                            <td class="label">
                                                City
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblCity" runat="server"></asp:Label>
                                                <input type="text" for="LblBAddress3,lblAddress3" name="TxtBillingCity" class="hidden"
                                                    id="TxtBillingCity" />
                                            </td>
                                        </tr>
                                        <tr id="tr5" runat="server">
                                            <td class="label">
                                                State
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblState" runat="server"></asp:Label>
                                                <input type="text" for="lblState" name="TxtBillingState" class="hidden" id="TxtBillingState" />
                                            </td>
                                        </tr>
                                        <tr id="tr6" runat="server">
                                            <td class="label">
                                                Country
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblCountry" for="LblBCountry" runat="server"></asp:Label>
                                                <input type="text" for="lblCountry" name="TxtBillingCountry" class="hidden" id="TxtBillingCountry" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Telephone
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhone" for="LblBTelephone1" runat="server" CssClass="txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr7" runat="server">
                                            <td class="label">
                                                Email
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBEmail" for="LblBEmail" runat="server" CssClass="txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Zip Code
                                            </td>
                                            <td>
                                                <asp:Label ID="lblZipCode" runat="server"></asp:Label>
                                                <input type="text" for="lblZipCode" name="TxtBillingZipcode" class="hidden" id="TxtBillingZipcode" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbMention" Text="Please do not mention name" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbCaller" Text="First time caller" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbShipping" Text="Use same information for shipping address" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td>
                                    <asp:Button ID="btnBillContinue" runat="server" Text="Continue" CssClass="btn" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- Shipping Address Accordation -->
                    <h3>
                        <a id="1" href="#" class="ShippingAddressAccordion">Shipping Address </a>
                    </h3>
                    <div class="divAccordian secondpara ShippingAddressAccordion">
                        <table>
                            <tr>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Name
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtCustomerName" for="LblSAName" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                            <td>
                                                <img class="search" id="ImgShippingAddressModify" alt="BillingAddressModify" src="../Images/search_button.png" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Address
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddress1" for="LblSAAddress1" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddress2" for="LblSAAddress2" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAddress3" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                City
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                State
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtState" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Country
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCountry" for="LblSACountry" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Telephone
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelephone" for="LblSATelephone1" runat="server" CssClass="required phoneUs txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Email
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" for="LblSAEmail" runat="server" CssClass="required email txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Zip Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="required zipUS txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td>
                                    <asp:Button ID="BtnShippingBack" runat="server" Text="<< Back" CssClass="btn" />
                                    <asp:Button ID="btnShipContinue" runat="server" Text="Continue" CssClass="btn" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- Offer Lines Accordation -->
                    <h3>
                        <a id="2" href="#" class="OffLinesAccordion">Offer Lines </a>
                    </h3>
                    <div class="divAccordian OffLinesAccordion">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="RadGridOfferLines" runat="server" AutoGenerateColumns="False"
                                        PagerStyle="" PageSize="10" AllowPaging="True" OnPageIndexChanged="RadGridOfferLinesDataBinding"
                                        CssClass="radGrid">
                                        <MasterTableView>
                                            <Columns>
                                                <telerik:GridBoundColumn HeaderText="Offer Id" DataField="OfferId">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Description" HeaderText="Description" DataField="Description">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Quantity" ItemStyle-HorizontalAlign="Right"
                                                    HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="TxtQuantity" Text='<%# Eval("Quantity") %>' CssClass="txtarea currencyfield"></asp:TextBox>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn UniqueName="Price" HeaderText="Price" DataField="Price"
                                                    DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Total" HeaderText="Total" DataField="Total"
                                                    DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                        <table class="total">
                            <tr>
                                <td class="label">
                                    SubTotal
                                </td>
                                <td class="totalright">
                                    <asp:TextBox ID="lblTotal" CssClass="txtarea currencyfield" runat="server" EnableViewState="True"
                                        ReadOnly="True">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="label">
                                    Shipping
                                </td>
                                <td class="totalright">
                                    <asp:TextBox ID="txtShipping" CssClass="txtarea currencyfield" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="label">
                                    AdditionalDonation
                                </td>
                                <td class="totalright">
                                    <asp:TextBox ID="txtADonation" CssClass="txtarea currencyfield" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="label">
                                    GroundTotal
                                </td>
                                <td class="totalright">
                                    <asp:TextBox ID="lblGrandTotal" runat="server" CssClass="txtarea currencyfield" ReadOnly="True"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="HdnGrandTotal" />
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td>
                                    <asp:Button ID="BtnOfferLineBack" runat="server" Text="<< Back" CssClass="btn" />
                                    <asp:Button ID="btnConfirmOffer" runat="server" Text="Continue" CssClass="btn" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- Credit Card Information -->
                    <h3>
                        <a id="3" href="#" class="creditCardAccordion">Credit Card Payment Information </a>
                    </h3>
                    <div class="divAccordian creditCardAccordion">
                        <table>
                            <tr>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Credit Card Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem>Visa</asp:ListItem>
                                                    <asp:ListItem>MasterCard</asp:ListItem>
                                                    <asp:ListItem>Discover</asp:ListItem>
                                                    <asp:ListItem>AmEx</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Credit Card No.
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCreditCardNo" for="LblCreditCardNo" runat="server" CssClass="required txtarea"
                                                    MaxLength="19" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Expiration Date (MMYY)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExpirationDate" for="LblCreditExpired" runat="server" CssClass="required txtarea"
                                                    MaxLength="4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                CVN
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCVN" runat="server" CssClass="required txtarea" MaxLength="3" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="BtnCreditCardBack" runat="server" Text="<< Back" CssClass="btn" />
                                    <asp:Button ID="BtnCreditCardProcess" runat="server" Text="Continue" CssClass="btn" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- Order Confirmation Accordation -->
                    <h3>
                        <a id="4" href="#" class="orderConfirmationAccordion">Total, Confirmation and Order
                            Submission </a>
                    </h3>
                    <div class="divAccordian fifthpara">
                        <table>
                            <tr>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Customer No.
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="TxtOCCustomerNO" ReadOnly="True" CssClass="txtarea readOnly"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Customer Name.
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="TxtOCCustomerName" ReadOnly="True" CssClass="txtarea readOnly"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Order Date
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="TxtCCOrderDate" ReadOnly="True" CssClass="txtarea readOnly currencyfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Order Total
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="TxtTotalOrder" ReadOnly="True" CssClass="txtarea readOnly currencyfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="BtnOrderConfirmationBack" runat="server" Text="<< Back" CssClass="btn" />
                                    <asp:Button ID="btnSaveOrder" runat="server" Text="Save" OnClick="BtnSaveOrderClick"
                                        CssClass="btn" />
                                    <asp:Button ID="btnProcessOrder" runat="server" Text="Process" CssClass="btn" OnClick="BtnProcessOrderClick" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
    <asp:HiddenField runat="server" ID="HdnCustomerNo" />
    <input id="hidAccordionIndex" runat="server" type="hidden" value="0" />
    <input id="hidAddressCode" runat="server" type="hidden" value="" />
    <script src="Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="Scripts/contentScript.js" type="text/javascript"></script>
    </form>
</body>
</html>