<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReportsAgingSummary.aspx.vb" Inherits="pcm.Website.ReportsAgingSummary" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <style type="text/css">
        .date_panel {
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function run(s, e) {
            debugger;
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
                    ClientInstanceName="lp"></dx:ASPxLoadingPanel>
                 <div class="mainContainer">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                    HeaderText="User Transactions Summary" CssClass="date_panel">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Total Outstanding >= " Width="152px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtAmount" runat="server" Width="170px" Text="0">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <dx:ASPxButton ID="cmdRun" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                                            <ClientSideEvents Click="run"/>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <br />
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="date_panel" Text="Outstanding Amounts for ALL accounts"></dx:ASPxLabel>
                <dx:ASPxGridView Width="900px" ID="dxGrid" runat="server" CssClass="date_panel"
                    AutoGenerateColumns="false"  OnDataBinding="dxGrid_DataBinding">
                </dx:ASPxGridView>
                <br />
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="date_panel" Text="Number of Customers to Call"></dx:ASPxLabel>
                <dx:ASPxGridView Width="900px" ID="dxGridCalls" runat="server" CssClass="date_panel"
                    AutoGenerateColumns="false" OnDataBinding="dxGrid_DataBinding_Calls">
                </dx:ASPxGridView>
                <br />
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="date_panel" Text="Number of Customers in each Section"></dx:ASPxLabel>
                <dx:ASPxGridView Width="900px" ID="dxGridSections" runat="server" CssClass="date_panel"
                    AutoGenerateColumns="false" OnDataBinding="dxGrid_DataBinding_Section">
                </dx:ASPxGridView>
                <br />
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="date_panel" Text="Number of Customers ACTIVE with balance <> 0 in each Section "></dx:ASPxLabel>
                <dx:ASPxGridView Width="900px" ID="dxGridSectionsActiveBalance" runat="server" CssClass="date_panel"
                    AutoGenerateColumns="false" OnDataBinding="dxGrid_DataBinding_SectionActiveBalance">
                </dx:ASPxGridView>  
                      </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
