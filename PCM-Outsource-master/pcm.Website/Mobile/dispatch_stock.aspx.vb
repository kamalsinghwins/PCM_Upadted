Imports DevExpress.Web
Imports Entities
Imports Entities.DispatchStock
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Imports System.IO

Public Class dispatch_stock
    Inherits System.Web.UI.Page
    Public Shared _BLayer As DispatchBL = New DispatchBL
    Public Shared _blErrorLogging As ErrorLogBL = New ErrorLogBL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Mobile/dispatch_login.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Mobile/dispatch_login.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.DispatchStock) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Mobile/dispatch_login.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Mobile/dispatch_login.aspx")
                    End If
                End If
            End If
        End If
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetStockDetail(ByVal IBTOutNumber As String) As GetStock
        Dim getStock As New GetStock

        Try

            Dim dt As New DataTable
            dt.Columns.Add("transaction_number")
            dt.Columns.Add("branch_code")
            dt.Columns.Add("branch_name")
            dt.Columns.Add("address_line_1")
            dt.Columns.Add("address_line_2")
            dt.Columns.Add("address_line_3")
            dt.Columns.Add("address_line_4")
            dt.Columns.Add("address_line_5")
            dt.Columns.Add("counter", GetType(Integer))

            Dim _ReturnedData As DataTable
            Try
                _ReturnedData = _BLayer.ReturnBranchDetails("010", IBTOutNumber, True)

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
                getStock.Message = "Something went wrong."
                getStock.Success = False
                Return getStock
            End Try

            If _ReturnedData.Rows(0)("error") <> "" Then
                getStock.ErrorMessages &= _ReturnedData.Rows(0)("error") & vbCrLf
                GoTo NoLine
            End If
            dt.Rows.Add(IBTOutNumber, _ReturnedData.Rows(0)("branch_code"), _ReturnedData.Rows(0)("branch_name"), _ReturnedData.Rows(0)("address1"), _ReturnedData.Rows(0)("address2"), _ReturnedData.Rows(0)("address3"), _ReturnedData.Rows(0)("address4"), _ReturnedData.Rows(0)("address5"))

NoLine:

            Dim json As String = JsonConvert.SerializeObject(dt, Formatting.Indented)
            getStock.dt = json
            getStock.Success = True

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            getStock.Message = "Something Went Wrong."
            getStock.Success = False
            Return getStock
        End Try

        Return getStock
    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function DispatchStock(ByVal dispatchStockCode As DispatchStockCode) As DispatchStockResponse
        Dim LogRows As String = String.Empty
        Dim DeliveryRows As String = String.Empty
        Dim TextToPrint As String = String.Empty
        Dim FullFile As String = String.Empty
        Dim WriteGUID As String = String.Empty
        Dim PrintDeliveryPath As String = String.Empty
        Dim PrintFilePath As String = String.Empty
        Dim dispatchResponse As New DispatchStockResponse
        Dim dispatch As New DataTable
        Dim dispatchNumber As String = String.Empty
        Dim branchDetails As New DataTable

        If dispatchStockCode.Driver = "" Then
            dispatchResponse.Success = False
            dispatchResponse.Message = "Please provide a Driver name."
            Return dispatchResponse
        End If
        If dispatchStockCode.KM = "" Then
            dispatchResponse.Success = False
            dispatchResponse.Message = "Please provide a KM reading."
            Return dispatchResponse
        End If
        If dispatchStockCode.Registration = "" Then
            dispatchResponse.Success = False
            dispatchResponse.Message = "Please provide a Registration number."
            Return dispatchResponse
        End If

        WriteGUID = Mid(Guid.NewGuid.ToString, 1, 8)
        Dim _ReturnedData As String = String.Empty

        Try

            dispatch.Columns.Add("branch_code")
            dispatch.Columns.Add("branch_name")
            dispatch.Columns.Add("transaction_number")
            dispatch.Columns.Add("counter", GetType(Integer))


            branchDetails.Columns.Add("branch_code")
            branchDetails.Columns.Add("address1")
            branchDetails.Columns.Add("address2")
            branchDetails.Columns.Add("address3")
            branchDetails.Columns.Add("address4")
            branchDetails.Columns.Add("address5")

            For CalLoop = 0 To dispatchStockCode.ListData.Count - 1
                dispatch.Rows.Add(dispatchStockCode.ListData(CalLoop).BranchCode, dispatchStockCode.ListData(CalLoop).BranchName, dispatchStockCode.ListData(CalLoop).IBTOutNumber, 1)
                branchDetails.Rows.Add(dispatchStockCode.ListData(CalLoop).BranchCode, dispatchStockCode.ListData(CalLoop).Address1, dispatchStockCode.ListData(CalLoop).Address2, dispatchStockCode.ListData(CalLoop).Address3, dispatchStockCode.ListData(CalLoop).Address4, dispatchStockCode.ListData(CalLoop).Address5)
            Next


            If dispatch IsNot Nothing AndAlso dispatch.Rows.Count > 0 Then
                dispatchNumber = _BLayer.GetDispatchNumber()
                _ReturnedData = _BLayer.DispatchIBT("010", dispatchStockCode.Driver, dispatchStockCode.KM, dispatchStockCode.Registration, dispatch, dispatchNumber)
                If _ReturnedData <> "Success" Then
                    dispatchResponse.Success = False
                    dispatchResponse.Message = _ReturnedData
                    Return dispatchResponse
                End If
            Else
                dispatchResponse.Success = False
                dispatchResponse.Message = "Nothing to process"
                Return dispatchResponse

            End If


            Dim query = From row In dispatch
                        Group row By BranchName = row.Field(Of String)("branch_name") Into BranchNameGroup = Group
                        Select New With {
                            Key BranchName,
                            .Boxes = BranchNameGroup.Sum(Function(r) r.Field(Of Integer)("counter")),
                            .Branchcode = BranchNameGroup.Select(Function(r) r.Field(Of String)("branch_code"))
                       }

            For Each x In query
                LogRows += "<tr><td><a class='cut'>-</a><span> " & x.Branchcode(0).ToString & "</span></td><td><span>" & x.BranchName & "</span></td><td><span data-prefix></span><span>" & x.Boxes & "</span></td><td><span>_______________</span></td><td><span>_______________</span></td></tr><tr><td><a class='cut'>-</a><span> Accepted By</span></td><td><span></span></td><td><span data-prefix></span><span>Signature</span></td><td colspan='2'><span></span></td></tr><tr></tr><tr></tr><tr></tr>"
                Dim dr() As DataRow = dispatch.Select("branch_code = '" & x.Branchcode(0).ToString & "'")
                Dim dr2() As DataRow = branchDetails.Select("branch_code = '" & x.Branchcode(0).ToString() & "'")
                If dr2.Length > 0 Then
                    DeliveryRows += "<hr><header> <h1>Delivery Note</h1><div><div style='width:40%'><address><p>Driver  :" & dispatchStockCode.Driver & "</p> <p> Registration: " & dispatchStockCode.Registration & "</p> <p> Branch Name : " & x.BranchName.ToString & "</p></address></div><div style='float:Right'><address><p>Date :" & Format(Now, "dd-MM-yyyy") & "</p><p>KM :" & dispatchStockCode.KM & "</p><p></p><p></p><p style='margin-top:  40px;'><b>Deliver To :</b>" & dr2(0)("address1") & "</p><p>" & dr2(0)("address2") & "</p><p>" & dr2(0)("address3") & "</p><p>" & dr2(0)("address4") & "</p><p>" & dr2(0)("address5") & "</p> </address></div></div></header><p align='left'>IBT Number <br/>" & "</p>"
                    For Each dloop As DataRow In dr
                        DeliveryRows += "<p align='left'> <br/>" & dloop("transaction_number") & "</p>"

                    Next
                    DeliveryRows += "<div class='pagebreak'><div><img style='height:70px' class='custom' src='../Images/rage-logo.png' /></div></div>"
                End If
                DeliveryRows += "<br/>"

            Next

            Dim DriverLogReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DriverLog.html"))
            FullFile = DriverLogReader.ReadToEnd
            TextToPrint = FullFile
            TextToPrint = TextToPrint.Replace("{{driver}}", dispatchStockCode.Driver)
            TextToPrint = TextToPrint.Replace("{{date}}", Format(Now, "dd-MM-yyyy"))
            TextToPrint = TextToPrint.Replace("{{km}}", dispatchStockCode.KM)
            TextToPrint = TextToPrint.Replace("{{registration}}", dispatchStockCode.Registration)
            TextToPrint = TextToPrint.Replace("{DataRows}", LogRows)
            Dim logpath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DriverLog_" & dispatchNumber & ".html  "), FileMode.Create)
            Dim writefile As New System.IO.StreamWriter(logpath)
            writefile.WriteLine(TextToPrint)
            writefile.Close()

            Dim PrintPath As String = "/temp/PrintDocument_" & WriteGUID & ".html"
            PrintFilePath = "/temp/DriverLog_" & dispatchNumber & ".html"

            Dim DeliveryNoteReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DeliveryNote.html"))
            FullFile = DeliveryNoteReader.ReadToEnd
            TextToPrint = FullFile
            TextToPrint = TextToPrint.Replace("{{DataRows}}", DeliveryRows)
            Dim deliverypath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DeliveryNote_" & dispatchNumber & ".html  "), FileMode.Create)
            Dim file As New System.IO.StreamWriter(deliverypath)
            file.WriteLine(TextToPrint)
            file.Close()
            PrintDeliveryPath = "/temp/DeliveryNote_" & dispatchNumber & ".html"

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            dispatchResponse.Success = False
            dispatchResponse.Message = "Something went wrong."
            Return dispatchResponse
        End Try

        dispatchResponse.Success = True
        dispatchResponse.DeliveryNotePath = PrintDeliveryPath
        dispatchResponse.DriverLogPath = PrintFilePath
        Return dispatchResponse

    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Sub DeleteFiles(ByVal receipts As DeleteReceipts)
        If receipts IsNot Nothing Then
            'Delete a Delivery Notes
            Try
                Dim notes As New IO.FileInfo(HttpContext.Current.Server.MapPath("~" & receipts.DeliveryNotesPath))
                notes.Delete()

                ''Delete Driver Log             
                Dim log As New IO.FileInfo(HttpContext.Current.Server.MapPath("~" & receipts.DriverLogPath))
                log.Delete()

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)

            End Try
        End If
    End Sub
End Class