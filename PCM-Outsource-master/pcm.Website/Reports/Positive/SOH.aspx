<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SOH.aspx.vb" Inherits="pcm.Website.SOH" %>

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
        function onChange(s, e) {
            e.processOnServer = false
            if (cboType.GetText() == "Range") {
                txtFromStockCode.SetVisible(true)
                txtToStockCode.SetVisible(true)
                lblFromStockCode.SetVisible(true)
                lblToStockCode.SetVisible(true)
                FromStockPopUp.SetVisible(true)
                ToStockPopUp.SetVisible(true)
                txtFromStockCode.SetText("")
                txtToStockCode.SetText("")
            }
            else {
                txtFromStockCode.SetVisible(false)
                txtToStockCode.SetVisible(false)
                lblFromStockCode.SetVisible(false)
                lblToStockCode.SetVisible(false)
                FromStockPopUp.SetVisible(false)
                ToStockPopUp.SetVisible(false)
            }
        }
        
        function openFromStockCodePopUp(s, e) {
            e.processOnServer = false
            document.getElementById('<%=hdWhichStockCode.ClientID%>').value = "FromStockCode";
            stockCodePopUp.Show()

        }
        function openToStockCodePopUp(s, e) {
            e.processOnServer = false
            document.getElementById('<%=hdWhichStockCode.ClientID%>').value = "ToStockCode";
            stockCodePopUp.Show()
        }
        function searchStockCodes(s, e) {
            e.processOnServer = false
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Search"
            lp.Show();
            cab.PerformCallback();
        }
        function selectStockCode(s, e) {
            e.processOnServer = false
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Select"
            lp.Show();
            cab.PerformCallback();
        }
        function run(s, e) {
            e.processOnServer = false
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run"
            lp.Show();
            cab.PerformCallback();
        }
        function showHide(s, e) {

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
                        HeaderText="Stock On Hand" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblQuantities" runat="server" Text="Quantities" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboQuantities" runat="server" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblType" runat="server" Text="Type" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboType" ClientInstanceName="cboType" runat="server" ValueType="System.String">
                                                <ClientSideEvents SelectedIndexChanged="onChange" />
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFrom" runat="server" Text="From" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxComboBox ID="cboFrom" runat="server" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 20px;">
                                            <dx:ASPxLabel ID="lblMasterCode" runat="server" Text="MasterCode" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxCheckBox ID="chkMasterCode" runat="server"></dx:ASPxCheckBox>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblBlocked" runat="server" Text="Blocked" Width="120px">
                                            </dx:ASPxLabel>
                                            <dx:ASPxCheckBox ID="chkBlocked" runat="server"></dx:ASPxCheckBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblFromStockCode" ClientInstanceName="lblFromStockCode" runat="server" Text="From StockCode" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtFromStockCode" AutoPostBack="false" ClientInstanceName="txtFromStockCode" CssClass="UpperCase" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="FromStockPopUp" ClientInstanceName="FromStockPopUp" runat="server" ImageUrl="~/images/search.png">
                                                <ClientSideEvents Click="openFromStockCodePopUp" />
                                            </dx:ASPxImage>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToStockCode" ClientInstanceName="lblToStockCode" runat="server" Text="To StockCode" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtToStockCode" AutoPostBack="false" ClientInstanceName="txtToStockCode" CssClass="UpperCase" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="ToStockPopUp" ClientInstanceName="ToStockPopUp" runat="server" ImageUrl="~/images/search.png">
                                                <ClientSideEvents Click="openToStockCodePopUp" />
                                            </dx:ASPxImage>
                                        </td>
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
                                                <ClientSideEvents Click="run" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>

                                <dx:ASPxPopupControl ClientInstanceName="stockCodePopUp" Width="800px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
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
                                                                <ClientSideEvents Click="searchStockCodes" />
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
                                                                <ClientSideEvents Click="selectStockCode" />
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

                    <dx:ASPxCheckBox ID="chkAll" Text="All Branches" runat="server" CheckState="Unchecked"></dx:ASPxCheckBox>
                    <dx:ASPxListBox Width="820px" SelectionMode="Multiple" Height="400px" ID="lstBranches" runat="server" ValueType="System.String"></dx:ASPxListBox>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <asp:HiddenField ID="hdWhichStockCode" runat="server" />
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
