Imports Entities
Imports pcm.DataLayer

Public Class ShopMiniStatementBusinessLayer

    Dim _dlDataLayer As New ShopMiniStatementDataLayer

    Public Function GetData(ByVal CardNumber As String,
                            ByVal strAccountNumber As String) As ShopMiniStatement

        'Getting statement from account number - most likely from Self Assist
        If CardNumber = "" Then
            Return _dlDataLayer.GetAccountData(CardNumber, strAccountNumber)
        End If

        If Mid$(CardNumber, 1, 4) = "6501" Then
            Return _dlDataLayer.GetAccountData(CardNumber, strAccountNumber)
        ElseIf Mid$(CardNumber, 1, 4) = "6502" Then
            Return _dlDataLayer.GetGiftCardData(CardNumber)
        End If

        Return Nothing
    End Function

End Class
