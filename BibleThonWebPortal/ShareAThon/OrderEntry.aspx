<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderEntry.aspx.cs" Inherits="ShareAThon.ShareAThonOrderEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Share-a-Thon Order Entry</title>
    <link href="../ShareAThon/Style/Style.css" rel="stylesheet" type="text/css" />
    <script src="../Script/jquery-1.8.3.js" type="text/javascript"></script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="hdOrderId" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Modal="true" Behaviors="Close" KeepInScreenBounds="True"
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
                    <asp:Label runat="server" ID="LblCustomerName"></asp:Label></p>
                <p>
                    <asp:Label runat="server" ID="LblDate"></asp:Label></p>
                <p>
                    Operator:
                    <asp:Label ID="LblOperator" runat="server"></asp:Label></p>
                <p>
                    From: Internet
                </p>
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
                    Donation
                </h3>
                <p>
                    <asp:Label runat="server" ID="LblDonationInfo"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="LblMethodOfDonation"></asp:Label>
                </p>
                <div id="CCInformation">
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
                    <p>
                        <asp:Label ID="LblCreditCardType" runat="server"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="LblInitialChargeOn" runat="server"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="LblRecurrence" runat="server"></asp:Label>
                    </p>
                </div>
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
                                                <asp:TextBox ID="txtCustName" for="LblBName,LblCustomerName" runat="server" CssClass="txtarea required"></asp:TextBox>
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
                                            <td colspan="2">
                                                <asp:Label ID="lblAddress1" for="LblBAddress1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr2" runat="server">
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblAddress2" for="LblBAddress2" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr3" runat="server">
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblAddress3" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr4" runat="server">
                                            <td class="label">
                                                City
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblCity" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr5" runat="server">
                                            <td class="label">
                                                State
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblState" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr6" runat="server">
                                            <td class="label">
                                                Country
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblCountry" for="LblBCountry" runat="server"></asp:Label>
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Referral Source
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpReferral" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem>Radio</asp:ListItem>
                                                    <asp:ListItem>Internet</asp:ListItem>
                                                    <asp:ListItem>Dish</asp:ListItem>
                                                    <asp:ListItem>DirectTV</asp:ListItem>
                                                    <asp:ListItem>Cable</asp:ListItem>
                                                    <asp:ListItem>Other</asp:ListItem>
                                                </asp:DropDownList>
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
                    <!--Donation -->
                    <h3>
                        <a id="1" href="#" class="DonationAccordion">Donation </a>
                    </h3>
                    <div class="divAccordian secondpara DonationAccordion">
                        <table>
                            <tr>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label">
                                                Frequency
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpFrequency" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                                    <asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                No.of Months
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpMonths" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                    <asp:ListItem>8</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Other Explanation
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExplanation" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Currently Donor of ($)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurDonor" runat="server" CssClass="required txtarea readOnly right"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Method of Donation
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpMethdDonation" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem>Credit Card</asp:ListItem>
                                                    <asp:ListItem>Cheque</asp:ListItem>
                                                    <asp:ListItem>Cash</asp:ListItem>
                                                    <asp:ListItem>Money Order</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Initial Charge on
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInitialCharge" runat="server" CssClass="required txtarea"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="halftd">
                                    <table class="hafdiv">
                                        <tr>
                                            <td class="label" colspan="2">
                                                <asp:CheckBox ID="ChkMayUseAdChallenge" Text="May use as challenge" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Donation Amount ($)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDonationAmt" for="LblSAEmail" runat="server" CssClass="required txtarea right"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Increasing To ($)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncreasing" runat="server" CssClass="required readOnly txtarea right"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Other Method of Delivery
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDLMethodOfDelivery" runat="server" CssClass="required txtarea">
                                                    <asp:ListItem>Mail</asp:ListItem>
                                                    <asp:ListItem>Will bring to church</asp:ListItem>
                                                    <asp:ListItem>Brought to radio station</asp:ListItem>
                                                    <asp:ListItem>Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                Day to Charge Monthly
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpchargeMontly" runat="server" CssClass="required txtarea">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!-- Offer Lines  -->
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="RadGridOfferLines" runat="server" AutoGenerateColumns="False"
                                        PagerStyle="" PageSize="10" AllowPaging="True" OnPageIndexChanged="RadGridOfferLinesDataBinding"
                                        CssClass="radGrid">
                                        <MasterTableView>
                                            <Columns>
                                                <telerik:GridBoundColumn HeaderText="Offer ID" DataField="OfferId">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Description" DataField="Description">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Quantity" ItemStyle-HorizontalAlign="Right"
                                                    HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="TxtQuantity" Text='<%# Eval("Quantity") %>' CssClass="txtarea currencyfield"></asp:TextBox>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn UniqueName="Price" HeaderText="Price" DataField="Price"
                                                    Visible="False" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                                <td style="vertical-align: top; padding-left: 15px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbNogiftRecord" Text="No Gift Record" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbRadioStation" Text="Product about radio station" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="buttunback">
                            <tr>
                                <td>
                                    <asp:Button ID="BtnDonationBack" runat="server" Text="<< Back" CssClass="btn" />
                                    <asp:Button ID="BtnDonationContinue" runat="server" Text="Continue" CssClass="btn" />
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
                        <a id="4" href="#" class="orderConfirmationAccordion">Confirmation and Order Submission
                        </a>
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
                                    <asp:Button ID="btnSaveOrder" runat="server" Text="Save and Process" CssClass="btn"
                                        OnClick="OrderEntrySave" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
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