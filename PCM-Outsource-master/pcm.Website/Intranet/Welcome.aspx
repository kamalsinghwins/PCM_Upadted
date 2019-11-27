<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Welcome.aspx.vb" Inherits="pcm.Website.Welcome" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/Collections/application.js"></script>

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
    <dx:ASPxRoundPanel ID="pnlWarning" runat="server" CssClass="date_panel" HeaderText="Welcome"
                    Visible="True" Width="950px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <img src="../Images/positive_live_logo.png" />
                                 </td>
                                    </tr>
                                 <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                               
                                
                                <tr>
                                    <td>
                                               <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Please select an item from the menu to begin." ForeColor="#3366CC" Font-Size="14px">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
</asp:Content>
