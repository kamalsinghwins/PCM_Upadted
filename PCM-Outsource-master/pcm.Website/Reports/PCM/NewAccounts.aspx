<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="NewAccounts.aspx.vb" Inherits="pcm.Website.NewAccounts" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

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

        .check-with-input {
            float: left;
            margin-right: 10px;
        }

        .input-check-with {
            float: left;
            width: 140px;
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%"
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <div class="left-side-tables" style="width: 100%; float: left;">
                        <fieldset>
                            <legend>New Accounts</legend>
                            <table id="tblContainer0">
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label2" runat="server" Text="Start Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxDateEdit ID="txtStart" runat="server" Width="170px" DisplayFormatString="dd-MM-yyyy" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td class="style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label3" runat="server" Text="End Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxDateEdit ID="txtEnd" runat="server" Width="170px" DisplayFormatString="dd-MM-yyyy" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td class="style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>

                                        &nbsp;

                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="L_40" runat="server" Text="Status">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="style3" colspan="2">
                                        <dx:ASPxComboBox ID="cboStat" runat="server" EnableClientSideAPI="True" ClientInstanceName="cbStat"
                                            ValueType="System.String">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>

                                        <dx:ASPxLabel ID="lblResult" runat="server" Text="Result:" Font-Size="Large"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="style3" colspan="2">
                                        <dx:ASPxCheckBox ID="chkCardIssued" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Card Issued">
                                        </dx:ASPxLabel>

                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                        <%--<dx:ASPxLabel ID="lblCards" runat="server" Text="Issued: 0" Font-Size="Large"></dx:ASPxLabel>--%>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="style3" colspan="2">
                                        <dx:ASPxButton ID="cmdCardIssued" runat="server" Text="Card Issued"></dx:ASPxButton>

                                        <dx:ASPxButton ID="cmdQuery" runat="server" Text="Query"></dx:ASPxButton>

                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3" colspan="6"></td>


                                </tr>


                                <tr>
                                    <td class="auto-style3" colspan="6"></td>


                                </tr>
                                <tr>
                                    <td class="auto-style3" colspan="6"></td>
                                </tr>


                            </table>
                            <div>
                                <dx:ASPxLabel ID="Label" runat="server" Text="Cards Issued shown by clicking the Cards Issued button is not dependant on the selected Status or the date when the account was opened. Cards Issued are ALL cards for the selected date range.">
                                </dx:ASPxLabel>
                                <br />
                                <br />
                                <dx:ASPxLabel ID="LabelCard" runat="server" Text="Cards Issued shown by clicking the Query button are cards that were issued to customers who opened accounts in the selected date range based on the selected Status.">
                                </dx:ASPxLabel>
                            </div>
                        </fieldset>


                    </div>
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
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }


    </script>
</asp:Content>
