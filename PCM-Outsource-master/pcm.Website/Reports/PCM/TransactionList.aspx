﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="TransactionList.aspx.vb" Inherits="pcm.Website.TransactionList" %>

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
    </style>

    <script type="text/javascript">
        function SubmitForm(s, e) {
            debugger;
            e.processOnServer = false;
            //do validation here
            if (!ASPxClientEdit.ValidateGroup("ErrorGroup")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CreateSurvey";

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
                            <legend>Daily Transactions</legend>
                            <table>

                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label2_0" runat="server" Text="Start Date"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="StartD" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>

                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label3" runat="server" Text="End Date"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="EndD" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkPur" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkPurLabel" runat="server" Text="Purchases"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkGPur" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkGPurLabel" runat="server" Text="Gift Card Purchases"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkPay" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkPayLabel" runat="server" Text="Payments"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkGPay" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkGPayLabel" runat="server" Text="Gift Card Payments"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkCn" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkCnLabel" runat="server" Text="Credit Notes"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkGCn" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkGCnLabel" runat="server" Text="Gift Card Credit Notes"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkLost" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkLostLabel" runat="server" Text="Lost Card Protection"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkInt" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkIntLabel" runat="server" Text="Interest"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkJCred" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkJCredLabel" runat="server" Text="Journal Credit - Balance Affected"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkJDeb" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkJDebLabel" runat="server" Text="Journal Debit - Balance Affected"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkJCredB" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkJCredBLabel" runat="server" Text="Journal Credit - Balance Not Affected"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkJDebN" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkJDebNLabel" runat="server" Text="Journal Debit - Balance Not Affected"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="2">
                                        <dx:ASPxCheckBox ID="chkBadD" runat="server"></dx:ASPxCheckBox>
                                        <dx:ASPxLabel ID="chkBadDLabel" runat="server" Text="Bad Debt Write off"></dx:ASPxLabel>
                                    </td>


                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Note: Selecting no Transactions will run ALL Transactions"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">

                                        <dx:ASPxButton ID="cmdView" runat="server" Text="View"></dx:ASPxButton>
                                    </td>

                                </tr>
                            </table>
                        </fieldset>


                    </div>
                </div>
                <table style="width: 100%">
                    <tr>
                        <td class="auto-style3"></td>
                        <td>&nbsp;
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style3"></td>
                        <td>&nbsp;
                                   
                        </td>
                        <td></td>
                        <td align="right" class="auto-style8" colspan="2"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style3"></td>
                        <td>&nbsp;
                                 
                        </td>
                        <td></td>
                        <td align="right" class="auto-style8"></td>
                        <td align="right">
                            <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" colspan="5">
                            <dx:ASPxGridView ID="grdTransactions" runat="server" AutoGenerateColumns="False" OnDataBinding="grdTransactions_DataBinding"
                                Width="100%">
                                <SettingsPager PageSize="100" />

                                <SettingsBehavior AllowSelectByRowClick="True" AllowSort="True" />
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True" ShowFilterBar="Visible" ShowGroupPanel="True" />
                                <Columns>

                                    <dx:GridViewDataTextColumn FieldName="tDate" Caption="Date" VisibleIndex="1" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tTime" Caption="Time" VisibleIndex="2" />
                                    <dx:GridViewDataTextColumn FieldName="tType" Caption="Transaction Type" VisibleIndex="3" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tReference" Caption="Reference" VisibleIndex="4" />
                                    <dx:GridViewDataTextColumn FieldName="tUser" Caption="User" VisibleIndex="5" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tAccount_Number" Caption="Account" VisibleIndex="6" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tAmount" Caption="Amount" VisibleIndex="7" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tPeriod" Caption="Period" VisibleIndex="8" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tAuth_Code" Caption="Auth Code" VisibleIndex="9" Settings-AllowAutoFilter="True" />
                                    <dx:GridViewDataTextColumn FieldName="tNotes" Caption="Notes" VisibleIndex="10" Settings-AllowAutoFilter="True" />

                                </Columns>

                             
                                <SettingsPager PageSize="50">
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True"
                                    ShowGroupPanel="True" />
                                <Settings ShowFooter="True" />
                                <TotalSummary>
                                    <dx:ASPxSummaryItem FieldName="tAmount" SummaryType="Sum" />
                                </TotalSummary>
                            </dx:ASPxGridView>
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-style3"></td>
                        <td>&nbsp;
                                 
                        </td>
                        <td></td>
                        <td align="right" class="auto-style8" colspan="2"></td>
                        <td></td>
                    </tr>

                </table>
                <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                </dx:ASPxGridViewExporter>
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
