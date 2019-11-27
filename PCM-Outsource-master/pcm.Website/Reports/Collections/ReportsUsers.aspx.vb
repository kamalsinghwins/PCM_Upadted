Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class ReportsUsers
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.Transactions) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If

    End Sub
    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub Run()
        Session.Remove("ReportUsers")
        dxGrid.DataBind()
    End Sub
    Private Function GetReports() As List(Of UserReport)

        If Not IsNothing(Session("ReportUsers")) Then
            Return Session("ReportUsers")
        End If

        Dim GetUserReports As New UserReportsBusinessLayer

        Dim reports As New List(Of UserReport)

        reports = GetUserReports.GenerateUserReport(txtFromDate.Text, txtToDate.Text)
        Session("ReportUsers") = reports
        Return reports
    End Function
    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        'Dim _NewReport As New UserReportsBusinessLayer
        'dxGrid.DataSource = _NewReport.GenerateUserReport(txtFromDate.Text, txtToDate.Text)
        dxGrid.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As New List(Of UserReport)
        data = GetReports()
        gridView.DataSource = data
        dxGrid.EndUpdate()
    End Sub
    Protected Sub dxGrid_SummaryDisplayText(sender As Object, e As ASPxGridViewSummaryDisplayTextEventArgs)
        If e.Value IsNot Nothing Then
            e.Text = "Sum of Amount is.." & e.Value.ToString()
        End If
    End Sub
    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub
    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub
    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse()
    End Sub
End Class