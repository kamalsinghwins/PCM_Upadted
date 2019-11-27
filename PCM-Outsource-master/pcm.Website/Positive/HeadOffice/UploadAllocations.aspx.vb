Imports DevExpress.Web
Imports System.IO
Imports pcm.BusinessLayer
Imports Entities

Public Class UploadAllocations
    Inherits System.Web.UI.Page

    Private Const UploadDirectory As String = "~/Uploaded/"

    Protected Sub UploadControl_FilesUploadComplete(ByVal sender As Object, ByVal e As FilesUploadCompleteEventArgs)
        'e.CallbackData = SavePostedFile(e.UploadedFile)

        'Dim file As UploadedFile = UploadControl.UploadedFiles(i)

        Dim strGuID As String
        strGuID = Guid.NewGuid.ToString()

        Dim fileName As String = ""

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

        Dim _BLayer As New StockcodesHOBL

        Dim ReturnString As String

        'If fileName.FileName <> "" AndAlso fileName.IsValid Then
        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)

            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)

            If file.FileName <> "" AndAlso file.IsValid Then
                Try
                    e.CallbackData = "success"
                    file.SaveAs(fileName)
                    ReturnString = _BLayer.RunStockAllocation(fileName, txtEmail.Text)

                    If ReturnString <> "Success" Then
                        IO.File.Delete(fileName)
                        e.ErrorText = ReturnString
                        e.CallbackData = "error"
                    End If

                Catch ex As System.IO.IOException
                    IO.File.Delete(fileName)
                    e.ErrorText = ex.Message
                    e.CallbackData = "error"
                End Try
                'End If
            End If
        Next i


        'Dim fileName As String = Path.Combine(MapPath(UploadDirectory), strGuID & ".csv")

        'e.UploadedFile.SaveAs(fileName)

        'lblError.Text = "The file was Uploaded successfully"

        'dxPopUpError.ShowOnPageLoad = True

        'Dim CreateFile As New FileStream(fileName, FileMode.Create)
        'Dim strStreamWriter As New StreamWriter(CreateFile)
        'strStreamWriter.Write(e.UploadedFile.FileContent.Write)
        'strStreamWriter.Close()
        'strStreamWriter.Dispose()

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

    End Sub
    Private Sub UploadAllocations_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.StockAllocationUpload) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If
    End Sub

    Protected Sub UploadControl_FileUploadComplete(sender As Object, e As FileUploadCompleteEventArgs) Handles UploadControl.FileUploadComplete

    End Sub
End Class