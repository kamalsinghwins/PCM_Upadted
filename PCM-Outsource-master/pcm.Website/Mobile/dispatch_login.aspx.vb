Imports System.Data
Imports pcm.BusinessLayer
Imports Entities
Imports System.Security.Cryptography
Imports System.IO
Public Class dispatch_login
    Inherits System.Web.UI.Page
    Dim _blUsers As New UsersBL()
    Private ReadOnly _encryptionKey As String = ConfigurationManager.AppSettings("EncryptionKey")  '"jg675762jhg18766789j2tqyyab"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'Check if the browser support cookies
            If Request.Browser.Cookies Then
                'Check if the cookies with name PBLOGIN exist on user's machine
                If Request.Cookies("PCMMOBILELOGIN") IsNot Nothing Then
                    'Pass the user name and password to the VerifyLogin method
                    Dim decrytUsernamePassword As String = Decrypt(Request.Cookies("PCMMOBILELOGIN").Value, _encryptionKey)
                    Dim encryptedUsernamePasswordArr As String() = decrytUsernamePassword.Split(":")

                    If encryptedUsernamePasswordArr.Length = 2 Then
                        Dim tmpUser As String = Decrypt(encryptedUsernamePasswordArr(0), _encryptionKey)
                        Dim tmpPassword As String = Decrypt(encryptedUsernamePasswordArr(1), _encryptionKey)
                        dvCaptcha.IsValid = True
                        'Response.Redirect("login.aspx")

                        Dim _User As New Users
                        _User.Username = tmpUser
                        _User.Password = tmpPassword

                        Dim RetrievedUser As Users = _blUsers.DoLogin(_User)


                        If RetrievedUser Is Nothing Then
                            hdLogin.Value = "false"
                            Exit Sub
                        Else
                            Session("username") = RetrievedUser.Username.ToString.ToUpper
                            Session("email") = RetrievedUser.Email
                            Session("processing_permission_sequence") = RetrievedUser.processing_permission_sequence
                            Session("is_pcm_admin") = RetrievedUser.isPCMAdmin
                            Session("user_is_administrator") = RetrievedUser.IsAdministrator
                            Session("current_company") = "010"
                            hdLogin.Value = "true"

                            Dim sIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                            If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")

                            Session("ipaddress") = sIPAddress
                            Response.Redirect("mobile_welcome.aspx")
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub cmdLogin_Click(sender As Object, e As System.EventArgs) Handles cmdLogin.Click

        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If dvCaptcha.IsValid = False Then
                Exit Sub
            End If
        End If

        Dim sIPAddress As String

        sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")

        If (Page.IsValid) Then
            Dim _User As New Users
            _User.Username = txtUsername.Text.ToUpper
            _User.Password = txtPassword.Text.ToUpper
            _User.IPAddress = sIPAddress

            Dim RetrievedUser As Users = _blUsers.DoLogin(_User)

            If RetrievedUser Is Nothing Then
                hdLogin.Value = "false"
                Exit Sub
            Else
                Session("username") = RetrievedUser.Username.ToString.ToUpper
                Session("email") = RetrievedUser.Email



                If HttpContext.Current.IsDebuggingEnabled Then
                    Session("processing_permission_sequence") = RetrievedUser.processing_permission_sequence.Replace("0", "1")
                    Session("maintenance_permission_sequence") = RetrievedUser.maintenance_permission_sequence.Replace("0", "1")
                    Session("reporting_permission_sequence") = RetrievedUser.reporting_permission_sequence.Replace("0", "1")
                Else
                    Session("processing_permission_sequence") = RetrievedUser.processing_permission_sequence
                    Session("maintenance_permission_sequence") = RetrievedUser.maintenance_permission_sequence
                    Session("reporting_permission_sequence") = RetrievedUser.reporting_permission_sequence
                End If

                Session("is_pcm_admin") = RetrievedUser.isPCMAdmin
                Session("user_is_administrator") = RetrievedUser.IsAdministrator
                Session("current_company") = "010"
                hdLogin.Value = "true"
                If chkRememberMe.Checked = True Then
                    If (Request.Browser.Cookies) Then
                        Dim username As String = Encrypt(txtUsername.Text, _encryptionKey)
                        Dim password As String = Encrypt(txtPassword.Text, _encryptionKey)
                        Dim encrytUsernamePassword As String = username & ":" & password

                        If (Request.Cookies("PCMMOBILELOGIN") Is Nothing) Then
                            Response.Cookies("PCMMOBILELOGIN").Expires = DateTime.Now.AddDays(14)
                        End If

                        Response.Cookies("PCMMOBILELOGIN").Value = Encrypt(encrytUsernamePassword, _encryptionKey)
                    End If
                End If
                Response.Redirect("mobile_welcome.aspx")
            End If

        End If
    End Sub
    Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            Dim bykey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            Dim InputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(strText)
            Dim des As New DESCryptoServiceProvider
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write)
            cs.Write(InputByteArray, 0, InputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Try
            Dim byKey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class