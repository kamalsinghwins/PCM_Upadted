Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class IncomingSMS
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.IncomingSMS) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("current_company") = "010"
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")

            Session.Remove("grid_data")

        End If

    End Sub

    Private Sub ReportsSMSSending_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Function GetData() As DataTable

        If Not IsNothing(Session("grid_data")) Then
            Return Session("grid_data")
        End If

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
        _Log.WebPage = "Incoming SMS Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Dim _BLayer As New ReportsBusinessLayer(Session("current_company"))

        Dim data As DataTable = _BLayer.ReturnIncomingSMS(txtFromDate.Text, txtToDate.Text)

        Session("grid_data") = data

        Return data

    End Function

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        dxGrid.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        gridView.DataSource = GetData()

        dxGrid.EndUpdate()

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("grid_data")

        'dxGrid.Columns.Clear()

        dxGrid.DataBind()

    End Sub

    'Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
    '    Exporter.WritePdfToResponse()
    'End Sub

    'Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
    '    Exporter.WriteXlsxToResponse()
    'End Sub

    'Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
    '    Exporter.WriteCsvToResponse()
    'End Sub


End Class