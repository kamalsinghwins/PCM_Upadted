<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="DebtorHistory.aspx.vb" Inherits="pcm.Website.DebtorHistory" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>

    <style type="text/css">
        .style3 {
        }

        .style4 {
            width: 104px;
        }

        .style6 {
            width: 422px;
        }

        .auto-style1 {
            width: 104px;
            height: 23px;
        }

        .auto-style3 {
            width: 150px;
        }

        .main_view {
            margin: 20px 0px 0px 20px;
        }

            .main_view .dxtc-strip {
                height: auto !important;
            }

            .main_view table tr td span, .main_view a > .dx-vam {
                text-transform: uppercase;
            }

        .UpperCase {
            text-transform: uppercase;
        }

        .auto-style8 {
            margin-left: 40px;
        }

        .burea_btn {
            margin: 0 5px 10px 0;
        }

        .text-center {
            text-align: center;
        }

        .mb-10 {
            margin-bottom: 10px;
        }

        .ml-10 {
            margin-left: 10px;
        }

        .mainContainer {
            padding: 10px;
        }

        .pull-left {
            float: left;
        }

        .pull-right {
            float: right;
        }

        .p-0 {
            padding: 0;
        }

        .boldLabel
    {
        font-weight: bold;
    
    }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }

        function SelectReport(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SelectReport";

            lp.Show();
            cab.PerformCallback();
        }

        function Query(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Query";

            lp.Show();
            cab.PerformCallback();
        }

        function FindDebtors(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Lookup";

            lp.Show();
            cab.PerformCallback();
        }

        function FillDebtorsDetails(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "DebtorSelected";

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
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>

        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>

                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="History" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <table>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblAccountnumber" runat="server" Text="Account Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8" colspan="3">
                                                <dx:ASPxTextBox ID="txtAccNum" runat="server" CssClass="pull-left " Width="170px">
                                                </dx:ASPxTextBox>
                                                <dx:ASPxImage ID="OpenPopup" runat="server" CssClass="pull-left ml-10" ImageUrl="~/images/search.png">
                                                </dx:ASPxImage>

                                            </td>
                                             <td class="style3">
                                               &nbsp;
                                            </td>
                                            <td>&nbsp; <dx:ASPxButton ID="cmdQuery" runat="server" Text="Query">
                                                    <ClientSideEvents Click="Query"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    
                                    </table>
                                    <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="700px" Height="250px"
                                        MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="LookupMain"
                                        ShowFooter="True" FooterText="" PopupElementID="OpenPopup" HeaderText="Debtor Lookup"
                                        runat="server" PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" EnableHierarchyRecreation="True" AllowDragging="True">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <table border="0" align="center" cellpadding="4" cellspacing="0" id="PopupContentDiv">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="txtSearchFieldlabel" runat="server" Text="Search Field"></dx:ASPxLabel>

                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxComboBox ID="cboSearchType" runat="server" Width="170px" ClientInstanceName="cboSearch">
                                                                            </dx:ASPxComboBox>
                                                                        </td>
                                                                        <td>
                                                                           
                                                                    </td>
                                                                            </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="txtCriteriaLabel" runat="server" Text="Criteria"></dx:ASPxLabel>

                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxTextBox ID="txtCriteria" CssClass="UpperCase" runat="server" Width="170px">
                                                                            </dx:ASPxTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxButton ID="cmdLookUp" runat="server" Text="LookUp" CssClass="px-0" AutoPostBack="false">
                                                                                <ClientSideEvents Click="FindDebtors"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                                <dx:ASPxGridView ID="grdDebtorsSearch" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding="grdDebtorsSearch_DataBinding"  Width="660px">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                     <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="id_number" Caption="ID Number" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="first_name" Caption="First Name" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="last_name" Caption="Last Name" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="status" Caption="Status" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="cardnum" Caption="Card Number" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="cell_number" Caption="Cellphone" VisibleIndex="7" />
                                                                 
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                                <dx:ASPxButton ID="cmdSelect" runat="server" Text="LOAD" Width="100%" AutoPostBack="false" ClientInstanceName="SelectStockcode">
                                                                    <ClientSideEvents Click="FillDebtorsDetails"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                   
                                                </asp:Panel>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                    <br />
                                 
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
                                  
                                </div>
                                   
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </div>
                <div class="date_panel" style="width: 90%; float: left;">
                    
                     <dx:ASPxButton ID="cmdExportToCSV" runat="server" Text="Export To CSV"  CssClass="float_right_menu mb-10" Width="35px" Height="16px" AutoPostBack="false">                                                 
                                                </dx:ASPxButton> 
                     <dx:ASPxNavBar ID="ASPxNavBar1" runat="server" Width="100%">
                                        <Groups>
                                            <dx:NavBarGroup  Expanded="False" Text="Account Changes" Name="AccountChanges">
                                                <Items>
                                                    <dx:NavBarItem>
                                                        <Template>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                       <dx:ASPxGridView ID="grdAccountChanges" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding=" grdAccountChanges_DataBinding"  Width="100%">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                     <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="change_date" Caption="Change Date" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="change_time" Caption="Change Time" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="old_value" Caption="Old Value" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="new_value" Caption="New Value" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="username" Caption="UserName" VisibleIndex="7" />
                                                                 
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Template>
                                                    </dx:NavBarItem>

                                                </Items>
                                            </dx:NavBarGroup>
                                            <dx:NavBarGroup Expanded="False" Text="Transaction" Name="Transaction">
                                                <Items>
                                                    <dx:NavBarItem>
                                                        <Template>
                                                            <table>
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                                 <tr>
                                                                    <td></td>
                                                                </tr>
                                                                 <tr>
                                                                    <td></td>
                                                                </tr>
                                                                  <tr>
                                                                    <td></td>
                                                                </tr>
                                                                <tr align="right">
                                                                    <td>
                                                                        <dx:ASPxLabel ID="lblCredit"   FontSize="" CssClass= "boldLabel" runat="server" Text="Credit :"></dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                           <dx:ASPxGridView ID="grdTransaction" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding="grdTransaction_DataBinding"  Width="100%">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                     <dx:GridViewDataTextColumn FieldName="account_number" Caption="ID" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="sale_date" Caption="Sale Date" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="sale_time" Caption="Sale Time" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="current_period" Caption="Current Period" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="username" Caption="UserName" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="reference_number" Caption="Refernce Number" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="transaction_type" Caption="Transaction Type" VisibleIndex="7" />
                                                                   <dx:GridViewDataTextColumn FieldName="transaction_amount" Caption="Transaction Amount" VisibleIndex="8" />
                                                                             <dx:GridViewDataTextColumn FieldName="auth_code" Caption="AuthCode" VisibleIndex="9" />

                                                                          <dx:GridViewDataTextColumn FieldName="pay_type" Caption="Pay Type" VisibleIndex="10" />
                                                                          <dx:GridViewDataTextColumn FieldName="ptp_user" Caption="PTP User" VisibleIndex="11" />
                                                                          <dx:GridViewDataTextColumn FieldName="branch_code" Caption="Branch Code" VisibleIndex="12" />
                                                                        
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Template>
                                                    </dx:NavBarItem>

                                                </Items>
                                            </dx:NavBarGroup>
                                            <dx:NavBarGroup Expanded="False" Text="Closing Balances" Name="ClosingBalances">
                                                <Items>
                                                    <dx:NavBarItem>
                                                        <Template>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                          <dx:ASPxGridView ID="grdClosingBalances" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding="grdClosingBalances_DataBinding"  Width="100%">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                     <dx:GridViewDataTextColumn FieldName="total" Caption="Total" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="current_balance" Caption="Current Balance" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="p30" Caption="p30" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="p60" Caption="p60" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="p90" Caption="p90" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="p120" Caption="p120" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="p150" Caption="p150" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="current_period" Caption="Current Period" VisibleIndex="8" />
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Template>
                                                    </dx:NavBarItem>
                                                </Items>
                                            </dx:NavBarGroup>
                                            <dx:NavBarGroup Expanded="False" Text="Age Analysis" Name="AgeAnalysis">
                                                <Items>
                                                    <dx:NavBarItem>
                                                        <Template>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxGridView ID="grdAgeAnalysis" runat="server" AutoGenerateColumns="False" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding="grdAgeAnalysis_DataBinding"  Width="100%">

                                                                    <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                      <dx:GridViewDataTextColumn FieldName="total" Caption="Total" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="current_balance" Caption="Current Balance" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="p30" Caption="p30" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="p60" Caption="p60" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="p90" Caption="p90" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="p120" Caption="p120" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="p150" Caption="p150" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="total_spent" Caption="Total Spent" VisibleIndex="8" />
                                                                 
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Template>
                                                    </dx:NavBarItem>
                                                </Items>
                                            </dx:NavBarGroup>
                                            <dx:NavBarGroup Text="Payment Plans" Expanded="False" Name="Payment">
                                                <Items>
                                                    <dx:NavBarItem>
                                                        <Template>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                          <dx:ASPxGridView ID="grdPayment" runat="server" AutoGenerateColumns="False" Settings-HorizontalScrollBarMode="Auto" ClientInstanceName="PopupGrid"
                                                                 OnDataBinding="grdPayment_DataBinding"  Width="100%" >
                                                                                
                                                                    <SettingsBehavior AllowSelectByRowClick="True"  AllowSort="False" />
                                                                   

                                                                    <Columns>
                                                                      <dx:GridViewDataTextColumn FieldName="financial_payment_plans_id" Caption="Id" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="sale_date" Caption="Sale Date" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="sale_time" Caption="Sale Time" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="reference_number" Caption="Reference Number" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="total_amount" Caption="Total" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="current_period" Caption="Current Period" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 1" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_1" Caption="Amount 1" VisibleIndex="8" />
                                                                   <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 2" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_2" Caption="Amount 2" VisibleIndex="8" />
                                                                   <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 3" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_3" Caption="Amount 3" VisibleIndex="8" />
                                                                   <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 4" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_4" Caption="Amount 4" VisibleIndex="8" />
                                                                   <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 5" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_5" Caption="Amount 5" VisibleIndex="8" />
                                                                   <dx:GridViewDataTextColumn FieldName="period_1" Caption="Period 6" VisibleIndex="7" />
                                                                  <dx:GridViewDataTextColumn FieldName="amount_6" Caption="Amount 6" VisibleIndex="8" />
                                                                  
                                                                 
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Template>
                                                    </dx:NavBarItem>
                                                </Items>
                                            </dx:NavBarGroup>
                                         
                                        </Groups>

                                    </dx:ASPxNavBar>
                </div>
          

                <div>
                      <dx:ASPxGridView ID="grdHidden" runat="server" Visible="false" >
                          <Columns>
                                                                     <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="1" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="2" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="3" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="4" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="5" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="6" />
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="7" />
                                                                 
                                                                    </Columns>
                      </dx:ASPxGridView>
                
                         <dx:ASPxGridViewExporter ID="GridExporter1" runat="server" GridViewID="grdAccountChanges" />
                        <dx:ASPxGridViewExporter ID="GridExporter2" runat="server" GridViewID="grdTransaction" />
                  <dx:ASPxGridViewExporter ID="GridExporter3" runat="server" GridViewID="grdClosingBalances" />
                        <dx:ASPxGridViewExporter ID="GridExporter4" runat="server" GridViewID="grdAgeAnalysis" />
                  <dx:ASPxGridViewExporter ID="GridExporter5" runat="server" GridViewID="grdPayment" />
                     <dx:ASPxGridViewExporter ID="GridExporter6" runat="server"  GridViewID="grdHidden" />
             </div>
                      
                <asp:HiddenField ID="hdWhichButton" runat="server" />
   
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
