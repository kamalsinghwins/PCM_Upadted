<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReportsCollectionAdmin.aspx.vb" Inherits="pcm.Website.ReportsPTP" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="cab" Width="100%" OnCallback="ASPxCallback1_Callback"
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                    HeaderText="Admin Report" CssClass="date_panel">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Status:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox runat="server" TextFormatString="{0}" Width="200px" ClientInstanceName="cboStatus" ID="cboStatus">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>

                                </tr>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Date &gt;=" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="txtFromDate" runat="server" Date="04/09/2013 16:21:55"
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
                                    <td>
                                        <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run">
                                            <ClientSideEvents Click="run" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>

                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <div>
                    <dx:ASPxGridView ID="dxGrid" OnDataBinding="dxGrid_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
                        EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Total" FieldName="total"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Current Balance" FieldName="current_balance"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p30" FieldName="p30" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p60" FieldName="p60"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p90" FieldName="p90"
                                VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p120" FieldName="p120"
                                VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p150" FieldName="p150"
                                VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Current Contact Level" FieldName="current_contact_level"
                                VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Last Payment Date" FieldName="date_of_last_payment"
                                VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="True" HorizontalScrollBarMode="Auto" />

                    </dx:ASPxGridView>
                </div>
                <div>
                    <dx:ASPxGridView ID="grdPTP" OnDataBinding="grdPTP_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
                        EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Total" FieldName="total"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Current Balance" FieldName="current_balance"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p30" FieldName="p30" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p60" FieldName="p60"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p90" FieldName="p90"
                                VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p120" FieldName="p120"
                                VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="p150" FieldName="p150"
                                VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PTP Date" FieldName="ptp_date"
                                VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PTP Amount" FieldName="ptp_amount"
                                VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Username" FieldName="username"
                                VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="True" HorizontalScrollBarMode="Auto" />

                    </dx:ASPxGridView>
                </div>
                <div>
                    <dx:ASPxGridView ID="grdContactLevel" OnDataBinding="grdContactLevel_DataBinding" CssClass="date_panel" runat="server" AutoGenerateColumns="False" Width="50%" EnableTheming="True">

                        <Columns>
                            <dx:GridViewDataTextColumn Width="50%" Caption="Current Contact Level" FieldName="current_contact_level"
                                VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Width="50%" Caption="Count" FieldName="count"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="True" HorizontalScrollBarMode="Auto" />

                    </dx:ASPxGridView>
                </div>
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <asp:HiddenField ID="hdWhichButton" runat="server" />
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
    <dx:ASPxGridViewExporter ID="GridExporter2" runat="server" GridViewID="grdContactLevel" />
</asp:Content>
