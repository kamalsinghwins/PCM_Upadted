<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile/MobileIntranet.Master" CodeBehind="dispatch_stock.aspx.vb" Inherits="pcm.Website.dispatch_stock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.0/jquery.validate.min.js"></script>
    <link href="../../css/custom.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        .error {
            color: red;
        }

        .reset {
            padding: 0px 10px !important;
        }

        .Pointer {
            cursor: pointer;
        }

        .table-hover > tbody > tr:hover > td, .table-hover > tbody > tr:hover > th {
            background-color: initial;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml">
    <body>
        <br />
        <div class="container" id="main">
            <div class="loader-container">
                <div class="loader"></div>
            </div>
            <div class="row centered-form">
                <div class="col-xs-12 col-md-6 col-md-offset-3">
                    <asp:Label ID="lblerror" runat="server" Text="Label" Visible="False"></asp:Label>
                    <div class="panel panel-default">
                        <div class="panel-heading header">
                            <h3 class="panel-title">Dispatch Stock 
                                <input type="button" onclick="clear()" value="Reset" class="btn btn-sm btn-default pull-right reset" id="btnReset" name="reset" />
                            </h3>
                        </div>
                        <div class="panel-body">
                            <form id="dispatchForm">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <div class="form-group">
                                            <label for="driver">Driver</label>
                                            <input type="text" required="required" class="form-control" id="txtDriver" name="driver" />
                                        </div>
                                        <div class="form-group">
                                            <label for="registration">Registration</label>
                                            <input type="text" required="required" class="form-control" id="txtRegistration" name="registration" />
                                        </div>
                                        <div class="form-group">
                                            <label for="KM">KM</label>
                                            <input type="text" required="required" class="form-control" id="txtKM" name="KM" />
                                        </div>
                                        <div class="form-group">
                                            <label for="barcode">Barcode</label>
                                            <input type="text" onkeyup="searchBarcode()" class="form-control" id="txtBarcode" name="barcode" />
                                        </div>
                                        <div id="errMsg" class="alert alert-danger hide">
                                            <p>There is no item to process.</p>
                                        </div>
                                        <div class="form-group">
                                           <input type="button" onclick="getDetails()" value="Search" class="btn btn-info btn-block" id="btnSearch" name="search" />
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <table id="stock" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th>IBT Out</th>
                                                <th>Store Name</th>
                                                <th>Branch Code</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                    <div id="errs" class="alert alert-danger hide">
                                        <p>There is no item to process.</p>
                                    </div>
                                    <div>
                                        <label>Quantity : </label>
                                        <label id="count">0</label>
                                        <input type="button" onclick="remove()" value="Remove" class="btn btn-danger btn-xs pull-right " id="btnremove" name="remove" />
                                    </div>
                                </div>
                                <input type="button" onclick="showPopUp()" value="Dispatch" class="btn btn-info btn-block " id="btnDispatch" name="dispatch" />
                                <div id="messagePopup" class="modal fade" role="dialog">
                                    <div class="modal-dialog">
                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-header" style="background-color: #5bc0de; color: azure">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title"></h4>
                                            </div>
                                            <div class="modal-body" style="font-size: 20px;">
                                                <p>Are you sure you want to submit dispatch?</p>
                                            </div>
                                            <div class="modal-footer" style="margin-top: 0!important">
                                                <button type="button" onclick="dispatchStock()" class="btn btn-info">Yes</button>
                                                <button type="button" class="btn btn-danger" data-dismiss="modal">No</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>
            $(document).ready(function () {
                $('.loader-container').hide();
                $("#dispatchForm").validate({
                    rules: {
                        driver: {
                            required: true,
                        },

                        registration: {
                            required: true,
                        },

                        KM: {
                            required: true,
                        },

                        //barcode: {
                        //    required: true,
                        //},

                    },
                    messages: {
                        driver: "Please enter the driver",
                        registration: "Please enter registration number",
                        KM: "Please enter KM",
                        barcode: "Please enter the barcode"
                    }
                })
            });
            function getDetails() {

                if ($('.loader-container').is(':visible')) return;
                if ($("#txtBarcode").val() == "") {
                    $("#errMsg").removeClass("hide")
                    $("#errMsg").addClass("alert-danger")
                    $("#errMsg p").text("Please enter barcode")
                    var errorsound1 = new Audio("../Audio/error_1.wav");
                    errorsound1.play();
                    setTimeout(function () {
                        Remove();
                    }, 5000);
                    return
                }
                var newItem = "";
                var bool;
                var Barcode = $('#txtBarcode').val()

                //check barcodes already exist
                //=========================================================================================

                var ids = $("#stock > tbody > tr > td:first-child").map(function () {
                    return this.textContent;
                }).get();

                if (ids.indexOf(Barcode) !== -1) {
                    $("#errs").removeClass("hide")
                    $("#errs").addClass("alert-danger")
                    $("#errs p").text("This Barcode  " + Barcode + " has already been scanned")
                    var errorsound2 = new Audio("../Audio/error_2.wav");
                    errorsound2.play();
                    setTimeout(function () {
                        Remove();
                    }, 5000);
                    return false

                }

                //=========================================================================================

                $('.loader-container').show();
                $.ajax({

                    url: 'dispatch_stock.aspx/GetStockDetail',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    async: true,
                    data: '{IBTOutNumber:"' + Barcode + '"}',
                    success: function (data) {
                        $('.loader-container').hide();
                        if (data.d.Success == true) {
                            var dataItems = JSON.parse(data.d.dt)
                            $.each(dataItems, function (index, row) {
                                debugger;
                                newItem += "<tr class='dispatch Pointer'>" +
                                    "<td id='IBTOutNumber' >" + row.transaction_number + "</td>" +
                                    "<td id='BranchName'>" + row.branch_name + "</td>" +
                                    "<td id='BranchCode'>" + row.branch_code + "</td>" +
                                    "<td id='Address1'  class='hide'>" + row.address_line_1 + "</td>" +
                                    "<td id='Address2'  class='hide'>" + row.address_line_2 + "</td>" +
                                    "<td id='Address3'  class='hide'>" + row.address_line_3 + "</td>" +
                                    "<td id='Address4'  class='hide'>" + row.address_line_4 + "</td>" +
                                    "<td id='Address5'  class='hide'>" + row.address_line_5 + "</td>" +
                                    "</tr>";
                                $("#errMsg").addClass("hide")
                                $("#stock tbody").append(newItem);
                                bool = true;

                            });
                            count()
                            if (data.d.ErrorMessages != "" && data.d.ErrorMessages != null) {
                                if (bool == false) {
                                    $("#stock tbody").html("");

                                }
                                $("#errMsg").removeClass("hide")
                                $("#errMsg").addClass("alert-danger")
                                $("#errMsg p").text(data.d.ErrorMessages)
                                if (data.d.ErrorMessages.indexOf("not exist") !== -1) { 
                                 var errorsound5 = new Audio("../Audio/error_5.wav");
                                errorsound5.play();
                                }
                                if (data.d.ErrorMessages.indexOf('been dispatched')!== -1) { 
                                  var errorsound5 = new Audio("../Audio/error_6.wav");
                                errorsound5.play();
                                }

                                 if (data.d.ErrorMessages.indexOf('been blocked')!== -1) { 
                                 var errorsound5 = new Audio("../Audio/error_7.wav");
                                errorsound5.play();
                                    }
                            }
                            $("#txtBarcode").val("")
                            $("#txtBarcode").focus()
                        }
                        else {

                            $("#errMsg").removeClass("hide")
                            $("#errMsg").addClass("alert-danger")
                            $("#errMsg p").text("Nothing to process")
                            return
                        }
                        setTimeout(function () {
                            Remove();
                        }, 5000);
                    }
                });


            }
            function dispatchStock() {
                $('.loader-container').show();
                 var mymodal = $('#messagePopup');
                    mymodal.find('.modal-title').text('Confirmation');
                    mymodal.modal('hide');
                var json = {
                    "Driver": $("#txtDriver").val(),
                    "KM": $("#txtKM").val(),
                    "Registration": $("#txtRegistration").val(),
                    "listData": []
                };
                var $gridItemElement = $("#stock tbody tr.dispatch");
                $gridItemElement.each(function (index, row) {
                    let temp = {};
                    var $tds = $(this).find('td')
                    for (i = 0; i < $tds.length; i++) {
                        temp[$tds.eq(i).prop("id")] = $tds.eq(i).text();
                    }
                    json.listData.push(temp);
                });
                //Dispatching Stock
                //=========================================================================================
                $.ajax(
                    {
                        url: 'dispatch_stock.aspx/DispatchStock',
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: '{dispatchStockCode:' + JSON.stringify(json) + '}',
                        success: function (data) {
                            $('.loader-container').hide();
                            if (data.d.Success == true) {
                                var DeliveryNotesPath = data.d.DeliveryNotePath;
                                var DriverLogPath = data.d.DriverLogPath;
                                PrintDocument(DeliveryNotesPath)
                                PrintDocument(DriverLogPath)
                                clear()

                                $("#errs").removeClass("hide")
                                $("#errs").removeClass("alert-danger")
                                $("#errs").addClass("alert-success")
                                $("#errs p").text("Dispatch updated successfully")
                                var successsound = new Audio("../Audio/success_1.wav");
                                successsound.play();
                                setTimeout(function () {
                                    DeleteFile(DeliveryNotesPath, DriverLogPath);
                                }, 5000);
                            }
                            else {
                                $("#errs").removeClass("hide")
                                $("#errs").addClass("alert-danger")
                                $("#errs p").text("Failed")
                                var errorsound5 = new Audio("../Audio/error_3.wav");
                                errorsound5.play();
                            }
                        }


                    }
                );
                setTimeout(function () {
                    Remove();
                }, 5000);
                //=========================================================================================


            }
            $("#txtKM").keypress(function (event) {
                var key = event.which;
                if (!(key >= 48 && key <= 57))
                    event.preventDefault();
            });
            function clear() {
                $("#txtDriver").val("")
                $("#txtRegistration").val("")
                $("#txtKM").val("")
                $("#txtBarcode").val("")
                $("#stock tbody tr.dispatch").remove()
                $("#count").text("0")
            }
            function PrintDocument(url) {

                var top = (screen.availHeight - 600) / 2;
                var left = (screen.availWidth - 800) / 2;
                window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +
                    "scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);

            }
            function DeleteFile(DeliveryNotesPath, DriverLogPath) {
                var receipts = {
                    DeliveryNotesPath: DeliveryNotesPath,
                    DriverLogPath: DriverLogPath
                }
                $.ajax({
                    url: 'dispatch_stock.aspx/DeleteFiles',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: '{ receipts:' + JSON.stringify(receipts) + '}',
                    success: function (data) {

                    }
                });
            }
            function Remove() {
                $("#errs").addClass("hide")
                $("#errMsg").addClass("hide")


            }
            function searchBarcode() {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    getDetails();
                }
            }
            $("#btnReset").click(function () {
                clear()
            });
            function remove() {
                var $gridItemElement = $("#stock tr.Pointer");
                if ($gridItemElement.length == 0) {
                    $("#errs").removeClass("hide")
                    $("#errs").addClass("alert-danger")
                    $("#errs p").text("There are no items to remove")
                    setTimeout(function () {
                        Remove();
                    }, 5000);
                    return
                }

                if ($('#stock tr.selected').length == 0) {
                    $("#errs").removeClass("hide")
                    $("#errs").addClass("alert-danger")
                    $("#errs p").text("Please select the row to remove")
                    setTimeout(function () {
                        Remove();
                    }, 5000);
                    return
                }
                $('#stock tr.selected').remove();
                count()
            }
            $(document).on('click', '#stock tr.Pointer', function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                }
                else {
                    $('#tableItemsGrid tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }



            });
            function count() {
                var rowCount = $('#stock >tbody >tr').length;
                $("#count").text(rowCount)
            }
            function showPopUp() {
                if ($("#form").valid() == true) {
                    var $gridItemElement = $("#stock tbody tr.dispatch");
                    if ($gridItemElement.length == 0) {
                        $("#errs").removeClass("hide")
                        $("#errs").addClass("alert-danger")
                        $("#errs p").text("Nothing to process")
                        var errorsound = new Audio("../Audio/error_4.wav");
                        errorsound.play();
                        setTimeout(function () {
                            Remove();
                        }, 5000);
                        return
                    }
                    var mymodal = $('#messagePopup');
                    mymodal.find('.modal-title').text('Confirmation');
                    mymodal.modal('show');
                }
            }
        </script>
    </body>
    </html>
</asp:Content>
