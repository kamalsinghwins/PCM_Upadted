<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ScreensaverImages.aspx.vb" Inherits="pcm.Website.ScreensaverImages" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

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
                ShowMessage("Your image has been uploaded successfully.");
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

    <script>
        function OnClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Accept";

            lp.Show();
            cab.PerformCallback();


        }


        function onEnd(s, e) {
            lp.Hide();

        }

    </script>
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
<SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                    HeaderText="Upload Screensaver Images" CssClass="date_panel">
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
                                                    <dx:ASPxTextBox ID="txtDescription" Style="width: 100%;" runat="server" NullText="Description of Image" ClientInstanceName="txtEmail">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" NullText="Click here to browse for file"
                                                        OnFilesUploadComplete="UploadControl_FilesUploadComplete" ShowUploadButton="True" UploadMode="Advanced" Width="330px" FileUploadMode="OnPageLoad">
                                                        <ValidationSettings AllowedFileExtensions=".jpg" MaxFileSize="5120000" ShowErrors="False">
                                                        </ValidationSettings>
                                                        <ClientSideEvents FilesUploadComplete="FilesUploadComplete" FileUploadStart="FileUploadStart" UploadingProgressChanged="UploadingProgressChanged" />
                                                    </dx:ASPxUploadControl>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="auto-style77"></td>
                                                <td class="note">
                                                    <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Allowed image types: jpg"
                                                        Font-Size="8pt">
                                                    </dx:ASPxLabel>
                                                    <br />
                                                    <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Maximum file size: 512kb" Font-Size="8pt">
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
                <br />
                <br />
                <dx:ASPxGridView ID="grdImages" runat="server" AutoGenerateColumns="False" Width="98%" CssClass="date_panel" OnDataBinding="dxGrid_DataBinding"
                    KeyFieldName="id">
                    
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="image_name" Caption="Image Name" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="image_description" Caption="Image Description" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Is Active" FieldName="is_active" VisibleIndex="3">
                            <DataItemTemplate>
                                <dx:ASPxCheckBox ID="chkBox" runat="server" OnInit="chkBox_Init"
                                    Value='<%#Eval("is_active")%>' Border-BorderStyle="None">
                                </dx:ASPxCheckBox>

                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataImageColumn FieldName="image_name" VisibleIndex="4" Caption="Image">
                            <PropertiesImage ImageUrlFormatString="~/Uploaded/{0}" ImageHeight="120px">
                            </PropertiesImage>
                        </dx:GridViewDataImageColumn>

                    </Columns>
                    <SettingsPager PageSize="10">
                    </SettingsPager>
                </dx:ASPxGridView>
                <table class="date_panel" width="98%">
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxButton ID="cmdAccept" Style="float: right;" runat="server" Text="Accept">
                                <ClientSideEvents Click="OnClick" />
                            </dx:ASPxButton>
                            <asp:Label ID="Label1" runat="server" Text="Please click Save before leaving each page on the grid in order to Update the records you have changed."></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="There was an error updating. Please contact support."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
