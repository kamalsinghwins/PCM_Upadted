Public Class Transactions

    Public Property tDate As String
    Public Property tTime As String
    Public Property tUser As String
    Public Property tType As String
    Public Property tReference As String
    Public Property tAmount As String
    Public Property tPeriod As String
    Public Property tRunningBalance As String
    Public Property tNotes As String
    Public Property tAccount_Number As String
    Public Property tAuth_Code As String

End Class

Public Class CheckCardRequest
    Public Property CardNumber As String
    Public Property AccountNumber As String
End Class

Public Class CheckCardResponse
    Inherits BaseResponse
    Public Property CardNumber As String
    Public Property AccountNumber As String
    Public Property Customer As String
    Public Property ID As String
End Class

Public Class ProcessTransactionRequest
    Public Property PaymentType As String
    Public Property CardNumber As String
    Public Property AccountNumber As String
    Public Property ShopCode As String
    Public Property Amount As String
    Public Property ShopAssistant As String
    Public Property TransactionType As String
End Class
Public Class ProcessTransactionResponse
    Inherits BaseResponse
    Public Property ProcessTransaction As String
End Class

Public Class CreditTransactionResponse
    Public Property Message As String
End Class
