<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="rep_WebServiceLogs.aspx.vb" Inherits="pcm.Website.rep_WebServiceLogs" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>











<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    HeaderText="Transunion Server Logs" CssClass="date_panel">
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
                            <td>
                               
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
            <dx:GridViewDataTextColumn Caption="Date" FieldName="logdate" 
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Time" FieldName="logtime" 
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID Number" FieldName="idnumber" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="accountnumber" 
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Pre-Existing" FieldName="preexisting" 
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Original Staus" FieldName="originalstatus" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Current Status" FieldName="currentstatus" 
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Emprica" FieldName="itcrating" 
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="creditlimit" 
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
                    </Columns>
        <SettingsPager PageSize="20">
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
            ShowGroupPanel="True" />
        <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="Accounts Processed: {0}" FieldName="accountnumber" SummaryType="Count" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Total Credit Allocated: {0}" FieldName="creditlimit" SummaryType="Sum" />
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </GroupSummary>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Accounts Processed: {0}" FieldName="accountnumber" SummaryType="Count" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Total Credit Allocated: {0}" FieldName="creditlimit" SummaryType="Sum" />
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
         <Settings ShowFooter="True" />
    </dx:ASPxGridView>
        <%--<table>
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
    </table>--%>
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
</asp:Content>
