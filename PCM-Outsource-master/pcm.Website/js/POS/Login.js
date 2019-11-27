$(document).ready(function () {

    $('#txtusername').focus();
});
function fadeOut() {
    dxPopUpError.Hide();
}
function logIn(event) {
    debugger;
    var mymodal = $('#myModal');
    var username = $('#txtusername').val();
    var password = $('#txtpassword').val();
    var tillnumber = $('#txttillnumber').val();
    var branch = $('#txtbranch').val();
    if (username == "") {
        lblError.SetText("Please Enter the Username")
        dxPopUpError.Show();
        event.preventDefault();
        return false;

    }
    if (password == "") {
        lblError.SetText("Please Enter the Password")
        dxPopUpError.Show();
        event.preventDefault();
        return false;
    }
    if (tillnumber == "") {
        lblError.SetText("Please Enter the Till Number")
        dxPopUpError.Show();
        event.preventDefault();
        return false;

    }
    if (branch == "") {
        lblError.SetText("Please Enter the Branch")
        dxPopUpError.Show();
        event.preventDefault();
        return false;
    }
    ld.Show();
}
function IsNumeric(event) {
    var keyCode = event.which ? event.which : event.keyCode
    if (((keyCode >= 48 && keyCode <= 57))) {
        return keyCode;
    }
    else {
        event.preventDefault();

    }
}
