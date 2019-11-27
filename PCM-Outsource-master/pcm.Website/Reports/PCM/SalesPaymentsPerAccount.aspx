<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SalesPaymentsPerAccount.aspx.vb" Inherits="pcm.Website.SalesPaymentsPerAccount" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel
        {}
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
    HeaderText="Sales / Payments By Account" CssClass="date_panel">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Account Opened From Date:" Width="180px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Account Opened To Date:" Width="180px">
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
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Sales From Date:" Width="180px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtSalesFrom" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Sales To Date:" Width="180px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtSalesTo" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                    DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td></td>

                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Payments From Date:" Width="180px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtPaymentsFrom" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Payments To Date:" Width="180px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtPaymentsTo" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                    DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td></td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Running this report for a period longer than a month for Accounts Opened will take a long time to run!" Width="600px">
                                </dx:ASPxLabel>
                            </td>
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
   <dx:ASPxGridView ID="dxGrid" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding" Width="98%">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number" Width="220px" 
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Number of Sales" FieldName="number_of_sales" 
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Number of Payments" FieldName="number_of_payments" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Sales Total" FieldName="sales" 
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Payments Total" FieldName="payments" 
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="ITC Rating" FieldName="itc_rating" 
                VisibleIndex="5">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Orginal Credit Limit" FieldName="original_credit_limit" 
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="current_credit_limit" 
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Current Balance" FieldName="balance" 
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" 
                VisibleIndex="9">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_name" 
                VisibleIndex="10">
            </dx:GridViewDataTextColumn>
                    </Columns>
        <SettingsPager PageSize="50">
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True"  ShowGroupPanel="True"  />
        
       
       <Settings ShowFooter="True" />
        <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="Sales: {0}" FieldName="sales" SummaryType="Sum" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Payments: {0}" FieldName="payments" SummaryType="Sum" />
            
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </GroupSummary>          
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Number of Sales: {0}" FieldName="number_of_sales" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="Number of Payments: {0}" FieldName="number_of_payments" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="Total Sales: {0}" FieldName="sales" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="Total Payments: {0}" FieldName="payments" SummaryType="Sum" />
        </TotalSummary>

                    
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
