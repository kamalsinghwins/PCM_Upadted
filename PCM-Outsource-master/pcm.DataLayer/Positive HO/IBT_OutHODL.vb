Imports System.IO
Imports Entities.IBT_Out
Imports Npgsql
Public Class IBT_OutHODL
    Inherits DataAccessLayerBase

    Dim searchResponse As New SearchResponse
    Dim generatedCodeInfoResponse As New GetGeneratedCodeInfoResponse
    Dim branchDetailResponse As New BranchDetailResponse
    Dim getIbt_OutNumber As New GetIbt_OutNumber
    Dim getTaxResponse As New GetTaxResponse
    Dim getCompanyResponse As New GetCompanyResponse
    Dim getBranchSettingsResponse As New GetBranchSettingsResponse
    Dim searchBranchResponse As New SearchBranchResponse
    Dim getGeneratedCodeResponse As New GetGeneratedCodeResponse

    Public Function GetBranchDetails(ByVal BrachCode As String) As BranchDetailResponse

        Try

            tmpSQL = "SELECT branch_name,
                      address_line_1,
                      address_line_2,
                      address_line_3,
                      address_line_4,
                      address_line_5,
                      telephone_number,
                      fax_number,
                      tax_number,
                      is_blocked " & "
                      FROM branch_details WHERE branch_code = '" & RG.Apos(BrachCode).ToUpper() & "'"

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) And Ds.Tables.Count > 0 And Ds.Tables(0).Rows.Count > 0 Then
                branchDetailResponse.BranchName = Ds.Tables(0).Rows(0)("branch_name").ToString
                branchDetailResponse.AddressLine1 = Ds.Tables(0).Rows(0)("address_line_1").ToString
                branchDetailResponse.AddressLine2 = Ds.Tables(0).Rows(0)("address_line_2").ToString
                branchDetailResponse.AddressLine3 = Ds.Tables(0).Rows(0)("address_line_3").ToString
                branchDetailResponse.AddressLine4 = Ds.Tables(0).Rows(0)("address_line_4").ToString
                branchDetailResponse.AddressLine5 = Ds.Tables(0).Rows(0)("address_line_5").ToString
                branchDetailResponse.TelephoneNumber = Ds.Tables(0).Rows(0)("telephone_number").ToString
                branchDetailResponse.FaxNumber = Ds.Tables(0).Rows(0)("fax_number").ToString
                branchDetailResponse.TaxNumber = Ds.Tables(0).Rows(0)("tax_number").ToString
                branchDetailResponse.isBlocked = Ds.Tables(0).Rows(0)("is_blocked").ToString
                branchDetailResponse.Success = True


            Else
                branchDetailResponse.Success = False
                branchDetailResponse.Message = "This Branch Account does not exist"

            End If
            Return branchDetailResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetGeneratedCodeInfo(ByVal strGeneratedCodeOrBarcode As String, ByVal _strBranchCode As String) As GetGeneratedCodeInfoResponse

        tmpSQL = "SELECT " &
                 "stockcodes_master.master_code,stockcodes_master.generated_code,stockcodes_master.sku_number,stockcodes_master.is_service_item," &
                 "stockcodes_master.description,stockcodes_master.purchase_tax_group,stockcodes_master.sales_tax_group,stockcodes_master.supplier," &
                 "stockcodes_master.suppliers_code,stockcodes_master.is_not_discountable,stockcodes_master.is_blocked," &
                 "stockcodes_master.minimum_stock_level,stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix," &
                 "stockcodes_master.colour_matrix,stockcodes_categories.category_1,stockcodes_categories.category_2,stockcodes_categories.category_3," &
                 "stockcodes_dates.updated,stockcodes_prices.selling_price_3,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_1," &
                 "stockcodes_prices.estimated_cost,stockcodes_prices.cost_price,stock_on_hand.qty_on_hand " &
                 "FROM " &
                 "stockcodes_master " &
                 "LEFT OUTER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                 "LEFT OUTER JOIN stockcodes_dates ON stockcodes_master.generated_code = stockcodes_dates.generated_code " &
                 "LEFT OUTER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                 "LEFT OUTER JOIN stock_on_hand ON stockcodes_master.generated_code = stock_on_hand.generated_code " &
                 "WHERE stockcodes_master.generated_code = '" & RG.Apos(strGeneratedCodeOrBarcode).ToUpper() & "' " &
                 "AND stock_on_hand.branch_code = '" & RG.Apos(_strBranchCode).ToUpper() & "'"

        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                generatedCodeInfoResponse.CodeMasterCodeSTR = Ds.Tables(0).Rows(0)("master_code").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeGeneratedCodeSTR = Ds.Tables(0).Rows(0)("generated_code").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeBarcodeSTR = Ds.Tables(0).Rows(0)("sku_number").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeDescriptionSTR = Ds.Tables(0).Rows(0)("description").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodePurchaseTaxGroupSTR = Ds.Tables(0).Rows(0)("purchase_tax_group") & ""
                generatedCodeInfoResponse.CodeSalesTaxGroupSTR = Ds.Tables(0).Rows(0)("sales_tax_group") & ""

                generatedCodeInfoResponse.CodeCategory1STR = Ds.Tables(0).Rows(0)("category_1").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeCategory2STR = Ds.Tables(0).Rows(0)("category_2").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeCategory3STR = Ds.Tables(0).Rows(0)("category_3").ToString.ToUpper & ""

                generatedCodeInfoResponse.CodeIsServiceItemBOOL = Ds.Tables(0).Rows(0)("is_service_item") & ""
                generatedCodeInfoResponse.CodeIsDiscountableBOOL = Ds.Tables(0).Rows(0)("is_not_discountable") & ""
                generatedCodeInfoResponse.CodeIsBlockedBOOL = Ds.Tables(0).Rows(0)("is_blocked") & ""

                generatedCodeInfoResponse.CodeSupplierAccountSTR = Ds.Tables(0).Rows(0)("supplier").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeSupplierItemCodeSTR = Ds.Tables(0).Rows(0)("suppliers_code").ToString.ToUpper & ""

                generatedCodeInfoResponse.CodeMinimumStockLevelDBL = If(Ds.Tables(0).Rows(0)("minimum_stock_level").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("minimum_stock_level")))

                generatedCodeInfoResponse.CodeCostAverageExclusiveDBL = If(Ds.Tables(0).Rows(0)("cost_price").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("cost_price")))
                generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL = If(Ds.Tables(0).Rows(0)("estimated_cost").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("estimated_cost")))
                generatedCodeInfoResponse.CodeSellingExlusive1DBL = If(Ds.Tables(0).Rows(0)("selling_price_1").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_1")))
                generatedCodeInfoResponse.CodeSellingExlusive2DBL = If(Ds.Tables(0).Rows(0)("selling_price_2").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_2")))
                generatedCodeInfoResponse.CodeSellingExlusive3DBL = If(Ds.Tables(0).Rows(0)("selling_price_3").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_3")))

                generatedCodeInfoResponse.CodeItemColourSTR = Ds.Tables(0).Rows(0)("item_colour").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeItemSizeSTR = Ds.Tables(0).Rows(0)("item_size").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeSizeMatrixSTR = Ds.Tables(0).Rows(0)("size_matrix").ToString.ToUpper & ""
                generatedCodeInfoResponse.CodeColourMatrixSTR = Ds.Tables(0).Rows(0)("colour_matrix").ToString.ToUpper & ""

                generatedCodeInfoResponse.CodeQuantityOnHand = If(Ds.Tables(0).Rows(0)("qty_on_hand").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("qty_on_hand")))
            Else
                'Check for barcode

                tmpSQL = "SELECT " &
                     "stockcodes_master.master_code,stockcodes_master.generated_code,stockcodes_master.sku_number,stockcodes_master.is_service_item," &
                     "stockcodes_master.description,stockcodes_master.purchase_tax_group,stockcodes_master.sales_tax_group,stockcodes_master.supplier," &
                     "stockcodes_master.suppliers_code,stockcodes_master.is_not_discountable,stockcodes_master.is_blocked," &
                     "stockcodes_master.minimum_stock_level,stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix," &
                     "stockcodes_master.colour_matrix,stockcodes_categories.category_1,stockcodes_categories.category_2,stockcodes_categories.category_3," &
                     "stockcodes_dates.updated,stockcodes_prices.selling_price_3,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_1," &
                     "stockcodes_prices.estimated_cost,stockcodes_prices.cost_price,stock_on_hand.qty_on_hand " &
                     "FROM " &
                     "stockcodes_master " &
                     "LEFT OUTER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN stockcodes_dates ON stockcodes_master.generated_code = stockcodes_dates.generated_code " &
                     "LEFT OUTER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                     "LEFT OUTER JOIN stock_on_hand ON stockcodes_master.generated_code = stock_on_hand.generated_code " &
                     "WHERE stockcodes_master.sku_number = '" & RG.Apos(strGeneratedCodeOrBarcode).ToUpper() & "' " &
                     "AND stock_on_hand.branch_code ='" & RG.Apos(_strBranchCode).ToUpper() & "'"

                Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(Ds) Then
                    'Exists as barcode
                    generatedCodeInfoResponse.CodeMasterCodeSTR = Ds.Tables(0).Rows(0)("master_code").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeGeneratedCodeSTR = Ds.Tables(0).Rows(0)("generated_code").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeBarcodeSTR = Ds.Tables(0).Rows(0)("sku_number").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeDescriptionSTR = Ds.Tables(0).Rows(0)("description").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodePurchaseTaxGroupSTR = Ds.Tables(0).Rows(0)("purchase_tax_group") & ""
                    generatedCodeInfoResponse.CodeSalesTaxGroupSTR = Ds.Tables(0).Rows(0)("sales_tax_group") & ""

                    generatedCodeInfoResponse.CodeCategory1STR = Ds.Tables(0).Rows(0)("category_1").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeCategory2STR = Ds.Tables(0).Rows(0)("category_2").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeCategory3STR = Ds.Tables(0).Rows(0)("category_3").ToString.ToUpper & ""

                    generatedCodeInfoResponse.CodeIsServiceItemBOOL = Ds.Tables(0).Rows(0)("is_service_item") & ""
                    generatedCodeInfoResponse.CodeIsDiscountableBOOL = Ds.Tables(0).Rows(0)("is_not_discountable") & ""
                    generatedCodeInfoResponse.CodeIsBlockedBOOL = Ds.Tables(0).Rows(0)("is_blocked") & ""

                    generatedCodeInfoResponse.CodeSupplierAccountSTR = Ds.Tables(0).Rows(0)("supplier").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeSupplierItemCodeSTR = Ds.Tables(0).Rows(0)("suppliers_code").ToString.ToUpper & ""

                    generatedCodeInfoResponse.CodeMinimumStockLevelDBL = If(Ds.Tables(0).Rows(0)("minimum_stock_level").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("minimum_stock_level")))

                    generatedCodeInfoResponse.CodeCostAverageExclusiveDBL = If(Ds.Tables(0).Rows(0)("cost_price").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("cost_price")))
                    generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL = If(Ds.Tables(0).Rows(0)("estimated_cost").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("estimated_cost")))
                    generatedCodeInfoResponse.CodeSellingExlusive1DBL = If(Ds.Tables(0).Rows(0)("selling_price_1").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_1")))
                    generatedCodeInfoResponse.CodeSellingExlusive2DBL = If(Ds.Tables(0).Rows(0)("selling_price_2").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_2")))
                    generatedCodeInfoResponse.CodeSellingExlusive3DBL = If(Ds.Tables(0).Rows(0)("selling_price_3").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_3")))

                    generatedCodeInfoResponse.CodeItemColourSTR = Ds.Tables(0).Rows(0)("item_colour").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeItemSizeSTR = Ds.Tables(0).Rows(0)("item_size").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeSizeMatrixSTR = Ds.Tables(0).Rows(0)("size_matrix").ToString.ToUpper & ""
                    generatedCodeInfoResponse.CodeColourMatrixSTR = Ds.Tables(0).Rows(0)("colour_matrix").ToString.ToUpper & ""

                    generatedCodeInfoResponse.CodeQuantityOnHand = If(Ds.Tables(0).Rows(0)("qty_on_hand").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("qty_on_hand")))
                Else
                    generatedCodeInfoResponse.CodeMasterCodeSTR = ""
                    generatedCodeInfoResponse.CodeGeneratedCodeSTR = ""
                    generatedCodeInfoResponse.CodeBarcodeSTR = ""
                    generatedCodeInfoResponse.CodeDescriptionSTR = ""
                    generatedCodeInfoResponse.CodePurchaseTaxGroupSTR = ""
                    generatedCodeInfoResponse.CodeSalesTaxGroupSTR = ""

                    generatedCodeInfoResponse.CodeCategory1STR = ""
                    generatedCodeInfoResponse.CodeCategory2STR = ""
                    generatedCodeInfoResponse.CodeCategory3STR = ""

                    generatedCodeInfoResponse.CodeIsServiceItemBOOL = False
                    generatedCodeInfoResponse.CodeIsDiscountableBOOL = False
                    generatedCodeInfoResponse.CodeIsBlockedBOOL = False

                    generatedCodeInfoResponse.CodeSupplierAccountSTR = ""
                    generatedCodeInfoResponse.CodeSupplierItemCodeSTR = ""

                    generatedCodeInfoResponse.CodeMinimumStockLevelDBL = 0

                    generatedCodeInfoResponse.CodeCostAverageExclusiveDBL = 0
                    generatedCodeInfoResponse.CodeEstimatedCostExclusiveDBL = 0
                    generatedCodeInfoResponse.CodeSellingExlusive1DBL = 0
                    generatedCodeInfoResponse.CodeSellingExlusive2DBL = 0
                    generatedCodeInfoResponse.CodeSellingExlusive3DBL = 0

                    generatedCodeInfoResponse.CodeItemColourSTR = ""
                    generatedCodeInfoResponse.CodeItemSizeSTR = ""
                    generatedCodeInfoResponse.CodeSizeMatrixSTR = ""
                    generatedCodeInfoResponse.CodeColourMatrixSTR = ""

                    generatedCodeInfoResponse.CodeQuantityOnHand = 0
                    generatedCodeInfoResponse.GetGeneratedCodeInfo = False
                    Return generatedCodeInfoResponse
                End If
            End If
        Catch ex As Exception
            Throw ex

        End Try

        generatedCodeInfoResponse.GetGeneratedCodeInfo = True
        Return generatedCodeInfoResponse
    End Function

    Public Function SearchBranch(ByVal SearchType As String, ByVal SearchText As String) As SearchBranchResponse

        If SearchType = "Branch Code" Then
            tmpSQL = "SELECT branch_code,branch_name FROM branch_details WHERE branch_code ILIKE '" & RG.Apos(SearchText) & "%' ORDER BY branch_code ASC LIMIT 50"
        ElseIf SearchType = "Branch Name" Then
            tmpSQL = "SELECT branch_code,branch_name FROM branch_details WHERE branch_name ILIKE '" & RG.Apos(SearchText) & "%' ORDER BY branch_name ASC LIMIT 50"
        End If

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                searchBranchResponse.searchBranchList = Ds.Tables(0)
                searchBranchResponse.Success = True

            Else
                searchBranchResponse.Success = False
                searchBranchResponse.Message = "No Data to display"
            End If
            Return searchBranchResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function Searchicibtout(ByVal SearchText As String, ByVal SearchType As String, ByVal Master As Boolean) As SearchResponse

        If Master = True Then
            If SearchType = "Generated Code" Then
                searchResponse.Success = False
                searchResponse.Message = "You cannot search by Generated Code when a Master Code search has been selected."
                Return searchResponse
                Exit Function

            End If

            If SearchType = "Master Code" Then
                tmpSQL = "SELECT master_code as code,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE master_code LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50"
            ElseIf SearchType = "Barcode" Then
                tmpSQL = "SELECT master_code as code,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE sku_number LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50 "
            ElseIf SearchType = "Description" Then
                tmpSQL = "SELECT master_code as code,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE description LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50 "
            End If

        Else
            If SearchType = "Master Code" Then
                searchResponse.Success = False
                searchResponse.Message = "You cannot search by Master Code when a Generated Code search has been selected"
                Return searchResponse
                Exit Function

            End If


            If SearchType = "Generated Code" Then
                tmpSQL = "SELECT generated_code as code,description,sku_number FROM stockcodes_master WHERE generated_code LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY generated_code ASC LIMIT 50 "
            ElseIf SearchType = "Barcode" Then
                tmpSQL = "SELECT generated_code as code,description,sku_number FROM stockcodes_master WHERE sku_number LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY sku_number ASC LIMIT 50 "
            ElseIf SearchType = "Description" Then
                tmpSQL = "SELECT generated_code as code,description,sku_number FROM stockcodes_master WHERE description LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY description ASC LIMIT 50 "
            End If
        End If

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                searchResponse.dt = Ds.Tables(0)
                searchResponse.Success = True

            Else
                searchResponse.dt = Nothing
            End If
            searchResponse.Success = True
            Return searchResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetTaxGroups() As GetTaxResponse
        Dim TaxGroup(11) As String
        Dim TaxDescript(11) As String
        Dim TaxRate(11) As String

        tmpSQL = "SELECT *,tax_group || ' - ' || tax_description || '(' ||  tax_value || ')' as ""TaxInfo"" FROM tax_groups ORDER BY tax_group ASC"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getTaxResponse.dt = Ds.Tables(0)


                For Each dr As DataRow In Ds.Tables(0).Rows
                    If Val(dr("tax_group")) <> 0 Then
                        TaxGroup(Val(dr("tax_group"))) = dr("tax_group") & ""
                        TaxDescript(Val(dr("tax_group"))) = dr("tax_description") & ""
                        TaxRate(Val(dr("tax_group"))) = dr("tax_value") & ""
                    End If
                Next
            Else
                getTaxResponse.Message = "Error retrieving Tax Groups."
                getTaxResponse.GetTaxGroups = False
                getTaxResponse.Success = False
                Return getTaxResponse
                Exit Function
            End If
            getTaxResponse.Success = True
            getTaxResponse.GetTaxGroups = True
            Return getTaxResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetCompanySettings() As GetCompanyResponse
        tmpSQL = "SELECT * FROM company_details"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getCompanyResponse.dt = Ds.Tables(0)
            Else
                getCompanyResponse.Message = "Error retrieving Company Settings."
                getCompanyResponse.Success = False
                getCompanyResponse.GetCompanySettings = False
                Return getCompanyResponse
                Exit Function
            End If
            getCompanyResponse.Success = True
            getCompanyResponse.GetCompanySettings = True
            Return getCompanyResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBranchSettings(ByVal BranchCode As String) As GetBranchSettingsResponse
        tmpSQL = "SELECT * FROM branch_details WHERE branch_code = '" & RG.Apos(BranchCode).ToUpper() & "'"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getBranchSettingsResponse.dt = Ds.Tables(0)
                If Ds.Tables(0).Rows(0)("is_blocked") & "" = True Then
                    getBranchSettingsResponse.Message = "The Branch you have selected is Blocked."
                    getBranchSettingsResponse.GetBranchSettings = False
                    Return getBranchSettingsResponse
                    Exit Function
                End If
            Else
                getBranchSettingsResponse.Message = "Error retrieving Company Settings."
                getBranchSettingsResponse.Success = False
                getBranchSettingsResponse.GetBranchSettings = False
                Return getBranchSettingsResponse
                Exit Function
            End If
            getBranchSettingsResponse.Success = True
            getBranchSettingsResponse.GetBranchSettings = True
            Return getBranchSettingsResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetNum(ByVal TransactionType As String, ByVal Current_Branch_Code As String) As GetIbt_OutNumber

        Select Case TransactionType
            Case "IBTIN"
                tmpSQL = "SELECT nextval('" & Current_Branch_Code.ToUpper() & "_ibtin_seq')"
            Case "IBTOUT"
                tmpSQL = "SELECT nextval('" & Current_Branch_Code.ToUpper() & "_ibtout_seq')"
            Case "GRV"
                tmpSQL = "SELECT nextval('" & Current_Branch_Code.ToUpper() & "_grv_seq')"
            Case "STKADJ"
                tmpSQL = "SELECT nextval('" & Current_Branch_Code.ToUpper() & "_stkadj_seq')"
        End Select

        Try


            Ds = usingObjDB.GetDataSet(_POSWriteConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getIbt_OutNumber.Number = Ds.Tables(0).Rows(0)("nextval") & ""
            Else

                getIbt_OutNumber.Message = "Error retrieving number."
                getIbt_OutNumber.Success = False
                Return getIbt_OutNumber
            End If

        Catch ex As Exception
            Throw ex
        End Try
        getIbt_OutNumber.Success = True
        Return getIbt_OutNumber

    End Function


    Public Function GenerateGUID() As String
        GenerateGUID = Guid.NewGuid.ToString

    End Function



    Public Function GetCodes(ByVal getCode As GetCodes) As GetGeneratedCodeResponse


        tmpSQL = "SELECT count(generated_code) FROM stockcodes_master WHERE master_code = '" & RG.Apos(getCode.MasterCode) & "'"

        Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        If usingObjDB.isR(Ds) Then
            getGeneratedCodeResponse.Count = Ds.Tables(0).Rows(0)("count") & ""

        Else
            getGeneratedCodeResponse.Success = False
            getGeneratedCodeResponse.Message = "No generated codes found"
            Return getGeneratedCodeResponse
        End If

        '================================================================================
        'Get the TAX percentage
        Dim tmpTaxArray() As String
        Dim tmpInTaxValue As String

        tmpTaxArray = Split(getCode.TaxGroup, "(")
        tmpInTaxValue = Val(Mid$(tmpTaxArray(1), 1, Len(tmpTaxArray(1)) - 1))
        '================================================================================


        Dim tmpRow As Long
        tmpRow = 0

        tmpSQL = "SELECT " &
                     "stockcodes_master.generated_code,stockcodes_master.sku_number,stockcodes_master.master_code,stockcodes_categories.category_3,stockcodes_categories.category_2," &
                     "stockcodes_categories.category_1,stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_master.item_size,stockcodes_master.item_colour," &
                     "stockcodes_master.size_matrix,stockcodes_master.colour_matrix,stockcodes_master.description,colour_grids.colour_description," &
                     "stockcodes_master.is_service_item,stockcodes_master.supplier,stockcodes_master.suppliers_code,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_3 " &
                     "FROM " &
                     "stockcodes_master " &
                     "LEFT OUTER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                     "LEFT OUTER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN colour_grids ON stockcodes_master.item_colour = colour_grids.colour_code " &
                     "WHERE stockcodes_master.master_code = '" & RG.Apos(getCode.MasterCode.ToUpper) & "' ORDER BY stockcodes_master.generated_code ASC"


        Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        If usingObjDB.isR(Ds) Then
            getGeneratedCodeResponse.dt = Ds.Tables(0)
            getGeneratedCodeResponse.Success = True
        Else
            getGeneratedCodeResponse.Success = False
            getGeneratedCodeResponse.Message = "No Data to display"

        End If

        Return getGeneratedCodeResponse

    End Function
End Class
