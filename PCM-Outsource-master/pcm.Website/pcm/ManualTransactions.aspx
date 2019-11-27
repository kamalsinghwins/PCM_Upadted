<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManualTransactions.aspx.vb" Inherits="pcm.Website.Transaction" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../js/General/application.js"></script>
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

        .auto-style9 {
            width: 150px;
            height: 27px;
        }

        .auto-style10 {
            height: 27px;
        }

        .auto-style11 {
            margin-left: 40px;
            height: 27px;
        }

        .auto-style12 {
            margin-left: 40px;
            width: 217px;
        }

        .auto-style13 {
            margin-left: 40px;
            height: 27px;
            width: 217px;
        }
    </style>



    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }

        function ShowHideDate(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "ShowHideDate";

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

        function CheckCard(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CheckCard";

            lp.Show();
            cab.PerformCallback();
        }

        function ClearAll(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "ClearAll";

            lp.Show();
            cab.PerformCallback();
        }

        function ANOnly(s, e) {
            if (!(e.htmlEvent.keyCode > 47 && e.htmlEvent.keyCode < 58) &&  // numeric (0-9)
                !(e.htmlEvent.keyCode > 64 && e.htmlEvent.keyCode < 91) &&  // upper alpha (A-Z)
                !(e.htmlEvent.keyCode > 96 && e.htmlEvent.keyCode < 123) &&  // lower alpha (a-z)            
                !(e.htmlEvent.keyCode == 32))
                //    if (!(e.htmlEvent.keyCode = 110))

                ASPxClientUtils.PreventEvent(e.htmlEvent);

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
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
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
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;
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
                        <fieldset>
                            <legend>Transaction</legend>
                            <table>

                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_1" runat="server" Text="Card Number">
                                        </dx:ASPxLabel>

                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxTextBox ID="txtCardNum" onkeypress="return isNumber(event)" runat="server" Width="100%" MaxLength="20">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a Card Number" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>

                                    </td>
                                    <td>
                                        <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                        </dx:ASPxImage>
                                    </td>

                                    <td>
                                        <dx:ASPxButton ID="cmdCheck" runat="server" Text="Check">
                                            <ClientSideEvents Click="CheckCard"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_0" runat="server" Text="Account Number">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxTextBox ID="txtAccNum" onkeypress="return isNumber(event)" runat="server" Width="100%">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a Card Number and click Check" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_4" runat="server" Text="Customer Name"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxTextBox ID="txtCustomer" runat="server" Width="100%"></dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_5" runat="server" Text="ID Number"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxTextBox ID="txtID" onkeypress="return isNumber(event)" runat="server" Width="100%"></dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label1_3" runat="server" Text="Shop Assistant"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxTextBox ID="txtShopA" runat="server" Width="100%" CssClass="UpperCase">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a Shop Assistant" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style9">
                                        <dx:ASPxLabel ID="Label2_1" runat="server" Text="Branch Code"></dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style10">&nbsp;
                                    </td>
                                    <td class="auto-style10">&nbsp;
                                    </td>
                                    <td class="auto-style11"></td>
                                    <td class="auto-style13">
                                        <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" Width="100%"
                                            runat="server" AutoPostBack="false"
                                            ValueType="System.String">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="auto-style10"></td>
                                    <td class="auto-style10">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label2_0" runat="server" Text="Amount"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxSpinEdit ID="txtAmount" runat="server" Width="100%" 
                                                         ReadOnly="false" MaxLength="10" MinValue="0" MaxValue="6000">
                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input an amount" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label8" runat="server" Text="Notes">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td class="auto-style8">&nbsp;</td>
                                    <td class="auto-style12">
                                        <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="100%">
                                            <ClientSideEvents KeyPress="ANOnly"></ClientSideEvents>
                                            <%--<ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a Note" />
                                            </ValidationSettings>--%>
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="Label4" runat="server" Text="Transaction Type">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxComboBox ID="cboTransactionType" runat="server" ValueType="System.String" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="ShowHideDate" />
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must select a Transaction Type" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="PaymentTypeLabel" runat="server" Text="Payment Type">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12">
                                        <dx:ASPxComboBox ID="cboPayType" runat="server" ValueType="System.String" Width="100%">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="lblPayDeposit" runat="server" Visible="false" Text="Pay Deposit"></dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td class="auto-style8">&nbsp;</td>
                                    <td class="auto-style12">
                                        <dx:ASPxCheckBox ID="chkPayDeposit" runat="server" Visible="false" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12" align="center">
                                        <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                            ValidationGroup="save" Width="100%">
                                        </dx:ASPxValidationSummary>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxButton ID="cmdClear" runat="server" Text="Clear">
                                            <ClientSideEvents Click="ClearAll"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8"></td>
                                    <td class="auto-style12" align="center">
                                        <dx:ASPxButton ID="cmdOK" runat="server" Text="Insert Transaction" ValidationGroup="save">
                                            <ClientSideEvents Click="insert"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>

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
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

    </script>
</asp:Content>


