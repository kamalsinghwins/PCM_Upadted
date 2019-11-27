<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageRegions.aspx.vb" Inherits="pcm.Website.ManageRegions" %>

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
        function clear(s, e) {
            e.processOnServer = false;
            txtBranchRegion.SetText("")
        }
        function confirm(s, e) {
            e.processOnServer = false;
            var action = s.globalName
            if (action == "Save") {
                var region = txtBranchRegion.GetText()
                if (region == '') {
                    lblConfirmation.SetText("Please enter branch region")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();
                    return
                }

                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
                lblConfirmation.SetText("Are you sure you want to add region " + region.toUpperCase() + "?")
                txtSave.SetVisible(true);
                txtCancel.SetVisible(true);
                popup.Show();
            }

            else {
                var count = grdRegion.GetSelectedRowCount();
                var value=grdRegion.GetRowKey(count)
                if (count > 0) {
                    document.getElementById('<%=hdWhichButton.ClientID%>').value = "Delete";
                    lblConfirmation.SetText("Are you sure you want to delete it ?")
                    txtSave.SetVisible(true);
                    txtCancel.SetVisible(true);
                    popup.Show();
                }
                else {
                      lblConfirmation.SetText("Please select the region to delete.")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();

                }

            }

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

        function DeleteRegion(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Delete";
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
                        HeaderText="Manage Regions">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblBranchRegion" runat="server" Width="120px" Text="Branch Region">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtBranchRegion" ClientInstanceName="txtBranchRegion" MaxLength="30" CssClass="UpperCase" runat="server" Width="210">
                                            </dx:ASPxTextBox>

                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
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
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td>
                                            <div style="float: right">
                                                <dx:ASPxButton AutoPostBack="false" ClientInstanceName="Save" ID="btnSave" runat="server" Text="Add">
                                                    <ClientSideEvents Click="confirm" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>

                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ID="dxConfirmation" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                                    HeaderText="Confirmation" Width="548px" CloseAction="None" ClientInstanceName="popup"
                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                                    DisappearAfter="1000" PopupAnimationType="Fade">
                                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                            <div>
                                                <div id="Div1">
                                                    <dx:ASPxLabel ID="lblConfirmation" ClientInstanceName="lblConfirmation" runat="server" Text="Are you sure you want to delete it?"
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
                    <div style="float: right">
                        <dx:ASPxButton ID="btnDelete" ClientInstanceName="Delete" runat="server" Text="Delete Region">
                            <ClientSideEvents Click="confirm"></ClientSideEvents>
                        </dx:ASPxButton>
                    </div>
                    <dx:ASPxGridView ID="grdRegion" ClientInstanceName="grdRegion" runat="server" AutoGenerateColumns="False" Width="100%" EnableTheming="True" KeyFieldName="region" OnDataBinding="grdRegion_DataBinding">
                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Region" FieldName="region" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <SettingsEditing EditFormColumnCount="3" />
                    </dx:ASPxGridView>
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
