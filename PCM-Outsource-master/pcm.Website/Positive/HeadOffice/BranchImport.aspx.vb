Imports DevExpress.Web
Imports System.IO
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.Utils

Public Class BranchImport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Private Const UploadDirectory As String = "~/Uploaded/"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ImportBranches) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub BranchImport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "downloadSample" Then
                Download()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub UploadControl_FilesUploadComplete(ByVal sender As Object, ByVal e As FilesUploadCompleteEventArgs)
        Dim fileName As String = String.Empty
        Dim username As String = String.Empty

        If Session("username") = "" Then
            e.CallbackData = "no username"
            Exit Sub
        Else
            username = Session("username")
        End If

        'Check that there is an email address
        If txtEmail.Text = "" Then
            e.ErrorText = "No email address was entered. Files were not processed."
            e.CallbackData = "error"
            Exit Sub
        End If

        'Check if any of the files have been uploaded previously
        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)
            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)
            If IO.File.Exists(Path.Combine(MapPath(UploadDirectory), file.FileName)) Then
                e.ErrorText = file.FileName & " has been uploaded previously. Files were not processed"
                e.CallbackData = "error"
                Exit Sub
            End If
        Next i

        Dim ReturnString As String = String.Empty
        Dim _manage As New ManageHOBL

        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)
            fileName = Path.Combine(MapPath(UploadDirectory), "_" & Guid.NewGuid().ToString() & file.FileName)
            If file.FileName <> "" AndAlso file.IsValid Then
                Try
                    file.SaveAs(fileName)
                    ReturnString = _manage.RunImport(fileName, txtEmail.Text, "Branches", username)
                    e.CallbackData = "success"

                    If ReturnString <> "Success" Then
                        IO.File.Delete(fileName)
                        e.ErrorText = ReturnString
                        e.CallbackData = "error"
                    End If

                Catch ex As System.IO.IOException
                    _blErrorLogging.ErrorLogging(ex)
                    IO.File.Delete(fileName)
                    e.ErrorText = ex.Message
                    e.CallbackData = "error"
                End Try
            End If
        Next i


    End Sub
    Private Sub Download()
        ASPxWebControl.RedirectOnCallback("~/temp/Import/" & "BranchImportSample.txt")
    End Sub
End Class