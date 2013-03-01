<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Alere_VerbalAuthExistingPatient_Summary.ascx.cs"
    Inherits="Alere_VerbalAuthExistingPatient_Summary" %>
<h2>
    Summary:</h2>
<h3>
    Basic Information</h3>
<ul>
    <li>
        <asp:Label runat="server" ID="lblOrderNum" Text="[Order Number]"></asp:Label></li>
    <li>
        <asp:Label runat="server" ID="lblPatientID" Text="[PatientID]"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblFirstName" Text="[First Name]"></asp:Label>&nbsp;
        <asp:Label runat="server" ID="lblLastName" Text="[Last Name]"></asp:Label></li> 
    <li>
        <asp:Label runat="server" ID="lblMeterType" Text="[Meter Type]"></asp:Label></li>       
    <li>
        <asp:Label runat="server" ID="lblLancetType" Text="[Lancet Type]"></asp:Label></li>       
</ul>
<h3>
    Patient Details</h3>
<ul>
    <li>
        <asp:Label runat="server" ID="lblPhone" Text="[Phone]"></asp:Label>
    </li>
    <li>
        <asp:Label runat="server" ID="lblAddressID" Text="[Address ID]"></asp:Label>
    </li>
    <li>
        <asp:Label runat="server" ID="lblPatientAddress" Text="[Address 1]"></asp:Label>
    </li>
    <li>
        <asp:Label runat="server" ID="lblCity" Text="[City]"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblState" Text="[State]">
        </asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblZip" Text="[Zip]"></asp:Label>
    </li>
    <li>
        <asp:Label runat="server" ID="lblEmail" Text="[E-Mail]"></asp:Label>
    </li>
</ul>
