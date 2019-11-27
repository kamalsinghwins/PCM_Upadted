Public Class Debtor

    Public Property IDNumber As String
    Public Property AccountNumber As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property CreditLimit As String
    Public Property Balance As String
    Public Property Overdue As String
    Public Property LastSaleDate As String
    Public Property LastSaleAmount As String
    Public Property LastPaymentDate As String
    Public Property LastPaymentAmount As String
    Public Property SendPromos As Boolean
    Public Property SelfActivate As Boolean
    Public Property ContactNumber As String
    Public Property HomeNumber1 As String
    Public Property HomeNumber2 As String
    Public Property WorkNumber As String
    Public Property AltNumber As String
    Public Property NextOfKin As String
    Public Property NextOfKinNumber As String
    Public Property SpouseContactNumber As String

    Public Property ConsumerRating As String

    Public Property NextContactTime As String

    Public Property CurrentContactLevel As String

    Public Property CurrentStatus As String

    Public Property ReturnMessage As String

    Public Property CardNumber As String
    Public Property LostCard As Boolean
    Public Property Autoincrease As Boolean

    Public Property EmployeeNumber As String
    Public Property BranchCode As String
    Public Property PreferredLanguage As String

    Public Property AgeAnalysis As List(Of Debtor_AgeAnalysis)
    Public Property ContactHistory As List(Of Debtor_ContactHistory)
    Public Property ChangeHistory As List(Of DebtorChangeLog)
    Public Property Transactions As List(Of Transactions)
    Public Property PaymentPlans As List(Of Debtor_PaymentPlan)
    Public Property ClosingBalances As List(Of Debtor_ClosingBalances)
    Public Property AlternativeContactNumbers As List(Of AlternativeNumbers)

    Public Property PayNewCard As Integer

    Public Property GuID As String

End Class

Public Class JournalEntryRequest
    'Public Property LoggedInUser As String
    Public Property AccountNumber As String
    Public Property TransactionType As String
    Public Property Amount As String
    Public Property BalanceAffected As Boolean
    Public Property AffectedPeriod As String
    Public Property User As String
    Public Property Notes As String
End Class

Public Class DebtorsSumResponse
    Inherits BaseResponse
    Public Property lblTA As String
    Public Property lblLCP As String
    Public Property lblLCPP As String
    Public Property lblAD As String
    Public Property lblAp As String
    Public Property lblPend As String
    Public Property lblPendP As String
    Public Property lblFraud As String
    Public Property lblFraudP As String
    Public Property lblSusp As String
    Public Property lblSuspP As String

    Public Property lblWO As String
    Public Property lblWOP As String

    Public Property lblBlock As String
    Public Property lblBlockP As String
    Public Property lblDD As String
    Public Property lblDDP As String

    Public Property lblLeg As String
    Public Property lblLegP As String
End Class

Public Class PostTransactionRequest
    'Public Property LoggedInUser As Integer
    Public Property AccountNumber As String
    Public Property TransactionType As String
    Public Property TransactionAmount As Double
    Public Property User As String
    Public Property PayType As String
    Public Property OverRidden As String
    Public Property BranchCode As String
    Public Property CardNumber As String
    Public Property CurrentAccountPeriod As Integer
    Public Property Notes As String
End Class

Public Class CardSumRequest
    Public Property CardNumber As String

End Class

Public Class GetDetails
    Inherits BaseResponse
    Public Property GetDatable As DataTable

End Class

Public Class HistoryRequest
    Public Property Accountnumber As String
End Class

Public Class GetHistoryResponse
    Inherits BaseResponse
    Public Property AccountChanges As DataTable
    Public Property Transactions As DataTable
    Public Property ClosingBalances As DataTable
    Public Property AgeAnalysis As DataTable
    Public Property PaymentPlans As DataTable
    'Public Property CheckAll As DataTable
    Public Property CreditLimit As String
    Public Property Balance As String


End Class

