<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageColourMatrix.aspx.vb" Inherits="pcm.Website.ManageColourMatrix" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <style type="text/css">
        .UpperCase input {
            text-transform: uppercase;
        }

        .auto-style2 {
            width: 232px;
        }
    </style>
    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();

        }
        function confirm(s, e) {
            e.processOnServer = false;
            var stationarycode = txtColourCode.GetText()
            var description = txtDescription.GetText()
            if (stationarycode == '') {
                lblConfirmation.SetText("You need to enter a Colour Code")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            if (description == '') {
                lblConfirmation.SetText("You need to enter a Description.")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }


            lblConfirmation.SetText("Are you sure you want to save stationary code " + stationarycode + "?")
            txtSave.SetVisible(true);
            txtCancel.SetVisible(true);
            popup.Show();
        }
        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }
        function save(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
            popup.Hide();
            lp.Show();
            cab.PerformCallback();

        }

        function clear(s, e) {
            e.processOnServer = false;
            txtColourCode.SetText('')
            txtDescription.SetText('')

        }

        function searchColourCode(s, e) {
            e.processOnServer = false;

            var searchType = cboSearch.GetText()
            var searchText = txtSearch.GetText()
            if (searchType == '') {
                lblConfirmation.SetText("You need to select a Search Type")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            //if (searchText == '') {
            //    lblConfirmation.SetText("You need to enter a search text.")
            //    txtSave.SetVisible(false);
            //    txtCancel.SetVisible(false);
            //    popup.Show();
            //    return
            //}
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SearchColourCode";
            lp.Show();
            cab.PerformCallback();
        }

        function selectColourCode(s, e) {
            e.processOnServer = false;
            var count = lstSearch.GetSelectedItems()

            if (count.length == 0) {
                lblConfirmation.SetText("Please select the colour code to continue.")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectColourCode";

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
                        HeaderText="Colour Matrix Manager">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblColourCode" runat="server" Width="150px" Text="Colour Code">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtColourCode" ClientInstanceName="txtColourCode" MaxLength="3" CssClass="UpperCase" runat="server" Width="210">
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblDescription" runat="server" Width="150px" Text="Stationary Description">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtDescription" ClientInstanceName="txtDescription" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
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
                                        <td></td>
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
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Search Branch"
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
                                                            <dx:ASPxComboBox ID="cboSearch" ClientInstanceName="cboSearch" Width="100%"
                                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearchDetails" runat="server" Text="Search Details"></dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtSearch" ClientInstanceName="txtSearch" CssClass="UpperCase" runat="server" Width="200px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                <ClientSideEvents Click="searchColourCode" />
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
                                                            <dx:ASPxListBox Width="760px" ClientInstanceName="lstSearch" ID="lstSearch" runat="server" ValueType="System.String"></dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="11">
                                                            <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" AutoPostBack="false">
                                                                <ClientSideEvents Click="selectColourCode" />
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
                                                                <dx:ASPxButton ID="txtSave" ClientInstanceName="txtSave" runat="server" Text="Yes" Width="83px">
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
