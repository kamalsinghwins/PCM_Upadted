var t;
var _searchType = 'B';
var Current_Company = '<%=Current_Company =%>';
var DefaultPurchaseTaxInt = '<%=DefaultPurchaseTaxInt =%>';
var CodeColourMatrixSTR;
var CodeSizeMatrixSTR;
var bCancel;
var qMasterCode;
var qTaxGroup;
var qTaxGroupSelling;

$(document).on('click', '#branch tbody tr', function () {
    var data = t.row(this).data();
    $('#txtAccnum').val(data.branch_code)
    $('#txtCompanyName').val(data.branch_name)
    getBranchDetails(data.branch_code);
    $('#Searchbrit').modal('hide');
});
function searchBranch() {
    _searchType = 'B';
    $('#cboSearch').html('');
    $('#cboSearch').append('<option value="Branch Code">BranchCode</option>',
        '<option value="Branch Name">BranchName</option>')
    $("#dataTableItems tbody tr").remove();
    $("#dataTableItems_wrapper").hide();
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Branch');
    mymodal.modal('show');
}
function getSearch() {
    if (_searchType == 'B') {
        $("#branch").css('visibility', 'visible');
        $("#dataTableItems").css('visibility', 'hidden');
        getBranch();
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
function clearDataTable(dataTable) {
    if (dataTable != null) {
        dataTable.destroy();
    }
}
function getBranch() {
    $('.loader-container').show();
    var SearchType = $('#cboSearch').val();
    var SearchDetail = $('#txtSearch').val();
    var formData = {
        SearchType: SearchType,
        SearchDetail: SearchDetail,
    };
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=GetBranch";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];
        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData  = respone.searchBranchList;
        }
            $("#branch").css('visibility', 'visible');
        t = $("#branch").DataTable({
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
function getItemCodes() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtAccnum').is(':enabled')) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please select a Branch before inputting stock');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false;
        }   
        if ($('#textBoxItemCode').val() != "") {
            var code = $('#textBoxItemCode').val()
            getCodeDetails(code)
        }
        else {
            searchItems();
        }      
    }
}
function searchItems() {
    _searchType = 'I';
    bCancel = true  
    $('#txtSearch').focus();
    $('#txtSearch').focus();
    $('#cboSearch').html('');
    $('#txtLimit').val('10');
    $('#cboSearch').append(
        '<option value = "Generated Code" >Generated Code</option > ',      
        )
    $('#branch tbody tr').remove()
    $('#branch_wrapper').hide()
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();

}
function getItems() {
    $('.loader-container').show();
    var SearchType = $('#cboSearch').val();
    var SearchText = $('#txtSearch').val();
    var formData = {
        SearchType: "Generated Code",
        SearchText: SearchText,
    };
    _searchType == 'I'
    $("#branch").hide();
    $("#branch_wrapper").hide()
    $('#cboSearch').html('');
    $('#cboSearch').append(
        '<option value = "Generated Code" >Generated Code</option > ',
    )
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=GetItems";
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
            t = $("#dataTableItems").DataTable({
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
$(document).on('click', '#dataTableItems tbody tr', function () {
    var data = t.row(this).data();
    $('#textBoxItemCode').val(data.code)
    getCodeDetails(data.code);
    $('#Searchbrit').modal('hide');
    $("#textBoxItemCode").focus();
});
function getCodeDetails(itemCode) {
    var formData = {
        itemCode: itemCode,
        BranchCode: $('#txtAccnum').val()
    };
    $('.loader-container').show();
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=GetItemCodeDetails";
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
        $("#txtExcl").val((generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL).toFixed(2))
        $("#textBoxDescription").val(generatedCodeInfoResponse.CodeDescriptionSTR);
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
        $("#lblSellingIncl").text(((parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL) * parseFloat(TaxRate[parseInt(generatedCodeInfoResponse.CodeSalesTaxGroupSTR)])) / 100 + parseFloat(generatedCodeInfoResponse.CodeSellingExlusive1DBL)).toFixed(2));

        // Set Global Variables
        CodeColourMatrixSTR = generatedCodeInfoResponse.CodeColourMatrixSTR
        CodeSizeMatrixSTR = generatedCodeInfoResponse.CodeSizeMatrixSTR
        CodeSalesTaxGroupSTR = generatedCodeInfoResponse.CodeSalesTaxGroupSTR      
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
        itemcode = $('#textBoxItemCode').val()
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
function CalculateTotalAndTax() {
    var tmpInTaxValue;
    var tmpTaxArray;
    if ($('#' + _dropDownTaxId).text() == "") {
        tmpInTaxValue = 0;
    }
    else {
        tmpTaxArray = $('#' + _dropDownTaxId).text().split('(');
        tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));
    }
    $("#txtIncl").val(((((parseFloat($("#txtExcl").val()) * tmpInTaxValue).toFixed(2)) / 100) + parseFloat($("#txtExcl").val())).toFixed(2));
    $("#txtQty").val("1")
    $("#txtTotalIncl").val(($("#txtQty").val() * $("#txtIncl").val()).toFixed(2))
    $("#lblTaxAmount").text(((parseFloat($("#txtExcl").val()) * parseFloat((tmpInTaxValue)) / 100) * parseFloat($("#txtQty").val())).toFixed(2))
}
function getBranchDetails(branchCode) {
    $('.loader-container').show();
    var formData = {
        BranchCode: $('#txtAccnum').val()
    };
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=GetBranchDetails";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) },  showBranchDetails, 'json');
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
            $('#txtAccnum').attr("disabled", "disabled")
            $('#txtCompanyName').attr("disabled", "disabled");
            $("#txtReference").focus();
        }
        else {

            $("#lblAdd1").text('');
            $("#lblAdd2").text('');
            $("#lblAdd3").text('');
            $("#lblAdd4").text('');
            $("#lblAdd5").text('');
            $("#lblTelephone").text('');
            $("#lblFax").text('');
            $("#lblTaxNumber").text('');
        }
    }
    else {
        $("#lblAdd1").text('');
        $("#lblAdd2").text('');
        $("#lblAdd3").text('');
        $("#lblAdd4").text('');
        $("#lblAdd5").text('');
        $("#lblTelephone").text('');
        $("#lblFax").text('');
        $("#lblTaxNumber").text('');
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(branchDetailResponse.Message);
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
    }
    $('.loader-container').hide();
}
function AddNewItem() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#textBoxItemCode').val() == "") {
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
                "<td id='ItemCode'>" + $("#textBoxItemCode").val() + "</td>" +
                "<td id='Description'>" + $("#textBoxDescription").val() + "</td>" +
                "<td id='Excl'>" + $("#txtExcl").val() + "</td>" +
                "<td id='Tax'>" + $("#" + _dropDownTaxId + " option:selected").text() + "</td>" +
                "<td id='Incl'>" + $("#txtIncl").val() + "</td>" +
                "<td id='Qty'>" + $("#txtQty").val() + "</td>" +
                "<td id='TotalCostIncl'>" + $("#txtTotalIncl").val() + "</td>" +
                "<td id='MCode' style='visibility:hidden'>" + $("#txtTotalIncl").val() + "</td>" +
                "<td id='Cat1' style='visibility:hidden'>" + $("#lblCat1").val() + "</td>" +
                "<td id='Cat2' style='visibility:hidden'>" + $("#lblCat2").val() + "</td>" +
                "<td id='Cat3' style='visibility:hidden'>" + $("#lblCat3").val() + "</td>" +
                "<td id='ItemSize' style='visibility:hidden'>" + $("#lblSize").val() + "</td>" +
                "<td id='SizeGrid' style='visibility:hidden'>" + $("#lblSizeGrid").val() + "</td>" +
                "<td id='Colour' style='visibility:hidden'>" + $("#lblColour").val() + "</td>" +
                "<td id='ColourGrid' style='visibility:hidden'>" + $("#lblColourGrid").val() + "</td>" +
                "<td id='OriginalCost' style='visibility:hidden'>" + $("#lblOriginalCost").val() + "</td>" +
                "<td id='Supplier' style='visibility:hidden'>" + $("#lblSupplier").val() + "</td>" +
                "<td id='SupplierCode' style='visibility:hidden'>" + $("#lblSupplierCode").val() + "</td>" +
                "<td id='IsServiceItem' style='visibility:hidden'>" + $("#lblIsServiceItem").val() + "</td>" +
                "<td id='SellEx' style='visibility:hidden'>" + $("#lblSellEx").val() + "</td>" +
                "<td id='SellingTaxGroup' style='visibility:hidden'>" + $("#lblSellingTaxGroup").val() + "</td>" +
                "<td id='SKUNumber' style='visibility:hidden'>" + $("#lblSKUNumber").val() + "</td>" +
                "<td id='SellingIncl' style='visibility:hidden'>" + $("#lblSellingIncl").val() + "</td>" +
                "</tr>";
            $("#tableItemsGrid tbody").first().append(newItem);
            $("#textBoxItemCode").val('');
            $("#textBoxDescription").val('');
            $("#txtExcl").val('');
            $("#txtIncl").val('');
            $("#txtQty").val('');
            $("#txtTotalIncl").val('');
            $("#textBoxItemCode").focus();
            DoTotals();
            clearItemCode();
        }
    }

}
function save() {    
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
    if ($('#lblQty').val() > 99999) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('You cannot process more than 99 999 items in one IBT.');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
    $('.loader-container').show();
    var json = '{'
    json += '"BranchName":"' + $("#txtCompanyName").val() + '",';
    json += '"Reference":"' + $("#txtReference").val() + '",';
    json += '"BranchCode":"' + $("#txtAccnum").val() + '",';
    json += '"Address1":"' + $("#lblAdd1").text() + '",';
    json += '"Address2":"' + $("#lblAdd2").text() + '",';
    json += '"Address3":"' + $("#lblAdd3").text() + '",';
    json += '"Address4":"' + $("#lblAdd4").text() + '",';
    json += '"Address5":"' + $("#lblAdd5").text() + '",';
    json += '"Quantity":"' + $("#lblQty").text() + '",';
    json += '"TotalTAX":"' + $("#lblTotalTax").text() + '",';
    json += '"TotalIncl":"' + $("#lblTotalIncl").text() + '",';
    json += '"TotalExcl":"' + $("#lblTotalExcl").text() + '",';
    json += '"BoxStyle":"' + $("#txtBoxStyle").val() + '",';
    json += '"CurrentCompany":"' + Current_Company + '",';
    json += '"Branch_Name":"' + Branch_Name + '",';
    json += '"Current_Branch_Code":"' + Current_Branch_Code + '",';
    json += '"Branch_Telephone_Number":"' + Branch_Telephone_Number + '",';
    json += '"Branch_Fax_Number":"' + Branch_Fax_Number + '",';
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
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=Save";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: json }, function (response) {
        $('.loader-container').hide();

        var result = response;
        var path;
        var LabelPath;
        if (result.Success == true) {
            downloadLabel(result.LabelPath)
            PrintDocument(result.Path)
            clearForm();

            path = result.Path;
            LabelPath = result.LabelPath
            setTimeout(function () {
                DeleteFile(path, LabelPath);
            }, 5000);
            
        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ?'Success' :'Error');
        mymodal.modal('show');

    }, 'json');
}
function Reload() {
    if ($('#txtAccnum').is(':enabled')) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('Please select a valid Branch');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    if ($gridItemElement.length > 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('Please clear all items in the list before Reloading the last IBT Out');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=Reload";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, function (respone) {
        if (respone.Success == true) {
            var result = respone.ListData
            for (var i = 0; i < result.length; i++) {
                var newItem = "<tr class='NewItem'>" +
                    "<td id='ItemCode'>" + result[i].ItemCode + "</td>" +
                    "<td id='Description'>" + result[i].Description + "</td>" +
                    "<td id='Excl'>" + result[i].Excl + "</td>" +
                    "<td id='Tax'>" + result[i].Tax + "</td>" +
                    "<td id='Incl'>" + result[i].Incl + "</td>" +
                    "<td id='Qty'>" + result[i].Qty + "</td>" +
                    "<td id='TotalCostIncl'>" + result[i].TotalCostIncl + "</td>" +
                    "<td id='MCode' style='visibility:hidden'>" + result[i].MCode + "</td>" +
                    "<td id='Cat1' style='visibility:hidden'>" + result[i].Cat1 + "</td>" +
                    "<td id='Cat2' style='visibility:hidden'>" + result[i].Cat2 + "</td>" +
                    "<td id='Cat3' style='visibility:hidden'>" + result[i].Cat3 + "</td>" +
                    "<td id='ItemSize' style='visibility:hidden'>" + result[i].ItemSize + "</td>" +
                    "<td id='SizeGrid' style='visibility:hidden'>" + result[i].SizeGrid + "</td>" +
                    "<td id='Colour' style='visibility:hidden'>" + result[i].Colour + "</td>" +
                    "<td id='ColourGrid' style='visibility:hidden'>" + result[i].ColourGrid + "</td>" +
                    "<td id='OriginalCost' style='visibility:hidden'>" + result[i].OriginalCost + "</td>" +
                    "<td id='Supplier' style='visibility:hidden'>" + result[i].Supplier + "</td>" +
                    "<td id='SupplierCode' style='visibility:hidden'>" + result[i].SupplierCode + "</td>" +
                    "<td id='IsServiceItem' style='visibility:hidden'>" + result[i].IsServiceItem + "</td>" +
                    "<td id='SellEx' style='visibility:hidden'>" + result[i].SellEx + "</td>" +
                    "<td id='SellingTaxGroup' style='visibility:hidden'>" + result[i].SellingTaxGroup + "</td>" +
                    "<td id='SKUNumber' style='visibility:hidden'>" + result[i].SKUNumber + "</td>" +
                    "<td id='SellingIncl' style='visibility:hidden'>" + result[i].SellingIncl + "</td>" +
                    "</tr>";
                $("#tableItemsGrid tbody").first().append(newItem);
            }
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }
        DoTotals();
    }, 'json');
  
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
function clearLineItem() {
    $("#textBoxItemCode").val('')
    $("#textBoxDescription").val('')
    $("#txtExcl").val('')
    $("#txtIncl").val('')
    $("#txtQty").val('')
    $("#txtTotalIncl").val('')
    $("#lblCat1").text('')
    $("#lblCat2").text('')
    $("#lblCat3").text('')
    $("#lblSize").text('')
    $("#lblSizeGrid").text('')
    $("#lblColour").text('')
    $("#lblColourGrid").text('')
    $("#lblOriginalCost").text('')
    $("#lblSupplier").text('')
    $("#lblSupplierCode").text('')
    $("#lblIsServiceItem").text('')
    $("#lblSellEx").text('')
    $("#lblSellingTaxGroup").text('')
    $("#lblSKUNumber").text('')
    $("#lblSellingIncl").text('')
    $('#lblQty').text("0")
    $('#lblTotalExcl').text("0")
    $('#lblTotalTax').text("0")
    $('#lblTotalIncl').text("0")
}
function clearItemCode() {
    $("#textBoxItemCode").val('')
    $("#textBoxDescription").val('')
    $("#txtExcl").val('')
    $("#txtIncl").val('')
    $("#txtQty").val('')
    $("#txtTotalIncl").val('')
    $('#lblGeneratedCode').text("")
    $('#lblMcode').text("")
    $("#lblCat1").text('')
    $("#lblCat2").text('')
    $("#lblCat3").text('')
    $("#lblSize").text('')
    $("#lblSizeGrid").text('')
    $("#lblColour").text('')
    $("#lblColourGrid").text('')
    $("#lblOriginalCost").text('')
    $("#lblSKUNumber").text('')
    $("#lblSellingIncl").text('')
    $("#textBoxItemCode").focus()
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
$(document).on('click', '#tableItemsGrid tr.NewItem', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $('#tableItemsGrid tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});
function CalculateTax() {
    var tmpInTaxValue;
    var tmpTaxArray;
    if ($('#' + _dropDownTaxId).text() == "") {
        tmpInTaxValue = 0;
    }
    else {
        tmpTaxArray = $('#' + _dropDownTaxId).text().split('(');
        tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));
    }
    $("#lblTaxAmount").text(((parseFloat($("#txtExcl").val()) * parseFloat((tmpInTaxValue)) / 100) * parseFloat($("#txtQty").val())))
    $("#txtIncl").val(parseFloat($("#txtExcl").val()) + parseFloat($("#lblTaxAmount").text()));
    $("#txtTotalIncl").val($("#txtExcl").val() + parseFloat($("#lblTaxAmount").text()) * parseFloat($("#txtQty").val()))
}
function CalculateTaxAmount() {
    var tmpInTaxValue;
    var tmpTaxArray;
    if ($('#' + _dropDownTaxId).text() == "") {
        tmpInTaxValue = 0;
    }
    else {
        tmpTaxArray = $('#' + _dropDownTaxId).text().split('(');
        tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));
    }
    var Tax = ((parseFloat($("#txtExcl").val()) * parseFloat((tmpInTaxValue)) / 100) * parseFloat($("#txtQty").val())).toFixed(2)
    var TotalIncl = (parseFloat($("#txtIncl").val()) * parseFloat($("#txtQty").val())).toFixed(2)

    if (isNaN(Tax)) {
        var tal = 0;
        $("#lblTaxAmount").text(tal)
    }
    else
        {
        $("#lblTaxAmount").text(Tax)

        }   
    if (isNaN(TotalIncl)) {
        var tIncl = 0;
        $("#txtTotalIncl").val(tIncl)
    }
    else {
        $("#txtTotalIncl").val(TotalIncl)

    }
}
function PrintDocument(url) {
    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 800) / 2;
    window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
        "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);  
}
function clearForm() {
    $('#lblAdd1').text("")
    $('#lblAdd2').text("")
    $('#lblAdd3').text("")
    $('#lblAdd4').text("")
    $('#lblAdd5').text("")
    $('#lblQty').text("0")
    $('#lblTotalExcl').text("0")
    $('#lblTotalTax').text("0")
    $('#lblTotalIncl').text("0")
    $('#lblCat1').text("")
    $('#lblCat2').text("")
    $('#lblCat3').text("")
    $('#lblSize').text("")
    $('#lblSizeGrid').text("")
    $('#lblMcode').text("")
    $('#lblColour').text("")
    $('#lblColourGrid').text("")
    $('#lblTaxAmount').text("")
    $('#lblOriginalCost').text("")
    $('#lblSupplier').text("")
    $('#lblGeneratedCode').text("")
    $('#lblSellingIncl').text("")
    $('#lblSKUNumber').text("")
    $('#lblSellingTaxGroup').text("")
    $('#lblSupplierCode').text("")
    $('#lblIsServiceItem').text("")
    $('#lblSellEx').text("")
    $('#lblTelephone').text("")
    $('#lblFax').text("")
    $('#lblTaxNumber').text("")
    $('#textBoxItemCode').val("")
    $('#textBoxDescription').val("")
    $('#txtExcl').val("")
    $('#txtIncl').val("")
    $('#txtQty').val("")
    $('#txtTotalIncl').val("")
    $('#txtAccnum').val("")
    $('#txtCompanyName').val("")
    $('#txtReference').val("")
    $('#txtBoxStyle').val("")
    $('#txtAccnum').removeAttr('disabled')
    $('#txtCompanyName').removeAttr('disabled')
    $('#' + _dropDownTaxId).val("1")
    $("#tableItemsGrid tr.NewItem").remove()
}
function getCodes() {
    qMasterCode = $('#lblMcode').text()
    qTaxGroup = $("#" + _dropDownTaxId + " option:selected").text()
    qTaxGroupSelling = $('#lblSellingTaxGroup').text()
    DoTotals()
    $('.loader-container').show();
    var formData = {
        MasterCode: qMasterCode,
        TaxGroup: qTaxGroup,
        TaxGroupSelling: qTaxGroupSelling
    };
    var tmpInTaxValue;
    var tmpTaxArray;  
    tmpTaxArray = (qTaxGroup.split('('));
        tmpInTaxValue = tmpTaxArray[1].substr(0, tmpTaxArray[1].indexOf(')'));
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
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
                    "<td  class='hide'>" + (((parseFloat((row.selling_price_1)) * TaxRate[parseInt(CodeSalesTaxGroupSTR)]) / 100) + parseFloat((row.selling_price_1))).toFixed(2) + "</td>" +
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
function addGeneratedCode() {
    var html = '';
    $('#codes>tbody>tr').each(function (index, row) {
        if ($(row).find('.qtyinput').val() > 0) {
            html += "<tr class='NewItem'>" +
                "<td id='ItemCode' >" + $(this).find("td").eq(0).html() + "</td>" +
                "<td id='Description'>" + $(this).find("td").eq(1).html() + "</td>" +
                "<td id='Excl'>" + $('#txtExcl').val() + "</td>" +
                "<td id='Tax'>" + $(this).find("td").eq(3).html() + "</td>" +    
                "<td  id='Incl'>" + $(this).find("td").eq(11).html() + "</td>" +
                "<td id='Qty' >" + $(this).find('.qtyinput').val() + "</td>" +
                "<td id='TotalCostIncl'>" + (parseFloat($(this).find(".totalcostinput").val() * $(this).find('.qtyinput').val())).toFixed(2) + "</td>" +
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
}
function saveCode()
{
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        addGeneratedCode()
        }
}
function CalculateTotalCostIncl(element) {
    var costprice = parseFloat($(element).closest('tr').find('.sum').text())
    var qty = parseFloat($(element).closest('tr').find('.qtyinput').val()).toFixed(2)
    var totalresult = costprice * qty
    if (isNaN(totalresult)) {
        var temp = 0;
        $(element).closest('tr').find('.totalcostinput').val(temp )
    }
    else {
        $(element).closest('tr').find('.totalcostinput').val(parseFloat(totalresult).toFixed(2))
    }
}
function setBranch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtAccnum').val() == "") {
            searchBranch()
        }
        else {
            var branchcode;
            branchcode = $('#txtAccnum').val()
            getBranchDetails(branchcode)
            $("#dataTableItems tbody tr").remove();
            $("#dataTableItems_wrapper").hide();
            $("#dataTableItems").css('visibility', 'hidden');
        }     
    }
}
function DeleteFile(Path,LabelPath) {
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
    RequestUrl += "&Action=DeleteFile";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    var formData = {
        Path:Path,
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
function doSearch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        getSearch()
    }
}
function downloadLabel(path) {
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT";
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
$('#Searchbrit').on('shown.bs.modal', function () {
    $('#txtSearch').trigger('focus');
});
$(document).on('shown.bs.modal', function () {
    $('#txtSearch').trigger('focus');
    $('#frmG_Grid #codes tbody tr:first').find('.qtyinput').trigger('focus');
});

