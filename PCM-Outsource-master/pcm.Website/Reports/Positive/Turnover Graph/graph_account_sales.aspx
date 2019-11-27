<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="graph_account_sales.aspx.vb" Inherits="pcm.Website.graph_account_sales" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web.Designer" TagPrefix="dxchartdesigner" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../../js/General/application.js"></script>
    <script type="text/javascript" src="../../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function OnClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cmdRun";

            lp.Show();
            cab.PerformCallback();


        }

        function OnIndexChangecboDateRange(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "DateRange";

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
        OnCallback="ASPxCallback1_Callback">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">

                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                        HeaderText="Account Sales" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                                    ClientInstanceName="lp">
                                </dx:ASPxLoadingPanel>
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblDateRange" runat="server" Text="Date Range">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboDateRange" runat="server" ClientInstanceName="cboDateRange"
                                                CssClass="UpperCase" TextFormatString="{0}" Width="200px">
                                                <ClientSideEvents SelectedIndexChanged="OnIndexChangecboDateRange" />
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Width="200px" DisplayFormatString="yyyy-MM-dd"
                                                EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Width="200px" DisplayFormatString="yyyy-MM-dd"
                                                EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td colspan="2">
                                            <dx:ASPxCheckBox ID="chkPayments" runat="server" CheckState="Unchecked" Text="Payments" Width="100px">
                                            </dx:ASPxCheckBox>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                                <ClientSideEvents Click="OnClick" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <div>
                        <asp:Panel ID="Panel1" CssClass="scrollGraph" runat="server" ScrollBars="Horizontal">
                            <dx:WebChartControl ID="deChart" runat="server" Height="510px" OnDataBinding="deChart_DataBinding" PaletteName="Flow" Width="414px">
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
                                <SeriesSerializable>
                                    <cc1:Series LabelsVisibility="True" Name="Turnover" ArgumentDataMember="branch" ValueDataMembersSerializable="turnover">
                                        <ViewSerializable>
                                            <cc1:SideBySideBarSeriesView BarWidth="0.8">
                                            </cc1:SideBySideBarSeriesView>
                                        </ViewSerializable>
                                        <LabelSerializable>
                                            <cc1:SideBySideBarSeriesLabel ResolveOverlappingMode="Default">
                                                <Border Color="White" />
                                            </cc1:SideBySideBarSeriesLabel>
                                        </LabelSerializable>
                                    </cc1:Series>
                                    <cc1:Series Name="Payments" ArgumentDataMember="branch" ValueDataMembersSerializable="payments">
                                        <ViewSerializable>
                                            <cc1:SideBySideBarSeriesView BarWidth="0.8">
                                            </cc1:SideBySideBarSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                </SeriesSerializable>
                                <Titles>
                                    <cc1:ChartTitle />
                                    <cc1:ChartTitle />
                                    <cc1:ChartTitle />
                                  <%--  <cc1:ChartTitle />
                                    <cc1:ChartTitle />--%>
                                </Titles>
                            </dx:WebChartControl>


                        </asp:Panel>
                    </div>
                </div>
                <br />
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
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="There was an error updating this account. Please contact support."
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
