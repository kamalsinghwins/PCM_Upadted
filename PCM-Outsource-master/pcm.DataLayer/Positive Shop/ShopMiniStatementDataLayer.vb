Imports Entities
Imports Npgsql
Imports NpgsqlTypes

Public Class ShopMiniStatementDataLayer
    Inherits DataAccessLayerBase
    Public Function GetAccountData(ByVal CardNumber As String,
                                   ByVal strAccountNumber As String) As ShopMiniStatement

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim ds As DataSet
        Dim tmpSQL As String
        Dim RG As New Utilities.clsUtil

        Dim AccountNumber As String = ""

        Dim OverDue As Double
        Dim TotalDue As Double

        Dim Current As Double
        Dim tmpAccountName As String = ""
        Dim CreditLimit As Double
        Dim TotalBalance As Double

        Dim Statement As New ShopMiniStatement

        Dim NewGuID As String = Guid.NewGuid.ToString

        'Get the current period
        Dim CurrentPeriod As String = ""
        tmpSQL = "SELECT current_period FROM general_settings"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = dr("current_period")
                Next
            Else
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        End Try

        'Get the account number from either the card number or the account number
        If strAccountNumber = "" Then
            'Get the account number
            tmpSQL = "SELECT account_number FROM card_details cd WHERE card_number = '" & RG.Apos(CardNumber) & "'"
            Try
                'ds = objDB.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        AccountNumber = dr("account_number")
                    Next
                Else
                    Statement.ReturnMessage = "This Card Number does not exist on the database"
                    'If (objDB IsNot Nothing) Then
                    '    objDB.CloseConnection()
                    'End If
                    Return Statement
                End If
            Catch ex As Exception
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return Nothing
            End Try
        Else
            AccountNumber = strAccountNumber
        End If


        'Get opening balance (which is the closing balance of the previous month
        Dim OpeningBalance As String
        tmpSQL = "SELECT total " & _
                 "FROM financial_closing_balances " & _
                 "WHERE account_number = '" & RG.Apos(AccountNumber) & "' AND current_period = '" & Val(CurrentPeriod) - 1 & "'"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    OpeningBalance = dr("total")
                Next
            Else
                OpeningBalance = "0"
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        End Try

        'Get account details
        tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.first_name,debtor_personal.initials," &
            "debtor_personal.title,debtor_personal.last_name,debtor_personal.cell_number," &
            "debtor_personal.dont_send_sms,financial_balances.total," &
            "financial_balances.current_balance,financial_balances.p30,financial_balances.p60,financial_balances.p90,financial_balances.p120," &
            "financial_balances.p150," &
            "debtor_addresses.postal_address_postal_code,debtor_addresses.postal_address_3,debtor_addresses.postal_address_2," &
            "debtor_addresses.postal_address_1," &
            "financial_balances.credit_limit," &
            "card_details.card_number " &
            "FROM " &
            "debtor_personal " &
            "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
            "LEFT OUTER JOIN debtor_addresses ON debtor_personal.account_number = debtor_addresses.account_number " &
            "LEFT OUTER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
            "WHERE debtor_personal.account_number = '" & RG.Apos(AccountNumber) & "'"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows

                    '2015-03-04 :: Updated calculation
                    OverDue = RG.Num(Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))

                    If Format(Now, "dd") >= 12 Then
                        OverDue = RG.Num(Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))
                        TotalDue = RG.Num(OverDue + Val(dr("p30") & ""))
                        Current = RG.Num(Val(dr("p30") & ""))
                    Else
                        OverDue = RG.Num(Val(dr("p30") & "") + Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))
                        TotalDue = RG.Num(OverDue + Val(dr("current_balance") & ""))
                        Current = RG.Num(Val(dr("current_balance") & ""))
                    End If


                    'OverDue = RG.Numb(Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))

                    'If Format(Now, "dd") >= 12 Then
                    '    Current = RG.Numb(Val(dr("p30") & ""))
                    'Else
                    '    Current = RG.Numb(Val(dr("current_balance") & "") + Val(dr("p30") & ""))
                    'End If

                    'TotalDue = RG.Numb(OverDue + Current)
                    tmpAccountName = dr("title") & " " & dr("first_name") & " " & dr("last_name") & ""
                    TotalBalance = RG.Numb(Val(dr("total") & ""))
                    CreditLimit = RG.Numb(Val(dr("credit_limit") & ""))
                Next
            Else
                Statement.ReturnMessage = "Internal Error"
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return Statement

            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        End Try

        Dim TransactionDescription As String = ""
        Dim tmpFormatDate As String

        Dim LineItems As New List(Of StatementLineItems)

        tmpSQL = "SELECT * FROM financial_transactions WHERE account_number = '" & RG.Apos(AccountNumber) & "' " & _
                 "AND current_period >= '" & Val(CurrentPeriod) - 2 & "' ORDER BY financial_transactions_id ASC"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim LineItem As New StatementLineItems
                    Select Case dr("transaction_type") & ""
                        Case "SALE"
                            '                         012345678901234567890123456789
                            TransactionDescription = "PURCHASE AT BRANCH: " & Mid$(dr("reference_number") & "", 1, 3) 'sale
                        Case "CN"
                            TransactionDescription = "CREDIT NOTE AT BRANCH: " & Mid$(dr("reference_number") & "", 1, 3) 'cn
                        Case "PAY"
                            TransactionDescription = "PAYMENT - THANK YOU" 'payment
                        Case "LCP"
                            '                         012345678901234567890123456789
                            TransactionDescription = "LOST CARD PROTECTION" 'lost card
                        Case "LEDD"
                            '                         1         11        21        31        
                            '                         0123456789012345678901234567890123
                            TransactionDescription = "LEDGER DEBIT: BALANCE AFFECT"  'ledger debit
                        Case "LEDC"
                            TransactionDescription = "LEDGER CREDIT: BALANCE AFFECT" 'ledger credit
                        Case "LEDDN"
                            TransactionDescription = "LEDGER DEBIT: BALANCE NO AFFECT" 'ledger debit
                        Case "LEDCN"
                            '                         1         11        21        31        
                            '                         012345678901234567890123456789012
                            TransactionDescription = "LEDGER CREDIT: BALANCE NO AFFECT" 'ledger credit
                        Case "INT"
                            TransactionDescription = "INTEREST CHARGE" 'interest
                    End Select

                    tmpFormatDate = dr("sale_date") & ""
                    '1234567890
                    '2013-02-15
                    'tmpFormatDate = Mid$(tmpFormatDate, 9, 2) & "/" & Mid$(tmpFormatDate, 6, 2) & "/" & Mid$(tmpFormatDate, 1, 4)
                    'tmpStatement.WriteLine(tmpFormatDate, dr("reference_number") & "", TransactionDescription, Util.Num(Val(dr("transaction_amount") & "")))
                    LineItem.DateOfTransaction = dr("sale_date") & ""
                    LineItem.TransactionAmount = RG.Numb(Val(dr("transaction_amount") & ""))
                    LineItem.TransactionDescription = TransactionDescription
                    LineItem.TransactionNumber = dr("reference_number") & ""

                    LineItems.Add(LineItem)
                Next

            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        End Try

        Statement.AccountName = tmpAccountName
        Statement.AccountNumber = AccountNumber
        Statement.ClosingBalance = RG.Num(TotalBalance)
        Statement.CreditLimit = CreditLimit
        Statement.Current = RG.Numb(Current)
        Statement.CurrentDate = Format(Now, "dd/MM/yyyy")
        Statement.LineItems = LineItems
        Statement.OpeningBalance = RG.Numb(OpeningBalance)
        Statement.Overdue = RG.Numb(OverDue)
        Statement.TotalDue = RG.Numb(TotalDue)

        'If (objDB IsNot Nothing) Then
        '    objDB.CloseConnection()
        'End If

        Return Statement

    End Function

    Public Function GetGiftCardData(ByVal CardNumber As String) As ShopMiniStatement

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim ds As DataSet
        Dim tmpSQL As String
        Dim RG As New Utilities.clsUtil

        Dim GiftCardInfo As New ShopMiniStatement

        tmpSQL = "SELECT card_details.current_status,card_gift_cards.balance " & _
                 "From " & _
                 "card_details " & _
                 "Inner Join card_gift_cards ON card_details.card_number = card_gift_cards.card_number " & _
                 "WHERE card_details.card_number = '" & CardNumber & "'"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("current_status") & "" <> "ACTIVE" Then
                        GiftCardInfo.ReturnMessage = "This card has been de-activated"
                        'If (objDB IsNot Nothing) Then
                        '    objDB.CloseConnection()
                        'End If
                        Return GiftCardInfo
                    End If

                    GiftCardInfo.AccountNumber = CardNumber
                    GiftCardInfo.AccountName = "GIFT CARD"
                    GiftCardInfo.CreditLimit = RG.Num(dr("balance") & "")
                    GiftCardInfo.TotalDue = "0.00"
                    GiftCardInfo.Overdue = "0.00"
                Next
            Else
                GiftCardInfo.ReturnMessage = "This Card Number cannot be found on the Database."
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return GiftCardInfo
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        End Try

        'If (objDB IsNot Nothing) Then
        '    objDB.CloseConnection()
        'End If

        Return GiftCardInfo

    End Function
End Class
