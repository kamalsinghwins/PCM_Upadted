<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="JournalTransactions.aspx.vb" Inherits="pcm.Website.JournalTransactions" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

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

        .pull-left {
            float: left;
        }

        .pull-right {
            float: right;
        }

        .p-0 {
            padding: 0;
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }

        //function isAlphaNumeric(s, e) {
        //    var code, i, len;

        //    for (i = 0, len = str.length; i < len; i++) {
        //        code = str.charCodeAt(i);
        //        if (!(code > 47 && code < 58) && // numeric (0-9)
        //            !(code > 64 && code < 91) && // upper alpha (A-Z)
        //            !(code > 96 && code < 123)) { // lower alpha (a-z)
        //            return false;
        //        }
        //    }
        //    return true;
        //};

        function ANOnly(s, e) {
            if (!(e.htmlEvent.keyCode > 47 && e.htmlEvent.keyCode < 58) &&  // numeric (0-9)
                !(e.htmlEvent.keyCode > 64 && e.htmlEvent.keyCode < 91) &&  // upper alpha (A-Z)
                !(e.htmlEvent.keyCode > 96 && e.htmlEvent.keyCode < 123) &&  // lower alpha (a-z)            
                !(e.htmlEvent.keyCode == 32))
                //    if (!(e.htmlEvent.keyCode = 110))

                ASPxClientUtils.PreventEvent(e.htmlEvent);

        }

       <%-- function checkAccountNumber(s, e) {
            e.processOnServer = false;
            var AN = s.GetValue();

            if (AN && AN != "") {
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "CheckAN";

                lp.Show();
                cab.PerformCallback();
            } else {
                return false;
            }

        }--%>

        function checkAccountNumber(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CheckAN";

            lp.Show();
            cab.PerformCallback();
        }

        function insert(s, e) {
            e.processOnServer = false;

            if (!ASPxClientEdit.ValidateGroup("save")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Insert";

            lp.Show();
            cab.PerformCallback();
        }

        function clear(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";

            lp.Show();
            cab.PerformCallback();
        }

        function FindDebtors(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Lookup";

            lp.Show();
            cab.PerformCallback();
        }

        function FillDebtorsDetails(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "DebtorSelected";

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
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>

        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>

                <div class="mainContainer">

                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Journal Entry" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="250px"
                                        MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                        ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Lookup Debtors"
                                        runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                <asp:Panel ID="Panel1" runat="server">
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
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>
                                                                           &nbsp;
                                                                        </td>
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
                                                                                <ClientSideEvents Click="FindDebtors"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
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
                                                                <dx:ASPxGridView ID="grdDebtorsSearch" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                    OnDataBinding="grdDebtorsSearch_DataBinding" Width="760px">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="True" />
                                                                    <%-- <ClientSideEvents RowDblClick="FillDebtorsDetails"></ClientSideEvents>--%>
                                                                    <Settings ShowFilterRow="True" />
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="id_number" Caption="ID Number" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="first_name" Caption="First Name" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="last_name" Caption="Last Name" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="status" Caption="Status" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="cardnum" Caption="Card Number" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="cell_number" Caption="Cellphone" VisibleIndex="7" />
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                                <dx:ASPxButton ID="cmdSelect" runat="server" Text="LOAD" Width="100%" AutoPostBack="false" ClientInstanceName="SelectStockcode">
                                                                    <ClientSideEvents Click="FillDebtorsDetails"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                    <table>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label2" runat="server" Text="Account Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <table style="padding: 0; margin: 0; border-collapse: collapse;">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxSpinEdit ID="txtAccNum" runat="server" Width="170px" DecimalPlaces="0">
                                                               <%-- <ClientSideEvents LostFocus="checkAccountNumber"></ClientSideEvents>--%>
                                                                <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                                <ValidationSettings ValidationGroup="save">
                                                                    <RequiredField IsRequired="True" ErrorText="You must select an Account Number" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                                            </dx:ASPxImage>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdCheck" runat="server" Text="Check">
                                                                <ClientSideEvents Click="checkAccountNumber"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel runat="server" Text="Name" ID="Label3"></dx:ASPxLabel>

                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtName" runat="server" ReadOnly="True" Width="170px">
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must select an Account Number" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxGridView ID="dxGrid" runat="server"
                                                    AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding">
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="total" Caption="Total Owing" VisibleIndex="1" />

                                                        <dx:GridViewDataColumn FieldName="current_balance" Caption="Current" VisibleIndex="2" />
                                                        <dx:GridViewDataColumn FieldName="p30" VisibleIndex="3" Caption="30 Days" />

                                                        <dx:GridViewDataTextColumn FieldName="p60" VisibleIndex="4" Caption="60 Days">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="p90" Caption="90 Days" VisibleIndex="5">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="p120" Caption="120 Days" VisibleIndex="6">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="p150" Caption="150 Days" VisibleIndex="7">
                                                        </dx:GridViewDataTextColumn>

                                                    </Columns>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label4" runat="server" Text="Amount">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxSpinEdit ID="txtAmount" runat="server" Width="170px" MinValue="0" MaxValue="6000" DecimalPlaces="2">
                                                    <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must select an Amount" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="Label6" runat="server" Text="Transaction Type">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboTransactionType" AutoPostBack="true" runat="server" ValueType="System.String">
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must select a Transaction Type" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label7" runat="server" Text="Balance Effected">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxRadioButton ID="radEffected" GroupName="effected" runat="server">
                                                </dx:ASPxRadioButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Balance NOT Effected">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxRadioButton ID="radNotEffected" GroupName="effected" runat="server">
                                                </dx:ASPxRadioButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label5" runat="server" Text="Type">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboType" runat="server">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="Label8" runat="server" Text="Notes">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="100%">
                                                    <ClientSideEvents KeyPress="ANOnly"></ClientSideEvents>
                                                </dx:ASPxMemo>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                                    ValidationGroup="save" Width="100%">
                                                </dx:ASPxValidationSummary>
                                            </td>
                                        </tr>

                                    </table>
                                    <br />

                                    <table style="width: 100%">
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8" colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxButton ID="cmdClear" runat="server" Text="Clear">
                                                    <ClientSideEvents Click="clear"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                &nbsp;
                                                <dx:ASPxButton ID="cmdInsert" runat="server" ValidationGroup="save" Text="Insert Transaction">
                                                    <ClientSideEvents Click="insert"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style8" colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td class="auto-style8" colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
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
                                </div>

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>

                </div>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
