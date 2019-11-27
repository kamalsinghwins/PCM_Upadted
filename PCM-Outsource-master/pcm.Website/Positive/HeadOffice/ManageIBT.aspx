<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageIBT.aspx.vb" Inherits="pcm.Website.ManageIBT" %>

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

        function update(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Update";
            lp.Show();
            cab.PerformCallback();
        }

        function clear(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";
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
                        HeaderText="Manage IBT">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblSentFrom" runat="server" Width="120px" Text="Sent From">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboSendBranch" ClientInstanceName="cboBranch" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblSentTo" runat="server" Width="120px" Text="Sent To">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboReceieveBranch" ClientInstanceName="cboBranch" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblIbtNumber" runat="server" Width="120px" Text="IBT Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtIbtNumber" CssClass="UpperCase" runat="server" Width="230px">
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
                                            <dx:ASPxLabel ID="lblNotes" runat="server" Width="120px" Text="Notes">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>                                         
                                            <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="230px" ClientInstanceName="notes"
                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save" ValidationSettings-ErrorText="ttt">
                                <ClientSideEvents KeyDown="RecalculateCharsRemaining" KeyUp="RecalculateCharsRemaining"
                                    GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
                                    Init="function(s, e) { InitMemoMaxLength(s, 2500); RecalculateCharsRemaining(s); }"></ClientSideEvents>
                                <ValidationSettings SetFocusOnError="True" ErrorText="You must fill in a note if you are marking an account as 'Under Investigation'">
                                </ValidationSettings>
                            </dx:ASPxMemo>
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
                                                <dx:ASPxButton AutoPostBack="false" ID="btnUpdate" runat="server" Text="Update IBT">
                                                    <ClientSideEvents Click="update" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>

                                    </tr>
                                </table>
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
