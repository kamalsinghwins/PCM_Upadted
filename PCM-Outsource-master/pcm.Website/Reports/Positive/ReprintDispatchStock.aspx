<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReprintDispatchStock.aspx.vb" Inherits="pcm.Website.ReprintDispatchStock" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <style type="text/css">
        .panel {
            margin: 0;
        }
    </style>
    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }

        function run(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run";
            lp.Show();
            cab.PerformCallback();
        }

        function download(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Download";
            lp.Show();
            cab.PerformCallback();
        }

        function OnAddItemButtonClick(s, e) {
            lp.Show();
            debugger;
            var t = s.mainElement.getAttribute('value');
            $.ajax(
                {
                    url: 'ReprintDispatchStock.aspx/GetDocument',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: '{DispatchNumber:"' + t + '"}',
                    success: function (data) {

                        if (data.d.Success == true) {
                            var DeliveryNotesPath = data.d.DeliveryNotePath;
                            var DriverLogPath = data.d.DriverLogPath;
                            PrintDocument(DeliveryNotesPath)
                            PrintDocument(DriverLogPath)
                              lp.Hide()
                            //$("#errs").removeClass("hide")
                            //$("#errs").removeClass("alert-danger")
                            //$("#errs").addClass("alert-success")
                            //$("#errs p").text("Dispatch updated successfully")

                            setTimeout(function () {
                                DeleteFile(DeliveryNotesPath, DriverLogPath);
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
        function DeleteFile(DeliveryNotesPath, DriverLogPath) {
            var receipts = {
                DeliveryNotesPath: DeliveryNotesPath,
                DriverLogPath: DriverLogPath
            }
            $.ajax({
                url: 'ReprintDispatchStock.aspx/DeleteFiles',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: '{ receipts:' + JSON.stringify(receipts) + '}',
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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                        HeaderText="Reprint Document" CssClass="date_panel">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblDispatchNumber" runat="server" Text="Dispatch Number" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtDispatchNumber" runat="server" Width="250px"></dx:ASPxTextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run">
                                                <ClientSideEvents Click="run" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxGridView ID="gvDispatchNumber" OnDataBinding="gvDispatchNumber_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
                        EnableTheming="True">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Dispatch Number" FieldName="dispatch_number" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="No. of Barcodes" FieldName="count" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Driver" FieldName="driver" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="KM" FieldName="km" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Registration" FieldName="rego" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Date" FieldName="dispatched_timestamp" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn Caption="Action" VisibleIndex="6">
                                <DataItemTemplate>
                                    <dx:ASPxButton ID="btnAddItem" runat="server" Text="Download"
                                        AutoPostBack="False"
                                        Value='<%#Eval("dispatch_number")%>' ClientInstanceName="btnAddItem">
                                        <ClientSideEvents Click="function(s, e) { OnAddItemButtonClick(s, e); }" />
                                    </dx:ASPxButton>

                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <Settings ShowFilterRow="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>

                        <Settings ShowFooter="True" />

                    </dx:ASPxGridView>
                  
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
