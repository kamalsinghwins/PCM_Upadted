Imports pcm.DataLayer
Imports Entities
Imports Entities.LoyaltyDiscount

Public Class LoyaltyDiscountHOBL

    Dim _dlLayer As LoyaltyDiscountHODL

    Public Sub New()
        _dlLayer = New LoyaltyDiscountHODL
    End Sub

    Public Function GetDiscountList() As DataSet
        Return _dlLayer.GetDiscountList()
    End Function

    Public Function SaveDiscount(discountRequest As SaveDiscount) As BaseResponse
        Return _dlLayer.SaveDiscount(discountRequest)
    End Function

    Public Function GetSelectedDiscountDetails(strSurveyId As String) As GetDiscountResponse
        Return _dlLayer.GetSelectedDiscountDetails(strSurveyId)
    End Function

    Public Function GetLoyaltyDiscounts(ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Return _dlLayer.GetLoyaltyDiscounts(StartDate, EndDate)
    End Function
End Class
