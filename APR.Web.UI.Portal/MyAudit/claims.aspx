<%@ Page Title="" Language="C#" MasterPageFile="~/AprMaster.master"  AutoEventWireup="True" CodeBehind="Claims.aspx.cs" Inherits="APR.Web.UI.Portal.MyAudit.Claims" ViewStateMode="Disabled" EnableViewState="true" %>

<%@ Register TagPrefix="dxrp" Namespace="DevExpress.Web.ASPxSplitter" Assembly="DevExpress.Web.v12.2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../scripts/ajaxupload.js"></script>
    <%--<script type="text/javascript" src="http://bp.yahooapis.com/2.4.21/browserplus-min.js"></script>--%>
    <script type="text/javascript" src="../scripts/browserplus-min.js"></script>
    <script type="text/javascript" src="../plupload/js/plupload.full.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="aprMain" runat="Server">
    <input type="hidden" runat="server" id="hdnSelectedAudit" />
    <div class="grid_3">
        <dx:ASPxComboBox ID="ASPxComboBoxAuditName" Native="true" runat="server" AllowMouseWheel="true" Theme="PlasticBlue" AnimationType="Auto" DropDownStyle="DropDownList" AutoPostBack="false" ClientSideEvents-SelectedIndexChanged="OnIndexChange" ClientInstanceName="ASPxComboBoxAuditName">
        </dx:ASPxComboBox>
    </div>

    <div class="grid_9">
    </div>

    <div class="clear"></div>

    <div class="grid_12">
       
        <dxrp:ASPxSplitter ID="ASPxSplitterGrid" ClientIDMode="Static" runat="server"  ResizingMode="Live"
             Orientation="Vertical" Height="400px" Width="100%" ShowSeparatorImage="true"
             SeparatorSize="5px" AllowResize="true"  ClientInstanceName="dxSplitter" PaneMinSize="100%">
            <Panes >
                <dxrp:SplitterPane Name="gvInvContainer" MinSize="192px" Size="99%" >
                    <ContentCollection>
                        <dxrp:SplitterContentControl ID="SplitterContentControl1" runat="server">
                            <div id="dvInvoiceGrid" runat="server" class="gvResizeInv">
                            </div>
                        </dxrp:SplitterContentControl>
                    </ContentCollection>
                </dxrp:SplitterPane>
                 <dxrp:SplitterPane Name="gvEmptyContainer" ScrollBars="None" AllowResize="True" MinSize="0px" Size="1%" >
                    <ContentCollection>
                        <dxrp:SplitterContentControl Visible="true" ID="SplitterContentControl3" runat="server">
                        </dxrp:SplitterContentControl>
                    </ContentCollection>
                </dxrp:SplitterPane>
            </Panes>
            <ClientSideEvents PaneResized="OnSplitterPaneResized" />
        </dxrp:ASPxSplitter>
        <dxrp:ASPxSplitter ID="ASPxSplitterNavGrid" ClientIDMode="Static" runat="server" 
             Orientation="Vertical" Height="350px" Width="100%" ShowSeparatorImage="true"
             SeparatorSize="5px" AllowResize="true"  ClientInstanceName="dxSplitter" PaneMinSize="100%">
            <Panes >
                <dxrp:SplitterPane Name="gvClmsContainer" MinSize="192px" Size="99%" >
                    <ContentCollection>
                        <dxrp:SplitterContentControl ID="SplitterContentControl2" runat="server">
                            <div id="dvClaimsGrid" runat="server" class="gvResizeClaims">
                            </div>
                        </dxrp:SplitterContentControl>
                    </ContentCollection>
                </dxrp:SplitterPane>
                <dxrp:SplitterPane Name="gvEmptyContainer" ScrollBars="None" AllowResize="True" MinSize="0px" Size="1%" >
                    <ContentCollection>
                        <dxrp:SplitterContentControl Visible="true" ID="SplitterContentControl4" runat="server">
                        </dxrp:SplitterContentControl>
                    </ContentCollection>
                </dxrp:SplitterPane>
            </Panes>
            <ClientSideEvents PaneResized="OnSplitterPaneResized" />
        </dxrp:ASPxSplitter>
    </div>

    <div class="clear"></div>

    <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" LoadContentViaCallback="None"
        PopupElementID="ShowButton" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AllowDragging="false"
        ShowFooter="true" HeaderText="Invoice Related Documents" ClientInstanceName="ClientPopupControl" Width="300px" Modal="false" Theme="DevEx" ShowCollapseButton="true" ShowShadow="true" CssClass="popup">
        
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server" CssClass="popupContainer">
                <div id="AttachmentsPopControl"></div>
                <div id="downloadQuee"></div>
            </dx:PopupControlContentControl>
        </ContentCollection>

        <FooterTemplate>
            <div style="float: right; margin: 3px;">
                <div class="dxbButton" id="container" style="position:relative;display:inline-block;">
                    <input type="button" id="lnkUpload" class="dxb" value="Add"/>
                </div>
                <div class="dxbButton" id="Div3" style="position:relative;display:none;">
                    <input type="button" id="lnkOpen" class="dxb" value="Open"/>
                </div>
                <div id="Div2"  class="dxbButton"  style="position:relative;display:inline-block;">
                    <dx:ASPxButton ID="attachmentDownload" ClientInstanceName="attachmentDownload" runat="server" Text="Download" AutoPostBack="False"
                        ClientSideEvents-Click="GetSelectedFileDownLoad" Native="true"/>
                </div>
                <div class="dxbButton" id="Div1" style="position:relative;display:inline-block;">
                    <dx:ASPxButton ID="UpdateButton" runat="server" Text="Print" AutoPostBack="False" ClientSideEvents-Click="GetSelectedFilePrint" Native="true"/>
                </div>
            </div>
        </FooterTemplate>

    </dx:ASPxPopupControl>
    
    <dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_Callback"></dx:ASPxCallback>
    <div id="pdfDisplay" style="position:absolute; left:-9999px;"></div>
    <iframe src="" id="download-iframe" style="width:0;height:0" ></iframe>
    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
    <script src="../scripts/UploadPrint.js"></script>
</asp:Content>