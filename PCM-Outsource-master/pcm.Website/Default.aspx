<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="pcm.Website._Default" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rage Credit - View your latest statement online</title>
    <link href="css/style.css?version=1" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        .auto-style28 {
            float: left;
            width: 595px;
        }
        .auto-style29 {
            width: 25%;
        }
    </style>
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
<body>
    <form id="form1" runat="server">
        <div class="main" style="align-content: center">
            <div class="logo">
                <br />
                <div style="text-align: center">
                    <a href="http://www.ragesa.co.za">
                        <img src="images/rage-logo.png" /></a>
                </div>
                <br />
                <table width="100px" style="float:left">
                <tr>
                    <td></td>
                </tr>
            </table>

                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Login" Theme="iOS" Width="600px">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                        
                        <br />
                        <table class="auto-style28">
                            <tr>
                                <td class="col1">&nbsp;</td>
                                <td class="auto-style29">
                                    <dx:ASPxLabel ID="lblUsername" runat="server" Text="ID Number" Theme="iOS">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                                <td class="col3">
                                    <dx:ASPxTextBox runat="server" Width="170px" Theme="iOS" ID="txtIDNumber" 
                                        MaxLength="13">
                                        
                                        </dx:ASPxTextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td class="auto-style29">
                                    <dx:ASPxLabel ID="lblSurname" runat="server" Text="Surname" Theme="iOS">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <dx:ASPxTextBox runat="server" Width="170px" Theme="iOS" ID="txtSurname" 
                                        MaxLength="30"></dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td class="auto-style29">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    <dx:ASPxCaptcha ID="dvCaptcha" runat="server" Theme="Office2010Blue" Width="213px"
                                        CharacterSet="123456789" CodeLength="4">
                                        <RefreshButton Visible="False">
                                        </RefreshButton>
                                        <TextBox Position="Bottom" />
                                    </dx:ASPxCaptcha>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td class="auto-style29">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td> <dx:ASPxLabel ID="lblStatus" runat="server" ForeColor="Red" Theme="iOS">
                                    </dx:ASPxLabel></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td class="auto-style29">
                                    <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="images/help.png">
                                    </dx:ASPxImage>
                                </td>
                                <td>&nbsp;</td>
                                <td class="col3" style="text-align:center">
                                    <dx:ASPxButton ID="cmdLogin" runat="server" Text="Login" Theme="iOS" 
                                        Width="174px">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td colspan="3">
                                   
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        
                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="330px" Height="150px"
        MaxWidth="800px" MaxHeight="250px" MinHeight="150px" MinWidth="150px" ID="pcMain"
        ShowFooter="False" PopupElementID="imgQ" HeaderText="Help!"
        runat="server" PopupHorizontalAlign="RightSides" PopupVerticalAlign="Above" 
                            EnableHierarchyRecreation="True" Theme="RedWine">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:Panel ID="Panel1" runat="server">
                    <table border="0" cellpadding="4" cellspacing="0">

                        <tr>
                            <td valign="top" style="color: #666666; font-family: Tahoma; font-size: 14px;">
                                Please enter the ID Number and Surname associated with your account in order to view your latest Statement.<br />
                                <br />
                                If you are having trouble logging in, please call Rage Accounts on:<br /> 0681 
                                444 7249<br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <%--<ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />--%>
    </dx:ASPxPopupControl>
                    </dx:PanelContent>

                </PanelCollection>

            </dx:ASPxRoundPanel>
                
                &nbsp;
            </div>
        </div>
        <%--<div class="g-recaptcha" data-sitekey="6LfMGXQUAAAAAG-DRtZlSrnubQiP16tF-WKAtuM5"></div>--%>
    </form>
</body>
</html>
