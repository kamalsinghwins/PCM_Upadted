Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports Entities.DispatchStock
Imports System.IO

Public Class ReprintDispatchStock
    Inherits System.Web.UI.Page
    Public Shared _BLayer As ReportsBusinessLayer = New ReportsBusinessLayer
    Public Shared _blErrorLogging As ErrorLogBL = New ErrorLogBL

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.TaskReports) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "Run" Then
                Run()
            End If

            If hdWhichButton.Value = "Download" Then
                Download()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub ReprintDispatchStock_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub Run()
        Session.Remove("DispatchNumber")
        gvDispatchNumber.DataBind()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("DispatchNumber")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet

        If Not IsNothing(Session("DispatchNumber")) Then
            Return Session("DispatchNumber")
        End If


        Dim reports As New DataTable

        reports = _BLayer.GetReprintDocuments(txtFromDate.Text, txtToDate.Text, txtDispatchNumber.Text)

        Dim data As DataSet
        reports.TableName = "DispatchNumber"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("DispatchNumber") = data

        Return data
    End Function
    Protected Sub gvDispatchNumber_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvDispatchNumber.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "dispatch_number"
        gridView.DataSource = data
        gvDispatchNumber.EndUpdate()

    End Sub
    Private Sub Download()
        Dim selectedValues = New List(Of Object)()
        selectedValues = Nothing
        Dim url1 As String
        Dim PrintDeliveryPath As String = "/temp/DeliveryNote_18" & ".html"

        url1 = HttpContext.Current.Server.MapPath("~" & PrintDeliveryPath)
        selectedValues = gvDispatchNumber.GetSelectedFieldValues("dispatch_number")
        'Response.Write(String.Format("<script>window.open('" & url1 & "','_blank');</script>"))
        ASPxWebControl.RedirectOnCallback(url1)
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetDocument(ByVal DispatchNumber As String) As DispatchStockResponse
        Dim reports As New DataTable
        Dim LogRows As String = String.Empty
        Dim DeliveryRows As String = String.Empty
        Dim TextToPrint As String = String.Empty
        Dim FullFile As String = String.Empty
        Dim PrintDeliveryPath As String = String.Empty
        Dim PrintFilePath As String = String.Empty
        Dim WriteGUID As String = String.Empty

        Dim dispatchResponse As New DispatchStockResponse
        WriteGUID = Mid(Guid.NewGuid.ToString, 1, 8)
        Dim xData As New DataTable
        Dim yData As New DataTable

        xData.TableName = "Dispatch"
        xData.Columns.Add("ibt_number")
        xData.Columns.Add("branch_name")
        xData.Columns.Add("branch_code")
        xData.Columns.Add("counter", GetType(Integer))

        Try

            reports = _BLayer.GetDispatchDocuments(DispatchNumber)

            If reports IsNot Nothing AndAlso reports.Rows.Count > 0 Then
                For Each dr As DataRow In reports.Rows
                    xData.Rows.Add(dr("transaction_number"), dr("branch_name"), dr("branch_code"), 1)
                Next

                yData = xData.DefaultView.ToTable(True, "ibt_number", "branch_name", "branch_code", "counter")

                Dim query = From row In yData
                            Group row By BranchName = row.Field(Of String)("branch_name") Into BranchNameGroup = Group
                            Select New With {
                                Key BranchName,
                                .Boxes = BranchNameGroup.Sum(Function(r) r.Field(Of Integer)("counter")),
                                .Branchcode = BranchNameGroup.Select(Function(r) r.Field(Of String)("branch_code"))
                           }

                For Each x In query
                    LogRows += "<tr><td><a class='cut'>-</a><span> " & x.Branchcode(0).ToString & "</span></td><td><span>" & x.BranchName & "</span></td><td><span data-prefix></span><span>" & x.Boxes & "</span></td><td><span>_______________</span></td><td><span>_______________</span></td></tr><tr><td><a class='cut'>-</a><span> Accepted By</span></td><td><span></span></td><td><span data-prefix></span><span>Signature</span></td><td colspan='2'><span></span></td></tr><tr></tr><tr></tr><tr></tr>"
                    Dim dr() As DataRow = yData.Select("branch_code = '" & x.Branchcode(0).ToString & "'")
                    Dim dr2() As DataRow = reports.Select("branch_code = '" & x.Branchcode(0).ToString() & "'")
                    If dr2.Length > 0 Then
                        DeliveryRows += "<hr><header> <h1>Delivery Note</h1><div><div style='width:40%'><address><p>Driver  :" & reports.Rows(0)("driver") & "</p> <p> Registration: " & reports.Rows(0)("rego") & "</p> <p> Branch Name : " & x.BranchName.ToString & "</p></address></div><div style='float:Right'><address><p>Date :" & Format(Now, "dd-MM-yyyy") & "</p><p>KM :" & reports.Rows(0)("km") & "</p><p></p><p></p><p style='margin-top:  40px;'><b>Deliver To :</b>" & dr2(0)("address_line_1") & "</p><p>" & dr2(0)("address_line_2") & "</p><p>" & dr2(0)("address_line_3") & "</p><p>" & dr2(0)("address_line_4") & "</p><p>" & dr2(0)("address_line_5") & "</p> </address></div></div></header><p align='left'>IBT Number <br/>" & "</p>"
                        For Each dloop As DataRow In dr
                            DeliveryRows += "<p align='left'> <br/>" & dloop("ibt_number") & "</p>"

                        Next
                    End If
                    DeliveryRows += "<br/>"

                Next

                Dim DriverLogReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DriverLog.html"))
                FullFile = DriverLogReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{driver}}", reports.Rows(0)("driver"))
                TextToPrint = TextToPrint.Replace("{{date}}", Format(Now, "dd-MM-yyyy"))
                TextToPrint = TextToPrint.Replace("{{km}}", reports.Rows(0)("km"))
                TextToPrint = TextToPrint.Replace("{{registration}}", reports.Rows(0)("rego"))
                TextToPrint = TextToPrint.Replace("{DataRows}", LogRows)
                Dim logpath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DriverLog_" & DispatchNumber & ".html  "), FileMode.Create)
                Dim writefile As New System.IO.StreamWriter(logpath)
                writefile.WriteLine(TextToPrint)
                writefile.Close()

                Dim PrintPath As String = "/temp/PrintDocument_" & WriteGUID & ".html"
                PrintFilePath = "/temp/DriverLog_" & DispatchNumber & ".html"

                Dim DeliveryNoteReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\DispatchStock\DeliveryNote.html"))
                FullFile = DeliveryNoteReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{DataRows}}", DeliveryRows)
                Dim deliverypath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\DeliveryNote_" & DispatchNumber & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(deliverypath)
                file.WriteLine(TextToPrint)
                file.Close()
                PrintDeliveryPath = "/temp/DeliveryNote_" & DispatchNumber & ".html"

            End If

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