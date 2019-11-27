<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POSLogin.aspx.vb" Inherits="pcm.Website.POSLogin" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--    <script type="text/javascript" src="../../js/General/Jquery-3.4.1.min.js"></script>--%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    <script src="../js/POS/Login.js"></script>
    <style>
        .logo {
            margin-left: 40px;
        }

        .login {
            margin: 50px 500px 0 500px;
        }
    </style>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="logo">
            <img id="logo" src="../Images/inner-logo.gif" />
        </div>
        <div class="panel panel-default login" style="border-color: cornflowerblue">
            <div class="panel-body">
                <div class="form-group col-md-12">
                    <label id="lblusername">Username</label>
                    <input type="text" id="txtUsername" runat="server" class="form-control text-uppercase" />
                </div>

                <div class="form-group col-md-12">
                    <label id="lblpassword">Password</label>
                    <input type="password" id="txtPassword" runat="server" class="form-control text-uppercase" />
                </div>
                <div class="col-md-12"></div>
                <div class="form-group col-md-12">
                    <label id="lbltillnumber">Till Number</label>
                    <input type="text" id="txtTillnumber" runat="server" class="form-control" onkeypress="IsNumeric(event)" />
                </div>
                <div class="col-md-12"></div>
                <div class="form-group col-md-12">
                    <label id="lblbranch">Branch</label>
                    <input type="text" id="txtBranch" runat="server" class="form-control text-uppercase" />
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-9 col-sm-10">
                        <asp:Button ID="btnlogin" runat="server" Text="LogIn" CssClass="btn btn-primary" OnClick="btnlogin_Click" OnClientClick="logIn(event)" />
                    </div>
                </div>
            </div>
        </div>

        <%--===================================Dev_LOADER=========================  --%>
        <dx:ASPxLoadingPanel ID="Loader" runat="server" ClientInstanceName="ld" Modal="true" ContainerElementID=""></dx:ASPxLoadingPanel>

        <%--===================================================DEV_POPUP==============================--%>
        <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
            HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
            DisappearAfter="1000" PopupAnimationType="Fade" HeaderStyle-BackColor="CornflowerBlue">
            <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                    <div>
                        <div id="Div2">
                            <dx:ASPxLabel ID="lblError" runat="server" ClientInstanceName="lblError"
                                Font-Size="16px">
                            </dx:ASPxLabel>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </form>
</body>
</html>
