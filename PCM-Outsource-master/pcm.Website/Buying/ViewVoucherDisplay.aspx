<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ViewVoucherDisplay.aspx.vb" Inherits="pcm.Website.ViewVoucherDisplay" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <dx:ReportToolbar ID="ReportToolbar1" runat='server' ShowDefaultButtons='False' 
            ReportViewerID="repStatementsViewer" Theme="iOS" Width="167px">
            <Items>
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft='3px' MarginRight='3px' />
<Margins MarginLeft="3px" MarginRight="3px"></Margins>
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        <dx:ReportViewer ID="repStatementsViewer" runat="server" Theme="iOS">
   
        </dx:ReportViewer>
        <dx:ASPxLabel ID="lblStatus" runat="server" Theme="iOS" Text="Please click on the Save image to create your Voucher">
        </dx:ASPxLabel>
    </div>
    <asp:HiddenField ID="hf" runat="server" />
   
    </form>
    
</body>
</html>
