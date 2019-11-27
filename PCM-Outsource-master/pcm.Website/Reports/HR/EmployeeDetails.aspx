<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="EmployeeDetails.aspx.vb" Inherits="pcm.Website.EmployeeDetails" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
     function onEnd(s, e) {
            lp.Hide();
        }

        //function OnIndexChange(s, e) {
        //    debugger;
        //    var employeeName = s.GetValue();
        //    if (!employeeName || employeeName == "")
        //        return;

        //    var arr = employeeName.split('(');

        //    var unfilteredName = arr[arr.length - 1];

        //    var employee_number = unfilteredName.substr(0, unfilteredName.length - 1);

        //    if (!employee_number || employee_number == "")
        //        return;

        //    window.location.href = '/Reports/HR/EmployeeDetails.aspx?id=' + employee_number;

        //}
        function OnIndexChange(s, e) {
            debugger;
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cboClockNumbers";

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
        SettingsLoadingPanel-Enabled="False" Height="354px">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                      <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%"
                        HeaderText="Employee Details" >
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                     <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number:" Width="120px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeNumber" runat="server">
                                        </dx:ASPxLabel>
                                        </td>
                                        <td style="width:100px">&nbsp;</td>
                                         
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeIDNumber" runat="server" Text="ID Number:" Width="90px">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeIDNumber" runat="server" >
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeFN" runat="server" Text="First Name:">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeFN" runat="server" >
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="width:100px">&nbsp;</td>
                                       
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeLN" runat="server" Text="Last Name:">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeLN" runat="server" >
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeEmail" runat="server" Text="Email:">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeEmail" runat="server">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="width:100px">&nbsp;</td>
                                        
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeCell" runat="server" Text="CellPhone: ">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeCell" runat="server">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeBankAccount" runat="server" Text="Bank Account:">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeBankAccount" runat="server">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td style="width:100px">&nbsp;</td>
                                      
                                        <td>
                                            <dx:ASPxLabel ID="lblEmployeeBranchCode" runat="server" Text="Branch Code: ">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="EmployeeBranchCode" runat="server">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Employee:">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <%--<dx:ASPxComboBox ID="cboEmployees" runat="server" ClientInstanceName="cboEmployees" TextFormatString="{0}" 
                                            CssClass="UpperCase" ValueType="System.String" Width="200px" DropDownStyle="DropDown" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith">
                                                <ClientSideEvents SelectedIndexChanged="OnIndexChange" />
                                            </dx:ASPxComboBox>--%>
                                             <dx:ASPxComboBox ID="cboEmployees" runat="server" EnableCallbackMode="true" CallbackPageSize="10"
                                            ValueType="System.String" ValueField="employee_number" ClientInstanceName="cboEmployees"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                                            Width="287px" DropDownStyle="DropDown" CssClass="UpperCase">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="employee_number" />
                                                <dx:ListBoxColumn FieldName="first_name" />
                                                <dx:ListBoxColumn FieldName="last_name" />
                                            </Columns>
                                            <ClientSideEvents SelectedIndexChanged="OnIndexChange" />
                                           
                                        </dx:ASPxComboBox>
                                        </td>
                                        <td style="width:100px">&nbsp;</td>
                                      
                                        <td>
                                            
                                        </td>
                                        <td>
                                           
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <br />
                         <dx:ASPxGridView ID="gvReview" runat="server" AutoGenerateColumns="False" Width="100%" 
                        EnableTheming="True">

                        <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>

                        <Columns>
                             <dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="0">                        
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Employee Number" CellStyle-CssClass="ellipsis" FieldName="employee_number" VisibleIndex="1">
                                <CellStyle CssClass="ellipsis"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_name" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Review" FieldName="type_of_comment" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Rating" FieldName="rating" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Warning" FieldName="type_of_warning" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Comment" CellStyle-CssClass="ellipsis" FieldName="comment" VisibleIndex="6">
                                <CellStyle CssClass="ellipsis"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Date" FieldName="time_stamp" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Warning Expiry" FieldName="warning_expiry_date" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <SettingsAdaptivity>
                            <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity>

                        <SettingsPager PageSize="10">
                        </SettingsPager>
                        <Settings ShowFooter="True"  ShowFilterRow="true" ShowGroupPanel="true" />
                    </dx:ASPxGridView>
                    <br />
                    <br />
                    <dx:ASPxGridView ID="gvMaster" runat="server"  AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding" Width="100%"
                        EnableTheming="True" >

<EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>

                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Date / Time Completed" FieldName="date_time_completed" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Survey Name" CellStyle-CssClass="ellipsis" FieldName="survey_name" VisibleIndex="1">
<CellStyle CssClass="ellipsis"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Survey" FieldName="type_of_survey" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="ID Number" FieldName="id_number" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Contact Number" FieldName="contact_number" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Questions In Survey" FieldName="totalquestions" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Questions Answered" FieldName="questionsanswered" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Correctly Answered" FieldName="correctanswers" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                        </Columns>

<SettingsAdaptivity>
<AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
</SettingsAdaptivity>

                        <Templates>
                            <DetailRow>
                                <dx:ASPxGridView ID="gvDetail" runat="server"
                                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="question_id" CellStyle-CssClass="ellipsis" Caption="Id" VisibleIndex="0" />
                                        <dx:GridViewDataColumn FieldName="question_text" CellStyle-CssClass="ellipsis" Caption="Question" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="option_text" CellStyle-CssClass="ellipsis" Caption="Option Selected" VisibleIndex="2" />
                                        <dx:GridViewDataColumn FieldName="is_correct" CellStyle-CssClass="text-transform-capitalize" VisibleIndex="3" Caption="Correct" />
                                    </Columns>
                                    <Settings ShowFooter="True" />
                                    <Settings ShowGroupPanel="True" />
                                    <SettingsPager PageSize="20">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>

                        <SettingsPager PageSize="10">
                        </SettingsPager>
                        

                        <Settings ShowFooter="True"  ShowFilterRow="true" ShowGroupPanel="true" HorizontalScrollBarMode="auto"/>

                        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
                    </dx:ASPxGridView>

                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
