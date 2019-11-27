Imports DevExpress.Web
Imports System.IO
Imports pcm.BusinessLayer
Imports Entities

Public Class CategoryReport

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CategoryReport) Then
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

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        'Check that there is an email address
        If txtEmail.Text = "" Then
            lblError.Text = "No email address was entered."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        'Just putting into Stockcodes as copied from stock allocation upload
        Dim _BLayer As New StockcodesHOBL

        Dim ReturnString As String = ""

        ReturnString = _BLayer.RunCategoryReport(txtEmail.Text, txtFromDate.Text, txtToDate.Text)

        If ReturnString <> "Success" Then
            lblError.Text = ReturnString
        Else
            lblError.Text = "Your report is running! It will be delivered to the specified email address(es) once it has completed."
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub

    Private Sub CategoryReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
End Class