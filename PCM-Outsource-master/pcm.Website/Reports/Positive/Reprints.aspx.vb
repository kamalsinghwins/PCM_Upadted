Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class Reprints
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.Reprints) Then
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

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "Run" Then
                Run()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub Run()
        Session.Remove("reprints")
        gvReprints.DataBind()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("Reprints")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet

        If Not IsNothing(Session("reprints")) Then
            Return Session("reprints")
        End If

        Dim GetTasksReports As New ReportsBusinessLayer

        Dim reports As New DataTable

        reports = GetTasksReports.GetReprints(txtFromDate.Text, txtToDate.Text)

        Dim data As DataSet
        reports.TableName = "Reprints"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("reprints") = data

        Return data
    End Function

    Protected Sub gvReprints_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReprints.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "running_id"
        gridView.DataSource = data
        gvReprints.EndUpdate()

    End Sub

    Private Sub Reprints_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
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