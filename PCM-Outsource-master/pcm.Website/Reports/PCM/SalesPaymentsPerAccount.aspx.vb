Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class SalesPaymentsPerAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Increase page timeout to 10 minutes
        Page.Server.ScriptTimeout = 600

        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SalesPaymentsPerAccount) Then
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
            txtSalesFrom.Text = Format(Now, "yyyy-MM-dd")
            txtSalesTo.Text = Format(Now, "yyyy-MM-dd")
            txtPaymentsFrom.Text = Format(Now, "yyyy-MM-dd")
            txtPaymentsTo.Text = Format(Now, "yyyy-MM-dd")
        End If

    End Sub

    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

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
        _Log.WebPage = "Sales VS Payments Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Session.Remove("data")

        dxGrid.DataBind()

    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim data As DataTable = GetData()

        dxGrid.DataSource = data

    End Sub

    Private Function GetData() As DataTable

        Dim data As DataTable

        If Not IsNothing(Session("data")) Then
            Return Session("data")
        End If

        Dim _NewReport As New ReportsBusinessLayer

        data = _NewReport.GetSalesPayments(txtFromDate.Text, txtToDate.Text, txtSalesFrom.Text, txtSalesTo.Text,
                                           txtPaymentsFrom.Text, txtPaymentsTo.Text)

        Session("data") = data

        Return data

    End Function

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