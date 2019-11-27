Public Class get_ip
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sIPAddress As String

        sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")
        Response.Write(sIPAddress)
    End Sub

End Class