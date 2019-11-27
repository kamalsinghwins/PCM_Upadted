Imports Npgsql

Public Class ReportsPositiveCashCardsDL
    Inherits DataAccessLayerBase

    Private ReadOnly RG As New Utilities.clsUtil()

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    Public Sub New(ByVal CompanyCode As String)
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")

    End Sub

    Public Sub New()
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")

    End Sub

    Public Function GetCustomerAccounts(ByVal SearchString As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " & _
                                     "account_number,first_name|| ' ' || last_name AS account_name " & _
                                     "FROM customer_personal " & _
                                     "WHERE last_name LIKE '" & RG.Apos(SearchString) & "%' ORDER BY account_name ASC"

            'Dim reader As New NpgsqlDataAdapter(strQuery, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData


    End Function

    Public Function ReturnTransactionListing(ByVal StartDate As String, ByVal EndDate As String, _
                                             Optional ByVal AccountNumber As String = "") As DataTable


        Dim xData As New DataTable

        Dim strQuery As String

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            If AccountNumber = "" Then
                strQuery = "SELECT " &
                           "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," &
                           "TO_CHAR(sale_time, 'HH24:MI:SS') AS sale_time," &
                           "branch_code,transaction_customer.account_number,guid,transaction_type,transaction_number,transaction_total,card_number,transaction_points " &
                           "FROM transaction_customer " &
                           "Left Join cash_card_details ON transaction_customer.account_number = cash_card_details.account_number " &
                           "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' ORDER BY sale_date ASC"
            Else
                strQuery = "SELECT " &
                           "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," &
                           "TO_CHAR(sale_time, 'HH24:MI:SS') AS sale_time," &
                           "branch_code,transaction_customer.account_number,transaction_type,guid,transaction_number,transaction_total,card_number,transaction_points " &
                           "FROM transaction_customer " &
                           "Left Join cash_card_details ON transaction_customer.account_number = cash_card_details.account_number " &
                           "WHERE transaction_customer.account_number = '" & RG.Apos(AccountNumber) & "' ORDER BY sale_date ASC"
            End If

            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function

    Public Function ReturnCashCardSummary(ByVal AccountOpenedFrom As String, ByVal AccountOpenedTo As String, ByVal TransactionFrom As String, ByVal TransactionFromTo As String) As DataTable

        Dim xData As New DataTable
        Dim AccountDate As String = String.Empty
        Dim TransactionDate As String = String.Empty
        Dim strQuery As String
        Dim orderby As String = String.Empty

        Try
            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            If AccountOpenedFrom <> "" And AccountOpenedTo <> "" Then
                AccountDate &= "between" & " " & "' " & AccountOpenedFrom & "'" & " " & "And" & " " & "'" & AccountOpenedTo & "'"
            End If


            If AccountOpenedFrom <> "" And AccountOpenedTo = "" Then
                AccountDate &= ">=" & " " & "'" & AccountOpenedFrom & "'"
            End If

            If AccountOpenedFrom = "" And AccountOpenedTo <> "" Then
                AccountDate &= "<= " & " " & "'" & AccountOpenedTo & "'"
            End If


            If TransactionFrom <> "" And TransactionFromTo <> "" Then
                TransactionDate &= "between" & " " & "'" & TransactionFrom & "'" & " " & "And" & " " & "'" & TransactionFromTo & "'"
            End If


            If TransactionFrom <> "" And TransactionFromTo = "" Then
                TransactionDate &= ">=" & " " & "'" & TransactionFrom & "'"
            End If

            If TransactionFrom = "" And TransactionFromTo <> "" Then
                TransactionDate &= "<=" & " " & "'" & TransactionFromTo & "'"
            End If


            strQuery = "SELECT " &
                       "customer_personal.account_number, " &
                       "customer_personal.first_name, " &
                       "customer_personal.last_name, " &
                       "TO_CHAR(customer_dates.date_account_opened, 'YYYY-MM-DD') AS date_account_opened," &
                       "TO_CHAR(customer_dates.date_last_transaction, 'YYYY-MM-DD') AS date_last_transaction," &
                       "customer_balances.total_spent, " &
                       "customer_balances.current_points_balance, " &
                       "customer_balances.total_points_accrued, " &
                       "w.card_number, " &
                       "customer_contact_details.email_address, " &
                       "customer_contact_details.contact_number " &
                       "FROM customer_personal " &
                       "INNER JOIN customer_dates ON customer_dates.account_number = customer_personal.account_number " &
                       "INNER JOIN customer_balances ON customer_balances.account_number = customer_personal.account_number " &
                       "LEFT OUTER JOIN " &
                       "(SELECT DISTINCT on (account_number) account_number,card_number FROM cash_card_details) w on w.account_number = customer_personal.account_number " &
                       "INNER JOIN customer_contact_details ON customer_contact_details.account_number = customer_personal.account_number " &
                       "WHERE " &
                       "customer_dates.date_account_opened  " & AccountDate & " AND customer_dates.date_last_transaction  " & TransactionDate & " " &
                       "ORDER BY customer_personal.account_number ASC"


            'orderby &= strQuery & " ORDER BY customer_personal.account_number ASC"


            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function

    Public Function ReturnCashCard(ByVal DateFrom As String, ByVal DateTo As String,
                                   ByVal AllAccounts As Boolean) As DataTable

        Dim xData As New DataTable

        Dim strQuery As String

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            strQuery = "SELECT " &
                       "customer_personal.account_number, " &
                       "customer_personal.account_name, " &
                       "customer_personal.first_name, " &
                       "customer_personal.last_name, " &
                       "customer_personal.branch_opened, " &
                       "customer_dates.date_account_opened, " &
                       "customer_dates.date_last_purchase, " &
                       "customer_dates.date_last_redeemed, " &
                       "customer_contact_details.email_address, " &
                       "customer_contact_details.contact_number, " &
                       "customer_contact_details.opted_out, " &
                       "customer_contact_details.number_failed, " &
                       "customer_balances.total_spent, " &
                       "customer_balances.current_balance, " &
                       "customer_balances.current_points_balance, " &
                       "customer_balances.total_points_accrued " &
                       "FROM " &
                       "customer_personal " &
                       "INNER Join customer_dates ON customer_dates.account_number = customer_personal.account_number And customer_personal.account_number = customer_dates.account_number " &
                       "INNER Join customer_contact_details ON customer_contact_details.account_number = customer_personal.account_number " &
                       "INNER Join customer_balances ON customer_balances.account_number = customer_personal.account_number "

            If AllAccounts = False Then
                strQuery &= "AND date_account_opened BETWEEN '" & DateFrom & "' AND '" & DateTo & "' "
            End If

            strQuery &= "ORDER BY customer_personal.account_number ASC"

            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function

    Public Function ReturnCashCardSummaryLineItems(ByVal AccountOpenedFrom As String, ByVal AccountOpenedTo As String, ByVal TransactionFrom As String, ByVal TransactionFromTo As String) As DataTable

        Dim xData As New DataTable

        Dim strQuery As String
        Dim AccountDate As String = String.Empty
        Dim TransactionDate As String = String.Empty

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            If AccountOpenedFrom <> "" And AccountOpenedTo <> "" Then
                AccountDate &= "between" & " " & "' " & AccountOpenedFrom & "'" & " " & "And" & " " & "'" & AccountOpenedTo & "'"
            End If


            If AccountOpenedFrom <> "" And AccountOpenedTo = "" Then
                AccountDate &= ">=" & " " & "'" & AccountOpenedFrom & "'"
            End If

            If AccountOpenedFrom = "" And AccountOpenedTo <> "" Then
                AccountDate &= "<= " & " " & "'" & AccountOpenedTo & "'"
            End If


            If TransactionFrom <> "" And TransactionFromTo <> "" Then
                TransactionDate &= "between" & " " & "'" & TransactionFrom & "'" & " " & "And" & " " & "'" & TransactionFromTo & "'"
            End If


            If TransactionFrom <> "" And TransactionFromTo = "" Then
                TransactionDate &= ">=" & " " & "'" & TransactionFrom & "'"
            End If

            If TransactionFrom = "" And TransactionFromTo <> "" Then
                TransactionDate &= "<=" & " " & "'" & TransactionFromTo & "'"
            End If

            strQuery = "SELECT " &
                       "customer_dates.account_number, " &
                       "TO_CHAR(transaction_customer.sale_date, 'YYYY-MM-DD') AS sale_date," &
                       "TO_CHAR(transaction_customer.sale_time, 'HH24:MI:SS') AS sale_time," &
                       "transaction_customer.branch_code, " &
                       "transaction_customer.transaction_type, " &
                       "transaction_customer.transaction_number, " &
                       "transaction_customer.transaction_total, " &
                       "transaction_customer.transaction_points " &
                       "FROM " &
                       "customer_dates " &
                       "INNER JOIN transaction_customer ON transaction_customer.account_number = customer_dates.account_number " &
                       "INNER JOIN customer_personal ON customer_dates.account_number = customer_personal.account_number " &
                       "LEFT OUTER JOIN ( " &
                       "SELECT DISTINCT " &
                       "ON (account_number) account_number, " &
                       "card_number " &
                       "FROM cash_card_details " &
                       ") AS w ON w.account_number= customer_personal.account_number  " &
                       "WHERE  " &
                       "customer_dates.date_account_opened  " & AccountDate & " AND customer_dates.date_last_transaction  " & TransactionDate & " " &
                       "ORDER BY sale_date ASC"

            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function

    Public Function ReturnCustomerDetails(ByVal AccountNumber As String) As DataTable


        Dim xData As New DataTable

        xData.Columns.Add("customer_name")
        xData.Columns.Add("contact_number")
        xData.Columns.Add("opened_at")
        xData.Columns.Add("email_address")
        xData.Columns.Add("date_opened")

        Dim strQuery As String

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            If AccountNumber = "" Then
                '    strQuery = "Select " & _
                '               "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," & _
            '               "TO_CHAR(sale_time, 'HH24:MI:SS') AS sale_time," & _
            '               "branch_code,account_number,transaction_type,transaction_number,transaction_total,transaction_points " & _
            '               "FROM transaction_customer " & _
            '               "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' ORDER BY sale_date ASC"
            'Else
            '    strQuery = "SELECT " & _
            '               "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," & _
            '               "TO_CHAR(sale_time, 'HH24:MI:SS') AS sale_time," & _
            '               "branch_code,account_number,transaction_type,transaction_number,transaction_total,transaction_points " & _
            '               "FROM transaction_customer " & _
            '               "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND account_number = '" & RG.Apos(AccountNumber) & "' ORDER BY sale_date ASC"
            End If

            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function

End Class
