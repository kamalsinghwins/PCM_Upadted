function NumericOnly(s, e) {
    if (e.htmlEvent.keyCode != 46 && e.htmlEvent.keyCode > 31
    && (e.htmlEvent.keyCode < 48 || e.htmlEvent.keyCode > 57))
        return _aspxPreventEvent(e.htmlEvent);

    return true;
}

function fadeOut(s, e) {
    var animationTime = 50;
    var opacity = 9;
    function func() {
        var popupHtmlElement = s.GetWindowElement(-1);
        popupHtmlElement.style.opacity = "0." + opacity;
        opacity--;
        if (opacity == -1) {
            window.clearInterval(fading);
            s.Hide();
            popupHtmlElement.style.opacity = "1";
        }
    }
    var fading = window.setInterval(func, animationTime);
}