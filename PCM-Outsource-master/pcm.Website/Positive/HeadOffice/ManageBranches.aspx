<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageBranches.aspx.vb" Inherits="pcm.Website.ManageBranches" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript">
        var alt;
        function onEnd(s, e) {
            lp.Hide();
        }

        function populate(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Populate";
            lp.Show();
            cab.PerformCallback();
        }


        function bind(s, e) {
            var branchCode = txtBranchCode.GetText()
            if (branchCode == '') {
                ASPxClientUtils.PreventEvent(e.htmlEvent);

                lblConfirmation.SetText("Please enter branch code")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            //ASPxClientUtils.PreventEvent(e.htmlEvent);
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Populate";
            lp.Show();
            cab.PerformCallback();

        }


        function clear(s, e) {
            e.processOnServer = false;
           <%-- document.getElementById('<%=hdWhichButton.ClientID%>').value = "Clear";
            lp.Show();
            cab.PerformCallback();--%>
            txtBranchCode.SetText("")
            txtAddressLine1.SetText("")
            txtAddressLine2.SetText("")
            txtAddressLine3.SetText("")
            txtAddressLine4.SetText("")
            txtAddressLine5.SetText("")
            txtBranchName.SetText("")
            txtTelephoneNumber.SetText("")
            txtFaxNumber.SetText("")
            txtEmail.SetText("")
            txtTaxNumber.SetText("")
            txtMerchantNumber.SetText("")
            cboBranchType.SetSelectedIndex(-1)
            cboRegion.SetSelectedIndex(-1)
            cboPriceList.SetSelectedIndex(-1)
            chkBlocked.SetChecked(false)
            txtMunicipality.SetText("")
            txtTradingHoursStart.SetValue(null);;
            txtTradingHoursEnd.SetValue(null);
            cboProvince.SetSelectedIndex(-1)
            txtStoreSquareMetres.SetText("")
            txtLongitude.SetText("")
            txtLatitude.SetText("")
            txtCompanyName.SetText("")
            cboTypeOfMall.SetSelectedIndex(-1)
            txtUrl.SetText("")
            txtBranchNameWeb.SetText("")
        }

        function confirm(s, e) {
            e.processOnServer = false;

            //Do Validation
            var branchcode = txtBranchCode.GetText()
            if (branchcode == '') {
                lblConfirmation.SetText("Please enter branch code")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            if (txtBranchName.GetText() == '') {
                lblConfirmation.SetText("Please enter branch name")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var branchType = cboBranchType.GetValue()
            if (!branchType) {
                lblConfirmation.SetText("Please select branch type ")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var price = cboPriceList.GetValue()
            if (!price) {
                lblConfirmation.SetText("Please select price ")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var province = cboPriceList.GetValue()
            if (!province) {
                lblConfirmation.SetText("Please select price ")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var municipality = txtMunicipality.GetText()
            if (municipality == '') {
                lblConfirmation.SetText("Please enter municipality")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var storeSquareMetres = txtStoreSquareMetres.GetText()
            if (storeSquareMetres == '') {
                lblConfirmation.SetText("Please enter store Square Metres")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var TradingHoursStart = txtTradingHoursStart.GetText()
            if (TradingHoursStart == '') {
                lblConfirmation.SetText("Please enter Trading Hours Start")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }


            var TradingHoursEnd = txtTradingHoursEnd.GetText()
            if (TradingHoursEnd == '') {
                lblConfirmation.SetText("Please enter Trading Hours End")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }


            var Longitude = txtLongitude.GetText()
            if (Longitude == '') {
                lblConfirmation.SetText("Please enter Longitude")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }


            var Latitude = txtLatitude.GetText()
            if (Latitude == '') {
                lblConfirmation.SetText("Please enter Latitude")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var mallType = cboTypeOfMall.GetValue()
            if (!mallType) {
                lblConfirmation.SetText("Please select type of mall")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var companyName = txtCompanyName.GetText()
            if (!companyName) {
                lblConfirmation.SetText("Please enter company name")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            var branchNameWeb = txtBranchNameWeb.GetText()
            if (!branchNameWeb) {
                lblConfirmation.SetText("Please enter branch name web")
                txtSave.SetVisible(false);
                txtCancel.SetVisible(false);
                popup.Show();
                return
            }

            //var storeStatus = txtStoreStatus.GetText()
            //if (!storeStatus) {
            //    lblConfirmation.SetText("Please enter store status")
            //    txtSave.SetVisible(false);
            //    txtCancel.SetVisible(false);
            //    popup.Show();
            //    return
            //}

            // Confirm
            lblConfirmation.SetText("Are you sure you want to add branch " + branchcode.toUpperCase() + "?")
            txtSave.SetVisible(true);
            txtCancel.SetVisible(true);
            popup.Show();
        }

        function searchBranch(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SearchBranch";

            lp.Show();
            cab.PerformCallback();
        }

        function selectBranch(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectBranch";

            lp.Show();
            cab.PerformCallback();
        }

        function OnClickCancel(s, e) {
            e.processOnServer = false;
            popup.Hide();

        }

        function save(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";
            popup.Hide();
            lp.Show();
            cab.PerformCallback();

        }

        function OnKeyDown(s, e) {
            if (!alt) alt = e.htmlEvent.keyCode == 18;

        }
        function OnKeyPress(s, e) {
            var action = s.globalName;
            if (e.htmlEvent.keyCode == 13 && action == "txtBranchCode") {
                bind()
            }
            else {
                var forbiddenChars = /[^a-z\d( )-]+$/ig;
                var key = String.fromCharCode(e.htmlEvent.keyCode);
                if (forbiddenChars.test(key) || alt) {
                    alt = false;
                    e.htmlEvent.preventDefault();
                }
            }

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
                        HeaderText="Manage Branches">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblBranchCode" runat="server" Width="120px" Text="Branch Code">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            <dx:ASPxTextBox ID="txtBranchCode" ClientInstanceName="txtBranchCode" MaxLength="3" CssClass="UpperCase" runat="server" Width="210">
                                                <ClientSideEvents KeyPress="OnKeyPress" KeyDown="OnKeyDown" />
                                            </dx:ASPxTextBox>
                                            <dx:ASPxImage ID="OpenPopup" runat="server" ImageUrl="~/images/search.png">
                                            </dx:ASPxImage>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine1" runat="server" Width="120px" Text="Address Line 1">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine1" ClientInstanceName="txtAddressLine1" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblBranchType" runat="server" Width="120px" Text="Branch Type">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboBranchType" ClientInstanceName="cboBranchType" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine2" runat="server" Width="120px" Text="Address Line 2">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine2" ClientInstanceName="txtAddressLine2" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblBranchName" runat="server" Width="120px" Text="Branch Name">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtBranchName" ClientInstanceName="txtBranchName" CssClass="UpperCase" runat="server" Width="230px">
                                                <ClientSideEvents KeyPress="OnKeyPress" KeyDown="OnKeyDown" />
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine3" runat="server" Width="120px" Text="Address Line 3">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine3" ClientInstanceName="txtAddressLine3" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTelephoneNumber" runat="server" Width="120px" Text="Telephone Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtTelephoneNumber" ClientInstanceName="txtTelephoneNumber" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine4" runat="server" Width="120px" Text="Address Line 4">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine4" ClientInstanceName="txtAddressLine4" CssClass="UpperCase" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblFaxNumber" runat="server" Width="120px" Text="Fax Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtFaxNumber" ClientInstanceName="txtFaxNumber" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAddressLine5" runat="server" Width="120px" Text="Address Line 5">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtAddressLine5" ClientInstanceName="txtAddressLine5" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmail" runat="server" Width="120px" Text="Email Address">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtEmail" ClientInstanceName="txtEmail" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblPriceList" runat="server" Width="120px" Text="Price List">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboPriceList" ClientInstanceName="cboPriceList" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTaxNumber" runat="server" Width="120px" Text="TAX Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxTextBox ID="txtTaxNumber" ClientInstanceName="txtTaxNumber" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>

                                            <dx:ASPxLabel ID="lblRegion" runat="server" Width="120px" Text="Region">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>

                                            <dx:ASPxComboBox ID="cboRegion" ClientInstanceName="cboRegion" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblMerchantNumber" runat="server" Width="120px" Text="Merchant Number">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtMerchantNumber" ClientInstanceName="txtMerchantNumber" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblMunicipality" runat="server" Width="120px" Text="Municipality">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtMunicipality" ClientInstanceName="txtMunicipality" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTradingHoursStart" runat="server" Width="120px" Text="Trading Hours Start">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTimeEdit ID="txtTradingHoursStart" ClientInstanceName="txtTradingHoursStart" Width="230px" runat="server">
                                            </dx:ASPxTimeEdit>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblTradingHoursEnd" runat="server" Width="120px" Text="Trading Hours End">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTimeEdit ID="txtTradingHoursEnd" ClientInstanceName="txtTradingHoursEnd" Width="230px" runat="server">
                                            </dx:ASPxTimeEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblProvince" runat="server" Width="120px" Text="Province">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboProvince" ClientInstanceName="cboProvince" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblStoreSquareMetres" runat="server" Width="120px" Text="Store Square Metres">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <%--<dx:ASPxTextBox ID="txtStoreSquareMetres" ClientInstanceName="txtStoreSquareMetres" runat="server" Width="230px">
                                            </dx:ASPxTextBox>--%>
                                            <dx:ASPxSpinEdit ID="txtStoreSquareMetres" ClientInstanceName="txtStoreSquareMetres" Width="230px" runat="server" DecimalPlaces="2">
                                                <SpinButtons ShowIncrementButtons="false" />
                                            </dx:ASPxSpinEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblTypeOfMall" runat="server" Width="120px" Text="Type Of Mall">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cboTypeOfMall" ClientInstanceName="cboTypeOfMall" Width="100%"
                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblCompanyName" runat="server" Width="120px" Text="Company Name">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtCompanyName" ClientInstanceName="txtCompanyName" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblLongitude" runat="server" Width="120px" Text="Longitude">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtLongitude" ClientInstanceName="txtLongitude" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblLatitude" runat="server" Width="120px" Text="Latitude">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtLatitude" ClientInstanceName="txtLatitude" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblurl" runat="server" Width="120px" Text="URL">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtUrl" ClientInstanceName="txtUrl" runat="server" Width="230px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>  
                                             <dx:ASPxLabel ID="lblBranchNameWeb" runat="server" Width="120px" Text="Branch Name Web">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td> 
                                           <dx:ASPxTextBox ID="txtBranchNameWeb" ClientInstanceName="txtBranchNameWeb" runat="server" Width="230px">
                                           <ClientSideEvents KeyPress="OnKeyPress" KeyDown="OnKeyDown" />
                                           </dx:ASPxTextBox>  
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>                                        
                                            <dx:ASPxLabel ID="lblStoreStatus" runat="server" Width="120px" Text="Store Status">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                        <dx:ASPxTextBox ID="txtStoreStatus" ClientInstanceName="txtStoreStatus" runat="server" Width="230px">
                                           <ClientSideEvents KeyPress="OnKeyPress" KeyDown="OnKeyDown" />
                                           </dx:ASPxTextBox>  
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                        <dx:ASPxLabel ID="lblBlocked" runat="server" Width="120px" Text="Blocked">
                                        </dx:ASPxLabel>
                                        </td>
                                        <td>
                                         <dx:ASPxCheckBox ID="chkBlocked" ClientInstanceName="chkBlocked" runat="server" CheckState="Unchecked">
                                         </dx:ASPxCheckBox>
                                        </td>
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
                                                <dx:ASPxButton AutoPostBack="false" ID="btnSave" runat="server" Text="Save">
                                                    <ClientSideEvents Click="confirm" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>

                                    </tr>
                                </table>
                                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="800px" Height="250px"
                                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                    ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Search Branch"
                                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table border="0" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearchType" runat="server" Text="Search Type"></dx:ASPxLabel>

                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cboSearchType" ClientInstanceName="cboSearchType" Width="100%"
                                                                runat="server" AutoPostBack="false" ValueType="System.String">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <dx:ASPxLabel ID="lblSearchDetails" runat="server" Text="Search Details"></dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtSearch" CssClass="UpperCase" runat="server" Width="200px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                <ClientSideEvents Click="searchBranch" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="12">
                                                            <dx:ASPxListBox Width="760px" ID="lstSearch" runat="server" ValueType="System.String"></dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="11">
                                                            <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" AutoPostBack="false">
                                                                <ClientSideEvents Click="selectBranch" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
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
