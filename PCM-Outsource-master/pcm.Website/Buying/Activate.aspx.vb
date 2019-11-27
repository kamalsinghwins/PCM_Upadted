Imports pcm.BusinessLayer
Imports DevExpress.Web

Public Class Activate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

        Session("current_company") = "010"

        'If Not url.Contains("localhost") Then
        '    If Session("username") = "" Then
        '        If Not IsCallback Then
        '            Response.Redirect("~/Buying/Default.aspx")
        '        Else
        '            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
        '        End If
        '    End If
        'End If

        Dim queryemail As String = Request.QueryString("em")
        Dim queryguid As String = Request.QueryString("guid")

        Dim _BLayer As New BuyingBL()
        Dim _dt As DataTable

        'First login with email address
        If queryemail <> "" Then
            _dt = _BLayer.CheckResetPassword(queryemail, "")
        ElseIf queryguid <> "" Then
            _dt = _BLayer.CheckResetPassword("", queryguid)
        Else
            Response.Redirect("~/Buying/Default.aspx")
        End If

        If IsNothing(_dt) Then
            Response.Redirect("~/Buying/Default.aspx")
        Else
            Session("user_id") = _dt.Rows(_dt.Rows.Count - 1)("user_id").ToString
        End If

    End Sub

    Protected Sub tbConfirmedPassword_Validation(ByVal sender As Object, ByVal e As DevExpress.Web.ValidationEventArgs)
        If e.IsValid AndAlso tbPassword.Text <> tbConfirmedPassword.Text Then
            e.ErrorText = "Passwords do not match"
            e.IsValid = False
        End If

        If e.IsValid Then
            If Session("user_id") = "" Then
                Response.Redirect("~/Buying/Default.aspx")
            End If

            Dim ReturnString As String

            Dim _BLayer As New BuyingBL()

            ReturnString = _BLayer.ResetPassword(Session("user_id"), tbPassword.Text)

            If ReturnString <> "Success" Then
                lblError.Text = ReturnString
            Else
                lblError.Text = "Your password has been updated! Please Login."
            End If

            dxPopUpError.ShowOnPageLoad = True

        End If
    End Sub

    Protected Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        'MsgBox("Success")

    End Sub
End Class