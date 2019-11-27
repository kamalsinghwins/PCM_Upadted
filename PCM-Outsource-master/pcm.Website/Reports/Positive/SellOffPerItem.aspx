<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SellOffPerItem.aspx.vb" Inherits="pcm.Website.SellOffPerItem" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();

        }
        function SearchStockcodes(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SearchStockCodes";
            lp.Show();
            cab.PerformCallback();
        }
        function SelectStockCode(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectStockCode";
            lp.Show();
            cab.PerformCallback();
        }

        function Run(s, e) {
            e.processOnServer = false;
             if (stockcode.GetText() == '') {
                lblConfirmation.SetText("Please enter stockcode")
                popup.Show();
                return
            }
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="cab" Width="100%" OnCallback="ASPxCallback1_Callback"
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
                        HeaderText="Sell Off Report Per Item" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" ClientInstanceName="FromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" ClientInstanceName="ToDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblStockCode" runat="server" Text="StockCode" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtStockcode" ClientInstanceName="stockcode" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td style="padding-left: 20px;"></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2"></td>
                                        <td colspan="4"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2"></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxButton ID="cmdRun" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                                                <ClientSideEvents Click="Run" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="StockCode Search"
                                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table border="0" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearch" runat="server" Text="StockCode "></dx:ASPxLabel>

                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtSearch" CssClass="UpperCase" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                <ClientSideEvents Click="SearchStockcodes" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="12">
                                                            <dx:ASPxListBox Width="760px" ID="lstSearch" runat="server" ValueType="System.String"></dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" Width="100%" AutoPostBack="false">
                                                                <ClientSideEvents Click="SelectStockCode" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxGridView ID="gvSellItem" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBinding="gvSellItem_DataBinding"
                        EnableTheming="True">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Branch Name" CellStyle-CssClass="ellipsis" FieldName="branch_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Province" FieldName="address_line_4" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Post Code" FieldName="address_line_5" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="IBT In" FieldName="ibt_in" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Sales" FieldName="sales" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataSpinEditColumn Caption="Sell Off Ratio" FieldName="SellOffRatio" UnboundType="Decimal" VisibleIndex="7">
                                <PropertiesSpinEdit DisplayFormatString="g">
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>
                        </Columns>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="true" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="ibt_in" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="sales" SummaryType="Sum" />
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
                     <dx:ASPxGridViewExporter ID="Exporter" GridViewID="gvSellItem" runat="server">
                </dx:ASPxGridViewExporter>
                </div>
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
                <dx:ASPxPopupControl ID="dxConfirmation" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="popup"
                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                                    DisappearAfter="1000" PopupAnimationType="Fade">
                                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                            <div>
                                                <div id="Div1">
                                                    <dx:ASPxLabel ID="lblConfirmation" ClientInstanceName="lblConfirmation" runat="server"
                                                        Font-Size="16px">
                                                    </dx:ASPxLabel>

                                                    <table style="float: right">
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr align="right">
                                                            <td align="right">
                                                                
                                                            </td>
                                                            <td>
                                                               
                                                            </td>
                                                        </tr>

                                                    </table>
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
