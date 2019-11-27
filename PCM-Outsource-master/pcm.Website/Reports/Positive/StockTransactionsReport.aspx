<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="StockTransactionsReport.aspx.vb" Inherits="pcm.Website.StockTransactionsReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function run(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run";
            lp.Show();
            cab.PerformCallback();
        }

        function onEnd(s, e) {
            lp.Hide();

        }

        function fromStockcode(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "FromStockCode";
            lp.Show();
            cab.PerformCallback();
        }
        function toStockCode(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "ToStockCode";
            lp.Show();
            cab.PerformCallback();
        }

        function searchStockcodes(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SearchStockCode";
            lp.Show();
            cab.PerformCallback();
        }

        function selectStockcode(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectStockCode";
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
                        HeaderText="Stock Transactions Report" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
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
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblFromStockCode" runat="server" Text="From StockCode" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td colspan="2">
                                            <div style="display: flex">
                                                <dx:ASPxTextBox ID="txtFromStockCode" runat="server" Width="230px"></dx:ASPxTextBox>
                                                <dx:ASPxImage ID="imgSearch" runat="server" ImageUrl="~/images/search.png">
                                                    <ClientSideEvents Click="fromStockcode" />
                                                </dx:ASPxImage>
                                            </div>
                                        </td>
                                        <%--                                        <td> </td>--%>
                                        <td>
                                            <dx:ASPxLabel ID="lblToStockCode" runat="server" Text="To StockCode" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <div style="display: flex">
                                                <dx:ASPxTextBox ID="txtToStockCode" runat="server" Width="230px"></dx:ASPxTextBox>
                                                <dx:ASPxImage ID="imgSearchTo" runat="server" ImageUrl="~/images/search.png">
                                                    <ClientSideEvents Click="toStockCode" />
                                                </dx:ASPxImage>
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblItemCategory1" runat="server" Text="Item Category 1 :" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboItemcat1" Width="250px" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblItemCategory2" runat="server" Text="Item Category 2 :" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboItemcat2" Width="250px" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblItemCategory3" runat="server" Text="Item Category 3 :" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboItemcat3" Width="250px" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" class="auto-style2">

                                            <dx:ASPxCheckBox ID="chkSales" Text="Sales" runat="server">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkCn" runat="server" Text="Credit Notes">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkRefunds" runat="server" Text="Refunds">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkIbtin" runat="server" Text="IBT In's">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkIbtOut" runat="server" Text="IBT Out's">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkStkAdj" runat="server" Text="Stock Adjustments">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkGRV" runat="server" Text="Goods Receieved">
                                            </dx:ASPxCheckBox>
                                            <dx:ASPxCheckBox ID="chkAll" runat="server" Text="All Branches">
                                            </dx:ASPxCheckBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="auto-style2"></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td colspan="3"></td>

                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ID="searchStockcodePopup" ShowCloseButton="True" ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px"
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="StockCode Search"
                                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table border="0" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearch" runat="server" Text="StockCode Search"></dx:ASPxLabel>

                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtSearch" CssClass="UpperCase" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                <ClientSideEvents Click="searchStockcodes" />
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
                                                                <ClientSideEvents Click="selectStockcode" />
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
                    <dx:ASPxListBox Width="793px" SelectionMode="Multiple" Height="229px" ID="lstBranches" runat="server" ValueType="System.String"></dx:ASPxListBox>
                    <br />
                    <div style="float: right; margin-right: 171px">
                        <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                            <ClientSideEvents Click="run" />
                        </dx:ASPxButton>
                    </div>
                    <br />
                    <br />
                </div>
                <asp:HiddenField ID="hdSearchWhat" runat="server" />
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
