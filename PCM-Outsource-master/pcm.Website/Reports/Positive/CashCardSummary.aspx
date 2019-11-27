<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CashCardSummary.aspx.vb" Inherits="pcm.Website.CashCardSummary" %>

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

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="350px"
        HeaderText="Cash Card Summary" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Account Opened From Date " Width="200px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtAccountFrom" runat="server" Font-Bold="True" Width="200px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Account Opened To Date" Width="200px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtAccountTo" runat="server" Font-Bold="True" Width="200px" Date="04/09/2013 16:22:30"
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td></td>

                    </tr>
                     <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Transaction From Date" Width="200px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtTransactionFrom" runat="server" Font-Bold="True" Width="200px"  DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Transaction To Date" Width="200px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtTransactionTo" runat="server" Font-Bold="True" Width="200px" 
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>
                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run"></dx:ASPxButton>
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>



    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" KeyFieldName="account_number" OnDataBinding="gvMaster_DataBinding" CssClass="date_panel" Width="98%"
        EnableTheming="True">
         <Settings HorizontalScrollBarMode="Auto" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number"
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name"
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name"
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="EMail Address" FieldName="email_address" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Contact Number" FieldName="contact_number"
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Spent" FieldName="total_spent"
                VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Current Points" FieldName="current_points_balance"
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Points" FieldName="total_points_accrued"
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Card Number" FieldName="card_number"
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Date Opened" FieldName="date_account_opened"
                VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Transaction" FieldName="date_last_transaction"
                VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            

        </Columns>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="gvDetail" runat="server"
                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="sale_date" Caption="Sale Date" VisibleIndex="1" />
                        <dx:GridViewDataColumn FieldName="sale_time" Caption="Sale Time" VisibleIndex="2" />
                        <dx:GridViewDataColumn FieldName="branch_code" VisibleIndex="3" Caption="Transaction Type" />
                        <dx:GridViewDataColumn FieldName="transaction_type" VisibleIndex="4" Caption="Transaction Type" />
                        <dx:GridViewDataTextColumn FieldName="transaction_number" VisibleIndex="5" Caption="Transaction Number">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="transaction_total" VisibleIndex="6" Caption="Transaction Amount">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="transaction_points" VisibleIndex="7" Caption="Points">
                        </dx:GridViewDataTextColumn>
                        
                    </Columns>
                    <SettingsPager PageSize="30">
                    </SettingsPager>
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
         <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Total Spent: {0}" FieldName="total_spent" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Current Points: {0}" FieldName="current_points_balance" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Total Points: {0}" FieldName="total_points_accrued" SummaryType="Sum" />
            
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
        <SettingsBehavior ColumnResizeMode="Control" />

        <SettingsPager PageSize="30">
        </SettingsPager>
          <Settings ShowFooter="True" />
        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
    </dx:ASPxGridView>


    <table>
        <tr>
            <td>
                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <dx:ASPxGridViewExporter ID="Exporter" runat="server" ExportedRowType="Selected">
    </dx:ASPxGridViewExporter>
</asp:Content>