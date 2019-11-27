<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="VintageReport.aspx.vb" Inherits="pcm.Website.VintageReport" %>
<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>--%>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
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
    </style>

    <script type="text/javascript">

       
        function onEnd(s, e) {
            lp.Hide();
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
                <%-- <dx:ASPxNavBar ID="ASPxNavBar2" runat="server" >
                    <Groups>
                        <dx:NavBarGroup Name="GetNewDebtor" Text="Get New Debtor">
                            <Items>
                                <%-- <dx:NavBarItem Name="30days" Text="30 Days">
                                </dx:NavBarItem>
                                <dx:NavBarItem Name="GetNewDebtor" Text="Get Debtor">
                                </dx:NavBarItem>

                            </Items>
                        </dx:NavBarGroup>
                    </Groups>
                </dx:ASPxNavBar>--%>
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
                <div class="left-side-tables" style="width: 40%; float: left;">
                    <fieldset>
                        <legend>Vintage Report</legend>
                        <table>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="Label1_1" runat="server" Text="Month">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" colspan="2">
                                    <dx:ASPxComboBox ID="cboMonth" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="Label1_0" runat="server" Text="Year">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" colspan="2">
                                    <dx:ASPxComboBox ID="cboYear" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                     <dx:ASPxLabel ID="Label1_2" runat="server" Text="Status">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" colspan="2">
                                    <dx:ASPxComboBox ID="cboStatus" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="Label1_3" runat="server" Text=" FileName">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" >
                                    <dx:ASPxTextBox ID="txtFile" runat="server" Width="170px"></dx:ASPxTextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="Label1_4" runat="server" Text="Score =">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8">
                                    <dx:ASPxTextBox ID="txtScore" runat="server" Width="170px"></dx:ASPxTextBox>
                                </td>
                                <td class="auto-style8">
                                </td>
                            </tr>

                
                             <tr>
                                <td class="auto-style3">
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" >
                                    <dx:ASPxCheckBox ID="chkthick" runat="server"></dx:ASPxCheckBox>
                                     <dx:ASPxLabel ID="chkthickLabel" runat="server" Text="Thick Files only"></dx:ASPxLabel>
                                </td>
                            </tr>
                             <tr>
                                <td class="auto-style3">
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8">
                                    <dx:ASPxCheckBox ID="chkMale" runat="server"></dx:ASPxCheckBox>
                                     <dx:ASPxLabel ID="chkMaleLabel" runat="server" Text="Male Only"></dx:ASPxLabel>
                                </td>
                            </tr>
                             <tr>
                                <td class="auto-style3">                                  
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8">
                                    <dx:ASPxCheckBox ID="chkAllPeriods" runat="server"></dx:ASPxCheckBox>
                                     <dx:ASPxLabel ID="chkAllPeriodsLabel" runat="server" Text="Include All Periods"></dx:ASPxLabel>
                                </td>
                            </tr>
                             <tr>
                                <td class="auto-style3">                                 
                                   
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" >
                                     <dx:ASPxCheckBox ID="chkZeroes" runat="server"></dx:ASPxCheckBox>
                                   <dx:ASPxLabel ID="chkZeroesLabel" runat="server" Text="Zeroes Only">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                             <tr>
                                <td class="auto-style3">
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="auto-style8" colspan="2">
                                     <dx:ASPxButton ID="cmdOK" runat="server" Text="OK">
                                         <ClientSideEvents Click="run" />
                                     </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                   
                 
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
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
        function onlyAlphabets(evt) {
            var charCode;
            if (window.event)
                charCode = window.event.keyCode;  //for IE
            else
                charCode = evt.which;  //for firefox
            if (charCode == 32) //for &lt;space&gt; symbol
                return true;
            if (charCode > 31 && charCode < 65) //for characters before 'A' in ASCII Table
                return false;
            if (charCode > 90 && charCode < 97) //for characters between 'Z' and 'a' in ASCII Table
                return false;
            if (charCode > 122) //for characters beyond 'z' in ASCII Table
                return false;
            return true;
        }
        function isNumberandTextValidTE(evt) {
            debugger;
            if (isNumber(evt) == false) {
                tP_KeyPress(s, e)
            }


        }

    </script>
</asp:Content>

