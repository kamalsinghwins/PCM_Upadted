<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BadDebtByStore.aspx.vb" Inherits="pcm.Website.BadDebtByStore" %>


<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
        }

        .auto-style2 {
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
        HeaderText="Bad Debt By Store" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td class="auto-style2" colspan="2">
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Accounts Opened Date:" Width="218px">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxImage ID="imgQ" runat="server" Height="20px" Width="20px" ImageUrl="~/Images/faq.png" ShowLoadingImage="True">
                            </dx:ASPxImage>
                            <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="150px"
                                MaxWidth="800px" MaxHeight="250px" MinHeight="150px" MinWidth="150px" ID="pcMain"
                                ShowFooter="False" PopupElementID="imgQ" HeaderText="Help!"
                                runat="server" PopupHorizontalAlign="Center" PopupVerticalAlign="Above" AllowDragging="true"
                                EnableHierarchyRecreation="True" Theme="Office2010Blue">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table border="0" cellpadding="4" cellspacing="0">

                                                <tr>
                                                    <td valign="top" style="color: #666666; font-family: Tahoma; font-size: 14px;">
                                                        Please keep in mind that the figures that you see in this report are only for the accounts opened in the selected date range.
                                                        <br />
                                                        i.e. Not the total turnover on accounts for the store.
                                                        <br />
                                                        <br />
                                                        The TOTAL SPENT is for ALL customers who opened their account in the specified store (during the selected date range.)
                                                       <br />
                                                        The TOTAL BAD DEBT is the total owing for the customers who meet the Outstanding criteria.
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <%--<ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />--%>
                            </dx:ASPxPopupControl>
                        </td>

                    </tr>

                    <tr>
                        <td class="auto-style2">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Font-Bold="True" UseMaskBehavior="True" Width="250px">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Date:" Width="90px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Date="04/09/2013 16:22:30" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Font-Bold="True" UseMaskBehavior="True" Width="250px">
                            </dx:ASPxDateEdit>
                        </td>
                        <td></td>
                    </tr>

                    <tr>
                        <td class="auto-style2">
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Outstanding > " Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtOutstanding" runat="server" Width="100px" Style="float: left;" ValueType="System.String" ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save">
                                <ValidationSettings SetFocusOnError="True" ErrorText="Please input an outstanding amount">
                                    <RequiredField IsRequired="True" ErrorText="Please input an outstanding amount" />
                                    <RequiredField IsRequired="True" ErrorText="Please input an outstanding amount"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="in" Width="21px" Style="padding: 3px 0 0 17px;">
                            </dx:ASPxLabel>
                            <dx:ASPxComboBox ID="cboPeriod" runat="server" Width="100px" Style="float: right;"
                                ValueType="System.String" ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save">
                                <ValidationSettings SetFocusOnError="True" ErrorText="Please select a Period">
                                    <RequiredField IsRequired="True" ErrorText="Please select a Period" />
                                    <RequiredField IsRequired="True" ErrorText="Please select a Period"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                ValidationGroup="save">
                            </dx:ASPxValidationSummary>
                            <dx:ASPxButton ID="cmdRun" runat="server" Style="float: right; margin-left: 0px;" Text="Run" ValidationGroup="save">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding" CssClass="date_panel" Width="98%"
        EnableTheming="True">

        <Columns>
            <dx:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" VisibleIndex="1" />

            <dx:GridViewDataColumn FieldName="branch_name" Caption="Branch Name" VisibleIndex="2" />
            <dx:GridViewDataColumn FieldName="total_opened" VisibleIndex="3" Caption="Total Acc Opened" />

            <dx:GridViewDataTextColumn FieldName="total_bad_accounts" VisibleIndex="4" Caption="Total Bad Acc">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="bad_people_percentage" VisibleIndex="5" Caption="Bad Acc %">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="total_spent" VisibleIndex="6" Caption="Total Spent">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="bad_debt" VisibleIndex="7" Caption="Total Bad Debt">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="bad_debt_percentage" VisibleIndex="8" Caption="BD %">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="gvDetail" runat="server"
                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" VisibleIndex="1" />

                        <dx:GridViewDataColumn FieldName="account_number" Caption="Account Number" VisibleIndex="2" />
                        <dx:GridViewDataTextColumn FieldName="date_of_creation" VisibleIndex="3" Caption="Date Created">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn FieldName="itc_rating" VisibleIndex="4" Caption="Rating" />

                        <dx:GridViewDataTextColumn FieldName="total" VisibleIndex="5" Caption="Total Owing">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="current_balance" VisibleIndex="6" Caption="Cur Bal">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="p30" VisibleIndex="7" Caption="30 Days">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="p60" VisibleIndex="8" Caption="60 Days">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="p90" VisibleIndex="9" Caption="90 Days">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="p120" VisibleIndex="10" Caption="120 Days">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="p150" VisibleIndex="11" Caption="150 Days">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="total_spent" VisibleIndex="12" Caption="Tot Spent">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="credit_limit" VisibleIndex="13" Caption="Credit Limit">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="file_type" VisibleIndex="14" Caption="File Type">
                        </dx:GridViewDataTextColumn>


                    </Columns>
                    <Settings ShowFooter="True" />
                    <GroupSummary>
                        <dx:ASPxSummaryItem DisplayFormat="Total Accounts: {0}" FieldName="account_number" SummaryType="Count" />
                        <dx:ASPxSummaryItem DisplayFormat="Total in 150: {0}" FieldName="p150" SummaryType="Sum" />
                    </GroupSummary>
                    <Settings ShowGroupPanel="True" />
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <SettingsPager PageSize="50">
                    </SettingsPager>
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>

        <%--<TotalSummary>
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" />
                </TotalSummary>--%>
        <SettingsBehavior ColumnResizeMode="Control" />
        <Settings ShowFilterRow="true" ShowFilterRowMenu="True" />
        <SettingsPager PageSize="50">
        </SettingsPager>

        <Settings ShowFooter="True" />

        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
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
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
</asp:Content>

