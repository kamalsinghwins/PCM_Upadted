Public Class Stock
    Public Class GetCodesRequest
        Public Property MasterCode As String
        Public Property BranchCode As String
        Public Property TaxGroup As String
        Public Property TaxGroupSelling As String

    End Class
    Public Class GeneratedCodeInfoResponse
        Inherits BaseResponse
        Public Property Count As String
        Public Property dt As DataTable


    End Class
    Public Class GetGeneratedCodeInfoResponse
        Inherits BaseResponse
        Public Property CodeMasterCodeSTR As String
        Public Property CodeGeneratedCodeSTR As String
        Public Property CodeBarcodeSTR As String
        Public Property CodeDescriptionSTR As String
        Public Property CodePurchaseTaxGroupSTR As String
        Public Property CodeSalesTaxGroupSTR As String
        Public Property CodeCategory1STR As String
        Public Property CodeCategory2STR As String
        Public Property CodeCategory3STR As String
        Public Property CodeIsServiceItemBOOL As Boolean
        Public Property CodeIsDiscountableBOOL As Boolean
        Public Property CodeIsBlockedBOOL As Boolean
        Public Property CodeSupplierAccountSTR As String
        Public Property CodeSupplierItemCodeSTR As String
        Public Property CodeMinimumStockLevelDBL As Double
        Public Property CodeCostAverageExclusiveDBL As Double
        Public Property CodeEstimatedCostExclusiveDBL As Double
        Public Property CodeSellingExlusive1DBL As Double
        Public Property CodeSellingExlusive2DBL As Double
        Public Property CodeSellingExlusive3DBL As Double
        Public Property CodeItemColourSTR As String
        Public Property CodeItemSizeSTR As String
        Public Property CodeSizeMatrixSTR As String
        Public Property CodeColourMatrixSTR As String
        Public Property CodeQuantityOnHand As Double
        Public Property GetGeneratedCodeInfo As Boolean
        Public Property QtyOnHand As String
    End Class
    Public Class SaveStock
        Public Property BranchName As String
        Public Property Reference As String
        Public Property BranchCode As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property Address4 As String
        Public Property Address5 As String
        Public Property Quantity As String
        Public Property TotalTAX As String
        Public Property TotalIncl As String
        Public Property TotalExcl As String
        Public Property Current_Company As String
        Public Property Branch_Name As String
        Public Property Current_Branch_Code As String
        Public Property Telephone_Number As String
        Public Property Branch_Fax_Number As String
        Public Property Current_User As String
        Public Property BoxStyle As String
        Public Property Fax As String
        Public Property ListData As List(Of ListData)

    End Class
    Public Class ListData
        Public Property ItemCode As String
        Public Property Description As String
        Public Property Excl As String
        Public Property Tax As String
        Public Property Incl As String
        Public Property Qty As String
        Public Property TotalCostIncl As String
        Public Property MCode As String
        Public Property Cat1 As String
        Public Property Cat2 As String
        Public Property Cat3 As String
        Public Property ItemSize As String
        Public Property SizeGrid As String
        Public Property Colour As String
        Public Property ColourGrid As String
        Public Property OriginalCost As String
        Public Property Supplier As String
        Public Property SupplierCode As String
        Public Property IsServiceItem As String
        Public Property SellEx As String
        Public Property SellingTaxGroup As String
        Public Property SKUNumber As String
        Public Property SellingIncl As String
    End Class
    Public Class GetNumber
        Inherits BaseResponse
        Public Property Number As String
    End Class
    Public Class StockTransactions
        Public Property MCode As String
        Public Property GCode As String
        Public Property DDescription As String
        Public Property TransType As String
        Public Property TheShiftOrBlank As String
        Public Property Current_UserOrBlank As String
        Public Property TransactionNumber As String
        Public Property TheRepOrBlank As String
        Public Property SellEx As String
        Public Property tQtyPositiveOnly As String
        Public Property TaxGroupDescriptionPercentage As String
        Public Property DSupplier As String
        Public Property DSupplierItemCode As String
        Public Property DisServiceItem As String
        Public Property DCategory1 As String
        Public Property DCategory2 As String
        Public Property DCategory3 As String
        Public Property DItemSize As String
        Public Property DItemSizeGrid As String
        Public Property DItemColour As String
        Public Property DItemColourGrid As String
        Public Property OriginalAvgCost As String
        Public Property DiscountAmount As String
        Public Property DiscountReason As String
        Public Property UpdatedCostEx As String
        Public Property tGuID As String
        Public Property OriginalEstimatedCost As String
        Public Property BranchToEffectForADJ As String
        Public Property Tax As String
        Public Property BranchCode As String

    End Class
    Public Class SaveResponse
        Inherits BaseResponse
        Public Property Path As String
        Public Property LabelPath As String
    End Class
    Public Class DeleteStockAdjustmentFile
        Public Property Path As String
    End Class

    Public Class GetLabels
        Inherits BaseResponse
        Public Property Path As String
    End Class
    Public Class DeleteGRVFiles
        Public Property Path As String
        Public Property LabelPath As String
    End Class
End Class
