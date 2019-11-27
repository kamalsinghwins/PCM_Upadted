<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CallsForToday.ascx.vb"
    Inherits="pcm.Website.CallsForToday" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<link href="../css/collections.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1
    {
        height: 33px;
    }
</style>
<table class="widget">
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Calls For Today:" Font-Bold="true"
                Font-Size="Small" Theme="iOS">
            </dx:ASPxLabel>
            &nbsp;
        </td>
        <td>
            <dx:ASPxLabel ID="lblCalls" runat="server" Text="0" Font-Bold="true" Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="PTPs For Today:" Font-Bold="true"
                Font-Size="Small" Theme="iOS">
            </dx:ASPxLabel>
            &nbsp;
        </td>
        <td>
            <dx:ASPxLabel ID="lblPTP" runat="server" Text="0" Font-Bold="true" Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Value of PTPs:" Font-Bold="true"
                Font-Size="Small" Theme="iOS">
            </dx:ASPxLabel>
            &nbsp;
            </td>
            <td>
            <dx:ASPxLabel ID="lblPTPAmount" runat="server" Text="0" Font-Bold="true" Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
</table>
<dx:ASPxTimer ID="ASPxTimer1" runat="server" Interval="300000">
</dx:ASPxTimer>
