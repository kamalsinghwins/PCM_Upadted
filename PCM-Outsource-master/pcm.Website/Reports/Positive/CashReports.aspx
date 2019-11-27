<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CashReports.aspx.vb" Inherits="pcm.Website.CashReports" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function runAdvanceSummary(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "RunAdvanceSummary";
            lp.Show();
            cab.PerformCallback();
        }

        function runCashUpSummary(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "RunCashUpSummary";
            lp.Show();
            cab.PerformCallback();
        }
        function onEnd(s, e) {
            lp.Hide();

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
                        HeaderText="Cash Reports" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <fieldset>
                                    <legend>Cash-Up Summary</legend>
                                    <table>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCashUpDate" runat="server" Text="From Date:" Width="120px">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="txtSummaryDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8" colspan="2">

                                                <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                                    <ClientSideEvents Click="runCashUpSummary" />
                                                </dx:ASPxButton>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8"></td>
                                            <td class="auto-style8"></td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <br />
                                <br />
                                <fieldset>
                                    <legend>Advanced Cash-Up Report</legend>
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
                                                <dx:ASPxLabel ID="lblAllBranches" runat="server" Text="Select Branch" Width="120px">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cboBranch" DropDownStyle="DropDownList" runat="server" Width="250px" ValueType="System.String">
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td align="right" colspan="3">
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Run">
                                                    <ClientSideEvents Click="runAdvanceSummary" />
                                                </dx:ASPxButton>
                                            </td>

                                        </tr>
                                    </table>
                                </fieldset>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                     <div style="float: right">
                        <dx:ASPxButton ID="btnExportCashSummary" runat="server" Text="Export">
                        </dx:ASPxButton>
                    </div>                 
                    <dx:ASPxGridView ID="gridCashSummary" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                        Styles-Cell-CssClass="textAlignLeft" Width="100%" runat="server" OnDataBinding="gridCashSummary_DataBinding">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Till Number" FieldName="Till Number"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type" FieldName="Type"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Cash" FieldName="Cash"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Cheque" FieldName="Cheque"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Credit Card" FieldName="Credit Card"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Vouchers" FieldName="Vouchers"
                                VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Accounts" FieldName="Accounts"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Totals" FieldName="Totals"
                                VisibleIndex="7">
                            </dx:GridViewDataTextColumn>

                        </Columns>

                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsResizing ColumnResizeMode="Control" />
                        <Settings ShowColumnHeaders="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                    </dx:ASPxGridView>
                    <br />
                    <br />
                    <br />
                    <div style="float: right">
                        <dx:ASPxButton ID="btnExportAdvance" runat="server" Text="Export">
                        </dx:ASPxButton>
                    </div>
                    <dx:ASPxGridView ID="grid" Settings-HorizontalScrollBarMode="Auto" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false" OnDataBinding="grid_DataBinding"
                        Styles-Cell-CssClass="textAlignLeft" ClientInstanceName="grid" Width="100%" runat="server">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Sale Date" FieldName="Sale Date"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Till Number" FieldName="Till Number"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction Type" FieldName="Transaction Type"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction Number" FieldName="Transaction Number"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Cash" FieldName="Cash"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Cheque" FieldName="Cheque" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Credit Card" FieldName="Credit Card"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Voucher" FieldName="Voucher"
                                VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Account" FieldName="Account"
                                VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Payment Details" FieldName="Payment Details"
                                VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsResizing ColumnResizeMode="Control" />
                        <Settings ShowColumnHeaders="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                    </dx:ASPxGridView>
                    <br />
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxGridViewExporter ID="cmdExportCashSummary" GridViewID="gridCashSummary" runat="server">
                </dx:ASPxGridViewExporter>
                <dx:ASPxGridViewExporter ID="cmdExportAdvanceSummary" GridViewID="grid" runat="server">
                </dx:ASPxGridViewExporter>
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

