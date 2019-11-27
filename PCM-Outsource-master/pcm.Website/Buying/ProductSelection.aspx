<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSelection.aspx.vb" Inherits="pcm.Website.ProductSelection" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style>
        /* Styles for dialog window */
        #small-dialog {
            background: white;
            padding: 20px 30px;
            text-align: left;
            max-width: 400px;
            margin: 40px auto;
            position: relative;
        }

        #small-dialog-question {
            background: white;
            padding: 20px 30px;
            text-align: left;
            max-width: 400px;
            margin: 40px auto;
            position: relative;
        }
    </style>
    <title>Rage - Buyers Rating</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Slider CSS -->
    <link href="../css/jssor.css" rel="stylesheet" />
    <!-- Main CSS -->
    <link href="../css/product_selection_style.css" rel="stylesheet" />

    <link href="../js/General/jRating.jquery.css" rel="stylesheet" />
    <link href="../css/blitzer/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <!-- Responsive CSS -->
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/magnific-popup.css" rel="stylesheet" />
    <%--<script src="../js/General/jquery-2.0.3.min.js"></script>--%>
    <script src="../js/General/jquery-1.9.1.min.js"></script>
    <script src="../js/General/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/General/jssor.core.js"></script>
    <script src="../js/General/jssor.utils.js"></script>
    <script src="../js/General/jssor.slider.js"></script>
    <script src="../js/General/main.js"></script>

    <script src="../js/General/jRating.jquery.min.js"></script>
    <script src="../js/General/jquery.magnific-popup.min.js"></script>

    <noscript>
        Javascript is disabled.
        <meta HTTP-EQUIV="REFRESH" content="0; url=www.myrage.co.za/buying/nojavascript.html" /> 
     </noscript>

    <script>
        setTimeout(function () {
            document.getElementById('<%=hdAllow.ClientID%>').value = "Allow";
            //$("#hdAllow").val() = "Allow"
            //alert('zubi');
            //your code to be executed after 1 seconds
        }, 5000)
    </script>

    <script type="text/ecmascript">
        $(document).ready(function () {

            $("#SaveItmButton").show();
            $("#hdAllow").val("NotAllow");
            $("#SaveItmButton").prop("disabled", false);
            $("#currentRating").val("");
            $("#currentPriceRange").val("");
            // simple jRating call
            $(".basic").jRating({
                canRateAgain: true,
                length: 5, // nb of stars
                rateMax: 5,
                nbRates: 10,
                bigStarsPath: '../images/stars.png',
                smallStarsPath: '../images/small.png',
                step: true,
                onClick: function (element, rate) {
                    $("#currentRating").val(rate);
                }
            });

            $(".pricerange").click(function () {
                $("#currentPriceRange").val($(this).val());
            });

            $(".clothing").click(function () {
                $("#currentClothing").val($(this).val());
            });

            $(".saveItemButtonPopUp").click(function () {
                $('.popup-with-zoom-anim').magnificPopup('close');
            });

            $('#form1').unbind('submit');

            $('#form1').submit(function () {
                if ($("#currentRating").val() == "" || $("#currentRating").val() == undefined
                    || $("#currentRating").val() == null || $("#currentPriceRange").val() == "" || $("#currentPriceRange").val() == undefined
                    || $("#currentPriceRange").val() == null) {
                    //alert("Please provide your rating and price suggestion.");
                    $("#dialogHeader").text("Please Rate");
                    $("#dialogInnerText").text("Please provide your rating and price suggestion.");
                    $('.popup-with-zoom-anim').magnificPopup('open');
                    return false;
                }
                else {

                }

                if ($("#hdAllow").val() == "" || $("#hdAllow").val() == undefined
                    || $("#hdAllow").val() == null) {
                    //alert("Please provide your rating and price suggestion.");
                    $("#dialogHeader").text("Please Wait");
                    $("#dialogInnerText").text("You cannot rank more than 1 style every 5 seconds.");
                    $("#SaveButton").prop('disabled', false);
                    $('.popup-with-zoom-anim').magnificPopup('open');
                    return false;
                }
                else {

                }
            });

            $('.popup-with-zoom-anim').magnificPopup({
                type: 'inline',

                fixedContentPos: false,
                fixedBgPos: true,

                overflowY: 'auto',

                closeBtnInside: true,
                preloader: false,

                midClick: true,
                removalDelay: 300,
                mainClass: 'my-mfp-zoom-in'
            });

            $('.popup-with-zoom-anim-1').magnificPopup({
                type: 'inline',

                fixedContentPos: false,
                fixedBgPos: true,

                overflowY: 'auto',

                closeBtnInside: true,
                preloader: false,

                midClick: true,
                removalDelay: 300,
                mainClass: 'my-mfp-zoom-in'
            });

            if ($("#ratingCompleted").val() == "true") {
                $("#SaveItmButton").hide();
                $("#dialogHeader").text("Thanks");
                $("#dialogInnerText").text("You have completed rating all the styles.Thanks for your time.");
                $('.popup-with-zoom-anim').magnificPopup('open');
            }

            if ($("#noItemRated").val() == "true") {
                $("#dialogHeader").text("No Styles");
                $("#dialogInnerText").text("Sorry you have no styles to rate.");
                $('.popup-with-zoom-anim').magnificPopup('open');
            }

            if ($("#viewClothing").val() == "") {
                $('.popup-with-zoom-anim-1').magnificPopup('open');
            }
        });


        function ValidForm() {
            if ($("#hdAllow").val() != "Allow") {
                //alert("Please provide your rating and price suggestion.");
                $("#dialogHeader").text("Please Wait");
                $("#dialogInnerText").text("You cannot rank more than 1 style every 3 seconds.");
                $("#SaveButton").prop('disabled', false);
                $('.popup-with-zoom-anim').magnificPopup('open');
                return false;
            }
            else {

            }

            if ($("#currentRating").val() == "" || $("#currentRating").val() == undefined
                  || $("#currentRating").val() == null || $("#currentPriceRange").val() == "" || $("#currentPriceRange").val() == undefined
                  || $("#currentPriceRange").val() == null) {
                $("#SaveButton").prop('disabled', false);
                //alert("Please provide your rating and price suggestion.");
                $("#dialog").dialog("open");
                return false;
            }
            else {
                $("#SaveItmButton").hide();
            }
        }
    </script>

</head>
<body>
    <a class="popup-with-zoom-anim" href="#small-dialog" style="display: none">Open with fade-zoom animation</a>
    <a class="popup-with-zoom-anim-1" href="#small-dialog-question" style="display: none">Open with fade-zoom animation</a>

    <div id="small-dialog" class="zoom-anim-dialog mfp-hide">
        <h2 id="dialogHeader">No Styles</h2>
        <p id="dialogInnerText">Sorry you have no styles to rate.</p>
    </div>
      <div id="small-dialog-question" class="zoom-anim-dialog mfp-hide">
          <div class="price_range">
        <h2>Would you like to rate clothing?</h2>
        <input type="radio" style="margin-left:15px;" class="clothing" name="clothing" value="Yes"><span>Yes</span>
              <br />
        <input type="radio" style="margin-left:15px;" class="clothing" name="clothing" value="No"><span>No</span>
              <br />
              <input id="cmdClothing" name="cmdClothing" class="saveItemButtonPopUp" type="button" value="Update" />
              </div>
          <br />
          <br />
         <%--<asp:Button ID="cmdClothing" ClientIDMode="Static" CssClass="saveItemButton" runat="server" Text="Update" />--%>
    </div>
    <form id="form1" onsubmit="return ValidForm()" runat="server">
        <asp:HiddenField ID="hdAllow" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="currentRateIndex" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdTimeDataServed" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="currentRating" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="currentPriceRange" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="ratingCompleted" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="noItemRated" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="viewClothing" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="currentClothing" ClientIDMode="Static" runat="server" />
        <div class="column">
            <div class="logo">
                <img src="../Images/logo.png" />
            </div>
            <div class="main_content">
                <div class="content">
                    <div class="left_content floatleft">
                        <div class="left_content_menu">
                            <ul>
                                <li><a href="ProductSelection.aspx">Rating</a></li>
                                <li><a href="RedeemVouchers.aspx">Redeem Vouchers</a></li>
                                 <li><a href="FAQs.aspx">FAQ</a></li>
                                <li><a href="mailto:buying_dept@ragesa.co.za">EMail Us</a></li>
                            </ul>
                        </div>
                        <div class="my_points">
                            <h3>My Points</h3>
                            <div class="left_content_para">
                                <p>Styles rated: <span id="stylesRated" runat="server"></span></p>
                                <p>Current points: <span id="pointsAwarded" runat="server"></span></p>
                            </div>
                        </div>
                    </div>
                    <div class="right_content floatright">
                        <div class="product_slider floatleft">
                            <!-- Jssor Slider Begin -->
                            <div id="slider1_container" style="position: relative; width: 320px; height: 407px;">

                                <!-- Loading Screen -->
                                <div u="loading" style="position: absolute; top: 0px; left: 0px;">
                                    <div style="filter: alpha(opacity=70); opacity: 0.7; position: absolute; display: block; background-color: #000; top: 0px; left: 0px; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: absolute; display: block; background: url(img/loading.gif) no-repeat center center; top: 0px; left: 0px; width: 100%; height: 100%;">
                                    </div>
                                </div>

                                <!-- Slides Container -->
                                <div u="slides" id="previewPanel" runat="server" style="cursor: move; position: absolute; left: 0px; top: 0px; width: 320px; height: 300px; overflow: hidden;">
                                </div>

                                <!-- Thumbnail Navigator Skin Begin -->
                                <div u="thumbnavigator" class="jssort05" style="position: absolute; width: 320px; height: 80px; left: 0px; bottom: 0px;">

                                    <div u="slides" style="cursor: move;">
                                        <div u="prototype" class="p" style="POSITION: absolute; WIDTH: 72px; HEIGHT: 72px; TOP: 0; LEFT: 0;">
                                            <div class="o" style="position: absolute; top: 1px; left: 1px; width: 72px; height: 72px; overflow: hidden;">
                                                <thumbnailtemplate class="b" style="width: 72px; height: 72px; border: none; position: absolute; TOP: 0; LEFT: 0;"></thumbnailtemplate>
                                                <div class="i"></div>
                                                <thumbnailtemplate class="f" style="width: 72px; height: 72px; border: none; position: absolute; TOP: 0; LEFT: 0;"></thumbnailtemplate>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Thumbnail Item Skin End -->

                                    <!-- Arrow Left -->
                                    <span u="arrowleft" class="jssord06l" style="width: 45px; height: 45px; top: 123px; left: -40px;"></span>
                                    <!-- Arrow Right -->
                                    <span u="arrowright" class="jssord06r" style="width: 45px; height: 45px; top: 123px; right: -40px;"></span>
                                    <!-- Direction Navigator Skin End -->
                                </div>
                                <!-- ThumbnailNavigator Skin End -->

                            </div>
                            <!-- Jssor Slider End -->
                        </div>
                        <div class="product_description_two floatright">
                            <div class="product_description">
                                <div class="pro_top">
                                    <h2 id="itemPrice" runat="server">The buying system is closed</h2>
                                    <div class="product_details">

                                        <p><span>Product Code: </span><span id="itemMaterial" runat="server"></span></p>
                                        <p id="itemDescription" runat="server"></p>
                                        <%-- <p>- Cuban heel.</p>
                                        <p>- Strappy design with buckle.</p>
                                        <p>- Almond toe.</p>--%>
                                        <h3 id="itemCode" runat="server">No new styles to rate :(</h3>
                                    </div>
                                    <div class="rating">
                                        <%--<div class="rating_star">
                                            <ul>
                                                <li><a href="">One</a></li>
                                                <li><a href="">two</a></li>
                                                <li><a href="">three</a></li>
                                                <li><a href="">four</a></li>
                                                <li><a href="">five</a></li>
                                            </ul>
                                        </div>--%>
                                        <div class="basic" data-average="0" data-id="1"></div>
                                        <p style="margin-top: 5px">Please rate this style out of 5 stars</p>
                                        <p style="margin-top: 5px">5 = you would <b>love to buy</b> this style</p>
                                        <p style="margin-top: 5px">1 = you would <b>never buy</b> this style</p>

                                    </div>
                                </div>
                                <div class="pro_bottom">
                                    <div class="price_range">
                                        <p>I thank that the price of this style is:</b></p>
                                        <br />
                                        <br />
                                        <input type="radio" class="pricerange" name="price" value="1"><span>Reasonable</span>
                                        <input type="radio" class="pricerange" name="price" value="2"><span>Expensive</span>
                                        <br />
                                        <br />
                                        Comments (Optional):<br />
                                        <asp:TextBox ID="txtComments" runat="server" Width="295px" Height="59px" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="save_skip">
                                <asp:Button ID="SaveItmButton" ClientIDMode="Static" CssClass="saveItemButton" runat="server" Text="Save" Enabled="False" />
                                <%--   <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Save" CssClass="save_skip" CssPostfix="SaveButtons"></dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Skip" CssClass="save_skip"  CssPostfix="SaveButtons"></dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Exit" CssClass="save_skip"  CssPostfix="SaveButtons"></dx:ASPxButton>--%>                                <%-- <button id="SaveButton" runat="server">Save</button>--%>                                <%-- <button id="SkipButton" runat="server">Skip</button>
                                <button>Exit</button>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
