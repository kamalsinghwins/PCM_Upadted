<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="HistoricalClockingReport.aspx.vb" Inherits="pcm.Website.HistoricalClockingReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%"
                        HeaderText="Historical Clocking Report" CssClass="mb-20">
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
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td colspan="7">
                                            <dx:ASPxRadioButton ID="radAll" GroupName="employees" runat="server">
                                            </dx:ASPxRadioButton>
                                            <dx:ASPxLabel ID="lblAll" runat="server" Text="All" Width="90px">
                                            </dx:ASPxLabel>
                                            <dx:ASPxRadioButton ID="radPermanent" GroupName="employees" runat="server">
                                            </dx:ASPxRadioButton>
                                            <dx:ASPxLabel ID="Permanent" runat="server" Text="Permanent" Width="120px">
                                            </dx:ASPxLabel>
                                            <dx:ASPxRadioButton ID="radCasual" GroupName="employees" runat="server">
                                            </dx:ASPxRadioButton>
                                            <dx:ASPxLabel ID="lblCasual" runat="server" Text="Casual" Width="90px">
                                            </dx:ASPxLabel>

                                        </td>

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
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBinding="gvMaster_DataBinding"
                        EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Branch Name" CellStyle-CssClass="ellipsis" FieldName="branch_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Permanent Employees Clocked In" FieldName="count1" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Casual Employees Clocked In" FieldName="count" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>


                        </Columns>
                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server"
                                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="employee_number" CellStyle-CssClass="ellipsis" Caption="Employee Number" VisibleIndex="0" />
                                        <dx:GridViewDataColumn FieldName="employee_name" CellStyle-CssClass="ellipsis" Caption="Employee Name" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="id_number" CellStyle-CssClass="ellipsis" Caption="ID Number" VisibleIndex="2" />
                                        <dx:GridViewDataColumn FieldName="clocking_date_in" CellStyle-CssClass="ellipsis" Caption="Clock In Date" VisibleIndex="3" />
                                        <dx:GridViewDataColumn FieldName="clocking_hour_in" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="4" Caption="Clock In Time" />
                                        <dx:GridViewDataColumn FieldName="clocking_date_out" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="5" Caption="Clock Out Date" />
                                        <dx:GridViewDataColumn FieldName="clocking_hour_out" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="6" Caption="Clock Out Time" />
                                        <dx:GridViewDataColumn FieldName="time_worked" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="7" Caption="Time Worked " />
                                        <dx:GridViewDataColumn FieldName="is_logged_in" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="8" Caption="Logged In " />
                                        <dx:GridViewDataColumn FieldName="reason" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="9" Caption="Reason" />
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
                            <dx:ASPxSummaryItem DisplayFormat="Total Permanent: {0}" FieldName="count1" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="Total Casuals: {0}" FieldName="count" SummaryType="Sum" />
                        </TotalSummary>

                        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
                    </dx:ASPxGridView>
                    <table class="mt-20">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                    </dx:ASPxGridViewExporter>
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
