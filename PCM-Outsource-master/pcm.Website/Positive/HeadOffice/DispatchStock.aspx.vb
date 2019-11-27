Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraReports.UI
Imports DevExpress.LookAndFeel
Imports System.IO
Imports DevExpress.XtraPrinting.Preview
Imports Entities.DispatchStock
Imports System.Linq

Public Class DispatchStock
    Inherits System.Web.UI.Page
    Dim _BLayer As New DispatchBL()
    Dim _blErrorLogging As New ErrorLogBL

    Private Const UploadDirectory As String = "~/Uploaded/"
    Shared branchdetails As DataTable
    Public Shared xData As DataTable
    Dim DeliveryNote As String
    Dim DriverLog As String
    Shared dt As DataTable
    Shared dt1 As DataTable
    Shared Name As String = String.Empty

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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Processing.DispatchStock) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            branchdetails = New DataTable
            branchdetails.Columns.Add("branch_code")
            branchdetails.Columns.Add("branch_name")
            branchdetails.Columns.Add("address1")
            branchdetails.Columns.Add("address2")
            branchdetails.Columns.Add("address3")
            branchdetails.Columns.Add("address4")
            branchdetails.Columns.Add("address5")
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Import" Then
            ScanBarcode()
        End If

        If hdWhichButton.Value = "Clear" Then
            ClearAll()
        End If

    End Sub
    Private Sub DispatchStock_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
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
                dt = New DataTable
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
                            System.Media.SystemSounds.Beep.Play()
                            GoTo NoLine
                        End If

                        For CalLoop = 0 To lvData.Items.Count - 1
                            If lvData.Items(CalLoop).Text = Details(0) Then
                                ErrorMessages &= Details(0) & " was not added as it already exists in the current list" & vbCrLf
                                GoTo NoLine
                            End If
                        Next


                        'xData.Rows.Add(Details(0), ReturnedData.Rows(0)("branch_code"), ReturnedData.Rows(0)("branch_name"), 1)
                        dt.Rows.Add(Details(0), _ReturnedData.Rows(0)("branch_name"), _ReturnedData.Rows(0)("branch_code"), 1)
                        branchdetails.Rows.Add(_ReturnedData.Rows(0)("branch_name"), _ReturnedData.Rows(0)("branch_name"), _ReturnedData.Rows(0)("address1"),
                                               _ReturnedData.Rows(0)("address2"), _ReturnedData.Rows(0)("address3"), _ReturnedData.Rows(0)("address4"),
                                               _ReturnedData.Rows(0)("address5"))

NoLine:

                    End If
                Loop Until line Is Nothing

                dt1 = New DataTable
                dt1 = dt.DefaultView.ToTable(True, "Details(0)", "branch_name", "branch_code", "counter")
                lvData.DataSource = dt1
                lvData.DataBind()
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
    Private Sub ClearAll()
        branchdetails.Clear()
        txtDriver.Text = String.Empty
        txtFile.Text = String.Empty
        txtKm.Text = String.Empty
        txtRegistration.Text = String.Empty
        lvData.Items.Clear()
        dt1.Clear()
        dt.Clear()
        Name = String.Empty
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim LogRows As String = String.Empty
        Dim DeliveryRows As String = String.Empty
        Dim TextToPrint As String = String.Empty
        Dim FullFile As String = String.Empty
        Dim WriteGUID As String = String.Empty
        Dim PrintDeliveryPath As String = ""


        If txtDriver.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please provide a Driver name."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtKm.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please provide a KM reading."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtRegistration.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please provide a Registration number."
            dxPopUpError.ShowOnPageLoad = True
        End If

        WriteGUID = Mid(Guid.NewGuid.ToString, 1, 8)

        Dim _ReturnedData As String = String.Empty

        Try
            If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then


                _ReturnedData = _BLayer.DispatchIBT("010", txtDriver.Text, txtKm.Text, txtRegistration.Text, dt1)


                If _ReturnedData <> "Success" Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = _ReturnedData
                    dxPopUpError.ShowOnPageLoad = True
                    Exit Sub
                End If
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Nothing to process"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
            Dim NewDriverLog As String = HttpContext.Current.Server.MapPath("~\Uploaded\" & Guid.NewGuid.ToString & ".XML")
            Dim NewDeliveryNotes As String = HttpContext.Current.Server.MapPath("~\Uploaded\" & Guid.NewGuid.ToString & ".XML")

            Dim NewDoc As New CreateDispatchDocument(NewDriverLog, txtDriver.Text, txtRegistration.Text, txtKm.Text)
            Dim NewDeliveryNote As New CreateDeliveryNote(NewDeliveryNotes)

            Dim query = From row In dt1
                        Group row By BranchName = row.Field(Of String)("branch_name") Into BranchNameGroup = Group
                        Select New With {
                            Key BranchName,
                            .Boxes = BranchNameGroup.Sum(Function(r) r.Field(Of Integer)("counter")),
                            .Branchcode = BranchNameGroup.Select(Function(r) r.Field(Of String)("branch_code"))
                       }

            For Each x In query
                NewDeliveryNote.ShopAttribute(True)
                NewDoc.WriteLine(x.Branchcode(0).ToString, x.BranchName, x.Boxes)
                LogRows += "<tr><td><a class='cut'>-</a><span> " & x.Branchcode(0).ToString & "</span></td><td><span>" & x.BranchName & "</span></td><td><span data-prefix></span><span>" & x.Boxes & "</span></td><td><span>_______________</span></td><td><span>_______________</span></td></tr><tr><td><a class='cut'>-</a><span> Accepted By</span></td><td><span></span></td><td><span data-prefix></span><span>Signature</span></td><td colspan='2'><span></span></td></tr><tr></tr><tr></tr><tr></tr>"

                Dim dr() As DataRow = dt1.Select("branch_code = '" & x.Branchcode(0).ToString & "'")
                For Each dloop As DataRow In dr
                    Dim dr2() As DataRow = branchdetails.Select("branch_code = '" & x.Branchcode(0).ToString & "'")
                    NewDeliveryNote.WriteLine(x.Branchcode(0).ToString, x.BranchName.ToString, dr2(0)("address1"), dr2(0)("address2"), dr2(0)("address3"),
                                          dr2(0)("address4"), dr2(0)("address5"), dloop("Details(0)"), txtDriver.Text, txtRegistration.Text, txtKm.Text)
                    DeliveryRows += "<header> <h1>Delivery Note</h1><div><div style='width:40%'><address><p>Driver  :" & txtDriver.Text & "</p> <p> Registration: " & txtRegistration.Text & "</p> <p> Branch Name : " & x.BranchName.ToString & "</p></address></div><div style='float:Right'><address><p>Date :" & Format(Now, "dd-MM-yyyy") & "</p><p>KM :" & txtKm.Text & "</p><p></p><p></p><p style='margin-top:  40px;'><b>Deliver To :</b>" & dr2(0)("address1") & "</p><p>" & dr2(0)("address2") & "</p><p>" & dr2(0)("address3") & "</p><p>" & dr2(0)("address4") & "</p><p>" & dr2(0)("address5") & "</p> </address></div></div></header><article><h1>Recipient</h1><p align='center'>IBT Number :" & dloop("Details(0)") & "</p></article><hr>"

                Next

                NewDeliveryNote.ShopAttribute(False)

            Next

            NewDoc.CloseStream()
            NewDeliveryNote.CloseStream()

            Dim DriverLogReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DriverLog.html"))
            FullFile = DriverLogReader.ReadToEnd
            TextToPrint = FullFile
            TextToPrint = TextToPrint.Replace("{{driver}}", txtDriver.Text)
            TextToPrint = TextToPrint.Replace("{{date}}", Format(Now, "dd-MM-yyyy"))
            TextToPrint = TextToPrint.Replace("{{km}}", txtKm.Text)
            TextToPrint = TextToPrint.Replace("{{registration}}", txtRegistration.Text)
            TextToPrint = TextToPrint.Replace("{DataRows}", LogRows)
            Dim logpath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DriverLog_" & WriteGUID & ".html  "), FileMode.Create)
            Dim writefile As New System.IO.StreamWriter(logpath)
            writefile.WriteLine(TextToPrint)
            writefile.Close()

            Dim PrintPath As String = "/temp/PrintDocument_" & WriteGUID & ".html"

            If chkNotes.Checked = True Then
                Dim DeliveryNoteReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DeliveryNote.html"))
                FullFile = DeliveryNoteReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{DataRows}}", DeliveryRows)
                Dim deliverypath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DeliveryNote_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(deliverypath)
                file.WriteLine(TextToPrint)
                file.Close()
                PrintDeliveryPath = "/temp/DeliveryNote_" & WriteGUID & ".html"
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "There was a problem connecting to the server."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End Try

        Dim PrintFilePath As String = "/temp/DriverLog_" & WriteGUID & ".html"
        PrintReports(PrintDeliveryPath, PrintFilePath)

        ClearAll()

        Dim common As New Common
        common.Path1 = HttpContext.Current.Server.MapPath("~" & PrintDeliveryPath)
        common.Path2 = HttpContext.Current.Server.MapPath("~" & PrintFilePath)
        Dim thr As System.Threading.Thread = New System.Threading.Thread(AddressOf Thread1)
        thr.Start(common)

    End Sub
    Public Sub PrintReports(ByVal url1 As String, ByVal url2 As String)
        If url1 <> "" Then
            Response.Write(String.Format("<script>window.open('" & url1 & "','_blank');</script>"))
        End If
        Response.Write(String.Format("<script>window.open('" & url2 & "','_blank');</script>"))
    End Sub
    Private Sub Thread1(ByVal Common As Object)
        Common.Path1 = Common.Path1
        Common.Path2 = Common.Path2
        Dim threads As List(Of System.Threading.Thread) = New List(Of System.Threading.Thread)
        Dim thr As System.Threading.Thread = New System.Threading.Thread(AddressOf Thread2)
        threads.Add(thr)
        threads.LastOrDefault().Start(Common)
    End Sub
    Private Sub Thread2(ByVal Common As Object)
        System.Threading.Thread.Sleep(5000)

        If Common.Path1 <> "" Then
            Dim fi As New IO.FileInfo((Common.Path1))
            fi.Delete()
        End If

        Dim si As New IO.FileInfo((Common.Path2))
        si.Delete()
    End Sub

End Class