<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="pcm.Website.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rage Questionnaire</title>
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <link href="../css/Main.css" rel="stylesheet" />
    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
</head>

<body>

    <div class="container">
        <div class="row centered-form">
            <div class="col-xs-12 col-md-6 col-md-offset-3">
                <asp:Label ID="lblerror" runat="server" Text="Label" Visible="False"></asp:Label>
                <div class="panel panel-default" >
                    <div class="panel-heading">
                        <h3 class="panel-title">Details for Questionnaire <span style="color:red" class="pull-right panel-title"><%= surveyname  %></span></h3>
                    </div>
                     <div class="panel-heading">
                        <div id="QuestionnaireHeading" runat="server"></div>
                    </div>
                    <div class="panel-body">
                        <form role="form" runat="server">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtFirstName" runat="server" class="form-control" placeholder="First Name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtFirstName" ForeColor="#CC0000">First name is required</asp:RequiredFieldValidator>
                                    </div>


                                    <div class="form-group">
                                        <asp:TextBox ID="txtLastname" runat="server" class="form-control" placeholder="Last Name">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtLastName" ForeColor="#CC0000">Last name is  required</asp:RequiredFieldValidator>
                                    </div>



                                    <div class="form-group">
                                        <asp:TextBox ID="txtIdNumber" runat="server" class="form-control" ondrop="return false;" onpaste="return false;">
                                            
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ValidatorID" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtIdNumber" ForeColor="#CC0000"><div id="divIDClock" runat="server"></div></asp:RequiredFieldValidator>
                                         <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="ID Number" onKeyPress="javascript:return isNumber(event)" ondrop="return false;" onpaste="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtIdNumber" ForeColor="#CC0000">ID is required</asp:RequiredFieldValidator>--%>
                                    </div>


                                    <div class="form-group">
                                        <asp:TextBox ID="txtContactNumber" runat="server" class="form-control" placeholder="Contact Number" onKeyPress="javascript:return isNumber(event)" ondrop="return false;" onpaste="return false;" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtContactNumber" ForeColor="#CC0000">Contact number is required</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnStart" runat="server" Text="Start" class="btn btn-info btn-block" />
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>

    <%-- <script type="text/javascript">
        var surveyName = '<%=surveyname %>';       
    </script>--%>
</html>
