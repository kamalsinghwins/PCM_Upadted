<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FAQs.aspx.vb" Inherits="pcm.Website.FAQs" %>

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

        .auto-style1 {
            text-indent: -18.0pt;
            line-height: 115%;
            font-size: 11.0pt;
            font-family: Calibri, sans-serif;
            margin-left: 36.0pt;
            margin-right: 0cm;
            margin-top: 0cm;
            margin-bottom: .0001pt;
        }

        .auto-style2 {
            text-indent: -18.0pt;
            line-height: 115%;
            font-size: 11.0pt;
            font-family: Calibri, sans-serif;
            margin-left: 36.0pt;
            margin-right: 0cm;
            margin-top: 0cm;
            margin-bottom: 10.0pt;
        }
    </style>
    <title>Rage - Redeem Vouchers</title>
    <link href="../css/product_selection_style.css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="../js/General/jquery-1.9.1.min.js"></script>
    <link href="../css/responsive.css" rel="stylesheet" />

    <link href="../css/magnific-popup.css" rel="stylesheet" />

    <script src="../js/General/jquery.magnific-popup.min.js"></script>

    <script type="text/JavaScript" language="JavaScript">
        function openwindow(s) {
            window.open("ViewVoucherDisplay.aspx?cn=" + s.GetValue(), "mywindow", "menubar=0,scrollbars=yes,resizable=1,width=600,height=400");
        }
    </script>

    <script type="text/ecmascript">
        $(document).ready(function () {


            $("#cmdGenerateNewVoucher").show();
            $("#cmdGenerateNewVoucher").prop("disabled", false);


            $('#form1').unbind('submit');

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

            if ($("#NotEnoughPoints").val() == "true") {
                $("#dialogHeader").text("Oops");
                $("#dialogInnerText").text("You have to have at least 20 points to redeem a voucher.");
                $('.popup-with-zoom-anim').magnificPopup('open');
            }


        });

        function ValidForm() {

        }
    </script>

</head>
<body>
    <a class="popup-with-zoom-anim" href="#small-dialog" style="display: none">Open with fade-zoom animation</a>

    <div id="small-dialog" class="zoom-anim-dialog mfp-hide">
        <h2 id="dialogHeader">No Styles</h2>
        <p id="dialogInnerText">Sorry you have no styles to rate.</p>
    </div>
    <form id="form1" runat="server">
        <asp:HiddenField ID="NotEnoughPoints" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="noItemRated" ClientIDMode="Static" runat="server" />
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
                      <%--  <div class="my_points">
                            <h3>My Points</h3>
                            <div class="left_content_para">
                            <p>Styles rated: <span id="stylesRated" runat="server"></span></p>
                                <p>Current points: <span id="pointsAwarded" runat="server"></span></p>
                            </div>
                        </div>--%>
                    </div>
                    <div class="right_content floatright">
                         <p>
                            <b>How do I vote?</b>
                        </p>
                        <p>
                            Select the amount of stars that you would like to rate the style on your screen. There is no right or wrong answer, an honest answer is always correct.<span> You will then need to select whether you think the price of the style is Reasonable or Expensive.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            <b>How do I save my votes?</b>
                        </p>
                        <p>
                            Once you have selected the amount of stars and the reason click on the “save” button and it will continue to the new item to vote. If it does this it was saved and you will see your points balance go up.<b></b>
                        </p>

                        <p>
                            <b>&nbsp;</b>
                        </p>

                        <p>
                            <b>How do I get points?</b>
                        </p>
                        <p>
                            For every style you rate, you get points. It does not matter what rating you gave the item.
                        </p>
                        <p>
                            <b>&nbsp;</b>
                        </p>
                        <p>
                            <b>When do I vote?</b>
                        </p>
                        <p>
                            You can login anytime and vote. It will save every time you vote.
                        </p>
                        <p>
                            <b>&nbsp;</b>
                        </p>
                        <p>
                            <b>When do I redeem my points?</b>
                        </p>
                        <p>
                            You can redeem at anytime. TIP: The more points you have, the bigger the voucher. So hang on to them. Your points will not expire
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            <b>How do I redeem my points?</b>
                        </p>
                        <p>
                            Select the 
                            Redeem Voucher button from the menu on the left. You can see the amount of points you have available in the &#39;My Points&#39; block. Select the “Generate New Voucher” button. It will convert your points into a voucher and give you a card number with the Rand value of the voucher. (<span id="stylesRated0" runat="server"><span id="pointsAwarded0" runat="server">You will see that your points will be deducted as you have converted it into a voucher.) </span></span>Click on the card number and a screen will pop up with a voucher and a number starting with 6502. Please print this voucher. </p>
                        <p>
                            <b>&nbsp;</b>
                        </p>
                        <p>
                            <b>What do I need to do have in order to redeem my points?</b>
                        </p>
                        <p>
                            Points don’t have to be converted to a physical card; you do not need any Rage card to load the points onto. The points will be allocated to a “card number” that you can print.
                        </p>
                        <p>
                            <b>&nbsp;</b>
                        </p>
                        <p>
                            <b>What do I do with a voucher once I have printed it?</b>
                        </p>
                        <p>
                            You can take the printed voucher to your favourite Rage to start shopping.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            <b>Do vouchers expire?</b>
                        </p>
                        <p>
                            Yes. Vouchers are valid for a year after you redeemed it.
                        </p>
                        <p>
                            <b>&nbsp;</b>
                        </p>
                        <p>
                            <b>Which voucher should I use first?</b>
                        </p>
                        <p>
                            If you have redeemed more than one voucher, it does not matter which one you use first, you can decide depending on your purchase. Another Tip: The bigger the voucher, the more you can buy and less you have to pay in. Please try and redeem the voucher when you have a substantial amount of points.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            <b>Which styles am I rating?</b>
                        </p>
                        <p>
                            We have selected a few exclusive Rage customers to rate our styles to see if there is a market for specific styles.&nbsp;
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            Some styles you see might currently be in the stores, depending on the season, and some might be a sneak peak of what is to launch in Rage stores soon.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            This all said, there might be styles on the system to rate that we are looking at ordering and might not order if the response is not great.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            This is why it is crucial to vote honestly.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            (Keep in mind that the styles you rate, might not be in the stores. A style could be sold out, not in the stores yet, or might not get ordered at all.
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            &nbsp;
                        </p>
                        <p>
                            <b>Voucher Terms and conditions</b>
                        </p>
                            <ul>
                        <li>- Only one voucher can be used per purchase. You can’t use more than one voucher on a single transaction at a Rage store.</li>
                        <li>- Voucher are valid to buy any item at a Rage store. If the voucher’s value is less than the item price, you can still buy the specific item. You will need to pay in the difference between the price of the item and your voucher.</li>
                        <li>- If the item you want to purchase costs less than the voucher value, you will not receieve a refund. The remaining value on the voucher can be used against your next purchase.</li>
                        <li>- You can only use one voucher. (i.e. There is no reason to print out mutiple copies of a voucher as all copies will have the same number and have the same amount of credit.</li>
                        <li>- Voucher is used in conjunction with cash purchases only. The difference can’t be loaded against a Rage account.</li>
                        <li>- Vouchers can’t be exchanged for cash.</li>
                        <li>- Vouchers can’t be used to pay an instalment on a Rage account.</li>
                        <li>- This Ranking / Voting system is promotional and Rage reserves the right to stop the promotion at any time. In such an event, all redeemed vouchers will not be affected and vouchers will continue to be valid. </li>
                        </ul>
                        <br />
                        
                           
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
