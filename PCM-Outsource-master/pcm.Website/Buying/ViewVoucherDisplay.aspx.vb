Imports System
Imports System.Data
Imports DevExpress.XtraReports.UI
Imports System.Drawing.Printing
Imports pcm.Website.UTIL
Imports DevExpress.Web

Public Class ViewVoucherDisplay
    Inherits System.Web.UI.Page

    Dim UTIL As New clsUtil
    Dim HasData As Boolean = False
    Dim tmpGuID As String
    Dim r As New ViewVoucher

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim cardnumber As String = Request.QueryString("cn")
        Session("card_number") = cardnumber

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If cardnumber = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Buying/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Buying/Default.aspx")
                End If
            End If

            If Session("card_number") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Buying/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Buying/Default.aspx")
                End If
            End If
        Else
            If Session("card_number") = "" Then
                Session("card_number") = "6502999999999999"
            End If
        End If

        AddHandler r.DataSourceDemanded, AddressOf Report_DataSourceDemanded

        'repStatementsViewer.Report = r

    End Sub

    Private Sub Report_DataSourceDemanded(sender As Object, e As System.EventArgs)
        GenerateXML()

        Dim report As XtraReport = CType(sender, XtraReport)
        report.XmlDataPath = HttpContext.Current.Server.MapPath("~\data\" & Session("card_number") & ".XML")

        'report.ExportToPdf("Rage_Gift_Voucher.pdf")

        'repStatementsViewer.Report = report
        'repStatementsViewer.DataBind()

        System.IO.File.Delete(HttpContext.Current.Server.MapPath("~\data\" & Session("card_number") & ".XML"))


    End Sub

    Private Sub GenerateXML()

        Dim _StrmWriter As IO.StreamWriter
        Dim UTIL As New clsUtil

        _StrmWriter = New IO.StreamWriter(HttpContext.Current.Server.MapPath("~\data\" & Session("card_number") & ".XML"))

        _StrmWriter.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
        _StrmWriter.WriteLine("<MainRoot>")
        _StrmWriter.WriteLine("<CardNumber>" & Session("card_number") & "</CardNumber>")
        _StrmWriter.WriteLine("</MainRoot>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

End Class