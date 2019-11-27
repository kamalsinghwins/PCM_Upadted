Imports pcm.DataLayer
Imports Entities.IBT_In
Imports Entities
Imports System.IO
Public Class IBT_InHOBL
    Dim _DLayer As IBT_InHODL
    Dim _blErrorLogging As New ErrorLogBL
    Dim _DShopTransaction As New ShopTransactionsDL
    Dim RG As New CommonFunctions.clsCommon
    Dim saveResponse As New SaveResponse

    Public Sub New()
        _DLayer = New IBT_InHODL
    End Sub

    Public Function FetchDetails(ByVal detailsRequest As FetchDetails) As FetchDetailsResponse
        Return _DLayer.FetchDetails(detailsRequest)
    End Function
    Public Function Save(ByVal saveDetails As SaveIBTIN) As SaveResponse
        Dim LineTax As Double
        Dim tmpGUID As String
        Dim tmpTaxGroup As String
        Dim tmpTaxPercentage As String
        Dim tmpTaxArray() As String
        Dim IBTINNumber As String
        Dim CalLoop As Integer
        Dim GetGUID As String
        Dim PrintPath As String
        Dim getIbt_InNumber As New GetIbt_InNumber
        Dim WriteGUID As String

        Dim Rows As String = String.Empty
        Dim TextToPrint As String
        Dim FullFile As String



        Try
            getIbt_InNumber = _DLayer.GetNum("IBTIN", "HHH")
            If getIbt_InNumber.Success = True Then
                IBTINNumber = getIbt_InNumber.Number

                Dim STRMReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\PrintSample\PrintReport.html"))
                FullFile = STRMReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("IBT OUT", "IBT IN")
                TextToPrint = TextToPrint.Replace("{{TelNum}}", saveDetails.Branch_Telephone_Number)
                TextToPrint = TextToPrint.Replace("{{Date}}", Format(Now, "dd-MM-yyyy"))
                TextToPrint = TextToPrint.Replace("{{transaction}}", IBTINNumber)
                TextToPrint = TextToPrint.Replace("{{Deliver To :}}", "")
                TextToPrint = TextToPrint.Replace("{{DeliverAdd1}}", "")
                TextToPrint = TextToPrint.Replace("{{DeliverAdd2}}", "")
                TextToPrint = TextToPrint.Replace("{{DeliverAdd3}}", "")
                TextToPrint = TextToPrint.Replace("{{DeliverAdd4}}", "")
                TextToPrint = TextToPrint.Replace("{{DeliverAdd5}}", "")
                TextToPrint = TextToPrint.Replace("{{totalqtyreceieved}}", saveDetails.TotalReceievedQuantity)
                TextToPrint = TextToPrint.Replace("{{IBTOut#}}", saveDetails.IBTOutNumber.ToUpper())
                TextToPrint = TextToPrint.Replace("{{CurrentBranchCode}}", "HHH")
                TextToPrint = TextToPrint.Replace("{{branchcode}}", saveDetails.BranchCode.ToUpper())
                TextToPrint = TextToPrint.Replace("To Branch", "From Branch")
                TextToPrint = TextToPrint.Replace("{{branchname}}", saveDetails.BranchName)
                TextToPrint = TextToPrint.Replace("{{totalqtysent}}", saveDetails.TotalSentQuantity)

                tmpGUID = _DLayer.GenerateGUID()
                WriteGUID = _DLayer.GenerateGUID()


                Dim transactionMaster As New TransactionMaster
                transactionMaster.transaction_type = "IBTIN"
                transactionMaster.transaction_number = IBTINNumber
                transactionMaster.reference_number = saveDetails.IBTOutNumber.ToUpper()
                transactionMaster.user_name = saveDetails.Current_User
                transactionMaster.customer_code = saveDetails.BranchCode.ToUpper()
                transactionMaster.rep_code = ""
                transactionMaster.number_of_items = saveDetails.TotalReceievedQuantity
                transactionMaster.transaction_total_tax = saveDetails.TotalTAX
                transactionMaster.transaction_total_non_taxable = 0
                transactionMaster.transaction_total_taxable = saveDetails.TotalExcl
                transactionMaster.till_number = ""
                transactionMaster.transaction_total_discount = 0
                transactionMaster.address_line_1 = saveDetails.Address1
                transactionMaster.address_line_2 = saveDetails.Address2
                transactionMaster.address_line_3 = saveDetails.Address3
                transactionMaster.address_line_4 = saveDetails.Address4
                transactionMaster.address_line_5 = saveDetails.Address5
                transactionMaster.transaction_guid = tmpGUID
                transactionMaster.branch_code = "HHH"
                transactionMaster.sale_time = Format(Now, "HH:mm:ss")
                transactionMaster.sale_date = Format(Now, "yyyy-MM-dd")
                transactionMaster.transaction_total = saveDetails.TotalIncl

                transactionMaster.current_month = Format(Now, "MM")
                transactionMaster.current_hour = Format(Now, "HH")
                transactionMaster.current_weekday = Format(Now, "dddd")
                transactionMaster.current_week = DatePart("ww", Now, vbUseSystemDayOfWeek, vbFirstFullWeek)


                _DShopTransaction.ProcessHeader(transactionMaster, "")

                For CalLoop = 0 To saveDetails.ListData.Count - 1

                    tmpTaxArray = Split(saveDetails.ListData(CalLoop).Tax, " - ")
                    tmpTaxGroup = tmpTaxArray(0)

                    Dim link_id As String = _DLayer.GenerateGUID()

                    tmpTaxArray = Split(saveDetails.ListData(CalLoop).Tax, "(")
                    tmpTaxPercentage = Mid(tmpTaxArray(1), 1, Len(tmpTaxArray(1)) - 1)

                    Dim transactionLineItems As New TransactionLineItems

                    transactionLineItems.Guid = link_id
                    transactionLineItems.Link_GuID = tmpGUID
                    transactionLineItems.master_code = saveDetails.ListData(CalLoop).MCode
                    transactionLineItems.generated_code = saveDetails.ListData(CalLoop).ItemCode
                    transactionLineItems.description = saveDetails.ListData(CalLoop).Description

                    transactionLineItems.tax_group = tmpTaxGroup
                    transactionLineItems.tax_percentage = tmpTaxPercentage
                    transactionLineItems.supplier_account = saveDetails.ListData(CalLoop).Supplier
                    transactionLineItems.supplier_item_code = saveDetails.ListData(CalLoop).SupplierCode
                    transactionLineItems.category_1 = saveDetails.ListData(CalLoop).Cat1

                    transactionLineItems.category_2 = saveDetails.ListData(CalLoop).Cat2
                    transactionLineItems.category_3 = saveDetails.ListData(CalLoop).Cat3
                    transactionLineItems.cost_exclusive_estimated = saveDetails.ListData(CalLoop).CostPrice
                    transactionLineItems.cost_exclusive_average = saveDetails.ListData(CalLoop).CostPrice
                    transactionLineItems.selling_exclusive = saveDetails.ListData(CalLoop).Excl


                    transactionLineItems.discount_value = ""
                    transactionLineItems.discount_reason = ""
                    transactionLineItems.quantity = saveDetails.ListData(CalLoop).Qty
                    transactionLineItems.size = saveDetails.ListData(CalLoop).ItemSize
                    transactionLineItems.size_grid = saveDetails.ListData(CalLoop).SizeGrid


                    transactionLineItems.colour = saveDetails.ListData(CalLoop).Colour
                    transactionLineItems.colour_grid = saveDetails.ListData(CalLoop).ColourGrid
                    transactionLineItems.is_service_item = saveDetails.ListData(CalLoop).IsServiceItem
                    transactionLineItems.branch_code = "HHH"
                    transactionLineItems.transaction_number = IBTINNumber

                    transactionLineItems.transsaction_type = "IBTIN"

                    transactionLineItems.updated_cost = ""

                    GetGUID = _DShopTransaction.ProcessLineItems(transactionLineItems, "")


                    'Update IBT Trasactions'

                    Dim iBTTransactionData As New IBTTransactionData
                    iBTTransactionData.received_qty = saveDetails.ListData(CalLoop).Qty
                    iBTTransactionData.receiving_date = Format(Now, "yyyy-MM-dd")
                    iBTTransactionData.transaction_number = saveDetails.IBTOutNumber
                    iBTTransactionData.receiving_branch_code = "HHH"
                    iBTTransactionData.generated_code = saveDetails.ListData(CalLoop).ItemCode
                    iBTTransactionData.sending_branch_code = saveDetails.BranchCode
                    _DShopTransaction.UpdateIBTTransactions(iBTTransactionData, "")


                    '    'Write the Document Items
                    LineTax = Val(saveDetails.ListData(CalLoop).Incl) - Val(saveDetails.ListData(CalLoop).Excl)


                    '  Print Report 

                    Rows += "<tr><td><a class='cut'>-</a><span> " & saveDetails.ListData(CalLoop).ItemCode & "</span></td><td><span>" & saveDetails.ListData(CalLoop).Description & "</span></td><td><span data-prefix></span><span>" & saveDetails.ListData(CalLoop).Quantity & "</span></td><td><span>" & saveDetails.ListData(CalLoop).Qty & "</span></td></tr>"


                Next

                TextToPrint = TextToPrint.Replace("{DataRows}", Rows)
                Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\PrintDocument_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(filepath)
                file.WriteLine(TextToPrint)
                file.Close()

                Dim PrintFilePath As String = "/temp/PrintDocument_" & WriteGUID & ".html"
                PrintPath = HttpContext.Current.Server.MapPath("~" & PrintFilePath)


                saveResponse.Path = PrintFilePath
                saveResponse.Message = "Saved Successfully"
                saveResponse.Success = True

            Else
                saveResponse.Success = False
                saveResponse.Message = getIbt_InNumber.Message
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            saveResponse.Success = False
            saveResponse.Message = "Something went wrong"
        End Try


        Return saveResponse
    End Function

    Public Sub DeleteFile(ByVal File As DeletePrintFile)
        'Delete a File
        If File.FileName IsNot Nothing Then
            Try
                Dim TextToPrint() As String = File.FileName.Split("/")
                Dim fi As New IO.FileInfo(HttpContext.Current.Server.MapPath("" & File.FileName & ""))
                fi.Delete()


            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try
        End If


    End Sub
End Class
