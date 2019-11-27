<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SendMarketingSMS.aspx.vb" Inherits="pcm.Website.SendMarketingSMS" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .mb-20 {
            margin-bottom: 20px;
        }

        .ellipsis {
            text-overflow: ellipsis;
            overflow: hidden;
            max-width: 100px !important;
        }

        .mr-20 {
            margin-right: 20px;
        }

        .mt-20 {
            margin-top: 20px;
        }

        .text-transform-capitalize {
            text-transform: capitalize;
        }

        .auto-style2 {
            width: 140px;
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function PhoneNumbersBuy_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "PhoneNumbersBuy";
             lp.Show();
            cab.PerformCallback();
        }

        function PhoneNumbersAll_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "PhoneNumbersAll";
             lp.Show();
            cab.PerformCallback();
        }

        function Branch_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Branch";
             lp.Show();
            cab.PerformCallback();
        }

          function Now_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Now";
               lp.Show();
            cab.PerformCallback();
        }

        function Later_changed(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Later";
            lp.Show();
            cab.PerformCallback();
        }

          function Save(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
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
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%"
                        HeaderText="Send Marketing SMS" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblCellPhoneNumbers" runat="server" Text="Cellphone Numbers (Can Buy)" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxRadioButton ID="radCellPhoneNumbersBuy" GroupName="numbers" runat="server">
                                                <ClientSideEvents CheckedChanged="PhoneNumbersBuy_changed" />

                                            </dx:ASPxRadioButton>
                                        </td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblCellPhonenumbersAll" runat="server" Text="Cellphone Numbers (All)" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxRadioButton ID="radCellPhoneNumbersAll" GroupName="numbers" runat="server">
                                                <ClientSideEvents CheckedChanged="PhoneNumbersAll_changed" />

                                            </dx:ASPxRadioButton>
                                        </td>
                                        <td>`
                                        </td>
                                        <td>&nbsp;</td>


                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblBranch" runat="server" Text="Cellphone By Branch" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxRadioButton ID="radBranch" AutoPostBack="false"  GroupName="numbers" runat="server">
                                                <ClientSideEvents CheckedChanged="Branch_changed" />
                                            </dx:ASPxRadioButton>
                                        </td>
                                        <td colspan="2">
                                            <dx:ASPxComboBox ID="cboBranch" 
                                                runat="server" AutoPostBack="false"
                                                ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
          
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblDateTime" runat="server" Text="Sending Options" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxRadioButton ID="radNow" GroupName="time" runat="server">
                                                <ClientSideEvents CheckedChanged="Now_changed" />

                                            </dx:ASPxRadioButton>
                                            Send Now
                                        </td>

                                        <td>
                                            <dx:ASPxRadioButton ID="radLater" GroupName="time" runat="server">
                                                <ClientSideEvents CheckedChanged="Later_changed" />

                                            </dx:ASPxRadioButton>
                                            Send Later
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblDate" runat="server" Text="Date Time" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                             <dx:ASPxDateEdit ID="txtDate" runat="server" Font-Bold="True"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True" Date="10/29/2018 10:30:46" MinDate="2018-10-29">
                                            </dx:ASPxDateEdit>
                                        </td>

                                        <td>
                                           <dx:ASPxTimeEdit ID="txtTime" runat="server" DateTime="10/29/2018 12:39:55">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <ValidationSettings ErrorDisplayMode="None" />
                                            </dx:ASPxTimeEdit>
                                        </td>


                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Message" Width="100%">
                                            </dx:ASPxLabel>
                                        </td>
                                       

                                        <td colspan="2">
                                            <dx:ASPxMemo ID="txtMessage" runat="server" Height="71px" Width="520px"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save" ValidationSettings-ErrorText="ttt">
                                                <ClientSideEvents KeyDown="RecalculateCharsRemaining" KeyUp="RecalculateCharsRemaining"
                                                    GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
                                                    Init="function(s, e) { InitMemoMaxLength(s, 160); RecalculateCharsRemaining(s); }"></ClientSideEvents>
                                                <ValidationSettings SetFocusOnError="True" ErrorText="Please enter a message'">
                                                </ValidationSettings>
                                            </dx:ASPxMemo>  <span class="chrm">
                                                <dx:ASPxLabel ID="txtMessage_cr" runat="server" EnableClientSideAPI="True" />
                                            </span>
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                      
                                        </td>

                                        <td>
                                          
                                        </td>


                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                              <dx:ASPxButton ID="cmdSave" runat="server" AutoPostBack="False" Style="height: 23px" Text="Send">
                                                <ClientSideEvents Click="Save" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>                            
                                <asp:HiddenField ID="HiddenField1" runat="server" />

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>

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


</asp:Content>

