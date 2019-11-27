Imports Microsoft.VisualBasic
Imports pcm.Website.UTIL

Public Class clsWriteStatement
    Dim _StrmWriter As IO.StreamWriter
    Dim UTIL As New clsUtil

    Public Sub New(ByVal FileNameToCreate As String, ByVal AccountNumber As String, ByVal AccountName As String,
                   ByVal Current As String,
                   ByVal Overdue As String, ByVal TotalDue As String,
                   ByVal OpeningBalance As String, ByVal ClosingBalance As String,
                   ByVal CreditLimit As String,
                   ByVal AddressLine1 As String,
                   ByVal AddressLine2 As String,
                   ByVal AddressLine3 As String,
                   ByVal AddressPostCode As String)

        _StrmWriter = New IO.StreamWriter(FileNameToCreate)

        _StrmWriter.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
        _StrmWriter.WriteLine("<MainRoot>")
        _StrmWriter.WriteLine("<Accounts>")
        _StrmWriter.WriteLine("<AccountNumber>" & Amp(AccountNumber) & "</AccountNumber>")
        _StrmWriter.WriteLine("<AccountName>" & Amp(AccountName) & "</AccountName>")
        _StrmWriter.WriteLine("<CurrentDate>" & Format(Now, "dd/MM/yyyy") & "</CurrentDate>")
        _StrmWriter.WriteLine("<Current>" & UTIL.Num(Amp(Current)) & "</Current>")
        _StrmWriter.WriteLine("<Overdue>" & UTIL.Num(Amp(Overdue)) & "</Overdue>")
        _StrmWriter.WriteLine("<TotalDue>" & UTIL.Num(Amp(TotalDue)) & "</TotalDue>")
        _StrmWriter.WriteLine("<OpeningBalance>" & UTIL.Num(Amp(OpeningBalance)) & "</OpeningBalance>")
        _StrmWriter.WriteLine("<ClosingBalance>" & UTIL.Num(Amp(ClosingBalance)) & "</ClosingBalance>")
        _StrmWriter.WriteLine("<CreditLimit>" & UTIL.Num(Amp(CreditLimit)) & "</CreditLimit>")
        _StrmWriter.WriteLine("<AddressLine1>" & Amp(AddressLine1) & "</AddressLine1>")
        _StrmWriter.WriteLine("<AddressLine2>" & Amp(AddressLine2) & "</AddressLine2>")
        _StrmWriter.WriteLine("<AddressLine3>" & Amp(AddressLine3) & "</AddressLine3>")
        _StrmWriter.WriteLine("<AddressPostCode>" & Amp(AddressPostCode) & "</AddressPostCode>")
        _StrmWriter.WriteLine("</Accounts>")


    End Sub

    Public Sub WriteLine(ByVal TransactionDate As String, ByVal TransactionNumber As String, ByVal TransactionDescription As String, ByVal TransactionAmount As String)
        _StrmWriter.WriteLine("<Transactions ID=""" & TransactionNumber & """>")
        _StrmWriter.WriteLine("<TransactionDate>" & Amp(TransactionDate) & "</TransactionDate>")
        _StrmWriter.WriteLine("<TransactionDescription>" & Amp(TransactionDescription) & "</TransactionDescription>")
        _StrmWriter.WriteLine("<TransactionAmount>" & UTIL.Num(Amp(TransactionAmount)) & "</TransactionAmount>")

        _StrmWriter.WriteLine("</Transactions>")
    End Sub

    Public Sub CloseStream()
        _StrmWriter.WriteLine("</MainRoot>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Function Amp(ByVal funcString As String) As String

        If InStr(funcString, "&") Then
            funcString = funcString.Replace("&", "And")
        End If

        Return funcString

    End Function

End Class
