
var _searchType = 'S';
var t;
$(document).on('click', '#branch tbody tr', function () {
    var data = t.row(this).data();
    getSpecialDetails(data.special_name);
    $('#Searchbrit').modal('hide');

});
$(document).on('click', '#dataTableItems tbody tr', function () {
    var data = t.row(this).data();
    $('#txtCode').val(data.code)
    $('#txtQuantity').val("1")
    $('#txtQuantity').focus()
    $('#Searchbrit').modal('hide');

});
function specialPopup() {
    _searchType = 'S';
    $('#cboSearch').html('');
    $('#cboSearch').append('<option value="Special Name">Special Name</option>'),
    $("#dataTableItems tbody tr").remove();
    $("#dataTableItems_wrapper").hide();
    $("#chkMaster").hide();
    $("#Master").hide();
    $('#dataTableItems').hide()
    $('#txtSearch').val("");

    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Specials');
    mymodal.modal('show');

}
function codePopup() {
    _searchType = 'C';
    bCancel = true


    $('#txtSearch').focus();
    $('#txtSearch').focus();
    $('#cboSearch').html('');
    $('#txtLimit').val('10');
    $('#cboSearch').append('<option value="Master Code">Master Code</option>',
        '<option value = "Generated Code" >Generated Code</option > ',
        '<option value="Barcode">Barcode</option>',
        '<option value="Description">Description</option>')

    $('#branch tbody tr').remove()
    $('#branch_wrapper').hide()
    $('#branch').hide()
    //$("#chkMaster").css('visibility', 'visible');
    $("#chkMaster").show();
    $("#Master").show();
    $('#txtSearch').val("");

    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Itemcode');
    mymodal.modal('show');
    $('#txtSearch').focus();

}
function getSearch() {
    if (_searchType == 'S') {
        $("#branch").css('visibility', 'visible');
        $("#dataTableItems").css('visibility', 'hidden');
        getSpecial();
        $("#branch").show();
        $("#branch_wrapper").show()
        $('#txtSearch').focus();

    }
    else if (_searchType == 'C') {
        $("#dataTableItems").css('visibility', 'visible');
        getCodes();
        $("#dataTableItems").show();
        $("#dataTableItems_wrapper").show()
        $('#txtSearch').focus();

    }
}
function getSpecial() {
    $('.loader-container').show();

    var SearchDetail = $('#txtSearch').val();
    var formData = {
        SpecialName: SearchDetail,
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=Specials";
    RequestUrl += "&Action=GetSpecial";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];

        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData = respone.dt;
        }

        $("#branch").css('visibility', 'visible');
        t = $("#branch").DataTable({
            paging: true,
            data: rowData,
            columns: [
                { 'data': 'special_name' },
                { 'data': 'start_date' },
                { 'data': 'end_date' },
            ],
            destroy: true

        });
    }, 'json');


}
function getCodes() {
    $('.loader-container').show();

    var SearchType = $('#cboSearch').val();
    var SearchText = $('#txtSearch').val();
    var Master = $("#chkMaster")[0].checked;;
    var formData = {
        SearchType: SearchType,
        SearchText: SearchText,
        Master: Master
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=Specials";
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

}
function getSpecialDetails(specialname) {

    $('.loader-container').show();

    var formData = {
        SpecialName: specialname.toUpperCase()
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=Specials";
    RequestUrl += "&Action=GetSelectedSpecialDetails";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showSpecialDetails, 'json');

}
function showSpecialDetails(SaveSpecial) {
    if (SaveSpecial.Success == true) {
        $('#txtSpecialName').val(SaveSpecial.SpecialName)
        txtStartDate.SetValue( new Date(SaveSpecial.StartDate))
        txtEndDate.SetDate(new Date(SaveSpecial.EndDate))
        SaveSpecial.IsActive == true ? $('#chkActive').prop("checked", true) : $('#chkActive').prop("checked", false)


        $('#txtPrice').val(SaveSpecial.Price)
        $("#size1 tbody tr").remove();

        $.each(SaveSpecial.ListData, function (index, row) {

            newItem = "<tr>" +
                "<td id='Mastercode' class='codes' >" + row.Mastercode + "</td>" +
                "<td id='Description' class='codes'>" + row.Description + "</td>" +
                "<td id='Quantity' class='codes'  >" + row.Quantity + "</td>" +
                "</tr>";

        });
        $("#size1 tbody").append(newItem);
    }
    else {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(SaveSpecial.Message);
        mymodal.find('.modal-title').text(SaveSpecial.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');
    }
    $('.loader-container').hide();

};
function addCode()
{
    if ($('#txtCode').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please enter a Stockcode");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }

    if ($('#txtQuantity').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please enter a quantity");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }


    var code = $('#txtCode').val().toUpperCase()
    var quantity = $('#txtQuantity').val()
    var alreadyExists;
    $('#size1 tbody tr').each(function () {
        var $firstCell = $(this).children('td').first(),
            firstCellContent = $firstCell.text();
        if (firstCellContent == code) {
            alreadyExists=true
            
        }
    });

    if (alreadyExists == true)
        {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("You cannot add an item to a special more than once.\ HINT: Use a quantity to add more than 1 of an item.");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false
    }

    $('.loader-container').show();

    var Stockcode = code
    var formData = {
        Stockcode
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=Specials";
    RequestUrl += "&Action=AddStock";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        $('.loader-container').hide();
        var rowData = [];
        if (respone.Success == true) {
            
               newItem = "<tr>" +
                "<td id='Mastercode' class='codes' >" + code + "</td>" +
                    "<td id='Description' class='codes'>" + respone.data + "</td>" +
                    "<td id='Quantity' class='codes' >" + quantity + "</td>" +
                    "</tr>";

            $("#size1 tbody").append(newItem);
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

}
function searchSpecial() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtSpecialName').val() == "") {
            specialPopup()
        }
        else {
            getSpecialDetails($('#txtSpecialName').val() )

        }
    }
}
function searchStockcode() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtQuantity').val() == "") {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text("Please enter the quantity");
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return
        }
        else {
            addCode()
               
            }

        }
    }
function save() {
    if ($('#txtSpecialName').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please enter a Special Name");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }
    if ($('#txtPrice').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please enter a Price for this Special");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }

    var startDate = txtStartDate.GetText()
    var endDate = txtEndDate.GetText()

    if (new Date(startDate) >= new Date(endDate)) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please enter a End Date which is after the Start Date");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }

    var tbody = $("#size1 tbody ")

    if (tbody.children().length == 0) {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text("Please select some items for this special");
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return
    }
    
    $('.loader-container').show();
    var json = '{'
    json += '"SpecialName":"' + $("#txtSpecialName").val() + '",';
    json += '"StartDate":"' + txtStartDate.GetText() + '",';
    json += '"EndDate":"' + txtEndDate.GetText() + '",';
    json += '"IsActive":"' + $("#chkActive").is(":checked") + '",';
    json += '"Price":"' + $("#txtPrice").val() + '",';
    json += '"listData": [';

    var $gridItemElement = $("#size1 tbody tr");
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
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=Specials";
    RequestUrl += "&Action=Save";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: json }, function (response) {
        $('.loader-container').hide();

        var result = response;
        var path;
        var LabelPath;
        if (result.Success == true) {
            clearForm();
        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');

    }, 'json');

}
function movenext() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtCode').val() == "") {
            codePopup()
        }
        else {
            $("#txtQuantity").focus();
            }

        }
    }
function clearForm() {
    $('#txtSpecialName').val("");
    $('#txtPrice').val("")
    $('#txtCode').val("")
    $('#txtQuantity').val("");
    $("#size1 tbody tr").remove();
    txtStartDate.SetDate()
    txtEndDate.SetDate()
    $('#chkActive').attr("checked", false)

}
function clearList() {
    $('#size1 tbody tr').remove();
}
function NumbersOnly() {
    debugger;
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    $(function () {
        $("#txtPrice").bind("keypress", function (e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            return ret;

        });
        $("#txtPrice").bind("paste", function (e) {
            e.preventDefault()
        });
        $("#txtPrice").bind("drop", function (e) {
            e.preventDefault()
        });
    });
}
function digits() {
    debugger;
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    $(function () {
        $("#txtQuantity").bind("keypress", function (e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            return ret;

        });
        $("#txtQuantity").bind("paste", function (e) {
            e.preventDefault()
        });
        $("#txtQuantity").bind("drop", function (e) {
            e.preventDefault()
        });
    });
}
function doSearch() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        getSearch()

    }
}



