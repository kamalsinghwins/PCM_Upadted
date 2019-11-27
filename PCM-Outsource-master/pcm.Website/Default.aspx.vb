Imports System.Data
Imports pcm.BusinessLayer
Imports System.Web.UI
Imports DevExpress.Web
Imports Newtonsoft.Json
Imports Entities

Public Class _Default
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blUsers As New UsersBL()

    Private ReadOnly _encryptionKey As String = ConfigurationManager.AppSettings("EncryptionKey")  '"jg675762jhg18766789j2tqyyab"


    Protected Sub cmdLogin_Click(sender As Object, e As System.EventArgs) Handles cmdLogin.Click

        'Dim EncodedResponse As String = Request.Form("g-Recaptcha-Response")
        'Dim IsCaptchaValid As Boolean = ReCaptchaClass.Validate(EncodedResponse)

        'If Not IsCaptchaValid Then
        '    'Valid Request
        '    lblStatus.Text = "Please verify that you are a human."
        '    Exit Sub
        'End If


        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If dvCaptcha.IsValid = False Then
                Exit Sub
            End If
        End If

        If Len(txtIDNumber.Text) <> 13 Then
            lblStatus.Text = "ID Number must be 13 digits"
            Exit Sub
        End If

        If Len(txtSurname.Text) <= 1 Then
            lblStatus.Text = "Please enter your Surname"
            Exit Sub
        End If

        Dim sIPAddress As String

        sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New StatementBL

        Try

            Dim AccountNumber As String = _BLayer.GetAccountNumber(txtIDNumber.Text, txtSurname.Text.ToUpper)

        Dim LoginDetails As New LoginDetails
        LoginDetails.Browser = Request.Browser.Browser
        LoginDetails.IPAddress = sIPAddress
        LoginDetails.IDNumber = txtIDNumber.Text
        LoginDetails.Device = Request.Browser.Platform
        LoginDetails.Status = If(AccountNumber <> "", True, False)
        LoginDetails.IsMobile = Request.Browser.IsMobileDevice
        _blUsers.AddLoginDetails(LoginDetails)


        If AccountNumber <> "" Then
            'Record the login
            Dim _Login As clsLogin = New clsLogin
            _Login.IDNumber = txtIDNumber.Text
            _Login.LastName = txtSurname.Text
            _Login.IPAddress = sIPAddress
            _Login.CaptchaCode = "reCaptcha"
            _Login.Successful = True
            Dim _LogEntry As New clsLogEntry(_Login)

            Session("account_number") = AccountNumber
            Response.Redirect("view_statement.aspx")
        Else
            'Record the login
            Dim _Login As clsLogin = New clsLogin() With {.IDNumber = txtIDNumber.Text, .LastName = txtSurname.Text,
                                                          .IPAddress = sIPAddress, .CaptchaCode = "reCaptcha", .Successful = False}
            Dim _LogEntry As New clsLogEntry(_Login)

            lblStatus.Text = "The details that you entered do not match to an account on our system"
        End If

        Catch ex As Exception
        _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    'Public Class ReCaptchaClass
    '    Public Shared Function Validate(ByVal EncodedResponse As String) As String
    '        Dim client = New System.Net.WebClient()

    '        Dim PrivateKey As String = "6LfMGXQUAAAAAOr2AkKkqyWjvuY9gYt3Mi4-wUVu"

    '        Dim GoogleReply = client.DownloadString(String.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse))

    '        Dim captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReCaptchaClass)(GoogleReply)

    '        Return captchaResponse.Success
    '    End Function

    '    <JsonProperty("success")>
    '    Public Property Success() As String
    '        Get
    '            Return m_Success
    '        End Get
    '        Set(value As String)
    '            m_Success = value
    '        End Set
    '    End Property
    '    Private m_Success As String

    '    <JsonProperty("error-codes")>
    '    Public Property ErrorCodes() As List(Of String)
    '        Get
    '            Return m_ErrorCodes
    '        End Get
    '        Set(value As List(Of String))
    '            m_ErrorCodes = value
    '        End Set
    '    End Property

    '    Private m_ErrorCodes As List(Of String)

    'End Class

End Class
