﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReportsPaymentsAndContacts.aspx.vb" Inherits="pcm.Website.ReportsPaymentsAndContacts" %>

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
        HeaderText="Payments VS Contacts Report" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                          <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                            </td>
                        <td>
                                 <dx:ASPxDateEdit ID="txtFromDate" runat="server" Date="04/09/2013 16:21:55" 
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Font-Bold="True" UseMaskBehavior="True" Width="200px">
                            </dx:ASPxDateEdit></td>
                        <td>                                       



                                    </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>

                    </tr>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="To Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Date="04/09/2013 16:21:55" 
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Font-Bold="True" UseMaskBehavior="True" Width="200px">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td></td>
                    </tr>

                    <tr>
                        <td></td>
                        <td> <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run"></dx:ASPxButton></td>
                        <td></td>
                        <td></td>
                        <td>
                           
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

     <dx:ASPxGridView ID="dxGrid" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding" Width="98%">
         <Columns>
            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number" 
                VisibleIndex="0">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Date" FieldName="sale_date" 
                VisibleIndex="1">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Time" FieldName="sale_time" VisibleIndex="2">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Reference Number" FieldName="reference_number" 
                VisibleIndex="3">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Transaction Amount" FieldName="transaction_amount" 
                VisibleIndex="4">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Called On" FieldName="timestamp_of_contact" VisibleIndex="5">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Result of Call" FieldName="result_of_action" 
                VisibleIndex="6">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="PTP Amount" FieldName="ptp_amount" 
                VisibleIndex="7">
                 <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="PTP Date" FieldName="ptp_date" 
                VisibleIndex="8">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Username" FieldName="username" 
                VisibleIndex="9">
                <CellStyle Wrap="False" />
            </dx:GridViewDataTextColumn>
           
        </Columns>
          <SettingsPager PageSize="100
              ">
        </SettingsPager>
         <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
            ShowGroupPanel="True" ShowHeaderFilterButton="true" />
         <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="Accounts Processed: {0}" FieldName="account_number" SummaryType="Count" />
             <dx:ASPxSummaryItem DisplayFormat="Payment Total: {0}" FieldName="transaction_amount" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="PTP Total: {0}" FieldName="ptp_amount" SummaryType="Sum" />
            
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </GroupSummary>
         
     </dx:ASPxGridView>




    <table>
    <tr>
    <td>
       <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF"  Width="164px" CssClass="date_panel">
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

