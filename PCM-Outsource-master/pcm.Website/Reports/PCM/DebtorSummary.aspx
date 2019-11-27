<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="DebtorSummary.aspx.vb" Inherits="pcm.Website.debtor_sum" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <style type="text/css">
        .style3 {
        }

        .style4 {
            width: 104px;
        }

        .style6 {
            width: 422px;
        }

        .auto-style1 {
            width: 104px;
            height: 23px;
        }

        .auto-style3 {
            width: 150px;
        }

        .main_view {
            margin: 20px 0px 0px 20px;
        }

            .main_view .dxtc-strip {
                height: auto !important;
            }

            .main_view table tr td span, .main_view a > .dx-vam {
                text-transform: uppercase;
            }

        .UpperCase {
            text-transform: uppercase;
        }

        .burea_btn {
            margin: 0 5px 10px 0;
        }

        .text-center {
            text-align: center;
        }

        .mb-10 {
            margin-bottom: 10px;
        }

        .mainContainer {
            padding: 10px;
        }

        .dxeBase_Office2010Blue {
            font: 14px Verdana, Geneva, sans-serif;
            font-weight: bold;
        }
        .auto-style9 {
            width: 300px;
        }
        .auto-style10 {
            width: 107px;
        }
    </style>
    <script type="text/javascript">
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
        OnCallback="ASPxCallback1_Callback"
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>

        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>

                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Debtor Summary" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <fieldset>
                                        <legend>Debtors</legend>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label3" runat="server" Text="Active :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblAD" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label7" runat="server" Text="Pending :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblPend" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label1" runat="server" Text="Declined :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblDD" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label9" runat="server" Text="Suspended :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblSusp" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label11" runat="server" Text="Write Off :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblWO" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label13" runat="server" Text="Blocked :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblBlock" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label15" runat="server" Text="Legal :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblLeg" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label12" runat="server" Text="Fraud :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblFraud" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style3" colspan="3" align="right">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <dx:ASPxLabel ID="Label6" runat="server" Text="LCP :">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" class="auto-style9">
                                                    <dx:ASPxLabel ID="lblLCP" runat="server" Text="00">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style3" align="right">
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>

                                    <table style="width: 100%; margin-top: 10px; border-top: 1px solid #ddd;">
                                        <tr>
                                            <td class="auto-style3" colspan="3" align="left">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label5" runat="server" Text="Total Apps :">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td class="auto-style3" align="left" colspan="2">
                                                <dx:ASPxLabel ID="lblTA" runat="server" Text="00">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label4" runat="server" Text="Active Cards :" Visible="false">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td align="left">
                                                <dx:ASPxLabel ID="lblAC" runat="server" Text="00" Visible="false">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td class="auto-style3" align="left">
                                             
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style3" colspan="3" align="left">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3" colspan="3" align="right">
                                                <dx:ASPxButton ID="commandBtn" runat="server" Text="Command" Visible="false">
                                                    <ClientSideEvents Click="cmdBtn_click"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3" colspan="3" align="left">&nbsp;
                                            </td>
                                        </tr>
                                    </table>

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
                                </div>

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </div>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
