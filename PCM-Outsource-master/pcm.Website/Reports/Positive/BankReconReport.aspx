<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BankReconReport.aspx.vb" Inherits="pcm.Website.BankReconReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
        HeaderText="Cash Card Customers Transactions" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Date:" Width="90px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run"></dx:ASPxButton>
                        </td>
                    </tr>


                </table>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <dx:ASPxGridView ID="gvMaster" runat="server" CssClass="date_panel" EnableTheming="True"
        AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding" Width="98%">
        <Columns>

            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_name"
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cash System" FieldName="cash_positive"
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cash Bank" FieldName="cash_bank"
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cash Diff" FieldName="cash_diff"
                VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="EFT System" FieldName="credit_card_positive"
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="EFT Bank" FieldName="credit_card_bank"
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="EFT Diff" FieldName="credit_card_diff"
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="# EFT System" FieldName="credit_card_transactions_positive"
                VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="# EFT Bank" FieldName="credit_card_transactions_bank"
                VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="# EFT Diff" FieldName="eft_diff"
                VisibleIndex="11">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Voucher System" FieldName="voucher_positive"
                VisibleIndex="12">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Diff" FieldName="total_diff"
                VisibleIndex="13">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Notes" FieldName="commentt"
                VisibleIndex="13">
            </dx:GridViewDataTextColumn>

        </Columns>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="gvDetail" runat="server"
                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="sale_date" Caption="Sales Date" VisibleIndex="1" />
                        <dx:GridViewDataColumn FieldName="cash_positive" Caption="Cash System" VisibleIndex="2" />
                        <dx:GridViewDataColumn FieldName="cash_bank" VisibleIndex="3" Caption="Cash Bank" />
                        <dx:GridViewDataTextColumn FieldName="cash_diff" VisibleIndex="4" Caption="Cash Diff" />
                        <dx:GridViewDataTextColumn FieldName="credit_card_positive" VisibleIndex="5" Caption="EFT System" />
                        <dx:GridViewDataTextColumn FieldName="credit_card_bank" VisibleIndex="6" Caption="EFT Bank" />
                        <dx:GridViewDataTextColumn FieldName="credit_card_diff" VisibleIndex="7" Caption="EFT Diff" />
                        <dx:GridViewDataTextColumn FieldName="credit_card_transactions_positive" VisibleIndex="8" Caption="# EFT System" />
                        <dx:GridViewDataTextColumn FieldName="credit_card_transactions_bank" VisibleIndex="9" Caption="# EFT Bank" />
                        <dx:GridViewDataTextColumn FieldName="eft_diff" VisibleIndex="10" Caption="# EFT Diff" />
                        <dx:GridViewDataTextColumn FieldName="voucher_positive" VisibleIndex="11" Caption="Voucher System" />
                        <dx:GridViewDataTextColumn FieldName="total_diff" VisibleIndex="12" Caption="Total Dif" />
                        <dx:GridViewDataTextColumn FieldName="commentt" VisibleIndex="13" Caption="Notes" />
                       
                    </Columns>

                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
        <SettingsDetail ShowDetailRow="true" />
        <SettingsPager PageSize="200">
        </SettingsPager>
        <Settings ShowFilterRow="True" />
        <Settings ShowFooter="True" />
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Cash System : {0}" FieldName="cash_positive" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Cash Bank: {0}" FieldName="cash_bank" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Cash Diff: {0}" FieldName="cash_diff" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="EFT System: {0}" FieldName="credit_card_positive" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="EFT Bank: {0}" FieldName="credit_card_bank" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="EFT Diff: {0}" FieldName="credit_card_diff" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="# EFT System: {0}" FieldName="credit_card_transactions_positive" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="# EFT Bank: {0}" FieldName="credit_card_transactions_bank" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="# EFT Diff: {0}" FieldName="eft_diff" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Voucher System: {0}" FieldName="voucher_positive" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Total Diff: {0}" FieldName="total_diff" SummaryType="Sum" />

            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
    </dx:ASPxGridView>

    <table>
        <tr>
            <td>
                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
</asp:Content>
