Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports Newtonsoft.Json


Public Class BestSellersReport
    Inherits System.Web.UI.Page
    Dim _NewReport As New ReportsBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL


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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.BestSellers) Then
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

    Private Sub BestSellersReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Run" Then
            RunReport()
        End If


    End Sub

    Protected Sub RunReport()
        Dim trim As String = String.Empty
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

        Dim seller As New Entities.Reports.BestSellers
        seller.MasterCode = chkMastercode.Checked
        seller.IsAdmin = Session("is_pcm_admin")
        seller.IPAddress = Session("ipaddress")

        Dim json As String = JsonConvert.SerializeObject(seller)



        Try

            ReturnString = _NewReport.RunBestSellersReport(email, txtFromDate.Text, txtToDate.Text, json, username)

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

    Public Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
        chkMastercode.Checked = False
    End Sub



End Class