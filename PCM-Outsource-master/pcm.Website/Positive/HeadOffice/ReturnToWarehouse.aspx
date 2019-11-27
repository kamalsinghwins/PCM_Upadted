<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReturnToWarehouse.aspx.vb" Inherits="pcm.Website.ReturnToWarehouse" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function onEnd(s, e) {  
            lp.Hide();
        }
        function imports(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Import";
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
            if (e.errorText != "")
                ShowMessage(e.errorText);
            else if (e.callbackData == "success")
                ShowMessage("File uploading has been successfully completed. Please click Import to import your Uploaded file.");
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
        function OnAddItemButtonClick(s, e) {
            lp.Show();
            $.ajax(
                {
                    url: 'ReturnToWarehouse.aspx/Save',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d.Success == true) {
                            var PrintPath = data.d.DriverLogPath;
                            PrintDocument(PrintPath)
                              lp.Hide()
                           
                            setTimeout(function () {
                                DeleteFile(PrintPath);
                            }, 5000);
                        }
                        else {
                            lblError.SetText(data.d.Message)
                            dxPopUpError.Show()
                             lp.Hide()
                        }
                    }

                })
          

        }
        function PrintDocument(url) {
            var top = (screen.availHeight - 600) / 2;
            var left = (screen.availWidth - 800) / 2;
            window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
                "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);
        }
        function DeleteFile( PrintPath) {
            var file =PrintPath
            $.ajax({
                url: 'ReturnToWarehouse.aspx/DeleteFile',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: '{ file: "' +  file + '"}',
                success: function (data) {

                }
            });
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
     <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="cab" Width="100%" OnCallback="ASPxCallback1_Callback"
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="300px"
                        HeaderText="Return To Warehouse">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>                              
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFile" runat="server" Width="120px" Text="File">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtFile"  runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblUploadFile" runat="server" Width="120px" Text="Upload File">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td colspan="6">
                                            <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" NullText="Click here to browse for file"
                                                OnFilesUploadComplete="UploadControl_FilesUploadComplete" ShowUploadButton="True" UploadMode="Advanced" Width="230px" FileUploadMode="OnPageLoad">
                                                <ValidationSettings AllowedFileExtensions=".txt" MaxFileSize="20000000" ShowErrors="False">
                                                </ValidationSettings>
                                                <ClientSideEvents FilesUploadComplete="FilesUploadComplete" FileUploadStart="FileUploadStart" UploadingProgressChanged="UploadingProgressChanged" />
                                            </dx:ASPxUploadControl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <div style="float: right">
                                                <dx:ASPxButton ID="cmdImport" runat="server" Text="Import">
                                                    <ClientSideEvents Click="imports"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="btnClear" runat="server" Text="Clear">
                                                <ClientSideEvents Click="clear"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td>
                                            <div style="float: right">
                                                <dx:ASPxButton AutoPostBack="false" ID="btnSave" runat="server" Text="Save">
                                                     <ClientSideEvents Click="function(s, e) { OnAddItemButtonClick(s, e); }" />
                                                </dx:ASPxButton>
                                            </div>                                       
                                        </td>
                                    </tr>
                                </table>                               
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <br />
                    <br />
                    <div style="width :470px" >
                    <dx:ASPxListBox AutoPostBack ="false" Height="200px"  ID="lvData" Width="700px" runat="server" ValueType="System.String">
                        <Columns>
                            <dx:ListBoxColumn Name="IBT Out" Caption="IBT Out" FieldName="Details(0)">
                            </dx:ListBoxColumn>
                            <dx:ListBoxColumn Caption="Store Name" FieldName="branch_code" Name="Store Name">
                            </dx:ListBoxColumn>
                            <dx:ListBoxColumn Caption="Branch Code" FieldName="branch_name" Name="Branch Code">
                            </dx:ListBoxColumn>
                        </Columns>
                    </dx:ASPxListBox>
                        </div>
                    <br />
                    <br />
                    <br />
                    <br />
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
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
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" ClientInstanceName="lblError" runat="server" Text="Error"
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
