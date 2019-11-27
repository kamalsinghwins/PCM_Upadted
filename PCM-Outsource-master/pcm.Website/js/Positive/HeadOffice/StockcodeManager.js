function OnKeyUpMarkup(s, e, key) {

    var str = salestax.GetInputElement().value
    if (str === '') {
        alert('Please select a Sales Tax Group');
        return;
    }

    var currentValue = s.GetValue();
    var priceValue = costprice.GetValue();
    var sellex = ASPxClientControl.GetControlCollection().GetByName("sellex" + key);
    var gp = ASPxClientControl.GetControlCollection().GetByName("gp" + key);
    var sellincl = ASPxClientControl.GetControlCollection().GetByName("sellincl" + key);

    var tsellex = parseFloat(priceValue * (1 + (currentValue / 100))).toFixed(2);
    sellex.SetText(tsellex);

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];

    var tsellincl = parseFloat(tsellex * (1 + taxpercentage / 100)).toFixed(2);
    sellincl.SetText(tsellincl);

    var tgp = parseFloat((tsellex - priceValue) / tsellex * 100).toFixed(2) + '%';
    gp.SetText(tgp);


}

function SetPCVisible(value) {
    var popupControl = GetPopupControl();
    if (value)
        popupControl.Show();
    else
        popupControl.Hide();
}
function SetImageState(value) {
    var img = document.getElementById('imgButton');
    var imgSrc = value ? 'Images/Buttons/pcButtonPress.gif' : 'Images/Buttons/pcButton.gif';
    img.src = imgSrc;
}
function GetPopupControl() {
    return ASPxPopupClientControl;
}


function OnKeyUpCostPrice() {

    var str = purchasetax.GetInputElement().value
    if (str === '') {
        alert('Please select a Purchase Tax Group');
        return;
    }

    var priceValue = costprice.GetValue();

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];

    var costincl = priceValue * (1 + taxpercentage / 100)

    var costincl = parseFloat(priceValue * (1 + taxpercentage / 100)).toFixed(2);
    costpriceincl.SetText(costincl);

    for (var i = 1; i < 4; i++) {
        var sellex = ASPxClientControl.GetControlCollection().GetByName("sellex" + i).GetValue();
        var markup = ASPxClientControl.GetControlCollection().GetByName("markup" + i);
        var gp = ASPxClientControl.GetControlCollection().GetByName("gp" + i);

        var tmarkup = parseFloat((sellex - priceValue) / priceValue * 100).toFixed(2);
        markup.SetText(tmarkup);

        var tgp = parseFloat((sellex - priceValue) / sellex * 100).toFixed(2) + '%';
        gp.SetText(tgp);

    }

}

function OnKeyUpCostPriceIncl() {

    var str = purchasetax.GetInputElement().value
    if (str === '') {
        alert('Please select a Purchase Tax Group');
        return;
    }

    var priceValue = costpriceincl.GetValue();

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];


    var costexc = parseFloat(priceValue / (1 + taxpercentage / 100)).toFixed(2);
    costprice.SetText(costexc);

    for (var i = 1; i < 4; i++) {
        var sellex = ASPxClientControl.GetControlCollection().GetByName("sellex" + i).GetValue();
        var markup = ASPxClientControl.GetControlCollection().GetByName("markup" + i);
        var gp = ASPxClientControl.GetControlCollection().GetByName("gp" + i);

        var tmarkup = parseFloat((sellex - priceValue) / priceValue * 100).toFixed(2);
        markup.SetText(tmarkup);

        var tgp = parseFloat((sellex - priceValue) / sellex * 100).toFixed(2) + '%';
        gp.SetText(tgp);

    }

}

function OnKeyUpSellEx(s, e, key) {

    var str = salestax.GetInputElement().value
    if (str === '') {
        alert('Please select a Sales Tax Group');
        return;
    }

    var currentValue = s.GetValue();
    var priceValue = costprice.GetValue();
    var markup = ASPxClientControl.GetControlCollection().GetByName("markup" + key);
    var gp = ASPxClientControl.GetControlCollection().GetByName("gp" + key);
    var sellincl = ASPxClientControl.GetControlCollection().GetByName("sellincl" + key);

    var tmarkup = parseFloat((currentValue - priceValue) / priceValue * 100).toFixed(2);
    markup.SetText(tmarkup);

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];

    var tsellincl = parseFloat(currentValue * (1 + taxpercentage / 100)).toFixed(2);
    sellincl.SetText(tsellincl);

    var tgp = parseFloat((currentValue - priceValue) / currentValue * 100).toFixed(2) + '%';
    gp.SetText(tgp);


}

function OnKeyUpSellIncl(s, e, key) {

    var str = salestax.GetInputElement().value
    if (str === '') {
        alert('Please select a Sales Tax Group');
        return;
    }

    var currentValue = s.GetValue();
    var priceValue = costprice.GetValue();
    var sellex = ASPxClientControl.GetControlCollection().GetByName("sellex" + key);
    var gp = ASPxClientControl.GetControlCollection().GetByName("gp" + key);
    var markup = ASPxClientControl.GetControlCollection().GetByName("markup" + key);

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];

    var tsellex = parseFloat(currentValue / (1 + taxpercentage / 100)).toFixed(2);
    sellex.SetText(tsellex);

    var tmarkup = parseFloat((tsellex - priceValue) / priceValue * 100).toFixed(2);
    markup.SetText(tmarkup);

    var tgp = parseFloat((tsellex - priceValue) / tsellex * 100).toFixed(2) + '%';
    gp.SetText(tgp);


}

function CalculateTotal(e_costprice, tmpmarkup, totalBox) {
    var total = e_costprice * tmpmarkup.GetValue();
    totalBox.SetText(total);

}
function OnValueChanged(s, e) {
    alert('value changed');
}

function CodeRegEx(s, e) {
    if (/[^A-Za-z0-9\-_.]/.test(String.fromCharCode(e.htmlEvent.keyCode))) {

        //fails test
        return _aspxPreventEvent(e.htmlEvent);
        return true;
    }

}

function BarcodeRegEx(s, e) {
    if (/[^A-Za-z0-9]/.test(String.fromCharCode(e.htmlEvent.keyCode))) {

        //fails test
        return _aspxPreventEvent(e.htmlEvent);
        return true;
    }

}

function DescriptionRegEx(s, e) {
    if (/[^A-Za-z0-9\-_. ]/.test(String.fromCharCode(e.htmlEvent.keyCode))) {

        //fails test
        return _aspxPreventEvent(e.htmlEvent);
        return true;
    }

}

function BarcodeRegEx(s, e) {
    if (/[^A-Za-z0-9]/.test(String.fromCharCode(e.htmlEvent.keyCode))) {

        //fails test
        return _aspxPreventEvent(e.htmlEvent);
        return true;
    }

}

function purchasetaxupdated() {

    var str = purchasetax.GetInputElement().value
    if (str === '') {
        alert('Please select a Purchase Tax Group');
        return;
    }

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];

    var priceValue = costprice.GetValue();
    var costinc = parseFloat(priceValue * (1 + taxpercentage / 100)).toFixed(2);
    costpriceincl.SetText(costinc);

}

function saletaxupdated() {


    var str = salestax.GetInputElement().value
    if (str === '') {
        alert('Please select a Sales Tax Group');
        return;
    }

    var n = str.split("(");
    var m = n[1].split(")");
    var taxpercentage = m[0];
    var priceValue = costprice.GetValue();

    for (var i = 1; i < 4; i++) {
        var sellex = ASPxClientControl.GetControlCollection().GetByName("sellex" + i).GetValue();
        var sellincl = ASPxClientControl.GetControlCollection().GetByName("sellincl" + i);

        var newsellprice = parseFloat(sellex * (1 + taxpercentage / 100)).toFixed(2);
        sellincl.SetText(newsellprice);

    }
}

function AddToSelected(s, e) {
    e.processOnServer = false;
    var tavailable = available.GetSelectedItem().value;
    selected.BeginUpdate();
    selected.AddItem(tavailable);
    selected.EndUpdate();

    //add colour to existing code
    var stock = stockcode.GetInputElement().readOnly;

    if (stock == true) {

        var hiddenvalue = hdClient.Get('colourstring');
        var n = tavailable.split(" - ");
        hdClient.Set('colourstring', hiddenvalue + ':' + n[0]);

    }

    available.RemoveItem(available.GetSelectedIndex());

    //Sort the list alphabetically
    var srcItems = new Array();
    for (var i = 0; i < selected.GetItemCount() ; i++) {
        srcItems[i] = selected.GetItem(i)

    }

    //Sort the array
    srcItems = srcItems.sort(function (a, b) {
        if (a.text < b.text) return -1;
        if (a.text == b.text) return 0;
        return 1;
    });

    //Clear the listbox
    selected.ClearItems();

    //repopulate the listbox
    for (var i = 0; i < srcItems.length; i++) {

        selected.AddItem(srcItems[i].text, srcItems[i].value);

    }
}

function AddToSelectedDbl(s, e) {
    //e.processOnServer = false;

    var tavailable = available.GetSelectedItem().value;
    selected.BeginUpdate();
    selected.AddItem(tavailable);
    selected.EndUpdate();

    //add colour to existing code
    var stock = stockcode.GetInputElement().readOnly;

    if (stock == true) {
        
        var hiddenvalue = hdClient.Get('colourstring');
        var n = tavailable.split(" - ");
        hdClient.Set('colourstring', hiddenvalue + ':' + n[0]);
        
    }
    
    available.RemoveItem(available.GetSelectedIndex());

    //Sort the list alphabetically
    var srcItems = new Array();
    for (var i = 0; i < selected.GetItemCount() ; i++) {
        srcItems[i] = selected.GetItem(i)

    }

    //Sort the array
    srcItems = srcItems.sort(function (a, b) {
        if (a.text < b.text) return -1;
        if (a.text == b.text) return 0;
        return 1;
    });

    //Clear the listbox
    selected.ClearItems();

    //repopulate the listbox
    for (var i = 0; i < srcItems.length; i++) {

        selected.AddItem(srcItems[i].text, srcItems[i].value);

    }
}

function AddToAvailable(s, e) {
    e.processOnServer = false;

    var stock = stockcode.GetInputElement().readOnly;
    
    if (stock == true)
    {
        alert('You cannot remove colours on an existing code');
        return;
    }

    var tselected = selected.GetSelectedItem().value;
    available.BeginUpdate();
    available.AddItem(tselected);
    available.EndUpdate();

    //removed.BeginUpdate();
    //removed.AddItem(tselected);
    //removed.EndUpdate();

    selected.RemoveItem(selected.GetSelectedIndex());

    //Sort the list alphabetically
    var srcItems = new Array();
    for (var i = 0; i < available.GetItemCount() ; i++) {
        srcItems[i] = available.GetItem(i)

    }

    //Sort the array
    srcItems = srcItems.sort(function (a, b) {
        if (a.text < b.text) return -1;
        if (a.text == b.text) return 0;
        return 1;
    });

    //Clear the listbox
    available.ClearItems();

    //repopulate the listbox
    for (var i = 0; i < srcItems.length; i++) {

        available.AddItem(srcItems[i].text, srcItems[i].value);

    }
}

function AddToAvailableDbl(s, e) {
    //e.processOnServer = false;

    var stock = stockcode.GetInputElement().readOnly;

    if (stock == true) {
        alert('You cannot remove colours on an existing code');
        return;
    }

    var tselected = selected.GetSelectedItem().value;
    available.BeginUpdate();
    available.AddItem(tselected);
    available.EndUpdate();

    //removed.BeginUpdate();
    //removed.AddItem(tselected);
    //removed.EndUpdate();

    selected.RemoveItem(selected.GetSelectedIndex());

    //Sort the list alphabetically
    var srcItems = new Array();
    for (var i = 0; i < available.GetItemCount() ; i++) {
        srcItems[i] = available.GetItem(i)

    }

    //Sort the array
    srcItems = srcItems.sort(function (a, b) {
        if (a.text < b.text) return -1;
        if (a.text == b.text) return 0;
        return 1;
    });

    //Clear the listbox
    available.ClearItems();

    //repopulate the listbox
    for (var i = 0; i < srcItems.length; i++) {

        available.AddItem(srcItems[i].text, srcItems[i].value);

    }
}

