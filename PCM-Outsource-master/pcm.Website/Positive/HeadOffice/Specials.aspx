<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Specials.aspx.vb" Inherits="pcm.Website.Specials" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/General/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../js/Positive/HeadOffice/Specials.js"></script>
    <link href="../../css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../../css/custom.css" rel="stylesheet" />
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
   
                <div class="mainContainer">
                     <div class="loader-container">
            <div class="loader"></div>
        </div>
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="300px"
                        HeaderText="Specials">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <td>
                                         
                                             <span class="dxeBase_Office2010Blue">Special Name</span>
                                        </td>
                                        <td >                                           
                                           <input style="width: 205px;text-transform: uppercase"; onkeyup="searchSpecial()" id="txtSpecialName"    type="text" />                                          
                                           <img id="SpecialPopup" onclick="specialPopup()" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>                                      
                                      <span class="dxeBase_Office2010Blue">Price</span>
                                        </td>
                                        <td>                                        
                                       <input style="width: 230px;text-transform: uppercase";  id="txtPrice" onkeydown="NumbersOnly()" type="text" />                                          
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblStartDate" runat="server" Width="120px" Text="StartDate">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtStartDate" ClientInstanceName="txtStartDate" runat="server" Font-Bold="True" Width="230px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblEndDate" runat="server" Width="120px" Text="EndDate">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="txtEndDate" ClientInstanceName="txtEndDate" runat="server" Font-Bold="True" Width="230px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                            <span class="dxeBase_Office2010Blue">Code</span>

                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                         
                                              <input style="width: 200px;text-transform: uppercase"; onkeyup="movenext()" id="txtCode"    type="text" />                                          
                                           <img id="CodePopup" onclick="codePopup()" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>                                       
                                            <span class="dxeBase_Office2010Blue">Quantity</span>
                                        </td>
                                        <td style="display: -webkit-inline-box;">
                                            
                                              <input style="width: 230px;text-transform: uppercase"; onkeydown="digits()" onkeyup="searchStockcode()" id="txtQuantity"    type="text" />                                          
                                        </td>
                                        <td>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                      <input type="checkbox" id="chkActive"  name="Active" value="Active"/>Active
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>
                                           
                                        </td>
                                        <td>
 <div>
                                              <input  style ="width:70px;float:right" type="button" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="addCode()"  value="Add" id="btnwSave" />
                                            </div>
                                        </td>

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
                                    <input style ="width:70px" type="button" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="clearForm()"  value="Clear" id="btnClear" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>

                                            <div style="float: right">
                                         <input style ="width:70px" type="button" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="save()"  value="Save" id="btnSave" />

                                            </div>
                                        </td>

                                    </tr>
                                </table>

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <div style="margin-bottom: 10px;">
                    <input style ="width:70px;float:right" type="button" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="clearList()"  value="Clear List" id="btnClearList" />
                  </div>
                        <br />
                     <table id="size1" class="table table-bordered table-hover custom">
            <thead>
                <tr>
                    <th>Stockcode
                    </th>
                    <th>Description
                    </th>
                    <th>Quantity
                    </th>
                  
                </tr>
            </thead>
            <tbody>
               
            </tbody>
        </table>
                    <br />

              <div id="Searchbrit" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Search Branch</h4>
                </div>
                <div class="modal-body">
                     <div class="row">
                            <div class="col-sm-12">
                                <label id="searchType">Search Type</label>

                                <select id="cboSearch" class="form-control">
                                </select>
                            </div>  </div>
                        <br />
                         <div class="row">
                            <div class="col-sm-12">
                                <label id="searchDetails">Search Details</label>
                                <input id="txtSearch" style="text-transform: uppercase" onkeypress="doSearch()"  type="text" class="form-control" />
                            </div>
                            </div>
                      <div class="row">
                            <div class="col-sm-3">
                                 <input type="checkbox" id="chkMaster"  name="Master" value="Master"/>
                                <label id="Master" for="chkMaster">Master</label>
                            </div>
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-3">
                          
                            </div>
                        </div>
                        <br />
                          <div class="row">
                           
                            <div class="col-sm-12">
                                <label></label>
                                <input type="button" onclick="getSearch()" class="btn  btn-primary pull-right " value="Search" id="Grid" />
                            </div>
                        </div>

                     
                        <div class="row">
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-3">
                            </div>
                            <div class="col-sm-3">
                            </div>
                        </div>
                        <div></div>
                    <div id="searchData" class="mt">
                        <table id="branch" class="table table-bordered table-hover" style="visibility: hidden">
                            <thead>
                                <tr>
                                    <th>Name
                                    </th>
                                    <th>Start Date
                                    </th>
                                     <th>End Date
                                    </th>
                                </tr>
                            </thead>
                            <tbody class="Pointer"></tbody>
                        </table>
                        <table id="dataTableItems" class="table table-bordered table-hover" style="visibility: hidden">
                            <thead>
                                <tr>
                                    <th>Master Code
                                    </th>
                                    <th>Description
                                    </th>
                                    <th>SKU
                                    </th>
                                </tr>
                            </thead>
                             <tbody class="Pointer"></tbody>
                        </table>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
        <div id="messagePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body" style="font-size: 20px;">
                    <div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    </div>
     <script type="text/javascript">        
         $('.loader-container').hide();
    </script>
          
  
</asp:Content>
