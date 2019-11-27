

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
function searchItems() {
    _searchType = 'I';

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
            rowData = respone.searchBranchList;
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
$(document).on('click', '#branch tbody tr', function () {
    var data = t.row(this).data();
    $('#txtAccnum').val(data.branch_code)
    $('#txtCompanyName').val(data.branch_name)
    getBranchDetails(data.branch_code);
    $('#Searchbrit').modal('hide');

});
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
            $('#txtAccnum').attr("disabled", "disabled")
            $('#txtCompanyName').attr("disabled", "disabled");
            $("#txtIBTOutNumber").focus();
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
function clearForm() {

    $('#lblAdd1').text("")
    $('#lblAdd2').text("")
    $('#lblAdd3').text("")
    $('#lblAdd4').text("")
    $('#lblAdd5').text("")

    $('#lblTelephone').text("")
    $('#lblFax').text("")
    $('#lblTaxNumber').text("")


    $('#lblQty').text("0")
    $('#lblTotalSentQTY').text("0")
    $('#lblTotalExcl').text("0")
    $('#lblTotalTax').text("0")
    $('#lblTotalIncl').text("0")

    $('#lblMcode').text("")
    $('#lblCat1').text("")
    $('#lblCat2').text("")
    $('#lblCat3').text("")
    $('#lblSize').text("")
    $('#lblSizeGrid').text("")
    $('#lblColour').text("")
    $('#lblColourGrid').text("")
    $('#lblOriginalCost').text("")

   
    $('#textBoxItemCode').val("")
    $('#textBoxDescription').val("")
    $('#txtSellExcl').val("")
    $('#txtTaxGroup').val("")
    $('#txtSellIncl').val("")
    $('#txtQty').val("")
    $('#txtTotalSellIncl').val("")
    $('#txtAccnum').val("")
    $('#txtCompanyName').val("")
    $('#txtIBTOutNumber').val("")
    $('#txtBoxStyle').val("")
    $('#txtAccnum').removeAttr('disabled')
    $('#txtCompanyName').removeAttr('disabled')
    $('#txtIBTOutNumber').removeAttr('disabled')

    $('#txtAccnum').focus()
    $("#tableItemsGrid tr.NewItem").remove()
}
function clearItemCode()
    {
    $('#textBoxItemCode').val("")
    $('#textBoxDescription').val("")
    $('#txtSellExcl').val("")
    $('#txtSellIncl').val("")
    $('#txtQty').val("")
    $('#txtTotalSellIncl').val("")
    $('#txtTaxGroup').val("")

    $('#lblMcode').text("")
    $('#lblCat1').text("")
    $('#lblCat2').text("")
    $('#lblCat3').text("")
    $('#lblSize').text("")
    $('#lblSizeGrid').text("")
    $('#lblColour').text("")
    $('#lblColourGrid').text("")
    $('#lblOriginalCost').text("")

    $('#textBoxItemCode').focus()
}
$(document).on('click', '#tableItemsGrid tr.NewItem', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#textBoxItemCode').val('')
        $('#textBoxDescription').val('')
        $('#txtSellExcl').val('')
        $('#txtTaxGroup').val('')
        $('#txtSellIncl').val('')
        $('#txtQty').val('')
        $('#txtTotalSellIncl').val('')
    }
    else {
        $('#tableItemsGrid tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var ItemCode = $(this).find("td").eq(0).text()
        var Description = $(this).find("td").eq(1).text()
        var SellExcl = $(this).find("td").eq(2).text()
        var TaxGroup = $(this).find("td").eq(3).text()
        var SellIncl = $(this).find("td").eq(4).text()
        var Qty = $(this).find("td").eq(5).text()
        var TotalSellIncl = $(this).find("td").eq(6).text()

        $('#textBoxItemCode').val(ItemCode)
        $('#textBoxDescription').val(Description)
        $('#txtSellExcl').val(SellExcl)
        $('#txtTaxGroup').val(TaxGroup)
        $('#txtSellIncl').val(SellIncl)
        $('#txtQty').val(Qty)
        $('#txtTotalSellIncl').val(TotalSellIncl)

    }



});
function updateQty()
{
    if ($('#tableItemsGrid tr.selected').length == 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('There are no items selected ');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
    var qty = parseFloat($('#txtQty').val()).toFixed(2)
    var totalSellIncl = (qty * $('#txtSellIncl').val()).toFixed(2)

    if (isNaN(qty)) {
        var tal = 0;
        $('#tableItemsGrid tr.selected').find('.qty').text(tal)
        $('#tableItemsGrid tr.selected').find('.totalSellIncl').text(tal)
    }
    else {
  
    $('#tableItemsGrid tr.selected').find('.qty').text(qty)
        $('#tableItemsGrid tr.selected').find('.totalSellIncl').text(totalSellIncl)
    }
    doTotals()
    clearItemCode()
        $('#tableItemsGrid tr.selected').removeClass('selected');

    }
function doTotals() {
    var tmpQty;
    var tmpSendQty;
    var tmpTax;
    var tmpTotalExcl;
    var tmpTotalIncl;

    $('#lblQty').text("0")
    $('#lblTotalExcl').text("0")
    $('#lblTotalTax').text("0")
    $('#lblTotalIncl').text("0")
    $('#lblTotalSentQTY').text("0")
    var $gridItemElement = $("#tableItemsGrid tr.NewItem");
    $gridItemElement.each(function (index, row) {
        var $tds = $(this).find('td')

        tmpQty = parseFloat($tds.eq(5).text())
        tmpSendQty = parseFloat($tds.eq(22).text())
        tmpTotalExcl = parseFloat($tds.eq(2).text()) * parseFloat($tds.eq(5).text())
        tmpTotalIncl = parseFloat($tds.eq(4).text()) * parseFloat($tds.eq(5).text())
        tmpTax = (tmpTotalIncl) - (tmpTotalExcl)

        $('#lblQty').text((parseFloat($('#lblQty').text()) + tmpQty).toFixed(2))
        $('#lblTotalSentQTY').text((parseFloat($('#lblTotalSentQTY').text()) + tmpSendQty).toFixed(2))
        $('#lblTotalExcl').text((parseFloat($('#lblTotalExcl').text()) + tmpTotalExcl).toFixed(2))
        $('#lblTotalTax').text((parseFloat($('#lblTotalTax').text()) + tmpTax).toFixed(2))
        $('#lblTotalIncl').text((parseFloat($('#lblTotalIncl').text()) + tmpTotalIncl).toFixed(2))

    });
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
function txtIbtoutKeypress() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        fetchData();
    }
}
function txtQtyKeyPress() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        updateQty();
    }
}
function fetchData() {
        if ($('#txtIBTOutNumber').val() == "") {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text('Please enter IBT Out Number');
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false;

        }
    if ($('#txtCompanyName').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('select a valid Branch to receive the stock from.');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
            if ($('#txtCompanyName').val() == "") {
                var mymodal = $('#messagePopup');
                mymodal.find('.modal-body').text('select a valid Branch to receive the stock from.');
                mymodal.find('.modal-title').text('Error');
                mymodal.modal('show');
                return false;

            }

            $('.loader-container').show();
    $("#tableItemsGrid tr.NewItem").remove()

    var BranchCode = $('#txtAccnum').val();
            var Branch = $('#txtCompanyName').val();
            var IBTNumber = $('#txtIBTOutNumber').val();
            var formData = {
                FromBranch: BranchCode,
                BranchName: Branch,
                IBTOutNumber: IBTNumber
            };

            RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT_In";
            RequestUrl += "&Action=FetchDetails";
            var stamp = new Date();
            RequestUrl += "&stamp=" + stamp.getTime();

    $('.loader-container').show();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        $('.loader-container').hide();

        if (respone.Success == true) {

            var result = respone.Data
            for (var i = 0; i < result.length; i++) {
                var newItem = "<tr class='NewItem'>" +
                    "<td id='ItemCode'>" + result[i].ItemCode + "</td>" +   /* 0*/
                    "<td id='Description'>" + result[i].Description + "</td>" +  /* 1*/
                    "<td id='Excl'>" + result[i].Excl + "</td>" +   /* 2*/  
                    "<td id='Tax'>" + result[i].Tax + "</td>" +    /* 3*/
                    "<td id='Incl'>" + result[i].Incl + "</td>" +  /* 4*/
                    "<td id='Qty' class='qty'>" + result[i].Qty + "</td>" +   /* 5*/
                    "<td id='TotalCostIncl' class='totalSellIncl'>" + result[i].TotalCostIncl + "</td>" +   /* 6*/
                    "<td id='MCode' style='visibility:hidden'>" + result[i].MCode + "</td>" +   /* 7*/
                    "<td id='Cat1' style='visibility:hidden'>" + result[i].Cat1 + "</td>" +   /* 8*/
                    "<td id='Cat2' style='visibility:hidden'>" + result[i].Cat2 + "</td>" +    /* 9*/
                    "<td id='Cat3' style='visibility:hidden'>" + result[i].Cat3 + "</td>" +    /* 10*/
                    "<td id='ItemSize' style='visibility:hidden'>" + result[i].ItemSize + "</td>" +    /* 11*/
                    "<td id='SizeGrid' style='visibility:hidden'>" + result[i].SizeGrid + "</td>" +    /* 12*/
                    "<td id='Colour' style='visibility:hidden'>" + result[i].Colour + "</td>" +       /* 13*/
                    "<td id='ColourGrid' style='visibility:hidden'>" + result[i].ColourGrid + "</td>" +    /* 14*/
                    "<td id='OriginalCost' style='visibility:hidden'>" + result[i].OriginalCost + "</td>" +   /* 15*/
                    "<td id='SellingPrice1' style='visibility:hidden'>" + result[i].SellingPrice1 + "</td>" +    /* 16*/
                    "<td id='Supplier' style='visibility:hidden'>" + result[i].Supplier + "</td>" +       /* 17*/
                    "<td id='SupplierCode' style='visibility:hidden'>" + result[i].SupplierCode + "</td>" +     /* 18*/
                    "<td id='IsServiceItem' style='visibility:hidden'>" + result[i].IsServiceItem + "</td>" +   /* 19*/
                    "<td id='CostPrice' style='visibility:hidden'>" + result[i].CostPrice + "</td>" +    /* 20*/
                    "<td id='SellingTaxGroup' style='visibility:hidden'>" + result[i].SellingTaxGroup + "</td>" +  /* 21*/
                    "<td id='Quantity' style='visibility:hidden'>" + result[i].Quantity + "</td>" +  /* 22*/

                    "</tr>";
                $("#tableItemsGrid tbody").first().append(newItem);
            }
            $('#txtIBTOutNumber').attr("disabled", "disabled");
            doTotals();
            $('#txtQty').focus()
        }
        else {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text(respone.Message);
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
        }
      
            }, 'json');


}
function save() {

    if ($('#txtAccnum').is(':enabled')) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('No Branch Selected');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        $('#txtAccnum').focus();
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
    $('.loader-container').show();


    var json = '{'
    json += '"BranchName":"' + $("#txtCompanyName").val() + '",';
    json += '"IBTOutNumber":"' + $("#txtIBTOutNumber").val() + '",';
    json += '"BranchCode":"' + $("#txtAccnum").val() + '",';
    json += '"Address1":"' + $("#lblAdd1").text() + '",';
    json += '"Address2":"' + $("#lblAdd2").text() + '",';
    json += '"Address3":"' + $("#lblAdd3").text() + '",';
    json += '"Address4":"' + $("#lblAdd4").text() + '",';
    json += '"Address5":"' + $("#lblAdd5").text() + '",';
    json += '"TotalSentQuantity":"' + $("#lblTotalSentQTY").text() + '",';
    json += '"TotalTAX":"' + $("#lblTotalTax").text() + '",';
    json += '"TotalIncl":"' + $("#lblTotalIncl").text() + '",';
    json += '"TotalExcl":"' + $("#lblTotalExcl").text() + '",';
    json += '"BoxStyle":"' + $("#txtBoxStyle").val() + '",';
    json += '"TotalReceievedQuantity":"' + $("#lblQty").text() + '",';

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
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT_In";
    RequestUrl += "&Action=Save";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: json }, function (response) {
        $('.loader-container').hide();

        var result = response;
        var path;
        if (result.Success == true) {
            debugger;

            printDocument(result.Path)
            clearForm();
            path = result.Path;
            setTimeout(function () {
                deleteFile(path);
            }, 5000);

        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');

    }, 'json');
}
function printDocument(url) {

    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 800) / 2;
    window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
        "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);

}
function deleteFile(Path) {
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=IBT_In";
    RequestUrl += "&Action=DeleteFile";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    var formData = {
        FileName: Path,
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


