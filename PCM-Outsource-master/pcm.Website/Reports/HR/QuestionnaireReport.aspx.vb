Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.Export
Public Class reports
    Inherits System.Web.UI.Page
    Private _ReportList As DataSet = Nothing
    Dim _Survey As SurveyBusinessLayer = New SurveyBusinessLayer

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.QuestionnaireReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If

        'GetReports()

    End Sub

    Private Function GetReports()
        _ReportList = _Survey.GetSurveysSummary(txtFromDate.Text, txtToDate.Text, txtSurveyName.Text)
        Session("_SureveyDS") = _ReportList
        gvMaster.DataBind()
        Return Session("_SureveyDS")
    End Function

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataSet = Session("_SureveyDS")
        gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data
        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim completed_survey_id As String = gridView.GetMasterRowKeyValue()
        _ReportList = _Survey.GetSurveyDetailsSummary(completed_survey_id)
        Dim data As DataSet = _ReportList
        gridView.DataSource = data

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click
        GetReports()
    End Sub

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsToResponse(New XlsExportOptionsEx With {.ExportType = ExportType.WYSIWYG})
    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse(New CsvExportOptionsEx() With {.ExportType = ExportType.WYSIWYG})
    End Sub

End Class