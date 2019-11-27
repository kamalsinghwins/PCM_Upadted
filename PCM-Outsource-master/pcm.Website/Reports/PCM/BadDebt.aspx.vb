Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.Web.UI
Imports System.IO
Imports Newtonsoft.Json


Public Class BadDebt
    Inherits System.Web.UI.Page
    Dim _BLayer As New ReportsBusinessLayer

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.BadDebt) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Dim _NewReport As New ReportsBusinessLayer

            cboPeriod.DataSource = _NewReport.GetInternalPeriods()
            cboPeriod.DataBind()

        End If

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "Ok" Then
            RunReport()

        End If



    End Sub

    Private Sub BadDebt_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Server.ScriptTimeout = 300
    End Sub

    Private Sub clearForm()
        cboPeriod.SelectedIndex = -1
    End Sub
    Protected Sub RunReport()

        Dim ReturnString As String = ""
        Dim SelectedPeriod As String() = cboPeriod.Text.Split(" - ")
        Dim email As String = String.Empty
        Dim Username As String = String.Empty

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            Username = Session("username")
            email = Session("email")
        End If
        Dim BadDebtRequest As New Entities.Reports.BadDebtRequest
        BadDebtRequest.Period = SelectedPeriod(0)
        BadDebtRequest.IsAdmin = Session("is_pcm_admin")
        BadDebtRequest.IPAddress = Session("ipaddress")

        Dim json As String = JsonConvert.SerializeObject(BadDebtRequest)

        ReturnString = _BLayer.RunBadDebtReport(json, email, Username)
        If ReturnString = "Success" Then
            clearForm()
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Your report is running! Please look for the completed report in the View Reports Page after it is done."
            dxPopUpError.ShowOnPageLoad = True
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "An error occured while saving the Bad Debt report"
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub
End Class