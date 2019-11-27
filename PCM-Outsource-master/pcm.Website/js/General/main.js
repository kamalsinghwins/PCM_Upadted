
// jssor slider

jQuery(document).ready(function ($) {
    if ($("#noItemRated").val() == "true") {
      //  $('.popup-with-zoom-anim').magnificPopup('open');
    }
    else {
        var options = {
            $AutoPlay: false,                                    //[Optional] Whether to auto play, to enable slideshow, this option must be set to true, default value is false
            $AutoPlayInterval: 4000,                            //[Optional] Interval (in milliseconds) to go for next slide since the previous stopped if the slider is auto playing, default value is 3000
            $SlideDuration: 500,                                //[Optional] Specifies default duration (swipe) for slide in milliseconds, default value is 500
            $DragOrientation: 0,                                //[Optional] Orientation to drag slide, 0 no drag, 1 horizental, 2 vertical, 3 either, default value is 1 (Note that the $DragOrientation should be the same as $PlayOrientation when $DisplayPieces is greater than 1, or parking position is not 0)

        $ThumbnailNavigatorOptions: {
            $Class: $JssorThumbnailNavigator$,              //[Required] Class to create thumbnail navigator instance
            $ChanceToShow: 2,                               //[Required] 0 Never, 1 Mouse Over, 2 Always

            $SpacingX: 10,                                  //[Optional] Horizontal space between each thumbnail in pixel, default value is 0
            $SpacingY: 3,                                   //[Optional] Vertical space between each thumbnail in pixel, default value is 0
            $DisplayPieces: 6,                              //[Optional] Number of pieces to display, default value is 1
            $ParkingPosition: 204,                          //[Optional] The offset position to park thumbnail,

            $DirectionNavigatorOptions: {
                $Class: $JssorDirectionNavigator$,              //[Requried] Class to create direction navigator instance
                $ChanceToShow: 2,                               //[Required] 0 Never, 1 Mouse Over, 2 Always
                $AutoCenter: 2,                                 //[Optional] Auto center arrows in parent container, 0 No, 1 Horizontal, 2 Vertical, 3 Both, default value is 0
                $Steps: 3                                       //[Optional] Steps to go for each navigation request, default value is 1
            }
        }
    };

        var jssor_slider1 = new $JssorSlider$("slider1_container", options);
    }
        });