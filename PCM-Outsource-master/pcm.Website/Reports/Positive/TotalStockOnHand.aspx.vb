Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.IO

Public Class TotalStockOnHand
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim stock As New ReportsBusinessLayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.TotalStockOnHand) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Session.Remove("TotalStockOnHand")
            gvReports.DataBind()
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
    End Sub
    Private Sub TotalStockOnHand_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub gvReport_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReports.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gvReports.KeyFieldName = "branch_code"
        gvReports.DataSource = data
        gvReports.EndUpdate()

    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("TotalStockOnHand")
        Else
            Return Nothing
        End If

    End Function
    Private Function GetReports() As DataSet
        If Not IsNothing(Session("TotalStockOnHand")) Then
            Return Session("TotalStockOnHand")
        End If
        Dim reports As New DataSet
        reports = stock.GetTotalStockOnHand()
        Dim data As DataSet
        reports.Tables(0).TableName = "TotalStockOnHand"
        data = New DataSet()
        data.Tables.Add(reports.Tables(0).Copy)
        Session("TotalStockOnHand") = data
        Return data
    End Function
    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub
    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub

End Class