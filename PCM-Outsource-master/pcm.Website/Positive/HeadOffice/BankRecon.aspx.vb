Imports DevExpress.Web
Imports System.IO
Imports pcm.BusinessLayer
Imports Entities

Public Class BankRecon

    Inherits System.Web.UI.Page

    Private Const UploadDirectory As String = "~/Uploaded/"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.Server.ScriptTimeout = 600
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.BankRecon) Then
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

        If Not IsPostBack Then

            cboFileType.Items.Add("Select Type of File...")
            cboFileType.Items.Add("EFT Statement")
            cboFileType.Items.Add("ABSA Statement")
            cboFileType.Items.Add("FNB Statement")
            cboFileType.Items.Add("Nedbank Statement")
            cboFileType.Items.Add("Standard Bank Statement")
            cboFileType.Items.Add("Capitec Bank Statement")

            cboFileType.Text = "Select Type of File..."

            Dim _BLayer As New GeneralHOBL()
            Dim _dt As DataTable

            _dt = _BLayer.GetBranches

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
                cboBranchEFT.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
            Next

            txtDate.Text = Format(Now, "yyyy-MM-dd")

        End If


    End Sub

    Protected Sub UploadControl_FilesUploadComplete(ByVal sender As Object, ByVal e As FilesUploadCompleteEventArgs)
        'e.CallbackData = SavePostedFile(e.UploadedFile)

        'Dim file As UploadedFile = UploadControl.UploadedFiles(i)

        Dim strGuID As String
        strGuID = Guid.NewGuid.ToString()

        Dim fileName As String = ""

        'Check that there is an email address
        If cboFileType.Text = "Select Type of File..." Then
            e.ErrorText = "The system was not able to process file. No Type of File selected."
            e.CallbackData = "error"
            Exit Sub
        End If

        If Not HttpContext.Current.IsDebuggingEnabled Then
            'Check If any of the files have been uploaded previously
            For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
                Dim file As UploadedFile = UploadControl.UploadedFiles(i)
                fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)
                If IO.File.Exists(Path.Combine(MapPath(UploadDirectory), file.FileName)) Then
                    e.ErrorText = file.FileName & " already exists on the system. Try changing the file name. Files were not processed"
                    e.CallbackData = "error"
                    Exit Sub
                End If
            Next i
        End If

        Dim _BLayer As New GeneralHOBL

        Dim ReturnString As String

        'If fileName.FileName <> "" AndAlso fileName.IsValid Then
        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)

            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)

            If file.FileName <> "" AndAlso file.IsValid Then
                Try
                    e.CallbackData = "success"
                    file.SaveAs(fileName)
                    ReturnString = _BLayer.RunStatementUpload(fileName, cboFileType.Text)

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

    Private Sub BankRecon_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        Dim _BLayer As New GeneralHOBL
        Dim _Result As String = _BLayer.InsertCashupComment(txtDate.Text,
                                                            Mid(cboBranch.Text, 1, 3),
                                                            txtNotes.Text, Session("username"))

        If _Result = "Success" Then
            txtNotes.Text = ""
            cboBranch.Text = ""
        Else
            lblStatus.Text = "Error: " & _Result
        End If

        dxPopUpError.ShowOnPageLoad = True

    End Sub

    Protected Sub cmdSaveEFTId_Click(sender As Object, e As EventArgs) Handles cmdSaveEFTId.Click

        Dim _BLayer As New GeneralHOBL

        Dim _Result As String = _BLayer.UpdateEFTID(Mid(cboBranchEFT.Text, 1, 3), Mid(cboBranchEFT.Text, 7),
                                                    cboEFTId.Text)

        If _Result = "success" Then
            lblStatus.Text = "EFT ID insert successfully"
            cboBranchEFT.Text = ""
            cboBranch.Text = ""
            cboEFTId.Items.Clear()
            cboEFTId.Text = ""
        Else
            lblStatus.Text = "Error: " & _Result
        End If

        dxPopUpError.ShowOnPageLoad = True

    End Sub

    Protected Sub cboEFTId_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEFTId.SelectedIndexChanged



    End Sub

    Protected Sub cmdDeleteEFTId_Click(sender As Object, e As EventArgs) Handles cmdDeleteEFTId.Click

        Dim _BLayer As New GeneralHOBL

        Dim _Result As String = _BLayer.DeleteEFTID(Mid(cboBranch.Text, 1, 3),
                                                    cboEFTId.Text)

        If _Result = "success" Then
            lblStatus.Text = "EFT ID deleted successfully"
            cboBranchEFT.Text = ""
            cboBranch.Text = ""
            cboEFTId.Items.Clear()
            cboEFTId.Text = ""
        Else
            lblStatus.Text = "Error: " & _Result
        End If

        dxPopUpError.ShowOnPageLoad = True

    End Sub

    Protected Sub cboBranchEFT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBranchEFT.SelectedIndexChanged

        Dim _BLayer As New GeneralHOBL()
        Dim _dt As DataTable

        cboEFTId.Items.Clear()
        cboEFTId.Text = ""

        _dt = _BLayer.GetEFTIDS(Mid(cboBranchEFT.Text, 1, 3))

        For i As Integer = 0 To _dt.Rows.Count - 1
            cboEFTId.Items.Add(_dt(i)("eft_id"))
            cboEFTId.Text = _dt(i)("eft_id")
        Next

    End Sub

    Private Sub cboBranchEFT_TextChanged(sender As Object, e As EventArgs) Handles cboBranchEFT.TextChanged


    End Sub
End Class