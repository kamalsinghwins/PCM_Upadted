<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="StockQuantityImport.aspx.vb" Inherits="pcm.Website.StockQuantityImport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .custom{
            color: #3300ff!important; 
        }
    </style>
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.hide();
        }
         function downloadSample(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "downloadSample";
            lp.Show();
            cab.PerformCallback();
        }

        function save(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
            lp.Show();
            cab.PerformCallback();
        }

        function clear(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";
            lp.Show();
            cab.PerformCallback();
        }
        function save(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
            lp.Show();
            cab.PerformCallback();
        }
        var uploadCompleteFlag;
        function FilesUploadComplete(s, e) {
            uploadCompleteFlag = true;
            PopupProgressingPanel.Hide();
            if (e.errorText != "") {
                ShowMessage(e.errorText);
            }
            else if (e.callbackData == "success") {
                ShowMessage("File uploading has been successfully completed. The email with your stock quantity import report will be sent shortly.");
            }
              else if (e.callbackData == "no username") {
                window.location = "/Intranet/Default.aspx";
            }
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
        function TextBoxKeyDown(s, e) {
            if (!((e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 57)
            ))
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
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
        OnCallback="ASPxCallbackPanel1_Callback"
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                    HeaderText="Stock Quantity Imports" CssClass="date_panel">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <div>
                                <table>
                                    <tr>
                                        <td style="padding-right: 20px; vertical-align: top;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtEmail" Style="width: 100%;" runat="server" NullText="EMail (Seperate email addresses with a comma)" ClientInstanceName="txtEmail">
                                                        </dx:ASPxTextBox>
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
                                                        <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Allowed file types: csv"
                                                            Font-Size="8pt">
                                                        </dx:ASPxLabel>
                                                        <br />
                                                        <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Maximum file size: 20Mb" Font-Size="8pt">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>CSV FILE MUST BE IN FORMAT:</b><br />
                                                        Branch Code,Generated Code,Quantity<br />
                                                        <dx:ASPxHyperLink CssClass="custom" Cursor="pointer" ID="lnkSample" runat="server" Text="Click here to see sample CSV">
                                                            <ClientSideEvents Click="downloadSample" />
                                                        </dx:ASPxHyperLink>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td><span style="color: red; font-size: small"><b>Note</b></span> :All fields must be separated by (,)<br />
                                                        <span>You can leave out the columns that you do not need
                                                            <br />
                                                            to update. The only required fields are: branch_code
                                                            <br />
                                                            and one other data field</span>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <div>
                    <%--<dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="false" CssClass="date_panel" Width="90%"
                        OnDataBinding="grid_DataBinding">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Generated Code" FieldName="generated_code">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Quantity" FieldName="quantity">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>--%>
                </div>
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
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="Error"
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
