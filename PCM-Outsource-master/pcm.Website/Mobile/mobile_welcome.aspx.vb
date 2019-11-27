Imports DevExpress.Web

Public Class mobile_welcome
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Mobile/dispatch_login.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Mobile/dispatch_login.aspx")
                End If
            End If
        End If
    End Sub

    Private Sub mobile_welcome_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
End Class