<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="AverageCashSale.aspx.vb" Inherits="pcm.Website.AverageCashSale" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>

    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }

        function GetTransactions(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "GetCashTransactions";

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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback"
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
                        HeaderText="Average Cash Sale" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td align="right">
                                            <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                                <ClientSideEvents Click="GetTransactions" />
                                            </dx:ASPxButton>
                                        </td>

                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="lblSum" runat="server" EncodeHtml="False" Text="<b> Sum:</b>" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="lblTotalSum" Text="0" runat="server" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxLabel ID="lblCount" runat="server" EncodeHtml="False" Text="<b>Count:</b>" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="lblTotalCount" Text="0" runat="server" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxLabel ID="lblAverage" EncodeHtml="False" runat="server" Text="<b>Average:</b>" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="lblTotalAverage" Text="0" runat="server" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel CssClass="scrollGraph" ID="Panel1" runat="server" ScrollBars="Horizontal">
                        <dx:WebChartControl ID="transactionChart" runat="server" Height="510px" CrosshairEnabled="True" OnDataBinding="transactionChart_DataBinding" PaletteName="Flow" Width="414px">
                            <DiagramSerializable>
                                <cc1:XYDiagram>
                                    <AxisX VisibleInPanesSerializable="-1">
                                        <Range AlwaysShowZeroLevel="True" SideMarginsEnabled="True" />
                                    </AxisX>
                                    <AxisY VisibleInPanesSerializable="-1">
                                        <Range AlwaysShowZeroLevel="True" SideMarginsEnabled="True" />
                                    </AxisY>
                                </cc1:XYDiagram>
                            </DiagramSerializable>

                            <Legend Name="Default Legend"></Legend>
                            <SeriesSerializable>
                                <cc1:Series LabelsVisibility="False" Name="Sum" ArgumentDataMember="sale_date" ValueDataMembersSerializable="sum">
                                    <ViewSerializable>
                                        <cc1:SideBySideBarSeriesView BarWidth="0.8"></cc1:SideBySideBarSeriesView>
                                    </ViewSerializable>
                                </cc1:Series>
                                <cc1:Series LabelsVisibility="False" Name="Count" ArgumentDataMember="sale_date" ValueDataMembersSerializable="count">
                                    <ViewSerializable>
                                        <cc1:SideBySideBarSeriesView BarWidth="0.8" PaneName="Pane 1" AxisYName="secondaryAxisY1"></cc1:SideBySideBarSeriesView>
                                    </ViewSerializable>
                                </cc1:Series>
                                <cc1:Series LabelsVisibility="False" Name="Average" ArgumentDataMember="sale_date" ValueDataMembersSerializable="avg">
                                    <ViewSerializable>
                                        <cc1:SideBySideBarSeriesView BarWidth="0.8" PaneName="Pane 2" AxisYName="secondaryAxisY2"></cc1:SideBySideBarSeriesView>
                                    </ViewSerializable>
                                </cc1:Series>
                            </SeriesSerializable>
                            <CrosshairOptions ArgumentLineColor="DeepSkyBlue" ArgumentLineStyle-Thickness="2"
                                ShowOnlyInFocusedPane="False">
                            </CrosshairOptions>
                            <Legend AlignmentHorizontal="Center" Direction="LeftToRight" AlignmentVertical="BottomOutside"></Legend>
                            <BorderOptions Visibility="False" />
                            <Titles>
                                <cc1:ChartTitle Text="Total Count: 0"></cc1:ChartTitle>
                                <cc1:ChartTitle Text="Average Cash Sales" Dock="Bottom"></cc1:ChartTitle>
                            </Titles>
                            <DiagramSerializable>
                                <cc1:XYDiagram PaneDistance="4">
                                    <AxisX Title-Text="Date" VisibleInPanesSerializable="1" Interlaced="True">
                                        <DateTimeScaleOptions MeasureUnit="Day" GridAlignment="Day" AutoGrid="False" GridSpacing="6" />
                                    </AxisX>
                                    <AxisY Title-Text="Sum" Title-Visibility="True" VisibleInPanesSerializable="-1">
                                        <WholeRange AlwaysShowZeroLevel="False"></WholeRange>
                                        <GridLines MinorVisible="True"></GridLines>
                                    </AxisY>
                                    <SecondaryAxesY>
                                        <cc1:SecondaryAxisY AxisID="0" Alignment="Near" Title-Text="Count" Title-Visibility="True" VisibleInPanesSerializable="0" Name="secondaryAxisY1">
                                            <WholeRange AlwaysShowZeroLevel="False"></WholeRange>
                                            <GridLines Visible="True" MinorVisible="True"></GridLines>
                                        </cc1:SecondaryAxisY>
                                        <cc1:SecondaryAxisY AxisID="1" Alignment="Near" Title-Text="Average" Title-Visibility="True" VisibleInPanesSerializable="1" Name="secondaryAxisY2">
                                            <WholeRange AlwaysShowZeroLevel="False"></WholeRange>
                                            <GridLines Visible="True" MinorVisible="True"></GridLines>
                                        </cc1:SecondaryAxisY>
                                    </SecondaryAxesY>
                                    <DefaultPane Weight="2"></DefaultPane>
                                    <Panes>
                                        <cc1:XYDiagramPane PaneID="0" Name="Pane 1"></cc1:XYDiagramPane>
                                        <cc1:XYDiagramPane PaneID="1" Name="Pane 2"></cc1:XYDiagramPane>
                                    </Panes>
                                </cc1:XYDiagram>
                            </DiagramSerializable>

                        </dx:WebChartControl>
                    </asp:Panel>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2" class="text-center">
                                    <dx:ASPxLabel ID="lblError" runat="server"
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
