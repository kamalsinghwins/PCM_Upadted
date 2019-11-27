var PCMjs =
{
    initialize: function () {



        PCMjs.attachpopUpEvents();
    },
    showPopUp: function (message) {
    },

    showErrorPopup: function (message) {
        $("#errorPopupInnerData").html(message);
        $(".errorPopup").show();
        PCMjs.attachpopUpEvents();
    },

    attachpopUpEvents: function () {
        $(".popupClose").unbind('click');
        $(".popupClose").click(function (event) {
            event.preventDefault();
            $(this).parents().closest(".popup").hide();
        });
    }
};