Public Class Manage

    Public Class BranchDetails
        Public Property BranchCode As String
        Public Property TelephoneNumber As String
        Public Property Fax As String
        Public Property Email As String
        Public Property TAX As String
        Public Property MerchantNumber As String
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property AddressLine3 As String
        Public Property AddressLine4 As String
        Public Property AddressLine5 As String
        Public Property Price As String
        Public Property Blocked As Boolean
        Public Property BranchType As String
        Public Property BranchName As String
        Public Property Region As String
        Public Property Municipality As String
        Public Property Province As String
        Public Property StoreSquareMetres As String
        Public Property TradingHourStart As String
        Public Property TradingHourEnd As String
        Public Property Longitude As String
        Public Property Latitude As String
        Public Property CompanyName As String
        Public Property TypeOfMall As String
        Public Property URL As String
        Public Property BranchNameWeb As String
        Public Property StoreStatus As String
    End Class
    Public Class SearchResponse
        Inherits BaseResponse
        Public Property dt As DataTable
    End Class
    Public Class AddStockResponse
        Inherits BaseResponse
        Public Property data As String
    End Class
    Public Class SaveSpecial
        Inherits BaseResponse
        Public Property SpecialName As String
        Public Property StartDate As String
        Public Property EndDate As String
        Public Property IsActive As Boolean
        Public Property Price As String
        Public Property GUID As String
        Public Property ListData As List(Of SpecialLineItems)

    End Class
    Public Class SpecialLineItems
        Public Property Mastercode As String
        Public Property Quantity As String
        Public Property Description As String
        Public Property LinkGUID As String
    End Class
    Public Class GetSpecialDetails
        Public Property SpecialName As String
    End Class
    Public Class GetSpecialItems
        Public Property SearchText As String
        Public Property SearchType As String
        Public Property Master As Boolean
    End Class
    Public Class StockRequest
        Public Property Stockcode As String
    End Class
    Public Class SupplierRequest
        Public Property SearchType As String
        Public Property SearchText As String
    End Class
    Public Class SupplierDetails
        Public Property SupplierCode As String
    End Class
    Public Class SupplierDetailResponse
        Inherits BaseResponse
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property AddressLine3 As String
        Public Property AddressLine4 As String
        Public Property AddressLine5 As String
        Public Property Telephone As String
        Public Property FAX As String
        Public Property TAX As String
        Public Property SupplierName As String
        Public Property IsBlocked As Boolean
    End Class
    Public Class GetGeneratedcodes
        Public Property SearchText As String
    End Class
    Public Class GetItemcodeDetails
        Public Property Itemcode As String
    End Class
    Public Class SaveGRV
        Public Property SupplierAccount As String
        Public Property Reference As String
        Public Property SupplierName As String
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
        Public Property TotalIncl As String
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
    Public Class SaveUpdateSupplier
        Public Property SupplierCode As String
        Public Property SupplierName As String
        Public Property Telephone As String
        Public Property FAX As String
        Public Property Email As String
        Public Property TAX As String
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property AddressLine3 As String
        Public Property AddressLine4 As String
        Public Property AddressLine5 As String
        Public Property IsBlocked As Boolean
    End Class
    Public Class Categories
        Public Property Description As String
        Public Property CategoryDescription As String
        Public Property CategoryNumber As String
        Public Property CategoryCode As String
        Public Property CategoryType As String
    End Class
End Class
