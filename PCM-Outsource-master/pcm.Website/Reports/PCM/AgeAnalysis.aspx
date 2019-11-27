<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="AgeAnalysis.aspx.vb" Inherits="pcm.Website.AgeAnalysis1" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <style type="text/css">
        .style3 {
        }

        .style4 {
            width: 104px;
        }

        .style6 {
            width: 422px;
        }

        .auto-style1 {
            width: 104px;
            height: 23px;
        }

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            width: 150px;
        }

        .main_view {
            margin: 20px 0px 0px 20px;
        }

            .main_view .dxtc-strip {
                height: auto !important;
            }

            .main_view table tr td span, .main_view a > .dx-vam {
                text-transform: uppercase;
            }

        .UpperCase {
            text-transform: uppercase;
        }

        .auto-style8 {
            margin-left: 40px;
        }

        .burea_btn {
            margin: 0 5px 10px 0;
        }

        .text-center {
            text-align: center;
        }

        .mb-10 {
            margin-bottom: 10px;
        }

        .mainContainer {
            padding: 10px;
        }

        .check-with-input {
            float: left;
            margin-right: 10px;
        }

        .input-check-with {
            float: left;
            width: 140px;
        }

        .auto-style9 {
            width: 150px;
            height: 23px;
        }

        .auto-style10 {
            margin-left: 40px;
            height: 23px;
        }
    </style>

    <script type="text/javascript">

        
        function onEnd(s, e) {
            lp.Hide();
        }


        function Ok(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Ok";

            lp.Show();
            cab.PerformCallback();
        }


        function opt1(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt1";

            lp.Show();
            cab.PerformCallback();
        }
        function opt2(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt2";

            lp.Show();
            cab.PerformCallback();
        }
        function chkAll(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "chkAll";

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
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <div class="left-side-tables" style="width: 100%; float: left;">
                        <fieldset>
                            <legend>Age Analysis</legend>
                            <table>
                                <tr>
                                    <td class="auto-style3">

                                        <dx:ASPxLabel ID="Label1_0" runat="server" Text="From Account">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">

                                        <dx:ASPxTextBox ID="txtFrom" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">

                                        <dx:ASPxLabel ID="Label1_1" runat="server" Text="To Account">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">

                                        <dx:ASPxTextBox ID="txtTo" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="chkAllLabel" runat="server" Text="All Accounts">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">

                                        <dx:ASPxCheckBox ID="chkAll" runat="server">
                                            <ClientSideEvents CheckedChanged="chkAll"></ClientSideEvents>
                                        </dx:ASPxCheckBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_2" runat="server" Text="Period">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxComboBox ID="cboPeriod" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Branch Code">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxComboBox ID="cboBranch" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style9">
                                        <dx:ASPxLabel ID="chkDebLabel" runat="server" Text="Total < 0">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style2">&nbsp;
                                    </td>
                                    <td class="auto-style2">&nbsp;
                                    </td>
                                    <td class="auto-style10" colspan="2">
                                        <dx:ASPxCheckBox ID="chkDeb" runat="server"></dx:ASPxCheckBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="chkZeroLabel" runat="server" Text="Print Zero Balance">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxCheckBox ID="chkZero" runat="server"></dx:ASPxCheckBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="chkCredLabel" runat="server" Text="Total > 0">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxCheckBox ID="chkCred" runat="server"></dx:ASPxCheckBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="chkRageEmployeeLabel" runat="server" Text="Rage Employees Only">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxCheckBox ID="chkRageEmployee" runat="server"></dx:ASPxCheckBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="opt1Label" runat="server" Text="Active Acc only ">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxRadioButton ID="opt1" runat="server">
                                            <ClientSideEvents CheckedChanged="opt1"></ClientSideEvents>
                                        </dx:ASPxRadioButton>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="opt2Label" runat="server" Text="Other Status ">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxRadioButton ID="opt2" CssClass="check-with-input" runat="server">
                                            <ClientSideEvents CheckedChanged="opt2"></ClientSideEvents>
                                        </dx:ASPxRadioButton>
                                        <dx:ASPxComboBox ID="cboOther" CssClass="input-with-check" runat="server" Width="140px" ValueType="System.String"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8"></td>
                                </tr>

                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2"></td>
                                    <td class="auto-style8"></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2" style="float: right">
                                        <dx:ASPxButton ID="cmdOk" runat="server" Text="RUN">
                                            <ClientSideEvents Click="Ok"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                    <td class="auto-style8"></td>
                                </tr>
                            </table>
                        </fieldset>
                        <table style="width: 100%">
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                    
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2">
                                    <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV">
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="color: #666666; font-family: Tahoma; font-size: 14px;" colspan="5">
                                    <dx:ASPxGridView ID="grdAgeAnalysisDetails" runat="server" AutoGenerateColumns="False" OnDataBinding="grdAgeAnalysisDetails_DataBinding"
                                        Width="100%">

                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <%-- <ClientSideEvents RowDblClick="FillDebtorsDetails"></ClientSideEvents>--%>

                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="first_name" Caption="First Name" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="last_name" Caption="Last Name" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="id_number" Caption="ID Number" VisibleIndex="4" />                                           
                                            <dx:GridViewDataTextColumn FieldName="clock_number" Caption="Clock Number" VisibleIndex="5" />                                           
                                            <dx:GridViewDataTextColumn FieldName="current_balance" Caption="Current Balance" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="p30" Caption="p30" VisibleIndex="7" />
                                            <dx:GridViewDataTextColumn FieldName="p60" Caption="p60" VisibleIndex="8" />
                                            <dx:GridViewDataTextColumn FieldName="p90" Caption="p90" VisibleIndex="9" />
                                            <dx:GridViewDataTextColumn FieldName="p120" Caption="p120" VisibleIndex="10" />
                                            <dx:GridViewDataTextColumn FieldName="p150" Caption="p150" VisibleIndex="11" />
                                            <dx:GridViewDataTextColumn FieldName="total" Caption="Total" VisibleIndex="12" />
                                            <dx:GridViewDataTextColumn FieldName="branch_code" Caption="Branch" VisibleIndex="13" />
                                            <dx:GridViewDataTextColumn FieldName="date_of_last_transaction" Caption="Last Transaction" VisibleIndex="14" />
                                            <dx:GridViewDataTextColumn FieldName="date_of_last_payment" Caption="Last Payment" VisibleIndex="15" />
                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="current_balance" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p30" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p60" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p90" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p120" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p150" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="total" SummaryType="Sum" />
                                        </TotalSummary>
                                        <SettingsPager PageSize="200" />
                                        <Settings ShowFooter="True" />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;                                   
                                   
                                 
                                </td>
                                <td></td>
                                <td class="auto-style8"></td>
                                <td></td>
                            </tr>
                        </table>
                        <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                        </dx:ASPxGridViewExporter>
                    </div>
                </div>
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
    <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
        function onlyAlphabets(evt) {
            var charCode;
            if (window.event)
                charCode = window.event.keyCode;  //for IE
            else
                charCode = evt.which;  //for firefox
            if (charCode == 32) //for &lt;space&gt; symbol
                return true;
            if (charCode > 31 && charCode < 65) //for characters before 'A' in ASCII Table
                return false;
            if (charCode > 90 && charCode < 97) //for characters between 'Z' and 'a' in ASCII Table
                return false;
            if (charCode > 122) //for characters beyond 'z' in ASCII Table
                return false;
            return true;
        }
        function isNumberandTextValidTE(evt) {
            debugger;
            if (isNumber(evt) == false) {
                tP_KeyPress(s, e)
            }


        }

    </script>
</asp:Content>
