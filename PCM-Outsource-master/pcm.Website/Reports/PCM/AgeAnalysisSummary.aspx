<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="AgeAnalysisSummary.aspx.vb" Inherits="pcm.Website.frmAgeAnalysisSummary" %>

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
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function OtherStatus(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt2";
            lp.Show();
            cab.PerformCallback();
        }
     <%--   function Ok(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Ok";
            lp.Show();
            cab.PerformCallback();
        }--%>
        function ActiveAccountOnly(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "opt1";
            lp.Show();
            cab.PerformCallback();
        }

        function CheckAll(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "chkAll";
            lp.Show();
            cab.PerformCallback();
        }

        function checkOption(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "checkOption";
            lp.Show();
            cab.PerformCallback();
        }

        function Ok(s,e) {
            e.processOnServer = false;
            debugger;

            if (!radopt1.GetChecked() && !radopt2.GetChecked()) {
                lblConfirmation.SetText("Please select a Status option.")
                popup.Show();
                return
            }

            if (radopt2.GetChecked() && cboOther.GetText() == "") {
                lblConfirmation.SetText("Please select an option for Other Status.")
                popup.Show();
                return
            }


            if (chkUseCurrent.GetChecked()) {
                if (cboCurrent.GetText() == "" || txtWhereCurrent.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

            if (chkUse30.GetChecked()) {
                if (cbo30Days.GetText() == "" || txtWhere30Days.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

             if (chkUse60.GetChecked()) {
                if (cbo60Days.GetText() == "" || txtWhere60Days.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

            if (chkuse90.GetChecked()) {
                if (cbo90Days.GetText() == "" || txtWhere90Days.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

            if (chkUse120.GetChecked()) {
                if (cbo120Days.GetText() == "" || txtWhere120Days.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

            if (chkUse150.GetChecked()) {
                if (cbo150Days.GetText() == "" || txtWhere150Days.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                }
            }

             if (chkUseTotal.GetChecked()) {
                if (cboTotal.GetText() == "" || txtWheretotal.GetText()=='' ) {
                    lblConfirmation.SetText("Please select corresponding option and fill value.")
                    popup.Show();
                    return
                 }
                 if (cboTotal.GetText() == "BETWEEN") {
                     if (txtWheretotal.GetText() == '' || txtToTotal.GetText() == '') {
                         lblConfirmation.SetText("Please input FROM and TO values.")
                         popup.Show();
                         return
                     }
                 }
            }

             if (chkOpenedBetween.GetChecked()) {
                if (txtStart.GetText() == "" || txtEnd.GetText()=='' ) {
                    lblConfirmation.SetText("Please select start date and end date.")
                    popup.Show();
                    return
                }
            }

             if (chkUseLastTransaction.GetChecked()) {
                if (txtLastTransactionStartDate.GetText() == "" || txtLastTransactionEndDate.GetText()=='' ) {
                    lblConfirmation.SetText("Please select start and end of last transaction date.")
                    popup.Show();
                    return
                }
            }
             document.getElementById('<%=hdWhichButton.ClientID%>').value = "Ok";
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
                <%-- <dx:ASPxNavBar ID="ASPxNavBar2" runat="server" >
                    <Groups>
                        <dx:NavBarGroup Name="GetNewDebtor" Text="Get New Debtor">
                            <Items>
                                <%-- <dx:NavBarItem Name="30days" Text="30 Days">
                                </dx:NavBarItem>
                                <dx:NavBarItem Name="GetNewDebtor" Text="Get Debtor">
                                </dx:NavBarItem>

                            </Items>
                        </dx:NavBarGroup>
                    </Groups>
                </dx:ASPxNavBar>--%>
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
                    <div class="left-side-tables" style="width: 40%; float: left;">
                        <fieldset>
                            <legend>Account Options</legend>
                            <table>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="opt1Label" runat="server" Text="Active Acc Only">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxRadioButton ID="opt1" Checked="true" ClientInstanceName="radopt1" runat="server" GroupName="radio">
                                            <ClientSideEvents CheckedChanged="ActiveAccountOnly"></ClientSideEvents>
                                        </dx:ASPxRadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="opt2Label" runat="server" Text="Other Status">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxRadioButton ID="opt2" ClientInstanceName="radopt2" runat="server" GroupName="radio">
                                            <ClientSideEvents CheckedChanged="OtherStatus"></ClientSideEvents>
                                        </dx:ASPxRadioButton>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxComboBox ID="cboOther" runat="server" ClientInstanceName="cboOther" EnableClientSideAPI="True" Width="100px" ValueType="System.String" ClientEnabled="false"></dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style8">&nbsp;
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
                        <fieldset>
                            <legend>Period Options</legend>
                            <table>

                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_13" runat="server" Text="Current"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboCurrent" ClientInstanceName="cboCurrent" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhereCurrent" ClientInstanceName="txtWhereCurrent" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUseCurrentLabel" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUseCurrent" ClientInstanceName="chkUseCurrent" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_12" runat="server" Text="30 Days"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbo30Days" ClientInstanceName="cbo30Days" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhere30Days" ClientInstanceName="txtWhere30Days" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUse30Label" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUse30" ClientInstanceName="chkUse30" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_11" runat="server" Text="60 Days"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbo60Days" ClientInstanceName="cbo60Days" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhere60Days" ClientInstanceName="txtWhere60Days" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUse60Label" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUse60" ClientInstanceName="chkUse60" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_10" runat="server" Text="90 Days"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbo90Days" ClientInstanceName="cbo90Days" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhere90Days" ClientInstanceName="txtWhere90Days" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkuse90Label" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkuse90" ClientInstanceName="chkuse90" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_9" runat="server" Text="120 Days"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbo120Days" ClientInstanceName="cbo120Days" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhere120Days" ClientInstanceName="txtWhere120Days" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUse120Label" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUse120" ClientInstanceName="chkUse120" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_8" runat="server" Text="150 Days"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbo150Days" ClientInstanceName="cbo150Days" runat="server" ValueType="System.String" Height="20px" Width="104px"></dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWhere150Days" ClientInstanceName="txtWhere150Days" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUse150Label" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUse150" ClientInstanceName="chkUse150" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_15" runat="server" Text="Total"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboTotal" ClientInstanceName="cboTotal" runat="server" ValueType="System.String" Height="20px" Width="104px">
                                            <ClientSideEvents SelectedIndexChanged="checkOption" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtWheretotal" ClientInstanceName="txtWheretotal" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtToTotal" ClientInstanceName="txtToTotal" runat="server" Width="70px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="chkUseTotalLabel" runat="server" Text="Use">
                                        </dx:ASPxLabel>
                                        <dx:ASPxCheckBox ID="chkUseTotal" ClientInstanceName="chkUseTotal" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <table>

                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;                                   
                                    <dx:ASPxButton ID="cmdOk" runat="server" Text="OK">
                                        <ClientSideEvents Click="Ok"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                                <td class="auto-style8" colspan="2"></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div class="right-side-tables" style="width: 60%; float: left;">
                        <fieldset>
                            <legend>Date Options</legend>
                            <table>
                                <tr>
                                    <td class="auto-style3" colspan="4">
                                        <dx:ASPxLabel ID="chkOpenedBetweenlabel" runat="server" Text="Use Accounts Opened Between">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxCheckBox ID="chkOpenedBetween" ClientInstanceName="chkOpenedBetween" runat="server"></dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label2" runat="server" Text="From Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxDateEdit ID="txtStart" ClientInstanceName="txtStart" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label3" runat="server" Text="To Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxDateEdit ID="txtEnd" ClientInstanceName="txtEnd" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3" colspan="4">
                                        <dx:ASPxLabel ID="chkUseLastTransactionLabel" runat="server" Text="Use Last Transaction Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxCheckBox ID="chkUseLastTransaction" ClientInstanceName="chkUseLastTransaction" runat="server"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label4" runat="server" Text="From Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxDateEdit ID="txtLastTransactionStartDate" ClientInstanceName="txtLastTransactionStartDate" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Date">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxDateEdit ID="txtLastTransactionEndDate" ClientInstanceName="txtLastTransactionEndDate" runat="server" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                        <fieldset>
                            <legend><strong>Result</strong></legend>
                            <table>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2" align="center">
                                        <dx:ASPxLabel ID="Label_7" runat="server" Text="Value"></dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style8" colspan="2" align="center">
                                        <dx:ASPxLabel ID="Label_14" runat="server" Text="No. of Accounts <> 0"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_0" runat="server" Text="Current">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txtCurrent" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txtCurrentAccounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_1" runat="server" Text="30 Days">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt30Days" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt30Accounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_2" runat="server" Text="60 Days">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt60Days" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt60Accounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_3" runat="server" Text="90 Days">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt90Days" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt90Accounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_4" runat="server" Text="120 Days">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt120Days" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt120Accounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_5" runat="server" Text="150 Days">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt150Days" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txt150Accounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label_6" runat="server" Text="Total">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txtTotal" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txtTotalAccounts" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Total Spent">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxTextBox ID="txtTotalSpent" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style8" colspan="2">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                            HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="popup"
                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                            DisappearAfter="1000" PopupAnimationType="Fade">
                            <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                                    <div>
                                        <div id="Div2" class="text-center">
                                            <dx:ASPxLabel ID="lblError" ClientInstanceName="lblConfirmation" runat="server"
                                                Font-Size="16px">
                                            </dx:ASPxLabel>
                                        </div>
                                    </div>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                            <ClientSideEvents CloseButtonClick="fadeOut" />
                        </dx:ASPxPopupControl>
                    </div>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
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

