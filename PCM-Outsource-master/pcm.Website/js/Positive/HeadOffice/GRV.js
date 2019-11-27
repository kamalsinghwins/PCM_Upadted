var _searchType;
var CodeColourMatrixSTR;
var CodeSizeMatrixSTR;
var CodeCostAverageExclusiveDBL
var CodeEstimatedCostExclusiveDBL


function setSupplier() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtSupplierAccount').val() == "") {
            searchSupplier()
        }
        else {
            getSupplierDetails()

        }
    }

}
function searchSupplier() {
    _searchType = 'S';
    $('#txtSearch').val("");
    $('#txtSearch').focus();
    $('#cboSearch').html('');
    $('#cboSearch').append('<option value="Account">Account</option>',
        '<option value="Company Name">Company Name</option>')
    $("#dataTableItems tbody tr").remove();
    $("#dataTableItems_wrapper").hide();
    $("#chkMaster").hide();
    $("#Master").hide();

    $('#gridItemCode').hide();
    $('#gridItemCode_wrapper').hide();
    $("#gridSupplier").show();
    $("#gridSupplier_wrapper").show()

    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Search');
    mymodal.modal('show');
   
}
function getSuppliers() {
    $('.loader-container').show();
    var SearchType = $('#cboSearch').val();
    var SearchText = $('#txtSearch').val();
    var formData = {
        SearchType: SearchType,
        SearchText: SearchText,
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=GetSupplier";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];

        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData = respone.dt;
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');

        }
        $('#gridSupplier').show();
        $('#gridSupplier_wrapper').show();
        $("#gridItemCode").hide();
        $("#gridItemCode_wrapper").hide()

        $("#gridSupplier").css('visibility', 'visible');
        t = $("#gridSupplier").DataTable({
            paging: true,
            data: rowData,
            columns: [
                { 'data': 'supplier_code' },
                { 'data': 'supplier_name' },
            ],
            destroy: true

        });
    }, 'json');


}
function getSupplierDetails() {
        $('.loader-container').show();
        var formData = {
            SupplierCode: $('#txtSupplierAccount').val()
        };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=SupplierDetails";
        var stamp = new Date();
        RequestUrl += "&stamp=" + stamp.getTime();
        $("#dataTableItems").css('visibility', 'visible');
        $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showSupplierDetails, 'json');

    
}
function showSupplierDetails(supplierDetailResponse) {
    if (supplierDetailResponse.Success == true) {

        if (supplierDetailResponse.IsBlocked == false) {
            $('#txtSupplierName').val(supplierDetailResponse.SupplierName)
            $("#lblAdd1").text(supplierDetailResponse.AddressLine1);
            $("#lblAdd2").text(supplierDetailResponse.AddressLine2);
            $("#lblAdd3").text(supplierDetailResponse.AddressLine3);
            $("#lblAdd4").text(supplierDetailResponse.AddressLine4);
            $("#lblAdd5").text(supplierDetailResponse.AddressLine5);
            $("#lblTelephone").text(supplierDetailResponse.Telephone);
            $("#lblFax").text(supplierDetailResponse.FAX);
            $("#lblTaxNumber").text(supplierDetailResponse.TAX);
            $('#txtSupplierAccount').attr("disabled", "disabled")
            $('#txtSupplierName').attr("disabled", "disabled");
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
        //clearForm();
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(supplierDetailResponse.Message);
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');

    }
    $('.loader-container').hide();

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

    $('#gridItemCode').show();
    $('#gridItemCode_wrapper').show();
    $("#gridSupplier").hide();
    $("#gridSupplier_wrapper").hide()

    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();
}
function doSearch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if (_searchType == 'S') {
            getSuppliers()
        }
        else {
            getItems()
        }
    }
}
function getItemCodes() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtSupplierAccount').is(':enabled')) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please select a supplier before inputting stock');
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
function getItems() {
    $('.loader-container').show();
    var SearchText = $('#txtSearch').val();
    var formData = {
        SearchText: SearchText,
    };
    _searchType == 'I'
    $("#gridSupplier").hide();
    $("#gridSupplier_wrapper").hide()
    $("#gridItemCode").show();
    $("#gridItemCode_wrapper").show();
    $("#gridItemCode").css('visibility', 'visible');

    $('#cboSearch').append(
        '<option value = "Generated Code" >Generated Code</option > ',
    )
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=GetGeneratedCodes";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        $('.loader-container').hide();
        var rowData = [];
        if (respone.Success == true) {
            if (respone.dt != null) {
                rowData = respone.dt;
            }
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }
        t = $("#gridItemCode").DataTable({
            'pagin': true,
            data: rowData,
            columns: [
                { 'data': 'code' },
                { 'data': 'description' },
                { 'data': 'sku_number' }
            ],
            destroy: true
        });
    }, 'json');
    var mymodal = $('#Searchbrit');
    mymodal.modal('show');

}
function getSearch() {
    if (_searchType == 'S') {
        $("#branch").css('visibility', 'visible');
        $("#dataTableItems").css('visibility', 'hidden');
        getSuppliers();
        $("#branch").show();
        $("#branch_wrapper").show()
        $('#txtSearch').focus();
    }
    else if (_searchType == 'I') {
        $("#dataTableItems").css('visibility', 'visible');
        getItems();
        $("#dataTableItems").show();
        $("#dataTableItems_wrapper").show()
        $('#txtSearch').focus();
    }
}
function getCodeDetails(itemCode) {
    var formData = {
        ItemCode: itemCode,
    };
    $('.loader-container').show();
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=GetCodes";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showItemCodeDetails, 'json');
}
function showItemCodeDetails(generatedCodeInfoResponse) {
    if (generatedCodeInfoResponse.GetGeneratedCodeInfo == true) {
        if (generatedCodeInfoResponse.CodeIsBlockedBOOL == true) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('This Item has been blocked.');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            $('.loader-container').hide();
            //clearLineItem()
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
        $("#txtExcl").val((generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL).toFixed(2))
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
        $("#lblSellEx").text((generatedCodeInfoResponse.CodeSellingExlusive1DBL).toFixed(2));
        $("#lblSKUNumber").text(generatedCodeInfoResponse.CodeBarcodeSTR);
        //$("#lblSellingIncl").text(((parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL) * parseFloat(TaxRate[parseInt(generatedCodeInfoResponse.CodeSalesTaxGroupSTR)])) / 100 + parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL)).toFixed(2));
        debugger;
        // Set Global Variables
        CodeColourMatrixSTR = generatedCodeInfoResponse.CodeColourMatrixSTR
        CodeSizeMatrixSTR = generatedCodeInfoResponse.CodeSizeMatrixSTR
        CodeSalesTaxGroupSTR = generatedCodeInfoResponse.CodeSalesTaxGroupSTR
        $('#' + _dropDownTaxId).val(generatedCodeInfoResponse.CodePurchaseTaxGroupSTR)
        CalculateTotalAndTax();
        //$("#lblSellingTaxGroup").text($('#' + _dropDownTaxId + " option:eq(" + parseInt(generatedCodeInfoResponse.CodeSalesTaxGroupSTR) + ")").text());
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
        getItems()
        //clearItemCode() 
        //$('#txtQty').attr("disabled", "disabled")
        //$('#textBoxDescription').attr("disabled", "disabled")
        //$('#txtIncl').attr("disabled", "disabled")
        //$('#txtExcl').attr("disabled", "disabled")
        //$('#textBoxDescription').attr("disabled", "disabled")
    }
    $('.loader-container').hide();
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
    $('#tableItemsGrid tr.selected').remove();
    DoTotals();
}
function clearForm()
{
    $('#txtSupplierAccount').val("")
    $('#txtSupplierName').val("")
    $('#txtReference').val("")
    $("#tableItemsGrid tr.NewItem").remove()
    $('#txtSupplierAccount').removeAttr('disabled')
    $('#txtSupplierName').removeAttr('disabled')
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
    $('#txtSupplierAccount').focus()
    $("#tableItemsGrid tr.NewItem").remove()
    $('#' + _dropDownTaxId).val("0")
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


    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=GetAllCodes";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        $('.loader-container').hide();
        if (respone.Success == true) {
            var newItem = ""; debugger;
            $.each(respone.dt, function (index, row) {
                var cost = (((parseFloat($('#txtExcl').val()) * parseFloat((tmpInTaxValue) / 100)) + parseFloat($('#txtExcl').val())).toFixed(2))
                newItem += "<tr data-itemcode='" + row.generated_code +"'>" +
                    "<td class='codes' >" + row.generated_code + "</td>" +
                    "<td  class='hide'>" + row.description + "</td>" +
                    "<td class='codes'>" + row.colour_description + "</td>" +
                    "<td class='hide'>" + qTaxGroup + "</td>" +
                    "<td class='codes' >" + row.item_size + "</td>" +
                    "<td class='codes' ><input type='text' value='0' autofocus onKeyup='CalculateTotalCostIncl(this)' onKeypress='saveCode()' class='qtyinput' ></td>" +
                    "<td class='codes'><input type='text' value='0' disabled class='totalcostinput'></td>" +
                    "<td  class='hide'>" + row.master_code + "</td>" +
                    "<td  class='hide'>" + row.category_1 + "</td>" +
                    "<td  class='hide'>" + row.category_2 + "</td>" +
                    "<td  class='hide'>" + row.category_3 + "</td>" +
                    "<td  class='sum'><input type='text' value="+ cost +" class='costinput'></td>" +
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
function saveCode() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        addGeneratedCode()
    }
}
function addGeneratedCode() {
    var html = '';
    var newItem = "";
    var bool;
    var addedCodes = $("#tableItemsGrid > tbody > tr.NewItem > td:first-child").map(function () {
        return this.textContent;
    }).get();

    debugger;

    //check itemcode already exist
    //=========================================================================================
    var popupCodes = $("#codes > tbody > tr > td:first-child").map(function () {
        return this.textContent;
    }).get();

    var existingCodes = [];
    var newCodes = [];

    var newCodesRowsSelector = "";

    popupCodes.forEach(function (code) {
        if ((addedCodes.indexOf(code) !== -1))
            existingCodes.push(code)
        else {
            newCodes.push(code);
            newCodesRowsSelector += "tr[data-itemcode='" + code +"'],"
        }
    });

    if (newCodesRowsSelector.length > 0) {
        newCodesRowsSelector = newCodesRowsSelector.substr(0, newCodesRowsSelector.length - 1);

    
    //=====================================================================================

    $(newCodesRowsSelector).each(function (index, row) {
        if ($(row).find('.qtyinput').val() > 0) {

            html += "<tr class='NewItem'>" +
                "<td id='ItemCode' >" + $(this).find("td").eq(0).html() + "</td>" +
                "<td id='Description'>" + $(this).find("td").eq(1).html() + "</td>" +
                "<td id='Excl'>" + $('#txtExcl').val() + "</td>" +
                "<td id='Tax'>" + $(this).find("td").eq(3).html() + "</td>" +
                "<td  id='Incl'>" + $(this).find('.costinput').val() + "</td>" +
                "<td id='Qty' >" + $(this).find('.qtyinput').val() + "</td>" +
                "<td id='TotalIncl'>" + (parseFloat($(this).find(".totalcostinput").val())) + "</td>" +
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
    }


    $("#tableItemsGrid tbody").first().append(html);
    $('#frmG_Grid').modal('hide')
    DoTotals();
    clearItemCode();
    $("#txtItemCode").focus();

    if (existingCodes.length > 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(existingCodes.join(",") + " already exists in the GRV. It will not be added to the current GRV.");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        $('#frmG_Grid').modal('hide')
        return false;

    }

}
function Save() {
    if ($('#txtSupplierAccount').is(':enabled')) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('No supplier selected.');
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
    json += '"SupplierAccount":"' + $("#txtSupplierAccount").val() + '",';
    json += '"Reference":"' + $("#txtReference").val() + '",';
    json += '"SupplierName":"' + $("#txtSupplierName").val() + '",';
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
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=SaveGRV";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: json }, function (response) {
        $('.loader-container').hide();

        var result = response;
        var path;
        var labelPath;
        if (result.Success == true) {
            downloadLabel(result.LabelPath)
            PrintDocument(result.Path)          
            clearForm();
            path = result.Path;
            labelPath = result.LabelPath
            setTimeout(function () {
                DeleteFile(path, labelPath);
            }, 5000);

        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');

    }, 'json');
}
function CalculateTotalAndTax() {
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
        debugger;
        var $tds = $(this).find('td')
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
function CalculateTotalCostIncl(element) {
    var costprice = parseFloat($(element).closest('tr').find('.costinput').val())
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
function PrintDocument(url) {
    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 800) / 2;
    window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
        "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);

}
function downloadLabel(path) {
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=DownloadLabel";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    //setup ajax
    $.ajaxSetup({
        beforeSend: function (jqXHR, settings) {
            if (settings.dataType === 'binary') {
                settings.xhr().responseType = 'arraybuffer';
                settings.processData = false;
            }
        }
    });
    $.ajax({
        method: 'Post',
        url: RequestUrl,
        data: { FormData: path },
        dataType: "binary",
        success: function (data) {
            console.log(data); //ArrayBuffer
            console.log(new Blob([data])) // Blob
        },
        error: function (error) {
            var a = window.document.createElement('a');
            a.href = window.URL.createObjectURL(new Blob([error.responseText], { type: 'application/octet-stream' }));
            a.download = path;
            // Append anchor to body.
            document.body.appendChild(a)
            a.click();
            // Remove anchor from body
            document.body.removeChild(a)
        }
    });
}
function DeleteFile(Path, LabelPath) {
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=GRV";
    RequestUrl += "&Action=DeleteFile";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    var formData = {
        Path: Path,
        LabelPath: LabelPath
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
$(document).on('click', '#gridSupplier tbody tr', function () {
    var data = t.row(this).data();
    $('#txtSupplierAccount').val(data.supplier_code)
    getSupplierDetails(data.supplier_code);
    $('#Searchbrit').modal('hide');

});
$(document).on('click', '#gridItemCode tbody tr', function () {
    var data = t.row(this).data();
    $('#txtItemCode').val(data.code)
    getCodeDetails(data.code);
    $('#Searchbrit').modal('hide');
    $("#textBoxItemCode").focus();
});
$(document).on('click', '#tableItemsGrid tr.NewItem', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $('#tableItemsGrid tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});
