Imports pcm.BusinessLayer
Imports Entities.Login
Public Class POSLogin
    Inherits System.Web.UI.Page
    Dim _blayer As New LoginBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("username") = "" Then
                'Response.Redirect("~/POS/POS_Main.aspx")
            End If
        End If
    End Sub

    Protected Sub btnlogin_Click(sender As Object, e As EventArgs)

        If txtUsername.Value() = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter the username"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtPassword.Value() = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter the password"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtTillnumber.Value() = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter till number"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        If txtBranch.Value() = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter branch code"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        Dim username As String = txtusername.Value().ToUpper
        Dim password As String = txtpassword.Value().ToUpper
        Dim branch As String = txtbranch.Value().ToUpper
        Dim till_number As String = txttillnumber.Value.ToUpper
        Dim baseResponse As New BaseResponse

        Try
            baseResponse = _blayer.DoLogIn(username, password, till_number, branch)
            If baseResponse.Sucess = True Then
                Session("username") = username
                Session("branch") = branch
                'Response.Redirect("~/POS/POS_Main.aspx")

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
End Class

