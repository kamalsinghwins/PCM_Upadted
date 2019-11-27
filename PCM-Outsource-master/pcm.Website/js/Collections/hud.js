//var prm = Sys.WebForms.PageRequestManager.getInstance();
//prm.add_initializeRequest(prm_InitializeRequest);
//prm.add_endRequest(prm_EndRequest);

//function prm_InitializeRequest(sender, args) {
//    dxLoading.Show();
//}
//function prm_EndRequest(sender, args) {
//    dxLoading.Hide();
//    btn.DoClick();
//}


// <![CDATA[
function OnCallResultsValidate(s, e) {
    var callresult = e.value;
    if (callresult == null)
        e.isValid = false;
    if (callresult === "")
        e.isValid = false;
}

// ]]>

// <![CDATA[

function OnInvestigationValidation(s, e) {
    var tmpText = cb.GetText();
    var tmpnotes = notes.GetText();
    
    if (tmpText == 'Under Investigation') {
        {
            if (tmpnotes == '')
                e.isValid = false;
                e.errorText = "You must fill in a note if you are marking an account as Under Investigation";
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

var DateDiff = {

    inDays: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },

    inWeeks: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },

    inMonths: function (d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();

        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },

    inYears: function (d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
}


function OnPTPDateValidation(s, e) {
    var tmpText = cb.GetText();
    var ptpdate = pd.GetText();

    var dString = ptpdate;
    var d1 = new Date();
    var d2 = new Date(dString);
    
    if (DateDiff.inDays(d1, d2) > 40) {
        e.errorText = "PTP Dates can be a maximum of 40 days in the future";
        e.isValid = false;
    };

    if (tmpText == 'PTP') {

        {
            if (ptpdate == '0100-01-01')
                e.isValid = false;
        }
    }
}

// ]]>

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
