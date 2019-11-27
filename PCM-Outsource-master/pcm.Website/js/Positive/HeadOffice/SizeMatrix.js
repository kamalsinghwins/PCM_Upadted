function searchGrid(){
   
    $('#cboSearch').html('');
    $('#cboSearch').append('<option value="Grid Number">Grid Number</option>',
        '<option value="Grid Description">Grid Description</option>')
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('Size Grid');
    mymodal.modal('show');


}
function getGrid() {
    $('.loader-container').show();

    var SearchType = $('#cboSearch').val();
    var SearchDetail = $('#txtSearch').val();
    var formData = {
        SearchType: SearchType,
        SearchDetail: SearchDetail,
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=SizeMatrix";
    RequestUrl += "&Action=GetGrid";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, function (respone) {
        var rowData = [];
        $('.loader-container').hide();
        if (respone.Success == true) {
            rowData = respone.searchGridList;
        }

        $("#grid").css('visibility', 'visible');
        t = $("#grid").DataTable({
            paging: true,
            data: rowData,
            columns: [
                { 'data': 'grid_number' },
                { 'data': 'grid_description' },
            ],
            destroy: true

        });
    }, 'json');


}
$(document).on('click', '#grid tbody tr', function () {
    var data = t.row(this).data();
    $('#txtGridNumber').val(data.grid_number)
    $('#txtDescription').val(data.grid_description)
    getGridDetails();
    $('#Searchbrit').modal('hide');

});
function getGridDetails() {

    $('.loader-container').show();
    var formData = {
        GridNumber: $('#txtGridNumber').val()
    };

    RequestUrl = "/Ajax/AjaxServer.aspx?Page=SizeMatrix";
    RequestUrl += "&Action=GetGridDetails";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $("#dataTableItems").css('visibility', 'visible');
    $.post(RequestUrl, { FormData: JSON.stringify(formData) }, showGridDetails, 'json');

}
function showGridDetails(searchGridList) {
    if (searchGridList.Success == true) {
        $.each(searchGridList.searchGridList, function (index, row) {
            $('#txtDescription').val(row.grid_description)

            newItem = "<tr>" +
                "<td class='codes' >" + row.s1 + "</td>" +
                $('#frmG_Grid #size1 tbody tr:first').find('.s1').val(row.s1);
            $('#frmG_Grid #size1 tbody tr:first').find('.s2').val(row.s2);
            $('#frmG_Grid #size1 tbody tr:first').find('.s3').val(row.s3);
            $('#frmG_Grid #size1 tbody tr:first').find('.s4').val(row.s4);
            $('#frmG_Grid #size1 tbody tr:first').find('.s5').val(row.s5);
            $('#frmG_Grid #size1 tbody tr:first').find('.s6').val(row.s6);
            $('#frmG_Grid #size1 tbody tr:first').find('.s7').val(row.s7);
            $('#frmG_Grid #size1 tbody tr:first').find('.s8').val(row.s8);
            $('#frmG_Grid #size1 tbody tr:first').find('.s9').val(row.s9);
            $('#frmG_Grid #size1 tbody tr:first').find('.s10').val(row.s10);
            $('#frmG_Grid #size2 tbody tr:first').find('.s11').val(row.s11);
            $('#frmG_Grid #size2 tbody tr:first').find('.s12').val(row.s12);
            $('#frmG_Grid #size2 tbody tr:first').find('.s13').val(row.s13);
            $('#frmG_Grid #size2 tbody tr:first').find('.s14').val(row.s14);
            $('#frmG_Grid #size2 tbody tr:first').find('.s15').val(row.s15);
            $('#frmG_Grid #size2 tbody tr:first').find('.s16').val(row.s16);
            $('#frmG_Grid #size2 tbody tr:first').find('.s17').val(row.s17);
            $('#frmG_Grid #size2 tbody tr:first').find('.s18').val(row.s18);
            $('#frmG_Grid #size2 tbody tr:first').find('.s19').val(row.s19);
            $('#frmG_Grid #size2 tbody tr:first').find('.s20').val(row.s20);
            $('#frmG_Grid #size3 tbody tr:first').find('.s21').val(row.s21);
            $('#frmG_Grid #size3 tbody tr:first').find('.s22').val(row.s22);
            $('#frmG_Grid #size3 tbody tr:first').find('.s23').val(row.s23);
            $('#frmG_Grid #size3 tbody tr:first').find('.s24').val(row.s24);
            $('#frmG_Grid #size3 tbody tr:first').find('.s25').val(row.s25);
            $('#frmG_Grid #size3 tbody tr:first').find('.s26').val(row.s26);
            $('#frmG_Grid #size3 tbody tr:first').find('.s27').val(row.s27);
            $('#frmG_Grid #size3 tbody tr:first').find('.s28').val(row.s28);
            $('#frmG_Grid #size3 tbody tr:first').find('.s29').val(row.s29);
            $('#frmG_Grid #size3 tbody tr:first').find('.s30').val(row.s30);

        });
        $('#txtDescription').focus();
    }
    else {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(searchGridList.Message);
        mymodal.find('.modal-title').text(searchGridList.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');
    }
    $('.loader-container').hide();

};
function clear() {
    
    $("#txtGridNumber").val('')
    $("#txtDescription").val('')
    $('#size1').find('input[type=text]').each(function () {
        $(this).val("");
    });
    $('#size2').find('input[type=text]').each(function () {
        $(this).val("");
    });
    $('#size3').find('input[type=text]').each(function () {
        $(this).val("");
    });
}
function save() {
    if ($('#txtGridNumber').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('Please input a Grid Number.');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return;

    }

    if ($('#txtDescription').val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('Please input a Grid Description.');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return;

    }

    var $tmpGridItemElement;
    $tmpGridItemElement = $("#size1 .s1");
    if ($tmpGridItemElement.val() == "") {
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text('You input a size into Size 1');
        mymodal.find('.modal-title').text('Error');
        mymodal.modal('show');
        return false;
    }
  
    $('.loader-container').show();

    var json = '{'
    json += '"GridNumber":"' + $("#txtGridNumber").val() + '",';
    json += '"Description":"' + $("#txtDescription").val() + '",';    

    var $gridItemElement = $("#size1 tbody tr ");
    $gridItemElement.each(function (index, row) {
        var $tds = $(this).find('td')
        for (i = 0; i < $tds.length; i++) {
            json += '"' + $tds.eq(i).find('input').prop("id") + '":"' + $tds.eq(i).find('input').val() + '"';
            if (i < $tds.length - 1)
                json += ','
        }
    });

     $gridItemElement = $("#size2 tbody tr");
    $gridItemElement.each(function (index, row) {
        json += ','
        var $tds = $(this).find('td')
        for (i = 0; i < $tds.length; i++) {
            json += '"' + $tds.eq(i).find('input').prop("id") + '":"' + $tds.eq(i).find('input').val() + '"';
            if (i < $tds.length - 1)
                json += ','
        }
    });

     $gridItemElement = $("#size3  tbody tr");
    $gridItemElement.each(function (index, row) {
        json += ','

        var $tds = $(this).find('td')
        for (i = 0; i < $tds.length; i++) {
            json += '"' + $tds.eq(i).find('input').prop("id") + '":"' + $tds.eq(i).find('input').val() + '"';
            if (i < $tds.length - 1)
                json += ','
        }
        json += '}' 

    });

                RequestUrl = "/Ajax/AjaxServer.aspx?Page=SizeMatrix";
            RequestUrl += "&Action=Save";
            var stamp = new Date();
            RequestUrl += "&stamp=" + stamp.getTime();
            $.post(RequestUrl, { FormData: json }, function (response) {
                $('.loader-container').hide();
                debugger;
                var result = response;
                if (result.Success == true) {
                    clear();

                }
                var mymodal = $('#messagePopup');
                mymodal.find('.modal-body').text(result.Message);
                mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
                mymodal.modal('show');

            }, 'json');
        
}
function getCode() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtGridNumber').val() == "") {
            searchGrid()
        }
        else {
            getGridDetails()

        }
    }
}
function search() {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        if ($('#txtSearch').val() != "") {
            getGrid()
        }
        
    }
}
