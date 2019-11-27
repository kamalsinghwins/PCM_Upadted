<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="IncomingSMS.aspx.vb" Inherits="pcm.Website.IncomingSMS" %>

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
    <%--   <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="1px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="1pxx">
                </dx:ASPxDockZone>--%>

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

             <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="1px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="1px">
                </dx:ASPxDockZone>


            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
        HeaderText="Incoming SMS Report" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Type of SMS:" Width="120px">
                            </dx:ASPxLabel></td>
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
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
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

  <%--   <dx:ASPxGridView ID="dxGrid1" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="True" OnDataBinding="dxGrid_DataBinding" Width="98%">

          <SettingsPager PageSize="100
              ">
        </SettingsPager>
         <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
            ShowGroupPanel="True" />
     </dx:ASPxGridView>--%>

    <dx:ASPxGridView ID="dxGrid" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding" Width="98%">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Date" FieldName="time_stamp" Width="220px" 
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID Number" FieldName="id_number" 
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name" 
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Salary" FieldName="salary" 
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Staus" FieldName="status" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="SMS Result" FieldName="sms_result" 
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="credit_limit" 
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Received From" FieldName="received_from" 
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Rage #" FieldName="sent_to" 
                VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Original Message" FieldName="original_message" 
                VisibleIndex="10">
            </dx:GridViewDataTextColumn>
           <%-- <dx:GridViewDataTextColumn Caption="Reply SMS" FieldName="reply_sms" 
                VisibleIndex="11">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn Caption="Bureau Call" FieldName="bureau_call" 
                VisibleIndex="12">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="T. Proc. Time" FieldName="total_processing_time" 
                VisibleIndex="13">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="B. Proc. Time" FieldName="bureau_processing_time" 
                VisibleIndex="14">
            </dx:GridViewDataTextColumn>
                    </Columns>
        <SettingsPager PageSize="20">
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
            ShowGroupPanel="True" />
        <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="Messages Processed: {0}" FieldName="time_stamp" SummaryType="Count" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Total Credit Allocated: {0}" FieldName="credit_limit" SummaryType="Sum" />
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </GroupSummary>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Messages Processed: {0}" FieldName="time_stamp" SummaryType="Count" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Total Credit Allocated: {0}" FieldName="creditlimit" SummaryType="Sum" />
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
         <Settings ShowFooter="True" />
    </dx:ASPxGridView>


   <%-- <table>
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
    </dx:ASPxGridViewExporter>--%>
</asp:Content>
