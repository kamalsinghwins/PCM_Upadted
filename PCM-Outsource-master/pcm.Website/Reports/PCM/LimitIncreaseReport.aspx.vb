Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Public Class LimitIncreaseReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.LimitIncreaseReport) Then
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

    Private Function GetLimitReportData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("DebtorLimitIncrease")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetData() As DataSet

        If Not IsNothing(Session("data")) Then
            Return Session("data")
        End If

        Dim ReportData As New ReportsBusinessLayer

        Dim limitIncreaseReportData As New DataTable

        limitIncreaseReportData = ReportData.GetLimitIncreaseReport(txtFromDate.Text, txtToDate.Text)

        Dim data As DataSet '= CType(Session("data"), DataSet)

        limitIncreaseReportData.PrimaryKey = New DataColumn() {limitIncreaseReportData.Columns("employee_number")}
        limitIncreaseReportData.TableName = "DebtorLimitIncrease"

        data = New DataSet()
        data.Tables.Add(limitIncreaseReportData)
        Session("data") = data

        Return data
    End Function

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvLimitIncreaseReport.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        'gridView.Columns.Clear()
        'gridView.AutoGenerateColumns = True
        Dim data As DataTable = GetLimitReportData()
        gridView.KeyFieldName = "guid" 'data.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = data

        gvLimitIncreaseReport.EndUpdate()

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("data")

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "From Date: " & txtFromDate.Text & " To Date: " & txtToDate.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Accounts Opened By Employee Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        gvLimitIncreaseReport.DataBind()
    End Sub

    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
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