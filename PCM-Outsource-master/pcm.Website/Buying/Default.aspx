<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="pcm.Website._Default1" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




















<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rage Buying Department</title>
    <link href="../css/style.css" rel="stylesheet" />
    <script type="text/javascript">
        // <![CDATA[

        function ShowMessage(message) {
            window.setTimeout("alert('" + message + "')", 0);
        }

        function onLogin(s, e) {
            e.processOnServer = false;

            if (!ASPxClientEdit.ValidateGroup("login")) return;

            hdDEWhichButton.Set("clicked", "Login");

            lp.Show();
            cab.PerformCallback();


        }


        function onEnd(s, e) {
            lp.Hide();

        }
    </script>

    <style type="text/css">
        .auto-style28 {
        }

        .auto-style29 {
            width: 26px;
        }
    </style>
     <script type="text/JavaScript" language="JavaScript">
         function openwindow() {
             window.open("terms_and_conditions.aspx", "mywindow", "menubar=0,scrollbars=yes,resizable=1,width=600,height=400");
         }
         </script>

</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">

        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
            ClientInstanceName="lp">
        </dx:ASPxLoadingPanel>
        <div class="main">
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
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Login" Theme="iOS" Width="750px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

                            <br />
                            <table class="float_left_l" style="width: 426px">
                                <tr>
                                    <td class="col1">&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="lblUsername" runat="server" Text="Email Address" Width="150px" Theme="iOS">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td class="col3">
                                        <dx:ASPxTextBox runat="server" ClientInstanceName="txtEmail" Width="170px" Theme="iOS" ID="txtEmail"
                                            MaxLength="120">
                                        </dx:ASPxTextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="lblSurname" runat="server" Text="Password" Width="170px" Theme="iOS">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxTextBox runat="server" ClientInstanceName="txtPassword" Width="170px" Theme="iOS" ID="txtPassword"
                                            MaxLength="30" Password="True">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="3">
                                        <table width="100%">
                                            <tr>
                                                <td class="auto-style28">I agree to the <a href="javascript: openwindow()">Terms and Conditions</a> for using this service
                                                </td>
                                                <td class="auto-style29">
                                                    <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Style="float: right">
                                                        <ClientSideEvents Validation="function(s, e) {


	                                                    if(!s.GetChecked())
	                                                    {
                                                            e.errorText = 'Required';
	                                                        e.isValid = false;
	   
	                                                        }
	    
	                                                    else
	                                                    {
	                                                        e.isValid = true;
	                                                        }
                                                    }" />
                                                        <ValidationSettings Display="Dynamic" ValidationGroup="login">
                                                        </ValidationSettings>
                                                        <InvalidStyle BackColor="#FFF5F5">
                                                            <Border BorderColor="Red" BorderStyle="Solid" BorderWidth="1px" />
                                                        </InvalidStyle>
                                                    </dx:ASPxCheckBox>




                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="auto-style28" style="text-align: right" colspan="2">
                                                    <dx:ASPxButton ID="cmdLogin" runat="server" Text="Login" Theme="iOS" ValidationGroup="login" Width="178px">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="auto-style28" colspan="2" style="text-align: left">
                                                     <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="I forgot my password" NavigateUrl="password_reset.aspx" Theme="RedWine" >
                                    </dx:ASPxHyperLink>
                                                
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEMail" CssClass="validationerror" Display="Dynamic" ErrorMessage="An Email Address is required" ValidationGroup="login"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" CssClass="validationerror" Display="Dynamic" ErrorMessage="Password is required" ValidationGroup="login"></asp:RequiredFieldValidator>
                                        <dx:ASPxLabel ID="lblStatus" runat="server" ForeColor="Red" Theme="iOS">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <table class="float_left">
                                <tr>
                                    <td class="col4">
                                        <dx:ASPxCaptcha ID="dvCaptcha" runat="server" Theme="RedWine"
                                            Width="213px" CharacterSet="123456789" CodeLength="4">
                                            <RefreshButton Visible="False">
                                            </RefreshButton>
                                            <TextBox Position="Bottom" />
                                            <ChallengeImage ForegroundColor="#000000">
                                            </ChallengeImage>
                                        </dx:ASPxCaptcha>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col4">&nbsp;</td>
                                </tr>

                            </table>
                            <dx:ASPxHiddenField ID="hdDEWhichButton" ClientInstanceName="hdDEWhichButton" runat="server">
                            </dx:ASPxHiddenField>
                        </dx:PanelContent>

                    </PanelCollection>

                </dx:ASPxRoundPanel>

                &nbsp;
            </div>
        </div>


    </form>
</body>
</html>
