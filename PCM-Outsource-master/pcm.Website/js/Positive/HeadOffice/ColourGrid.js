var _searchType;
function searchFromStockcode() {
    _searchType="from"
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('StockCode');
    mymodal.modal('show');

}
function searchToStockcode() {
    _searchType = "to"
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('StockCode');
    mymodal.modal('show');

}
function searchStockCode() {
    
    $('.loader-container').show();
    var SearchDetail = $("#txtSearch").val();
    var formData = {
        SearchText: SearchDetail,
    };
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=ColourGrids";
    RequestUrl += "&Action=Search";
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
                { 'data': 'master_code' },
                { 'data': 'description' },
            ],
            destroy: true

        });
    }, 'json'); 

}
function showHide() {
    var type=$("#type").val()
    if (type == "Range") {
        $("#PartialOption").css("display", "none")
        $("#Range").css("display", "block")
        $("#stockcodelist").html('')
    }
    else if (type =="Partial") {
        $("#Range").css("display", "none")
        $("#PartialOption").css("display", "block")

    }
    else {
        $("#PartialOption").css("display","none")
        $("#Range").css("display", "none")
    }
}
function searchStockcode() {
    _searchType = "partial"
    var mymodal = $('#Searchbrit');
    mymodal.find('.modal-title').text('StockCode');
    mymodal.modal('show');
}
function remove() {
    $('#stockcodelist li.selected').remove();
}
function run() {
    var startdate = formatDate(txtFromDate.GetDate());
    var enddate = formatDate(txtToDate.GetDate());
    $('.loader-container').show();
    var json = {};
    var type = $("#type").val()

    if (type == "Range") {
        json.FromStockCode = $("#txtFromStockCode").val();
        json.ToStockCode = $("#txtToStockCode").val();
    } else if (type == "Partial") {
        var $gridItemElement = $("#stockcodelist li ");
        var paritalList = [];
        
        for (i = 0; i < $gridItemElement.length; i++) {
            paritalList.push($($gridItemElement[i]).text());
        }
        json.PartialList = paritalList;
    }

    var checked = $("#chkAll").prop('checked');
    var $branchList = $("#branchlist li" + (checked == false ? ".selected": "") )
    var branchList = [];

    $branchList.each(function (index, row) {
        branchList.push($(row).text());
    });

    json.BranchList = branchList;
    json.StartDate = startdate;
    json.EndDate = enddate;
    json.Type = type;
    json.AllBranches = checked;
    json.Username = Username;
    json.Email = Email;
    json.IsAdmin = IsAdmin;
    json.IPAddress = IPAddress;
    
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=ColourGrids";
    RequestUrl += "&Action=Save";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();
    $.post(RequestUrl, { FormData: JSON.stringify(json) }, function (response) {
        $('.loader-container').hide();
        var result = response;
        if (result.Success == true) {
            clearForm();

        }
        var mymodal = $('#messagePopup');
        mymodal.find('.modal-body').text(result.Message);
        mymodal.find('.modal-title').text(result.Success == true ? 'Success' : 'Error');
        mymodal.modal('show');

    }, 'json');
}
function getBranches() {
    $('.loader-container').show();
    RequestUrl = "/Ajax/AjaxServer.aspx?Page=ColourGrids";
    RequestUrl += "&Action=GetBranch";
    var stamp = new Date();
    RequestUrl += "&stamp=" + stamp.getTime();

    $.post(RequestUrl, {}, function (respone) {
        var rowData = [];
        $('.loader-container').hide();
       
            $.each(respone, function (index, row) {
                $('#branchlist').append('<li class="list">' + row.branch_code + "-" + row.branch_name + '</li>');
            });
       
      
    }, 'json');


}
function selectBranch() {
    var checked = $("#chkAll").prop('checked')
    if (checked == true) {
        $("#branchlist li").addClass('selected');
    }
    else {
        $("#branchlist li").removeClass('selected');
    }
}
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}
function clearForm() {
    $("#PartialOption").css("display", "none")
    $("#Range").css("display", "none")
    $("#branchlist li").removeClass('selected');
    $("#stockcodelist li").html("");
    $("#type").val("Full")
}
$(document).ready(function () {
    $("#PartialOption").css("display", "none")
    $("#Range").css("display", "none")
    $("#type").val("Full")
    getBranches()
});
$(document).on('click', '#branch tbody tr', function () {
    var data = t.row(this).data();
    if (_searchType == "from") {
        $('#txtFromStockCode').val(data.master_code)
    }
    else if (_searchType == "to") {
        $('#txtToStockCode').val(data.master_code)

    }
    else {
        var new_stockcode = data.master_code;
        //check stockcodes already exist
        //=========================================================================================

        var ids = $("#stockcodelist > li").map(function () {
            return this.textContent;
        }).get();
        if (ids.indexOf(new_stockcode) !== -1) {
            var mymodal = $('#messagePopup');
            mymodal.find('.modal-body').text("This stockcode is already added to the list");
            mymodal.find('.modal-title').text('Error');
            mymodal.modal('show');
            return false

        }
        //=========================================================================================
        $('#stockcodelist').append('<li class="list">' + new_stockcode + '</li>');
    }
    $('#Searchbrit').modal('hide');

});
$(document).on('click', '#stockcodelist li', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).addClass('selected');
    }
});
$(document).on('click', '#branchlist li', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).addClass('selected');
    }
});