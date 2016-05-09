<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AprUploadControl.ascx.cs" Inherits="APR.Web.UI.Portal.Controls.Usercontrols.AprUploadControl" %>
<dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" LoadContentViaCallback="None"
    PopupElementID="ShowButton" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AllowDragging="false"
    ShowFooter="true" HeaderText="Invoice Related Documents" ClientInstanceName="ClientPopupControl" Width="300px" Modal="false" Theme="PlasticBlue" ShowCollapseButton="true" ShowShadow="true" CssClass="popup">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server" CssClass="popupContainer">

            <div id="AttachmentsPopControl"></div>
            <div id="downloadQuee"></div>
        </dx:PopupControlContentControl>
    </ContentCollection>

    <FooterTemplate>
        <div style="float: right; margin: 3px;">
            <dx:ASPxButton ID="UpdateButton" runat="server" Text="Print" AutoPostBack="False" ClientSideEvents-Click="GetSelectedFilePrint" />
        </div>
        <div style="float: right; margin: 3px;">
            <dx:ASPxButton ID="attachmentDownload" ClientInstanceName="attachmentDownload" runat="server" Text="Download" AutoPostBack="False"
                ClientSideEvents-Click="GetSelectedFileDownLoad" />
        </div>
        <div style="float: right; margin: 3px;">
            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Open" AutoPostBack="False" />
        </div>
        <div style="float: right; margin: 3px;">
            <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Add" AutoPostBack="False" ClientSideEvents-Click="ShowUploader" />
            <div class="UploadDiv" title="Upload File">

                <%--<dx:ASPxCallback ClientInstanceName="clientCallBack" ID="ASPxCallbackClient" runat="Server" OnCallback="ASPxCallbackClient_Callback"></dx:ASPxCallback>--%>
                <div id="UploadStatus">
                </div>
                <input type="button" id="UploadButton" class="UploadButton" />
                <div id="UploadedFile">
                </div>
            </div>
            <%--<dxup:ASPxUploadControl ID="UploadControl" runat="server" ShowAddRemoveButtons="True"
                    Width="300px" ShowUploadButton="True" AddUploadButtonsHorizontalPosition="Left"
                    ShowProgressPanel="True" ClientInstanceName="UploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete"
                    FileInputCount="3">
                    <ValidationSettings AllowedFileExtensions=".doc,.docx,.pdf,.xls,">
                    </ValidationSettings>
                    <ClientSideEvents FileUploadComplete="function(s, e) { FileUploaded(s, e) }" FileUploadStart="function(s, e) { FileUploadStart(); }" />
                </dxup:ASPxUploadControl>--%>
        </div>
    </FooterTemplate>
</dx:ASPxPopupControl>
<div id="pdfDisplay" style="position: absolute; left: -9999px;"></div>
<iframe src="" id="download-iframe" style="width: 0; height: 0" ></iframe>