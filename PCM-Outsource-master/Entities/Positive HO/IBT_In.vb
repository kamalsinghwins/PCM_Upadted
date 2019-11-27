Public Class IBT_In

    Public Class FetchDetails
        Public Property FromBranch As String
        Public Property BranchName As String
        Public Property IBTOutNumber As String
    End Class


    Public Class FetchDetailsResponse
        Inherits BaseResponse
        Public Property Data As List(Of Data)

    End Class

    Public Class Data
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
        Public Property SellingPrice1 As String

        Public Property Supplier As String
        Public Property SupplierCode As String
        Public Property IsServiceItem As String
        Public Property CostPrice As String
        Public Property SellingTaxGroup As String
        Public Property Quantity As String

    End Class

    Public Class GetIbt_InNumber
        Inherits BaseResponse
        Public Property Number As String
    End Class

    Public Class SaveIBTIN
        Public Property BranchName As String
        Public Property IBTOutNumber As String
        Public Property BranchCode As String
        Public Property Address1 As String
        Public Property Address2 As String

        Public Property Address3 As String
        Public Property Address4 As String
        Public Property Address5 As String
        Public Property TotalSentQuantity As String
        Public Property TotalReceievedQuantity As String

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
        Public Property ListData As List(Of Data)
    End Class

    Public Class SaveResponse
        Inherits BaseResponse
        Public Property Path As String
    End Class

    Public Class DeletePrintFile
        Public Property FileName As String
    End Class
End Class



