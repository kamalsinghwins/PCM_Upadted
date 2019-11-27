Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.IO

Public Class SelfAssistHomeImages

    Inherits System.Web.UI.Page

    Private Const UploadDirectory As String = "~/Uploaded/"

    Protected Sub chkBox_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxCheckBox = TryCast(sender, ASPxCheckBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "chkbox" + key

    End Sub

    Protected Sub UploadControl_FilesUploadComplete(ByVal sender As Object, ByVal e As FilesUploadCompleteEventArgs)

        Dim fileName As String = ""

        If txtDescription.Text = "" Then
            e.ErrorText = "No description was entered. File was not processed."
            e.CallbackData = "error"
            Exit Sub
        End If

        Dim tmpFile As String = ""

        'Check if any of the files have been uploaded previously and whether any of the file names contain a space
        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)
            If file.FileName.Contains(" ") Then
                e.ErrorText = "File names may not contain a space. The image was not uploaded."
                e.CallbackData = "error"
                Exit Sub
            End If
            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)
            tmpFile = file.FileName
            If IO.File.Exists(Path.Combine(MapPath(UploadDirectory), file.FileName)) Then
                e.ErrorText = file.FileName & " has been uploaded previously. The image was not uploaded."
                e.CallbackData = "error"
                Exit Sub
            End If
        Next i

        Dim _BLayer As New SelfAssistBL '(Session("current_company"))

        Dim ReturnString As String

        'If fileName.FileName <> "" AndAlso fileName.IsValid Then
        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)

            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)

            If file.FileName <> "" AndAlso file.IsValid Then
                Try
                    e.CallbackData = "success"
                    file.SaveAs(fileName)
                    ReturnString = _BLayer.UploadScreensaverImage(tmpFile, txtDescription.Text, fileName)

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


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageSelfAssistImages) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            grdImages.DataBind()
        End If


    End Sub

    Private Sub ScreensaverImages_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

        'Submit
        If hdWhichButton.Value = "Accept" Then
            Dim _ssi As New entScreensaverImages

            'Dim startVisibleIndex As Integer = grdImages.VisibleStartIndex
            '' The number of visible rows displayed within the current page.  
            'Dim visibleRowCount As Integer = grdImages.GetCurrentPageRowValues("ProductID").Count
            '' The visible index of the last row within the current page.  
            'Dim endVisibleIndex As Integer = startVisibleIndex + visibleRowCount - 1

            '    ProcessDataRows(startVisibleIndex, endVisibleIndex)

            For i As Integer = 0 To grdImages.GetCurrentPageRowValues("image_name").Count - 1

                Dim column2 As GridViewDataColumn = TryCast(grdImages.Columns("is_active"), GridViewDataColumn)
                Dim is_active As ASPxCheckBox = CType(grdImages.FindRowCellTemplateControl(i, column2, "chkBox"), ASPxCheckBox)

                Dim _si As New ssImages
                _si.image_name = grdImages.GetRowValues(i, "image_name")
                _si.is_active = is_active.Checked.ToString

                _ssi.lstImages.Add(_si)

            Next

            Dim ReturnString As String = ""

            Dim _BLayer As New SelfAssistBL '(Session("current_company"))

            ReturnString = _BLayer.UpdateImages(_ssi)

            If ReturnString <> "Success" Then
                lblError.Text = ReturnString
            Else
                lblError.Text = "The Self Assist Image listing has been updated!"
            End If
        End If

        dxPopUpError.ShowOnPageLoad = True

    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim _BLayer As New SelfAssistBL '(Session("current_company"))

        grdImages.DataSource = _BLayer.GetImageList
        'grdImages.DataBind()


    End Sub

    Private Sub ClearScreen()
        Dim _BLayer As New SelfAssistBL '(Session("current_company"))

        grdImages.DataSource = _BLayer.GetImageList
        grdImages.DataBind()

    End Sub
End Class