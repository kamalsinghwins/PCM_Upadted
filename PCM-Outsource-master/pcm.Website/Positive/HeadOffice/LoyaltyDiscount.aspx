<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="LoyaltyDiscount.aspx.vb" Inherits="pcm.Website.LoyaltyDiscount" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">


        function onEnd(s, e) {
            lp.Hide();

        }

        function SubmitForm(s, e) {
            e.processOnServer = false;
            //do validation here
            if (!ASPxClientEdit.ValidateGroup("ErrorGroup")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CreateDiscount";

            lp.Show();
            cab.PerformCallback();
        }

        function OpenAddDiscountPopup(s, e) {
            e.processOnServer = false;
            ASPxClientEdit.ClearEditorsInContainer(null);

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "OpenAddLoyaltyDiscountPopup";

            lp.Show();
            cab.PerformCallback();
        }

        function OpenEditDiscountPopup(s, e) {
            e.processOnServer = false;

            ASPxClientEdit.ClearEditorsInContainer(null);

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "OpenEditLoyaltyDiscountPopup";

            lp.Show();
            cab.PerformCallback();
        }

        function OnlyNumeric(s, e) {
            if (!((e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 57) ||
                (e.htmlEvent.keyCode == 8 || e.htmlEvent.keyCode == 46 || e.htmlEvent.keyCode == 9 || e.htmlEvent.keyCode == 190 || e.htmlEvent.keyCode == 37 ||
                    e.htmlEvent.keyCode == 39)))
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
        }
        function run(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Run";
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
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
                        HeaderText="Loyalty Discounts" CssClass="mb-20">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td class="auto-style2">
                                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2"></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td colspan="3"></td>

                                    </tr>
                                    <tr>
                                        <td class="auto-style2"></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td colspan="3" align="right">
                                            <dx:ASPxButton ID="cmdRun" runat="server" Text="Run">
                                                <ClientSideEvents Click="run" />
                                            </dx:ASPxButton>

                                        </td>

                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <div class="buttons">
                        <div class="text-left pull-left">
                            <dx:ASPxButton ID="btnCreatediscount" runat="server" Text="Create">
                                <ClientSideEvents Click="OpenAddDiscountPopup"></ClientSideEvents>
                            </dx:ASPxButton>
                        </div>
                        <div class="text-right pull-right">
                            <dx:ASPxButton ID="btnEdit" runat="server" Text="Edit Loyalty Discount">
                                <ClientSideEvents Click="OpenEditDiscountPopup"></ClientSideEvents>
                            </dx:ASPxButton>

                        </div>
                    </div>
                    <br />
                    <br />
                    <dx:ASPxGridView ID="grdLoyaltyDiscount" Width="100%" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="discount_name" OnDataBinding="grdLoyaltyDiscount_DataBinding">
                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                        <Columns>
                            <%-- <dx:GridViewCommandColumn  ShowEditButton="true" VisibleIndex="0" ShowUpdateButton ="true" ButtonRenderMode="Button">
                                                                   </dx:GridViewCommandColumn>--%>
                            <dx:GridViewDataTextColumn Caption="Name" FieldName="discount_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Date" FieldName="discount_date" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Discount Percentage" FieldName="discount_percentage" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Active" FieldName="is_enabled" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <SettingsEditing EditFormColumnCount="3" />
                        <Settings ShowFooter="True" ShowFilterRow="true" />

                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                    </dx:ASPxGridViewExporter>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px">
                                </dx:ASPxButton>
                            </td>

                        </tr>
                    </table>
                </div>

                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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

                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="700px" Height="250px"
                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="DiscountPopup"
                    ShowFooter="True" FooterText="" PopupElementID="btnCreateSurvey" HeaderText=""
                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <div class="pull-left">
                                    <asp:HiddenField ID="HDLoyaltyDiscountName" runat="server" />
                                    <table id="tblContainer0">
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblDiscountName" runat="server" Text="Name Of Discount">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtDiscountName" runat="server" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Enter Discount   Name" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Enter Discount Name" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblDiscountDate" runat="server" Text="Date">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxDateEdit ID="txtDiscountDate" runat="server" Font-Bold="True" Width="170px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Select Discount Date" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Select Discount Date" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblDiscountPercentage" runat="server" Text="Discount Percentage">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">

                                                <dx:ASPxTextBox ID="txtDiscountPercentage" runat="server" Width="170px">
                                                    <ClientSideEvents KeyDown="OnlyNumeric" />
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Enter Discount Percentage" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Enter Discount Percentage" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblIsActive" runat="server" Text="Is Active">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxCheckBox ID="chkIsActive" runat="server"></dx:ASPxCheckBox>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxButton ID="cmdDiscount" runat="server" Text="Save" AutoPostBack="false" ValidationGroup="save">
                                                    <ClientSideEvents Click="SubmitForm"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="right">
                                    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList" ValidationGroup="ErrorGroup" ForeColor="#FF3300">
                                    </dx:ASPxValidationSummary>
                                </div>
                            </asp:Panel>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
