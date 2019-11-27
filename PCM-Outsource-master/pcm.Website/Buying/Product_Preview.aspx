<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="product_preview.aspx.vb" Inherits="pcm.Website.product_preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Preview</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Slider CSS -->
    <link href="../css/jssor.css" rel="stylesheet" />
    <!-- Main CSS -->

    <link href="../css/product_preview.css" rel="stylesheet" />

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

</head>
<body>
    <form id="form1" runat="server">
        <div class="column">
            <div class="logo">
                <img src="../Images/logo.png" />
            </div>
            <div class="main_content">
                <div class="content">

                    <div class="right_content">
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
                                    <h2 id="itemPrice" runat="server">The Rating System is Closed</h2>
                                    <div class="product_details">
                                        <h3 id="itemCode" runat="server">No new styles to rate :(</h3>
                                        <p><span>Material: </span><span id="itemMaterial" runat="server"></span></p>
                                        <p id="itemDescription" runat="server"></p>
                                        <%-- <p>- Cuban heel.</p>
                                        <p>- Strappy design with buckle.</p>
                                        <p>- Almond toe.</p>--%>
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
                                    </div>
                                    <div class="pro_bottom">
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
