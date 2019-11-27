Imports Npgsql
Imports Entities
Imports System.Globalization
Imports Entities.Reports

Public Class ReportsDataLayer

    Inherits DataAccessLayerBase

    Dim pos_connection As Npgsql.NpgsqlConnection = Nothing
    Dim pcm_connection As Npgsql.NpgsqlConnection = Nothing

    Dim objDBWritePCM As dlNpgSQL
    Dim objDBReadPCM As dlNpgSQL
    Dim objDBReadPOS As dlNpgSQL
    Dim objDBWrite As dlNpgSQL

    Dim where As String
    Private currentTurnoverResponse As New CurrentTurnoverResponse
    Private getSegmentsResponse As New GetSegmentsResponse
    Private getTransactionsResponse As New GetTransactionsResponse
    Private getAccountSalesResponse As New GetAccountSalesResponse
    Dim _dlErrorLogging As New ErrorLogDL
    Private searchResponse As New SearchResponse
    Dim baseresponse As New BaseResponse
    Dim employeeCasual As String


    Public Sub New(Optional ByVal CompanyCode As String = "")

        pcm_connection = Me.DataBase("PostgreConnectionStringPCMRead")
        pos_connection = Me.DataBase("PostgreConnectionStringPositiveRead")

        objDBWritePCM = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        objDBReadPCM = New dlNpgSQL("PostgreConnectionStringPCMRead")
        objDBReadPOS = New dlNpgSQL("PostgreConnectionStringPositiveRead")

    End Sub

    Public Sub New()
        pcm_connection = Me.DataBase("PostgreConnectionStringPCMRead")
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")

    End Sub


    Public Function BadDebtByStoreMaster(ByVal StartDate As String, ByVal EndDate As String,
                                         ByVal Period As String, ByVal BadDebtAmount As Double) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xDataPos As New DataTable

        Dim OneYearAgo As String
        OneYearAgo = Format(Now.Date.AddMonths(-12), "yyyy-MM-dd")

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPositiveRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT branch_code,branch_name FROM branch_details WHERE inserted >= '" & OneYearAgo & "';"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xDataPos)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        If xDataPos.Rows.Count > 0 Then
            For i As Integer = 0 To xDataPos.Rows.Count - 1

                tmpSQL = "SELECT branch_code FROM branch_details WHERE branch_code = '" & xDataPos.Rows(i)("branch_code") & "'"

                Try
                    Ds = objDBReadPCM.GetDataSet(tmpSQL)
                    If Not objDBReadPCM.isR(Ds) Then
                        tmpSQL = "INSERT INTO branch_details (branch_code,branch_name) VALUES ('" & xDataPos.Rows(i)("branch_code") & "','" & RG.Apos(xDataPos.Rows(i)("branch_name")) & "')"
                        Try
                            objDBWritePCM.ExecuteQuery(tmpSQL)
                        Catch ex As Exception
                            objDBWritePCM.CloseConnection()
                            Return Nothing
                        End Try
                    End If
                Catch ex As Exception
                    If (objDBReadPCM IsNot Nothing) Then
                        objDBReadPCM.CloseConnection()
                    End If
                    Return Nothing
                End Try
            Next
        End If

        If (objDBReadPOS IsNot Nothing) Then
            objDBReadPOS.CloseConnection()
        End If

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "debtor_first_purchase.first_purchase AS branch_code," &
                                     "SUM(financial_balances.total_spent) AS total_spent," &
                                     "COALESCE(Sum(case when p" & Period & " > " & BadDebtAmount & " THEN total end),0) AS bad_debt," &
                                     "COUNT(financial_balances.account_number) AS total_opened," &
                                     "COUNT(case when p" & Period & " > " & BadDebtAmount & " THEN financial_balances.account_number end) AS total_bad_accounts," &
                                     "((COUNT(case when p" & Period & " > " & BadDebtAmount & " THEN financial_balances.account_number end)::numeric(16,2) / COUNT(financial_balances.account_number)::numeric(16,2)) *100)::numeric(16,2) AS bad_people_percentage," &
                                     "((COALESCE(SUM(case when p" & Period & " > " & BadDebtAmount & " THEN total end)::numeric(16,2),0) / COALESCE(SUM(financial_balances.total_spent)::numeric(16,2),0)) *100)::numeric(16,2) AS bad_debt_percentage," &
                                     "MAX(branch_details.branch_name) AS branch_name " &
                                     "FROM debtor_dates " &
                                     "INNER JOIN debtor_first_purchase ON debtor_dates.account_number = debtor_first_purchase.account_number " &
                                     "INNER JOIN financial_balances ON debtor_first_purchase.account_number = financial_balances.account_number " &
                                     "INNER JOIN branch_details ON debtor_first_purchase.first_purchase = branch_details.branch_code " &
                                     "WHERE " &
                                     "debtor_dates.date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "'" &
                                     "AND total_spent > 0 AND (first_purchase NOT LIKE 'HO%' AND first_purchase <> '') " &
                                     "GROUP BY " &
                                     "debtor_first_purchase.first_purchase " &
                                     "ORDER BY debtor_first_purchase.first_purchase"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function BadDebtByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "debtor_personal.account_number," &
                                     "debtor_personal.id_number," &
                                     "debtor_personal.itc_rating," &
                                     "debtor_first_purchase.first_purchase AS branch_code," &
                                     "financial_balances.total," &
                                     "financial_balances.current_balance," &
                                     "financial_balances.p30," &
                                     "financial_balances.p60," &
                                     "financial_balances.p90," &
                                     "financial_balances.p120," &
                                     "financial_balances.p150," &
                                     "financial_balances.total_spent," &
                                     "financial_balances.credit_limit," &
                                     "debtor_dates.date_of_creation," &
                                     "CASE WHEN CAST(itc_rating AS numeric) > 6 THEN 'THICK' ELSE 'THIN' END as file_type " &
                                     "FROM debtor_personal " &
                                     "INNER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                                     "INNER JOIN debtor_first_purchase ON debtor_personal.account_number = debtor_first_purchase.account_number " &
                                     "INNER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                                     "WHERE debtor_dates.date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                                     "AND total_spent > 0 AND (first_purchase NOT LIKE 'HO%' AND first_purchase <> '')"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData
    End Function

    Public Function GetBadDebtData(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text


            Dim strQuery As String = "SELECT " &
                                         "b2.account_number, " &
                                         "b2.total, " &
                                         "b2.current_balance, " &
                                         "b2.p30, " &
                                         "b2.p60, " &
                                         "b2.p90, " &
                                         "b2.p120, " &
                                         "b2.p150, " &
                                         "b2.purchase_amount, " &
                                         "b2.interest_amount, " &
                                          SelectedPeriod & " AS current_period " &
                                     "FROM " &
                                         "financial_closing_balances b1 " &
                                     "JOIN financial_closing_balances b2 ON ( " &
                                         "b1.account_number = b2.account_number " &
                                     "AND b1.current_period = " & Val(SelectedPeriod) - 1 & " " &
                                     "AND b2.current_period = " & SelectedPeriod & " " &
                                     ") " &
                                     "WHERE " &
                                        "b1.p150 = 0 " &
                                     "AND b2.p150 > 0 " &
                                     "ORDER BY b1.account_number;"


            'Dim strQuery As String = "SELECT account_number,total,current_balance,p30,p60,p90,p120,p150," &
            '                         "purchase_amount,interest_amount,current_period " &
            '                         "FROM financial_closing_balances " &
            '                         "WHERE p150 > 0  " &
            '                         "AND current_period = " & SelectedPeriod & " " &
            '                         "AND account_number IN (" &
            '                            "SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 = 0 " &
            '                            "AND current_period = " & Val(SelectedPeriod) - 1 & ")"

            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetBadDebtRecoveredData(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "b1.account_number, " &
                                     "b1.total, " &
                                     "b1.current_balance, " &
                                     "b1.p30, " &
                                     "b1.p60, " &
                                     "b1.p90, " &
                                     "b1.p120, " &
                                     "b1.p150, " &
                                     "b1.purchase_amount, " &
                                     "b1.interest_amount, " &
                                     SelectedPeriod & " AS current_period " &
                                     "FROM " &
                                        "financial_closing_balances b1 " &
                                     "JOIN financial_closing_balances b2 ON ( " &
                                        "b1.account_number = b2.account_number " &
                                        "AND b1.current_period = " & Val(SelectedPeriod) - 1 & " " &
                                        "AND b2.current_period = " & SelectedPeriod & " " &
                                     ") " &
                                     "WHERE " &
                                        "b1.p150 > 0 " &
                                     "AND b2.p150 = 0 " &
                                     "ORDER BY b1.account_number;"


            'Dim strQuery As String = "SELECT account_number,total,current_balance,p30,p60,p90,p120,p150," &
            '                         "purchase_amount,interest_amount," & SelectedPeriod & " AS current_period " &
            '                         "FROM financial_closing_balances " &
            '                         "WHERE current_period = " & Val(SelectedPeriod) - 1 & " " &
            '                         "AND account_number IN (" &
            '                            "SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 = 0 AND current_period = " & SelectedPeriod & " " &
            '                         "AND account_number IN " &
            '                            "(SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 > 0 AND current_period = " & Val(SelectedPeriod) - 1 & "))"


            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt_recovered " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetReductionIn150(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT current_period,account_number," &
                                     "prev_balance,p150 new_balance, prev_balance - p150 AS paid," &
                                     "interest_amount,prev_interest_amount,prev_interest_amount - interest_amount as paid_interest," &
                                     "purchase_amount,prev_purchase_amount,prev_purchase_amount - purchase_amount as paid_purchase " &
                                     "FROM (" &
                                     "SELECT current_period,account_number,p150,interest_amount,purchase_amount," &
                                         "  LAG ( p150 ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_balance," &
                                         "	LAG ( interest_amount ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_interest_amount," &
                                         "	LAG ( purchase_amount ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_purchase_amount " &
                                         "  FROM financial_closing_balances " &
                                         "  WHERE current_period IN ('" & Val(SelectedPeriod) - 1 & "','" & SelectedPeriod & "')) w " &
                                     "WHERE prev_balance IS NOT NULL " &
                                     "AND prev_balance > p150 " &
                                     "AND p150 > 0"
            'Dim strQuery As String = "SELECT current_period,account_number,prev_balance,p150 new_balance, prev_balance - p150 AS paid " &
            '                         "FROM (" &
            '                            "SELECT current_period,account_number,p150" &
            '                            "LAG ( p150 ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_balance " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE current_period IN ('" & Val(SelectedPeriod) - 1 & "','" & SelectedPeriod & "')) w " &
            '                         "WHERE prev_balance IS NOT NULL " &
            '                         "AND prev_balance > p150 " &
            '                         "AND p150 > 0"


            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt_recovered " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function



    Public Function GetInternalPeriods() As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "internal_period ||' - '|| " &
                                     "real_month ||' ' || " &
                                     "real_year AS periods " &
                                     "FROM internal_period_to_date " &
                                     "WHERE internal_period >= 120 " &
                                     "ORDER BY internal_period"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetAgingSummary(ByVal MinimumAmount As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select c || ' days' AS aging, COUNT(account_number) AS number_of_accounts, SUM(total) AS total_of_accounts " &
                                     "FROM " &
                                        "(SELECT CASE " &
                                        "WHEN p150 > 0 THEN 150 " &
                                        "WHEN p120 > 0 THEN 120 " &
                                        "WHEN p90 > 0 THEN 90 " &
                                        "WHEN p60 > 0 THEN 60 " &
                                        "WHEN p30 > 0 THEN 30 " &
                                        "END AS c, * FROM financial_balances) s " &
                                     "WHERE c IS NOT NULL " &
                                     "AND total > " & Val(MinimumAmount) & " " &
                                     "GROUP BY c ORDER BY c;"
            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetAccountsToBeCalled(ByVal MinimumAmount As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT c || ' days' AS aging, TO_CHAR(COUNT(s.account_number), '999,999,999') AS number_of_accounts, " &
                                     "TO_CHAR(SUM(total), '999,999,999') AS total_of_accounts " &
                                     "FROM " &
                                     "   (SELECT CASE " &
                                     "    WHEN p150 > 0 THEN 150 " &
                                     "    WHEN p120 > 0 THEN 120 " &
                                     "    WHEN p90 > 0 THEN 90 " &
                                     "    WHEN p60 > 0 THEN 60 " &
                                     "    WHEN p30 > 0 THEN 30 " &
                                     "    END AS c, * FROM financial_balances) s" &
                                     "  INNER JOIN debtor_personal dp ON s.account_number = dp.account_number " &
                                     "WHERE s.current_contact_level BETWEEN 0 And 5 " &
                                     "      And s.next_contact_time < now() " &
                                     "      And total > " & MinimumAmount & " " &
                                     "    And dp.status = 'ACTIVE' " &
                                     "    AND contact_investigation = False " &
                                     "      AND is_legal = False " &
                                     "      AND under_investigation = False " &
                                     "      AND c IS NOT NULL " &
                                     "  GROUP BY c " &
                                     "  ORDER BY c;"
            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetAccountsSections() As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strquery As String = "SELECT " &
                                     "TO_CHAR(COUNT( CASE WHEN dont_send_sms = true THEN 1 END ), '999,999,999') AS dont_send_sms," &
                                     "TO_CHAR(COUNT( CASE WHEN contact_investigation = true THEN 1 END ), '999,999,999') AS contact_investigation," &
                                     "TO_CHAR(COUNT( CASE WHEN under_investigation = TRUE THEN 1 END ), '999,999,999') As under_investigation," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 0 THEN 1 END ), '999,999,999') As contact_level_0," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 1 THEN 1 END ), '999,999,999') As contact_level_1," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 2 Then 1 End ), '999,999,999') As contact_level_2," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 3 THEN 1 END ), '999,999,999') As contact_level_3," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 5 THEN 1 END ), '999,999,999') As contact_level_5," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 7 THEN 1 END ), '999,999,999') As contact_level_7 " &
                                     "FROM debtor_personal " &
                                     "LEFT OUTER JOIN financial_balances on financial_balances.account_number = debtor_personal.account_number"

            Dim reader As New NpgsqlDataAdapter(strquery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetAccountsSectionsActiveBalance() As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strquery As String = "SELECT " &
                                     "TO_CHAR(COUNT( CASE WHEN dont_send_sms = true THEN 1 END ), '999,999,999' ) AS dont_send_sms," &
                                     "TO_CHAR(COUNT( CASE WHEN contact_investigation = true THEN 1 END ), '999,999,999') AS contact_investigation," &
                                     "TO_CHAR(COUNT( CASE WHEN under_investigation = TRUE THEN 1 END ), '999,999,999') As under_investigation," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 0 THEN 1 END ), '999,999,999') As contact_level_0," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 1 THEN 1 END ), '999,999,999') As contact_level_1," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 2 Then 1 End ), '999,999,999') As contact_level_2," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 3 THEN 1 END ), '999,999,999') As contact_level_3," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 5 THEN 1 END ), '999,999,999') As contact_level_5," &
                                     "TO_CHAR(COUNT( CASE WHEN current_contact_level = 7 THEN 1 END ), '999,999,999') As contact_level_7 " &
                                     "FROM debtor_personal " &
                                     "LEFT OUTER JOIN financial_balances on financial_balances.account_number = debtor_personal.account_number " &
                                     "WHERE status = 'ACTIVE' and total <> 0"

            Dim reader As New NpgsqlDataAdapter(strquery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetIncomingSMS(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT time_stamp,id_number,first_name,last_name,salary,sms_result,status,credit_limit,received_from,sent_to," &
                                     "original_message,reply_sms,bureau_call," &
                                     "'--' || total_processing_time AS total_processing_time, " &
                                     "'--' || bureau_processing_time AS bureau_processing_time " &
                                     "FROM sms_incoming_sms " &
                                     "WHERE time_stamp between '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59' " &
                                     "ORDER BY time_stamp"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetSalesPayments(ByVal OpenedFrom As String, ByVal OpenedTo As String,
                                     ByVal SalesFrom As String, ByVal SalesTo As String,
                                     ByVal PaymentsFrom As String, ByVal PaymentsTo As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT financial_transactions.account_number,SUM(CASE WHEN transaction_type = 'SALE' THEN 1 END) AS number_of_sales, " &
                                     "SUM(Case When transaction_type = 'PAY' THEN 1 END) AS number_of_payments, " &
                                     "SUM(CASE WHEN transaction_type = 'SALE' THEN transaction_amount END) AS sales, " &
                                     "SUM(CASE WHEN transaction_type = 'PAY' THEN transaction_amount END) AS payments, " &
                                     "MAX(itc_rating) AS itc_rating, " &
                                     "MAX(original_credit_limit) as original_credit_limit, " &
                                     "MAX(dp.branch_code) as branch_code, " &
                                     "MAX(bd.branch_name) as branch_name, " &
                                     "MAX(fcl.credit_limit) as current_credit_limit, " &
                                     "MAX(fcl.total) AS balance " &
                                     "FROM financial_transactions  " &
                                     "LEFT OUTER JOIN debtor_personal dp ON dp.account_number = financial_transactions.account_number " &
                                     "LEFT OUTER JOIN financial_balances fcl ON fcl.account_number = financial_transactions.account_number " &
                                     "LEFT OUTER JOIN branch_details bd ON dp.branch_code = bd.branch_code " &
                                     "WHERE ((financial_transactions.transaction_type = 'SALE' AND financial_transactions.sale_date " &
                                     "BETWEEN '" & SalesFrom & "' AND '" & SalesTo & "') " &
                                     "OR (financial_transactions.transaction_type = 'PAY' AND financial_transactions.sale_date " &
                                     "BETWEEN '" & PaymentsFrom & "' AND '" & PaymentsTo & "')) " &
                                     "AND financial_transactions.account_number  " &
                                     "IN (  " &
                                     "SELECT debtor_dates.account_number  " &
                                     "FROM debtor_dates  " &
                                     "INNER JOIN financial_balances ON debtor_dates.account_number = financial_balances.account_number  " &
                                     "WHERE debtor_dates.date_of_creation BETWEEN '" & OpenedFrom & "' AND '" & OpenedTo & "' AND total_spent > 0  " &
                                     "GROUP BY debtor_dates.account_number) GROUP BY financial_transactions.account_number  "

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetLastUpdateData() As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pos_connection.Open()
            command.Connection = pos_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select " &
                                    "branch_details.branch_name," &
                                    "version_numbers.branch_code," &
                                    "version_numbers.last_update_date," &
                                    "version_numbers.last_update_time," &
                                    "version_numbers.positive_shop_version," &
                                    "version_numbers.uploader_version," &
                                    "version_numbers.config_version," &
                                    "version_numbers.last_error," &
                                    "version_numbers.queries_uploaded," &
                                    "version_numbers.positive_ho_version," &
                                    "version_numbers.ip_address " &
                                    "FROM version_numbers " &
                                    "INNER JOIN branch_details On version_numbers.branch_code = branch_details.branch_code " &
                                    "ORDER BY branch_name"

            Dim reader As New NpgsqlDataAdapter(strQuery, pos_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pos_connection IsNot Nothing) Then
                pos_connection.Close()
            End If

        End Try

        Return xData


    End Function


    Public Function ReturnBureauLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select " &
                        "TO_CHAR(web_service_logs.log_date, 'YYYY-MM-DD') AS logdate," &
                        "TO_CHAR(web_service_logs.log_time, 'HH24:MI:SS') AS logtime," &
                        "web_service_logs.id_number idnumber," &
                        "web_service_logs.account_number accountnumber," &
                        "web_service_logs.pre_existing preexisting," &
                        "web_service_logs.status originalstatus," &
                        "debtor_personal.status currentstatus," &
                        "debtor_personal.itc_rating itcrating," &
                        "financial_balances.credit_limit creditlimit " &
                        "FROM " &
                        "web_service_logs " &
                        "LEFT OUTER JOIN debtor_personal ON web_service_logs.account_number = debtor_personal.account_number " &
                        "LEFT OUTER JOIN financial_balances ON web_service_logs.account_number = financial_balances.account_number " &
                        "WHERE log_date BETWEEN '" & StartDate & "' AND '" & EndDate & "'"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnPTPListing(ByVal PTPDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "DISTINCT ON (fb.account_number) " &
                                     "fb.account_number," &
                                     "dp.first_name," &
                                     "dp.last_name," &
                                     "fb.total," &
                                     "fb.current_balance," &
                                     "fb.p30," &
                                     "fb.p60," &
                                     "fb.p90," &
                                     "fb.p120," &
                                     "fb.p150," &
                                     "dch.ptp_date," &
                                     "dch.ptp_amount," &
                                     "dch.username " &
                                     "FROM financial_balances AS fb " &
                                     "INNER JOIN debtor_personal AS dp ON fb.account_number = dp.account_number " &
                                     "INNER JOIN debtor_contact_history dch ON fb.account_number = dch.account_number " &
                                     "WHERE " &
                                     "fb.current_contact_level = 5 " &
                                     "AND result_of_action = 'PTP' " &
                                     "AND ptp_date >= '" & PTPDate & "' " &
                                     "AND p60 + p90 + p120 + p150 > " & MinimumCollectionAmount & " " &
                                     "ORDER BY " &
                                     "fb.account_number, dch.ptp_date DESC;"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnSMSReportData(ByVal SMSType As String, ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "TO_CHAR(sms_date, 'YYYY-MM-DD') AS sms_date," &
                                     "TO_CHAR(sms_time, 'HH24:MI:SS') AS sms_time," &
                                     "type_of_sms,account_number,cellphone_number,sms_message,sms_reply," &
                                     "current_balance,credit_limit,user_name,sms_sent,reason,amount_owing,amount_to_pay " &
                                     "FROM sms_sending " &
                                     "WHERE sms_date >= '" & StartDate & "' and sms_date <= '" & EndDate & "'"

            If SMSType <> "All" Then
                strQuery &= " AND type_of_sms = '" & SMSType & "'"
            End If

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnPaymentsVSContactsReportData(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "DISTINCT ON (financial_transactions.account_number)  " &
                                     "financial_transactions.account_number," &
                                     "TO_CHAR(financial_transactions.sale_date, 'YYYY-MM-DD') AS sale_date," &
                                     "sale_time," &
                                     "financial_transactions.reference_number," &
                                     "financial_transactions.transaction_amount," &
                                     "debtor_contact_history.timestamp_of_contact," &
                                     "debtor_contact_history.result_of_action," &
                                     "debtor_contact_history.ptp_amount," &
                                     "debtor_contact_history.ptp_date," &
                                     "debtor_contact_history.username " &
                                     "FROM financial_transactions " &
                                     "LEFT OUTER JOIN debtor_contact_history ON financial_transactions.account_number = debtor_contact_history.account_number " &
                                     "WHERE sale_date between '" & FromDate & "' and '" & ToDate & "' " &
                                     "AND transaction_type = 'PAY' " &
                                     "ORDER BY " &
                                     "financial_transactions.account_number, debtor_contact_history.timestamp_of_contact DESC;"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnInvestigationListing(ByVal Status As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String

            If Status = "Debt Review" Then
                strQuery = "SELECT " &
                                     "fb.account_number," &
                                     "dp.first_name," &
                                     "dp.last_name," &
                                     "fb.total," &
                                     "fb.current_balance," &
                                     "fb.p30," &
                                     "fb.p60," &
                                     "fb.p90," &
                                     "fb.p120," &
                                     "fb.p150, " &
                                     "fb.current_contact_level, " &
                                     "debtor_date.date_of_last_payment" &
                                     " FROM financial_balances fb " &
                                     "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number " &
                                     "LEFT JOIN (select dd.account_number, dd.date_of_last_payment,Row_number() over(partition by account_number order by date_of_last_payment DESC) as seqnum from debtor_dates dd) debtor_date On debtor_date.account_number=fb.account_number and seqnum=1 "
            Else

                strQuery = " Select  " &
                                     "fb.account_number," &
                                     "dp.first_name," &
                                     "dp.last_name," &
                                     "fb.total," &
                                     "fb.current_balance," &
                                     "fb.p30," &
                                     "fb.p60," &
                                     "fb.p90," &
                                     "fb.p120," &
                                     "fb.p150, " &
                                     "fb.current_contact_level " &
                                     "FROM financial_balances fb " &
                                     "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number "
                xData.Columns.Add("date_of_last_payment")

            End If

            If Status = "Under Investigation" Then
                strQuery &= " WHERE under_investigation = True AND dp.contact_investigation = False AND dp.is_legal = False ORDER BY fb.account_number"
            ElseIf Status = "Contact Investigation" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = True AND dp.is_legal = False ORDER BY fb.account_number"
            ElseIf Status = "Legal" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = False AND dp.status = 'LEGAL' ORDER BY fb.account_number"
            ElseIf Status = "Debt Review" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = False AND dp.status = 'DEBT REVIEW' ORDER BY fb.account_number"
            ElseIf Status = "All Active" Then
                strQuery &= " WHERE dp.status = 'ACTIVE' ORDER BY fb.account_number"
            ElseIf Status = "Fraud" Then
                strQuery &= " WHERE dp.status = 'FRAUD' ORDER BY fb.account_number"
            ElseIf Status = "Out of Queue" Then
                strQuery &= " WHERE fb.current_contact_level = '6' ORDER BY fb.account_number"
            End If

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)
        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function


    Public Function GetLimitIncreaseReport(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "select date_of_run,account_number,previous_limit,new_limit,limit_increased,auto_increase,current_balance,total_spent,income_amount,total_payments,number_sms_sent,additional_notes,sms_reply,guid from debtor_limit_increase where date_of_run between '" & FromDate & "' and '" & ToDate & "'"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetErrors(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        Dim xData As New DataTable
        pos_connection = Me.DataBase("PostgreConnectionStringPositiveRead")

        Try
            Dim command As New NpgsqlCommand()
            pos_connection.Open()
            command.Connection = pos_connection
            command.CommandType = CommandType.Text
            command.CommandType = CommandType.Text
            Dim strQuery As String = "select error_log_id,inner_exception,message,source,stack_trace,help_link,hresult,target_site,data,created_date from error_log where created_date between '" & FromDate & " 12:00:00 AM' and '" & ToDate & " 11:59:59 PM' order by created_date desc"

            Dim reader As New NpgsqlDataAdapter(strQuery, pos_connection)
            reader.Fill(xData)



        Catch ex As Exception
            Throw ex
        Finally
            If (pos_connection IsNot Nothing) Then
                pos_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetCurrentTurnover(ByVal OrderBy As String) As CurrentTurnoverResponse
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim drow As DataRow
        Dim runningTotal As Long = 0
        Dim runningTotalProfit As Long = 0
        Dim TotalStores As Long = 0
        Dim dsData As New DataSet

        dt.Columns.Add("branch")
        Dim colturnover As DataColumn = New DataColumn("turnover")
        colturnover.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colturnover)

        Dim colprofit As DataColumn = New DataColumn("profit")
        colprofit.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colprofit)

        dsData.Tables.Add(dt)

        tmpSQL = "SELECT * FROM current_turnover_mv "
        If OrderBy = "By Branch Name" Then
            tmpSQL &= "ORDER BY branch_name ASC"
        Else
            tmpSQL &= "ORDER BY sales DESC"
        End If

        Try
            Ds = objDBPositive.GetDataSet(tmpSQL)
            If objDBPositive.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotalProfit += (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    dt.Rows.Add(drow)
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try
        currentTurnoverResponse.GetDataResponse = dt
        currentTurnoverResponse.RunningTotal = runningTotal
        currentTurnoverResponse.RunningTotalProfit = runningTotalProfit
        currentTurnoverResponse.TotalStores = TotalStores
        Return currentTurnoverResponse

    End Function
    Public Function GenerateData(ByVal OrderBy As String, ByVal StartDate As String, ByVal EndDate As String) As CurrentTurnoverResponse
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim drow As DataRow
        Dim runningTotal As Long = 0
        Dim runningTotalProfit As Long = 0
        Dim TotalStores As Long = 0
        Dim dsData As New DataSet

        dt.Columns.Add("branch")
        Dim colturnover As DataColumn = New DataColumn("turnover")
        colturnover.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colturnover)

        Dim colprofit As DataColumn = New DataColumn("profit")
        colprofit.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colprofit)

        dsData.Tables.Add(dt)

        tmpSQL = "SELECT " &
                 "branch_code,branch_name," &
                 "SUM(CASE when item_num=1 AND transaction_type = 'POSSALE' then transaction_total end) as sales," &
                 "SUM(CASE when item_num=1 AND transaction_type = 'POSREF' then transaction_total end) as refunds," &
                 "SUM(CASE WHEN transaction_type = 'POSSALE' THEN estimated_cost_price * quantity ELSE 0 END) AS cost_sale_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSSALE' THEN selling_price * quantity ELSE 0 END) AS sell_sale_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSREF' THEN estimated_cost_price * quantity ELSE 0 END) AS cost_refunds_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSREF' THEN selling_price * quantity ELSE 0 END) AS sell_refunds_ex " &
                 "FROM " &
                 "(SELECT d.branch_code,p.branch_name,d.transaction_type,d.transaction_total,g.selling_price,g.estimated_cost_price,g.quantity,row_number() " &
                 "OVER (PARTITION by d.branch_code,g.link_guid) as item_num " &
                 "FROM transaction_master d " &
                 "JOIN transaction_line_items g ON d.guid = g.link_guid " &
                 "JOIN branch_details p ON d.branch_code = p.branch_code " &
                 "WHERE d.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "')" &
                 "AS s GROUP BY branch_code,branch_name "

        If OrderBy = "By Branch Name" Then
            tmpSQL &= "ORDER BY branch_name ASC"
        Else
            tmpSQL &= "ORDER BY sales DESC"
        End If

        Try
            Ds = objDBPositive.GetDataSet(tmpSQL)
            If objDBPositive.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotalProfit += (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    dt.Rows.Add(drow)
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try
        currentTurnoverResponse.GetDataResponse = dt
        currentTurnoverResponse.RunningTotal = runningTotal
        currentTurnoverResponse.RunningTotalProfit = runningTotalProfit
        currentTurnoverResponse.TotalStores = TotalStores
        Return currentTurnoverResponse
    End Function

    Public Function GetTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse

        tmpSQL = "SELECT  " &
                    "sale_date," &
                    "ROUND(AVG(transaction_amount),2) as avg," &
                    "ROUND(SUM(transaction_amount),2) as sum," &
                    "COUNT(account_number) as count " &
                 "FROM financial_transactions " &
                    "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND transaction_type = 'SALE' " &
                 "GROUP BY sale_date"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)

            End If



        Catch ex As Exception
            Throw ex
        End Try
        getTransactionsResponse.dt = dt

        Dim totalCount As Double = 0

        tmpSQL = "Select COUNT(DISTINCT account_number) as total_count  from financial_transactions
                    where sale_date between '" & StartDate & "' and  '" & EndDate & "'
                    And transaction_type = 'SALE'"

        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                totalCount = Ds.Tables(0).Rows(0)("total_count")
            End If
        Catch ex As Exception
            Throw ex
        End Try




        getTransactionsResponse.TotalCount = totalCount

        Return getTransactionsResponse
    End Function

    Public Function GetCashTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse

        tmpSQL = "SELECT sale_date, ROUND(AVG(sale_total), 2) as avg, COUNT(sale_total) AS count, ROUND(SUM(sale_total),2) as sum " &
                        "From cash_transactions " &
                        "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                        "And transaction_type = 'POSSALE' AND account = 0 " &
                        "Group BY sale_date " &
                        "ORDER BY sale_date"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)


            End If
        Catch ex As Exception
            Throw ex
        End Try
        getTransactionsResponse.dt = dt

        'Dim totalCount As Double = 0

        'tmpSQL = "Select COUNT(DISTINCT sale_total) as total_count  from cash_transactions where sale_date between '" & StartDate & "' and  '" & EndDate & "' And transaction_type = 'POSSALE' AND account = 0"

        'Try
        '    Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

        '    If usingObjDB.isR(Ds) Then
        '        totalCount = Ds.Tables(0).Rows(0)("total_count")
        '    End If
        'Catch ex As Exception
        '    Throw ex
        'End Try

        'getTransactionsResponse.TotalCount = totalCount

        Return getTransactionsResponse
    End Function

    Public Function GetBranchDetails() As DataTable
        tmpSQL = "SELECT branch_code,
                  branch_name
                 FROM branch_details
                 ORDER by branch_code ASC"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function


    Public Function GetSegments(StartDate As String, EndDate As String, DateRange As String, Segment As String, AllBranches As Boolean, Optional ByVal strBranch As String = "") As GetSegmentsResponse
        Dim arrayWeekday(8) As String
        Dim arrayValue(8) As String

        Dim dsData As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("segment")
        dt.Columns.Add("value")
        dsData.Tables.Add(dt)

        Dim drow As DataRow
        Dim TotalCount As Double = 0

        tmpSQL = "SELECT " &
                 "SUM(CASE WHEN m.transaction_type = 'POSSALE' THEN m.transaction_total ELSE 0 END) as sales, " &
                 "SUM(CASE WHEN m.transaction_type = 'POSCN' THEN m.transaction_total ELSE 0 END) as cn, " &
                 "SUM(CASE WHEN m.transaction_type = 'POSREF' THEN m.transaction_total ELSE 0 END) as refunds, "

        If Segment = "Hours" Then
            tmpSQL = tmpSQL & "t.currrent_hour AS segment "
        ElseIf Segment = "Weeks" Then
            tmpSQL = tmpSQL & "t.current_week AS segment "
        ElseIf Segment = "Months" Then
            tmpSQL = tmpSQL & "t.period AS segment "
        ElseIf Segment = "Day of Week" Then
            tmpSQL = tmpSQL & "t.current_weekday AS segment "
        End If

        tmpSQL = tmpSQL & "FROM transaction_master m " &
                          "LEFT OUTER JOIN transaction_times t ON m.guid = t.guid " &
                          "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND " &
                          "(m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') "

        'Get the branches
        If Not String.IsNullOrWhiteSpace(strBranch) Then
            tmpSQL = tmpSQL & " AND (" & strBranch & ") "
        End If

        tmpSQL = tmpSQL & "GROUP BY segment " &
                          "ORDER BY segment"


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalCount += 1
                    'Sort the weekdays
                    If Segment = "Day of Week" Then
                        If drBranch("segment") & "" = "Monday" Then
                            arrayWeekday(0) = "Monday"
                            arrayValue(0) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Tuesday" Then
                            arrayWeekday(1) = "Tuesday"
                            arrayValue(1) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Wednesday" Then
                            arrayWeekday(2) = "Wednesday"
                            arrayValue(2) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Thursday" Then
                            arrayWeekday(3) = "Thursday"
                            arrayValue(3) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Friday" Then
                            arrayWeekday(4) = "Friday"
                            arrayValue(4) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Saturday" Then
                            arrayWeekday(5) = "Saturday"
                            arrayValue(5) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Sunday" Then
                            arrayWeekday(6) = "Sunday"
                            arrayValue(6) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        End If
                    Else
                        drow = dt.NewRow
                        drow(0) = drBranch("segment")
                        drow(1) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        dt.Rows.Add(drow)
                    End If
                Next
            End If

            If Segment = "Day of Week" Then
                For zLoop As Integer = 0 To 6
                    drow = dt.NewRow
                    drow(0) = arrayWeekday(zLoop)
                    drow(1) = arrayValue(zLoop)
                    dt.Rows.Add(drow)
                Next
            End If

        Catch ex As Exception
            Throw ex
        End Try
        getSegmentsResponse.TotalCount = TotalCount
        getSegmentsResponse.dt = dt
        Return getSegmentsResponse
    End Function

    Public Sub InsertUserRecord(ByVal Username As String, ByVal isManager As String,
                             ByVal IPAddress As String, ByVal TypeOfAction As String,
                             ByVal ReportName As String, ByVal ReportOptions As String)


        tmpSQL = "INSERT INTO web_users_logs (user_name,is_admin,ip_address,type_of_action,report_name,report_options) VALUES " &
                 "('" & RG.Apos(Mid$(Username, 1, 30)) & "','" & isManager & "','" & RG.Apos(Mid$(IPAddress, 1, 150)) & "'," &
                 "'" & RG.Apos(Mid$(TypeOfAction, 1, 100)) & "','" & RG.Apos(Mid$(ReportName, 1, 100)) & "','" & RG.Apos(Mid$(ReportOptions, 1, 250)) & "')"

        Try
            usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function GetSentSMSLog(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        tmpSQL = "SELECT sms_log_id,
                   user_name,
                   data,status,
                   error_message,
                   result,
                   created_date,
                   sent_message,
                   total_phone_numbers_count,
                    failed_phone_number_count
                   FROM sent_sms_log
                   ORDER by created_date desc"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetSentEmployeeDetails() As DataTable
        tmpSQL = "SELECT
                 employee_number,
                 first_name,
                 last_name,
                 id_number, 
                 email_address,
                 cellphone, 
                 last_login 
                 FROM employee_details
                 ORDER by date_last_updated desc"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetBestSellersReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Trim As String) As DataSet
        Dim data As DataSet

        tmpSQL = "SELECT " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN l.quantity END) AS qty, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.selling_price * l.quantity + 0.001) END) as selling," &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.estimated_cost_price * l.quantity) END) AS estimated, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.average_cost_price * l.quantity) END) AS average, "

        tmpSQL = tmpSQL & Trim
        tmpSQL = tmpSQL & "min(l.description) as description " &
                          "FROM transaction_master m " &
                          "INNER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                          "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                          "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                          "GROUP BY stockcode ORDER BY qty DESC nulls LAST"


        Dim masterData As New DataTable

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                masterData = Ds.Tables(0).Copy()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        masterData.PrimaryKey = New DataColumn() {masterData.Columns("stockcode")}
        masterData.TableName = "Master"


        tmpSQL = "SELECT m.sale_date,b.branch_name,m.transaction_type,"

        tmpSQL = tmpSQL & Trim

        tmpSQL &= "(l.selling_price * l.quantity) * (1 + l.tax_percentage /100) as selling_price,l.quantity " &
                 "FROM transaction_master m " &
                 "LEFT OUTER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                 "INNER JOIN branch_details b ON m.branch_code = b.branch_code " &
                 "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                 "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                 "ORDER BY sale_date DESC nulls LAST"

        Dim detailData As New DataTable
        Dim relation As DataRelation

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                detailData = Ds.Tables(0).Copy()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        detailData.PrimaryKey = New DataColumn() {detailData.Columns("guid")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        relation = New DataRelation("MasterDetail", masterData.Columns("stockcode"), detailData.Columns("stockcode"))
        data.Relations.Add(relation)


        Return data
    End Function

    Public Function GetAccountSales(StartDate As String, EndDate As String) As GetAccountSalesResponse

        Dim dsData As New DataSet
        Dim dt As New DataTable
        Dim drow As DataRow





        dt.Columns.Add("branch")
        Dim colturnover As DataColumn = New DataColumn("turnover")
        colturnover.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colturnover)
        Dim colpayments As DataColumn = New DataColumn("payments")
        colpayments.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colpayments)

        Dim runningTotal As Long = 0
        Dim runningPayments As Long = 0
        Dim TotalStores As Long = 0

        tmpSQL = "SELECT " &
             "d.branch_code,p.branch_name," &
             "SUM(CASE WHEN transaction_type = 'POSSALE' THEN d.account ELSE 0 END) AS sales," &
             "SUM(CASE WHEN transaction_type = 'POSREF' THEN d.account ELSE 0 END) AS refunds," &
             "SUM(CASE WHEN transaction_type = 'ACCPAY' THEN d.sale_total ELSE 0 END) AS payments " &
             "FROM cash_transactions d " &
             "JOIN branch_details p ON d.branch_code = p.branch_code " &
             "WHERE d.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
             "GROUP BY d.branch_code,p.branch_name ORDER BY d.branch_code ASC"


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = drBranch("payments")
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    runningPayments += Val(drBranch("payments"))
                    dt.Rows.Add(drow)
                Next
            End If


        Catch ex As Exception
            Throw ex
        End Try

        getAccountSalesResponse.TotalStores = TotalStores
        getAccountSalesResponse.RunningTotal = runningTotal
        getAccountSalesResponse.RunningPayments = runningPayments
        getAccountSalesResponse.dt = dt
        Return getAccountSalesResponse
    End Function

    Public Function GetMasterEmployeesPerStore(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Dim WhereCondition As String = String.Empty
        Dim subQuery As String = String.Empty
        Dim query As String = String.Empty
        Dim permantQuery As String = String.Empty
        Dim casualQuery As String = String.Empty

        Dim ColumnsList As String = " branch_details.branch_code,branch_name,"

        If (All = True) Then
            query = ""
            permantQuery = ""
            casualQuery = ""
        ElseIf Permanent = True Then
            query = "and is_casual=false"

            casualQuery = IIf(Permanent, " and 1=2", "")
            permantQuery = ""
        ElseIf Casual = True Then
            query = "and is_casual=true"

            casualQuery = ""
            permantQuery = IIf(Casual, " and 1=2", "")
        End If


        subQuery &= "(Select count(emptr.branch_code) from employee_transactions empTr inner join employee_details as emp on emp.employee_number = emptr.employee_number and emp.last_guid = emptr.guid where emptr.branch_code = branch_details.branch_code and 
                         is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'  AND '" & EndDate & " 23:59:59' and
                      emp.is_casual=true" & casualQuery & "),
                     (Select count(emptr.branch_code) from employee_transactions empTr inner join employee_details as emp on emp.employee_number = emptr.employee_number and emp.last_guid = emptr.guid where emptr.branch_code = branch_details.branch_code and 
                     is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'   AND '" & EndDate & " 23:59:59' and
                     emp.is_casual=false" & permantQuery & ") as count1"
        where = " FROM branch_details  
                     left join employee_transactions on employee_transactions.branch_code = branch_details.branch_code
                    left join employee_details on employee_details.employee_number = employee_transactions.employee_number and employee_details.last_guid = employee_transactions.guid 
                   where  is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'   AND '" & EndDate & " 23:59:59' " & query & " and 
                  branch_details.is_blocked=false"

        subQuery &= where & " group by branch_details.branch_code ORDER BY count"


        Dim dt As New DataSet

        Try
            tmpSQL = "Select " & ColumnsList & " " & " " & subQuery & ""
            dt = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            dt.Tables.Add(GetEmployeesPerStoreDetails(StartDate, EndDate, where))
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetEmployeesPerStoreDetails(StartDate As String, EndDate As String, query As String) As DataTable


        tmpSQL = "SELECT " &
                 "employee_transactions.employee_number, " &
                 "employee_details.first_name ||' '|| employee_details.last_name AS employee_name, " &
                 "employee_details.is_logged_in, " &
                 "employee_transactions.branch_code, " &
                 "employee_transactions.clocking_date_in, " &
                 "employee_transactions.clocking_hour_in, " &
                 "employee_transactions.clocking_date_out, " &
                 "employee_transactions.clocking_hour_out, " &
                 "employee_transactions.time_worked, " &
                 "employee_transactions.reason " &
                 "FROM employee_transactions " &
                 "INNER JOIN employee_details ON employee_transactions.employee_number = employee_details.employee_number " &
                 "WHERE  clocking_time_in BETWEEN '" & StartDate & " 00:00:00' " &
                 "AND '" & EndDate & " 23:59:59'"

        If query <> "" Then
            tmpSQL &= " and employee_transactions.branch_code IN (SELECT DISTINCT
		                 branch_details.branch_code " & where & ")"
        End If

        Try

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt



    End Function

    Public Function GetClockingEmployees(ByVal EmployeeNumber As String) As DataTable
        tmpSQL = "SELECT employee_number,
                  first_name,
                  last_name 
                 FROM employee_details
                  WHERE employee_number " & "LIKE '" & UCase$(EmployeeNumber) & "%' ORDER BY employee_details ASC LIMIT 30"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetEmployeesBySearch(ByVal Employee As String) As DataTable

        tmpSQL = "SELECT employee_number,first_name|| ' ' || last_name AS employee_name FROM employee_details WHERE " &
                 "employee_number LIKE '" & RG.Apos(Employee) & "%' OR first_name LIKE '" &
                 RG.Apos(Employee) & "%' OR last_name LIKE '" & RG.Apos(Employee) & "%'"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetGridClockingEmployees(ByVal StartDate As String, ByVal EndDate As String, ByVal Employee As String) As DataSet

        tmpSQL = "SELECT t.employee_number,
                         employee_details.id_number,
                         MAX(employee_details.first_name) AS first_name,MAX(employee_details.last_name) AS last_name," &
                        "TO_CHAR((SUM(CASE WHEN t.is_sunday = true THEN time_worked END) || ' minute')::interval, 'HH24:MI') AS sunday," &
                        "TO_CHAR((SUM(CASE WHEN t.is_public_holiday = true THEN time_worked END) || ' minute')::interval, 'HH24:MI') AS public_holiday," &
                        "TO_CHAR((SUM(CASE WHEN t.is_sunday = false and t.is_public_holiday = false THEN time_worked END) || ' minute')::interval, 'HH24:MI') AS normal " &
                        "FROM employee_transactions t INNER JOIN branch_details ON t.branch_code = branch_details.branch_code " &
                        "INNER JOIN employee_details ON t.employee_number = employee_details.employee_number " &
                        "WHERE clocking_date_in BETWEEN '" & StartDate & "' AND '" & EndDate & "' "

        If Employee <> "" Then
            tmpSQL &= "AND t.employee_number = '" & RG.Apos(Employee) & "' "
        End If

        tmpSQL &= "GROUP BY t.employee_number, employee_details.id_number ORDER BY t.employee_number ASC"


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            Ds.Tables.Add(GetGridClockingEmployeesDetails(StartDate, EndDate, Employee))
        Catch ex As Exception
            Throw ex
        End Try

        Return Ds

    End Function

    Public Function GetGridClockingEmployeesDetails(ByVal StartDate As String, ByVal EndDate As String, ByVal Employee As String) As DataTable

        tmpSQL = "SELECT guid,t.employee_number,t.reason,t.clocking_date_in, t.clocking_hour_in,t.clocking_date_out,t.clocking_hour_out, t.branch_code, branch_details.branch_name, " &
                             "(CASE WHEN t.is_sunday = true THEN TO_CHAR((time_worked || ' minute')::interval, 'HH24:MI') END) AS sunday," &
                             "(CASE WHEN t.is_public_holiday = true THEN TO_CHAR((time_worked || ' minute')::interval, 'HH24:MI') END) AS public_holiday," &
                             "(CASE WHEN t.is_sunday = false AND t.is_public_holiday = false THEN TO_CHAR((time_worked || ' minute')::interval, 'HH24:MI') END) AS normal " &
                             "FROM employee_transactions t " &
                             "INNER JOIN branch_details ON t.branch_code = branch_details.branch_code " &
                             "WHERE clocking_date_in BETWEEN '" & StartDate & "' AND '" & EndDate & "' "

        If Employee <> "" Then
            tmpSQL &= "AND t.employee_number = '" & RG.Apos(Employee) & "' "
        End If

        tmpSQL &= "ORDER BY t.clocking_date_in"


        Try

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetBranches(Optional ByVal All As Boolean = False) As DataSet
        Dim condition As String = String.Empty
        If All = True Then
            condition = ""
        Else
            condition = "WHERE is_blocked = 'False'"
        End If

        tmpSQL = "SELECT 
                  branch_code,
                  branch_name,
                 address_line_1,
                 address_line_2,
                 address_line_3,
                 address_line_4,
                 address_line_5,
                 telephone_number,
                 fax_number,
                 email_address,
                 tax_number,
                 pricelevel,
                 is_head_office,
                 is_blocked,
                 inserted,
                 updated,
                 branch_type,
                 merchant_number,
                 no_stock_until,
                 region,
                 municipality,
                 province,
                 store_square_metres,
                 trading_hour_start,
                 trading_hour_end,
                 longitude,
                 latitude,
                 mall_type,
                 company_name,
                 url,
                 branch_name_web,
                 store_status
                  FROM branch_details 
                  " & condition & "
                  ORDER by branch_code ASC"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return Ds

    End Function

    Public Function RunEmployeeReport(ByVal EMailAddresses As String,
                                    ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                     timestamp_created,
                                     email_addresses,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('employee_report',
                                    '',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & EMailAddresses & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                    '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function



    Public Function RunCashSummaryReport(
                                 ByVal StartDate As String, ByVal EndDate As String, ByVal Json As String, ByVal Email As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('cash_summary_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & Json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function RunBadDebtReport(ByVal json As String, ByVal Email As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                     timestamp_created,
                                     email_addresses,
                                     username,
                                     additional_options) VALUES " &
                                    "('bad_debt_report',
                                    '',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                   '" & Email & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function RunCashDiscrepancyReport(
                                ByVal StartDate As String, ByVal EndDate As String, ByVal Json As String, ByVal Email As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('cash_discrepancy_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & Json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function RunCashTransactionsReport(
                                ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('cash_transaction_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function RunAccountOpenedByEmployee(
                                ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                    additional_options
                                     ) VALUES " &
                                    "('account_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "'
                                    )"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function GetHistoricalClockingReport(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Dim WhereCondition As String = String.Empty
        Dim query As String = String.Empty
        Dim isCasual As String
        Dim isPermanent As String

        Dim ColumnsList As String = " branch_details.branch_code,branch_name,"

        If (All = True) Then
            isCasual = True
            isPermanent = False
            employeeCasual = ""
        ElseIf Permanent = True Then
            isCasual = "null"
            isPermanent = False
            employeeCasual = " and employee_details.is_casual = False "
        ElseIf Casual = True Then
            isCasual = True
            isPermanent = "null"
            employeeCasual = " and employee_details.is_casual = True "
        End If


        query = "Select branch_details.branch_code,branch_details.branch_name, 
                     COUNT(employee_transactions.clocking_date_in) filter (where employee_details.is_casual = " & isCasual & ") as Count, 
                     count(employee_transactions.clocking_date_in) filter (where employee_details.is_casual = " & isPermanent & ") as Count1"
        where = "From branch_details left join employee_transactions
                     on branch_details.branch_code=employee_transactions.branch_code
                     left join employee_details on employee_transactions.employee_number=employee_details.employee_number
                     where employee_transactions.time_worked > 120 
                     and branch_details.is_blocked=false
                    and employee_transactions.clocking_date_in 
                    between '" & StartDate & "  00:00:00' and '" & EndDate & "  23:59:59'
                    group by branch_details.branch_code
                    order by branch_details.branch_code"


        Dim dt As New DataSet

        Try
            tmpSQL = query & " " & where
            dt = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            dt.Tables.Add(GetClockingDetails(StartDate, EndDate, where))
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetClockingDetails(StartDate As String, EndDate As String, query As String) As DataTable


        tmpSQL = "SELECT " &
                 "employee_transactions.employee_number, " &
                 "employee_details.first_name ||' '|| employee_details.last_name AS employee_name, " &
                 "employee_details.is_logged_in, " &
                 "employee_details.id_number, " &
                 "employee_transactions.branch_code, " &
                 "employee_transactions.clocking_date_in, " &
                 "employee_transactions.clocking_hour_in, " &
                 "employee_transactions.clocking_date_out, " &
                 "employee_transactions.clocking_hour_out, " &
                 "employee_transactions.time_worked, " &
                 "employee_transactions.reason " &
                 " FROM employee_transactions " &
                 "INNER JOIN employee_details ON employee_transactions.employee_number = employee_details.employee_number " &
                 "WHERE employee_transactions.time_worked > 120  " & employeeCasual & " and  employee_details.is_casual  notnull " &
                 "and clocking_date_in BETWEEN '" & StartDate & " 00:00:00' " &
                 "AND '" & EndDate & " 23:59:59'"

        If query <> "" Then
            tmpSQL &= " and employee_transactions.branch_code IN (SELECT DISTINCT
		                 branch_details.branch_code " & where & ")"
        End If

        Try

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt



    End Function

    Public Function GetCategories(ByVal CategoryNumber As Integer) As DataSet
        tmpSQL = "SELECT * FROM categories WHERE category_number = '" & CategoryNumber & "' ORDER BY category_code ASC"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return Ds
    End Function

    Public Function GetStockCodes(ByVal Stockcode As String) As DataTable
        tmpSQL = "SELECT generated_code, description FROM stockcodes_master WHERE generated_code LIKE '" & Stockcode.ToUpper & "%' ORDER BY generated_code ASC LIMIT 30"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function RunStockTransactionsReport(ByVal Email As String,
                               ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('stock_transaction_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function RunBestSellersReport(ByVal Email As String,
                              ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('best_sellers_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function BlockStock(ByVal BranchCode As String, ByVal NoStockUntill As String) As String


        tmpSQL = "update branch_details set no_stock_until= '" & NoStockUntill & "' where branch_code='" & BranchCode & "' "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function GetReports(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        tmpSQL = "  Select 
                    tasks_id,
                    task_type,
                    timestamp_created,
                    task_completed,
                    email_addresses,
                    username,
                    error_occured,
                    start_date,
                    end_date
                    from
                    tasks
                   where timestamp_created >= '" & FromDate & " 00:00:00' and timestamp_created <='" & ToDate & " 23:59:59'
                    order by tasks_id desc"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetReprints(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        tmpSQL = "SELECT " &
                 "TO_CHAR(reprint_timestamp,'YYYY-MM-DD HH12:MI') AS datetime," &
                 "reprints.branch_code," &
                 "bd.branch_name AS branch_name," &
                 "username," &
                 "transaction_type," &
                 "transaction_number " &
                 "FROM reprints " &
                 "LEFT OUTER JOIN branch_details bd on reprints.branch_code = bd.branch_code " &
                 "WHERE reprint_timestamp BETWEEN '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59'"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function DeletedIBTs(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        tmpSQL = "SELECT " &
                 "time_stamp," &
                 "sent_date," &
                 "username," &
                 "sending_branch_code," &
                 "receiving_branch_code," &
                 "sent_qty," &
                 "transaction_number," &
                 "generated_code," &
                 "reason_for_delete," &
                 "authorised_by " &
                 "FROM deleted_ibts 
                 WHERE time_stamp BETWEEN '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59'"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetCreditCardAuth(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        tmpSQL = "  Select 
                    time_stamp,
                    branch_code,
                    tran,
                    transaction_number,
                    amount,
                    response_code,
                    response_text,
                    reference,
                    rrn,
                    pos,
                    store,
                    card,
                    card_name,
                    card_entry,
                    sign,
                    cryptogram_type,
                    cryptogram,
                    apl,
                    tvr,
                    tsi,
                    cvm_results,
                    iad,
                    pin_statement,
                    data,
                    receipt,
                    merchant_id,
                    aid,
                    transaction_type,
                    sale_date,
                    sale_time,
                    transaction_guid
                    from
                    credit_card_auth
                   where time_stamp >= '" & FromDate & " 00:00:00' and time_stamp <='" & ToDate & " 23:59:59'
                    order by time_stamp desc"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetTillNumber(ByVal BranchCode As String) As DataTable
        tmpSQL = "SELECT till_number FROM till_numbers WHERE branch_code = '" & BranchCode & "' ORDER BY till_number ASC"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetTotal(ByVal TillNumber As String, ByVal TransactionType As String, ByVal Column As String, ByVal SummaryDate As String) As String

        tmpSQL = "SELECT sum(" & Column & ") FROM cash_transactions
                   WHERE transaction_type = '" & TransactionType & "'
                   AND till_number = '" & TillNumber & "' 
                   AND sale_date = '" & SummaryDate & "'"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                GetTotal = RG.Numb(Ds.Tables(0).Rows(0)("sum") & "")
            Else
                GetTotal = "0.00"
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return GetTotal

    End Function

    Public Function GetCashTransaction(ByVal TillNumber As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

        tmpSQL = "SELECT * FROM cash_transactions WHERE till_number = '" & TillNumber & "' " &
                             "AND transaction_type = 'POSSALE' AND " &
                             "sale_date between '" & FromDate & "' AND '" & ToDate & "'"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetCashTransactionByType(ByVal TillNumber As String, ByVal FromDate As String, ByVal ToDate As String, ByVal TransactionType As String) As DataTable

        tmpSQL = "SELECT * FROM cash_transactions WHERE till_number = '" & TillNumber & "' " &
                             "AND transaction_type = '" & TransactionType & "' AND " &
                             "sale_date between '" & FromDate & "' AND '" & ToDate & "'"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function
    Public Function GetContactLevels() As DataTable

        tmpSQL = "SELECT current_contact_level,
                         count(*) 
                         from financial_balances
                         GROUP BY current_contact_level"

        Try
            dt = usingObjDB.GetDataTable(_PCMReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function
    Public Function GetReprintDocuments(ByVal StartDate As String, ByVal EndDate As String, ByVal DispatchNumber As String) As DataTable

        Try

            If StartDate <> "" And EndDate <> "" Then
                tmpSQL = "select 
                            count(distinct transaction_number),
                            dispatch_number,
                            driver,
                            km,
                            rego,
                            dispatched_timestamp
                            from public.ibt_transactions 
                            where dispatched_timestamp >= '" & StartDate & "' and dispatched_timestamp <= '" & EndDate & "' group by dispatch_number, driver,km,rego,dispatched_timestamp"
            End If

            If DispatchNumber IsNot Nothing And DispatchNumber <> "" Then
                tmpSQL = "select 
                            count(distinct transaction_number),
                            dispatch_number,
                            driver,
                            km,
                            rego,
                            dispatched_timestamp
                            from public.ibt_transactions 
                            where dispatch_number = '" & DispatchNumber & "'  group by dispatch_number, driver,km,rego,dispatched_timestamp"
            End If



            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetDispatchDocuments(ByVal DispatchNumber As String) As DataTable
        Try
            tmpSQL = "select driver,
                             transaction_number,
                             km,
                             rego,
                             address_line_1,
                             address_line_2,
                             address_line_3,
                             address_line_4,
                             address_line_5,
                             dispatched_timestamp,
                             branch_code,
                             branch_name
                             from ibt_transactions as ibt
                             inner join branch_details bd on bd.branch_code=ibt.receiving_branch_code
                             where dispatch_number='" & DispatchNumber & "'"




            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetSellOffPerItems(ByVal StartDate As String, ByVal EndDate As String, ByVal StockCode As String) As DataTable
        Try
            tmpSQL = "SELECT " &
                "transaction_master.branch_code AS branch_code," &
                "MAX(branch_name) AS branch_name," &
                "MAX(branch_details.address_line_4) As address_line_4," &
                "MAX(branch_details.address_line_5) As address_line_5," &
                "ROUND(SUM(Case When transaction_master.transaction_type = 'POSSALE' THEN transaction_line_items.quantity END)) AS sales," &
                "ROUND(SUM(CASE WHEN transaction_master.transaction_type = 'IBTIN' THEN transaction_line_items.quantity END)) AS ibt_in " &
                "FROM transaction_master " &
                "INNER JOIN transaction_line_items ON transaction_master.guid = transaction_line_items.link_guid " &
                "INNER JOIN branch_details ON transaction_master.branch_code = branch_details.branch_code " &
                "WHERE " &
                "transaction_line_items.master_code = '" & RG.Apos(StockCode) & "' " &
                "AND transaction_master.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "'" &
                "GROUP BY " &
                "transaction_master.branch_code "

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function SearchStockcodes(ByVal SearchText As String) As DataTable
        Try
            tmpSQL = "SELECT DISTINCT ON (master_code) master_code " &
                 "FROM stockcodes_master WHERE " &
                 "master_code ILIKE '" & RG.Apos(SearchText) & "%' " &
                 "ORDER BY master_code " &
                 "LIMIT 20 "

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetCategory(ByVal CategoryNumber As String) As DataTable
        Try
            tmpSQL = "SELECT * FROM categories 
                      WHERE category_type = 'STOCK' 
                      AND category_number = '" & CategoryNumber & "'
                      ORDER BY category_code ASC "

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function RunCategorySummaryReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('category_summary',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function GetAllStockCodes(ByVal SearchText As String, ByVal MasterCode As Boolean) As SearchResponse
        Try
            If MasterCode = True Then
                tmpSQL = "SELECT DISTINCT ON (master_code) master_code,description FROM stockcodes_master WHERE master_code LIKE '" & (SearchText).ToUpper & "%' ORDER BY master_code ASC LIMIT 30"
            Else
                tmpSQL = "SELECT generated_code,description FROM stockcodes_master WHERE generated_code LIKE '" & (SearchText).ToUpper & "%' ORDER BY generated_code ASC LIMIT 30"
            End If

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            searchResponse.Success = True
            searchResponse.dt = dt
        Catch ex As Exception
            Throw ex
            searchResponse.Success = False

        End Try

        Return searchResponse
    End Function
    Public Function RunColourGridsReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As BaseResponse


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('colour_grid',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                     '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            baseresponse.Success = False
            baseresponse.Message = "Something Went wrong"
            Return baseresponse
        Finally
            objDBWrite.CloseConnection()
        End Try
        baseresponse.Success = True
        baseresponse.Message = "Your report is running! Please look for the completed report in the View Reports Page after it is done."
        Return baseresponse

    End Function
    Public Function GetALLBranches() As DataTable
        tmpSQL = "SELECT 
                  branch_code,
                  branch_name                
                  FROM branch_details 
                  WHERE is_blocked = 'False'
                  ORDER by branch_code ASC"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function
    Public Function GetGridSize() As DataTable
        Try
            tmpSQL = "SELECT grid_number,
                             grid_description 
                             FROM size_grids 
                             ORDER BY grid_number ASC"

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function RunGridSizeReport(ByVal StartDate As String, ByVal EndDate As String, ByVal EMailAddresses As String, ByVal Username As String, ByVal json As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                     timestamp_created,
                                     email_addresses,
                                     start_date,
                                     end_date,
                                     username,
                                     additional_options) VALUES " &
                                    "('size_grid',
                                    '',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & EMailAddresses & "',
                                    '" & StartDate & "',
                                    '" & EndDate & "',
                                    '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex

        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"
    End Function
    Public Function RunAgingSummaryReport(ByVal json As String, ByVal Email As String, ByVal Username As String) As String
        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                     timestamp_created,
                                     email_addresses,
                                     username,
                                     additional_options) VALUES " &
                                    "('aging_summary',
                                    '',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & Email & "',
                                    '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex

        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"
    End Function
    Public Function GetMasterBestSellers(ByVal StartDate As String, ByVal EndDate As String, ByVal MasterCode As Boolean) As DataTable

        tmpSQL = "SELECT " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN l.quantity END) AS qty, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.selling_price * l.quantity + 0.001) END) as selling," &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.estimated_cost_price * l.quantity) END) AS estimated, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.average_cost_price * l.quantity) END) AS average, "

        If MasterCode = True Then
            tmpSQL = tmpSQL & "TRIM(both ' '  from UPPER(l.master_code)) as stockcode, "
        Else
            tmpSQL = tmpSQL & "TRIM(both ' '  from UPPER(l.generated_code)) as stockcode, "
        End If

        tmpSQL = tmpSQL & "min(l.description) as description " &
                          "FROM transaction_master m " &
                          "INNER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                          "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                          "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                          "GROUP BY stockcode ORDER BY qty DESC nulls LAST"

        Dim masterData As New DataTable

        Try
            masterData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return masterData
    End Function
    Public Function GetDetailBestSellers(ByVal StartDate As String, ByVal EndDate As String, ByVal MasterCode As Boolean) As DataTable

        Dim detailData As New DataTable

        tmpSQL = "SELECT m.sale_date,b.branch_name,m.transaction_type,"

        If MasterCode = True Then
            tmpSQL = tmpSQL & "TRIM(both ' '  from UPPER(l.master_code)) as stockcode, "
        Else
            tmpSQL = tmpSQL & "TRIM(both ' '  from UPPER(l.generated_code)) as stockcode, "
        End If

        tmpSQL &= "(l.selling_price * l.quantity) * (1 + l.tax_percentage /100) as selling_price,l.quantity " &
                 "FROM transaction_master m " &
                 "LEFT OUTER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                 "INNER JOIN branch_details b ON m.branch_code = b.branch_code " &
                 "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                 "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                 "ORDER BY sale_date DESC nulls LAST"

        Try
            detailData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return detailData
    End Function
    Public Function GetTotalStockOnHand() As DataSet

        tmpSQL = "select branch_code,
                         sum(qty_on_hand)
                         from stock_on_hand 
                         where qty_on_hand <> 0 
                         GROUP BY branch_code
                         ORDER BY branch_code"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return Ds

    End Function
    Public Function GetTotalStockByBranch(ByVal BranchCode As String) As DataTable
        tmpSQL = "      Select 
                         generated_code,
                         branch_code,
                         qty_on_hand
                         from stock_on_hand
                        where branch_code='" & BranchCode & "' "

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function GetBranchStockHistory(ByVal FromDate As String) As DataTable
        tmpSQL = "  Select 
                    bsh.branch_code,
                    branch_name,
                    stock_date,
                    stock_on_hand
                    from
                    branch_stock_history bsh
                    inner join branch_details bd on bd.branch_code=bsh.branch_code
                    where stock_date = '" & FromDate & " 00:00:00'
                    order by bd.branch_code asc"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function
    Public Function GetVoidsAndNoSalesReports(ByVal StartDate As String, ByVal EndDate As String, ByVal BranchList As String) As DataTable
        tmpSQL = "SELECT v.void_date,
                         v.void_time,
                         v.user_name,
                         v.generated_code,
                         v.quantity,
                         branch_details.branch_name " &
                         "FROM voids v " &
                         "INNER JOIN branch_details ON v.branch_code = branch_details.branch_code " &
                         "WHERE void_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                         "AND v.branch_code IN(" & BranchList & ") ORDER BY void_date ASC"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function GetDiscountReports(ByVal StartDate As String, ByVal EndDate As String, ByVal BranchList As String) As DataTable
        tmpSQL = "SELECT transaction_master.sale_date,
                         transaction_master.branch_code,
                         transaction_master.sale_time," & "
                         transaction_master.transaction_number,
                         transaction_line_items.generated_code,
                         transaction_line_items.description," & "
                         transaction_line_items.selling_price,
                         transaction_line_items.discount_per_item,
                         transaction_line_items.discount_reason," & "
                         transaction_line_items.quantity 
                         FROM transaction_master " & "
                         INNER JOIN transaction_line_items ON transaction_master.guid = transaction_line_items.link_guid " & "
                         WHERE  transaction_line_items.discount_per_item <> 0 " & "
                         AND transaction_master.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND transaction_master.branch_code IN(" & BranchList & ")  "
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function GetStockcodesList(ByVal StockCode As String, ByVal MasterCode As Boolean) As DataTable
        If MasterCode = True Then
            tmpSQL = "SELECT DISTINCT ON (master_code) master_code,description FROM stockcodes_master WHERE master_code LIKE '" & (StockCode).ToUpper & "%' ORDER BY master_code ASC LIMIT 30"
        Else
            tmpSQL = "SELECT generated_code,description FROM stockcodes_master WHERE generated_code LIKE '" & (StockCode).ToUpper & "%' ORDER BY generated_code ASC LIMIT 30"

        End If

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function RunSOHReport(ByVal Email As String, ByVal Username As String, ByVal json As String) As String

        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                     timestamp_created,
                                     email_addresses,
                                     username,
                                     additional_options) VALUES " &
                                    "('stock_on_hand',
                                    '',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & Email & "',
                                    '" & Username & "',
                                    '" & json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
        Finally
            objDBWrite.CloseConnection()
        End Try
        Return "Success"
    End Function
    Public Function GetWebServiceWebLogs(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        tmpSQL = "  Select 
                    time_stamp,
                    web_service,
                    ip_address,
                    master_code,
                    ws_colour,
                    ws_size,
                    ws_response,
                    branch_code
                    from
                    web_service_web_logs
                   where time_stamp BETWEEN '" & FromDate & "  00:00:00 ' AND '" & ToDate & "  11:59:59'
                    order by time_stamp desc"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function
End Class



