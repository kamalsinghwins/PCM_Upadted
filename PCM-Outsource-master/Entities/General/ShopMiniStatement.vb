Public Class ShopMiniStatement

    Public Property AccountNumber As String
    Public Property AccountName As String
    Public Property CurrentDate As String
    Public Property Current As String
    Public Property Overdue As String
    Public Property TotalDue As String
    Public Property OpeningBalance As String
    Public Property ClosingBalance As String
    Public Property CreditLimit As String
    Public Property ReturnMessage As String
    Public Property LineItems As List(Of StatementLineItems)

End Class

Public Class StatementLineItems

    Public Property DateOfTransaction As String
    Public Property TransactionNumber As String
    Public Property TransactionDescription As String
    Public Property TransactionAmount As String

End Class
