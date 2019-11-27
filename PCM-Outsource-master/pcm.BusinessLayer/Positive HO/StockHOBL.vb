Imports pcm.DataLayer
Imports Entities
Imports Entities.Stock
Imports System.IO

Public Class StockHOBL
    Dim _DLayer As StockHODL
    Dim _blErrorLogging As New ErrorLogBL
    Dim _DShopTransaction As New ShopTransactionsDL
    Dim generatedCodeInfoResponse As New GeneratedCodeInfoResponse
    Dim saveResponse As New SaveResponse


    Public Sub New()
        _DLayer = New StockHODL
    End Sub
    Public Function GetCodes(ByVal getCode As GetCodesRequest) As GeneratedCodeInfoResponse
        Return _DLayer.GetCodes(getCode)
    End Function
    Public Function GetGeneratedCodeInfo(ByVal _strGeneratedCodeOrBarcode As String, ByVal _strBranchCode As String) As GetGeneratedCodeInfoResponse
        Return _DLayer.GetGeneratedCodeInfo(_strGeneratedCodeOrBarcode, _strBranchCode)
    End Function
    Public Function DoStock(stockAdjustmentItems As StockTransactions)
        Return _DShopTransaction.StockAdjustments(stockAdjustmentItems)
    End Function
    Public Sub AddHeader(ByVal transactionMaster As TransactionMaster)
        _DShopTransaction.ProcessHeader(transactionMaster, "")
    End Sub
    Public Function Save(ByVal saveRequest As SaveStock) As SaveResponse
        Dim LineTax As Double
        Dim getNumber As New GetNumber
        Dim CalLoop As Integer
        Dim StkAdjNumber As String
        Dim tmpGUID As String
        Dim stockTransactions As New StockTransactions
        Dim Rows As String = String.Empty
        Dim TextToPrint As String
        Dim FullFile As String
        Dim PrintPath As String
        Dim WriteGUID As String
        WriteGUID = Guid.NewGuid.ToString

        'Get number
        Try
            getNumber = _DLayer.GetNum("IBTOUT", "HHH")
            If getNumber.Success = True Then
                StkAdjNumber = getNumber.Number
                tmpGUID = Guid.NewGuid.ToString()

                Dim STRMReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\PrintSample\StockAdjustments.html"))
                FullFile = STRMReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{TelNum}}", saveRequest.Telephone_Number)
                TextToPrint = TextToPrint.Replace("{{Date}}", Format(Now, "dd-MM-yyyy"))
                TextToPrint = TextToPrint.Replace("{{transaction}}", StkAdjNumber)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd1}}", saveRequest.Address1)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd2}}", saveRequest.Address2)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd3}}", saveRequest.Address3)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd4}}", saveRequest.Address4)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd5}}", saveRequest.Address5)
                TextToPrint = TextToPrint.Replace("{{totalqty}}", saveRequest.Quantity)
                TextToPrint = TextToPrint.Replace("{{reference}}", saveRequest.Reference.ToUpper())
                TextToPrint = TextToPrint.Replace("{{CurrentBranchCode}}", "HHH")
                TextToPrint = TextToPrint.Replace("{{branchcode}}", saveRequest.BranchCode.ToUpper())
                TextToPrint = TextToPrint.Replace("{{branchname}}", saveRequest.BranchName.ToUpper())
                TextToPrint = TextToPrint.Replace("{{subtotal}}", saveRequest.TotalExcl)
                TextToPrint = TextToPrint.Replace("{{tax}}", saveRequest.TotalTAX)
                TextToPrint = TextToPrint.Replace("{{total}}", saveRequest.TotalIncl)
                TextToPrint = TextToPrint.Replace("{{FaxNum}}", saveRequest.Fax)

                'Post the header
                Dim transactionMaster As New TransactionMaster
                transactionMaster.transaction_type = "STKADJ"
                transactionMaster.account_number = saveRequest.BranchCode.ToUpper
                transactionMaster.transaction_number = StkAdjNumber
                transactionMaster.reference_number = saveRequest.Reference.ToUpper()
                transactionMaster.user_name = saveRequest.Current_User
                transactionMaster.rep_code = ""
                transactionMaster.number_of_items = saveRequest.Quantity
                transactionMaster.transaction_total_tax = saveRequest.TotalTAX
                transactionMaster.transaction_total_non_taxable = 0
                transactionMaster.transaction_total_taxable = saveRequest.TotalExcl
                transactionMaster.till_number = ""
                transactionMaster.transaction_total_discount = 0
                transactionMaster.address_line_1 = saveRequest.Address1
                transactionMaster.address_line_2 = saveRequest.Address2
                transactionMaster.address_line_3 = saveRequest.Address3
                transactionMaster.address_line_4 = saveRequest.Address4
                transactionMaster.address_line_5 = saveRequest.Address5
                transactionMaster.transaction_guid = tmpGUID
                transactionMaster.branch_code = saveRequest.BranchCode.ToUpper
                transactionMaster.sale_time = Format(Now, "HH:mm:ss")
                transactionMaster.sale_date = Format(Now, "yyyy-MM-dd")
                transactionMaster.transaction_total = saveRequest.TotalIncl

                transactionMaster.current_month = Format(Now, "MM")
                transactionMaster.current_hour = Format(Now, "HH")
                transactionMaster.current_weekday = Format(Now, "dddd")
                transactionMaster.current_week = DatePart("ww", Now, vbUseSystemDayOfWeek, vbFirstFullWeek)

                AddHeader(transactionMaster)

                Dim FsBody As New FileStream(HttpContext.Current.Server.MapPath("~\temp\" & "LASTIBTOUT_" & saveRequest.Current_User & ".txt "), FileMode.Create)
                Dim StrmBody As New StreamWriter(FsBody)
                'Document Transaction
                For CalLoop = 0 To saveRequest.ListData.Count - 1
                    'Post the stock
                    'Bring the GUID all the way back from the transaction_line_item to tie up the stockcode details to save the extra query
                    'in the IBT In
                    If Val(saveRequest.ListData(CalLoop).Qty) >= 0 Then
                        stockTransactions.MCode = saveRequest.ListData(CalLoop).MCode
                        stockTransactions.GCode = saveRequest.ListData(CalLoop).ItemCode
                        stockTransactions.DDescription = saveRequest.ListData(CalLoop).Description
                        stockTransactions.TransType = "STKADJ+"
                        stockTransactions.TheShiftOrBlank = ""
                        stockTransactions.Current_UserOrBlank = "STKADJ+"
                        stockTransactions.TransactionNumber = StkAdjNumber
                        stockTransactions.TheRepOrBlank = ""
                        stockTransactions.SellEx = saveRequest.ListData(CalLoop).SellEx
                        stockTransactions.tQtyPositiveOnly = saveRequest.ListData(CalLoop).Qty
                        stockTransactions.TaxGroupDescriptionPercentage = saveRequest.ListData(CalLoop).Tax
                        stockTransactions.DSupplier = saveRequest.ListData(CalLoop).Supplier
                        stockTransactions.DSupplierItemCode = saveRequest.ListData(CalLoop).SupplierCode
                        stockTransactions.DisServiceItem = saveRequest.ListData(CalLoop).IsServiceItem
                        stockTransactions.DCategory1 = saveRequest.ListData(CalLoop).Cat1
                        stockTransactions.DCategory2 = saveRequest.ListData(CalLoop).Cat2
                        stockTransactions.DCategory3 = saveRequest.ListData(CalLoop).Cat3
                        stockTransactions.DItemSize = saveRequest.ListData(CalLoop).ItemSize
                        stockTransactions.DItemSizeGrid = saveRequest.ListData(CalLoop).SizeGrid
                        stockTransactions.DItemColour = saveRequest.ListData(CalLoop).Colour
                        stockTransactions.DItemColourGrid = saveRequest.ListData(CalLoop).ColourGrid
                        stockTransactions.OriginalAvgCost = saveRequest.ListData(CalLoop).OriginalCost
                        stockTransactions.OriginalEstimatedCost = saveRequest.ListData(CalLoop).Excl
                        stockTransactions.DiscountAmount = ""
                        stockTransactions.DiscountReason = ""
                        stockTransactions.UpdatedCostEx = ""
                        stockTransactions.tGuID = tmpGUID
                        stockTransactions.BranchCode = saveRequest.BranchCode.ToUpper
                        stockTransactions.Tax = saveRequest.ListData(CalLoop).Tax

                    Else
                        stockTransactions.MCode = saveRequest.ListData(CalLoop).MCode
                        stockTransactions.GCode = saveRequest.ListData(CalLoop).ItemCode
                        stockTransactions.DDescription = saveRequest.ListData(CalLoop).Description
                        stockTransactions.TransType = "STKADJ-"
                        stockTransactions.TheShiftOrBlank = ""
                        stockTransactions.Current_UserOrBlank = "STKADJ+"
                        stockTransactions.TransactionNumber = StkAdjNumber
                        stockTransactions.TheRepOrBlank = ""
                        stockTransactions.SellEx = saveRequest.ListData(CalLoop).SellEx
                        stockTransactions.tQtyPositiveOnly = Val(Val(saveRequest.ListData(CalLoop).Qty) * -1)
                        stockTransactions.TaxGroupDescriptionPercentage = saveRequest.ListData(CalLoop).Tax
                        stockTransactions.DSupplier = saveRequest.ListData(CalLoop).Supplier
                        stockTransactions.DSupplierItemCode = saveRequest.ListData(CalLoop).SupplierCode
                        stockTransactions.DisServiceItem = saveRequest.ListData(CalLoop).IsServiceItem
                        stockTransactions.DCategory1 = saveRequest.ListData(CalLoop).Cat1
                        stockTransactions.DCategory2 = saveRequest.ListData(CalLoop).Cat2
                        stockTransactions.DCategory3 = saveRequest.ListData(CalLoop).Cat3
                        stockTransactions.DItemSize = saveRequest.ListData(CalLoop).ItemSize
                        stockTransactions.DItemSizeGrid = saveRequest.ListData(CalLoop).SizeGrid
                        stockTransactions.DItemColour = saveRequest.ListData(CalLoop).Colour
                        stockTransactions.DItemColourGrid = saveRequest.ListData(CalLoop).ColourGrid
                        stockTransactions.OriginalAvgCost = saveRequest.ListData(CalLoop).OriginalCost
                        stockTransactions.OriginalEstimatedCost = saveRequest.ListData(CalLoop).Excl
                        stockTransactions.DiscountAmount = ""
                        stockTransactions.DiscountReason = ""
                        stockTransactions.UpdatedCostEx = ""
                        stockTransactions.tGuID = tmpGUID
                        stockTransactions.BranchCode = saveRequest.BranchCode.ToUpper
                        stockTransactions.Tax = saveRequest.ListData(CalLoop).Tax
                    End If

                    DoStock(stockTransactions)
                    'Write the Document Items
                    LineTax = Val(saveRequest.ListData(CalLoop).Incl) - Val(saveRequest.ListData(CalLoop).Excl)
                    Rows += "<tr><td><a class='cut'>-</a><span> " & saveRequest.ListData(CalLoop).ItemCode & "</span></td><td><span>" & saveRequest.ListData(CalLoop).Description & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).Qty & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).Excl & "</span></td><td><span data-prefix></span><span>" & LineTax & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).TotalCostIncl & "</span></td></tr>"

                Next
                StrmBody.Close()
                FsBody.Close()


                TextToPrint = TextToPrint.Replace("{DataRows}", Rows)
                Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\StockAdjustments_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(filepath)
                file.WriteLine(TextToPrint)
                file.Close()

                Dim PrintFilePath As String = "/temp/StockAdjustments_" & WriteGUID & ".html"
                PrintPath = HttpContext.Current.Server.MapPath("~" & PrintFilePath)
                saveResponse.Path = PrintFilePath
                saveResponse.Message = "Saved Successfully"
                saveResponse.Success = True

            Else
                saveResponse.Success = False
                saveResponse.Message = getNumber.Message
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            saveResponse.Success = False
            saveResponse.Message = getNumber.Message
        End Try

        Return saveResponse

    End Function
    Public Sub DeleteFile(ByVal File As DeleteStockAdjustmentFile)
        'Delete a File
        If File IsNot Nothing Then
            Try
                Dim fi As New IO.FileInfo(HttpContext.Current.Server.MapPath("~" & File.Path & ""))
                fi.Delete()
            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try
        End If
    End Sub
End Class
