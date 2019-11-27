<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="graph_segments.aspx.vb" Inherits="pcm.Website.graph_segments" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
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
                    <div style="width: 100%">
                        <div style="width: 50%">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" Height="490px" Style="float: left;"
                                HeaderText="Segments">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                                            ClientInstanceName="lp">
                                        </dx:ASPxLoadingPanel>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblDateRange" runat="server" Text="Date Range" Width="120px">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cboDateRange" runat="server"
                                                        CssClass="UpperCase" Width="200px">
                                                        <ClientSideEvents SelectedIndexChanged="OnIndexChangecboDateRange" />
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td rowspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblSegment" runat="server" Text="Segments" Width="90px">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cboSegments" runat="server"
                                                        CssClass="UpperCase" TextFormatString="{0}" Width="200px">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="All Branches" Width="90px">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxCheckBox ID="chkAll" runat="server" CheckState="checked" Text="" Width="100px">
                                                    </dx:ASPxCheckBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="200px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="200px"
                                                        DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td>&nbsp;</td>

                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <dx:ASPxButton ID="cmdRun" Style="float: right" runat="server" Text="Run">
                                                        <ClientSideEvents Click="OnClick" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>&nbsp;</td>

                                            </tr>
                                        </table>

                                        <dx:ASPxListBox ID="lstBranches" runat="server" Font-Names="Verdana" Font-Size="Small"
                                            Height="250px" SelectionMode="Multiple"
                                            TabIndex="6">
                                        </dx:ASPxListBox>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                        <div style="width: 50%; float: right">
                            <table style="width: 100%">
                                <tr>
                                    <asp:Panel CssClass="scrollGraph" ID="Panel1" runat="server" ScrollBars="Horizontal">
                                        <asp:Chart ID="chartSegments" runat="server" BackColor="#d4e2f4" BackGradientStyle="TopBottom"
                                            BackSecondaryColor="White" BorderColor="26, 59, 105" BorderlineDashStyle="Solid"
                                            BorderWidth="2" Palette="BrightPastel" Width="1000px" Height="480px">

                                            <Titles>
                                                <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                    ShadowColor="32, 0, 0, 0" ShadowOffset="3" Text="Date Range">
                                                </asp:Title>
                                            </Titles>
                                            <Legends>
                                                <asp:Legend BackColor="Transparent" Enabled="False" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                    IsTextAutoFit="False" Name="Default">
                                                </asp:Legend>
                                            </Legends>
                                            <BorderSkin BackColor="Transparent" PageColor="Transparent"
                                                SkinStyle="Emboss" />
                                            <Series>
                                                <asp:Series BorderColor="180, 26, 59, 105" ChartArea="ChartArea1" Color="220, 65, 140, 240"
                                                    CustomProperties="DrawingStyle=Cylinder" Name="Series1" ToolTip="#VALX: #VAL"
                                                    XValueMember="segment" YValueMembers="value">
                                                </asp:Series>
                                            </Series>
                                            <ChartAreas>
                                                <asp:ChartArea BackColor="WhiteSmoke" BackSecondaryColor="White" BorderColor="64, 64, 64, 64"
                                                    Name="ChartArea1" ShadowColor="Transparent">
                                                    <Area3DStyle Enable3D="True" Inclination="15" IsClustered="False" IsRightAngleAxes="False"
                                                        PointGapDepth="0" Rotation="10" WallWidth="0" />
                                                </asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </asp:Panel>
                                </tr>
                            </table>
                        </div>
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
