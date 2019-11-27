Public Class DebtorContactInvestigation

    'The reason for not combining this debtor with the Debtor class is that this debtor class does not require
    'the transactional data and the other debtor class doesn't require the debtor change history
    'As these queries are expensive, it will be more efficient to have different classes

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

    Public Property ConsumerRating As String

    Public Property ContactNumber As String
    Public Property HomeNumber1 As String
    Public Property HomeNumber2 As String
    Public Property WorkNumber As String
    Public Property AltNumber As String
    Public Property NextOfKin As String
    Public Property NextOfKinNumber As String
    Public Property SpouseContactNumber As String
    Public Property NextContactTime As String

    Public Property AgeAnalysis As List(Of Debtor_AgeAnalysis)
    Public Property ContactHistory As List(Of Debtor_ContactHistory)
    Public Property ChangeHistory As List(Of DebtorChangeLog)
    Public Property AlternativeContactNumbers As List(Of AlternativeNumbers)
    Public Property Transactions As List(Of Transactions)

    Public Property PreferredLanguage As String

    Public Property FirstContactInvestigationFailed As String

End Class

Public Class AlternativeNumbers

    Public Property NumberDescription As String
    Public Property NumberNumber As String
    Public Property NumberUpdateDate As String
    Public Property DateRecordInserted As String

End Class
