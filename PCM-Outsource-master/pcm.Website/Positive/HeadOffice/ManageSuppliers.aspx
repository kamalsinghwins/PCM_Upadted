<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageSuppliers.aspx.vb" Inherits="pcm.Website.ManageSuppliers" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }
        function populate(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Populate";
            lp.Show();
            cab.PerformCallback();
        }

        function bind(s, e) {
            if (e.htmlEvent.keyCode == 13) {
                var supplierCode = txtSupplierCode.GetText()
                if (supplierCode == '') {
                    ASPxClientUtils.PreventEvent(e.htmlEvent);
                    lblConfirmation.SetText("Please enter supplier code")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();
                    return
                }
                ASPxClientUtils.PreventEvent(e.htmlEvent);
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Populate";
                lp.Show();
                cab.PerformCallback();
            }
        }
        function clear(s, e) {
            e.processOnServer = false;
            txtSupplierCode.SetText("")
            txtAddressLine1.SetText("")
            txtAddressLine2.SetText("")
            txtAddressLine3.SetText("")
            txtAddressLine4.SetText("")
            txtAddressLine5.SetText("")
            txtSupplierName.SetText("")
            txtTelephoneNumber.SetText("")
            txtFAXNumber.SetText("")
            txtEmailAddress.SetText("")
            txtTAXNumber.SetText("")
            chkBlocked.SetChecked(false)
        }
        function confirm(s, e) {
            e.processOnServer = false;
            var action = s.globalName           
            var supplierCode = txtSupplierCode.GetText()
            if (supplierCode == '') {
                lblConfirmation.SetText("Please enter supplier code")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }
                if (txtSupplierName.GetText() == '') {
                    lblConfirmation.SetText("Please enter supplier name")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();
                    return
                }
                lblConfirmation.SetText("Are you sure you want to add supplier " + supplierCode + "?")
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
                txtSave.SetVisible(true);
                txtCancel.SetVisible(true);
                popup.Show();
            }
           
        
        function searchSupplier(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SearchSupplier";
            lp.Show();
            cab.PerformCallback();
        }
        function selectSupplier(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectSupplier";
            lp.Show();
            cab.PerformCallback();
        }
        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }
        function save(s, e) {
            e.processOnServer = false;
            popup.Hide();
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
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="300px"
                        HeaderText="Manage Suppliers">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblSupplierCode" runat="server" Width="120px" Text="Supplier Code">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtSupplierCode" ClientInstanceName="txtSupplierCode" CssClass="UpperCase" runat="server" Width="210">
                                                <ClientSideEvents KeyPress="bind" />
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine1" runat="server" Width="120px" Text="Address Line 1">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine1" ClientInstanceName="txtAddressLine1" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblSupplierName" runat="server" Width="120px" Text="Supplier Name">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtSupplierName" ClientInstanceName="txtSupplierName" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine2" runat="server" Width="120px" Text="Address Line 2">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine2" ClientInstanceName="txtAddressLine2" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTelephoneNumber" runat="server" Width="120px" Text="Telephone Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtTelephoneNumber" ClientInstanceName="txtTelephoneNumber" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine3" runat="server" Width="120px" Text="Address Line 3">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine3" ClientInstanceName="txtAddressLine3" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFAXNumber" runat="server" Width="120px" Text="FAX Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtFAXNumber" ClientInstanceName="txtFAXNumber" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine4" runat="server" Width="120px" Text="Address Line 4">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine4" ClientInstanceName="txtAddressLine4" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmailAddress" runat="server" Width="120px" Text="E-Mail Address">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtEmailAddress" ClientInstanceName="txtEmailAddress" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine5" runat="server" Width="120px" Text="Address Line 5">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine5" ClientInstanceName="txtAddressLine5" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTAXNumber" runat="server" Width="120px" Text="TAX Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtTAXNumber" ClientInstanceName="txtTAXNumber" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <label>Blocked</label>
                                        </td>
                                        <td>
                                            <dx:ASPxCheckBox ID="chkBlocked" ClientInstanceName="chkBlocked" runat="server"></dx:ASPxCheckBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="btnClear" runat="server" Text="Clear">
                                                <ClientSideEvents Click="clear"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <div>
                                          <%--      <dx:ASPxButton AutoPostBack="false" ID="txtDelete" runat="server" Text="Delete">
                                                    <ClientSideEvents Click="confirm" />
                                                </dx:ASPxButton>--%>
                                            </div>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td>
                                            <div style="float: right">
                                                <dx:ASPxButton AutoPostBack="false" ID="btnSave" runat="server" Text="Save">
                                                    <ClientSideEvents Click="confirm" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                  <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Search"
                                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table border="0" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearchType" runat="server" Text="Search Type"></dx:ASPxLabel>

                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cboSearchType" ClientInstanceName="cboSearchType" Width="100%"
                                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearchDetails" runat="server" Text="Search Details"></dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtSearch" CssClass="UpperCase" runat="server" Width="200px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                <ClientSideEvents Click="searchSupplier" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="12">
                                                            <dx:ASPxListBox Width="760px" ID="lstSearch" runat="server" ValueType="System.String"></dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="11">
                                                            <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" AutoPostBack="false">
                                                                <ClientSideEvents Click="selectSupplier" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                                  <dx:ASPxPopupControl ID="dxConfirmation" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                                    HeaderText="Confirmation" Width="548px" CloseAction="None" ClientInstanceName="popup"
                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                                    DisappearAfter="1000" PopupAnimationType="Fade">
                                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                            <div>
                                                <div id="Div1">
                                                    <dx:ASPxLabel ID="lblConfirmation" ClientInstanceName="lblConfirmation" runat="server"
                                                        Font-Size="16px">
                                                    </dx:ASPxLabel>

                                                    <table style="float: right">
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr align="right">
                                                            <td align="right">
                                                                <dx:ASPxButton ID="txtSave" ClientInstanceName="txtSave" UseSubmitBehavior="false" runat="server" Text="Yes" Width="83px">
                                                                    <ClientSideEvents Click="save" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="txtCancel" ClientInstanceName="txtCancel" runat="server" Text="No" Width="83px">
                                                                    <ClientSideEvents Click="OnClickCancel" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                    <ClientSideEvents CloseButtonClick="fadeOut" />
                                </dx:ASPxPopupControl>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
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
