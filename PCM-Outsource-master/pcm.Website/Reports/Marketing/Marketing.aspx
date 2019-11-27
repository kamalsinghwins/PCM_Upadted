<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Marketing.aspx.vb" Inherits="pcm.Website.Marketing" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>

    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .mb-20 {
            margin-bottom: 20px;
        }

        .ellipsis {
            text-overflow: ellipsis;
            overflow: hidden;
            max-width: 100px !important;
        }

        .mr-20 {
            margin-right: 20px;
        }

        .mt-20 {
            margin-top: 20px;
        }

        .text-transform-capitalize {
            text-transform: capitalize;
        }

        .auto-style2 {
            width: 140px;
        }
    </style>

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
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="600px"
        HeaderText="Export" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td style="width: 200px">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="EMail Addresses" Width="100%">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="cmdRunEMailAddresses" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>


                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Cellphone Numbers (Loyalty Accounts)" Width="100%">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="cmdCellphoneNumbersLoyalty" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>


                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Cellphone Numbers (Can Buy)" Width="100%">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="cmdCellphoneNumbers" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>


                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Cellphone Numbers (All)" Width="100%">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="cmdCellphoneNumbersAll" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>


                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Cellphone By Branch" Width="100%">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" CssClass="pull-left " Width="170px"
                                runat="server" AutoPostBack="false"
                                ValueType="System.String">
                                <ValidationSettings ValidationGroup="save">
                                    <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="cmdCellphoneByBranch" ValidationGroup="save" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>


                    </tr>
                    <tr>
                        <td></td>
                        <td>

                            <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                ValidationGroup="save" Width="100%">
                            </dx:ASPxValidationSummary>
                        </td>
                        <td></td>
                        <td>&nbsp;</td>


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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
