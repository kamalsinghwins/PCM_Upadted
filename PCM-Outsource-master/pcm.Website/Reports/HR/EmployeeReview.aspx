<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="EmployeeReview.aspx.vb" Inherits="pcm.Website.EmployeeReview" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>

    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .mb-20 {
            margin-bottom: 20px;
        }

        .ellipsis {
            text-overflow: ellipsis;
            overflow: hidden;
            max-width: 100px !important;
        }

        .mr-20 {
            margin-right: 20px;
        }

        .mt-20 {
            margin-top: 20px;
        }

        .text-transform-capitalize {
            text-transform: capitalize;
        }

        .auto-style2 {
            width: 140px;
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }

        function FindEmployees(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Lookup";

            lp.Show();
            cab.PerformCallback();
        }

        function Export(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Export";

            lp.Show();
            cab.PerformCallback();
        }

        function FillEmployeeDetails(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "EmployeeSelected";
            lp.Show();
            cab.PerformCallback();
        }

        function opt1_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt1_changed";

            cab.PerformCallback();
        }

        function opt2_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt2_changed";

            cab.PerformCallback();
        }

        function Run(s, e) {
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

    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback"
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%"
                        HeaderText="Employee Review Summary" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>

                                    <tr>
                                        <td>
                                            <dx:ASPxRadioButton ID="opt1" runat="server" GroupName="Radio">
                                                <ClientSideEvents CheckedChanged="opt1_changed" />
                                            </dx:ASPxRadioButton>
                                        </td>
                                        <td></td>
                                        <td></td>
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
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td class="auto-style2">&nbsp;</td>
                                        <td></td>

                                    </tr>

                                    <tr>
                                        <td>
                                            <dx:ASPxRadioButton ID="opt2" GroupName="Radio" runat="server">
                                                <ClientSideEvents CheckedChanged="opt2_changed" />
                                            </dx:ASPxRadioButton>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeName" runat="server" Text="Clock Number:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtClockNumber" runat="server" Width="250px"></dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" CssClass="pull-left ml-10" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td>

                                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run">
                                                <ClientSideEvents Click="Run" />
                                            </dx:ASPxButton>

                                        </td>

                                        <td class="auto-style2">
                                            <dx:ASPxButton ID="cmdExport" Style="float: right; margin-left: 0px;" runat="server" Text="Export">
                                                <%--    <ClientSideEvents Click="Export" />--%>
                                            </dx:ASPxButton>
                                        </td>

                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="700px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Lookup Employees"
                                    runat="server" PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" EnableHierarchyRecreation="True" AllowDragging="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server" Height="475px">
                                                <table border="0" align="center" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="txtSearchFieldlabel" runat="server" Text="Search Field"></dx:ASPxLabel>

                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cboSearchType" runat="server" Width="170px" ClientInstanceName="cboSearch">
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="txtCriteriaLabel" runat="server" Text="Criteria"></dx:ASPxLabel>

                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtCriteria" CssClass="UpperCase" runat="server" Width="170px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="cmdLookUp" runat="server" Text="LookUp" CssClass="px-0" AutoPostBack="false">
                                                                            <ClientSideEvents Click="FindEmployees"></ClientSideEvents>
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                            <dx:ASPxGridView ID="grdEmployeeSearch" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                OnDataBinding="grdEmployeesSearch_DataBinding" Width="660px">

                                                                <SettingsAdaptivity>
                                                                    <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                                                </SettingsAdaptivity>

                                                                <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />


                                                                <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>


                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn FieldName="employee_number" Caption="Employee Number" VisibleIndex="1" />
                                                                    <dx:GridViewDataTextColumn FieldName="first_name" Caption="First Name" VisibleIndex=" 2" />
                                                                    <dx:GridViewDataTextColumn FieldName="last_name" Caption="Last Name" VisibleIndex="3" />
                                                                </Columns>
                                                            </dx:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                            <dx:ASPxButton ID="cmdSelect" runat="server" Text="LOAD" Width="100%" AutoPostBack="false" ClientInstanceName="SelectStockcode">
                                                                <ClientSideEvents Click="FillEmployeeDetails"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxGridView ID="gvReview" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBinding="gvReview_DataBinding"
                        EnableTheming="True">

                        <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>

                        <Columns>

                            <%--<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="0">                        
                            </dx:GridViewDataTextColumn>--%>
                            <dx:GridViewDataColumn Caption="Name" VisibleIndex="0">
                                <DataItemTemplate>
                                    <dx:ASPxHyperLink runat="server" Target="_blank" NavigateUrl='<%#String.Format("EmployeeDetails.aspx?id={0}", Eval("employee_number")) %>' RenderMode="Link" Text='<%# Eval("name")%>' AutoPostBack="false"></dx:ASPxHyperLink>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataTextColumn Caption="Employee Number" CellStyle-CssClass="ellipsis" FieldName="employee_number" VisibleIndex="1">
                                <CellStyle CssClass="ellipsis"></CellStyle>
                            </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_name" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Review" FieldName="type_of_comment" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Rating" FieldName="rating" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Warning" FieldName="type_of_warning" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Comment" CellStyle-CssClass="ellipsis" FieldName="comment" VisibleIndex="7">
                                <CellStyle CssClass="ellipsis"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Date" FieldName="time_stamp" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Warning Expiry" FieldName="warning_expiry_date" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <SettingsAdaptivity>
                            <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity>

                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="True" ShowFilterBar="Visible" ShowFilterRow="true" ShowGroupPanel="true" />
                    </dx:ASPxGridView>
                </div>
                <dx:ASPxGridViewExporter ID="Exporter" GridViewID="gvReview" runat="server">
                </dx:ASPxGridViewExporter>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2" class="text-center">
                                    <dx:ASPxLabel ID="lblError" runat="server"
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
