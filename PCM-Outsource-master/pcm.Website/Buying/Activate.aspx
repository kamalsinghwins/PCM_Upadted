﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Activate.aspx.vb" Inherits="pcm.Website.Activate" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password Reset</title>
    <script src="../js/General/application.js"></script>
    <style>
        .negativeBar {
            background-color: #E8E8E8;
        }

        .pwdBlankBar .positiveBar {
            width: 0%;
        }

        .pwdBlankBar .negativeBar {
            width: 100%;
        }

        .pwdWeakBar .positiveBar {
            background-color: Red;
            width: 30%;
        }

        .pwdWeakBar .negativeBar {
            width: 70%;
        }

        .pwdFairBar .positiveBar {
            background-color: #FFCC33;
            width: 65%;
        }

        .pwdFairBar .negativeBar {
            width: 35%;
        }

        .pwdStrengthBar .positiveBar {
            background-color: Green;
            width: 100%;
        }

        .pwdStrengthBar .negativeBar {
            width: 0%;
        }
    </style>
    <%-- <link href="../css/style.css" rel="stylesheet" />--%>
    <script>
        var minPwdLength = 5;
        var strongPwdLength = 6;

        function UpdateIndicator() {
            var strength = GetPasswordStrength(tbPassword.GetText());

            var className;
            var message;
            if (strength == -1) {
                className = 'pwdBlankBar';
                message = "Empty";
            } else if (strength == 0) {
                className = 'pwdBlankBar';
                message = "Too short";
            } else if (strength <= 0.2) {
                className = 'pwdWeakBar';
                message = "Weak";
            } else if (strength <= 0.6) {
                className = 'pwdFairBar';
                message = "Fair";
            } else {
                className = 'pwdStrengthBar';
                message = "Strong";
            }

            // update css and message
            var bar = document.getElementById("PasswordStrengthBar");
            bar.className = className;
            lbMessagePassword.SetValue(message);
        }
        function GetPasswordStrength(password) {
            if (password.length == 0) return -1;
            if (password.length < minPwdLength) return 0;

            var rate = 0;
            if (password.length >= strongPwdLength) rate++;
            if (password.match(/[0-9]/)) rate++;
            if (password.match(/[a-z]/)) rate++;
            if (password.match(/[A-Z]/)) rate++;
            if (password.match(/[!,@,#,$,%,^,&,*,?,_,~,\-,(,),\s,\[,\],+,=,\,,<,>,:,;]/)) rate++;
            return rate / 5;
        }

        function ConfirmedPassword_OnValidation(s, e) {
            if (tbPassword.GetValue() != tbConfirmedPassword.GetValue()) {
                e.isValid = false;
                e.errorText = "Passwords do not match";
            }
        }
    </script>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
        <div>
            <table align="center">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Reset Password" Theme="iOS">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <%--<table align="center">
                <tr>
                    <td>

                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="New Password" Theme="iOS">
                        </dx:ASPxLabel>

                    </td>
                    <td></td>
                    <td>

                        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Theme="iOS" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>

                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Repeat Password" Theme="iOS">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                    <td>

                        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Theme="iOS" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>

            </table>--%>
            <table align="center">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="lbPassword" runat="server" Text="New Password:" AssociatedControlID="tbPassword" Theme="iOS">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="tbPassword" runat="server" Width="200px" Password="True" ClientInstanceName="tbPassword" Theme="iOS">
                            <ClientSideEvents KeyUp="UpdateIndicator" TextChanged="UpdateIndicator" />
                            <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="Text"
                                ValidateOnLeave="False" Display="Dynamic">
                                <RegularExpression ValidationExpression=".{5,}" ErrorText="Must have at least 5 characters" />
                                <RequiredField IsRequired="True" ErrorText="Required field cannot be left empty" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                        
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbMessagePassword" ClientInstanceName="lbMessagePassword" runat="server"
                            Text="">
                        </dx:ASPxLabel>
                        <table id="PasswordStrengthBar" class="pwdBlankBar" border="0" cellspacing="0" cellpadding="0"
                            style="height: 4px; width: 220px">
                            <tbody>
                                <tr>
                                    <td id="PositiveBar" class="positiveBar"></td>
                                    <td id="NegativeBar" class="negativeBar"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="lbConfirmedPassword" runat="server" Text="Repeat Password:" AssociatedControlID="tbConfirmedPassword" Theme="iOS">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="tbConfirmedPassword" runat="server" Password="True" Width="200px"
                            OnValidation="tbConfirmedPassword_Validation" Theme="iOS">
                            <ClientSideEvents Validation="ConfirmedPassword_OnValidation" />
                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="True" ErrorTextPosition="Bottom"
                                ErrorDisplayMode="Text" ValidateOnLeave="False" Display="Dynamic">
                                <RequiredField IsRequired="True" ErrorText="Required field cannot be left empty" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="cmdSubmit" runat="server" Text="Submit" Theme="iOS">
                        </dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="There was an error updating this account. Please contact support."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                    <br />
                                    <br />

                                    <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Login" NavigateUrl="~/Buying/Default.aspx" Theme="iOS" >
                                    </dx:ASPxHyperLink>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
