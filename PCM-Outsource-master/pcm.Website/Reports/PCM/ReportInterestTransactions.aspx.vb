Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class ReportInterestTransactions
    Inherits System.Web.UI.Page

    Dim _AccountsBL As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.InterestPayments) Then
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
            StartD.Text = Format(Now, "yyyy-MM-dd")
            EndD.Text = Format(Now, "yyyy-MM-dd")


        End If

    End Sub

    Protected Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdView.Click


        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & StartD.Text & " End Date: " & EndD.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Interest Payment Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Dim _dt As New DataTable

        _dt = _AccountsBL.GetPaymentTransactions(StartD.Text, EndD.Text)

        If _dt IsNot Nothing Then
            Session("_Transactions") = _dt
            grdTransactions.DataBind()
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Details Found"
            dxPopUpError.ShowOnPageLoad = True
            Session("_Transactions") = Nothing
            grdTransactions.DataBind()
        End If

    End Sub


    Protected Sub grdTransactions_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdTransactions.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_Transactions")
        gridView.DataSource = data
        grdTransactions.EndUpdate()

    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Export Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & StartD.Text & " End Date: " & EndD.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Payment Transaction Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Exporter.WriteCsvToResponse()
    End Sub
End Class