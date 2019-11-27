<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BestSellers.aspx.vb" Inherits="pcm.Website.BestSellers" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">   
        .dxgvCSD  
    {  
        height: auto !important;  
        max-height: 600px !important;  
    }
    </style>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }

        function run(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run";
            lp.Show();
            cab.PerformCallback();
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="cab" Width="100%" OnCallback="ASPxCallbackPanel1_Callback"
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                        HeaderText="Best Sellers" CssClass="date_panel">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblmastercode" runat="server" Text="Mastercode"></dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxCheckBox ID="chkmastercode" runat="server"></dx:ASPxCheckBox>

                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run">
                                                <ClientSideEvents Click="run" />
                                            </dx:ASPxButton>
                                        </td>

                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <br />
                    <dx:ASPxGridView ID="gvMaster" runat="server" Width="900px" AutoGenerateColumns="false"
                        OnDataBinding="gvMaster_DataBinding">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="stockcode" Caption="Stockcode" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="2">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="estimated_calc" Caption="GP% on Est." VisibleIndex="3" UnboundExpression="(selling-estimated)/selling" UnboundType="Decimal">
                                <PropertiesTextEdit DisplayFormatString="p" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="average_calc" Caption="GP% on Avg." VisibleIndex="4" UnboundExpression="(selling-average)/selling" UnboundType="Decimal">
                                <PropertiesTextEdit DisplayFormatString="p" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server" AutoGenerateColumns="false"
                                    CssClass="date_panel" OnDataBinding="gvDetail_DataBinding">
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="sale_date" Caption="Sale Date" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="branch_name" Caption="Branch Name" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="transaction_type" Caption="Transaction Type" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="quantity" Caption="Quantity" VisibleIndex="5">
                                            <PropertiesTextEdit DisplayFormatString="f" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="selling_price" Caption="Selling Price Total" VisibleIndex="6">
                                            <PropertiesTextEdit DisplayFormatString="f" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFooter="true" />
                                    <TotalSummary>
                                        <dx:ASPxSummaryItem FieldName="quantity" SummaryType="Sum" />
                                        <dx:ASPxSummaryItem FieldName="selling_price" SummaryType="Sum" />
                                    </TotalSummary>
                                    <Settings ShowGroupPanel="true" />
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsPager Mode="EndlessPaging" PageSize="20"></SettingsPager>
                        <SettingsDetail AllowOnlyOneMasterRowExpanded="false" ShowDetailRow="true" ExportMode="Expanded" />
                        <Settings ShowGroupPanel="true" />
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
                </div>

                <dx:ASPxGridViewExporter ID="gridexporter" runat="server">
                </dx:ASPxGridViewExporter>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="Error"
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
