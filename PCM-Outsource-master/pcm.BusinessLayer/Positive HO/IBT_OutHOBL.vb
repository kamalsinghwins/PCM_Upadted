Imports pcm.DataLayer
Imports Entities.IBT_Out
Imports Entities
Imports System.IO
Imports System.Web




Public Class IBT_OutHOBL
    Dim _DLayer As IBT_OutHODL
    Dim _DShopTransaction As New ShopTransactionsDL
    Dim _blErrorLogging As New ErrorLogBL
    Dim RG As New CommonFunctions.clsCommon
    Dim reloadResponse As New ReloadResponse
    Dim saveResponse As New SaveResponse
    Dim baseresponse As New BaseResponse



    Public Sub New()
        _DLayer = New IBT_OutHODL
    End Sub
    Public Function GetBranchDetails(ByVal _BranchCode As String) As BranchDetailResponse
        Return _DLayer.GetBranchDetails(_BranchCode)
    End Function

    Public Function GetGeneratedCodeInfo(ByVal _strGeneratedCodeOrBarcode As String, ByVal _strBranchCode As String) As GetGeneratedCodeInfoResponse
        Return _DLayer.GetGeneratedCodeInfo(_strGeneratedCodeOrBarcode, _strBranchCode)
    End Function

    Public Function SearchBranch(ByVal _searchType As String, ByVal _searchDetail As String) As SearchBranchResponse
        Return _DLayer.SearchBranch(_searchType, _searchDetail)
    End Function
    Public Function Searchicibtout(ByVal _searchText As String, ByVal _searchType As String, ByVal _master As Boolean) As SearchResponse
        Return _DLayer.Searchicibtout(_searchText, _searchType, _master)
    End Function

    Public Function GetTaxGroups() As GetTaxResponse
        Return _DLayer.GetTaxGroups()

    End Function

    Public Function GetBranchSettings(ByVal Current_Branch_Code As String) As GetBranchSettingsResponse
        Return _DLayer.GetBranchSettings(Current_Branch_Code)

    End Function

    Public Function GetCompanySettings() As GetCompanyResponse
        Return _DLayer.GetCompanySettings()

    End Function

    Public Sub AddHeader(ByVal transactionMaster As TransactionMaster)

        _DShopTransaction.ProcessHeader(transactionMaster, "")

    End Sub

    Public Function DoStock(transactionLineItems As TransactionLineItems)
        Return _DShopTransaction.ProcessLineItems(transactionLineItems, "")

    End Function
    Public Function InsertIBTTransactions(ByVal TransactionData As IBTTransactionData) As String

        Return _DShopTransaction.InsertIBTTransactions(TransactionData, "")

    End Function

    Public Function Save(ByVal saveDetails As Save) As SaveResponse
        Dim LineTax As Double
        Dim tmpGUID As String
        Dim tmpTaxGroup As String
        Dim tmpTaxPercentage As String
        Dim tmpTaxArray() As String
        Dim IBTOutNumber As String
        Dim CalLoop As Integer
        Dim GetGUID As String
        Dim PrintPath As String
        Dim getIbt_OutNumber As New GetIbt_OutNumber
        Dim WriteGUID As String

        Dim Rows As String = String.Empty
        Dim TextToPrint As String
        Dim FullFile As String



        Try
            getIbt_OutNumber = _DLayer.GetNum("IBTOUT", "HHH")
            If getIbt_OutNumber.Success = True Then
                IBTOutNumber = getIbt_OutNumber.Number

                Dim STRMReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\PrintSample\PrintReport.html"))
                FullFile = STRMReader.ReadToEnd
                TextToPrint = FullFile
                TextToPrint = TextToPrint.Replace("{{TelNum}}", saveDetails.Branch_Telephone_Number)
                TextToPrint = TextToPrint.Replace("{{Date}}", Format(Now, "dd-MM-yyyy"))
                TextToPrint = TextToPrint.Replace("{{transaction}}", IBTOutNumber)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd1}}", saveDetails.Address1)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd2}}", saveDetails.Address2)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd3}}", saveDetails.Address3)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd4}}", saveDetails.Address4)
                TextToPrint = TextToPrint.Replace("{{DeliverAdd5}}", saveDetails.Address5)
                TextToPrint = TextToPrint.Replace("{{totalqtysent}}", saveDetails.Quantity)
                TextToPrint = TextToPrint.Replace("{{IBTOut#}}", saveDetails.Reference.ToUpper())
                TextToPrint = TextToPrint.Replace("{{CurrentBranchCode}}", "HHH")
                TextToPrint = TextToPrint.Replace("{{branchcode}}", saveDetails.BranchCode.ToUpper())
                TextToPrint = TextToPrint.Replace("{{branchname}}", saveDetails.BranchName.ToUpper())
                TextToPrint = TextToPrint.Replace("{{totalqtyreceieved}}", "_______________")
                TextToPrint = TextToPrint.Replace("{{Deliver To :}}", "Deliver To :")

                tmpGUID = _DLayer.GenerateGUID()
                WriteGUID = _DLayer.GenerateGUID()


                Dim transactionMaster As New TransactionMaster
                transactionMaster.transaction_type = "IBTOUT"
                transactionMaster.transaction_number = IBTOutNumber
                transactionMaster.reference_number = saveDetails.Reference.ToUpper()
                transactionMaster.user_name = saveDetails.Current_User
                transactionMaster.customer_code = saveDetails.BranchCode.ToUpper()
                transactionMaster.rep_code = ""
                transactionMaster.number_of_items = saveDetails.Quantity
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


                AddHeader(transactionMaster)

                Dim FsBody As New FileStream(HttpContext.Current.Server.MapPath("~\temp\" & "LASTIBTOUT_" & saveDetails.Current_User & ".txt "), FileMode.Create)

                Dim StrmBody As New StreamWriter(FsBody)

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
                    transactionLineItems.cost_exclusive_estimated = saveDetails.ListData(CalLoop).Excl
                    transactionLineItems.cost_exclusive_average = saveDetails.ListData(CalLoop).OriginalCost
                    transactionLineItems.selling_exclusive = saveDetails.ListData(CalLoop).SellEx


                    transactionLineItems.discount_value = ""
                    transactionLineItems.discount_reason = ""
                    transactionLineItems.quantity = saveDetails.ListData(CalLoop).Qty
                    transactionLineItems.size = saveDetails.ListData(CalLoop).ItemSize
                    transactionLineItems.size_grid = saveDetails.ListData(CalLoop).SizeGrid


                    transactionLineItems.colour = saveDetails.ListData(CalLoop).Colour
                    transactionLineItems.colour_grid = saveDetails.ListData(CalLoop).ColourGrid
                    transactionLineItems.is_service_item = saveDetails.ListData(CalLoop).IsServiceItem
                    transactionLineItems.branch_code = "HHH"
                    transactionLineItems.transaction_number = IBTOutNumber

                    transactionLineItems.transsaction_type = "IBTOUT"

                    transactionLineItems.updated_cost = ""

                    GetGUID = DoStock(transactionLineItems)



                    Dim iBTTransactionData As New IBTTransactionData
                    iBTTransactionData.Guid = link_id
                    iBTTransactionData.sending_branch_code = "HHH"
                    iBTTransactionData.sending_date = Format(Now, "yyyy-MM-dd")
                    iBTTransactionData.transaction_number = IBTOutNumber
                    iBTTransactionData.receiving_branch_code = RG.Apos((saveDetails.BranchCode).ToUpper())
                    iBTTransactionData.generated_code = saveDetails.ListData(CalLoop).ItemCode.ToUpper
                    iBTTransactionData.sent_qty = RG.Numb(saveDetails.ListData(CalLoop).Qty)
                    iBTTransactionData.selling_tax_group = saveDetails.ListData(CalLoop).SellingTaxGroup
                    InsertIBTTransactions(iBTTransactionData)


                    '    'Write the Document Items
                    LineTax = Val(saveDetails.ListData(CalLoop).Incl) - Val(saveDetails.ListData(CalLoop).Excl)


                    '  Print Report 

                    Rows += "<tr><td><a class='cut'>-</a><span> " & saveDetails.ListData(CalLoop).ItemCode & "</span></td><td><span>" & saveDetails.ListData(CalLoop).Description & "</span></td><td><span data-prefix></span><span>" & saveDetails.ListData(CalLoop).Qty & "</span></td><td><span>_______________</span></td></tr>"

                    StrmBody.WriteLine(saveDetails.ListData(CalLoop).ItemCode & "," & saveDetails.ListData(CalLoop).Description &
                                           "," & saveDetails.ListData(CalLoop).Excl & "," & saveDetails.ListData(CalLoop).Tax &
                                           "," & saveDetails.ListData(CalLoop).Incl & "," & saveDetails.ListData(CalLoop).Qty &
                                           "," & saveDetails.ListData(CalLoop).TotalCostIncl & "," & saveDetails.ListData(CalLoop).MCode &
                                           "," & saveDetails.ListData(CalLoop).Cat1 & "," & saveDetails.ListData(CalLoop).Cat2 &
                                           "," & saveDetails.ListData(CalLoop).Cat3 & "," & saveDetails.ListData(CalLoop).ItemSize &
                                           "," & saveDetails.ListData(CalLoop).SizeGrid & "," & saveDetails.ListData(CalLoop).Colour &
                                           "," & saveDetails.ListData(CalLoop).ColourGrid & "," & saveDetails.ListData(CalLoop).OriginalCost &
                                           "," & saveDetails.ListData(CalLoop).Supplier & "," & saveDetails.ListData(CalLoop).SupplierCode &
                                           "," & saveDetails.ListData(CalLoop).IsServiceItem & "," & saveDetails.ListData(CalLoop).SellEx &
                                           "," & saveDetails.ListData(CalLoop).SellingTaxGroup & "," & saveDetails.ListData(CalLoop).SKUNumber &
                                           "," & saveDetails.ListData(CalLoop).SellingIncl)
                Next

                StrmBody.Close()
                FsBody.Close()


                TextToPrint = TextToPrint.Replace("{DataRows}", Rows)
                Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\PrintDocument_" & WriteGUID & ".html  "), FileMode.Create)
                Dim file As New System.IO.StreamWriter(filepath)
                file.WriteLine(TextToPrint)
                file.Close()

                Dim PrintFilePath As String = "/temp/PrintDocument_" & WriteGUID & ".html"
                PrintPath = HttpContext.Current.Server.MapPath("~" & PrintFilePath)



                Dim labelResponse As New GetLabels
                labelResponse = PrintDispatchBarcodes(IBTOutNumber, saveDetails, WriteGUID)
                If labelResponse.Success = True Then
                    saveResponse.LabelPath = labelResponse.Path
                End If

                saveResponse.Path = PrintFilePath
                saveResponse.Message = "Saved Successfully"
                saveResponse.Success = True

            Else
                saveResponse.Success = False
                saveResponse.Message = getIbt_OutNumber.Message
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            saveResponse.Success = False
            saveResponse.Message = "Something went wrong"
        End Try


        Return saveResponse
    End Function

    Public Function Reload(ByVal user As String) As ReloadResponse

        Dim Details() As String
        Dim list As New List(Of ListData)
        Dim filePath As String
        filePath = HttpContext.Current.Server.MapPath("~\temp\LASTIBTOUT_" & user & ".txt")
        Try
            If System.IO.File.Exists(filePath) Then

                Using sr As New StreamReader(filePath)
                    Dim line As String
                    ' Read and display lines from the file until the end of
                    ' the file is reached.
                    Do
                        line = sr.ReadLine()
                        If Not (line Is Nothing) Then
                            Details = line.Split(",")
                            Dim listData As New ListData
                            listData.ItemCode = Details(0)
                            listData.Description = (Details(1))
                            listData.Excl = (Details(2))
                            listData.Tax = (Details(3))
                            listData.Incl = (Details(4))
                            listData.Qty = (Details(5))
                            listData.TotalCostIncl = (Details(6))
                            listData.MCode = (Details(7))
                            listData.Cat1 = (Details(8))
                            listData.Cat2 = (Details(9))
                            listData.Cat3 = (Details(10))
                            listData.ItemSize = (Details(11))
                            listData.SizeGrid = (Details(12))
                            listData.Colour = (Details(13))
                            listData.ColourGrid = (Details(14))
                            listData.OriginalCost = (Details(15))
                            listData.Supplier = (Details(16))
                            listData.SupplierCode = (Details(17))
                            listData.IsServiceItem = (Details(18))
                            listData.SellEx = (Details(19))
                            listData.SellingTaxGroup = (Details(20))
                            listData.SKUNumber = (Details(21))
                            listData.SellingIncl = (Details(22))

                            list.Add(listData)


                        End If
                    Loop Until line Is Nothing
                End Using
                reloadResponse.Success = True
                reloadResponse.ListData = list
            Else
                reloadResponse.Success = False
                reloadResponse.Message = "The file does not exist"

            End If


        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            reloadResponse.Success = False
            reloadResponse.Message = "The file could not be read:"
        End Try




        Return reloadResponse

    End Function

    Public Function PrintDispatchBarcodes(ByVal IBTNumber As String, ByVal saveDetails As Save, ByVal WriteGUID As String) As GetLabels

        Dim labelResponse As New GetLabels
        Dim FullFile As String
        Dim TextToPrint As String


        Dim IsfileExists As System.IO.FileInfo = New System.IO.FileInfo(HttpContext.Current.Server.MapPath("~\temp\Warehouse\warehouse.prn"))

        If IsfileExists.Exists Then
            Dim STRMReader As New StreamReader(HttpContext.Current.Server.MapPath("~\temp\Warehouse\warehouse.prn"))
            FullFile = STRMReader.ReadToEnd

            If FullFile = "" Then
                labelResponse.Success = False
                labelResponse.Message = "The Barcode Label file is empty. Please make sure you have a valid Barcocde Label file in order to print Barcode Labels."
                Return labelResponse
            End If


            TextToPrint = FullFile
            TextToPrint = TextToPrint.Replace("*STYLE*", saveDetails.BoxStyle)
            TextToPrint = TextToPrint.Replace("*BOXTYPE*", "NA")
            TextToPrint = TextToPrint.Replace("*BRANCH*", Mid$(saveDetails.BranchName, 1, 30))
            TextToPrint = TextToPrint.Replace("*ADDRESS1*", Mid$(saveDetails.Address1, 1, 30))
            TextToPrint = TextToPrint.Replace("*ADDRESS2*", Mid$(saveDetails.Address2, 1, 30))
            TextToPrint = TextToPrint.Replace("*ADDRESS3*", Mid$(saveDetails.Address3, 1, 30))
            TextToPrint = TextToPrint.Replace("*ADDRESS4*", Mid$(saveDetails.Address4, 1, 30))
            TextToPrint = TextToPrint.Replace("*ADDRESS5*", Mid$(saveDetails.Address5, 1, 30))
            TextToPrint = TextToPrint.Replace("*BAY*", "NA")

            TextToPrint = TextToPrint.Replace("*BARCODE*", IBTNumber)
            TextToPrint = TextToPrint.Replace("*IBTNUMBER*", IBTNumber)

            TextToPrint = TextToPrint.Replace("!@#$%^&*", "P1")

            Dim filepath As New FileStream(HttpContext.Current.Server.MapPath("~\temp\LABEL_" & WriteGUID & ".txt "), FileMode.Create)
            Dim file As New System.IO.StreamWriter(filepath)
            file.WriteLine(TextToPrint)
            file.Close()


            Dim path As String = HttpContext.Current.Server.MapPath("~\temp\LABEL_" & WriteGUID & ".txt ")
            labelResponse.Path = "LABEL_" & WriteGUID & ".txt"

            labelResponse.Success = True

        Else
            labelResponse.Success = False
            labelResponse.Message = "The barcode label file doesn't exist. Please make sure that the 'warehouse.prn' file exists "
        End If

        Return labelResponse

    End Function

    Public Function GetCodes(ByVal getCode As GetCodes) As GetGeneratedCodeResponse
        Return _DLayer.GetCodes(getCode)
    End Function

    Public Sub DeleteFile(ByVal Files As DeleteFiles)
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
