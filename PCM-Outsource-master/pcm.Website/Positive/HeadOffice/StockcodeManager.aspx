<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master"
    CodeBehind="StockcodeManager.aspx.vb" Inherits="pcm.Website.StockcodeManager" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%--<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/StockcodeManager/InventoryAndTax.ascx" TagName="InventoryAndTax"
    TagPrefix="widget" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 4px;
        }

        .UpperCase input {
            text-transform: uppercase;
        }

        .auto-style2 {
            height: 30px;
        }

        .auto-style3 {
            width: 4px;
            height: 30px;
        }

        .auto-style4 {
            width: 486px;
        }

        .auto-style5 {
            width: 170px;
        }
    </style>
    <script src="../../js/Positive/HeadOffice/StockcodeManager.js"></script>
    <script src="../../js/General/application.js"></script>
    <script>

        function onSearchClick(s, e) {
            e.processOnServer = false;

            var SearchText = cboSearch.GetText();
            var isChecked = chkMasterCode.GetChecked();
            if (SearchText == 'Master Code') {
                if (isChecked == false) {
                    alert('Master Code must be ticked to be able to Search by Master Code');
                    return false;
                }

            }

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Search";

            lp.Show();
            cab.PerformCallback();


        }

        function onSubmitClick(s, e) {
            e.processOnServer = false;

            //do validation here
            

            var isSubmitChecked = chkSure.GetChecked();
           
            if (isSubmitChecked == false) {
                    alert('You must confirm that you want to Submit the stockcode.');
                    return false;
                }

          
                if (!ASPxClientEdit.ValidateGroup("save")) return;
          
            var hiddenvalue = hdClient.Get('colourstring');
            var selectedcolors = selected.itemsValue;
            debugger;
            var colourstring = '';
            for (i = 0; i < selectedcolors.length; i++) {
                  var n = selectedcolors[i].split(" - ");
                    colourstring += i == 0 ? n[0] : ':' + n[0];
            }
            

            hdClient.Set('colourstring', colourstring);
       

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Submit";

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

        function onDeleteClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Delete";

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
        <%--The panel collection for the CallbackPanel--%>
        <PanelCollection>
            <%--The panel content for the Callbackpanel--%>
            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="pnlWarning" runat="server" CssClass="date_panel" HeaderText="Warning"
                    Visible="False" Theme="RedWine">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblWarning" runat="server" Text="You are editing a master item code. All generated codes will get updated with the same values.">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                <div id="all_controls">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Stockcode Manager"
                        CssClass="date_panel">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <%--<widget:InventoryAndTax ID="Inventory" runat="server" />--%>
                                <table>
                                    <tr>
                                        <td colspan="3" class="auto-style2">
                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Inventory" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td class="auto-style2">&nbsp;
                                        </td>
                                        <td class="auto-style3">&nbsp;
                                        </td>
                                        <td class="auto-style2">&nbsp;
                                        </td>
                                        <td class="auto-style2">&nbsp;
                                        </td>
                                        <td colspan="3" class="auto-style2">
                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Tax" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage4" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td class="auto-style2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Stockcode:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtStockcode" runat="server" CssClass="UpperCase" Width="170px"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save" MaxLength="20" ClientInstanceName="stockcode">
                                                <ValidationSettings CausesValidation="True" SetFocusOnError="True" ErrorText="A Stockcode is required">
                                                    <RequiredField IsRequired="True" ErrorText="A Stockcode is required" />
                                                    <RegularExpression ValidationExpression="^[a-zA-Z0-9]*$" ErrorText="Only AlphaNumeric characters are allowed in the Stockcode" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>

                                        </td>
                                        <td>
                                            <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Purchase Tax:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxComboBox ID="cboPurchaseTax" runat="server" ClientInstanceName="purchasetax">
                                                <ClientSideEvents SelectedIndexChanged="purchasetaxupdated"></ClientSideEvents>

                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Barcode:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtBarcode" runat="server" CssClass="UpperCase" Width="170px" NullText="AUTO GENERATED" MaxLength="30"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save">
                                                <ValidationSettings CausesValidation="True" SetFocusOnError="True" ErrorText="Only Numeric characters are allowed in the Barcode">
                                                    <RegularExpression ValidationExpression="^[0-9]*$" ErrorText="Only Numeric characters are allowed in the Barcode" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                            <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="526px"
                                                MaxWidth="800px" MaxHeight="250px" MinWidth="150px" ID="ASPxPopupControl1"
                                                ShowFooter="True" FooterText="" PopupElementID="txtBarcode" HeaderText="Hint"
                                                runat="server" PopupHorizontalAlign="OutsideRight" EnableHierarchyRecreation="True" SaveStateToCookies="True" SaveStateToCookiesID="BarcodeHint" AllowDragging="True">

                                                <ContentCollection>
                                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                                        <asp:Panel ID="Panel2" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td class="auto-style4">
                                                                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="To let the barcode be generated automatically, leave the barcode field blank.">
                                                                        </dx:ASPxLabel>

                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style4">
                                                                        <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="You cannot specify a barcode for a Master Code as the barcodes for the Generated Codes will be automatically generated.">
                                                                        </dx:ASPxLabel>

                                                                    </td>

                                                                </tr>

                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PopupControlContentControl>
                                                </ContentCollection>

                                            </dx:ASPxPopupControl>

                                        </td>
                                        <td></td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Sales Tax:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxComboBox ID="cboSalesTax" ClientInstanceName="salestax" runat="server" ValueType="System.String">

                                                <ClientSideEvents SelectedIndexChanged="saletaxupdated" />
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Description:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtDescription" runat="server" CssClass="UpperCase" Width="170px"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save" MaxLength="50">
                                                <ValidationSettings SetFocusOnError="True" ErrorText="A Description is required">
                                                    <RequiredField IsRequired="True" ErrorText="A Description is required" />
                                                    <RegularExpression ValidationExpression="^[a-zA-Z0-9\s]*$" ErrorText="Only AlphaNumeric characters are allowed in the Description" />
                                                </ValidationSettings>
                                                <ClientSideEvents KeyPress="DescriptionRegEx"></ClientSideEvents>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
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
                                                                                    <dx:ASPxComboBox ID="cboSearchType" runat="server" Width="250px" ClientInstanceName="cboSearch">
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="chkMasterCode" runat="server" CheckState="Unchecked" Text="Select Master Code" ClientInstanceName="chkMasterCode">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
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

<EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                                                            <Columns>
                                                                                <dx:GridViewDataTextColumn FieldName="stockcode" Caption="Stockcode" VisibleIndex="1" />
                                                                                <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="2" />
                                                                            </Columns>
<SettingsAdaptivity>
<AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
</SettingsAdaptivity>

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
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Supplier Information" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="3">
                                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Categories" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage5" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Supplier:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="drpSupplier" runat="server" Width="170px" CssClass="UpperCase"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save"
                                                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="1"
                                                IncrementalFilteringDelay="1" EnableCallbackMode="true" OnItemsRequestedByFilterCondition="cboSupplier_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="cboSupplier_OnItemRequestedByValue_SQL" NullText="Start typing..."
                                                TextFormatString="{0}">
                                                <%-- <ClientSideEvents KeyPress="CodeRegEx" />--%>
                                                <ClientSideEvents Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = 'upperCase';}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}" />
                                                <ClientSideEvents KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = &#39;upperCase&#39;;}"></ClientSideEvents>
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Supplier Code" FieldName="supplier_code" />
                                                    <dx:ListBoxColumn Caption="Supplier Name" FieldName="supplier_name" />
                                                </Columns>
                                                <ValidationSettings SetFocusOnError="True" ErrorText="A Description is required">
                                                    <RegularExpression ValidationExpression="^[a-zA-Z0-9]*$" ErrorText="Only AlphaNumeric characters are allowed in the Supplier" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Category 1:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxComboBox ID="cboCategory1" runat="server" Width="170px"
                                                DropDownStyle="DropDownList" ValueField="category_code"
                                                ValueType="System.String" TextFormatString="{0}" IncrementalFilteringMode="StartsWith"
                                                CallbackPageSize="10">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Category Code" FieldName="category_code" />
                                                    <dx:ListBoxColumn Caption="Category Description" FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                            <%--<dx:ASPxComboBox ID="cboCategory1" runat="server" Width="170px" ValueType="System.String"
                                                EnableCallbackMode="true" CallbackPageSize="10"
                                                ValueField="category_code" ClientInstanceName="cboCategory1"
                                                OnItemsRequestedByFilterCondition="cboCategory1_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="cboCategory1_OnItemRequestedByValue_SQL"
                                                TextFormatString="{0}">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Category Code" FieldName="category_code" />
                                                    <dx:ListBoxColumn Caption="Category Description" FieldName="category_description" />
                                                </Columns>
                                                <%--<ClientSideEvents SelectedIndexChanged="OnIndexChange" />
                                               
                                            </dx:ASPxComboBox>--%>

                                            <%-- <dx:ASPxComboBox ID="cboCategory1" runat="server" Width="170px" ValueType="System.String"
                                                EnableCallbackMode="true" OnItemsRequestedByFilterCondition="cboCategory1_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="cboCategory1_OnItemRequestedByValue_SQL" NullText="Start typing..."
                                                TextFormatString="{0}">
                                               
                                                <ClientSideEvents KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = &#39;upperCase&#39;;}"></ClientSideEvents>
                                                <ClientSideEvents Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = 'upperCase';}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}" />
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Category Code" FieldName="category_code" />
                                                    <dx:ListBoxColumn Caption="Category Description" FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>--%>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Supplier Item Code:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtSupplierItemCode" runat="server" CssClass="UpperCase" Width="170px" MaxLength="30"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save">
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RegularExpression ValidationExpression="^[a-zA-Z0-9]*$" ErrorText="Only AlphaNumeric characters are allowed in the Supplier Code" />
                                                </ValidationSettings>

                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Category 2:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxComboBox ID="cboCategory2" runat="server" Width="170px"
                                                DropDownStyle="DropDownList" ValueField="category_code"
                                                ValueType="System.String" TextFormatString="{0}" IncrementalFilteringMode="StartsWith"
                                                CallbackPageSize="10">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Category Code" FieldName="category_code" />
                                                    <dx:ListBoxColumn Caption="Category Description" FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>

                                            <%--<dx:ASPxComboBox ID="cboCategory2" runat="server" Width="170px" CssClass="UpperCase"
                                                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="1"
                                                IncrementalFilteringDelay="1" EnableCallbackMode="true" OnItemsRequestedByFilterCondition="cboCategory2_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="cboCategory2_OnItemRequestedByValue_SQL" NullText="Start typing..."
                                                TextFormatString="{0}">
                                                <%-- <ClientSideEvents KeyPress="CodeRegEx" />
                                                <ClientSideEvents KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = &#39;upperCase&#39;;}"></ClientSideEvents>
                                                <ClientSideEvents Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = 'upperCase';}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}" />
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="category_code" />
                                                    <dx:ListBoxColumn FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>--%>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Category 3:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxComboBox ID="cboCategory3" runat="server" Width="170px"
                                                DropDownStyle="DropDownList" ValueField="category_code"
                                                ValueType="System.String" TextFormatString="{0}" IncrementalFilteringMode="StartsWith"
                                                CallbackPageSize="10">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Category Code" FieldName="category_code" />
                                                    <dx:ListBoxColumn Caption="Category Description" FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>

                                            <%--   <dx:ASPxComboBox ID="cboCategory3" runat="server" Width="170px" CssClass="UpperCase"
                                                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="1"
                                                IncrementalFilteringDelay="1" EnableCallbackMode="true" OnItemsRequestedByFilterCondition="cboCategory3_OnItemsRequestedByFilterCondition_SQL"
                                                OnItemRequestedByValue="cboCategory3_OnItemRequestedByValue_SQL" NullText="Start typing..."
                                                TextFormatString="{0}">
                                                <%-- <ClientSideEvents KeyPress="CodeRegEx" />
                                                <ClientSideEvents KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = &#39;upperCase&#39;;}"></ClientSideEvents>
                                                <ClientSideEvents Init="function(s,e) {var t = s.GetInputElement(); t.style.textTransform = 'upperCase';}"
                                                    LostFocus="function(s,e) {var t = s.GetInputElement(); s.SetValue(t.value);}"
                                                    KeyUp="function(s,e) {var t = s.GetInputElement(); t.value = t.value.toUpperCase();}" />
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="category_code" />
                                                    <dx:ListBoxColumn FieldName="category_description" />
                                                </Columns>
                                            </dx:ASPxComboBox>--%>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Cost Prices" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage9" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="3">
                                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Selling Prices" Style="margin-bottom: 10px"
                                                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
                                            </dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px; margin-bottom: 6px">
                                            </dx:ASPxImage>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Cost Exclusive:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtCostExclusive" ClientInstanceName="costprice" runat="server"
                                                Width="170px"
                                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                                ValidationSettings-ValidationGroup="save">
                                                <ValidationSettings CausesValidation="True" SetFocusOnError="True" ErrorText="Cost Exclusive is required">
                                                    <RequiredField IsRequired="True" ErrorText="Cost Exclusive is required" />

                                                </ValidationSettings>

                                                <ClientSideEvents KeyPress="ANOnly" KeyUp="OnKeyUpCostPrice"></ClientSideEvents>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Average Cost:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">
                                            <dx:ASPxTextBox ID="txtAverageCost" runat="server" Width="170px" ReadOnly="True">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Cost Inclusive:" ClientInstanceName="costpriceincl"
                                                Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtCostPriceIncl" ClientInstanceName="costpriceincl" runat="server"
                                                Width="170px">

                                                <ClientSideEvents KeyPress="NumericOnly" KeyUp="OnKeyUpCostPriceIncl"></ClientSideEvents>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5"></td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="auto-style5">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
                                            <dx:ASPxGridView ID="grdPrices" Width="98%" runat="server" AutoGenerateColumns="False"
                                                KeyFieldName="ID">

<EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Price Level" FieldName="PriceLevel" ShowInCustomizationForm="True"
                                                        VisibleIndex="1">
                                                        <EditFormSettings Visible="False" />
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Markup" FieldName="Markup" VisibleIndex="2">
                                                        <PropertiesTextEdit ClientInstanceName="Markup">
                                                        </PropertiesTextEdit>
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" ClientInstanceName="Markup" OnInit="txtMarkup_Init" Width="100%"
                                                                runat="server" Value='<%#Eval("Markup")%>' Border-BorderStyle="None">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Sell Excl" FieldName="SellExcl" VisibleIndex="3">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" Width="100%" runat="server" OnInit="txtSellEx_Init" Value='<%#Eval("SellExcl")%>'
                                                                Border-BorderStyle="None">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Sell Incl" FieldName="SellIncl" VisibleIndex="4">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" Width="100%" runat="server" OnInit="txtSellIncl_Init"
                                                                Value='<%#Eval("SellIncl")%>' Border-BorderStyle="None">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="GP" FieldName="GP" VisibleIndex="5">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" Width="100%" runat="server" Value='<%#Eval("GP")%>' Border-BorderStyle="None"
                                                                OnInit="txtGP_Init" ReadOnly="True">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
<SettingsAdaptivity>
<AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
</SettingsAdaptivity>

                                                <SettingsPager Visible="False">
                                                </SettingsPager>
                                            </dx:ASPxGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
                                            <%--<widget:PriceGrid ID="grdPriceI" runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
                                            <dx:ASPxNavBar ID="navGrid" runat="server" Style="margin-top: 5px" Width="98%" ClientInstanceName="navGrid">
                                                <Groups>
                                                    <dx:NavBarGroup Expanded="False" Text="Grids" Name="Grids">
                                                        <Items>
                                                            <dx:NavBarItem>
                                                                <Template>
                                                                    <table>
                                                                        <tr>
                                                                            <td class="auto-style1">
                                                                                <dx:ASPxLabel ID="ASPxLabel1" Style="margin-top: 10px" runat="server" Width="100px"
                                                                                    Text="Size Grid">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td class="auto-style1"></td>
                                                                            <td class="auto-style1">
                                                                                <dx:ASPxComboBox ID="cboSizeGrid" runat="server" Width="200px" Style="margin-top: 10px"
                                                                                    ValueType="System.String" TextFormatString="{0}">
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                            <td></td>
                                                                            <td class="auto-style1"></td>
                                                                            <td class="auto-style1"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Available Colours">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Selected Colours">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxListBox ID="lstAvailable" runat="server" Height="200px" ValueType="System.String"
                                                                                    ClientInstanceName="available" Width="200px">

                                                                                    <%-- <Columns>
                                                                        <dx:ListBoxColumn Caption="Colour Code" FieldName="colour_code" />
                                                                         <dx:ListBoxColumn Caption="Colour Description" FieldName="colour_description" />
                                                                    </Columns>--%>
                                                                                </dx:ASPxListBox>
                                                                                <dx:ASPxListBox ID="lstRemoved" runat="server" Height="200px" ValueType="System.String"
                                                                                    ClientInstanceName="removed" Visible="false" Width="200px">
                                                                                    <%-- <Columns>
                                                                        <dx:ListBoxColumn Caption="Colour Code" FieldName="colour_code" />
                                                                         <dx:ListBoxColumn Caption="Colour Description" FieldName="colour_description" />
                                                                    </Columns>--%>
                                                                                </dx:ASPxListBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="----->">
                                                                                    <ClientSideEvents Click="AddToSelected"></ClientSideEvents>
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton ID="ASPxButton2" Style="margin-top: 3px" runat="server" Text="<-----">
                                                                                    <ClientSideEvents Click="AddToAvailable"></ClientSideEvents>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxListBox ID="lstSelected" runat="server" ValueType="System.String" Height="200px"
                                                                                    ClientInstanceName="selected" Width="200px">
                                                                                    <%--<Columns>
                                                                        <dx:ListBoxColumn Caption="Colour Code" FieldName="colour_code" />
                                                                         <dx:ListBoxColumn Caption="Colour Description" FieldName="colour_description" />
                                                                    </Columns>--%>
                                                                                </dx:ASPxListBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td></td>
                                                                            <td></td>
                                                                            <td></td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </dx:NavBarItem>
                                                        </Items>
                                                    </dx:NavBarGroup>
                                                </Groups>
                                            </dx:ASPxNavBar>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
                                            <dx:ASPxNavBar ID="navOptions" runat="server" Style="margin-top: 5px" Width="98%">
                                                <Groups>
                                                    <dx:NavBarGroup Expanded="False" Text="Options" Name="Options">
                                                        <Items>
                                                            <dx:NavBarItem>
                                                                <Template>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel2" Style="margin-top: 10px" runat="server" Text="Minimum Level:"
                                                                                    Width="100px">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="txtMinimumLevel" Style="margin-top: 10px" runat="server" CssClass="UpperCase"
                                                                                    Width="200px">
                                                                                    <ClientSideEvents KeyPress="NumericOnly" />
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td></td>
                                                                            <td class="auto-style1">&nbsp;
                                                                            </td>
                                                                            <td style="width: 20px;">&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel10" Style="margin-top: 10px" runat="server" Text="Service Item:"
                                                                                    Width="120px">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxCheckBox ID="chkServiceItem" Style="margin-top: 10px" runat="server">
                                                                                </dx:ASPxCheckBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td></td>
                                                                            <td class="auto-style1">&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Not Discountable:" Width="120px">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxCheckBox ID="chkNotDiscountable" runat="server">
                                                                                </dx:ASPxCheckBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td></td>
                                                                            <td class="auto-style1">&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Blocked:" Width="120px">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxCheckBox ID="chkIsBlocked" runat="server">
                                                                                </dx:ASPxCheckBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </dx:NavBarItem>
                                                        </Items>
                                                    </dx:NavBarGroup>
                                                </Groups>
                                            </dx:ASPxNavBar>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
                                            <table width="98%">
                                                <tr>
                                                    <td colspan="2">
                                                        <dx:ASPxCheckBox ID="chkSure" runat="server" CheckState="Unchecked" Text="I would like to Submit this Stockcode"
                                                            ClientInstanceName="chkSure">
                                                            
                                                        </dx:ASPxCheckBox>
                                                    </td>
                                                    <td>&nbsp;</td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="cmdClear" runat="server" AutoPostBack="False" Style="float: left;" Text="Clear">
                                                            <ClientSideEvents Click="onClearClick" />
                                                        </dx:ASPxButton>
                                                        <dx:ASPxButton ID="cmdDelete" runat="server" AutoPostBack="False" Style="float: right;" Text="Delete">
                                                            <ClientSideEvents Click="onDeleteClick" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" ValidationGroup="save">
                                                        </dx:ASPxValidationSummary>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="cmdSubmit" runat="server" AutoPostBack="False" Style="float: right;" Text="Submit">
                                                            <ClientSideEvents Click="onSubmitClick" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </div>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <%--<dx:ASPxHiddenField ID="hdIsForUpdate" SyncWithServer="true" ClientInstanceName="hdIsForUpdate" runat="server"></dx:ASPxHiddenField>--%>
                <dx:ASPxHiddenField ID="hdClient" ClientInstanceName="hdClient" runat="server"></dx:ASPxHiddenField>
                <%--<dx:ASPxHiddenField ID="hdOriginalColoursString" SyncWithServer="true" ClientInstanceName="hdOriginalColoursString" runat="server"></dx:ASPxHiddenField>--%>
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


