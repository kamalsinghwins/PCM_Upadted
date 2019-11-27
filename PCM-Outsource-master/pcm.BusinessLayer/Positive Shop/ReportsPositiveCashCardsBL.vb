Imports pcm.DataLayer
Public Class ReportsPositiveCashCardsBL

    Private ReadOnly _dLayer As ReportsPositiveCashCardsDL

    Public Sub New(ByVal CompanyCode As String)
        _dLayer = New ReportsPositiveCashCardsDL(CompanyCode)
    End Sub

    Public Sub New()
        _dLayer = New ReportsPositiveCashCardsDL
    End Sub


    Public Function GetCashCardAccounts(ByVal SearchString As String) As DataTable

        Return _dLayer.GetCustomerAccounts(SearchString)

    End Function

    Public Function ReturnTransactionListing(ByVal StartDate As String, ByVal EndDate As String, _
                                             Optional ByVal AccountNumber As String = "") As DataTable

        Return _dLayer.ReturnTransactionListing(StartDate, EndDate, AccountNumber)

    End Function

    Public Function ReturnCustomerDetails(ByVal AccountNumber As String) As DataTable

        Return _dLayer.ReturnCustomerDetails(AccountNumber)

    End Function

    Public Function ReturnCashCardSummary(ByVal AccountOpenedFrom As String, ByVal AccountOpenedTo As String, ByVal TransactionFrom As String, ByVal TransactionFromTo As String) As DataTable

        Return _dLayer.ReturnCashCardSummary(AccountOpenedFrom, AccountOpenedTo, TransactionFrom, TransactionFromTo)

    End Function

    Public Function ReturnCashCard(ByVal DateFrom As String, ByVal DateTo As String,
                                   ByVal AllAccounts As Boolean) As DataTable

        Return _dLayer.ReturnCashCard(DateFrom, DateTo, AllAccounts)

    End Function

    Public Function ReturnCashCardSummaryLineItems(ByVal AccountOpenedFrom As String, ByVal AccountOpenedTo As String, ByVal TransactionFrom As String, ByVal TransactionFromTo As String) As DataTable

        Return _dLayer.ReturnCashCardSummaryLineItems(AccountOpenedFrom, AccountOpenedTo, TransactionFrom, TransactionFromTo)

    End Function

End Class
