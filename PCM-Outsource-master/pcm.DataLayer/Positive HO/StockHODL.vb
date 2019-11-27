Imports Entities
Imports Entities.Stock

Public Class StockHODL
    Inherits DataAccessLayerBase
    Dim generatedCodeInfoResponse As New GeneratedCodeInfoResponse
    Dim generatedCodeResponse As New GetGeneratedCodeInfoResponse
    Dim getNumber As New GetNumber


    Public Function GetCodes(ByVal getCode As GetCodesRequest) As GeneratedCodeInfoResponse


        tmpSQL = "SELECT count(generated_code) FROM stockcodes_master WHERE master_code = '" & RG.Apos(getCode.MasterCode) & "'"

        Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        If usingObjDB.isR(Ds) Then
            generatedCodeInfoResponse.Count = Ds.Tables(0).Rows(0)("count") & ""

        Else
            generatedCodeInfoResponse.Success = False
            generatedCodeInfoResponse.Message = "No generated codes found"
            Return generatedCodeInfoResponse
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
                     "stockcodes_master.is_service_item,stockcodes_master.supplier,stockcodes_master.suppliers_code,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_3, " &
                     "(SELECT sum(stock_on_hand.qty_on_hand) FROM stock_on_hand WHERE stockcodes_master.generated_code = stock_on_hand.generated_code " &
                     "AND branch_code = '" & RG.Apos(getCode.BranchCode.ToUpper) & "')  AS qty_on_hand " &
                     "FROM " &
                     "stockcodes_master " &
                     "LEFT OUTER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                     "LEFT OUTER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN colour_grids ON stockcodes_master.item_colour = colour_grids.colour_code " &
                     "WHERE stockcodes_master.master_code = '" & RG.Apos(getCode.MasterCode.ToUpper) & "' ORDER BY stockcodes_master.generated_code ASC"


        Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        If usingObjDB.isR(Ds) Then

            generatedCodeInfoResponse.dt = Ds.Tables(0)
            generatedCodeInfoResponse.Success = True
        Else
            generatedCodeInfoResponse.Success = False
            generatedCodeInfoResponse.Message = "No Data to display"

        End If

        Return generatedCodeInfoResponse

    End Function
    Public Function GetGeneratedCodeInfo(ByVal strGeneratedCodeOrBarcode As String, ByVal _strBranchCode As String) As GetGeneratedCodeInfoResponse


        tmpSQL = "SELECT qty_on_hand FROM stock_on_hand WHERE generated_code = '" & RG.Apos(strGeneratedCodeOrBarcode.ToUpper) & "' AND branch_code = '" & RG.Apos(_strBranchCode.ToUpper) & "'"

        Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        If usingObjDB.isR(Ds) Then
            generatedCodeResponse.QtyOnHand = Ds.Tables(0).Rows(0)("qty_on_hand").ToString.ToUpper & ""

        Else
            generatedCodeResponse.QtyOnHand = "0.00"
        End If


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
                generatedCodeResponse.CodeMasterCodeSTR = Ds.Tables(0).Rows(0)("master_code").ToString.ToUpper & ""
                generatedCodeResponse.CodeGeneratedCodeSTR = Ds.Tables(0).Rows(0)("generated_code").ToString.ToUpper & ""
                generatedCodeResponse.CodeBarcodeSTR = Ds.Tables(0).Rows(0)("sku_number").ToString.ToUpper & ""
                generatedCodeResponse.CodeDescriptionSTR = Ds.Tables(0).Rows(0)("description").ToString.ToUpper & ""
                generatedCodeResponse.CodePurchaseTaxGroupSTR = Ds.Tables(0).Rows(0)("purchase_tax_group") & ""
                generatedCodeResponse.CodeSalesTaxGroupSTR = Ds.Tables(0).Rows(0)("sales_tax_group") & ""

                generatedCodeResponse.CodeCategory1STR = Ds.Tables(0).Rows(0)("category_1").ToString.ToUpper & ""
                generatedCodeResponse.CodeCategory2STR = Ds.Tables(0).Rows(0)("category_2").ToString.ToUpper & ""
                generatedCodeResponse.CodeCategory3STR = Ds.Tables(0).Rows(0)("category_3").ToString.ToUpper & ""

                generatedCodeResponse.CodeIsServiceItemBOOL = Ds.Tables(0).Rows(0)("is_service_item") & ""
                generatedCodeResponse.CodeIsDiscountableBOOL = Ds.Tables(0).Rows(0)("is_not_discountable") & ""
                generatedCodeResponse.CodeIsBlockedBOOL = Ds.Tables(0).Rows(0)("is_blocked") & ""

                generatedCodeResponse.CodeSupplierAccountSTR = Ds.Tables(0).Rows(0)("supplier").ToString.ToUpper & ""
                generatedCodeResponse.CodeSupplierItemCodeSTR = Ds.Tables(0).Rows(0)("suppliers_code").ToString.ToUpper & ""

                generatedCodeResponse.CodeMinimumStockLevelDBL = If(Ds.Tables(0).Rows(0)("minimum_stock_level").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("minimum_stock_level")))

                generatedCodeResponse.CodeCostAverageExclusiveDBL = If(Ds.Tables(0).Rows(0)("cost_price").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("cost_price")))
                generatedCodeResponse.CodeEstimatedCostExclusiveDBL = If(Ds.Tables(0).Rows(0)("estimated_cost").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("estimated_cost")))
                generatedCodeResponse.CodeSellingExlusive1DBL = If(Ds.Tables(0).Rows(0)("selling_price_1").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_1")))
                generatedCodeResponse.CodeSellingExlusive2DBL = If(Ds.Tables(0).Rows(0)("selling_price_2").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_2")))
                generatedCodeResponse.CodeSellingExlusive3DBL = If(Ds.Tables(0).Rows(0)("selling_price_3").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_3")))

                generatedCodeResponse.CodeItemColourSTR = Ds.Tables(0).Rows(0)("item_colour").ToString.ToUpper & ""
                generatedCodeResponse.CodeItemSizeSTR = Ds.Tables(0).Rows(0)("item_size").ToString.ToUpper & ""
                generatedCodeResponse.CodeSizeMatrixSTR = Ds.Tables(0).Rows(0)("size_matrix").ToString.ToUpper & ""
                generatedCodeResponse.CodeColourMatrixSTR = Ds.Tables(0).Rows(0)("colour_matrix").ToString.ToUpper & ""
                generatedCodeResponse.CodeQuantityOnHand = If(Ds.Tables(0).Rows(0)("qty_on_hand").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("qty_on_hand")))
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
                    generatedCodeResponse.CodeMasterCodeSTR = Ds.Tables(0).Rows(0)("master_code").ToString.ToUpper & ""
                    generatedCodeResponse.CodeGeneratedCodeSTR = Ds.Tables(0).Rows(0)("generated_code").ToString.ToUpper & ""
                    generatedCodeResponse.CodeBarcodeSTR = Ds.Tables(0).Rows(0)("sku_number").ToString.ToUpper & ""
                    generatedCodeResponse.CodeDescriptionSTR = Ds.Tables(0).Rows(0)("description").ToString.ToUpper & ""
                    generatedCodeResponse.CodePurchaseTaxGroupSTR = Ds.Tables(0).Rows(0)("purchase_tax_group") & ""
                    generatedCodeResponse.CodeSalesTaxGroupSTR = Ds.Tables(0).Rows(0)("sales_tax_group") & ""

                    generatedCodeResponse.CodeCategory1STR = Ds.Tables(0).Rows(0)("category_1").ToString.ToUpper & ""
                    generatedCodeResponse.CodeCategory2STR = Ds.Tables(0).Rows(0)("category_2").ToString.ToUpper & ""
                    generatedCodeResponse.CodeCategory3STR = Ds.Tables(0).Rows(0)("category_3").ToString.ToUpper & ""

                    generatedCodeResponse.CodeIsServiceItemBOOL = Ds.Tables(0).Rows(0)("is_service_item") & ""
                    generatedCodeResponse.CodeIsDiscountableBOOL = Ds.Tables(0).Rows(0)("is_not_discountable") & ""
                    generatedCodeResponse.CodeIsBlockedBOOL = Ds.Tables(0).Rows(0)("is_blocked") & ""

                    generatedCodeResponse.CodeSupplierAccountSTR = Ds.Tables(0).Rows(0)("supplier").ToString.ToUpper & ""
                    generatedCodeResponse.CodeSupplierItemCodeSTR = Ds.Tables(0).Rows(0)("suppliers_code").ToString.ToUpper & ""

                    generatedCodeResponse.CodeMinimumStockLevelDBL = If(Ds.Tables(0).Rows(0)("minimum_stock_level").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("minimum_stock_level")))
                    generatedCodeResponse.CodeCostAverageExclusiveDBL = If(Ds.Tables(0).Rows(0)("cost_price").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("cost_price")))
                    generatedCodeResponse.CodeEstimatedCostExclusiveDBL = If(Ds.Tables(0).Rows(0)("estimated_cost").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("estimated_cost")))
                    generatedCodeResponse.CodeSellingExlusive1DBL = If(Ds.Tables(0).Rows(0)("selling_price_1").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_1")))
                    generatedCodeResponse.CodeSellingExlusive2DBL = If(Ds.Tables(0).Rows(0)("selling_price_2").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_2")))
                    generatedCodeResponse.CodeSellingExlusive3DBL = If(Ds.Tables(0).Rows(0)("selling_price_3").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("selling_price_3")))


                    generatedCodeResponse.CodeItemColourSTR = Ds.Tables(0).Rows(0)("item_colour").ToString.ToUpper & ""
                    generatedCodeResponse.CodeItemSizeSTR = Ds.Tables(0).Rows(0)("item_size").ToString.ToUpper & ""
                    generatedCodeResponse.CodeSizeMatrixSTR = Ds.Tables(0).Rows(0)("size_matrix").ToString.ToUpper & ""
                    generatedCodeResponse.CodeColourMatrixSTR = Ds.Tables(0).Rows(0)("colour_matrix").ToString.ToUpper & ""
                    generatedCodeResponse.CodeQuantityOnHand = If(Ds.Tables(0).Rows(0)("qty_on_hand").ToString() = "", 0, Convert.ToDouble(Ds.Tables(0).Rows(0)("qty_on_hand")))

                Else
                    generatedCodeResponse.CodeMasterCodeSTR = ""
                    generatedCodeResponse.CodeGeneratedCodeSTR = ""
                    generatedCodeResponse.CodeBarcodeSTR = ""
                    generatedCodeResponse.CodeDescriptionSTR = ""
                    generatedCodeResponse.CodePurchaseTaxGroupSTR = ""
                    generatedCodeResponse.CodeSalesTaxGroupSTR = ""

                    generatedCodeResponse.CodeCategory1STR = ""
                    generatedCodeResponse.CodeCategory2STR = ""
                    generatedCodeResponse.CodeCategory3STR = ""

                    generatedCodeResponse.CodeIsServiceItemBOOL = False
                    generatedCodeResponse.CodeIsDiscountableBOOL = False
                    generatedCodeResponse.CodeIsBlockedBOOL = False

                    generatedCodeResponse.CodeSupplierAccountSTR = ""
                    generatedCodeResponse.CodeSupplierItemCodeSTR = ""

                    generatedCodeResponse.CodeMinimumStockLevelDBL = 0

                    generatedCodeResponse.CodeCostAverageExclusiveDBL = 0
                    generatedCodeResponse.CodeEstimatedCostExclusiveDBL = 0
                    generatedCodeResponse.CodeSellingExlusive1DBL = 0
                    generatedCodeResponse.CodeSellingExlusive2DBL = 0
                    generatedCodeResponse.CodeSellingExlusive3DBL = 0

                    generatedCodeResponse.CodeItemColourSTR = ""
                    generatedCodeResponse.CodeItemSizeSTR = ""
                    generatedCodeResponse.CodeSizeMatrixSTR = ""
                    generatedCodeResponse.CodeColourMatrixSTR = ""

                    generatedCodeResponse.CodeQuantityOnHand = 0
                    generatedCodeResponse.GetGeneratedCodeInfo = False
                    Return generatedCodeResponse
                End If
            End If
        Catch ex As Exception
            Throw ex

        End Try

        generatedCodeResponse.GetGeneratedCodeInfo = True
        Return generatedCodeResponse
    End Function
    Public Function GetNum(ByVal TransactionType As String, ByVal Current_Branch_Code As String) As GetNumber

        tmpSQL = "SELECT nextval('" & Current_Branch_Code & "_stkadj_seq')"
        Try
            Ds = usingObjDB.GetDataSet(_POSWriteConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getNumber.Number = Ds.Tables(0).Rows(0)("nextval") & ""
            Else
                getNumber.Message = "Error retrieving number."
                getNumber.Success = False
                Return getNumber
            End If

        Catch ex As Exception
            Throw ex
        End Try
        getNumber.Success = True
        Return getNumber
    End Function
End Class
