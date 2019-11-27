Imports pcm.DataLayer
Imports Entities.Manage
Imports Entities.Stock
Imports Entities
Imports System.IO
Public Class GRVHOBL
    Dim _DLayer As GRVHODL
    Dim _blErrorLogging As New ErrorLogBL
    Dim _DShopTransaction As New ShopTransactionsDL
    Dim RG As New CommonFunctions.clsCommon
    Dim saveResponse As New SaveResponse

    Public Sub New()
        _DLayer = New GRVHODL
    End Sub
    Public Function SearchSupplier(ByVal SearchType As String, ByVal SearchText As String) As SearchResponse
        Return _DLayer.SearchSupplier(SearchType, SearchText)
    End Function
    Public Function GetSupplierDetails(ByVal SupplierCode As String) As SupplierDetailResponse
        Return _DLayer.GetSupplierDetails(SupplierCode)
    End Function
    Public Function GetGeneratedCode(ByVal SearchText As String) As SearchResponse
        Return _DLayer.SearchCode(SearchText)
    End Function
    Public Function DoStock(grv As TransactionLineItems)
        'Return _DLayer.DoGRV(grv.GCode, grv.tQtyPositiveOnly)
        Return _DShopTransaction.ProcessLineItems(grv, "")

    End Function
    Public Sub AddHeader(ByVal transactionMaster As TransactionMaster)
        _DShopTransaction.ProcessHeader(transactionMaster, "")
    End Sub
    Public Function GetGeneratedCodeInfo(ByVal strGeneratedCodeOrBarcode As String, ByVal _strBranchCode As String) As GetGeneratedCodeInfoResponse
        Return _DLayer.GetGeneratedCodeInfo(strGeneratedCodeOrBarcode, _strBranchCode)
    End Function
    Public Function Save(ByVal saveRequest As SaveGRV) As SaveResponse
        Dim LineTax As Double
        Dim getNumber As New GetNumber
        Dim CalLoop As Integer
        Dim GRVNumber As String
        Dim tmpGUID As String
        Dim grv As New TransactionLineItems
        Dim Rows As String = String.Empty
        Dim TextToPrint As String
        Dim FullFile As String
        Dim PrintPath As String
        Dim WriteGUID As String
        WriteGUID = Guid.NewGuid.ToString
        Dim tmpTaxGroup As String
        Dim tmpTaxPercentage As String
        Dim tmpTaxArray() As String

        'Get number
        Try
            getNumber = _DLayer.GetNum("GRV", "HHH")
            If getNumber.Success = True Then
                GRVNumber = getNumber.Number
                tmpGUID = Guid.NewGuid.ToString()

                Dim STRMReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\PrintSample\GRV.html"))
                FullFile = STRMReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{TelNum}}", saveRequest.Telephone_Number)
                TextToPrint = TextToPrint.Replace("{{Date}}", Format(Now, "dd-MM-yyyy"))
                TextToPrint = TextToPrint.Replace("{{transaction}}", GRVNumber)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd1}}", saveRequest.Address1)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd2}}", saveRequest.Address2)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd3}}", saveRequest.Address3)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd4}}", saveRequest.Address4)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd5}}", saveRequest.Address5)
                TextToPrint = TextToPrint.Replace("{{totalqty}}", saveRequest.Quantity)
                TextToPrint = TextToPrint.Replace("{{reference}}", saveRequest.Reference.ToUpper())
                TextToPrint = TextToPrint.Replace("{{CurrentBranchCode}}", "HHH")
                TextToPrint = TextToPrint.Replace("{{suppliercode}}", saveRequest.SupplierAccount.ToUpper())
                TextToPrint = TextToPrint.Replace("{{suppliername}}", saveRequest.SupplierName.ToUpper())
                TextToPrint = TextToPrint.Replace("{{subtotal}}", saveRequest.TotalExcl)
                TextToPrint = TextToPrint.Replace("{{tax}}", saveRequest.TotalTAX)
                TextToPrint = TextToPrint.Replace("{{total}}", saveRequest.TotalIncl)
                TextToPrint = TextToPrint.Replace("{{FaxNum}}", saveRequest.Fax)

                'Post the header
                Dim transactionMaster As New TransactionMaster
                transactionMaster.transaction_type = "GRV"
                transactionMaster.customer_code = saveRequest.SupplierAccount.ToUpper
                transactionMaster.transaction_number = GRVNumber
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
                transactionMaster.branch_code = "HHH"
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
                    tmpTaxArray = Split(saveRequest.ListData(CalLoop).Tax, " - ")
                    tmpTaxGroup = tmpTaxArray(0)

                    Dim link_id As String = Guid.NewGuid.ToString

                    tmpTaxArray = Split(saveRequest.ListData(CalLoop).Tax, "(")
                    tmpTaxPercentage = Mid(tmpTaxArray(1), 1, Len(tmpTaxArray(1)) - 1)
                    grv.Link_GuID = tmpGUID
                    grv.master_code = saveRequest.ListData(CalLoop).MCode
                    grv.generated_code = saveRequest.ListData(CalLoop).ItemCode
                    grv.description = saveRequest.ListData(CalLoop).Description
                    grv.transsaction_type = "GRV+"
                    grv.user_name = ""
                    grv.transaction_number = GRVNumber
                    grv.rep_code = ""
                    grv.selling_exclusive = saveRequest.ListData(CalLoop).SellEx
                    grv.quantity = saveRequest.ListData(CalLoop).Qty
                    grv.tax_percentage = saveRequest.ListData(CalLoop).Tax
                    grv.supplier_account = saveRequest.ListData(CalLoop).Supplier
                    grv.supplier_item_code = saveRequest.ListData(CalLoop).SupplierCode
                    grv.is_service_item = saveRequest.ListData(CalLoop).IsServiceItem
                    grv.category_1 = saveRequest.ListData(CalLoop).Cat1
                    grv.category_2 = saveRequest.ListData(CalLoop).Cat2
                    grv.category_3 = saveRequest.ListData(CalLoop).Cat3
                    grv.size = saveRequest.ListData(CalLoop).ItemSize
                    grv.size_grid = saveRequest.ListData(CalLoop).SizeGrid
                    grv.colour = saveRequest.ListData(CalLoop).Colour
                    grv.colour_grid = saveRequest.ListData(CalLoop).ColourGrid
                    grv.cost_exclusive_average = saveRequest.ListData(CalLoop).OriginalCost
                    grv.cost_exclusive_estimated = saveRequest.ListData(CalLoop).Excl
                    grv.discount_value = ""
                    grv.discount_reason = ""
                    grv.updated_cost = ""
                    grv.Guid = link_id
                    grv.branch_code = "HHH"
                    grv.tax_percentage = tmpTaxPercentage
                    grv.tax_group = tmpTaxGroup
                    DoStock(grv)
                    'Write the Document Items
                    LineTax = Val(saveRequest.ListData(CalLoop).Incl) - Val(saveRequest.ListData(CalLoop).Excl)
                    Rows += "<tr><td><a class='cut'>-</a><span> " & saveRequest.ListData(CalLoop).ItemCode & "</span></td><td><span>" & saveRequest.ListData(CalLoop).Description & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).Qty & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).Excl & "</span></td><td><span data-prefix></span><span>" & LineTax & "</span></td><td><span data-prefix></span><span>" & saveRequest.ListData(CalLoop).TotalIncl & "</span></td></tr>"

                Next
                StrmBody.Close()
                FsBody.Close()


                TextToPrint = TextToPrint.Replace("{DataRows}", Rows)
                Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\GRV_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(filepath)
                file.WriteLine(TextToPrint)
                file.Close()

                Dim PrintFilePath As String = "/temp/GRV_" & WriteGUID & ".html"
                PrintPath = HttpContext.Current.Server.MapPath("~" & PrintFilePath)
                saveResponse.Path = PrintFilePath

                Dim labelResponse As New GetLabels
                labelResponse = PrintDispatchBarcodes(saveRequest, WriteGUID)
                If labelResponse.Success = True Then
                    saveResponse.LabelPath = labelResponse.Path
                End If

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
    Public Function GetCodes(ByVal getCode As GetCodesRequest) As GeneratedCodeInfoResponse
        Return _DLayer.GetCodes(getCode)
    End Function
    Public Function PrintDispatchBarcodes(ByVal saveDetails As SaveGRV, ByVal WriteGUID As String) As GetLabels

        Dim labelResponse As New GetLabels


        Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\LABEL_" & WriteGUID & ".txt "), FileMode.Create)
        Dim StrmBody As New StreamWriter(filepath)

        For CalLoop = 0 To saveDetails.ListData.Count - 1
            StrmBody.WriteLine()
            StrmBody.WriteLine("*CODE*: " & saveDetails.ListData(CalLoop).ItemCode)
            StrmBody.WriteLine("*DESCRIPTION*: " & Mid$(saveDetails.ListData(CalLoop).Description, 1, 30))
            StrmBody.WriteLine("*PRICE*: " & Mid$(saveDetails.ListData(CalLoop).SellEx, 1, 30))
            StrmBody.WriteLine("*0000000000000*: " & Mid$(saveDetails.ListData(CalLoop).SKUNumber, 1, 30))
            StrmBody.WriteLine("!@#$%^&*: " & "P " & (saveDetails.ListData(CalLoop).Qty))
        Next
        StrmBody.Close()
        filepath.Close()


        Dim path As String = HttpContext.Current.Server.MapPath("~\temp\LABEL_" & WriteGUID & ".txt ")
        labelResponse.Path = "LABEL_" & WriteGUID & ".txt"

        labelResponse.Success = True



        Return labelResponse


    End Function
    Public Sub DeleteFile(ByVal Files As DeleteGRVFiles)
        'Delete a File
        If Files IsNot Nothing Then
            Try
                Dim TextToPrint() As String = Files.Path.Split("/")
                Dim fi As New IO.FileInfo(HttpContext.Current.Server.MapPath("~\temp\" & TextToPrint(2) & ""))
                fi.Delete()

                ''Delete Labels
                Dim label As New IO.FileInfo(HttpContext.Current.Server.MapPath("~\temp\" & Files.LabelPath & ""))
                label.Delete()

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)

            End Try
        End If


    End Sub
End Class
