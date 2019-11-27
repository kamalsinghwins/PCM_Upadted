<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CardSummary.aspx.vb" Inherits="pcm.Website.CardSummary" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

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

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            width: 150px;
        }

        .auto-style4 {
            width: 104px;
            height: 100%;
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

        .auto-style8 {
            margin-left: 40px;
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

        .pull-left {
            float: left;
        }

        .pull-right {
            float: right;
        }

        .p-0 {
            padding: 0;
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }

        function checkCardNumber(s, e) {
            debugger;
            if (!txtCardNum.GetValue() || txtCardNum.GetValue() == "") return;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "checkCard";
            lp.Show();
            cab.PerformCallback();
        }
        //function OnKeyDown(s, e) {
        //    if (e.htmlEvent.keyCode == 13) {
        //        checkCardNumber();
        //    }
        //}

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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Single Card Summary" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <table>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblCardNum" runat="server" Text="Card Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtCardNum" runat="server" ClientInstanceName="txtCardNum" Width="170px">
                                                 <%--   <ClientSideEvents KeyDown="OnKeyDown" LostFocus=" checkCardNumber" />--%>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxButton ID="cmdCheck" runat="server" Text="LookUp" CssClass="px-0" AutoPostBack="false">
                                                    <ClientSideEvents Click="checkCardNumber"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblAccountNumber" runat="server" Text="Account Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtAccN" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCustomerName" runat="server" Text="Customer Name">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtName" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCreatedBy" runat="server" Text="Created By">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtCBy" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>

                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblAssignedBy" runat="server" Text="Assigned By">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtAct" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblShopAssistant" runat="server" Text="Shop Assistant">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtShop" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblAssignedAtBranch" runat="server" Text="Shop Assistant">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtBranch" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblDateAssigned" runat="server" Text="Date Assigned">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtDateAssigned" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCreateDate" runat="server" Text="Create Date">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtCreate" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblModifyDate" runat="server" Text="Modify Date">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtModify" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblDateLastUsed" runat="server" Text="Date Last Used">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtDateLastUsed" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblStatus" runat="server" Text="Status">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtStatus" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                    </table>
                                    <br />


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
