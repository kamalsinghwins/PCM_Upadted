Public Class IBT_Out
    Public Class SearchResponse
        Inherits BaseResponse
        Public Property dt As DataTable
    End Class


    Public Class BranchDetailResponse
        Inherits BaseResponse
        Public Property BranchName As String
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property AddressLine3 As String
        Public Property AddressLine4 As String
        Public Property AddressLine5 As String
        Public Property TelephoneNumber As String
        Public Property FaxNumber As String
        Public Property TaxNumber As String

        Public Property isBlocked As Boolean
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
    End Class

    Public Class GetIbt_OutNumber
        Inherits BaseResponse
        Public Property Number As String
    End Class

    Public Class GetTaxResponse
        Inherits BaseResponse
        Public Property dt As DataTable

        Public Property GetTaxGroups As Boolean
    End Class

    Public Class GetCompanyResponse
        Inherits BaseResponse
        Public Property dt As DataTable

        Public Property GetCompanySettings As Boolean
    End Class


    Public Class GetBranchSettingsResponse
        Inherits BaseResponse
        Public Property dt As DataTable

        Public Property GetBranchSettings As Boolean
    End Class

    Public Class DoStockResponse
        Inherits BaseResponse
        Public Property dt As DataTable

        Public Property GuidReturn As String
    End Class

    Public Class GetBranch
        Public Property SearchType As String
        Public Property SearchDetail As String

        Public Property Limit As String

    End Class


    Public Class ItemCode
        Public Property ItemCode As String
        Public Property BranchCode As String
    End Class



    Public Class BranchDetail

        Public Property BranchCode As String
    End Class



    Public Class GetItems

        Public Property SearchText As String
        Public Property SearchType As String
        Public Master As Boolean
        Public Limit As String

    End Class

    Public Class SearchBranchResponse
        Inherits BaseResponse
        Public Property searchBranchList As DataTable
    End Class

    Public Class BranchResponse
        Inherits BaseResponse
        Public Property BranchName As String
        Public Property BranchCode As String

    End Class


    Public Class Save
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
        Public Property Branch_Telephone_Number As String
        Public Property Branch_Fax_Number As String
        Public Property Current_User As String
        Public Property BoxStyle As String
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

    Public Class ReloadResponse
        Inherits BaseResponse
        Public Property ListData As List(Of ListData)
    End Class

    Public Class SaveResponse
        Inherits BaseResponse
        Public Property Path As String
        Public Property LabelPath As String
    End Class

    Public Class GetCodes
        Public Property MasterCode As String
        Public Property TaxGroup As String
        Public Property TaxGroupSelling As String
    End Class

    Public Class GetGeneratedCodeResponse
        Inherits BaseResponse
        Public Property Count As String
        Public Property dt As DataTable


    End Class

    Public Class DeleteFile
        Public Property FileName As String
    End Class

    Public Class GetLabels
        Inherits BaseResponse
        Public Property Path As String
    End Class

    Public Class DeleteFiles
        Public Property Path As String
        Public Property LabelPath As String
    End Class
End Class
