<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SizeGridsReport.aspx.vb" Inherits="pcm.Website.SizeGridsReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function run(s, e) {
            e.processOnServer = false;
            popup.Hide();
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run";
            lp.Show();
            cab.PerformCallback();
        }
        function searchStockCodes(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Search";
            lp.Show();
            cab.PerformCallback();
        }
        function selectStockCode(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Select";
            lp.Show();
            cab.PerformCallback();
        }
        function confirm(s, e) {
            e.processOnServer = false;
            lblConfirmation.SetText("Are you sure you want to run it?")
            txtSave.SetVisible(true);
            txtCancel.SetVisible(true);
            popup.Show();
            return
        }
        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();
        }
        function showPopup(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdSearchWhat.ClientID%>').value = "from";
            StockCodePopUp.Show();
        }
        function showStockCodePopup(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdSearchWhat.ClientID%>').value = "to";
            StockCodePopUp.Show();
        }
    </script>
    <style type="text/css">
        .note {
            width: 50% !important;
            float: left;
            padding: 20px !important;
            box-sizing: border-box !important;
            font-size: 14px !important;
            line-height: 24px !important;
        }

        .head {
            color: red;
            font-weight: bold !important;
            font-size: 14px !important;
            line-height: 24px !important;
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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="300px"
                        HeaderText="Size Grid">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Width="120px" Text="From Date">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Width="120px" Text="To Date">
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
                                            <dx:ASPxLabel ID="lblCategory1" runat="server" Width="120px" Text="Category 1">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboCategory1" ClientInstanceName="cboCategory1" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblCategory2" runat="server" Width="120px" Text="Category 2">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboCategory2" ClientInstanceName="cboCategory2" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblCategory3" runat="server" Width="120px" Text="Category3">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboCategory3" ClientInstanceName="cboCategory3" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblSizeGrid" runat="server" Width="120px" Text="Size Grid">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboSizeGrid" ClientInstanceName="cboSizeGrid" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblStartCode" runat="server" Width="120px" Text="Start Code">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtStartCode" ClientInstanceName="txtStartCode" CssClass="UpperCase" runat="server" Width="230">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                                <ClientSideEvents Click="showPopup" />
                                            </dx:ASPxImage>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblEndCode" runat="server" Width="120px" Text="End Code">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtEndCode" ClientInstanceName="txtEndCode" CssClass="UpperCase" runat="server" Width="230">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/images/search.png">
                                                <ClientSideEvents Click="showStockCodePopup" />
                                            </dx:ASPxImage>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td align="right" colspan="2">
                                            <div style="float: right; width: 100%">
                                                <dx:ASPxButton AutoPostBack="false" ID="btnSave" runat="server" Text="Run">
                                                    <ClientSideEvents Click="confirm" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ClientInstanceName="StockCodePopUp" Width="800px" Height="250px"
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
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" UseSubmitBehavior="false" Text="Search" AutoPostBack="false">
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
                                <dx:ASPxPopupControl ID="dxConfirmation" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                                    HeaderText="Confirmation" Width="548px" CloseAction="None" ClientInstanceName="popup"
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
                                                                <dx:ASPxButton ID="txtSave" ClientInstanceName="txtSave" UseSubmitBehavior="false" runat="server" Text="Yes" Width="83px">
                                                                    <ClientSideEvents Click="run" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="txtCancel" ClientInstanceName="txtCancel" runat="server" Text="No" Width="83px">
                                                                    <ClientSideEvents Click="OnClickCancel" />
                                                                </dx:ASPxButton>
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
                    </dx:ASPxRoundPanel>
                    <br />
                    <div>
                        <div style="width: 50%!important; float: left">
                            <dx:ASPxCheckBox ID="chkAll" Text="All Branches" runat="server" CheckState="Unchecked"></dx:ASPxCheckBox>
                            <dx:ASPxListBox Width="100%" Height="300px" SelectionMode="Multiple" ID="lstBranches" runat="server" ValueType="System.String"></dx:ASPxListBox>
                        </div>
                        <div class="note">
                            <label><span class='head'>Please Note:</span> The Stockcodes in this report run according to Alphabetical order. This mean that you can view the Sizes sold for all Codes between AAA and CCC. You are not required to input valid Stockcodes, you could, for example, run the report from 0 to ZZZ. Based on the Database standards data will be sorted as following (as an example:) 1,11,12,13,2,20,3,4,5,a,A,AB,C"</label>
                        </div>
                    </div>
                    <br />

                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <asp:HiddenField ID="hdSearchWhat" runat="server" />
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
