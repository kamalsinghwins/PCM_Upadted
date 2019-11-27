<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="test.ascx.vb" Inherits="pcm.Website.test" %>
<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<dx:ASPxTextBox ID="txtZubi" runat="server" Width="170px">
</dx:ASPxTextBox>

<div  id='<% =HttpUtility.HtmlAttributeEncode(Me.GetSubElementId("container0"))%>' ></div>