Public Class MobileIntranet
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub HeaderMenu_ItemClick(source As Object, e As DevExpress.Web.MenuItemEventArgs) Handles HeaderMenu.ItemClick
        Select Case e.Item.Name
            Case "Collections"
                'Response.Redirect("hud.aspx")


            Case "Logout"
                If (Request.Cookies("PCMMOBILELOGIN") IsNot Nothing) Then
                    '  'Expire the login cookie
                    Response.Cookies("PCMMOBILELOGIN").Expires = DateTime.Now.AddDays(-30)
                End If

                Dim sIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")

                Session("username") = ""
                Session("email") = ""
                Session("processing_permission_sequence") = ""
                Session("is_pcm_admin") = ""
                Session("user_is_administrator") = ""
                Session("current_company") = ""
                Session("ipaddress") = ""
                Response.Redirect("~/Mobile/dispatch_login.aspx")
        End Select


    End Sub
End Class