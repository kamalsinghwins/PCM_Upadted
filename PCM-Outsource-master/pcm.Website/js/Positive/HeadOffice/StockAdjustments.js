var _searchType;
var CodeColourMatrixSTR;
var CodeSizeMatrixSTR;
var CodeCostAverageExclusiveDBL
var CodeEstimatedCostExclusiveDBL


function clearForm() {
    $('#txtBranchCode').val("")
    $('#txtCompanyName').val("")
    $('#txtReference').val("")
    $('#txtItemCode').val("")
    $('#txtDescription').val("")
    $('#txtExcl').val("")
    $('#txtIncl').val("")
    $('#txtQty').val("")
    $('#txtTotalIncl').val("")
    $('#lblTotalExcl').text("0.00")
    $('#lblTotalTax').text("0.00")
    $('#lblTotalIncl').text("0.00")
    $('#lblQty').text("0.00")
    $('#lblCurrentQTY').text("0.00")
    $('#lblMcode').text("")
    $('#lblCat1').text("")
    $('#lblCat2').text("")
    $('#lblCat3').text("")
    $('#lblSize').text("")
    $('#lblSizeGrid').text("")
    $('#lblColour').text("")
    $('#lblColourGrid').text("")
    $('#lblTaxAmount').text("")
    $('#lblOriginalCost').text("")
    $('#lblSupplier').text("")
    $('#lblGeneratedCode').text("")
    $('#lblSellingTaxGroup').text("")
    $('#lblSupplierCode').text("")
    $('#lblIsServiceItem').text("")
    $('#lblSellEx').text("")
    $('#txtBranchCode').focus()
    $("#tableItemsGrid tr.NewItem").remove()
    $('#txtBranchCode').removeAttr('disabled')
    $('#txtCompanyName').removeAttr('disabled')
    $('#txtReference').removeAttr('disabled')
    $('#' + _dropDownTaxId).val("0")

}
function clearItemCode() {
    $('#txtItemCode').val("")
    $('#txtDescription').val("")
    $('#txtExcl').val("")
    $('#txtIncl').val("")
    $('#txtQty').val("")
    $('#txtTotalIncl').val("")
    $('#lblMcode').text("")
    $('#lblCat1').text("")
    $('#lblCat2').text("")
    $('#lblCat3').text("")
    $('#lblSize').text("")
    $('#lblSizeGrid').text("")
    $('#lblColour').text("")
    $('#lblColourGrid').text("")
    $('#lblTaxAmount').text("")
    $('#lblOriginalCost').text("")
    $('#lblSupplier').text("")
    $('#lblGeneratedCode').text("")
    $('#lblSellingTaxGroup').text("")
    $('#lblSupplierCode').text("")
    $('#lblIsServiceItem').text("")
    $('#lblSellEx').text("")
    $('#' + _dropDownTaxId).val("0")

}
function searchBranch() {
    _searchType = 'B';
    $('#txtSearch').val("");
    $('#txtSearch').focus();
    $('#cboSearch').html('');
    $('#cboSearch').append('<option value="Branch Code">BranchCode</option>',
        '<option value="Branch Name">BranchName</option>')
    $("#dataTableItems tbody tr").remove();
    $("#dataTableItems_wrapper").hide();
    $("#chkMaster").hide();
    $("#Master").hide();
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Branch');
    mymodal.modal('show');
    $('#gridItemCode').hide();
    $('#gridItemCode_wrapper').hide();
}
function searchItems() {
    _searchType = 'I';

    $('#gridBranch').hide();
    $('#gridBranch_wrapper').hide();
    $('#txtSearch').val("");
    $('#gridBranch').hide();
    $('#txtSearch').focus();
    $('#cboSearch').html('');
    $('#cboSearch').append(
        '<option value = "Generated Code" >Generated Code</option > '
    )

    $('#branch tbody tr').remove()
    $('#branch_wrapper').hide()
    $("#chkMaster").css('visibility', 'visible');
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();
}
function getBranchGrid() {
    $('.loader-container').show();
    var SearchType = $('#cboSearch').val();
    var SearchDetail = $('#txtSearch').val();
    var formData = {
        SearchType: SearchType,
        SearchDetail: SearchDetail,
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=GetBranch";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];

        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData = respone.searchBranchList;
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');

        }
        $('#gridBranch').show();
        $('#gridBranch_wrapper').show();

        $("#gridBranch").css('visibility', 'visible');
        t = $("#gridBranch").DataTable({
            paging: true,
            data: rowData,
            columns: [
                { 'data': 'branch_code' },
                { 'data': 'branch_name' },
            ],
            destroy: true

        });
    }, 'json');


}
function search() {
    if (_searchType == 'B') {
        getBranchGrid()
    }
    else {
        getItemCodeGrid()
    }
}
function getItemCodeGrid() {
    $('.loader-container').show();
    var SearchType = $('#cboSearch').val();
    var SearchDetail = $('#txtSearch').val();

    var formData = {
        SearchType: "Generated Code",
        SearchText: SearchDetail,
    };
    _searchType == 'I'
    $('#gridBranch').hide();
    $('#gridBranch_wrapper').hide();
    $('#cboSearch').html('');
    $('#cboSearch').append(
        '<option value = "Generated Code" >Generated Code</option > ',
    )
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();


    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=GetItems";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];
        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData = respone.dt

        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');

        }
        $('#gridItemCode').show();
        $('#gridItemCode_wrapper').show();

        $("#gridItemCode").css('visibility', 'visible');
        t = $("#gridItemCode").DataTable({
            paging: true,
            data: rowData,
            columns: [
                { 'data': 'code' },
                { 'data': 'description' },
                { 'data': 'sku_number' },
            ],
            destroy: true

        });
    }, 'json');
}
$(document).on('click', '#gridBranch tbody tr', function () {
    var data = t.row(this).data();
    $('#txtBranchCode').val(data.branch_code)
    $('#txtCompanyName').val(data.branch_name)
    getBranchDetails(data.branch_code);
    $('#Searchbrit').modal('hide');

});
function getBranchDetails(branchCode) {
    $('.loader-container').show();
    var formData = {
        BranchCode: $('#txtBranchCode').val()
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=GetBranchDetails";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showBranchDetails, 'json');

}
function showBranchDetails(branchDetailResponse) {
    if (branchDetailResponse.Success == true) {

        if (branchDetailResponse.isBlocked == false) {
            $('#txtCompanyName').val(branchDetailResponse.BranchName)
            $("#lblAdd1").text(branchDetailResponse.AddressLine1);
            $("#lblAdd2").text(branchDetailResponse.AddressLine2);
            $("#lblAdd3").text(branchDetailResponse.AddressLine3);
            $("#lblAdd4").text(branchDetailResponse.AddressLine4);
            $("#lblAdd5").text(branchDetailResponse.AddressLine5);
            $("#lblTelephone").text(branchDetailResponse.TelephoneNumber);
            $("#lblFax").text(branchDetailResponse.FaxNumber);
            $("#lblTaxNumber").text(branchDetailResponse.TaxNumber);
            $('#txtBranchCode').attr("disabled", "disabled")
            $('#txtCompanyName').attr("disabled", "disabled");
            $("#txtReference").focus();
        }
        else {
            clearForm();
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text("This Account is blocked. You cannot use it.");
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }
    }
    else {
        clearForm();
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(branchDetailResponse.Message);
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');

    }
    $('.loader-container').hide();

}
function getItemCodes() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {

        if ($('#txtBranchCode').is(':enabled')) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please select a Branch before inputting stock');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false;

        }

        if ($('#txtItemCode').val() != "") {
            var code = $('#txtItemCode').val()
            getCodeDetails(code)
        }
        else {
            searchItems();
        }
    }
}
function setBranch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtBranchCode').val() == "") {
            searchBranch()
        }

        else {
            var branchcode;
            branchcode = $('#txtBranchCode').val()
            getBranchDetails(branchcode)
            $("#dataTableItems tbody tr").remove();
            $("#dataTableItems_wrapper").hide();
            $("#dataTableItems").css('visibility', 'hidden');

        }

    }
}
$(document).on('click', '#gridItemCode tbody tr', function () {
    var data = t.row(this).data();
    $('#txtItemCode').val(data.code)
    //$('#textBoxDescription').val(data.description)
    getCodeDetails(data.code);
    $('#Searchbrit').modal('hide');
    $("#textBoxItemCode").focus();
});
function getCodeDetails(itemCode) {
    var formData = {
        itemCode: itemCode,
        BranchCode: $('#txtBranchCode').val()
    };
    $('.loader-container').show();
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=GetItemCodeDetails";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showItemCodeDetails, 'json');

}
function showItemCodeDetails(generatedCodeInfoResponse) {
    if (generatedCodeInfoResponse.GetGeneratedCodeInfo == true) {
        debugger;
        if (generatedCodeInfoResponse.CodeIsBlockedBOOL == true) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('This Item has been blocked.');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            $('.loader-container').hide();
            clearLineItem()
            return false;

        }
        if (generatedCodeInfoResponse.CodeIsServiceItemBOOL == true) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('This is Item is a service Item. You cannot purchase a service item.');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            $('.loader-container').hide();
            clearLineItem()
            return false;
        }

        // Set Global Variables
        CodeColourMatrixSTR = generatedCodeInfoResponse.CodeColourMatrixSTR
        CodeSizeMatrixSTR = generatedCodeInfoResponse.CodeSizeMatrixSTR
        CodeSalesTaxGroupSTR = generatedCodeInfoResponse.CodeSalesTaxGroupSTR
        CodeCostAverageExclusiveDBL = generatedCodeInfoResponse.CodeCostAverageExclusiveDBL
        CodeEstimatedCostExclusiveDBL = generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL
        if (CodeCostAverageExclusiveDBL == "0") {
            $("#txtExcl").val(CodeEstimatedCostExclusiveDBL)
        }
        else {
            $("#txtExcl").val(CodeCostAverageExclusiveDBL)

        }

        $("#txtDescription").val(generatedCodeInfoResponse.CodeDescriptionSTR);
        $("#lblGeneratedCode").text(generatedCodeInfoResponse.CodeGeneratedCodeSTR);
        $("#lblMcode").text(generatedCodeInfoResponse.CodeMasterCodeSTR);
        $("#lblCat1").text(generatedCodeInfoResponse.CodeCategory1STR);
        $("#lblCat2").text(generatedCodeInfoResponse.CodeCategory2STR);
        $("#lblCat3").text(generatedCodeInfoResponse.CodeCategory3STR);
        $("#lblSize").text(generatedCodeInfoResponse.CodeItemSizeSTR);
        $("#lblSizeGrid").text(generatedCodeInfoResponse.CodeSizeMatrixSTR);
        $("#lblColour").text(generatedCodeInfoResponse.CodeItemColourSTR);
        $("#lblColourGrid").text(generatedCodeInfoResponse.CodeColourMatrixSTR);
        $("#lblOriginalCost").text(generatedCodeInfoResponse.CodeCostAverageExclusiveDBL);
        $("#lblSupplier").text(generatedCodeInfoResponse.CodeSupplierAccountSTR);
        $("#lblSupplierCode").text(generatedCodeInfoResponse.CodeSupplierItemCodeSTR);
        $("#lblIsServiceItem").text(generatedCodeInfoResponse.CodeIsServiceItemBOOL);
        $("#lblSellEx").text(generatedCodeInfoResponse.CodeSellingExlusive1DBL);
        //$("#lblSKUNumber").text(generatedCodeInfoResponse.CodeBarcodeSTR);
        //$("#lblCurrentQTY").text(generatedCodeInfoResponse.QtyOnHand);
        //$("#lblSellingIncl").text(((parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL) * parseFloat(TaxRate[parseInt(generatedCodeInfoResponse.CodeSalesTaxGroupSTR)])) / 100 + parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL)).toFixed(2));


        $('#' + _dropDownTaxId).val(generatedCodeInfoResponse.CodePurchaseTaxGroupSTR)

        CalculateTotalAndTax();
        $("#lblSellingTaxGroup").text($('#' + _dropDownTaxId + " option:eq(" + parseInt(generatedCodeInfoResponse.CodeSalesTaxGroupSTR) + ")").text());
        $('#txtIncl').attr("disabled", "disabled")
        $('#txtTotalIncl').attr("disabled", "disabled")
        $('#textBoxDescription').attr("disabled", "disabled")
        $('#txtExcl').attr("disabled", "disabled")
        $('#txtQty').removeAttr('disabled')
        $("#txtQty").focus();

    }
    else {
        var itemcode;
        itemcode = $('#txtItemCode').val()
        $('#txtSearch').val(itemcode)
        getItemCodeGrid()
        //clearItemCode()
        //$('#txtQty').attr("disabled", "disabled")
        //$('#textBoxDescription').attr("disabled", "disabled")
        //$('#txtIncl').attr("disabled", "disabled")
        //$('#txtExcl').attr("disabled", "disabled")
        //$('#textBoxDescription').attr("disabled", "disabled")

    }
    $('.loader-container').hide();

}

//$('#lblQty').text("0")
//$('#lblTotalExcl').text("0")
//$('#lblTotalTax').text("0")
//$('#lblTotalIncl').text("0")
//$('#' + _dropDownTaxId).removeAttr('disabled')

function CalculateTotalAndTax() {
    debugger;
    var tmpInTaxValue;
    var tmpTaxArray;
    var Incl;
    var TotalIncl;

    if ($('#' + _dropDownTaxId).text() == "") {
        tmpInTaxValue = 0;
    }
    else {
        tmpTaxArray = $('#' + _dropDownTaxId).text().split('(');
        tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));
    }
    Incl = ((((parseFloat($("#txtExcl").val()) * tmpInTaxValue).toFixed(2)) / 100) + parseFloat($("#txtExcl").val())).toFixed(2)
    $("#txtQty").val("1")

    if (isNaN(Incl)) {
        $("#txtIncl").val("0");
    }
    else {
        $("#txtIncl").val(Incl);
    }
    TotalIncl = ($("#txtQty").val() * $("#txtIncl").val()).toFixed(2)

    if (isNaN(TotalIncl)) {
        $("#txtTotalIncl").val("0")
    }
    else {
        $("#txtTotalIncl").val(TotalIncl)
    }


    $("#lblTaxAmount").text(((parseFloat($("#txtExcl").val()) * parseFloat((tmpInTaxValue)) / 100) * parseFloat($("#txtQty").val())).toFixed(2))

}
function AddNewItem() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtItemCode').val() == "") {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please select an Itemcode');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false;

        }
        if ($("#" + _dropDownTaxId + " option:selected").text() == "") {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please select a valid Tax Group');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false;
        }

        if (CodeColourMatrixSTR != null && CodeColourMatrixSTR != "") {
            getCodes()
            return false

        }

        if (CodeSizeMatrixSTR != null && CodeSizeMatrixSTR != "") {
            getCodes()
            return false

        }


        if ($("#txtQty").val() != '') {
            var newItem = "<tr class='NewItem'>" +
                "<td id='ItemCode'>" + $("#txtItemCode").val().toUpperCase() + "</td>" +
                "<td id='Description'>" + $("#txtDescription").val().toUpperCase() + "</td>" +
                "<td id='Excl'>" + $("#txtExcl").val() + "</td>" +
                "<td id='Tax'>" + $("#" + _dropDownTaxId + " option:selected").text() + "</td>" +
                "<td id='Incl'>" + $("#txtIncl").val() + "</td>" +
                "<td id='Qty'>" + $("#txtQty").val() + "</td>" +
                "<td id='TotalCostIncl'>" + $("#txtTotalIncl").val() + "</td>" +
                "<td id='MCode' style='visibility:hidden'>" + $("#lblMcode").text() + "</td>" +
                "<td id='Cat1' style='visibility:hidden'>" + $("#lblCat1").text() + "</td>" +
                "<td id='Cat2' style='visibility:hidden'>" + $("#lblCat2").text() + "</td>" +
                "<td id='Cat3' style='visibility:hidden'>" + $("#lblCat3").text() + "</td>" +
                "<td id='ItemSize' style='visibility:hidden'>" + $("#lblSize").text() + "</td>" +
                "<td id='SizeGrid' style='visibility:hidden'>" + $("#lblSizeGrid").text() + "</td>" +
                "<td id='Colour' style='visibility:hidden'>" + $("#lblColour").text() + "</td>" +
                "<td id='ColourGrid' style='visibility:hidden'>" + $("#lblColourGrid").text() + "</td>" +
                "<td id='OriginalCost' style='visibility:hidden'>" + $("#lblOriginalCost").text() + "</td>" +
                "<td id='Supplier' style='visibility:hidden'>" + $("#lblSupplier").text() + "</td>" +
                "<td id='SupplierCode' style='visibility:hidden'>" + $("#lblSupplierCode").text() + "</td>" +
                "<td id='IsServiceItem' style='visibility:hidden'>" + $("#lblIsServiceItem").text() + "</td>" +
                "<td id='SellEx' style='visibility:hidden'>" + $("#lblSellEx").text() + "</td>" +
                "<td id='SellingTaxGroup' style='visibility:hidden'>" + $("#lblSellingTaxGroup").val() + "</td>" +
                "<td id='SKUNumber' style='visibility:hidden'>" + $("#lblSKUNumber").text() + "</td>" +
                "<td id='SellingIncl' style='visibility:hidden'>" + $("#lblSellingIncl").val() + "</td>" +
                "<td id='CurrentQty'>" + $("#lblSellingIncl").val() + "</td>" +
                "</tr>";
            $("#tableItemsGrid tbody").first().append(newItem);
            $("#txtItemCode").val('');
            $("#textBoxDescription").val('');
            $("#txtExcl").val('');
            $("#txtIncl").val('');
            $("#txtQty").val('');
            $("#txtTotalIncl").val('');
            $("#textBoxItemCode").focus();
            DoTotals();
            clearItemCode();
            $("#txtItemCode").focus()
        }
    }

}
function getCodes() {

    qMasterCode = $('#lblMcode').text()
    qTaxGroup = $("#" + _dropDownTaxId + " option:selected").text()
    qTaxGroupSelling = $('#lblSellingTaxGroup').text()
    qBranchCode = $('#txtBranchCode').val()
    //ClearItemCode()
    $('.loader-container').show();

    var formData = {
        MasterCode: qMasterCode,
        TaxGroup: qTaxGroup,
        TaxGroupSelling: qTaxGroupSelling,
        BranchCode: qBranchCode
    };

    var tmpInTaxValue;
    var tmpTaxArray;
    tmpTaxArray = (qTaxGroup.split('('));
    tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));


    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=GetCodes";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        $('.loader-container').hide();
        if (respone.Success == true) {
            var newItem = "";
            $.each(respone.dt, function (index, row) {
                newItem += "<tr>" +
                    "<td class='codes' >" + row.generated_code + "</td>" +
                    "<td  class='hide'>" + row.description + "</td>" +
                    "<td class='codes'>" + row.colour_description + "</td>" +
                    "<td class='hide'>" + qTaxGroup + "</td>" +
                    "<td class='codes' >" + row.item_size + "</td>" +
                    "<td class='codes' ><input type='text' value='0' autofocus onKeyup='CalculateTotalCostIncl(this)' onKeypress='saveCode()' class='qtyinput' ></td>" +
                    "<td ><input type='text' value='0' class='totalcostinput' disabled></td>" +
                    "<td  class='hide'>" + row.master_code + "</td>" +
                    "<td  class='hide'>" + row.category_1 + "</td>" +
                    "<td  class='hide'>" + row.category_2 + "</td>" +
                    "<td  class='hide'>" + row.category_3 + "</td>" +
                    "<td  class='hide sum'>" + (((parseFloat($('#txtExcl').val()) * parseFloat((tmpInTaxValue) / 100)) + parseFloat($('#txtExcl').val())).toFixed(2)) + "</td>" +
                    "<td  class='hide'>" + row.size_matrix + "</td>" +
                    "<td  class='hide'>" + (row.estimated_cost).toFixed(2) + "</td>" +
                    "<td  class='hide'>" + row.colour_matrix + "</td>" +
                    "<td  class='hide'>" + row.item_colour + "</td>" +
                    "<td class='hide'>" + row.supplier + "</td>" +
                    "<td  class='hide'>" + row.suppliers_code + "</td>" +
                    "<td  class='hide'>" + (row.selling_price_1).toFixed(2) + "</td>" +
                    "<td  class='hide'>" + row.is_service_item + "</td>" +
                    "<td  class='hide'>" + row.sku_number + "</td>" +
                    "<td  class='hide'></td>" +
                    "<td>" + row.qty_on_hand + "</td>" +

                    "</tr>";

            });

            $("#codes tbody").html(newItem);
            $('#frmG_Grid').modal('show')
            $('#frmG_Grid codes tbody tr:first').find('.qtyinput').trigger('focus');
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }

    }, 'json');
}
function CalculateTotalCostIncl(element) {
    var costprice = parseFloat($(element).closest('tr').find('.sum').text())
    var qty = parseFloat($(element).closest('tr').find('.qtyinput').val()).toFixed(2)
    var totalresult = costprice * qty

    if (isNaN(totalresult)) {
        var temp = 0;
        $(element).closest('tr').find('.totalcostinput').val(temp)
    }
    else {
        $(element).closest('tr').find('.totalcostinput').val(parseFloat(totalresult).toFixed(2))

    }

}
function saveCode() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        addGeneratedCode()
    }
}
function addGeneratedCode() {
    var html = '';
    $('#codes>tbody>tr').each(function (index, row) {
        var qty = $(row).find('.qtyinput').val();
        //if (Number(qty) != 0) {
        if (qty > 0 || qty < 0) {

            html += "<tr class='NewItem'>" +
                "<td id='ItemCode' >" + $(this).find("td").eq(0).html() + "</td>" +
                "<td id='Description'>" + $(this).find("td").eq(1).html() + "</td>" +
                "<td id='Excl'>" + $('#txtExcl').val() + "</td>" +
                "<td id='Tax'>" + $(this).find("td").eq(3).html() + "</td>" +
                "<td  id='Incl'>" + $(this).find("td").eq(11).html() + "</td>" +
                "<td id='Qty' >" + $(this).find('.qtyinput').val() + "</td>" +
                "<td id='TotalCostIncl'>" + (parseFloat($(this).find(".sum").text() * $(this).find('.qtyinput').val())).toFixed(2) + "</td>" +
                "<td id='MCode' class='hide'>" + $(this).find("td").eq(7).html() + "</td>" +
                "<td id='Cat1' class='hide'>" + $(this).find("td").eq(8).html() + "</td>" +
                "<td id='Cat2' class='hide'>" + $(this).find("td").eq(9).html() + "</td>" +
                "<td id='Cat3' class='hide'>" + $(this).find("td").eq(10).html() + "</td>" +
                "<td id='ItemSize' class='hide'>" + $(this).find("td").eq(4).html() + "</td>" +
                "<td id='SizeGrid' class='hide'>" + $(this).find("td").eq(12).html() + "</td>" +
                "<td id='Colour' class='hide'>" + $(this).find("td").eq(15).html() + "</td>" +
                "<td id='ColourGrid' class='hide'>" + $(this).find("td").eq(14).html() + "</td>" +
                "<td id='OriginalCost' class='hide'>" + $(this).find("td").eq(13).html() + "</td>" +
                "<td id='Supplier'class='hide'>" + $(this).find("td").eq(16).html() + "</td>" +
                "<td id='SupplierCode' class='hide'>" + $(this).find("td").eq(17).html() + "</td>" +
                "<td id='IsServiceItem' class='hide'>" + $(this).find("td").eq(19).html() + "</td>" +
                "<td id='SellEx' class='hide'>" + $(this).find("td").eq(18).html() + "</td>" +
                "<td id='SellingTaxGroup' class='hide'>" + qTaxGroupSelling + "</td>" +
                "<td id='SKUNumber' class='hide'>" + $(this).find("td").eq(20).html() + "</td>" +
                "<td id='SellingIncl' class='hide'>" + $(this).find("td").eq(21).text() + "</td>" +

                "</tr>";
        }
    });

    $("#tableItemsGrid tbody").first().append(html);
    $('#frmG_Grid').modal('hide')
    DoTotals();
    clearItemCode();
    $("#txtItemCode").focus()

}
function DoTotals() {
    var tmpQty;
    var tmpTotalExcl;
    var tmpTotalIncl;
    var tmpTax;

    $('#lblQty').text("0")
    $('#lblTotalExcl').text("0")
    $('#lblTotalTax').text("0")
    $('#lblTotalIncl').text("0")
    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    $gridItemElement.each(function (index, row) {
        var $tds = $(this).find('td')
        debugger;
        tmpQty = parseFloat($tds.eq(5).text())
        tmpTotalExcl = parseFloat($tds.eq(2).text()) * parseFloat($tds.eq(5).text())
        tmpTotalIncl = parseFloat($tds.eq(4).text()) * parseFloat($tds.eq(5).text())
        tmpTax = (tmpTotalIncl) - (tmpTotalExcl)

        $('#lblQty').text((parseFloat($('#lblQty').text()) + tmpQty).toFixed(2))
        $('#lblTotalExcl').text((parseFloat($('#lblTotalExcl').text()) + tmpTotalExcl).toFixed(2))
        $('#lblTotalTax').text((parseFloat($('#lblTotalTax').text()) + tmpTax).toFixed(2))
        $('#lblTotalIncl').text((parseFloat($('#lblTotalIncl').text()) + tmpTotalIncl).toFixed(2))

    });
}
function remove() {
    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    if ($gridItemElement.length == 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('There are no items to remove');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }

    if ($('#tableItemsGrid tr.selected').length == 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('There are no items selected to remove');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }

    //$('#tableItemsGrid').row('.selected').remove().draw(false);
    $('#tableItemsGrid tr.selected').remove();
    DoTotals();

}
$(document).on('click', '#tableItemsGrid tr.NewItem', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $('#tableItemsGrid tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }



});
function Save() {
    if ($('#txtAccnum').is(':enabled')) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('Please select a valid Branch');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;

    }

    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    if ($gridItemElement.length == 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('There are no items to process');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
    if (parseInt($('#lblQty').text()) > 9999) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('You cannot process more than 9 999 items in one IBT.');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;

    }

    $('.loader-container').show();
    var json = '{'
    json += '"BranchName":"' + $("#txtCompanyName").val() + '",';
    json += '"Reference":"' + $("#txtReference").val() + '",';
    json += '"BranchCode":"' + $("#txtBranchCode").val() + '",';
    json += '"Address1":"' + $("#lblAdd1").text() + '",';
    json += '"Address2":"' + $("#lblAdd2").text() + '",';
    json += '"Address3":"' + $("#lblAdd3").text() + '",';
    json += '"Address4":"' + $("#lblAdd4").text() + '",';
    json += '"Address5":"' + $("#lblAdd5").text() + '",';
    json += '"Telephone_Number":"' + $("#lblTelephone").text() + '",';
    json += '"Fax":"' + $("#lblFax").text() + '",';
    json += '"Quantity":"' + $("#lblQty").text() + '",';
    json += '"TotalTAX":"' + $("#lblTotalTax").text() + '",';
    json += '"TotalIncl":"' + $("#lblTotalIncl").text() + '",';
    json += '"TotalExcl":"' + $("#lblTotalExcl").text() + '",';
    json += '"Current_User":"' + Current_User + '",';


    json += '"listData": [';

    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    $gridItemElement.each(function (index, row) {
        json += '{';
        var $tds = $(this).find('td')
        for (i = 0; i < $tds.length; i++) {
            json += '"' + $tds.eq(i).prop("id") + '":"' + $tds.eq(i).text() + '"';
            if (i < $tds.length - 1)
                json += ','
        }
        json += '}' + (index != $gridItemElement.length - 1 ? "," : "");

    });
    json += ']}';
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=Save";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: json }, function (response) {
        $('.loader-container').hide();

        var result = response;
        var path;
        if (result.Success == true) {
            PrintDocument(result.Path)
            clearForm();
            path = result.Path;
            setTimeout(function () {
                DeleteFile(path);
            }, 5000);

        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');

    }, 'json');
}
function PrintDocument(url) {
    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 800) / 2;
    window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
        "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);

}
function DeleteFile(Path) {
    debugger;
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=StockAdjustments";
    RequestUrl += "&Action=DeleteFile";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    var formData = {
        Path: Path,
    };
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        if (respone.Success == true) {

        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }
    }, 'json');
}
function DoSearch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        search()
    }
}