﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Alere_Verbal_Auth_Existing_Patient_PickList.aspx.cs"
    Inherits="Alere_Verbal_Auth_Existing_Patient_PickList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alere Verbal Auth Existing Patient PickList</title>
    <style type="text/css">
        html, body, form
        {
            padding: 0;
            margin: 0;
            height: 100%;
            background: #f2f2de;
        }
        
        body
        {
            font: normal 11px Arial, Verdana, Sans-serif;
        }
        
        fieldset
        {
            height: 150px;
        }
        
        * + html fieldset
        {
            height: 154px;
            width: 268px;
        }
    </style>
</head>
<body onload="AdjustRadWidow();">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator ID="QsfFromDecorator" runat="server" DecoratedControls="All"
        EnableRoundedCorners="false" />
    <div>
        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <telerik:RadGrid runat="server" ID="RadGrid1" DataSourceID="LinqDataSource1" AllowAutomaticUpdates="True"
            AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AutoGenerateColumns="False"
            AllowPaging="True" OnItemUpdated="RadGrid1_ItemUpdated" OnItemInserted="RadGrid1_ItemInserted"
            OnItemDeleted="RadGrid1_ItemDeleted" OnPreRender="RadGrid1_PreRender" 
            AutoGenerateDeleteColumn="True" AutoGenerateEditColumn="True" CellSpacing="0" 
            GridLines="None">
            <MasterTableView DataKeyNames="PicklistKey, PicklistItem" CommandItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                AllowPaging="false" DataSourceID="LinqDataSource1">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="PicklistKey" 
                        FilterControlAltText="Filter PicklistKey column" HeaderText="PicklistKey" 
                        ReadOnly="false" SortExpression="PicklistKey" UniqueName="PicklistKey">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="PicklistItem" 
                        FilterControlAltText="Filter PicklistItem column" HeaderText="PicklistItem" 
                        ReadOnly="false" SortExpression="PicklistItem" UniqueName="PicklistItem">
                    </telerik:GridBoundColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn ButtonType="ImageButton" />
                    <PopUpSettings Modal="true" />
                </EditFormSettings>

<PagerStyle AlwaysVisible="True"></PagerStyle>
            </MasterTableView>
            <PagerStyle AlwaysVisible="true" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="AlereVerbalAuthExistingPatientDataDataContext"
            EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
            TableName="AS_Alere_VAEP_PickLists" EntityTypeName="">
        </asp:LinqDataSource>
    </div>
    </form>
</body>
</html>
