<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageUsers.aspx.vb" Inherits="pcm.Website.ManageUsers" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/General/application.js"></script>
    <style type="text/css">
        .UpperCase input {
            text-transform: uppercase;
        }

        .auto-style2 {
            width: 232px;
        }
    </style>
    <script type="text/javascript">
        //function OnCustomDataCallback(s, e) {
        //    document.getElementById('treeListCountCell').innerHTML = e.result;
        //}
        function OnSelectionChanged(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0);
        }

        function OnIndexChange(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cboUsers";

            lp.Show();
            cab.PerformCallback();

        }

        <%--function OnLostFocus(s, e) {
            e.processOnServer = false;

            var User = cboUsers.GetText();
            if (User != '') {
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "LostFocus";

                lp.Show();
                cab.PerformCallback();
            }
                        
        }--%>

        function onEnd(s, e) {
            lp.Hide();

        }

        function OnClickSave(s, e) {
            e.processOnServer = false;

            //var Pass = Password.GetText();
            //if (Pass == '') {
            //    alert('No password entered');
            //    return false;
            //}

            var User = cboUsers.GetText();
            if (User == '') {
                alert('No username entered');
                return false;
            }


            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickClear(s, e) {
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Manage Users" Width="200px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="User:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboUsers" runat="server" ClientInstanceName="cboUsers" TextFormatString="{0}"
                                            CssClass="UpperCase" ValueType="System.String" Width="200px" DropDownStyle="DropDown" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith">
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
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Password:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td style="display: -webkit-inline-box;">
                                        <dx:ASPxTextBox ID="txtPassword" runat="server" Width="180px" ClientInstanceName="Password" CssClass="UpperCase">
                                        </dx:ASPxTextBox>
                                        <dx:ASPxImage ID="imgQ" runat="server" ToolTip="Please enter in the textbox to update or insert the password." Height="20px" Width="20px" ImageUrl="~/Images/faq.png" ShowLoadingImage="True">
                                        </dx:ASPxImage>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblEmail" runat="server" Text="Email:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtEmail" runat="server" Width="200px" ClientInstanceName="Email" CssClass="UpperCase">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Active:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkActive" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="PCM Admin:" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkPCMAdmin" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>

                            <dx:ASPxTreeList ID="treeListAll" runat="server">
                                <Columns>
                                    <dx:TreeListTextColumn FieldName="Name">
                                        <PropertiesTextEdit EncodeHtml="false" />
                                    </dx:TreeListTextColumn>
                                </Columns>
                                <Settings ShowColumnHeaders="False" />

                                <SettingsPopupEditForm VerticalOffset="-1"></SettingsPopupEditForm>

                                <SettingsPopup>
                                    <EditForm VerticalOffset="-1"></EditForm>
                                </SettingsPopup>

                                <Styles>

                                    <%--The checkbox--%>
                                    <SelectionCell BackColor="#EAF1FA">
                                    </SelectionCell>

                                    <%--The area under the down arrow--%>
                                    <TreeLineRoot BackColor="#EAF1FA">
                                    </TreeLineRoot>

                                    <%--The down arrow--%>
                                    <TreeLineFirst BackColor="#EAF1FA">
                                    </TreeLineFirst>

                                    <%--The area under the main checkboxes--%>
                                    <TreeLineMiddle BackColor="#EAF1FA">
                                    </TreeLineMiddle>

                                    <%--Some other area--%>
                                    <TreeLineLast BackColor="#EAF1FA">
                                    </TreeLineLast>


                                    <Cell BackColor="#EAF1FA">
                                        <Paddings PaddingLeft="1px" />
                                    </Cell>

                                </Styles>
                                <Templates>
                                    <DataCell>
                                        <table>
                                            <tr>
                                                <td style="width: 1px;">
                                                    <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl='<%# GetIconUrl(Container) %>'
                                                        Width="1" Height="1" />
                                                </td>

                                                <td style="padding-bottom: 1px;">
                                                    <%# Container.Text %>
                                                </td>
                                            </tr>
                                        </table>
                                    </DataCell>
                                </Templates>
                                <Border BorderWidth="0" />
                                <%--<SettingsBehavior ExpandCollapseAction="NodeDblClick" />--%>

                                <SettingsBehavior AllowAutoFilter="True"></SettingsBehavior>

                                <SettingsCustomizationWindow PopupHorizontalAlign="RightSides" PopupVerticalAlign="BottomSides"></SettingsCustomizationWindow>

                                <SettingsSelection Enabled="True" AllowSelectAll="True" Recursive="True" />
                                <ClientSideEvents SelectionChanged="OnSelectionChanged" />
                            </dx:ASPxTreeList>



                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        <dx:ASPxButton ID="cmdClear" runat="server" Text="Clear" Width="83px">
                                            <ClientSideEvents Click="OnClickClear" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="cmdSave" runat="server" Text="Save" Width="83px">
                                            <ClientSideEvents Click="OnClickSave" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>

                            </table>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="The User file was updated successfully."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
