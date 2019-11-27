<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile/MobileIntranet.Master" CodeBehind="mobile_welcome.aspx.vb" Inherits="pcm.Website.mobile_welcome" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <br />
    <div class="container">
    <div class = "panel panel-default" style="background-color: #dae5f2;width:100%">
         <div style="width:100%" class = "panel-heading header">
             Welcome
   </div>
        <div class = "panel-body">
                <img src="../Images/positive_live_logo.png" />
                       <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Please select an item from the menu to begin." ForeColor="#3366CC" Font-Size="14px">
                </dx:ASPxLabel>
            </div>
</div></div>
</asp:Content>
