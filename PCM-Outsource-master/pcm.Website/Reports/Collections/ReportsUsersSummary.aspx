﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReportsUsersSummary.aspx.vb" Inherits="pcm.Website.ReportsUsersSummary" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
        }
    </style>
</asp:Content>--%>
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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                        HeaderText="User Transactions Summary" CssClass="date_panel">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Date:" Width="90px">
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
                    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding" CssClass="date_panel" Width="98%"
                        EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Username" FieldName="username"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Num of Calls" FieldName="numberofcalls"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Avg Len of Calls" FieldName="averagelengthofcalls" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Total Len of Calls" FieldName="totallengthofcalls"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Num of PTPs" FieldName="numberofptps"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Amount of PTPs" FieldName="amountofptps" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server"
                                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="timestampofaction" Caption="Date / Time" VisibleIndex="0" />
                                        <dx:GridViewDataColumn FieldName="accountnumber" Caption="Account Number" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="usercomment" VisibleIndex="2" Caption="Comment" />
                                        <dx:GridViewDataTextColumn FieldName="actionresult" VisibleIndex="3" Caption="Result">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ptpamount" VisibleIndex="4" Caption="PTP Amount">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ptpdate" VisibleIndex="5" Caption="PTP Date">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="collectionsperiod" VisibleIndex="6" Caption="Col. Period">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="lengthofaction" VisibleIndex="7" Caption="Time Spent">                                        

                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFooter="True" />
                                    <GroupSummary>
                                        <dx:ASPxSummaryItem DisplayFormat="Accounts Processed: {0}" FieldName="accountnumber" SummaryType="Count" />
                                        <dx:ASPxSummaryItem DisplayFormat="PTP Total: {0}" FieldName="ptpamount" SummaryType="Sum" />
                                    </GroupSummary>
                                    <Settings ShowGroupPanel="True" />
                                    <SettingsPager PageSize="20">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <TotalSummary>
                            <dx:ASPxSummaryItem DisplayFormat="Number of Calls: {0}" FieldName="numberofcalls" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="Number of PTPs: {0}" FieldName="numberofptps" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="Amount of PTPs: {0}" FieldName="amountofptps" SummaryType="Sum" />
                            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
                        </TotalSummary>
                        <%--<TotalSummary>
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" />
                </TotalSummary>--%>
                        <SettingsBehavior ColumnResizeMode="Control" />

                        <SettingsPager PageSize="20">
                        </SettingsPager>

                        <Settings ShowFooter="True" />

                        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
                    </dx:ASPxGridView>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="date_panel">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px" CssClass="date_panel">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px" CssClass="date_panel">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                </dx:ASPxGridViewExporter>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
