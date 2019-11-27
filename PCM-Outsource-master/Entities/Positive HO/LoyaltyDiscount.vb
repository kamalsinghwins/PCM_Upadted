Public Class LoyaltyDiscount

    Public Class SaveDiscount
        Public Property DiscountName As String
        Public Property DiscountDate As String
        Public Property DiscountPercentage As String
        Public Property IsActive As Boolean
        Public Property UpdateDate As String
        Public Property LoyaltyDiscountId As String
        Public Property IsInsert As Boolean
        Public Property IsUpdate As Boolean
    End Class

    Public Class GetDiscountResponse
        Inherits BaseResponse
        Public Property GetSelectedDiscountResponse As DataTable
    End Class
End Class
