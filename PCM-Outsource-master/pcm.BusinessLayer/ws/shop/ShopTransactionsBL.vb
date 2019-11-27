Imports pcm.DataLayer
Imports Entities

Public Class ShopTransactionsBL

    Dim _DLayer As ShopTransactionsDL

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New ShopTransactionsDL(CompanyCode)
    'End Sub

    Public Sub New()
        _DLayer = New ShopTransactionsDL
    End Sub

    Public Function ProcessCashUp(ByVal TransactionData As CashUpData) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.ProcessCashUp(TransactionData)

    End Function

    Public Function NewVAT(ByVal Password As String, ByVal BranchCode As String, ByVal RecordsUpdated As String,
                           ByVal TimeStampCompleted As String) As String

        If Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.NewVAT(BranchCode, RecordsUpdated, TimeStampCompleted)

    End Function

    Public Function ProcessCashTransaction(ByVal CashTransaction As CashTransaction, ByVal ServerPath As String) As String

        If CashTransaction.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.ProcessCashTransaction(CashTransaction, ServerPath)

    End Function

    Public Function ProcessReprints(ByVal ReprintTransacion As ReprintTransaction, ByVal ServerPath As String) As String

        If ReprintTransacion.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.ProcessReprint(ReprintTransacion, ServerPath)

    End Function

    Public Function ProcessHeader(ByVal TransactionData As TransactionMaster, ByVal ServerPath As String) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.ProcessHeader(TransactionData, ServerPath)

    End Function

    Public Function ProcessLineItems(ByVal TransactionData As TransactionLineItems, ByVal ServerPath As String) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.ProcessLineItems(TransactionData, ServerPath)

    End Function

    Public Function UpdateIBTTransactions(ByVal TransactionData As IBTTransactionData, ByVal ServerPath As String) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.UpdateIBTTransactions(TransactionData, ServerPath)

    End Function

    Public Function InsertIBTTransactions(ByVal TransactionData As IBTTransactionData, ByVal ServerPath As String) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.InsertIBTTransactions(TransactionData, ServerPath)

    End Function

    Public Function EmployeeNoClockOut(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        If EmployeeData.password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.EmployeeNoClockOut(EmployeeData, ServerPath)

    End Function

    Public Function InsertVoid(ByVal wsPassword As String,
                               ByVal _void As Void) As String

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.InsertVoid(_void)

    End Function

    Public Function LastUpdate(ByVal wsPassword As String,
                               ByVal ShopVersion As String, ByVal HOVersion As String,
                               ByVal UploaderVersion As String, ByVal ConfigVersion As String,
                               ByVal LastError As String, ByVal Queries As String,
                               ByVal IPAddess As String, ByVal BranchCode As String) As String

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.LastUpdate(ShopVersion, HOVersion, UploaderVersion, ConfigVersion, LastError, Queries, IPAddess, BranchCode)

    End Function

    Public Function EmployeeLogIn(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        If EmployeeData.password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.EmployeeLogIn(EmployeeData, ServerPath)

    End Function

    Public Function EmployeeAbsent(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        If EmployeeData.password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.EmployeeAbsent(EmployeeData, ServerPath)

    End Function

    Public Function EmployeeLogout(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        If EmployeeData.password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.EmployeeLogout(EmployeeData, ServerPath)

    End Function

    Public Function EmployeeUpdateTimeWorked(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        If EmployeeData.password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.EmployeeUpdateTimeWorked(EmployeeData, ServerPath)

    End Function

    Public Function UpdateStationaryCount(ByVal TransactionData As StationaryTransactionData, ByVal ServerPath As String) As String

        If TransactionData.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.UpdateStationaryCount(TransactionData, ServerPath)

    End Function

    Public Function StockEx(ByVal Password As String, ByVal BranchCode As String, ByVal GeneratedCode As String,
                            ByVal ipAddress As String, ByVal ServerPath As String) As String

        If Password <> "JaiRL10nFMNo$forany" Then
            Return "Incorrect WS Password"
        End If

        Return _DLayer.StockEx(BranchCode, GeneratedCode, ipAddress, ServerPath)

    End Function

    Public Function GetIBTOut(ByVal password As String,
                              ByVal receiving_branch_code As String,
                              ByVal sending_branch_code As String,
                              ByVal IBTOutNumber As String,
                              ByVal ServerPath As String) As IBTOut

        If password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Return _DLayer.GetIBTOut(receiving_branch_code, sending_branch_code, IBTOutNumber, ServerPath)

    End Function

    Public Function CreditCardAuth(ByVal CreditCardAuthoristaion As CreditCardAuth, ByVal ServerPath As String) As String

        If CreditCardAuthoristaion.Password <> "JaiRL10nFMNo$forany" Then
            Return ""
        End If

        Return _DLayer.CreditCardAuth(CreditCardAuthoristaion, ServerPath)

    End Function
End Class
