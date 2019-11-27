<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CCAuthorisation.aspx.vb" Inherits="pcm.Website.CCAuthorisation" %>


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
                        HeaderText="Task Report" CssClass="date_panel">
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
                                        <td></td>
                                        <td></td>
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

                    <dx:ASPxGridView ID="gvReports" OnDataBinding="gvReport_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
                        EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction" FieldName="tran"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction Number" FieldName="transaction_number"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Amount" FieldName="amount"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Response Code" FieldName="response_code"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Response Text" FieldName="response_text" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Reference" FieldName="reference"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RRN" FieldName="rrn"
                                VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="POS" FieldName="pos"
                                VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Store" FieldName="store"
                                VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Card" FieldName="card"
                                VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Card Number" FieldName="card_name"
                                VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Card Entry" FieldName="card_entry"
                                VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Sign" FieldName="sign"
                                VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Cryptogram Type" FieldName="cryptogram_type"
                                VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Cryptogram" FieldName="cryptogram"
                                VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="APL" FieldName="apl"
                                VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TVR" FieldName="tvr"
                                VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TSI" FieldName="tsi"
                                VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CVM Results" FieldName="cvm_results"
                                VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="IAD" FieldName="iad"
                                VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Data" FieldName="data"
                                VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Receipt" FieldName="receipt"
                                VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Merchant ID" FieldName="merchant_id"
                                VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AID" FieldName="aid"
                                VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction Type" FieldName="transaction_type"
                                VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Sale Date" FieldName="sale_date"
                                VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Sale Time" FieldName="sale_time"
                                VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Crated Date" FieldName="time_stamp"
                                VisibleIndex="28">
                            </dx:GridViewDataTextColumn> 
                            <dx:GridViewDataTextColumn Caption="PIN Statement" FieldName="pin_statement"
                                VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>

                        <Settings ShowFooter="True" HorizontalScrollBarMode="Auto"/>

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
                <dx:ASPxGridViewExporter ID="Exporter" runat="server">
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
