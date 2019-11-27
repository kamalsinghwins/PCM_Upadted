<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="PrePacks.aspx.vb" Inherits="pcm.Website.PrePacks" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/StockcodeManager/InventoryAndTax.ascx" TagName="InventoryAndTax"
    TagPrefix="widget" %>



<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .UpperCase input {
            text-transform: uppercase;
        }
    </style>
    <script src="../../js/General/application.js"></script>

    <script>

        function OnPPKeyUp(s, e) {
            e.processOnServer = false;

            var Size = cboSize.GetText();
            if (Size == '') {
                alert('Please select a size grid before inputting a Pre Pack');
                ASPxClientEdit.ClearEditorsInContainerById('prepack');
                return false;
            }

            
        }

        function OnIndexChangePrePack(s, e) {
            e.processOnServer = false;
            
            var Size = cboSize.GetText();
            if (Size == '') {
                alert('Please select a size grid before inputting or selecting a Pre Pack');
                ASPxClientEdit.ClearEditorsInContainerById('prepack');
                return false;
            }

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "PrePackIndexChange";

            lp.Show();
            cab.PerformCallback();

            
        }

        function OnClick(s, e) {
            e.processOnServer = false;

            var Size = cboSize.GetText();
            if (Size == '') {
                alert('Size Grid not selected');
                return false;
            }

            var PrePack = cboPackSize.GetText();
            if (PrePack == '') {
                alert('Pre Pack code not entered');
                return false;
            }


            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Accept";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickClear(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";

            lp.Show();
            cab.PerformCallback();


        }

        function OnIndexChange(s, e) {
            e.processOnServer = false;

            var Size = cboSize.GetText();
            if (Size == '') {
                alert('Please select a size grid');
                return false;
            }

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "IndexChange";

            lp.Show();
            cab.PerformCallback();


        }

        function onEnd(s, e) {
            lp.Hide();

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
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
<SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                 <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                    HeaderText="Pre Packs" CssClass="date_panel">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Size Grid:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSizeGrid" runat="server" Width="200px" Style="margin-top: 10px" ClientInstanceName="cboSize"
                                            ValueType="System.String" TextFormatString="{0}">
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChange" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>

                                </tr>
                                 <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Pre Pack ID:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <div id="prepack">
                                        <dx:ASPxComboBox ID="cboPackSize" runat="server" Width="200px" Style="margin-top: 10px" ClientInstanceName="cboPackSize"
                                            ValueType="System.String" TextFormatString="{0}" DropDownStyle="DropDown" CssClass="UpperCase" MaxLength="15">
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChangePrePack" KeyUp="OnPPKeyUp" />
                                        </dx:ASPxComboBox>
                                            </div>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>

                                </tr>

                               
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <dx:ASPxGridView ID="grdSizes" Width="30%" runat="server" AutoGenerateColumns="False" CssClass="date_panel"
                    KeyFieldName="ID">

                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Size Grid" FieldName="SizeGridNumber" ShowInCustomizationForm="True"
                            VisibleIndex="1">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Size" FieldName="Size" VisibleIndex="2">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Pre Pack Qty" FieldName="PrePack" VisibleIndex="3">
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="txtBox" Width="100%" runat="server" OnInit="PrePack_Init"
                                    Value='<%#Eval("PrePack")%>' Border-BorderStyle="None">
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <SettingsPager Visible="False" PageSize="20">
                    </SettingsPager>

                </dx:ASPxGridView>
                <table class="date_panel" Width="30%">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="cmdClear" style="float:left;" runat="server" Text="Clear">
                                <ClientSideEvents Click="OnClickClear" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="cmdAccept" style="float:right;" runat="server" Text="Accept">
                                <ClientSideEvents Click="OnClick" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
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
