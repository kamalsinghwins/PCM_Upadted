Imports pcm.BusinessLayer
Imports Entities

Public Class collections
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub HeaderMenu_ItemClick(source As Object, e As DevExpress.Web.MenuItemEventArgs) Handles HeaderMenu.ItemClick
        Select Case e.Item.Name
            Case "Collections"
                'Response.Redirect("hud.aspx")


            Case "Logout"
                'If (Request.Cookies("PBLOGIN") IsNot Nothing) Then
                '    'Expire the login cookie
                '    Response.Cookies("PBLOGIN").Expires = datetime.Now.AddDays(-30)
                'End If
                Dim sIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")


                Dim _dlLog As New LoggingBusinessLayer


                'Create the login record
                Dim _NewLog As New CollectionCallLog

                _NewLog.AccountNumber = ""
                _NewLog.ActionResult = "Success"
                _NewLog.ActionType = "User Logout"
                _NewLog.IPAddress = sIPAddress
                _NewLog.LengthOfAction = "0"
                _NewLog.PTPAmount = "0"
                _NewLog.UserComment = ""
                _NewLog.UserName = Session("username")
                _NewLog.PTPDate = ""

                _dlLog.InsertUserLogRecord(_NewLog)

                Session("username") = ""
                Session("user_permissions") = ""
                Session("user_is_administrator") = ""

                Response.Redirect("~/Intranet/Default.aspx")
        End Select


    End Sub
End Class