<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransactionSummary.ascx.cs"
    Inherits="UserControls_TransactionSummary" %>
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
    Insurance</h3>
<ul>
    <li>
        <asp:Label ID="label1" runat="server" Text="Primary Plan: "></asp:Label><asp:Label runat="server" ID="lblPrimaryPlan" Text="[Primary Plan]"></asp:Label></li>
    <li>
        <asp:Label ID="lblPrimaryPolicyNumber" runat="server" Text="[Primary Policy Number]"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblPrimaryGroupNumber" Text="[Primary Group Number]"></asp:Label></li>
    <li>
        <br /></li>
    <li>
        <asp:Label ID="Label2" runat="server" Text="Secondary Plan: "></asp:Label><asp:Label runat="server" ID="lblSecondaryPlan" Text="[Secondary Plan]"></asp:Label></li>
    <li>
        <asp:Label ID="lblSecondaryPolicyNumber" runat="server" Text="[Secondary Policy Number]"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblSecondaryGroupNumber" Text="[Secondary Group Number]"></asp:Label></li>
</ul>
<h3>
    Doctor</h3>
<ul>
    <li>
        <asp:Label runat="server" ID="lblDoctor" Text="[Doctor]"></asp:Label></li>    
        <li>
        <asp:Label runat="server" ID="lblDoctorPhoneNumber" Text="[DoctorPhoneNumber]"></asp:Label></li>    
        <li>
        <asp:Label runat="server" ID="lblDoctorCity" Text="[DoctorCity]"></asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblDoctorState" Text="[DoctorState]">
        </asp:Label>&nbsp;|&nbsp;<asp:Label runat="server" ID="lblDoctorZip" Text="[DoctorZip]"></asp:Label></li>    
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
