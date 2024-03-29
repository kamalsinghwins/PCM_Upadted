﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="EmployeeReviews.aspx.vb" Inherits="pcm.Website.EmployeeReviews" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

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

        .auto-style4 {
            width: 104px;
            height: 100%;
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

        .ml-10 {
            margin-left: 10px;
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

        function SelectReport(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectReport";

            lp.Show();
            cab.PerformCallback();
        }

        function Save(s, e) {
            e.processOnServer = false;

            if (!ASPxClientEdit.ValidateGroup("save")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";

            lp.Show();
            cab.PerformCallback();
        }

        function FindEmployees(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Lookup";

            lp.Show();
            cab.PerformCallback();
        }

        function ClockClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "LookupClockNumber";

            lp.Show();
            cab.PerformCallback();
        }

        function FillEmployeeDetails(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "EmployeeSelected";

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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Employee Note" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <table>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblEmployeename0" runat="server" Text="Clock Number">
                                                </dx:ASPxLabel>

                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtClockNumber" runat="server" CssClass="pull-left " Width="170px">
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must enter a Clock Number and Click Check" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                                <dx:ASPxImage ID="OpenPopup" runat="server" CssClass="pull-left ml-10" ImageUrl="~/images/search.png">
                                                </dx:ASPxImage>

                                            </td>
                                            <td class="style3">
                                                <dx:ASPxButton ID="cmdCheck" runat="server" Text="Check">
                                                    <ClientSideEvents Click="ClockClick" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblEmployeename" runat="server" Text="Employee Name">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp; </td>
                                            <td>&nbsp; </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtEmployeeName" runat="server" CssClass="pull-left " Width="170px">
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must enter a Clock Number and Click Check" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>


                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp; </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblEmployeename1" runat="server" Text="Branch Code">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" CssClass="pull-left " Width="170px"
                                                    runat="server" AutoPostBack="false"
                                                    ValueType="System.String">
                                                    <ValidationSettings ValidationGroup="save">
                                                        <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>

                                            <td class="style3">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblTypeOfReport" runat="server" Text="Type Of Report">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboTypeOfReport" runat="server" ClientInstanceName="combo" ValueType="System.String" AutoPostBack="true">
                                                    <ClientSideEvents SelectedIndexChanged="SelectReport" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblWarning" runat="server" Text="Warning">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboWarning" ClientInstanceName="combo" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                                <dx:ASPxRatingControl ID="rating" runat="server" ItemCount="10" Value="0"></dx:ASPxRatingControl>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"> <dx:ASPxLabel ID="lblWarningExpiry" runat="server" Text="Warning Expiry Date">
                                                </dx:ASPxLabel></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8"> <dx:ASPxDateEdit ID="txtExpiryDate" runat="server" Font-Bold="True" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit></td>
                                            <td class="style3">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblNote" runat="server" Text="Note">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="520px" ClientInstanceName="notes"
                                                    ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                    ValidationSettings-ValidationGroup="save" ValidationSettings-ErrorText="ttt" MaxLength="2500">
                                                    <%--<ClientSideEvents KeyDown="RecalculateCharsRemaining" KeyUp="RecalculateCharsRemaining"
                                    GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
                                    Init="function(s, e) { InitMemoMaxLength(s, 2500); RecalculateCharsRemaining(s); }"></ClientSideEvents>--%>

                                                    <ValidationSettings Display="Dynamic" ErrorText="ttt" ValidationGroup="save"></ValidationSettings>

                                                </dx:ASPxMemo>
                                                <span class="chrm">
                                                    <dx:ASPxLabel ID="txtOldNotes_cr" runat="server" EnableClientSideAPI="True" />
                                                </span>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="700px" Height="250px"
                                        MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                        ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Lookup Employees"
                                        runat="server" PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" EnableHierarchyRecreation="True" AllowDragging="True">
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

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />


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
                                    <br />

                                    <table>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                                    ValidationGroup="save" Width="100%">
                                                </dx:ASPxValidationSummary>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8" colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxButton ID="cmdSave" runat="server" ValidationGroup="save" Text="Save">
                                                    <ClientSideEvents Click="Save"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center"></td>
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
                <%-- <asp:HiddenField ID="HDEmployeeNumber" runat="server" />--%>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>

