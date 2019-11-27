<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="EmployeeDetailsReport.aspx.vb" Inherits="pcm.Website.EmployeeDetailsReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../../js/General/application.js"></script>
    <script type="text/javascript" src="../../../js/Collections/contactinvestigation.js"></script>
    <link href="css/new_style.css" rel="stylesheet" />
    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .mb-20 {
            margin-bottom: 20px;
        }

        .ellipsis {
            text-overflow: ellipsis;
            overflow: hidden;
            max-width: 100px !important;
        }

        .mr-20 {
            margin-right: 20px;
        }

        .mt-20 {
            margin-top: 20px;
        }

        .text-transform-capitalize {
            text-transform: capitalize;
        }

        .auto-style2 {
            width: 140px;
        }
    </style>

    <script type="text/javascript">

        function GetSMSErrors(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cmdRun";

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
        OnCallback="ASPxCallback1_Callback">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">

                <div class="mainContainer">
                    <dx:ASPxGridView ID="gvEmployeeDetails" OnDataBinding="gvEmployeeDetails_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
                        EnableTheming="True">

                        <Columns>                          
                            <dx:GridViewDataHyperLinkColumn FieldName="employee_number" VisibleIndex="0">
                                <PropertiesHyperLinkEdit NavigateUrlFormatString="~/Reports/HR/EmployeeDetails.aspx?id={0}" />
                            </dx:GridViewDataHyperLinkColumn>

                            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name"
                                VisibleIndex="1">
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name"
                                VisibleIndex="2">
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Id Number" FieldName="id_number"
                                VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Email Address" FieldName="email_address"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Cellphone" FieldName="cellphone"
                                VisibleIndex="5">
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Last Login" FieldName="last_login"
                                VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowEllipsisInText="true" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                        <SettingsPager PageSize="100">
                        </SettingsPager>

                        <Settings ShowFooter="True" />

                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
                    </dx:ASPxGridViewExporter>
                    <br />
                    <br />
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
