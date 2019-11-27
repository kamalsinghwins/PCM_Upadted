<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ManageSizeMatrix.aspx.vb" Inherits="pcm.Website.ManageSizeMatrix" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/General/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../js/Positive/HeadOffice/SizeMatrix.js"></script>
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
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="376px"
            HeaderText="Size Matrix Manager">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td>
                                <span class="dxeBase_Office2010Blue">Size Grid</span>
                            </td>
                            <td>
                                <input style="width: 170px;text-transform: uppercase"; id="txtGridNumber" maxlength="4" onkeyup="getCode()"   type="text" />
                                <img id="Search" onclick="searchGrid()" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <span class="dxeBase_Office2010Blue">Description</span>
                            </td>
                            <td>
                                <input  id="txtDescription" style="text-transform: uppercase;width: 170px" type="text" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                         <tr>
                            <td>                          
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>&nbsp;</td>
                            <td></td>
                            <td>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>                          
                               <input style ="width:70px" type="reset" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="clear()"  value="Clear" id="btnClear" />
                                </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>&nbsp;</td>
                            <td></td>
                            <td>
                              <input style ="width:70px" type="button" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="save()"  value="Save" id="btnSave" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br />
        <br />
        <br />
        <div id="frmG_Grid">
        <table id="size1" class="table table-bordered table-hover custom">
            <thead>
                <tr>
                    <th>Size 1
                    </th>
                    <th>Size 2
                    </th>
                    <th>Size 3
                    </th>
                    <th>Size 4
                    </th>
                    <th>Size 5
                    </th>
                    <th>Size 6
                    </th>
                    <th>Size 7
                    </th>
                    <th>Size 8
                    </th>
                    <th>Size 9
                    </th>
                    <th>Size 10
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <input type='text' id="s1"  maxlength="5" class='s1' /></td>
                    <td>
                        <input type='text' id="s2"  maxlength="5" class='s2' /></td>
                    <td>
                        <input type='text' id="s3"  maxlength="5" class='s3' /></td>
                    <td>
                        <input type='text' id="s4"  maxlength="5" class='s4' /></td>
                    <td>
                        <input type='text' id="s5"  maxlength="5" class='s5' /></td>
                    <td>
                        <input type='text' id="s6"  maxlength="5" class='s6' /></td>
                    <td>
                        <input type='text' id="s7" maxlength="5" class='s7' /></td>
                    <td>
                        <input type='text' id="s8"  maxlength="5"  class='s8' /></td>
                    <td>
                        <input type='text' id="s9"  maxlength="5" class='s9' /></td>
                    <td>
                        <input type='text' id="s10"  maxlength="5" class='s10' /></td>
                </tr>
            </tbody>
        </table>
        <table id="size2" class="table table-bordered table-hover custom">
            <thead>
                <tr>
                    <th>Size 11
                    </th>
                    <th>Size 12
                    </th>
                    <th>Size 13
                    </th>
                    <th>Size 14
                    </th>
                    <th>Size 15
                    </th>
                    <th>Size 16
                    </th>
                    <th>Size 17
                    </th>
                    <th>Size 18
                    </th>
                    <th>Size 19
                    </th>
                    <th>Size 20
                    </th>

                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <input type='text' id="s11"   maxlength="5" class='s11' /></td>
                    <td>
                        <input type='text' id="s12"  maxlength="5"  class='s12' /></td>
                    <td>
                        <input type='text' id="s13"  maxlength="5" class='s13' /></td>
                    <td>
                        <input type='text' id="s14"   maxlength="5" class='s14' /></td>
                    <td>
                        <input type='text' id="s15"  maxlength="5"  class='s15' /></td>
                    <td>
                        <input type='text' id="s16"  maxlength="5"  class='s16' /></td>
                    <td>
                        <input type='text' id="s17"  maxlength="5"  class='s17' /></td>
                    <td>
                        <input type='text' id="s18"  maxlength="5"  class='s18' /></td>
                    <td>
                        <input type='text' id="s19"  maxlength="5" class='s19' /></td>
                    <td>
                        <input type='text' id="s20"  maxlength="5"  class='s20' /></td>
                </tr>
            </tbody>
        </table>
        <table id="size3" class="table table-bordered table-hover custom">
            <thead>
                <tr>
                    <th>Size 21
                    </th>
                    <th>Size 22
                    </th>
                    <th>Size 23
                    </th>
                    <th>Size 24
                    </th>
                    <th>Size 25
                    </th>
                    <th>Size 26
                    </th>
                    <th>Size 27
                    </th>
                    <th>Size 28
                    </th>
                    <th>Size 29
                    </th>
                    <th>Size 30
                    </th>

                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <input type='text' id="s21" maxlength="5"  class='s21' /></td>
                    <td>
                        <input type='text' id="s22" maxlength="5"  class='s22' /></td>
                    <td>
                        <input type='text' id="s23" maxlength="5"  class='s23' /></td>
                    <td>
                        <input type='text' id="s24" maxlength="5"  class='s24' /></td>
                    <td>
                        <input type='text' id="s25" maxlength="5"   class='s25' /></td>
                    <td>
                        <input type='text' id="s26" maxlength="5"  class='s26' /></td>
                    <td>
                        <input type='text' id="s27" maxlength="5"   class='s27' /></td>
                    <td>
                        <input type='text' id="s28" maxlength="5"   class='s28' /></td>
                    <td>
                        <input type='text' id="s29" maxlength="5"   class='s29' /></td>
                    <td>
                        <input type='text' id="s30" maxlength="5"   class='s30' /></td>
                </tr>
            </tbody>
        </table>
                   </div>
        <div id="Searchbrit" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Size Grid</h4>
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
                                <input id="txtSearch" style="text-transform: uppercase" onkeyup="search()" type="text" class="form-control" />
                            </div>
                            </div>
                        <br />
                          <div class="row">
                           
                            <div class="col-sm-12">
                                <label></label>
                                <input type="button" onclick="getGrid()" class="btn  btn-primary pull-right " value="Search" id="Grid" />
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
                            <table id="grid" class="table table-bordered table-hover" style="visibility: hidden">
                                <thead>
                                    <tr>
                                        <th>Grid Number
                                        </th>
                                        <th>Description
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
