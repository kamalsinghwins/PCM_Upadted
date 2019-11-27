<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RedeemVouchers.aspx.vb" Inherits="pcm.Website.RedeemVouchers" %>

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
    <a class="popup-with-zoom-anim" href="#small-dialog" style="display:none">Open with fade-zoom animation</a>

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
                        <div class="my_points">
                            <h3>My Points</h3>
                            <div class="left_content_para">
                                <p>Styles rated: <span id="stylesRated" runat="server"></span></p>
                                <p>Current points: <span id="pointsAwarded" runat="server"></span></p>
                            </div>
                        </div>
                    </div>
                    <div class="right_content floatright">
                       <dx:ASPxGridView ID="dxGrid" Width="98%" style="margin-left:10px" runat="server" AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding"
                                                KeyFieldName="id">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="CardNumber" FieldName="voucher_number" VisibleIndex="1">
                                                        <PropertiesTextEdit ClientInstanceName="CardNumber">
                                                        </PropertiesTextEdit>
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" EnableClientSideAPI="true" ClientInstanceName="CardNumber" OnInit="txtCardNumber_Init" Width="100%"
                                                                runat="server" Value='<%#Eval("voucher_number")%>' Border-BorderStyle="None">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Credit Remaining" FieldName="balance" VisibleIndex="3">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="txtBox" Width="100%" runat="server" OnInit="txtBalance_Init" Value='<%#Eval("balance")%>'
                                                                Border-BorderStyle="None">
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    
                                                </Columns>
                                                <SettingsPager Visible="False">
                                                </SettingsPager>
                                            </dx:ASPxGridView>
                        <br />
                       You can redeem points for a voucher once you have 20 points or more.
                        <br />
                        <div id="redeem" runat="server">
                        
                        </div>
                            <div class="generate_new_voucher">
                            <%--<asp:Button ID="cmdGenerateNewVoucher" ClientIDMode="Static" CssClass="saveItemButton" runat="server" Text="Generate New Voucher" />--%>
                            <asp:Button ID="cmdGenerateNewVoucher" CssClass="saveItemButton" runat="server" Text="Generate New Voucher" />
                            <%--   <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Save" CssClass="save_skip" CssPostfix="SaveButtons"></dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Skip" CssClass="save_skip"  CssPostfix="SaveButtons"></dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Exit" CssClass="save_skip"  CssPostfix="SaveButtons"></dx:ASPxButton>--%>
                            <%-- <button id="SaveButton" runat="server">Save</button>--%>
                            <%-- <button id="SkipButton" runat="server">Skip</button>
                                <button>Exit</button>--%>
                            <br />
                            <br />
                           * Please click on the Card Number to view and print the card
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
