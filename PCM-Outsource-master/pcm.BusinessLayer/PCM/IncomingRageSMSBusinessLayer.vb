Imports pcm.DataLayer
Imports Entities

Public Class IncomingRageSMSBusinessLayer

    Dim _DLayer As New IncomingRageSMSDataLayer

    Public Sub InsertRageSMS(ByVal ReceivedFromNumber As String, ByVal SentToNumber As String, ByVal Message As String)

        _DLayer.InsertRageSMS(ReceivedFromNumber, SentToNumber, Message)

    End Sub

    Public Function GetRageSMSDetails(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Return _DLayer.GetRageSMSDetails(FromDate, ToDate)

    End Function

    Public Function GetManualSaleSMSDetails(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Return _DLayer.GetManualSaleSMSDetails(FromDate, ToDate)

    End Function

    Public Sub DoLimitIncrease(ByVal PhoneNumber As String)

        'Get the account, mark account as auto-update,see if an increase is allowed, give the increase, sms the increase
        _DLayer.DoLimitIncrease(PhoneNumber)

    End Sub

End Class
