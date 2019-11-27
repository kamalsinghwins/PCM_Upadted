Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Imports Newtonsoft.Json


Public Class AccountsOpenedByEmployee
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer
    Dim _BLayer As New ReportsBusinessLayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AccountsDrive) Then
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
    Protected Sub RunAccountsReport()

        Dim ReturnString As String = ""
        Dim username As String = String.Empty
        Dim email As String = String.Empty

        If txtFromDate.Text = "" Or txtToDate.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please Select the date"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim accounts As New Entities.Reports.AccountsDrive
        accounts.IsAdmin = Session("is_pcm_admin")
        accounts.IPAddress = Session("ipaddress")
        accounts.Transactions = chkTransactions.Checked

        Dim json As String = JsonConvert.SerializeObject(accounts)

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
            email = Session("email")
        End If

        Try
            ReturnString = _BLayer.RunAccountOpenedByEmployee(txtFromDate.Text, txtToDate.Text, email, username, json)

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

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Public Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Run" Then
            RunAccountsReport()
        End If

    End Sub
End Class

