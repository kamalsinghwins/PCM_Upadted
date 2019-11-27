<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Apply.aspx.vb" Inherits="pcm.Website.Apply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Apply</title>
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
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

     <!-- Main CSS -->

    <link href="../css/product_preview.css" rel="stylesheet" />

    <link href="../css/blitzer/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <!-- Responsive CSS -->
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/magnific-popup.css" rel="stylesheet" />
    <%--<script src="../js/General/jquery-2.0.3.min.js"></script>--%>
    <script src="../js/General/jquery-1.9.1.min.js"></script>
    <script src="../js/General/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/General/jquery.magnific-popup.min.js"></script>
    <script src="../js/General/jmasked_input.js"></script>
    <script>
        jQuery(function ($) {
            $("#txtContactNumber").mask("999-999-9999");
            $("#txtKids").mask("9");
            $("#txtIDNumber").mask("9999999999999");
        });
    </script>
      <script type="text/ecmascript">
          $(document).ready(function () {

              $("#SaveItmButton").show();
              $("#SaveItmButton").prop("disabled", false);
              //$("#hdCompleted").val("");
            

             $('#form1').unbind('submit');

              $('#form1').submit(function () {
                  if ($("#txtIDNumber").val() == "" || $("#txtIDNumber").val() == undefined
                      || $("#txtFirstName").val() == "" || $("#txtFirstName").val() == undefined
                      || $("#txtLastName").val() == "" || $("#txtLastName").val() == undefined
                      || $("#txtEmail").val() == "" || $("#txtEmail").val() == undefined
                      || $("#txtContactNumber").val() == "" || $("#txtContactNumber").val() == undefined) {
                      
                      $("#SaveItmButton").show();
                      $("#dialogHeader").text("Error");
                      $("#dialogInnerText").text("Please complete all fields.");
                      $('.popup-with-zoom-anim').magnificPopup('open');
                      return false;
                  }
                  else {

                  }
              });

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

              if ($("#hdCompleted").val() == "true") {
                  $("#SaveItmButton").hide();
                  $("#dialogHeader").text("Thanks");
                  $("#dialogInnerText").text("Thank you for giving us your details. We will be in touch with you soon.");
                  $('.popup-with-zoom-anim').magnificPopup('open');
              }

              if ($("#hdError").val() == "true") {
                  $("#SaveItmButton").hide();
                  $("#dialogHeader").text("Error");
                  $("#dialogInnerText").text("Something went wrong when we were trying to save your details. Please email us your details at: buyingdept@ragesa.co.za");
                  $('.popup-with-zoom-anim').magnificPopup('open');
              }

             
          });


          function ValidForm() {
              if ($("#txtIDNumber").val() == "" || $("#txtIDNumber").val() == undefined
                      || $("#txtFirstName").val() == "" || $("#txtFirstName").val() == undefined
                      || $("#txtLastName").val() == "" || $("#txtLastName").val() == undefined
                      || $("#txtEmail").val() == "" || $("#txtEmail").val() == undefined
                      || $("#txtContactNumber").val() == "" || $("#txtContactNumber").val() == undefined) {

                  $("#SaveItmButton").show();
                  $("#dialogHeader").text("Error");
                  $("#dialogInnerText").text("Please complete all fields.");
                  $('.popup-with-zoom-anim').magnificPopup('open');
                  return false;
              }
              else {
                  $("#SaveItmButton").hide();
              }
          }
    </script>

</head>
<body>
      <a class="popup-with-zoom-anim" href="#small-dialog" style="display:none" >Open with fade-zoom animation</a>
   

    
    <div id="small-dialog" class="zoom-anim-dialog mfp-hide">
        <h2 id="dialogHeader">No Styles</h2>
        <p id="dialogInnerText">Sorry you have no styles to rate.</p>
      </div>
    <form id="form1" onsubmit="return ValidForm()" runat="server">
        <asp:HiddenField ID="hdCompleted" runat="server" />
         <asp:HiddenField ID="hdError" runat="server" />
  <div class="column">
            <div class="logo">
                <img src="../Images/logo.png" />
            </div>
            <div class="main_content">
                <asp:Label ID="Label2" runat="server" Text="Applications are closed for now. Sorry."></asp:Label>
                <div class="content" style="margin-top:15px">

                    <div class="aligncenter" style="width:450px">
                      <table>
                          <tr>
                              <td>
                                  <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtFirstName" runat="server" Width="150px" MaxLength="30"></asp:TextBox></td>
                          </tr>
                           <tr>
                              <td>
                                  <asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtLastName" runat="server" Width="150px" MaxLength="30"></asp:TextBox></td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:Label ID="Label4" runat="server" Text="ID Number"></asp:Label>
                                

                              </td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtIDNumber" runat="server" Width="150px" MaxLength="13"></asp:TextBox>
                                  
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:Label ID="Label5" runat="server" Text="EMail Address"></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtEmail" runat="server" Width="150px" MaxLength="120"></asp:TextBox></td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:Label ID="Label6" runat="server" Text="Contact Number"></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtContactNumber" runat="server" Width="150px" MaxLength="12"></asp:TextBox></td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:Label ID="Label7" runat="server" Text="Province" ></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:DropDownList ID="cboProvince" runat="server" Width="162px" Font-Size="18px">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:Label ID="Label8" runat="server" Text="Number Of Kids"></asp:Label></td>
                              <td>&nbsp;</td>
                              <td>
                                  <asp:TextBox ID="txtKids" runat="server" Width="150px" MaxLength="2"></asp:TextBox></td>
                          </tr>
                           <tr>
                              <td>
                                  
                                  &nbsp;</td>
                              <td>&nbsp;</td>
                              <td>
                                    
                                                                    <div class="save_skip">
                                <asp:Button ID="SaveItmButton" ClientIDMode="Static" CssClass="saveItemButton" runat="server" Text="Save" ValidationGroup="save" Enabled="False" />
                           
                            </div>

                              </td>
                          </tr>

                      </table>
                    </div>
                    <div>
                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
