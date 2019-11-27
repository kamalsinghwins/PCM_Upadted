<%@ Page Language="VB" AutoEventWireup="false" Inherits="pcm.Website.collections_Default"
    CodeBehind="Default.aspx.vb" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>







<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rage Credit - Collections</title>
    <link href="../css/style.css?version=1" rel="stylesheet" type="text/css" />
    <link href="../css/errors.css" rel="stylesheet" type="text/css" />
    <script src="../js/Collections/application.js"></script>
    <script src="../js/General/json2.js"></script>
    <script src="../js/General/jquery-2.0.3.js"></script>
    <script src="../js/General/jquery-2.0.3.min.js"></script>
    <script src="../js/General/sliderAccess.js"></script>
    <script src="../js/General/jquery.nicescroll.min.js"></script>
    <script src="../js/General/jquery.uniform.min.js"></script>

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
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
        <div class="main" style="align-content: center">
            <div class="logo">
                <br />
                <div style="text-align: center">
                    <a href="http://www.ragesa.co.za">
                        <img src="../images/rage-logo.png" /></a>
                </div>
                <br />
                <table width="100px" style="float: left">
                    <tr>
                        <td></td>
                    </tr>
                </table>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Login" Theme="iOS"
                    Width="750px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <table class="float_left_l" style="width: 426px">
                                <tr>
                                    <td class="col1">&nbsp;
                                    </td>
                                    <td class="col2">
                                        <dx:ASPxLabel ID="lblUsername" runat="server" Text="Username" Theme="iOS">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="col3">
                                        <dx:ASPxTextBox runat="server" Width="170px" Theme="iOS" ID="txtUsername" MaxLength="13">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lblSurname" runat="server" Text="Password" Theme="iOS">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" Width="170px" Theme="iOS" ID="txtPassword" MaxLength="30"
                                            Password="True">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <dx:ASPxLabel ID="lblRememberMe" runat="server" Text="Remember Me" Theme="iOS">
                                        </dx:ASPxLabel>

                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkRememberMe" runat="server"></dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="col3" style="text-align: center">
                                        <dx:ASPxButton ID="cmdLogin" runat="server" Text="Login" Theme="iOS" ValidationGroup="SignUp"
                                            Width="174px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center" colspan="2">
                                       <p>For mobile version</p> <a class="link" href="/Mobile/dispatch_login.aspx"> click here</a>
                                    </td>

                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtUsername"
                                            runat="server" Display="Dynamic" ValidationGroup="SignUp" CssClass="validationerror"
                                            ErrorMessage="Username is required"></asp:RequiredFieldValidator><br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtPassword"
                                            runat="server" Display="Dynamic" ValidationGroup="SignUp" CssClass="validationerror"
                                            ErrorMessage="Password is required"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table class="float_left">
                                <tr>
                                    <td class="col4">
                                        <dx:ASPxCaptcha ID="dvCaptcha" runat="server" Theme="Office2010Blue" Width="213px"
                                            CharacterSet="123456789" CodeLength="4">
                                            <RefreshButton Visible="False">
                                            </RefreshButton>
                                            <TextBox Position="Bottom" />
                                        </dx:ASPxCaptcha>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col4">
                                        <asp:HiddenField ID="hdLogin" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                            </table>
                            <div id="errorpop_up" class="errorPopup popup" style="display: none">
                                <div id="errorpop_box">
                                    <a href="#" class="popupClose">
                                        <img src="../images/cross.jpg" alt="" /></a>
                                    <div id="errorPopupInnerData">
                                    </div>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                &nbsp;
            </div>
        </div>
    </form>
</body>
</html>
