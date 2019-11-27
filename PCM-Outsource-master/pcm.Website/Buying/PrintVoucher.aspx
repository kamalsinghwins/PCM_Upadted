<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintVoucher.aspx.vb" Inherits="pcm.Website.PrintVoucher" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .pic_background {
            background: url("../images/gift_card_2.png") no-repeat scroll 0 0;
            display: block;
            height: 200px;
            margin-right: 5px;
            text-indent: -99999px;
            width: 400px;
            position: relative;
        }

        .inside_image {
            z-index: 1000;
            position: absolute;
            top: 97px;
            left: 518px;
            width: 271px;
        }

        #container {
            position: relative;
        }

        #inside_image {
            position: absolute;
            top: 127px;
            left: 85px;
            width: 142px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">

            <div class="inside_image">
                <img src="../images/gift_card_small.png" />

                <div id="inside_image" runat="server"></div>
               <%-- <asp:Label ID="lblNumber" runat="server" Text="0000 0000 0000 0000" Font-Size="30pt"></asp:Label></div>--%>

            <%--<img alt="" class="auto-style1" src="../Images/gift_card.png" /></div>--%>
        </div>
    </form>
</body>
</html>
