Imports pcm.DataLayer
Imports Entities

Public Class DebtorsBusinessLayer

    Private _dlDebtors As New DebtorsDataLayer

    Public Function GetCardDetails(_cardSumRequest As CardSumRequest) As GetDetails
        Dim _getDetailsResponse As New GetDetails
        _getDetailsResponse = _dlDebtors.GetCardDetails(_cardSumRequest)
        Return _getDetailsResponse

    End Function

    Public Function RunPaymentUpload(ByVal Filename As String,
                                     ByVal EmailAddresses As String,
                                     ByVal IDNumberCheck As Boolean) As String

        Return _dlDebtors.RunPaymentUpload(Filename, EmailAddresses, IDNumberCheck)

    End Function

    Public Function GetQueryResponse(_historyRequest As HistoryRequest) As GetHistoryResponse
        Dim _getHistoryResponse As GetHistoryResponse

        _getHistoryResponse = _dlDebtors.GetQueryResponse(_historyRequest)

        Return _getHistoryResponse
    End Function

    Public Function GetDebtor(ByVal AccountNumber As String) As Debtor

        Dim _NewDebtorData As Debtor

        _NewDebtorData = _dlDebtors.GetDebtorData(AccountNumber)

        If _NewDebtorData Is Nothing Then
            Return Nothing
        End If

        Return _NewDebtorData

    End Function

    Public Function GetDebtorContactInvestigation(ByVal AccountNumber As String) As DebtorContactInvestigation

        Dim _NewDebtorData As DebtorContactInvestigation

        _NewDebtorData = _dlDebtors.GetDebtorContactInvestigationData(AccountNumber)

        If _NewDebtorData Is Nothing Then
            Return Nothing
        End If

        Return _NewDebtorData

    End Function

    Public Function GetDebtorInvestigation(ByVal AccountNumber As String) As Debtor

        Dim _NewDebtorData As Debtor

        _NewDebtorData = _dlDebtors.GetDebtorInvestigationData(AccountNumber)

        If _NewDebtorData Is Nothing Then
            Return Nothing
        End If

        Return _NewDebtorData

    End Function

    'Private _dlDebitOrder As New DebtorOrderDataLayer

    Public Function GetDebitOrders() As DataTable
        Dim debitorDT As DataTable
        debitorDT = _dlDebtors.GetDebitOrders()
        Return debitorDT
    End Function

    Public Function getDebtorsSumDetails() As DebtorsSumResponse
        Dim _DebtorsSumResponse As New DebtorsSumResponse
        _DebtorsSumResponse = _dlDebtors.getDebtorsSumDetails()
        Return _DebtorsSumResponse
    End Function

    Public Function CheckAccountNumber(ByVal _AccountNumber As String) As DataTable

        Return _dlDebtors.CheckAccountNumber(_AccountNumber)

    End Function

    Dim _returnDebtor As New Debtor

    Public Function GetSelfActivateDetails(ByVal IDNumber As String) As Debtor
        _returnDebtor = _dlDebtors.GetSelfActivateDetails(IDNumber)
        Return _returnDebtor
    End Function

    Public Function InsertSelfActivated(ByVal DebtorDetails As Debtor) As Debtor
        _returnDebtor = _dlDebtors.InsertSelfActivated(DebtorDetails)
        Return _returnDebtor
    End Function

    Public Function GetBranchList(Optional ByVal BranchCode As String = "") As DataSet
        Dim _branchListDS As DataSet

        'Get Branch List
        _branchListDS = _dlDebtors.GetBranchList(BranchCode)
        If _branchListDS Is Nothing Then
            Return Nothing
        End If
        Return _branchListDS
    End Function

End Class

