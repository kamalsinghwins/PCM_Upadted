Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Newtonsoft.Json

Public Class AgingSummaryReport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AgingSummaryReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub AgingSummaryReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "Run" Then
                RunAgingSummaryReport()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Public Sub RunAgingSummaryReport()
        Dim ReturnString As String = String.Empty
        Dim username As String = String.Empty
        Dim email As String = String.Empty

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
            email = Session("email")
        End If


        Dim agingSummary As New Entities.Reports.AgingSummary
        agingSummary.IPAddress = Session("ipaddress")
        agingSummary.IsAdmin = Session("is_pcm_admin")
        agingSummary.TotalOutstanding = txtTotalOutstanding.Text

        Dim json As String = JsonConvert.SerializeObject(agingSummary)

        ReturnString = reports.RunAgingSummaryReport(json, email, username)

        If ReturnString <> "Success" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Something went wrong"
            dxPopUpError.ShowOnPageLoad = True

        Else
            clearForm()
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Your report is running! Please look for the completed report in the View Reports Page after it is done."
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub
    Private Sub clearForm()
        txtTotalOutstanding.Text = String.Empty
    End Sub
End Class