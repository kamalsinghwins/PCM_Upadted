<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="UploadPayments.aspx.vb" Inherits="pcm.Website.UploadPayments" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../js/General/application.js"></script>
    <script type="text/javascript">
        // <![CDATA[
        var uploadCompleteFlag;
        function FilesUploadComplete(s, e) {
            uploadCompleteFlag = true;
            PopupProgressingPanel.Hide();
            if (e.errorText != "")
                ShowMessage(e.errorText);
            else if (e.callbackData == "success")
                ShowMessage("File uploading has been successfully completed. The processing results will be sent to your email address shortly.");
        }
        function ShowMessage(message) {
            window.setTimeout("alert('" + message + "')", 0);
        }
        function FileUploadStart(s, e) {
            uploadCompleteFlag = false;
            window.setTimeout("ShowPopupProgressingPanel()", 500);
        }
        function ShowPopupProgressingPanel() {
            if (!uploadCompleteFlag) {
                PopupProgressingPanel.Show();
                pbProgressing.SetPosition(0);
                pnlProgressingInfo.SetContentHtml("");
            }
        }
        function UploadingProgressChanged(s, e) {
            pbProgressing.SetPosition(e.progress);
            var info = e.currentFileName + "&emsp;[" + GetKBytes(e.uploadedContentLength) + " / " + GetKBytes(e.totalContentLength) + "] KBytes";
            pnlProgressingInfo.SetContentHtml(info);
        }
        function GetKBytes(bytes) {

            return Math.floor(bytes / 1024);
        }
        // ]]> 
    </script>
    <style type="text/css">
        .auto-style77 {
            width: 88px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SideHolder" runat="server">
    <table>
        <tr>
            <td>
                <dx:ASPxDockPanel runat="server" ID="ASPxDockPanel1" PanelUID="DateTime" HeaderText="Date & Time"
                    Height="95px" ClientInstanceName="dateTimePanel" Width="230px" OwnerZoneUID="zone1">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <widget:DateTime ID="xDTWid" runat="server" />
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>

            </td>

        </tr>
    </table>
    <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
      <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
        HeaderText="Upload Allocations" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>
                    <tr>
                        <td style="padding-right: 20px; vertical-align: top;">
                            <table>
                                <tr>
                                    
                                    <td>

                                        <%--  <dx:ASPxUploadControl ID="uplImage" runat="server" ClientInstanceName="uploader" ShowProgressPanel="True"
                                            NullText="Click here to browse files..." Size="35" OnFileUploadComplete="uplImage_FileUploadComplete" ShowUploadButton="True">
                                            <ClientSideEvents FileUploadStart="FileUploadStart"
                                                UploadingProgressChanged="UploadingProgressChanged"
                                                FilesUploadComplete="FilesUploadComplete" />
                                            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".csv">
                                            </ValidationSettings>
                                        </dx:ASPxUploadControl>--%>
                                        <dx:ASPxTextBox ID="txtEmail" style="width:100%;" runat="server" NullText="EMail (Seperate email addresses with a comma)" ClientInstanceName="txtEmail">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td>

                                        <dx:ASPxCheckBox ID="chkIDNumber" runat="server" Text="Check ID Number"></dx:ASPxCheckBox>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" NullText="Click here to browse for file" 
                                            OnFilesUploadComplete="UploadControl_FilesUploadComplete" ShowUploadButton="True" UploadMode="Advanced" Width="330px" FileUploadMode="OnPageLoad">
                                            <ValidationSettings AllowedFileExtensions=".csv" MaxFileSize="20000000" ShowErrors="False">
                                            </ValidationSettings>
                                            <ClientSideEvents FilesUploadComplete="FilesUploadComplete" FileUploadStart="FileUploadStart" UploadingProgressChanged="UploadingProgressChanged" />
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style77"></td>
                                    <td class="note">
                                        <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Allowed image types: csv"
                                            Font-Size="8pt">
                                        </dx:ASPxLabel>
                                        <br />
                                        <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Maximum file size: 20Mb" Font-Size="8pt">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="2" class="buttonCell">
                                        <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload"
                                            Width="100px" ClientEnabled="False" Style="margin: 0 auto;">
                                            <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>CSV FILE MUST BE IN FORMAT:<br /> Account Number, ID Number (or blank), Amount (in positive value), Date of Payment</td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="PopupProgressingPanel"
        Modal="True" CloseAction="None" Width="400px" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" AllowDragging="True" PopupAnimationType="None"
        HeaderText="Uploading Info" ShowCloseButton="False" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 100%;">
                            <dx:ASPxProgressBar ID="pbProgressing" ClientInstanceName="pbProgressing" runat="server"
                                Width="100%">
                            </dx:ASPxProgressBar>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) { UploadControl.Cancel(); PopupProgressingPanel.Hide(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPanel ID="pnlProgressingInfo" ClientInstanceName="pnlProgressingInfo" runat="server"
                    Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    
</asp:Content>
