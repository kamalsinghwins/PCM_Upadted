<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="GiftCardDetails.aspx.vb" Inherits="pcm.Website.GiftCardDetails" %>
<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

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

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            width: 150px;
        }

        .auto-style4 {
            width: 104px;
            height: 100%;
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

        .mainContainer {
            padding: 10px;
        }

        .check-with-input {
            float: left;
            margin-right: 10px;
        }

        .input-check-with {
            float: left;
            width: 140px;
        }
    </style>

    <script type="text/javascript">

       
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" 
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <div class="left-side-tables" style="width: 100%; float: left;">
                        <fieldset>
                            <legend>Gift Card Details Report</legend>
                            <table>
                                <tr>
                                    <td class="auto-style3">

                                        <dx:ASPxLabel ID="Label1" runat="server" Text="Card Number">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">

                                        <dx:ASPxTextBox ID="txtAccNum" onkeypress="return isNumber(event)" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                           <dx:ASPxButton ID="cmdQuery" runat="server" Text="Query" >
                                               </dx:ASPxButton>
                                    </td>
                                    <td align="right">
                                     <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" >
                                    </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <table  style ="width:100%">
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td></td>
                                <td>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="pB" runat="server" Text=""></dx:ASPxLabel>
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2" >
                                  
                                </td>
                                <td>
                                     
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2" >
                                 
                                </td>
                                <td>
                                     
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #666666; font-family: Tahoma; font-size: 14px;"  colspan="5">
                                    <dx:ASPxGridView ID="grdGiftCardDetails" runat="server" AutoGenerateColumns="False"  OnDataBinding ="grdGiftCardDetails_DataBinding"
                                        Width="100%">
                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <Columns>

                                            <dx:GridViewDataTextColumn FieldName="balance" Caption="Balance" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="total_spent" Caption="Total Spent" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="date_created" Caption="Date Created" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="date_modified" Caption="Date Modified" VisibleIndex="4" />
                                            <dx:GridViewDataTextColumn FieldName="date_last_used" Caption="Date Last Used" VisibleIndex="5" />
                                            <dx:GridViewDataTextColumn FieldName="current_status" Caption="Current Status" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="created_by" Caption="Created By" VisibleIndex="7" />
                                          
                                        </Columns>
                                    </dx:ASPxGridView>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2" >
                                 
                                </td>
                                <td>
                                     
                                </td>
                            </tr>
                            <tr>
                             <td style="color: #666666; font-family: Tahoma; font-size: 14px;"  colspan="5">
                                    <dx:ASPxGridView ID="grdCardtransactionsDetails" runat="server" AutoGenerateColumns="False" OnDataBinding ="grdCardtransactionsDetails_DataBinding"
                                        Width="100%">

                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <Columns>

                                            <dx:GridViewDataTextColumn FieldName="sale_date" Caption="Date" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="sale_time" Caption="Time" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="username" Caption="User" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="transaction_type" Caption="Type" VisibleIndex="4" />
                                            <dx:GridViewDataTextColumn FieldName="reference_number" Caption="Reference" VisibleIndex="5" />
                                            <dx:GridViewDataTextColumn FieldName="transaction_amount" Caption="Amount" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="current_period" Caption="Period" VisibleIndex="7" />
                                           
                                        </Columns>
                                        <Settings ShowFooter="true" />
                                        <TotalSummary>
                                        <dx:ASPxSummaryItem DisplayFormat="Balance: {0}" FieldName="transaction_amount" SummaryType="Sum" />
                                       </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>   
                            </tr>
                        </table>
                     <%--   <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                        </dx:ASPxGridViewExporter>--%>
                        <dx:ASPxGridViewExporter ID="GridExporter1" runat="server" GridViewID="grdGiftCardDetails" />
                        <dx:ASPxGridViewExporter ID="GridExporter2" runat="server" GridViewID="grdCardtransactionsDetails" />
                    </div>
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
    <script type="text/javascript">
 
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }


    </script>
</asp:Content>

