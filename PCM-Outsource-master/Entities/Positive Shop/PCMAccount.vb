Public Class PCMAccount

    Public Property wsPassword As String
    Public Property UserName As String
    Public Property AccountNumber As String
    Public Property CustomerName As String
    Public Property ContactNumber As String
    Public Property EMailAddress As String
    Public Property CardNumber As String
    Public Property IDNumber As String
    Public Property CreditLimit As String
    Public Property CreditAvailable As String
    Public Property CurrentBalance As String
    Public Property CurrentPayment As String
    Public Property AmountDueAfterPayment As String
    Public Property TransactionAmount As String
    Public Property TransactionType As String
    Public Property TransactionNumber As String
    Public Property CurrentAccountPeriod As String
    Public Property pCurrent As String
    Public Property p30 As String
    Public Property p60 As String
    Public Property p90 As String
    Public Property p120 As String
    Public Property p150 As String
    Public Property Total As String
    Public Property PayType As String
    Public Property ReturnMessage As String
    Public Property isFirstPurchase As Boolean
    Public Property GuID As String
    Public Property SpecialCondition As String
    Public Property SpecialPaymentAmount As String
    Public Property isSpecialPayment As Boolean
    Public Property Success As Boolean 'For on website manual payments
    Public Property AuthCode As String 'For on website manual payments
    Public Property isOnlineTransaction As Boolean 'For on website manual payments
    Public Property Notes As String 'For on website manual payments
    Public Property isDontSendSMS As Boolean 'To check if we need a new contact number



End Class
