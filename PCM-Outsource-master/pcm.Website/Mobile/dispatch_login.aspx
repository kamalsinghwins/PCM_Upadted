<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dispatch_login.aspx.vb" Inherits="pcm.Website.dispatch_login" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rage Credit - Collections</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!--Styles -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />

    <style>
        .chkfont label {
            font-weight: bold !important
        }

        .chkRemember {
            padding: 0px
        }

        .backColor {
            background-color: #e4ecf0;
        }
    </style>
    <!-- End of Styles -->

    <!--Scripts -->
    <script src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script src="../js/Collections/application.js"></script>
    <!-- End of Scripts -->
</head>
<script type="text/javascript">
    // <![CDATA[
    function OnIDValidation(s, e) {
        var txtIDNumber = e.value;
        if (txtIDNumber == null)
            return;
        if (txtIDNumber.length < 13)
            e.isValid = false;
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        PCMjs.initialize();
        if ($("#hdLogin").val() != "") {
            if ($("#hdLogin").val() == "true") {
                $(".hdLogin").show();
                PCMjs.attachpopUpEvents();
                $("#hdLogin").val("");
            }
            else {
                PCMjs.showErrorPopup("<p>The user details you entered are incorrect.</p>");
            }
        }
    });


</script>

<body>
    <div class="container backColor">
        <form id="form1" runat="server">
            <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
            <br />
            <div style="text-align: center">
                <a href="http://www.ragesa.co.za">
                    <img src="../images/rage-logo.png" /></a>
            </div>
            <br />
            <div class="row centered-form">
                <div class="col-xs-12 col-md-6 col-md-offset-3">
                    <asp:Label ID="lblerror" runat="server" Text="Label" Visible="False"></asp:Label>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Login</h3>
                        </div>

                        <div class="panel-body backColor">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label for="driver">Username</label>
                                        <dx:ASPxTextBox runat="server" CssClass="form-control" ID="txtUsername" MaxLength="13">
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="registration">Password</label>
                                        <dx:ASPxTextBox runat="server" CssClass="form-control" ID="txtPassword" Password="True" MaxLength="13">
                                        </dx:ASPxTextBox>

                                    </div>
                                    <div class="form-group">
                                        <dx:ASPxCheckBox CssClass="pull-left form-check-input checkbox chkfont chkRemember" Text="Remember Me" ID="chkRememberMe" runat="server">
                                        </dx:ASPxCheckBox>
                                        <dx:ASPxCaptcha CssClass="pull-right" ID="dvCaptcha" runat="server"
                                            CharacterSet="123456789" CodeLength="4">
                                            <RefreshButton Visible="False">
                                            </RefreshButton>
                                            <TextBox Position="Bottom" />
                                        </dx:ASPxCaptcha>
                                    </div>

                                    <div class="form-group">
                                        <dx:ASPxButton ID="cmdLogin" runat="server" Text="Login" CssClass="btn btn-info btn-block">
                                        </dx:ASPxButton>                                    
                                    </div>

                                      <div class="form-group">
                                           <p style="text-align :center">Go to main  <a class="link" href="/Intranet/Default.aspx"> Login</a> screen</p>
                                       </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdLogin" runat="server" ClientIDMode="Static" />
        </form>

    </div>
</body>
</html>

