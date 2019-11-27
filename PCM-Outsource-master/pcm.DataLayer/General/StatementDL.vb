Imports Npgsql
Imports Entities


Public Class StatementDL

    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    Public Sub New()
        objDBRead = New dlNpgSQL("PostgreConnectionStringPCMRead")
        connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPCMRead").ConnectionString)

    End Sub

    Public Function GetAccountNumber(ByVal IDNumber As String, ByVal LastName As String) As String

        Dim AccountNumber As String = ""

        tmpSQL = "SELECT account_number FROM debtor_personal WHERE id_number = '" & Trim(RG.Apos(IDNumber)) & "' " &
                 "AND TRIM(both ' '  from UPPER(last_name)) = '" & Trim(RG.Apos(LastName.ToUpper)) & "'"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    AccountNumber = Val(dr("account_number") & "")
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return ""
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Return AccountNumber

    End Function

    Public Function PrintStatement(ByVal AccountNumber As String) As String

        Dim _tmpGuId As String = Guid.NewGuid.ToString

        Dim overDue As Long
        Dim totalDue As Long

        Dim openingBalance As Long
        Dim tmpP30 As Long
        Dim tmpAccountName As String = ""
        Dim creditLimit As Long
        Dim totalBalance As Long
        Dim addressLine1 As String = ""
        Dim addressLine2 As String = ""
        Dim addressLine3 As String = ""
        Dim addressPostCode As String = ""

        Dim CurrentPeriod As Integer

        tmpSQL = "SELECT minimum_payment,current_period FROM general_settings"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = Val(dr("current_period") & "")
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        End Try

        tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.first_name,debtor_personal.initials," &
                    "debtor_personal.title,debtor_personal.last_name,debtor_personal.cell_number," &
                    "debtor_personal.dont_send_sms,financial_balances.total," &
                    "financial_balances.current_balance,financial_balances.p30,financial_balances.p60,financial_balances.p90,financial_balances.p120," &
                    "financial_balances.p150," &
                    "debtor_addresses.postal_address_postal_code,debtor_addresses.postal_address_1,debtor_addresses.postal_address_2," &
                    "debtor_addresses.postal_address_3," &
                    "financial_balances.credit_limit,financial_balances.total," &
                    "card_details.card_number " &
                    "FROM " &
                    "debtor_personal " &
                    "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                    "LEFT OUTER JOIN debtor_addresses ON debtor_personal.account_number = debtor_addresses.account_number " &
                    "LEFT OUTER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                    "WHERE debtor_personal.account_number = '" & RG.Apos(AccountNumber) & "'"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows

                    addressLine1 = dr("postal_address_1") & ""
                    addressLine2 = dr("postal_address_2") & ""
                    addressLine3 = dr("postal_address_3") & ""
                    addressPostCode = dr("postal_address_postal_code") & ""

                    '2015-03-04 :: Updated calculation
                    overDue = RG.Num(Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))

                    If Format(Now, "dd") >= 12 Then
                        overDue = RG.Num(Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))
                        totalDue = RG.Num(overDue + Val(dr("p30") & ""))
                        tmpP30 = RG.Num(Val(dr("p30") & ""))
                    Else
                        overDue = RG.Num(Val(dr("p30") & "") + Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & ""))
                        totalDue = RG.Num(overDue + Val(dr("current_balance") & ""))
                        tmpP30 = RG.Num(Val(dr("current_balance") & ""))
                    End If

                    tmpAccountName = dr("title") & " " & Mid(dr("first_name"), 1, 1) & " " & dr("last_name") & ""

                    totalBalance = RG.Num(Val(dr("total") & ""))
                    creditLimit = RG.Num(Val(dr("credit_limit") & ""))

                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        End Try


        Dim tmpStatement As New WriteStatementDL(HttpContext.Current.Server.MapPath("data\" & _tmpGuId & ".XML"),
                                                      AccountNumber & "", tmpAccountName,
                                                      tmpP30, overDue, totalDue, openingBalance, totalBalance, creditLimit,
                                                      addressLine1, addressLine2, addressLine3, addressPostCode)



        Dim transactionDescription As String
        Dim tmpFormatDate As String = Format(Now, "yyyy-MM-dd")
        Dim ThreeMonthsAgo As Date
        ThreeMonthsAgo = CDate(tmpFormatDate)

        tmpSQL = "SELECT " &
                 "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date,transaction_type,reference_number,transaction_amount " &
                 "FROM financial_transactions WHERE account_number = '" & RG.Apos(AccountNumber) & "' " &
                 "AND sale_date >= '" & Format(ThreeMonthsAgo.AddDays(-90), "yyyy-MM-dd") & "' ORDER BY financial_transactions_id ASC"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows

                    Select Case dr("transaction_type") & ""
                        Case "SALE"
                            transactionDescription = "PURCHASE AT RAGE BRANCH: " & Mid$(dr("reference_number") & "", 1, 3) 'sale
                        Case "CN"
                            transactionDescription = "CREDIT NOTE AT RAGE BRANCH: " & Mid$(dr("reference_number") & "", 1, 3) 'cn
                        Case "PAY"
                            transactionDescription = "PAYMENT - THANK YOU" 'payment
                        Case "LCP"
                            transactionDescription = "RAGE LOST CARD PROTECTION" 'lost card
                        Case "LEDD"
                            transactionDescription = "RAGE LEDGER DEBIT - BALANCE AFFECT"  'ledger debit
                        Case "LEDC"
                            transactionDescription = "RAGE LEDGER CREDIT - BALANCE AFFECT" 'ledger credit
                        Case "LEDDN"
                            transactionDescription = "RAGE LEDGER DEBIT - BALANCE NO AFFECT" 'ledger debit
                        Case "LEDCN"
                            transactionDescription = "RAGE LEDGER CREDIT - BALANCE NO AFFECT" 'ledger credit
                        Case "INT"
                            transactionDescription = "RAGE INTEREST CHARGE" 'interest
                        Case Else
                            Exit Select
                    End Select

                    tmpStatement.WriteLine(dr("sale_date"), dr("reference_number") & "",
                                           transactionDescription, RG.Num(Val(dr("transaction_amount") & "")))

                Next
            End If
            'Throw New Exception()
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Dim _dlErrorLogging As New ErrorLogDL
            _dlErrorLogging.ErrorLogging(ex)

            tmpStatement.CloseStream()
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

        End Try

        tmpStatement.CloseStream()

        Return _tmpGuId

    End Function

End Class
