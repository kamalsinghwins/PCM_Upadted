Imports Entities
Imports pcm.DataLayer
Public Class PCMReportingBusinessLayer
    Private _dlPCMReportingDataLayer As New PCMReportingDataLayer

    Public Function GetDetails(_getDetailsRequest As GetDetailsRequest) As GetDetailsResponse
        Dim _getDetailsResponse As GetDetailsResponse
        _getDetailsResponse = _dlPCMReportingDataLayer.GetDetails(_getDetailsRequest)
        Return _getDetailsResponse

    End Function
    Public Function GetQuery(_queryRequest As QueryRequest) As QueryResponse
        Dim _queryResponse As QueryResponse
        _queryResponse = _dlPCMReportingDataLayer.GetQuery(_queryRequest)
        Return _queryResponse
    End Function

    Public Function GetEMailAddresses() As String

        Return _dlPCMReportingDataLayer.GetEMailAddresses

    End Function

    Public Function GetCellphoneNumbers() As String

        Return _dlPCMReportingDataLayer.GetCellphoneNumbers

    End Function

    Public Function GetCellphonesByBranch(ByVal BranchCode As String) As String

        Return _dlPCMReportingDataLayer.GetCellphonesByBranch(BranchCode)

    End Function

    Public Function GetAllCellphoneNumbers() As String

        Return _dlPCMReportingDataLayer.GetAllCellphoneNumbers

    End Function

    Public Function GetAgeAnalysisDetails(_getAgeAnalysisDetailRequest As GetAgeAnalysisDetailRequest) As GetAgeAnalysisDetailResponse
        Dim _ageAnalysisDetailResponse As GetAgeAnalysisDetailResponse
        _ageAnalysisDetailResponse = _dlPCMReportingDataLayer.GetAgeAnalysisDetails(_getAgeAnalysisDetailRequest)
        Return _ageAnalysisDetailResponse
    End Function

    Public Function GetPeriods() As GetPeriodResponse
        Dim _periodResponse As GetPeriodResponse
        _periodResponse = _dlPCMReportingDataLayer.GetReport()
        Return _periodResponse
    End Function

    'Public Function GetReport(_reportRequest As ReportRequest) As ReportResponse
    '    Dim _reportResponse As ReportResponse
    '    _reportResponse = _dlPCMReportingDataLayer.GetReport(_reportRequest)
    '    Return _reportResponse
    'End Function

    Public Function GetCardDetails(_giftCardDetailsRequest As GiftCardDetailsRequest) As GiftCardDetailsResponse
        Dim _giftCardDetailsResponse As GiftCardDetailsResponse
        _giftCardDetailsResponse = _dlPCMReportingDataLayer.GetCardDetails(_giftCardDetailsRequest)
        Return _giftCardDetailsResponse
    End Function

    Public Function GetTransactionListDetails(ByVal TransactionListRequest As TransactionListRequest) As TransactionListResponse
        Dim _transactionListResponse As New TransactionListResponse
        _transactionListResponse = _dlPCMReportingDataLayer.GetTransactionListDetails(TransactionListRequest)
        Return _transactionListResponse
    End Function

    Public Function CardIssued(ByVal NewAccountRequest As NewAccountRequest) As CardIssuedResponse
        Dim _cardIssuedResponse As New CardIssuedResponse
        _cardIssuedResponse = _dlPCMReportingDataLayer.CardIssued(NewAccountRequest)
        Return _cardIssuedResponse
    End Function

    Public Function GetQuery(ByVal NewAccountRequest As NewAccountRequest) As GetQueryResponse
        Dim _queryResponse As New GetQueryResponse
        _queryResponse = _dlPCMReportingDataLayer.GetQuery(NewAccountRequest)
        Return _queryResponse
    End Function

    Public Function GetPaymentTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _dlPCMReportingDataLayer.GetPaymentTransactions(StartDate, EndDate)

    End Function

    Public Function GetNumbersForSMS(ByVal Type As String, ByVal BranchCode As String) As DataTable

        Return _dlPCMReportingDataLayer.GetNumbersForSMS(Type, BranchCode)

    End Function

    Public Function RunVintageReport(ByVal Email As String, ByVal Username As String, ByVal Json As String)
        Return _dlPCMReportingDataLayer.RunVintageReport(Email, Username, Json)

    End Function

    Public Function GetCellphoneNumbersByLoayaltyAccounts() As String

        Return _dlPCMReportingDataLayer.GetCellphoneNumbersByLoayaltyAccounts

    End Function
End Class
