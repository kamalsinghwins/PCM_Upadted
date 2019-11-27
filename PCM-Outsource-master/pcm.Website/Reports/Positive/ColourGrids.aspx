<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ColourGrids.aspx.vb" Inherits="pcm.Website.ColourGrids" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/General/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../js/Positive/HeadOffice/ColourGrid.js"></script>
    <link href="../../css/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
   
    <style type="text/css">
        .list {
            padding: 8px 15px;
            border-bottom: 1px solid #ccc;
           list-style-type : none;
        }
        .ul {
          padding: 0;
          height: 300px;
          width: 100%; 
          display: inline-block; 
          background: #fff; 
          border: 1px solid #8ba0bc; 
          overflow: auto
        }
        #branchlist > li:nth-child(odd) {
    background: #f8f8f8;
}
          #stockcodelist > li:nth-child(odd) {
    background: #f8f8f8;
}
        .selected {
    background: #dfeaf8 !important;
}
    </style>
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
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
            HeaderText="Colour Grids">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtFromDate" ClientInstanceName="txtFromDate" runat="server" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtToDate" ClientInstanceName="txtToDate" runat="server" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                </dx:ASPxDateEdit>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="lblType" runat="server" Text="Type" Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <select id="type" style="width: 250px" onchange="showHide()">
                                    <option value="Full">Full</option>
                                    <option value="Range">Range</option>
                                    <option value="Partial">Partial</option>
                                </select>
                            </td>
                            <td>&nbsp;</td>
                            <td></td>
                            <td></td>
                            <td align="right"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>&nbsp;</td>
                            <td colspan="2"></td>
                            <td align="right"></td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td colspan="2"></td>
                            <td colspan="2" align="right"></td>
                        </tr>
                    </table>
                    <fieldset id="Range">
                        <legend>Range Options</legend>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxLabel ID="lblFromStockCode" runat="server" Text="From StockCode:" Width="120px">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 220px; border-collapse: collapse;">
                                        <tbody>
                                            <tr>
                                                <td class="dxic" style="width: 100%;">
                                                    <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" name="txtFromStockCode" id="txtFromStockCode" type="text" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td>
                                    <img id="SearchFrom" onclick="searchFromStockcode()" style="margin-left: 6px;" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                                </td>
                                <td>
                                    <dx:ASPxLabel ID="lblToStockCode" runat="server" Text="To StockCode:" Width="90px">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 220px; border-collapse: collapse;">
                                        <tbody>
                                            <tr>
                                                <td class="dxic" style="width: 100%;">
                                                    <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" name="txtToStockCode" id="txtToStockCode" type="text" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td>
                                    <img id="Search" onclick="searchToStockcode()" style="margin-left: 6px;" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td colspan="2"></td>
                                <td align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td colspan="2"></td>
                                <td colspan="2" align="right"></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="PartialOption">
                        <legend>Partial Options</legend>
                        <div onclick="searchStockcode()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-right" style="-webkit-user-select: none; margin-top: 10px">
                            <div class="dxb">
                                <div class="dxb-hbc" onclick="save()">
                                    <input type="button" onclick="searchStockcode()" class="btn  btn-primary" value="Add" id="cmdAdd" />
                                </div>
                                <span class="dx-vam">Add</span>
                            </div>
                        </div>
                        <div onclick="remove()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-right" style="-webkit-user-select: none; margin-top: 10px">
                            <div class="dxb">
                                <div class="dxb-hbc" onclick="remove()">
                                    <input type="button" onclick="remove()" class="btn  btn-primary" value="Remove" id="cmdRemove" />
                                </div>
                                <span class="dx-vam">Remove</span>
                            </div>
                        </div>
                        <br />
                        <ul id="stockcodelist"class="ul">
                        </ul>
                    </fieldset>
                    <br />
                   
                    <input type="checkbox" id="chkAll" name="chkAll" value="All Brances" onchange="selectBranch()" /><span>All Branches</span>
                    <dx:ASPxCheckBox ID="chkMaster" Visible="False" Text="Master code only" runat="server" CheckState="Unchecked">
                    </dx:ASPxCheckBox>
                    <ul id="branchlist" class="ul">
                    </ul>
                    <br />
                    <div style="float: right">
                        <div onclick="run()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-right" style="-webkit-user-select: none; margin-top: 10px">
                            <div class="dxb">
                                <div class="dxb-hbc" onclick="run()">
                                    <input type="button" onclick="run()" class="btn  btn-primary" value="Run" id="cmdRun" />
                                </div>
                                <span class="dx-vam">Run</span>
                            </div>
                        </div>
                    </div>
                    <br />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br />
        <br />
        <div id="Searchbrit" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">StockCode</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-12">
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                <label id="searchDetails">StockCode Search </label>
                                <input id="txtSearch" style="text-transform: uppercase" onkeyup="search()" type="text" class="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">

                            <div class="col-sm-12">
                                <label></label>
                                <input type="button" onclick="searchStockCode()" class="btn  btn-primary pull-right " value="Search" id="Grid" />
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
                    </div>
                    <table id="branch" class="table table-bordered table-hover" style="visibility: hidden">
                        <thead>
                            <tr>
                                <th>Master Code
                                </th>
                                <th>Description
                                </th>
                            </tr>
                        </thead>
                        <tbody class="Pointer"></tbody>
                    </table>
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
        var Username ='<%=Username %>';
        var Email ='<%=Email%>';
        var IsAdmin ='<%=IsAdmin%>';
        var IPAddress ='<%=IPAddress %>';
        $('.loader-container').hide();

    </script>
</asp:Content>
