<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="GrdEmployeeClocking.aspx.vb" Inherits="pcm.Website.GrdEmployeeClocking" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function searchEmployees(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Search";
            lp.Show();
            cab.PerformCallback();
        }

        function selectEmployee(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Select";
            lp.Show();
            cab.PerformCallback();
        }

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
                        HeaderText="Employee Clocking Report" CssClass="mb-20">
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
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployee" runat="server" Text="Employee" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td colspan="5" style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtEmployee" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>

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
                                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="526px" Height="250px"
                                    MaxWidth="800px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="pcMain"
                                    ShowFooter="True" FooterText="" PopupElementID="imgQ" HeaderText="Employee Search"
                                    runat="server" PopupHorizontalAlign="WindowCenter" EnableHierarchyRecreation="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table border="0" cellpadding="4" cellspacing="0">

                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtEmployeeSearch" runat="server" Width="250px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search">
                                                                            <ClientSideEvents Click="searchEmployees" />
                                                                        </dx:ASPxButton>
                                                                    </td>

                                                                </tr>

                                                            </table>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                            <dx:ASPxGridView ID="grdEmployeeSearch" runat="server" AutoGenerateColumns="False" OnDataBinding="grdEmployeeSearch_DataBinding" Width="425px">
                                                                <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn FieldName="employee_number" Caption="Employee Number" VisibleIndex="1" />
                                                                    <dx:GridViewDataTextColumn FieldName="employee_name" Caption="Employee Name" VisibleIndex="2" />
                                                                </Columns>
                                                                <SettingsAdaptivity>
                                                                    <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                                                </SettingsAdaptivity>
                                                                <SettingsBehavior AllowSelectByRowClick="true" EnableRowHotTrack="True" AllowSort="False" />
                                                            </dx:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">&nbsp;<dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" Width="425">
                                                            <ClientSideEvents Click="selectEmployee" />
                                                        </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                    <ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />
                                </dx:ASPxPopupControl>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <br />

                    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding"
                        EnableTheming="True" Width="900px">

                        <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>

                        <Columns>
                            <dx:GridViewDataColumn FieldName="employee_number" Caption="Emp #" VisibleIndex="1" />
                            <dx:GridViewDataColumn FieldName="id_number" Caption="ID Number" VisibleIndex="2" />

                            <dx:GridViewDataColumn FieldName="first_name" Caption="First Name" VisibleIndex="3" />
                            <dx:GridViewDataColumn FieldName="last_name" VisibleIndex="4" Caption="Last Name" />

                            <dx:GridViewDataTextColumn FieldName="normal" VisibleIndex="5" Caption="Normal">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sunday" VisibleIndex="6" Caption="Sunday">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="public_holiday" VisibleIndex="7" Caption="Public Holiday">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>
                        </Columns>

                        <SettingsAdaptivity>
                            <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity>

                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server"
                                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="clocking_date_in" Caption="Clock In Date" VisibleIndex="1" />

                                        <dx:GridViewDataColumn FieldName="clocking_hour_in" Caption="Clock In Time" VisibleIndex="3" />
                                        <dx:GridViewDataColumn FieldName="clocking_date_out" VisibleIndex="4" Caption="Clock Out Date" />

                                        <dx:GridViewDataTextColumn FieldName="clocking_hour_out" VisibleIndex="5" Caption="Clock Out Time">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="branch_name" VisibleIndex="6" Caption="Branch Name">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="normal" VisibleIndex="7" Caption="Normal">
                                            <PropertiesTextEdit DisplayFormatString="f" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="sunday" VisibleIndex="8" Caption="Sunday">
                                            <PropertiesTextEdit DisplayFormatString="f" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="public_holiday" VisibleIndex="9" Caption="Public Holiday">
                                            <PropertiesTextEdit DisplayFormatString="f" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="reason" VisibleIndex="10" Caption="Reason">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFooter="True" />
                                    <%-- <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="normal" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="sunday" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="public_holiday" SummaryType="Sum" />
                        </TotalSummary>--%>
                                    <Settings ShowGroupPanel="True" />
                                    <SettingsPager PageSize="20">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="normal" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="sunday" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="public_holiday" SummaryType="Sum" />
                        </TotalSummary>
                        <%--<TotalSummary>
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" />
                </TotalSummary>--%>
                        <SettingsBehavior ColumnResizeMode="Control" />

                        <SettingsPager PageSize="20">
                        </SettingsPager>

                        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
                    </dx:ASPxGridView>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px">
                                </dx:ASPxButton>
                            </td>

                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxGridViewExporter ID="Exporter" runat="server" GridViewID="gvMaster">
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
