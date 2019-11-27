
Public Class GetDetailsRequest
    Public Property FromAccount As String
    Public Property ToAccount As String
    Public Property CheckAll As Boolean
    Public Property ActiveOnly As Boolean
    Public Property OtherStatus As Boolean
    Public Property Other As String


    Public Property AccountsOpenedBetween As Boolean
    Public Property StartDate As String
    Public Property EndDate As String
    Public Property LastTransaction As Boolean
    Public Property LastDateTransactionStartDate As String
    Public Property LastDateTransactionEndDate As String


    Public Property CboCurrent As String
    Public Property WhereCurrent As String
    Public Property CheckCurrentUse As Boolean

    Public Property Cbo30Days As String
    Public Property Where30Days As String
    Public Property CheckUse30 As Boolean

    Public Property Cbo60Days As String
    Public Property Where60Days As String
    Public Property CheckUse60 As Boolean

    Public Property Cbo90Days As String
    Public Property Where90Days As String
    Public Property CheckUse90 As Boolean

    Public Property Cbo120Days As String
    Public Property Where120Days As String
    Public Property CheckUse120 As Boolean

    Public Property Cbo150Days As String
    Public Property Where150Days As String
    Public Property CheckUse150 As Boolean

    Public Property CboTotal As String
    Public Property Wheretotal As String
    Public Property ToTotal As String
    Public Property CheckUsetotal As Boolean



    Public Property TickOn As Boolean
    Public Property TickOff As Boolean

End Class


Public Class GetDetailsResponse
    Inherits BaseResponse
    Public Property GetSelectedResponse As DataTable
End Class

Public Class QueryRequest
    Public Property AccountStartDate As String
    Public Property AccountEndDate As String
    Public Property SalesStartDate As String
    Public Property SalesEndDate As String
    Public Property PaymentStartDate As String
    Public Property PaymentEndDate As String
    Public Property NeverPaid As Boolean
    Public Property MoreLessThan As String
    Public Property Amount As String
    Public Property File As String
End Class
Public Class QueryResponse
    Inherits BaseResponse
    Public Property GetQueryDetails As DataTable
End Class

Public Class ReportRequest
    Public Property Month As String
    Public Property Year As String
    Public Property Status As String
    Public Property FileName As String
    Public Property Score As String
    Public Property CheckBadDebtStoresonly As Boolean
    Public Property CheckThickFilesOnly As Boolean
    Public Property CheckMaleOnly As Boolean
    Public Property CheckIncludeAllPeriods As Boolean
    Public Property CheckZeroes As Boolean
    Public Property IsAdmin As Boolean
    Public Property IPAddress As String
End Class

Public Class ReportResponse
    Inherits BaseResponse
    Public Property CSV As String
End Class

Public Class GetPeriodResponse
    Inherits BaseResponse
    Public Property Period As Integer

End Class

Public Class GetAgeAnalysisDetailRequest
    Public Property FromAccount As String
    Public Property ToAccount As String
    Public Property Period As String
    Public Property AllAccounts As Boolean
    Public Property PrintDebit As Boolean
    Public Property PrintZero As Boolean

    Public Property BranchCode As String

    Public Property PrintCredit As Boolean
    Public Property RageEmployee As Boolean
    Public Property ActiveAccount As Boolean
    Public Property CheckOtherStatus As Boolean
    Public Property OtherStatus As String

End Class

Public Class GetAgeAnalysisDetailResponse
    Inherits BaseResponse
    Public Property AgeAnalysisDetails As DataTable
    Public Property LongTotalCount As Long
End Class

Public Class GiftCardDetailsRequest
    Public Property CardNumber As String
End Class

Public Class GiftCardDetailsResponse
    Inherits BaseResponse
    Public Property GiftCardDetails As DataTable
    Public Property CardTransactions As DataTable

End Class

Public Class TransactionListRequest
    Public Property StartDate As String
    Public Property EndDate As String
    Public Property CheckPurchase As Boolean
    Public Property CheckGiftCardPurchase As Boolean
    Public Property CheckPayments As Boolean
    Public Property CheckGiftCardPayments As Boolean
    Public Property CheckCreditNotes As Boolean
    Public Property CheckGiftCardCreditNotes As Boolean
    Public Property CheckLostCardProtection As Boolean
    Public Property CheckInterest As Boolean
    Public Property CheckCreditBalanceAffected As Boolean
    Public Property CheckDebitBalanceAffected As Boolean
    Public Property CheckCreditBalanceNotAffected As Boolean
    Public Property CheckDebitBalanceNotAffected As Boolean
    Public Property BadDebtWriteOff As Boolean
    Public Property TransactionType As String
    Public Property FullTransactionType As String

End Class

Public Class TransactionListResponse
    Inherits BaseResponse
    Public Property TransactionList As List(Of Transactions)
End Class
Public Class PrintTransactionDetailsResponse
    Inherits BaseResponse
    'Public Property Count As Integer
    Public Property FinancialTransactionDetails As List(Of Transactions)
End Class

Public Class NewAccountRequest
    Public Property StartDate As String
    Public Property EndDate As String
    Public Property Status As String
    Public Property CheckCardIssued As Boolean
End Class

Public Class CardIssuedResponse
    Inherits BaseResponse
    Public Property Count As Integer
End Class

Public Class GetQueryResponse
    Inherits BaseResponse
    Public Property Count As Integer
    Public Property PreferredCount As Integer
End Class

Public Class GetSegmentsResponse
    Public Property TotalCount As Double
    Public Property dt As DataTable
End Class


Public Class GetTransactionsResponse
    Public Property TotalCount As Double
    Public Property dt As DataTable

End Class

Public Class GetAccountSalesResponse
    Public Property RunningTotal As Long
    Public Property RunningPayments As Long
    Public Property TotalStores As Long
    Public Property dt As DataTable
End Class