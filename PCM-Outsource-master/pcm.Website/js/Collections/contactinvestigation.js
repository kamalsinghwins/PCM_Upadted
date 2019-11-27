
function OnContactNumberValidation(s, e) {
    var tmpText = cb.GetText();
    var contactnum = contactnumber.GetText();
  
    if (tmpText !== 'Unable to obtain valid contact number') {
        {
            if (contactnum.indexOf("_") !== -1)
                e.isValid = false;
        }


    }
}

function OnPTPValidation(s, e) {
    var tmpText = cb.GetText();
    var ptpamount = tp.GetText();
    var ptpdate = pd.GetText();

    if (tmpText == 'PTP') {
        {
            if (ptpamount == '')
                e.isValid = false;
        }


    }
}

function OnPTPDateValidation(s, e) {
    var tmpText = cb.GetText();
    var ptpdate = pd.GetText();

    if (tmpText == 'PTP') {

        {
            if (ptpdate == '0100-01-01')
                e.isValid = false;
        }
    }
}


function RecalculateCharsRemaining(editor) {
    var maxLength = parseInt(editor.maxLength ? editor.maxLength : editor.GetInputElement().maxLength);
    var editValue = editor.GetValue();
    var valueLength = editValue != null ? editValue.toString().length : 0;
    var charsRemaining = maxLength - valueLength;
    SetCharsRemainingValue(editor, charsRemaining >= 0 ? charsRemaining : 0);
}
function SetCharsRemainingValue(textEditor, charsRemaining) {
    var associatedLabel = ASPxClientControl.GetControlCollection().Get(textEditor.name + "_cr");
    var color = GetLabelColor(charsRemaining).toString();
    associatedLabel.SetText("<span style='color: " + color + ";'>" + charsRemaining.toString() + "</span>");
}
function GetLabelColor(charsRemaining) {
    if (charsRemaining < 5) return "red";
    if (charsRemaining < 12) return "#F3A250";
    return "green";
}

// ASPxMemo - MaxLength emulation
function InitMemoMaxLength(memo, maxLength) {
    memo.maxLength = maxLength;
}
function EnableMaxLengthMemoTimer(memo) {
    memo.maxLengthTimerID = window.setInterval(function () {
        var text = memo.GetText();
        if (text.length > memo.maxLength) {
            memo.SetText(text.substr(0, memo.maxLength));
            RecalculateCharsRemaining(memo);
        }
    }, 50);
}
function DisableMaxLengthMemoTimer(memo) {
    if (memo.maxLengthTimerID) {
        window.clearInterval(memo.maxLengthTimerID);
        delete memo.maxLengthTimerID;
    }
}
