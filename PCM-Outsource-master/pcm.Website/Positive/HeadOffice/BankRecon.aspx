<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BankRecon.aspx.vb" Inherits="pcm.Website.BankRecon" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/StockcodeManager/InventoryAndTax.ascx" TagName="InventoryAndTax"
    TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../js/General/jquery-2.0.3.min.js"></script>
    <script>
        function RecalculateCharsRemaining(editor) {
            var maxLength = parseInt(editor.maxLength ? editor.maxLength : editor.GetInputElement().maxLength);
            var editValue = editor.GetValue();
            var valueLength = editValue != null ? editValue.toString().length : 0;
            var charsRemaining = maxLength - valueLength;
            SetCharsRemainingValue(editor, charsRemaining >= 0 ? charsRemaining : 0);
        }
        function SetCharsRemainingValue(textEditor, charsRemaining) {
            var associatedLabel = ASPxClientControl.GetControlCollection().Get(textEditor.name + "_cr");
            var color = GetLabelColor(charsRemaining).toString();
            associatedLabel.SetText("<span style='color: " + color + ";'>" + charsRemaining.toString() + "</span>");
        }
        function GetLabelColor(charsRemaining) {
            if (charsRemaining < 5) return "red";
            if (charsRemaining < 12) return "#F3A250";
            return "green";
        }

        // ASPxMemo - MaxLength emulation
        function InitMemoMaxLength(memo, maxLength) {
            memo.maxLength = maxLength;
        }
        function EnableMaxLengthMemoTimer(memo) {
            memo.maxLengthTimerID = window.setInterval(function () {
                var text = memo.GetText();
                if (text.length > memo.maxLength) {
                    memo.SetText(text.substr(0, memo.maxLength));
                    RecalculateCharsRemaining(memo);
                }
            }, 50);
        }
        function DisableMaxLengthMemoTimer(memo) {
            if (memo.maxLengthTimerID) {
                window.clearInterval(memo.maxLengthTimerID);
                delete memo.maxLengthTimerID;
            }
        }

        function fadeOut(s, e) {
            var animationTime = 50;
            var opacity = 9;
            function func() {
                var popupHtmlElement = s.GetWindowElement(-1);
                popupHtmlElement.style.opacity = "0." + opacity;
                opacity--;
                if (opacity == -1) {
                    window.clearInterval(fading);
                    s.Hide();
                    popupHtmlElement.style.opacity = "1";
                }
            }
            var fading = window.setInterval(func, animationTime);
        }
    </script>
    <script type="text/javascript">
        // <![CDATA[
        var uploadCompleteFlag;
        function FilesUploadComplete(s, e) {
            uploadCompleteFlag = true;
            PopupProgressingPanel.Hide();
            if (e.errorText != "")
                ShowMessage(e.errorText);
            else if (e.callbackData == "success")
                ShowMessage("File uploading has been successfully completed.");
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
        HeaderText="Upload Files" CssClass="date_panel">
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
                                        <%-- <dx:ASPxTextBox ID="txtEmail" style="width:100%;" runat="server" NullText="EMail (Seperate email addresses with a comma)" 
                                            ClientInstanceName="txtEmail">
                                        </dx:ASPxTextBox>--%>
                                        <%--<dx:ASPxDropDownEdit ID="drpFileType" ClientInstanceName="drpFileType" style="width:100%;" runat="server"></dx:ASPxDropDownEdit>--%>
                                        <dx:ASPxComboBox ID="cboFileType" ClientInstanceName="cboFileType" Style="width: 100%;" runat="server" ValueType="System.String">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" NullText="Click here to browse for file"
                                            OnFilesUploadComplete="UploadControl_FilesUploadComplete" ShowUploadButton="True" UploadMode="Advanced" Width="330px" FileUploadMode="OnPageLoad">
                                            <ValidationSettings MaxFileSize="20000000" ShowErrors="False">
                                            </ValidationSettings>
                                            <ClientSideEvents FilesUploadComplete="FilesUploadComplete" FileUploadStart="FileUploadStart" UploadingProgressChanged="UploadingProgressChanged" />
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style77"></td>
                                    <td class="note">
                                        <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Allowed image types: All"
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
                            </table>
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server"
        HeaderText="Comments" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                <table>
                    <tr>
                        <td style="padding-right: 20px; vertical-align: top;">
                            <table>
                                <tr>

                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Date"
                                            Font-Size="12pt">
                                        </dx:ASPxLabel>
                                        &nbsp;</td>
                                    <td>
                                        <dx1:ASPxDateEdit ID="txtDate" runat="server" ClientInstanceName="pd"
                                            DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd"
                                            UseMaskBehavior="True">
                                            <CalendarProperties ShowClearButton="False">
                                            </CalendarProperties>

                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must select a Date" />
                                            </ValidationSettings>

                                        </dx1:ASPxDateEdit>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Branch"
                                            Font-Size="12pt">
                                        </dx:ASPxLabel>
                                        &nbsp;</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" Style="width: 100%;"
                                            runat="server"
                                            ValueType="System.String">

                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Comments"
                                            Font-Size="12pt">
                                        </dx:ASPxLabel>
                                        &nbsp;</td>
                                    <td class="auto-style77">
                                        <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="345px" ClientInstanceName="notes">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="A comment is required" />
                                            </ValidationSettings>
                                            <ClientSideEvents KeyDown="RecalculateCharsRemaining" KeyUp="RecalculateCharsRemaining"
                                                GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
                                                Init="function(s, e) { InitMemoMaxLength(s, 400); RecalculateCharsRemaining(s); }"></ClientSideEvents>

                                        </dx:ASPxMemo>
                                    </td>
                                    <td class="note"></td>
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
                                    <td class="auto-style77">&nbsp;</td>
                                    <td class="auto-style77">
                                        <dx:ASPxLabel ID="txtNotes_cr" runat="server" EnableClientSideAPI="True" />
                                        &nbsp;</td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">&nbsp;</td>
                                    <td class="auto-style77">
                                        <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                            ValidationGroup="save" Width="319px">
                                        </dx:ASPxValidationSummary>
                                        &nbsp;</td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">&nbsp;</td>
                                    <td class="auto-style77">
                                        <dx1:ASPxButton ID="cmdSave" ValidationGroup="save" runat="server" Text="Save" Width="130px">
                                        </dx1:ASPxButton>
                                    </td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server"
        HeaderText="EFT IDs" CssClass="date_panel" Width="503px">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                <table>
                    <tr>
                        <td style="padding-right: 20px; vertical-align: top;">
                            <table>
                                

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Branch"
                                            Font-Size="12pt">
                                        </dx:ASPxLabel>
                                        &nbsp;</td>
                                    <td colspan="2">
                                        <dx:ASPxComboBox ID="cboBranchEFT" ClientInstanceName="cboBranchEFT" Style="width: 100%;"
                                            runat="server" AutoPostBack="true"
                                            ValueType="System.String">
                                            <ValidationSettings ValidationGroup="save_eft">
                                                <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="EFT ID"
                                            Font-Size="12pt">
                                        </dx:ASPxLabel>
                                        &nbsp;</td>
                                    <td class="auto-style77" >
                                         <dx:ASPxComboBox ID="cboEFTId" ClientInstanceName="cboEFTid"
                                            runat="server" DropDownStyle="DropDown"
                                            ValueType="System.String" Width="160px">

                                            <ValidationSettings ValidationGroup="save_eft">
                                                <RequiredField IsRequired="True" ErrorText="You must input an EFT ID" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>

                                    </td>
                                    <td class="note"></td>
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
                                    <td class="auto-style77">&nbsp;</td>
                                    <td class="auto-style77" colspan="2">
                                      
                                        &nbsp;</td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">&nbsp;</td>
                                    <td class="auto-style77" colspan="2">
                                        <dx:ASPxValidationSummary ID="ASPxValidationSummary2" runat="server" RenderMode="BulletedList"
                                            ValidationGroup="save_eft" Width="319px">
                                        </dx:ASPxValidationSummary>
                                        &nbsp;</td>
                                    <td class="note">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style77">&nbsp;</td>
                                    <td >
                                         <dx1:ASPxButton ID="cmdDeleteEFTId" ValidationGroup="save_eft" runat="server" Text="Delete" Width="130px">
                                        </dx1:ASPxButton>
                                        
                                    </td>
                                    <td><dx1:ASPxButton ID="cmdSaveEFTId" ValidationGroup="save_eft" runat="server" Text="Save" Width="130px">
                                        </dx1:ASPxButton></td>
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
    <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
        HeaderText="Info" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpPanel"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
        DisappearAfter="1000" PopupAnimationType="Fade">
        <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div>
                    <div id="offer">
                        <dx:ASPxLabel ID="lblStatus" runat="server" Text="Comment Inserted" Font-Size="16px">
                        </dx:ASPxLabel>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseButtonClick="fadeOut" />
    </dx:ASPxPopupControl>
</asp:Content>
