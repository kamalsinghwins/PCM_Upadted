Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.IO
Public Class ManageTargets
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _BLayer As New MaintenanceBL()
    Private Const UploadDirectory As String = "~/Uploaded/"
    Shared Name As String = String.Empty
    Shared branchdetails As DataTable
    Shared xData As DataTable
    Dim isOld As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri
        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageTargets) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            cboMonths.Items.Clear()
            cboMonths.Text = ""

            For i As Integer = 1 To 12
                cboMonths.Items.Add(i)
            Next

            cboYear.Enabled = True
            cboMonths.ClientEnabled = False

            cboYear.Items.Clear()
            cboYear.Text = ""
            cboYear.Items.Add(Format(Now, "yyyy"))
            cboYear.Items.Add(Val(Format(Now, "yyyy")) + 1)
            txtFile.Text = String.Empty
        End If

    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Import" Then
            Import()
        End If

        If hdWhichButton.Value = "Clear" Then
            clear
        End If

        If hdWhichButton.Value = "Targets" Then
            GetTargets()
        End If

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

    End Sub
    Private Sub ManageTargets_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub UploadControl_FilesUploadComplete(ByVal sender As Object, ByVal e As FilesUploadCompleteEventArgs)

        Dim fileName As String = String.Empty

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

        For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
            Dim file As UploadedFile = UploadControl.UploadedFiles(i)

            fileName = Path.Combine(MapPath(UploadDirectory), file.FileName)

            If file.FileName <> "" AndAlso file.IsValid Then
                Try
                    file.SaveAs(fileName)
                    Name = file.FileName
                    e.CallbackData = "success"

                Catch ex As System.IO.IOException
                    _blErrorLogging.ErrorLogging(ex)
                    IO.File.Delete(fileName)
                    e.ErrorText = ex.Message
                    e.CallbackData = "error"
                End Try
                txtFile.Text = Name
            End If
        Next i

    End Sub
    Private Sub GetTargets()
        Session.Remove("test")

        If cboMonths.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Month"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboYear.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Year"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim _ReturnedData As DataTable

        Try
            branchdetails = New DataTable
            branchdetails.TableName = "Branch"
            branchdetails.Columns.Add("branch_code")
            branchdetails.Columns.Add("branch_name")
            branchdetails.Columns.Add("target")

            _ReturnedData = _BLayer.GetBranchTargets(10, cboYear.Text, cboMonths.Text)

            If _ReturnedData.Rows(0)("error") <> "" Then
                cboMonths.ClientEnabled = True
                dxPopUpError.HeaderText = "Error"
                lblError.Text = _ReturnedData.Rows(0)("error")
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If



            For i As Integer = 1 To Val(_ReturnedData.Rows.Count)
                branchdetails.Rows.Add(_ReturnedData.Rows(i - 1)("branch_code"), _ReturnedData.Rows(i - 1)("branch_name"), _ReturnedData.Rows(i - 1)("target"))
            Next

            Session("test") = branchdetails
            lvData.DataBind()
            cboMonths.ClientEnabled = True

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try


    End Sub
    Private Sub Save()

        If cboYear.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Year before Saving"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboMonths.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Month before Saving"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim _ReturnedData As String = String.Empty

        Try
            If branchdetails IsNot Nothing AndAlso branchdetails.Rows.Count > 0 Then
                _ReturnedData = _BLayer.UpdateBranchTargets(10, cboYear.Text, cboMonths.Text, branchdetails)

                If _ReturnedData <> "Success" Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = _ReturnedData
                    dxPopUpError.ShowOnPageLoad = True
                Else
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = "Targets Uploaded Successfully"
                    dxPopUpError.ShowOnPageLoad = True
                End If

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "There is no data to save"
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub
    Protected Sub ASPxGridView1_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
        Dim gridView As ASPxGridView = DirectCast(sender, ASPxGridView)
        branchdetails.PrimaryKey = New DataColumn() {branchdetails.Columns("branch_code")}
        Dim dataTable As DataTable = If(gridView.GetMasterRowKeyValue() IsNot Nothing, branchdetails, branchdetails)
        Dim row As DataRow = dataTable.Rows.Find(e.Keys(0))
        Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
        enumerator.Reset()
        Do While enumerator.MoveNext()
            row(enumerator.Key.ToString()) = enumerator.Value
        Loop
        gridView.CancelEdit()
        e.Cancel = True
        Session("test") = branchdetails
        lvData.DataBind()

    End Sub
    Protected Sub ASPxGridView1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Session("test") = branchdetails
        lvData.DataSource = Session("test")
    End Sub
    Private Sub Import()
        Dim Details() As String
        Dim ErrorMessages As String = String.Empty
        Dim filePath As String = String.Empty
        Dim ext As String = ".txt"

        If cboYear.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Year before importing"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboMonths.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Month before importing"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If



        If txtFile.Text <> "" Then
            isOld = True
        End If

        If Name <> "" AndAlso isOld = False Then
            txtFile.Text = Name
            ext = ""
        End If

        filePath = HttpContext.Current.Server.MapPath("~\Uploaded\" & txtFile.Text & ext)

        If txtFile.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter a filename or upload to import"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub

        End If

        If IO.File.Exists(filePath) = False Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "This file does not exist."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim _ReturnedData As Branches

        Try
            _ReturnedData = _BLayer.GetAllBranches("010")

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "There was a problem connecting to the server"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End Try

        If _ReturnedData.return_message <> "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = _ReturnedData.return_message
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Session.Remove("test")

        Try
            ' Create an instance of StreamReader to read from a file.
            ' The using statement also closes the StreamReader.
            Using sr As New StreamReader(filePath)
                Dim line As String
                ' Read and display lines from the file until the end of
                ' the file is reached.
                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        Details = line.Split(",")

                        If branchdetails IsNot Nothing Then
                            For CalLoop = 1 To branchdetails.Rows.Count - 1
                                If branchdetails.Rows(CalLoop)("branch_code") = Details(0) Then
                                    ErrorMessages &= Details(0) & " was not added as it already exists in the current list" & vbCrLf
                                    GoTo NoLine
                                End If
                            Next

                        End If

                        Dim was_found As Boolean = False

                        For i As Integer = 0 To _ReturnedData.ListOfBranches.Count - 1
                            If _ReturnedData.ListOfBranches(i).branch_code = Details(0) Then
                                was_found = True
                                branchdetails.Rows.Add(_ReturnedData.ListOfBranches(i).branch_code, _ReturnedData.ListOfBranches(i).branch_name, Val(Details(1)))

                            End If
                        Next

                        If was_found = False Then
                            ErrorMessages &= Details(0) & " was not added as the branch code does not exist" & vbCrLf
                            GoTo NoLine
                        End If
NoLine:

                    End If
                Loop Until line Is Nothing
            End Using

            txtFile.Text = String.Empty
            lvData.DataBind()

        Catch ex As Exception
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "The file could Not be read."
            dxPopUpError.ShowOnPageLoad = True
        End Try

        If ErrorMessages <> "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = ErrorMessages
            dxPopUpError.ShowOnPageLoad = True
        End If


    End Sub
    Private Sub Clear()
        cboYear.SelectedIndex = -1
        cboMonths.SelectedIndex = -1
        txtFile.Text = String.Empty
        cboMonths.ClientEnabled = False
        If branchdetails IsNot Nothing Then
            branchdetails.Clear()
            lvData.DataBind()
        End If
    End Sub

End Class