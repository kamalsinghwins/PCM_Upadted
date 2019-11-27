<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CategoryMaintenance.aspx.vb" Inherits="pcm.Website.CategoryMaintenance" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }
        function selectCategory1(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectCategory";
            document.getElementById('<%=hdWhichButton1.ClientID%>').value = "Category1";
            lp.Show();
            cab.PerformCallback();
        }
        function selectCategory2(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectCategory";
            document.getElementById('<%=hdWhichButton1.ClientID%>').value = "Category2";
            lp.Show();
            cab.PerformCallback();
        }
        function selectCategory3(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectCategory";
            document.getElementById('<%=hdWhichButton1.ClientID%>').value = "Category3";
            lp.Show();
            cab.PerformCallback();
        }
        function save(s, e) {
            e.processOnServer = false;
            if (document.getElementById('<%=hdWhichButton1.ClientID%>').value == "Save") {
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
            }
            else {
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "Delete";
            }
            popup.Hide();
            lp.Show();
            cab.PerformCallback();
        }
        function deleteCategory(s, e) {
            e.processOnServer = false;
            var index = CategoryTab.GetActiveTabIndex();
            if (index == 0) {
                lblConfirmation.SetText("Please select the code to delete.")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return false
            }
            lblConfirmation.SetText("Are you sure you want to delete it ?")
            txtSave.SetVisible(true);
            txtCancel.SetVisible(true);
            document.getElementById('<%=hdWhichButton1.ClientID%>').value = "Delete";
            popup.Show();
        }
        function confirm(s, e) {
            e.processOnServer = false;
            if (txtCategoryCode.GetText() == '') {
                lblConfirmation.SetText("Please input a Category Codes.")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            if (txtCategoryDescription.GetText() == '') {
                lblConfirmation.SetText("A Category Description is required")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var category = cboCategory.GetValue()
            if (!category) {
                lblConfirmation.SetText("Please select a Category Number ")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }
            lblConfirmation.SetText("Are you sure you want to add category " + txtCategoryCode.GetText().toUpperCase() + "?")
            txtSave.SetVisible(true);
            txtCancel.SetVisible(true);
            document.getElementById('<%=hdWhichButton1.ClientID%>').value = "Save";
            popup.Show();
        }
        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }
        function clear(s, e) {
            e.processOnServer = false
            txtCategoryCode.SetText("")
            txtCategoryDescription.SetText("")
            cboCategory.SetSelectedIndex(-1)
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
                    <dx:ASPxPageControl ID="CategoryTab" Height="400px" runat="server" ActiveTabIndex="0" ClientInstanceName="CategoryTab" CssClass="main_view">
                        <TabPages>
                            <dx:TabPage Text="Add/Edit">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                        <table width="760px">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblCategoryCode" runat="server" Width="120px" Text="Category Code">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td style="display: -webkit-inline-box;">
                                                    <dx:ASPxTextBox ID="txtCategoryCode" ClientInstanceName="txtCategoryCode" MaxLength="4" CssClass="UpperCase" runat="server" Width="230">
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
                                                    <dx:ASPxLabel ID="lblCategoryDescription" runat="server" Width="120px" Text="Category Description">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtCategoryDescription" ClientInstanceName="txtCategoryDescription" CssClass="UpperCase" runat="server" Width="230px">
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
                                                    <dx:ASPxLabel ID="lblCategoryNumber" runat="server" Width="120px" Text="Category Number">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cboCategory" ClientInstanceName="cboCategory" Width="230px"
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
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td>&nbsp;</td>
                                                <td></td>
                                                <td></td>

                                            </tr>                                                    
                                        </table>
                                        <div style="float: right; margin-right: 277px">
                                            <dx:ASPxButton ID="btnClear" runat="server" Text="Clear">
                                                        <ClientSideEvents Click="clear" />
                                                    </dx:ASPxButton>
                                                        <dx:ASPxButton AutoPostBack="false" ID="btnSave" runat="server" Text="Add">
                                                            <ClientSideEvents Click="confirm" />
                                                        </dx:ASPxButton>
                                        </div>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="Category 1">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl2" runat="server">
                                        <dx:ASPxListBox Height="400px" Width="760px" ID="lstCategory1" runat="server" ValueType="System.String">
                                            <ClientSideEvents ItemDoubleClick="selectCategory1" />
                                        </dx:ASPxListBox>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="Category 2">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl3" runat="server">
                                        <dx:ASPxListBox Height="400px" Width="760px" ID="lstCategory2" runat="server" ValueType="System.String">
                                            <ClientSideEvents ItemDoubleClick="selectCategory2" />
                                        </dx:ASPxListBox>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="Category 3">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl4" runat="server">
                                        <dx:ASPxListBox Height="400px" Width="760px" ID="lstCategory3" runat="server" ValueType="System.String">
                                            <ClientSideEvents ItemDoubleClick="selectCategory3" />
                                        </dx:ASPxListBox>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>
                    <br />
                     <div style="float: right; margin-right: 180px">
                    <dx:ASPxButton ID="cmdDelete" runat="server" Text="Delete"
                                    AutoPostBack="false" >
                                    <ClientSideEvents Click="deleteCategory"></ClientSideEvents>
                                </dx:ASPxButton>
                </div>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <asp:HiddenField ID="hdWhichButton1" runat="server" />
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
    </dx:ASPxCallbackPanel>
</asp:Content>
