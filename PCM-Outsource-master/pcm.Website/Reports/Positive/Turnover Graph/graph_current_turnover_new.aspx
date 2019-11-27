<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="graph_current_turnover_new.aspx.vb" Inherits="pcm.Website.graph_current_turnover_new" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web.Designer" TagPrefix="dxchartdesigner" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges.Gauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges.Gauges.State" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraCharts.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnIndexChangecboDateRange(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cboDateRangeIndexChanged";

            lp.Show();
            cab.PerformCallback();


        }

        function OnIndexChangecboOrderBy(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cboOrderByIndexChanged";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cmdRun";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickNow(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cmdRunNow";

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
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" Style="float: left;"
                    HeaderText="Turnover" >
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                             <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                     </dx:ASPxLoadingPanel>
                            <table>

                                <tr>
                                    <td>
                                        <dx:ASPxComboBox ID="cboDateRange" runat="server" ClientInstanceName="cboDateRange"
                                            CssClass="UpperCase" TextFormatString="{0}" Width="200px">
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChangecboDateRange" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboOrderBy" runat="server" ClientInstanceName="cboOrderBy"
                                            CssClass="UpperCase" TextFormatString="{0}" Width="200px">
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChangecboOrderBy" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                            <ClientSideEvents Click="OnClick" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkProfit" runat="server" CheckState="Unchecked" Text="Show Profit" Width="100px">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>


                                <tr>
                                    <td> <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date" >
                                </dx:ASPxLabel>
                                        <dx:ASPxDateEdit ID="txtDateFrom" runat="server" Width="200px" DisplayFormatString="yyyy-MM-dd"
                                EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td><dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date">
                                </dx:ASPxLabel>
                                        <dx:ASPxDateEdit ID="txtDateTo" runat="server" Width="200px" DisplayFormatString="yyyy-MM-dd"
                                EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td> <dx:ASPxButton ID="cmdRunNow" runat="server" Text="Run Now">
                                            <ClientSideEvents Click="OnClickNow" />
                                        </dx:ASPxButton></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>


                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <div style="margin-left:50px;">
                    <table>

                        <tr>
                            <td style="text-align:center;"> &nbsp;</td>
                            <td style="text-align:center;">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Turnover">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align:center;">&nbsp;</td>
                            <td style="text-align:center;"><dx:ASPxLabel ID="lblProfit" runat="server" Text="Profit">
                                </dx:ASPxLabel></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxGaugeControl ID="deGauge1" runat="server" BackColor="White" Height="80px" LayoutInterval="6" Value="00.000" Width="250px">
                                    <Gauges>
                                        <dx:DigitalGauge id="gTurnover" AppearanceOff-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#E9EFF3&quot;/&gt;" 
                                            AppearanceOn-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#0F89B8&quot;/&gt;" 
                                            Bounds="0, 0, 250, 80" DigitCount="5" Name="digitalGauge8" Padding="26, 20, 26, 20" Text="00.000">
                                            <backgroundlayers>
                                                <dx:DigitalBackgroundLayerComponent AcceptOrder="-1000" BottomRight="265.8125, 99.9625" 
                                                    Name="digitalBackgroundLayerComponent1" ShapeType="Style22" TopLeft="26, 0" ZOrder="1000" />
                                            </backgroundlayers>
                                        </dx:DigitalGauge>
                                    </Gauges>
                                    <LayoutPadding All="0" Bottom="0" Left="0" Right="0" Top="0" />
                                </dx:ASPxGaugeControl>
                            </td>
                            <td>&nbsp;</td>
                            <td><dx:ASPxGaugeControl ID="dProfit" runat="server" BackColor="White" Height="80px" LayoutInterval="6" Value="0.000.000" Width="250px">
                                    <Gauges>
                                        <dx:DigitalGauge id="gProfit" AppearanceOff-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#E9EFF3&quot;/&gt;" 
                                            AppearanceOn-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#0F89B8&quot;/&gt;" 
                                            Bounds="0, 0, 150, 80" DigitCount="5" Name="digitalGauge8" Padding="26, 20, 26, 20" Text="00.000">
                                            <backgroundlayers>
                                                <dx:DigitalBackgroundLayerComponent AcceptOrder="-1000" BottomRight="265.8125, 99.9625" 
                                                    Name="digitalBackgroundLayerComponent1" ShapeType="Style22" TopLeft="26, 0" ZOrder="1000" />
                                            </backgroundlayers>
                                        </dx:DigitalGauge>
                                    </Gauges>
                                    <LayoutPadding All="0" Bottom="0" Left="0" Right="0" Top="0" />
                                </dx:ASPxGaugeControl></td>
                        </tr>
                    </table>
                   
                    

                </div>
                <br />
             <div>
                   <asp:Panel CssClass="scrollGraph" ID="Panel1" runat="server" ScrollBars="Horizontal">
                 <dx:WebChartControl ID="deChart" runat="server" Height="510px" OnDataBinding="deChart_DataBinding" PaletteName="Flow" Width="414px">
                     <diagramserializable>
                         <cc1:XYDiagram>
                             <axisx visibleinpanesserializable="-1">
                                 <range alwaysshowzerolevel="True" sidemarginsenabled="True" />
                             </axisx>
                             <axisy visibleinpanesserializable="-1">
                                 <range alwaysshowzerolevel="True" sidemarginsenabled="True" />
                             </axisy>
                         </cc1:XYDiagram>
                     </diagramserializable>
                     <seriesserializable>
                         <cc1:Series LabelsVisibility="True" Name="Series 1" ArgumentDataMember="branch" ValueDataMembersSerializable="turnover">
                             <viewserializable>
                                 <cc1:SideBySideBarSeriesView BarWidth="0.8">
                                 </cc1:SideBySideBarSeriesView>
                             </viewserializable>
                             <labelserializable>
                                 <cc1:SideBySideBarSeriesLabel ResolveOverlappingMode="Default">
                                     <border color="White" />
                                 </cc1:SideBySideBarSeriesLabel>
                             </labelserializable>
                         </cc1:Series>
                         <cc1:Series Name="Series 2" ArgumentDataMember="branch" ValueDataMembersSerializable="profit">
                             <viewserializable>
                                 <cc1:SideBySideBarSeriesView BarWidth="0.8">
                                 </cc1:SideBySideBarSeriesView>
                             </viewserializable>
                         </cc1:Series>
                     </seriesserializable>
                 </dx:WebChartControl>
                       </asp:Panel>

             </div>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
