<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PTPsForToday.ascx.vb"
    Inherits="pcm.Website.PTPsForToday" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<table style="width: 150px">
    <tr>
        <td align="center">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Calls For Today" Font-Bold="true"
                Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="lblCalls" runat="server" Text="0" Font-Bold="true" Font-Size="X-Large">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td align="center">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="PTPs For Today" Font-Bold="true"
                Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="lblPTP" runat="server" Text="0" Font-Bold="true" Font-Size="X-Large">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td align="center">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Value of PTPs" Font-Bold="true"
                Font-Size="Medium">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="lblPTPAmount" runat="server" Text="0" Font-Bold="true" Font-Size="X-Large">
            </dx:ASPxLabel>
        </td>
    </tr>
</table>
<dx:ASPxTimer ID="ASPxTimer1" runat="server" Interval="300000">
</dx:ASPxTimer>
