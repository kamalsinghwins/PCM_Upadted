<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="AccountsDrive.aspx.vb" Inherits="pcm.Website.AccountsDrive" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
        }
    </style>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="500px"
                        HeaderText="Accounts Drive" CssClass="mb-20">
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
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td colspan="5"></td>
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
                    <br />
                    <br />
                    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBinding="gvMaster_DataBinding"
                        EnableTheming="True">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Employee #" FieldName="employee_number" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="First Name" CellStyle-CssClass="ellipsis" FieldName="first_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Account Opened" FieldName="total_opened" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Active Accounts" FieldName="total_active" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Commission Accounts" FieldName="total_spent" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server"
                                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="account_number" CellStyle-CssClass="ellipsis" Caption="Account #" VisibleIndex="0" />
                                        <dx:GridViewDataColumn FieldName="first_name" CellStyle-CssClass="ellipsis" Caption="First Name" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="last_name" CellStyle-CssClass="ellipsis" Caption="Last Name" VisibleIndex="2" />
                                        <dx:GridViewDataColumn FieldName="status" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="3" Caption="Account Status" />
                                        <dx:GridViewDataColumn FieldName="spent" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="4" Caption="Spent" />
                                        <dx:GridViewDataColumn FieldName="sent_to" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="5" Caption="Sent To" />
                                        <dx:GridViewDataColumn FieldName="first_purchase" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="6" Caption="Branch Code " />
                                    </Columns>

                                    <Settings ShowFooter="True" />

                                    <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                                    <SettingsPager PageSize="20">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="true" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem DisplayFormat="Total Opened: {0}" FieldName="total_opened" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="Total Active: {0}" FieldName="total_active" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="Total Commission : {0}" FieldName="total_spent" SummaryType="Sum" />
                        </TotalSummary>
                        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
                    </dx:ASPxGridView>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px">
                                </dx:ASPxButton>
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
                                        <dx:ASPxLabel ID="lblError" runat="server" Text="Error"
                                            Font-Size="16px">
                                        </dx:ASPxLabel>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                        <ClientSideEvents CloseButtonClick="fadeOut" />
                    </dx:ASPxPopupControl>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
</asp:Content>


