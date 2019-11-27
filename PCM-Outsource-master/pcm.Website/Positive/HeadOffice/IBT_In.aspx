<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="IBT_In.aspx.vb" Inherits="pcm.Website.IBT_In" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/General/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../js/Positive/HeadOffice/IBT_In.js"></script>
    <link href="../../css/jquery.dataTables.min.css" rel="stylesheet" />
     <style type="text/css">
        .mt {
            margin-top: 20px;
        }

        .labelSpan {
            border: 1px solid grey;
            margin-top: 10px;
            width: 33.33%;
            float: left;
            min-height: 20px
        }

        .selected {
            background: #ccc;
        }

        .NewItem {
            cursor: pointer;
        }

        #tableItemsGrid tr>td {
            text-align: center;
        }
        #tableItemsGrid tr>td>table {
            margin: 0 auto;
        }
        .ellipsis{
  overflow:hidden;
    white-space:nowrap;
    text-overflow:ellipsis;
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
        <div style="width: 80%; float: left;">
            <table class="dxrpControl_Office2010Blue" cellspacing="0" cellpadding="0" style="width: 100%; border-collapse: collapse; border-collapse: separate;">
                <tbody>
                    <tr>
                        <td class="dxrpHeader_Office2010Blue dx-borderBox"><span class="dxrpHT dx-vam">IBT In</span></td>
                    </tr>
                    <tr class="dxrpCR">
                        <td class="dxrp dxrpcontent dx-borderBox" style="width: 100%;">
                            <div class="dxrpAW">
                                <div class="dx-borderBox dxrpCW">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <span class="dxeBase_Office2010Blue">From Branch</span>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 170px; border-collapse: collapse;">
                                                        <tbody>
                                                            <tr>
                                                                <td class="dxic" style="width: 100%;">
                                                                    <input id="txtAccnum" style="text-transform: uppercase" class="dxeEditArea_Office2010Blue dxeEditAreaSys" onkeypress="setBranch()" type="text" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <img id="Search" onclick="searchBranch()" style="margin-left: 6px;" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td></td>
                                                <td>
                                                    <span class="dxeBase_Office2010Blue">Branch Name</span>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td></td>
                                                <td>
                                                    <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 170px; border-collapse: collapse;">
                                                        <tbody>
                                                            <tr>
                                                                <td class="dxic" style="width: 100%;">
                                                                    <input id="txtCompanyName" style="text-transform: uppercase" class="dxeEditArea_Office2010Blue dxeEditAreaSys" name="ctl00$ASPxSplitter1$MainHolder$ASPxCallbackPanel1$ASPxRoundPanel1$txtCompanyName" onfocus="ASPx.EGotFocus('ASPxSplitter1_MainHolder_ASPxCallbackPanel1_ASPxRoundPanel1_txtCompanyName')" onblur="ASPx.ELostFocus('ASPxSplitter1_MainHolder_ASPxCallbackPanel1_ASPxRoundPanel1_txtCompanyName')" type="text" maxlength="80" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="dxeBase_Office2010Blue">IBT Out No</span>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <table class="dxeValidDynEditorTable dxeRoot_Office2010Blue" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 170px; border-collapse: collapse;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="dxic" style="width: 100%;">
                                                                                    <input id="txtIBTOutNumber" style="text-transform: uppercase" class="dxeEditArea_Office2010Blue dxeEditAreaSys" type="text" onkeypress="txtIbtoutKeypress()" /></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                                <td class="dxeErrorCell_Office2010Blue dxeErrorFrame_Office2010Blue dxeErrorFrameSys dxeErrorCellSys dxeNoBorderLeft" style="vertical-align: middle; display: none;">
                                                                    
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td></td>

                                                <td>
                                                    <div style="margin-left: 6px;" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" onclick="fetchData()" style="-webkit-user-select: none;">
                                                        <div class="dxb">
                                                            <div class="dxb-hbc" onclick="fetchData()">
                                                                <input onclick="fetchData()" class="dxb-hb" value="Fetch" type="button" />
                                                            </div>
                                                            <span class="dx-vam">Fetch</span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>

                                        </tbody>
                                    </table>
                                    <div class="dxpcLoadingDiv_Office2010Blue dxlpLoadingDiv_Office2010Blue dx-ft" style="left: 0px; top: 0px; z-index: 29999; display: none; position: absolute;">
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div>
            <table  id="tableItemsGrid" class="table table-bordered" style="margin-top: 20px;width: 100%;" border="1">
                <tbody>
                    <tr>
                        <td>
                            <span class="dxeBase_Office2010Blue">Item Code</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys " cellspacing="0" cellpadding="0" style="width: 100px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled" style="text-transform: uppercase" name="textBoxItemCode" id="textBoxItemCode"  type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Description</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys " cellspacing="0" cellpadding="0"  style="width: 150px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled"  id="textBoxDescription" name="textBoxDescription" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Sell Excl</span>
                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys " cellspacing="0" cellpadding="0" style="width: 80px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled" onkeyup="CalculateTax()" id="txtSellExcl" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Tax Group</span>
                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 100px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled"  id="txtTaxGroup" type="text" />

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Sell Incl</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0"  style="width: 100px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled" name="txtSellIncl" id="txtSellIncl" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Qty</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 50px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" name="txtQty" id="txtQty"  onkeypress="txtQtyKeyPress()" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Total Sell Incl</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0"  style="width: 80px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys text-truncate" disabled="disabled" name="txtTotalIncl" id="txtTotalSellIncl" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>

                </div>
            <div onclick="updateQty()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-left" style="-webkit-user-select: none;margin-top: 10px;">
                <div class="dxb">
                    <div class="dxb-hbc" onclick="updateQty()">
                        <input type="button" onclick="updateQty()" class="btn  btn-primary" value="save" id="cmdUpdateQty" />
                    </div>
                    <span class="dx-vam">Update Qty</span>
                </div>
            </div>
            <div id="tableFooter">

                 <span id="lblMcode" class="labelSpan hidden"></span>
                <span id="lblCat1" class="labelSpan hidden"></span>
                <span id="lblCat2" class="labelSpan hidden"></span>
                <span id="lblCat3" class="labelSpan hidden"></span>
                <span id="lblSize" class="labelSpan hidden"></span>
                <span id="lblSizeGrid" class="labelSpan hidden"></span>
                <span id="lblColour" class="labelSpan hidden"></span>
                <span id="lblColourGrid" class="labelSpan hidden"></span>
                <span id="lblTaxAmount" class="labelSpan hidden"></span>
                <span id="lblOriginalCost" class="labelSpan hidden"></span>
                

            </div>
            <div class="" style="width: 100%; float: left;">
                <div>
                <div class="" style="width: 70%; float: left;">
                    <div style="margin-top: 32px;">
                       
                         <table >
                        <tbody>
                            <tr>
                                <td>
                                    <span class="dxeBase_Office2010Blue">Total Received Quantity</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                                   <label id="lblQty">0</label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="dxeBase_Office2010Blue">Total Sent Quantity</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                        <label id="lblTotalSentQTY">0</label>

                                </td>
                            </tr>
                       
                        </tbody>
                    </table>

                    </div>              
                </div>
                <div class="" style="width: 30%; float: left; margin-top: 30px;">
                    <table class="pull-right">
                        <tbody>
                            <tr>
                                <td>
                                    <span class="dxeBase_Office2010Blue">Total Exclusive</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                                    <label id="lblTotalExcl">0</label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="dxeBase_Office2010Blue">Total Tax</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                                    <label id="lblTotalTax">0</label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="dxeBase_Office2010Blue">Total Inclusive</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                                    <label id="lblTotalIncl">0</label>

                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
                      <div onclick="clearForm()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" style="-webkit-user-select: none;margin-top :10px"">
                <div class="dxb">
                    <div class="dxb-hbc" onclick="clearForm()">
                        <input type="button" onclick="clearForm()" class="btn  btn-primary" value="save" id="cmdClear" />
                    </div>
                    <span class="dx-vam">Clear</span>
                </div>
            </div>
                <div   onclick="save()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-right" style="-webkit-user-select: none;margin-top :10px">
                <div class="dxb">
                    <div class="dxb-hbc" onclick="save()">
                        <input type="button" onclick="save()" class="btn  btn-primary" value="save" id="cmdSave" />
                    </div>
                    <span class="dx-vam">Save</span>
                </div>
            </div>
            </div>
                </div>
        
        </div>
        <div style="width: 20%; float: right; padding: 0 30px">

            <div>
                <span class="dxeBase_Office2010Blue"></span>
                <span class="dxeBase_Office2010Blue hidden">0. Itemcode<br/>
                    1. Description<br/>
                    2. SellEx<br/>
                    3. Tax<br/>
                    4. SellIncl<br/>
                    5. Qty<br/>
                    6. SellCostIncl<br/>
                    7. Mcode<br/>
                    8. Cat1<br/>
                    9. Cat2<br/>
                    10. Cat3<br/>
                    11. Size<br/>
                    12. SizeGrid<br/>
                    13. Colour<br/>
                    14. ColourGrid<br/>
                    15. Tax Amount<br/>
                    16. Original Cost<br/>
                    17. Supplier
                    <br/>
                    18. Supplier Code<br/>
                    19. IsServiceItem<br/>
                    20. CostExcl<br/></span>
            </div>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblAdd1"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblAdd2"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblAdd3"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblAdd4"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblAdd5"></span>
            <br />
            <br />

            <span class="dxeBase_Office2010Blue hidden" id="lblTelephone"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblFax"></span>
            <br />
            <span class="dxeBase_Office2010Blue hidden" id="lblTaxNumber"></span>
            <br />
        </div>
    </div>
       

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
                        <div class="col-sm-3">
                            <label id="searchType">Search Type</label>

                            <select id="cboSearch" class="form-control">
                            </select>
                        </div>
                        <div class="col-sm-3">
                            <label id="searchDetails">Search Details</label>
                            <input id="txtSearch" style="text-transform: uppercase"  type="text" class="form-control" />

                        </div>
                        <div class="col-sm-3">
                          <%--  <label id="chkMasterCode" style="margin-top: 20px;">
                                Check Master
                            <input id="chkMaster" type="checkbox" /></label>--%>

                        </div>
                        <div class="col-sm-3">

                            <label></label>
                            <input type="button" onclick="getSearch()" class="btn  btn-primary " value="Search" id="cmdSearch" />

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
                                    <th>Branch Code
                                    </th>
                                    <th>Branch Name
                                    </th>
                                </tr>
                            </thead>
                            <tbody class="Pointer"></tbody>
                        </table>
                        <table id="dataTableItems" class="table table-bordered table-hover" style="visibility: hidden">
                            <thead>
                                <tr>
                                    <th>Item Code
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

     <script type="text/javascript">
       
        $('.loader-container').hide();
        var Current_Company ='<%=Current_Company %>';
        var Branch_Name ='<%=Branch_Name%>';
        var Branch_Telephone_Number ='<%=Branch_Telephone_Number %>';
        var Branch_Fax_Number = '<%=Branch_Fax_Number%>';
        var Current_User = '<%=Current_User%>';
       var Current_Branch_Code ='<%=Current_Company_Code%>';

    </script>
</asp:Content>
