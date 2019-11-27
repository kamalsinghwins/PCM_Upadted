<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageMunicipality.aspx.vb" Inherits="pcm.Website.ManageMunicipality" %>

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
            txtMunicipality.SetText("")
            cboBranch.SetSelectedIndex("-1")
        }
        function confirm(s, e) {
            e.processOnServer = false;
            var action = s.globalName
            if (action == "Save") {
                var municipality = txtMunicipality.GetText()
                if (municipality == '') {
                    lblConfirmation.SetText("Please enter municipality")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();
                    return
                }
                var branch = cboBranch.GetValue();
                if (branch == null) {
                    lblConfirmation.SetText("Please select the branch")
                    txtSave.SetVisible(false);
                    txtCancel.SetVisible(false);
                    popup.Show();
                    return
                }

                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
                lblConfirmation.SetText("Are you sure you want to add municipality " + municipality.toUpperCase() + "?")
                txtSave.SetVisible(true);
                txtCancel.SetVisible(true);
                popup.Show();
            }

            else {
                var count = grdMunicipality.GetSelectedRowCount();
                var value = grdMunicipality.GetRowKey(count)
                if (count > 0) {
                    document.getElementById('<%=hdWhichButton.ClientID%>').value = "Update";
                    lblConfirmation.SetText("Are you sure you want to delete it ?")
                    txtSave.SetVisible(true);
                    txtCancel.SetVisible(true);
                    popup.Show();
                }
                else {
                    lblConfirmation.SetText("Please select the municipality to delete.")
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
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Update";
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
                        HeaderText="Manage Municipalities">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblMunicipality" runat="server" Width="120px" Text="Municipality">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtMunicipality" ClientInstanceName="txtMunicipality" MaxLength="30" CssClass="UpperCase" runat="server" Width="210">
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
                                            <dx:ASPxLabel ID="lblBranch" runat="server" Width="120px" Text="Branch">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" DropDownStyle="DropDownList" runat="server" Width="210px" ValueType="System.String">
                                            </dx:ASPxComboBox>
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
                   <%-- <div style="float: right">
                        <dx:ASPxButton ID="btnDelete" ClientInstanceName="Delete" runat="server" Text="Update Municipalities">
                            <ClientSideEvents Click="confirm"></ClientSideEvents>
                        </dx:ASPxButton>
                    </div>--%>
                    <dx:ASPxGridView ID="grdMunicipality" ClientInstanceName="grdMunicipality" runat="server" AutoGenerateColumns="False" Width="100%" EnableTheming="True" KeyFieldName="branch_code" OnDataBinding="grdMunicipality_DataBinding">
                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Municipality" FieldName="municipality" VisibleIndex="1">
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
