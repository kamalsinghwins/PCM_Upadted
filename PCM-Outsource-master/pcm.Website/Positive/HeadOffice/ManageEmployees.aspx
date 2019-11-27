<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageEmployees.aspx.vb" Inherits="pcm.Website.ManageEmployees" %>

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
        //function OnCustomDataCallback(s, e) {
        //    document.getElementById('treeListCountCell').innerHTML = e.result;
        //}
        function OnSelectionChanged(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0);
        }

        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            return ret;
        }

        function OnIndexChange(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cboClockNumbers";

            lp.Show();
            cab.PerformCallback();


        }

        function OnLostFocus(s, e) {
            e.processOnServer = false;

            var User = cboUsers.GetText();
            if (User != '') {
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "LostFocus";

                lp.Show();
                cab.PerformCallback();
            }

        }

        function onEnd(s, e) {
            lp.Hide();

        }

        function OnClickSave(s, e) {
            e.processOnServer = false;

            if (!ASPxClientEdit.ValidateGroup("save")) return;

            //var First = FirstName.GetText();
            //if (FirstName == '') {
            //    alert('No First Name entered');
            //    return false;
            //}

            //var Last = LastName.GetText();
            //if (LastName == '') {
            //    alert('No Last Name entered');
            //    return false;
            //}

            //popup.Show();

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CalculateAge";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickDelete(s, e) {
            e.processOnServer = false;

            var User = cboUsers.GetText();
            if (User == '') {
                alert('No username selected');
                return false;
            }



            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Delete";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickClear(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";

            lp.Show();
            cab.PerformCallback();


        }

        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }

        function Save(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";

            lp.Show();
            cab.PerformCallback();


        }
        function TextBoxKeyDown(s, e) {
            if (!((e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 57)
            ))
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
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
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Employee Clock Number" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboClockNumber" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                            ValueType="System.String" ValueField="employee_number" ClientInstanceName="cboClockNumber"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                                            Width="287px" DropDownStyle="DropDown" CssClass="UpperCase">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="employee_number" />
                                                <dx:ListBoxColumn FieldName="first_name" />
                                                <dx:ListBoxColumn FieldName="last_name" />
                                            </Columns>
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChange" />
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must have a clock number" />
                                                <RegularExpression ValidationExpression="[a-zA-Z0-9]*$" ErrorText="Clock Number must contain alphanumeric only" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="First Name" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtFirstName" runat="server" Width="200px" ClientInstanceName="FirstName" CssClass="UpperCase">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a First Name" />
                                                <RegularExpression ValidationExpression="[a-zA-Z\s]+" ErrorText="First Name must contain alphabets only" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Last Name" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtLastName" runat="server" Width="200px" ClientInstanceName="LastName" CssClass="UpperCase">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input a Last Name" />
                                                <RegularExpression ValidationExpression="[a-zA-Z\s]+" ErrorText="Last Name must contain alphabets only" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="ID Number" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtIDNumber" runat="server" Width="200px" ClientInstanceName="IDNumber" CssClass="UpperCase">
                                            <ValidationSettings ValidationGroup="save">
                                                <RequiredField IsRequired="True" ErrorText="You must input an ID Number" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="emailaddress" runat="server" Text="Email Address" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtEmailAddress" runat="server" Width="200px" ClientInstanceName="EmailAddress" CssClass="UpperCase">
                                            <ValidationSettings ValidationGroup="save">
                                                <RegularExpression ValidationExpression="^\w+([-+.'%]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                                    ErrorText="Invalid e-mail" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="lblCellphone" runat="server" Text="Cellphone">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style8">
                                        <dx:ASPxTextBox ID="txtCellphone" runat="server" Width="200px" MaxLength="12" onkeypress="return IsNumeric(event);" ondrop="return false;" onpaste="return false;">
                                            <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Bank Branch Code" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtBranchCode" runat="server" Width="200px" ClientInstanceName="BranchCode" CssClass="UpperCase">
                                            <ClientSideEvents KeyPress="TextBoxKeyDown" />
                                        </dx:ASPxTextBox>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Bank Account Number" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtAccountNumber" runat="server" Width="200px" ClientInstanceName="AccountNumber"
                                            CssClass="UpperCase">
                                            <ClientSideEvents KeyPress="TextBoxKeyDown" />
                                        </dx:ASPxTextBox>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Last Login" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtLastLogin" runat="server" Width="200px" ClientInstanceName="LastLogin"
                                            ReadOnly="true" CssClass="UpperCase">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Last Logout" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtLastLogout" runat="server" Width="200px" ClientInstanceName="LastLogout"
                                            ReadOnly="true" CssClass="UpperCase">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Enabled" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkEnabled" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Logged In Now" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkLoggedIn" runat="server" ReadOnly="true" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblCasual" runat="server" Text="Casual" Width="120px">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkCasual" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList"
                                            ValidationGroup="save" Width="100%">
                                        </dx:ASPxValidationSummary>
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                            </table>
                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        <dx:ASPxButton ID="cmdClear" runat="server" Text="Clear" Width="83px">
                                            <ClientSideEvents Click="OnClickClear" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <%-- <dx:ASPxButton ID="cmdDelete" runat="server" Text="Delete" Width="83px">
                                            <ClientSideEvents Click="OnClickDelete" />
                                        </dx:ASPxButton>--%>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="cmdSave" runat="server" Text="Save" ValidationGroup="save" Width="83px">
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
                <dx:ASPxPopupControl ID="dxConfirmation" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Confirmation" Width="548px" CloseAction="None" ClientInstanceName="popup"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <div>
                                <div id="Div1">
                                    <dx:ASPxLabel ID="lblConfirmation" runat="server" Text="Please note that this person 27 or older. Are you sure that you would like to add them?"
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
                                                <dx:ASPxButton ID="txtSave" runat="server" Text="Yes" Width="83px">
                                                    <ClientSideEvents Click="Save" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="txtCancel" runat="server" Text="Cancel" Width="83px">
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
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
