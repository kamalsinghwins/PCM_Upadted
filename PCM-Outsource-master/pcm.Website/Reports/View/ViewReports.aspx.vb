Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports System.IO
Imports System.Security.Cryptography


Public Class ViewReports
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer
    Dim _blUsers As New UsersBL()
    Private ReadOnly _encryptionKey As String = ConfigurationManager.AppSettings("EncryptionKey")

    Dim ds As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then

                If Request.Browser.Cookies Then
                    'Check if the cookies with name PBLOGIN exist on user's machine
                    If Request.Cookies("PCMLOGIN") IsNot Nothing Then
                        'Pass the user name and password to the VerifyLogin method
                        Dim decrytUsernamePassword As String = Decrypt(Request.Cookies("PCMLOGIN").Value, _encryptionKey)
                        Dim encryptedUsernamePasswordArr As String() = decrytUsernamePassword.Split(":")

                        If encryptedUsernamePasswordArr.Length = 2 Then
                            Dim tmpUser As String = Decrypt(encryptedUsernamePasswordArr(0), _encryptionKey)
                            Dim tmpPassword As String = Decrypt(encryptedUsernamePasswordArr(1), _encryptionKey)
                            Session("username") = tmpUser

                            Dim _User As New Users
                            _User.Username = tmpUser
                            _User.Password = tmpPassword

                            Dim RetrievedUser As Users = _blUsers.DoLogin(_User)
                            If RetrievedUser IsNot Nothing Then
                                Session("username") = RetrievedUser.Username.ToString.ToUpper
                                Session("email") = RetrievedUser.Email
                                Session("processing_permission_sequence") = RetrievedUser.processing_permission_sequence
                                Session("maintenance_permission_sequence") = RetrievedUser.maintenance_permission_sequence
                                Session("reporting_permission_sequence") = RetrievedUser.reporting_permission_sequence
                                Session("is_pcm_admin") = RetrievedUser.isPCMAdmin
                                Session("user_is_administrator") = RetrievedUser.IsAdministrator
                                Session("current_company") = "010"
                                Dim sIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                                If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")
                                Session("ipaddress") = sIPAddress
                            End If

                        End If
                    Else
                        If Not IsCallback Then
                            Response.Redirect("~/Intranet/Default.aspx")
                        Else
                            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                        End If

                    End If
                End If

            End If

        End If


        If Not IsPostBack Then
            GetReports()
        End If

    End Sub

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

    End Sub

    Public Shared Function DecodedSpaces(ByVal numberOfSpaces As Integer) As String
        Dim spaces As String = ""
        For i As Integer = 0 To numberOfSpaces - 1
            spaces &= "&nbsp;"
        Next
        Return HttpUtility.HtmlDecode(spaces)
    End Function

    Private Function ProcessString(ByVal strin As String) As String
        Dim astrWords As String() = New String() {"_" & Session("database") & "_"} 'you can add multiple words in CSV format
        Dim strOut As String = strin
        Dim strWord As String
        For Each strWord In astrWords
            If strin.IndexOf(strWord, 0) >= 0 Then
                strOut = strWord
                Exit For
            End If
        Next
        Return strOut
    End Function

    Public Sub GetReports()
        Dim files() As String
        Dim File
        Dim Filename
        Dim lvItem As ListEditItem

        Try

            'ADDING ALL FILES INTO A COLLECTION
            files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~\Docs\"))

            If files.Length > 0 Then
                For Each File In files

                    'GETTING THE FILE NAME PART
                    Filename = System.IO.Path.GetFileName(File)


                    lvItem = New ListEditItem
                    lvItem.Value = Filename

                    Dim arrArray() As String
                    Dim SecondArray() As String

                    Try

                        'FORMATTING THE FILENAME TO MAKE IT APPEAR THAT IT'S INSIDE A GRID
                        arrArray = Split(Filename, "_010_")
                        SecondArray = Split(arrArray(1), "_")

                    Catch ex As Exception
                        GoTo NextRecord
                    End Try


                    If SecondArray(0).ToString.Contains(Session("username")) Then

                            lvItem.Text = arrArray(0) & DecodedSpaces(25 - Len(arrArray(0))) & " " & SecondArray(0) & DecodedSpaces(15 - Len(SecondArray(0))) & " " & Mid$(SecondArray(1), 1, 4) & "-" & Mid$(SecondArray(1), 5, 2) & "-" & Mid$(SecondArray(1), 7, 2) & " " & Mid$(SecondArray(1), 9, 2) & ":" & Mid$(SecondArray(1), 11, 2) & ":" & Mid$(SecondArray(1), 13, 2)

                            'ADDING THE TEXT TO THE LISTVIEW
                            lstReports.Items.Add(lvItem)
                        End If

NextRecord:
                Next
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Protected Sub cmdDownload_Click(sender As Object, e As EventArgs) Handles cmdDownload.Click
        If lstReports.Items.Count = 0 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "There are no reports to download"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If lstReports.SelectedItems.Count = 0 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select the report to download"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Response.Redirect("~/Docs/" & lstReports.SelectedItem.Value)

    End Sub
    Protected Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click

        If lstReports.Items.Count = 0 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "There are no reports to delete"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If lstReports.SelectedItems.Count = 0 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a report to delete"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        Try

            For i As Integer = lstReports.Items.Count - 1 To 0 Step -1
                If lstReports.Items(i).Selected Then
                    Dim fileName As String = String.Empty
                    fileName = lstReports.Items(i).Value
                    If fileName IsNot Nothing Then
                        Dim fi As New IO.FileInfo(HttpContext.Current.Server.MapPath("~\Docs\" & fileName & ""))
                        fi.Delete()
                        lstReports.Items.RemoveAt(i)
                    End If
                End If
            Next i

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
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