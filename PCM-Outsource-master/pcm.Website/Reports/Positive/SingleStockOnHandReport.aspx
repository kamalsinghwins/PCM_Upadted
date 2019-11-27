<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SingleStockOnHandReport.aspx.vb" Inherits="pcm.Website.SingleStockOnHandReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
        }

        .auto-style2 {
            width: 100px;
        }

        .auto-style3 {
            width: 225px;
        }
    </style>
    <script src="../../js/Positive/HeadOffice/StockcodeManager.js"></script>
    <script src="../../js/General/application.js"></script>
    <script>

        function onSearchClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Search";

            lp.Show();
            cab.PerformCallback();


        }

        function onFetchClick(s, e) {
            e.processOnServer = false;

            //do validation here
            if (!ASPxClientEdit.ValidateGroup("save")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Fetch";

            lp.Show();
            cab.PerformCallback();

        }

        function onSelectClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Select";

            lp.Show();
            cab.PerformCallback();


        }

        function onEnd(s, e) {
            lp.Hide();

        }

        function onClearClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";

            lp.Show();
            cab.PerformCallback();
        }

        function ANOnly(s, e) {
            if (e.htmlEvent.keyCode != 46 && e.htmlEvent.keyCode > 31
            && (e.htmlEvent.keyCode < 48 || e.htmlEvent.keyCode > 57))

                //if (!(e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 56))
                //    if (!(e.htmlEvent.keyCode = 110))
                ASPxClientUtils.PreventEvent(e.htmlEvent);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SideHolder" runat="server">
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
        ClientInstanceName="lp">
    </dx:ASPxLoadingPanel>
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
<SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <%--The panel collection for the CallbackPanel--%>
        <PanelCollection>
            <%--The panel content for the Callbackpanel--%>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server"
                    HeaderText="Stock On Hand" CssClass="date_panel" Width="330px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td style="padding-right: 20px; vertical-align: top;">
                                        <table>


                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Branch"
                                                        Font-Size="12pt">
                                                    </dx:ASPxLabel>
                                                    &nbsp;</td>
                                                <td class="auto-style3" colspan="2">
                                                    <dx:ASPxComboBox ID="cboBranch" ClientInstanceName="cboBranch" Width="225px"
                                                        runat="server"
                                                        ValueType="System.String">

                                                        <ValidationSettings ValidationGroup="save">
                                                            <RequiredField IsRequired="True" ErrorText="You must select a Branch" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Stockcode" Font-Size="12pt" Width="120px">
                                                    </dx:ASPxLabel>
                                                </td>

                                                <td class="auto-style3">

                                                    <dx:ASPxTextBox ID="txtStockcode" runat="server" CssClass="UpperCase" Width="225px"
                                                        ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                        ValidationSettings-ValidationGroup="save" MaxLength="20" ClientInstanceName="stockcode">
                                                        <ValidationSettings CausesValidation="True" SetFocusOnError="True" ErrorText="A Stockcode is required">
                                                           <%-- <RequiredField IsRequired="True" ErrorText="A Stockcode is required" />
                                                            <RegularExpression ValidationExpression="^[a-zA-Z0-9]*$" ErrorText="Only AlphaNumeric characters are allowed in the Stockcode" />--%>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>

                                                </td>
                                                <td class="auto-style3">
                                                    <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="~/images/search.png">
                                                    </dx:ASPxImage>
                                                </td>
                                                <td></td>
                                            </tr>

                                            <tr>
                                                <td>&nbsp;</td>
                                                <td class="auto-style3">
                                                    <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="526px" Height="250px"
                                                        MaxWidth="800px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="pcMain"
                                                        ShowFooter="True" FooterText="" PopupElementID="imgQ" HeaderText="Stockcode Search"
                                                        runat="server" PopupHorizontalAlign="WindowCenter" EnableHierarchyRecreation="True">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                                <asp:Panel ID="Panel1" runat="server">
                                                                    <table border="0" align="center" cellpadding="4" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dx:ASPxTextBox ID="txtStockcodeSearch" CssClass="UpperCase" runat="server" Width="250px">
                                                                                            </dx:ASPxTextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                                                <ClientSideEvents Click="onSearchClick"></ClientSideEvents>
                                                                                            </dx:ASPxButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                                                <dx:ASPxGridView ID="grdStockcodeSearch" runat="server" AutoGenerateColumns="False"
                                                                                    OnDataBinding="grdStockcodeSearch_DataBinding" Width="425px">
                                                                                    <Columns>
                                                                                        <dx:GridViewDataTextColumn FieldName="stockcode" Caption="Stockcode" VisibleIndex="1" />
                                                                                        <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="2" />
                                                                                    </Columns>
                                                                                    <SettingsBehavior AllowSelectByRowClick="true" EnableRowHotTrack="True" AllowSort="False" />
                                                                                    <SettingsBehavior AllowSort="False" AllowSelectByRowClick="True" EnableRowHotTrack="True"></SettingsBehavior>
                                                                                </dx:ASPxGridView>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">&nbsp;
                                                                <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" Width="425" AutoPostBack="false" ClientInstanceName="SelectStockcode">
                                                                    <ClientSideEvents Click="onSelectClick"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                        <%--<ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />--%>
                                                    </dx:ASPxPopupControl>
                                                </td>
                                                <td class="auto-style3">&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>

                                            <tr>
                                                <td class="auto-style77">&nbsp;</td>
                                                <td class="auto-style3" colspan="2">
                                                    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                                        ValidationGroup="save" Width="319px">
                                                    </dx:ASPxValidationSummary>
                                                    &nbsp;</td>
                                                <td class="note">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style77">
                                                    <dx:ASPxButton ID="cmdClear" runat="server" Style="float: right;" Text="Clear" AutoPostBack="false">
                                                        <ClientSideEvents Click="onClearClick"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td class="auto-style3" colspan="2">
                                                    <dx:ASPxButton ID="cmdFetch" runat="server" Style="float: right;" Text="Fetch" AutoPostBack="false">
                                                        <ClientSideEvents Click="onFetchClick"></ClientSideEvents>
                                                    </dx:ASPxButton>

                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
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
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="There was an error updating this account. Please contact support."
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
