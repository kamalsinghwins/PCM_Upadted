<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Discounts.aspx.vb" Inherits="pcm.Website.Discounts" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
                        HeaderText="Discount Report" CssClass="mb-20">
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
                                            <dx:ASPxLabel ID="lblAllBranches" runat="server" Text="All Branches" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxCheckBox ID="chkAll" runat="server" CheckState="Unchecked" Style="float: right">
                                            </dx:ASPxCheckBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                                <ClientSideEvents Click="run" />
                                            </dx:ASPxButton> 
                                        </td>

                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxListBox Width="820px" SelectionMode="Multiple" Height="100px" ID="lstBranches" runat="server" ValueType="System.String"></dx:ASPxListBox>
                    <br />
                    <div style="float: right; margin-right: 202px">
                    </div>

                    <dx:ASPxGridView ID="gvReports" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBinding="gvReports_DataBinding"
                        EnableTheming="True">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Sale Date" FieldName="sale_date" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Sale Time" CellStyle-CssClass="ellipsis" FieldName="sale_time" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Transaction Number" FieldName="transaction_number" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Generated Code" FieldName="generated_code" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Selling Price" FieldName="selling_price" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Discount" FieldName="discount_per_item" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Reason" FieldName="discount_reason" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Quantity" FieldName="quantity" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="true" ShowFilterRow="true" ShowGroupPanel="true" />
                    </dx:ASPxGridView>
                    <table class="mt-20">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px" CssClass="mr-20">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                    </dx:ASPxGridViewExporter>
                    <br />
                    <br />
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
