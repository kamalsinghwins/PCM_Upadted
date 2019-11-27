<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="IBT_Out.aspx.vb" Inherits="pcm.Website.IBT_Out" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/General/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../js/Positive/HeadOffice/IBT_Out.js"></script>
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
        <div style="width: 75%; float: left;">
            <table class="dxrpControl_Office2010Blue" cellspacing="0" cellpadding="0" style="width: 100%; border-collapse: collapse; border-collapse: separate;">
                <tbody>
                    <tr>
                        <td class="dxrpHeader_Office2010Blue dx-borderBox"><span class="dxrpHT dx-vam">IBT Out</span></td>
                    </tr>
                    <tr class="dxrpCR">
                        <td class="dxrp dxrpcontent dx-borderBox" style="width: 100%;">
                            <div class="dxrpAW">
                                <div class="dx-borderBox dxrpCW">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <span class="dxeBase_Office2010Blue">Branch Code</span>
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
                                                    <img id="Search" onclick="searchBranch()" class="dxeImage_Office2010Blue" src="../../images/search.png" alt="" />
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
                                                    <span class="dxeBase_Office2010Blue">Reference</span>
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
                                                                                    <input id="txtReference" style="text-transform: uppercase" class="dxeEditArea_Office2010Blue dxeEditAreaSys" type="text" /></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                                <td class="dxeErrorCell_Office2010Blue dxeErrorFrame_Office2010Blue dxeErrorFrameSys dxeErrorCellSys dxeNoBorderLeft" style="vertical-align: middle; display: none;">
                                                                    <table cellspacing="0" cellpadding="0" style="width: 100%; border-collapse: collapse;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="dxEditors_edtError_Office2010Blue" src="/DXR.axd?r=1_112-VghCh" alt=""></td>
                                                                                <td style="width: 100%; white-space: nowrap;">Please Supply An ID Number </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td></td>

                                                <td>
                                                    <div onclick="Reload()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys" style="-webkit-user-select: none;">
                                                        <div class="dxb">
                                                            <div class="dxb-hbc">
                                                                <input onclick="Reload()" class="dxb-hb" value="Reload" type="button" />
                                                            </div>
                                                            <span class="dx-vam">Reload</span>
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
            <table  id="tableItemsGrid" style="margin-top: 20px;width: 100%;" border="1">
                <tbody>
                    <tr>
                        <td>
                            <span class="dxeBase_Office2010Blue">Item Code</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 100px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" style="text-transform: uppercase" name="textBoxItemCode" onkeypress="getItemCodes()" id="textBoxItemCode"  type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Description</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0"  style="width: 150px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" disabled="disabled"  id="textBoxDescription" name="textBoxDescription" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Cost Excl</span>
                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 80px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" disabled="disabled" onkeyup="CalculateTax()" id="txtExcl" type="text" /></td>
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
                                            <asp:DropDownList ID="dropDownTax" runat="server" Enabled="false"></asp:DropDownList>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Cost Incl</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0"  style="width: 100px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" disabled="disabled" name="txtIncl" id="txtIncl" type="text" /></td>
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
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" name="txtQty" id="txtQty" onkeyup="CalculateTaxAmount()" disabled="disabled" onkeypress="AddNewItem()" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td>
                            <span class="dxeBase_Office2010Blue">Total Cost Incl</span>

                            <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0"  style="width: 80px; border-collapse: collapse;">
                                <tbody>
                                    <tr>
                                        <td class="dxic" style="width: 100%;">
                                            <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" disabled="disabled" name="txtTotalIncl" id="txtTotalIncl" type="text" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div onclick="remove()" class="dxbButton_Office2010Blue dxbButtonSys dxbTSys pull-right" style="-webkit-user-select: none;margin-top: 10px;">
                <div class="dxb">
                    <div class="dxb-hbc" onclick="remove()">
                        <input type="button" onclick="remove()" class="btn  btn-primary" value="save" id="cmdRemove" />
                    </div>
                    <span class="dx-vam">Remove</span>
                </div>
            </div>
            <div id="tableFooter">


                <span id="lblCat1" class="labelSpan hidden"></span>
                <span id="lblCat2" class="labelSpan hidden"></span>
                <span id="lblCat3" class="labelSpan hidden"></span>
                <span id="lblSize" class="labelSpan hidden"></span>
                <span id="lblSizeGrid" class="labelSpan hidden"></span>
                <span id="lblMcode" class="labelSpan hidden"></span>
                <span id="lblColour" class="labelSpan hidden"></span>
                <span id="lblColourGrid" class="labelSpan hidden"></span>
                <span id="lblTaxAmount" class="labelSpan hidden"></span>
                <span id="lblOriginalCost" class="labelSpan hidden"></span>
                <span id="lblSupplier" class="labelSpan hidden"></span>
                <span id="lblGeneratedCode" class="labelSpan hidden"></span>
                <span id="lblSellingIncl" class="labelSpan hidden"></span>
                <span id="lblSKUNumber" class="labelSpan hidden"></span>
                <span id="lblSellingTaxGroup" class="labelSpan hidden"></span>
                <span id="lblSupplierCode" class="labelSpan hidden"></span>
                <span id="lblIsServiceItem" class="labelSpan hidden"></span>
                <span id="lblSellEx" class="labelSpan hidden"></span>

            </div>
            <div class="" style="width: 100%; float: left;">

                <div class="" style="width: 70%; float: left;">
                         <div style="margin-top: 32px;">
                       
                         <table >
                        <tbody>
                            <tr>
                                <td>
                        <span class="dxeBase_Office2010Blue" id="lblTotalQuantity">Total Quantity</span>
                                </td>
                                <td></td>
                                <td style="padding-left :20px">
                        <label id="lblQty">0</label>

                                </td>
                            </tr>
                     
                        </tbody>
                    </table>

                    </div>        
                    <div>
                        <span class="dxeBase_Office2010Blue">Box Style</span>
                        <table class="dxeTextBoxSys dxeTextBox_Office2010Blue dxeTextBoxDefaultWidthSys" cellspacing="0" cellpadding="0" style="width: 80px; border-collapse: collapse;">
                            <tbody>
                                <tr>
                                    <td class="dxic" style="width: 100%;">
                                        <input class="dxeEditArea_Office2010Blue dxeEditAreaSys" id="txtBoxStyle"  type="text" /></td>
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
                    <div class="dxb-hbc" onclick="save()">
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
        <div style="width: 25%; float: right; padding: 0 30px">           
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
                            <div class="col-sm-12">
                                <label id="searchType">Search Type</label>
                                <select id="cboSearch" class="form-control">
                                </select>
                            </div>  </div>
                        <br />
                    <div class="row">
                            <div class="col-sm-12">
                                <label id="searchDetails">Search Details</label>
                                <input id="txtSearch" style="text-transform: uppercase"  type="text" class="form-control" />
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
    <div id="frmG_Grid" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Generated Codes</h4>
                </div>
                <div class="modal-body">
                    
                    <div id="generatedCodes" class="mt">
                       
                        <table id="codes" class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>Code
                                    </th>
                                    <th class="hide">Description
                                    </th>
                                    <th>Colour
                                    </th>
                                    <th class="hide">Tax
                                    </th><th>Size
                                    </th>
                                    <th>Qty
                                    </th><th>Total Incl
                                    </th>
                                    <th class="hide">mcode
                                    </th><th  class="hide">cat1
                                    </th>
                                    <th class="hide">cat2
                                    </th><th  class="hide">cat3
                                    </th>
                                    <th  class="hide">Cost Incl
                                    </th>
                                     <th  class="hide">size grid
                                    </th>
                                     <th  class="hide">Cost Excl
                                    </th>
                                     <th  class="hide">colourgrid
                                    </th>
                                     <th  class="hide">colourcode
                                    </th>
                                     <th  class="hide">supplier
                                    </th>
                                     <th  class="hide">suppliercode
                                    </th>
                                     <th  class="hide">sellex
                                    </th>
                                     <th  class="hide">isserviceitem
                                    </th>
                                     <th  class="hide">skunumber
                                    </th>
                                     <th  class="hide">sellingprice
                                    </th>
                                    
                                </tr>
                            </thead>
                            <tbody>

                            </tbody>
                        </table>
                         <p>* Press Tab to move to the next row</p>
            
                    </div>
                </div>
                <div class="modal-footer">
                     <input id="cmdSaveCode" class="btn btn-primary " onclick ="addGeneratedCode()" type="button" value="Save" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        var _dropDownTaxId = '<%=dropDownTax.ClientID.ToString()%>';
        var Current_Company ='<%=Current_Company %>';
        var Branch_Name ='<%=Branch_Name%>';
        var Current_Branch_Code ='<%=Current_Company_Code%>';
        var Branch_Telephone_Number ='<%=Branch_Telephone_Number %>';
        var Branch_Fax_Number = '<%=Branch_Fax_Number%>';
        var Current_User = '<%=Current_User%>';
        var TaxDescript = '<%=TaxDescriptString%>';
        var TaxGroup = '<%=TaxGroupString%>';
        var TaxRate = <%=TaxRateString%>;
        var DefaultPurchaseTaxInt = '<%=DefaultPurchaseTaxInt%>'

        $("#" + _dropDownTaxId).change(function () {
            CalculateTotalAndTax()
        })
        $('.loader-container').hide();

    </script>
</asp:Content>
