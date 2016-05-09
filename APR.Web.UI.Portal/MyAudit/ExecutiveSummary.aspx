<%@ Page Title="" Language="C#" MasterPageFile="~/AprMaster.Master" AutoEventWireup="true" CodeBehind="ExecutiveSummary.aspx.cs" Inherits="APR.Web.UI.Portal.MyAudit.ExecutiveSummary" ViewStateMode="Disabled" EnableViewState="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="aprMain" runat="server">
        <div class="grid_3">
        <dx:ASPxComboBox ClientIDMode="Static" ID="ASPxComboBoxAuditName" EnableCallbackMode="true" runat="server" AllowMouseWheel="true" AnimationType="Auto" DropDownStyle="DropDownList" AutoPostBack="true" SelectedIndexChanged="ASPxComboBoxAuditName_SelectedIndexChanged"
            ClientInstanceName="ASPxComboBoxAuditName" >
        </dx:ASPxComboBox>
    </div>
    <div class="grid_9">
    </div>
    <div class="clear"></div>

    <div class="grid_12">

        <div id="dvExecutiveGrid" runat="server" ></div>
        </div>
    <div class="clear"></div>
    <div class="grid_12">
        <div id="dvExecutiveSummeryChart" runat="server" ></div>
        </div>
    <div class="clear"></div>
</asp:Content>
