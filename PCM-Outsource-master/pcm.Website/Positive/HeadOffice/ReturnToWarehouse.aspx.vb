Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.IO
Imports Entities.DispatchStock
Public Class ReturnToWarehouse
    Inherits System.Web.UI.Page
    Private Const UploadDirectory As String = "~/Uploaded/"
    Shared _blErrorLogging As New ErrorLogBL
    Shared _BLayer As New DispatchBL()
    Shared Name As String = String.Empty
    Shared dt1 As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Import" Then
            ScanBarcode()
        End If

        If hdWhichButton.Value = "Clear" Then
            Clear()
        End If

    End Sub
    Private Sub DispatchStock_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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
    Private Sub ScanBarcode()
        Dim Details() As String
        Dim ErrorMessages As String = String.Empty
        Dim filePath As String = String.Empty
        Dim ext As String = ".txt"
        If Name <> "" Then
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
        Try
            Using sr As New StreamReader(filePath)
                Dim line As String
                Dim dt = New DataTable
                dt.Columns.Add("Details(0)")
                dt.Columns.Add("branch_code")
                dt.Columns.Add("branch_name")
                dt.Columns.Add("counter", GetType(Integer))
                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        Details = line.Split(",")
                        Dim _ReturnedData As DataTable
                        Try
                            _ReturnedData = _BLayer.ReturnBranchDetails("010", Details(0), True)
                        Catch ex As Exception
                            dxPopUpError.HeaderText = "Error"
                            lblError.Text = "This file does not exist."
                            dxPopUpError.ShowOnPageLoad = True
                            Exit Sub
                        End Try
                        If _ReturnedData.Rows(0)("error") <> "" Then
                            ErrorMessages &= _ReturnedData.Rows(0)("error") & vbCrLf
                            GoTo NoLine
                        End If
                        dt.Rows.Add(Details(0), _ReturnedData.Rows(0)("branch_name"), _ReturnedData.Rows(0)("branch_code"), 1)
NoLine:
                    End If
                Loop Until line Is Nothing
                dt1 = New DataTable
                dt1 = dt.DefaultView.ToTable(True, "Details(0)", "branch_name", "branch_code", "counter")
                lvData.DataSource = dt1
                lvData.DataBind()
                dt.Clear()
            End Using
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "The file could not be read."
            dxPopUpError.ShowOnPageLoad = True
        End Try
        If ErrorMessages <> "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = ErrorMessages
            dxPopUpError.ShowOnPageLoad = True
        End If
        txtFile.Text = String.Empty

    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Save() As DispatchStockResponse
        Dim _ReturnedData As String = String.Empty
        Dim DeliveryRows As String = String.Empty
        Dim LogRows As String = String.Empty
        Dim FullFile As String = String.Empty
        Dim TextToPrint As String = String.Empty
        Dim WriteGUID As String = String.Empty
        Dim warehouseResponse As New DispatchStockResponse
        Dim PrintReturnedToWarehouse As String = String.Empty
        Try
            If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                WriteGUID = Mid(Guid.NewGuid.ToString, 1, 8)

                _ReturnedData = _BLayer.ReturnToWarehouse("010", dt1)
                If _ReturnedData <> "Success" Then
                    warehouseResponse.Success = False
                    warehouseResponse.Message = _ReturnedData
                    Return warehouseResponse
                End If


                For Each dr As DataRow In dt1.Rows
                    LogRows += "<tr><td><a class='cut'>-</a><span> " & dr("Details(0)").ToString & "</span></td><td><span>" & dr("branch_code") & "</span></td><td><span data-prefix></span><span>" & dr("branch_name") & "</span></td></tr>"
                Next
                DeliveryRows += "<header> <h1>Return To Warehouse</h1><div><div style='width:40%'></div><div style='float:Right'><address><p>Date :" & Format(Now, "dd-MM-yyyy") & " </address></div></div></header><article><h1>Recipient</h1> <table class='inventory'><thead><tr><th width='25%'><span>IBT Out Number</span></th><th width='25%' span>Branch Code</th><th width='50%'><span>Branch Name</span></th></tr></thead><tbody>" & LogRows & "</tbody></table></article><hr>"

                Dim DeliveryNoteReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DeliveryNote.html"))
                FullFile = DeliveryNoteReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{DataRows}}", DeliveryRows)
                Dim deliverypath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\RTW_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(deliverypath)
                file.WriteLine(TextToPrint)
                file.Close()
                PrintReturnedToWarehouse = "/temp/RTW_" & WriteGUID & ".html"
                warehouseResponse.Success = True
                warehouseResponse.DriverLogPath = PrintReturnedToWarehouse
            Else
                warehouseResponse.Success = False
                warehouseResponse.Message = "Nothing to process"
            End If


        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            warehouseResponse.Success = False
            warehouseResponse.Message = "Something went wrong."
        End Try
        Return warehouseResponse
    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Sub DeleteFile(ByVal file As String)
        If file IsNot Nothing Then
            'Delete a Delivery Notes
            Try
                Dim doc As New IO.FileInfo(HttpContext.Current.Server.MapPath("~" & file))
                doc.Delete()

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try
        End If

    End Sub
    Protected Sub Clear()
        dt1.Clear()
        lvData.DataSource = Nothing
        lvData.DataBind()
    End Sub
End Class