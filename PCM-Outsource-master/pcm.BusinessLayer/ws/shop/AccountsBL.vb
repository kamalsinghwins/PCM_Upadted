Imports Entities
Imports pcm.DataLayer

Public Class AccountsBL

    Dim _DLayer As New AccountsDL

    Public Function CheckCardnumber(ByVal CheckCardRequest As CheckCardRequest) As CheckCardResponse
        Dim _checkCardResponse As New CheckCardResponse

        If Len(CheckCardRequest.CardNumber) <> 16 Then
            _checkCardResponse.Message = "The card number is invalid."
            _checkCardResponse.Success = False
            Return _checkCardResponse
        End If

        If Mid$(CheckCardRequest.CardNumber, 1, 4) <> "6501" And Mid$(CheckCardRequest.CardNumber, 1, 4) <> "6502" Then
            _checkCardResponse.Message = "The card number is invalid."
            _checkCardResponse.Success = False
            Return _checkCardResponse
        End If

        _checkCardResponse = _DLayer.CheckCardnumber(CheckCardRequest)
        Return _checkCardResponse

    End Function

    Public Function CheckAccountNumber(ByVal CheckCardRequest As CheckCardRequest) As CheckCardResponse
        Dim _checkCardResponse As New CheckCardResponse
        _checkCardResponse = _DLayer.CheckAccountNumber(CheckCardRequest)
        Return _checkCardResponse
    End Function

    Public Function ProcessJournalEntry(ByVal _JournalEntryRequest As JournalEntryRequest) As String
        Dim _queryResponse As String
        _queryResponse = _DLayer.ProcessJournalEntry(_JournalEntryRequest)
        Return _queryResponse
    End Function

    Public Function ProcessMultipleTransaction(ByVal AccountDetails As PCMAccount) As PCMAccount
        Dim _PCMAccount As New PCMAccount

        If Len(AccountDetails.CardNumber) <> 16 Then
            _PCMAccount.ReturnMessage = "The card number is invalid."
            _PCMAccount.Success = False
            Return _PCMAccount
        End If

        If AccountDetails.AccountNumber = "" Then
            _PCMAccount.ReturnMessage = "Please select a valid Card Number."
            _PCMAccount.Success = False
            Return _PCMAccount
        End If

        If Len(AccountDetails.TransactionNumber) <> 3 Then
            _PCMAccount.ReturnMessage = "Please enter a valid Branch Code."
            _PCMAccount.Success = False
            Return _PCMAccount
        End If

        If Val(AccountDetails.TransactionAmount) = 0 Then
            _PCMAccount.ReturnMessage = "Please input a valid amount."
            _PCMAccount.Success = False
            Return _PCMAccount
        End If

        If Val(AccountDetails.TransactionNumber) > 5000 Then
            _PCMAccount.ReturnMessage = "Please enter a smaller amount to process."
            _PCMAccount.Success = False
            Return _PCMAccount
        End If

        AccountDetails.isOnlineTransaction = True

        Return _DLayer.ProcessAccountTransaction(AccountDetails)
        '_processTransactionResponse = _DLayer.ProcessMultipleTransaction(ProcessTransactionRequest)

        Return _PCMAccount

    End Function

    Public Function ProcessAccountTransaction(ByVal AccountDetails As PCMAccount) As PCMAccount

        If AccountDetails.wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        'Manual online transaction
        AccountDetails.isOnlineTransaction = False

        Return _DLayer.ProcessAccountTransaction(AccountDetails)

    End Function

    Public Function ProcessManualPayment(ByVal AccountDetails As PCMAccount) As PCMAccount

        If AccountDetails.wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Return _DLayer.ProcessManualPayment(AccountDetails)

    End Function


    Public Function GetSelfActivateDetails(ByVal Password As String, ByVal IDNumber As String, ByVal BranchCode As String) As Debtor

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _DLayer As New AccountsDL
        Return _DLayer.GetSelfActivateDetails(IDNumber, BranchCode)

    End Function

    Public Function InsertSelfActivated(ByVal Password As String, ByVal DebtorDetails As Debtor) As Debtor

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _DLayer As New AccountsDL
        Return _DLayer.InsertSelfActivated(DebtorDetails)

    End Function



    Public Sub UpdateCellPhoneNumber(ByVal Password As String, ByVal strAccountNumber As String,
                                     ByVal strUser As String,
                                     ByVal tOldVal As String, ByVal tNewVal As String, ByVal tEMailAddress As String)

        If Password <> "JaiRL10nFMNo$forany" Then
            Exit Sub
        End If

        Dim _DLayer As New AccountsDL
        _DLayer.UpdateCellPhoneNumber(strAccountNumber, strUser, tOldVal, tNewVal, tEMailAddress)

    End Sub

    Public Function GetCardNumber(ByVal Password As String, ByVal IDNumber As String) As String

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _DLayer As New AccountsDL
        Return _DLayer.GetCardNumber(IDNumber)

    End Function

End Class
