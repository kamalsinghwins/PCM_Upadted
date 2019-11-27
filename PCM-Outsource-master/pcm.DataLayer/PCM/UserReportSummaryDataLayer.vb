Imports Entities
Imports Npgsql

Public Class UserReportSummaryDataLayer
    Inherits DataAccessLayerBase

    Public Function ReturnUsersSummary(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT username," & _
                                     "COUNT(guid) AS numberofcalls," & _
                                     "TO_CHAR((SUM(length_of_action) || ' second')::interval, 'HH24:MI:SS') AS totallengthofcalls," & _
                                     "TO_CHAR((AVG(length_of_action) || ' second')::interval, 'HH24:MI:SS') AS averagelengthofcalls," & _
                                     "COUNT(CASE WHEN action_result LIKE 'PTP%' THEN 1 END) AS numberofptps," & _
                                     "SUM(CASE WHEN action_result like 'PTP%' THEN ptp_amount END) AS amountofptps " & _
                                     "FROM users_actions " & _
                                     "WHERE timestamp_of_action between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59' " & _
                                     "AND action_type <> 'User Login' " & _
                                     "GROUP BY username;"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

            'records = EntityMappers.MapDataToBusinessEntityCollection(Of StatsByIndustry)(reader)


        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnUsersSummaryTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "username AS username," &
                                     "TO_CHAR(timestamp_of_action, 'YYYY-MM-DD HH24:MI:SS') AS timestampofaction," &
                                     "account_number AS accountnumber,user_comment AS usercomment," &
                                     "action_result AS actionresult," &
                                     "TO_CHAR(ptp_date, 'YYYY-MM-DD HH24:MI:SS') AS ptpdate," &
                                     "ptp_amount AS ptpamount," &
                                     "TO_CHAR((length_of_action || ' second')::interval, 'HH24:MI:SS') AS lengthofaction," &
                                     "collections_period AS collectionsperiod " &
                                     "FROM users_actions " &
                                     "WHERE timestamp_of_action between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59' " &
                                     "AND action_type <> 'User Login' " &
                                     "ORDER BY timestamp_of_action;"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

            'records = EntityMappers.MapDataToBusinessEntityCollection(Of StatsByIndustry)(reader)


        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function EQToDataTable(ByVal parIList As System.Collections.IEnumerable) As System.Data.DataTable
        Dim ret As New System.Data.DataTable()
        Try
            Dim ppi As System.Reflection.PropertyInfo() = Nothing
            If parIList Is Nothing Then Return ret
            For Each itm In parIList
                If ppi Is Nothing Then
                    ppi = DirectCast(itm.[GetType](), System.Type).GetProperties()
                    For Each pi As System.Reflection.PropertyInfo In ppi
                        Dim colType As System.Type = pi.PropertyType
                        If (colType.IsGenericType) AndAlso
                           (colType.GetGenericTypeDefinition() Is GetType(System.Nullable(Of ))) Then colType = colType.GetGenericArguments()(0)
                        ret.Columns.Add(New System.Data.DataColumn(pi.Name, colType))

                    Next
                End If
                Dim dr As System.Data.DataRow = ret.NewRow
                For Each pi As System.Reflection.PropertyInfo In ppi
                    dr(pi.Name) = If(pi.GetValue(itm, Nothing) Is Nothing, DBNull.Value, pi.GetValue(itm, Nothing))
                Next
                ret.Rows.Add(dr)
            Next
            For Each c As System.Data.DataColumn In ret.Columns
                c.ColumnName = c.ColumnName.Replace("_", " ")
            Next
        Catch ex As Exception
            ret = New System.Data.DataTable()
        End Try
        Return ret
    End Function

    Public Function CollectionsByEmployee(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT ptp_user AS username,sum(transaction_amount) * -1 total_payments,count(transaction_amount) number_of_payments " & _
                                     "FROM financial_transactions " & _
                                     "WHERE sale_date BETWEEN '" & StartDate & "' and '" & EndDate & "' and transaction_type = 'PAY' " & _
                                     "AND (ptp_user IS NOT NULL AND ptp_user <> '') " & _
                                     "GROUP BY ptp_user "
                                     
            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function CollectionsByEmployeeTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT ptp_user AS username,account_number,sale_date,reference_number,transaction_amount * -1 as transaction_amount " & _
                                     "FROM financial_transactions " & _
                                     "WHERE sale_date BETWEEN '" & StartDate & "' and '" & EndDate & "' AND transaction_type = 'PAY' " & _
                                     "AND (ptp_user IS NOT NULL AND ptp_user <> '') "

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function AccountsOpenedByEmployee(ByVal StartDate As String, ByVal EndDate As String) As DataTable

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

        Dim tmpSQL As String
        Dim Ds As New DataSet
        Dim objDBReadPCM As dlNpgSQL
        Dim objDBWritePCM As dlNpgSQL
        objDBWritePCM = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        objDBReadPCM = New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim RG As New Utilities.clsUtil

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

        If (objDBWritePCM IsNot Nothing) Then
            objDBWritePCM.CloseConnection()
        End If

        If (objDBReadPCM IsNot Nothing) Then
            objDBReadPCM.CloseConnection()
        End If

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPositiveRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT employee_number,first_name,last_name FROM employee_details;"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

            'records = EntityMappers.MapDataToBusinessEntityCollection(Of StatsByIndustry)(reader)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try


        Dim yData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT TRIM(LOWER(employee_number)) AS employee_number," &
                                     "COUNT (dd.account_number) as total_opened," &
                                     "COUNT (CASE WHEN status = 'ACTIVE' THEN status END) as total_active," &
                                     "COUNT (CASE WHEN total_spent > 50 THEN total_spent END) AS total_spent " &
                                     "FROM debtor_dates dd " &
                                     "INNER JOIN financial_balances ON dd.account_number = financial_balances.account_number " &
                                     "INNER JOIN debtor_personal ON dd.account_number = debtor_personal.account_number " &
                                     "WHERE date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                                     "AND (employee_number IS NOT NULL AND employee_number <> '') " &
                                     "GROUP BY TRIM(LOWER(employee_number)) " &
                                     "ORDER BY employee_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(yData)

            'records = EntityMappers.MapDataToBusinessEntityCollection(Of StatsByIndustry)(reader)

        Catch exx As Exception
            Throw exx
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Dim accounts_list As New List(Of NewAccounts)

        If yData.Rows.Count > 0 Then
            For Each row In yData.Rows
                Dim acc As New NewAccounts
                acc.employee_number = row("employee_number")
                acc.total_active = Val(row("total_active"))
                acc.total_opened = Val(row("total_opened"))
                acc.total_spent = Val(row("total_spent"))

                Dim zubzub = (From x In xData
                              Where x.Field(Of String)("employee_number") = acc.employee_number
                              Select New With
                                    {
                                        .first_name = x.Field(Of String)("first_name"),
                                        .last_name = x.Field(Of String)("last_name")
                                        })
                For Each xx In zubzub
                    acc.first_name = xx.first_name
                    acc.last_name = xx.last_name
                Next

                accounts_list.Add(acc)
            Next
        End If

        Dim dt As New DataTable
        dt.Columns.Add("employee_number")
        dt.Columns.Add("total_active")
        dt.Columns.Add("total_opened")
        dt.Columns.Add("total_spent")
        dt.Columns.Add("first_name")
        dt.Columns.Add("last_name")


        For Each xx In accounts_list
            dt.Rows.Add(xx.employee_number, xx.total_active, xx.total_opened, xx.total_spent, xx.first_name, xx.last_name)
        Next

        Return dt

    End Function

    Public Sub InsertBranchesIntoPCM()

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xDataPos As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPositiveRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT branch_code,branch_name FROM branch_details ORDER BY branch_code"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xDataPos)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Dim tmpSQL As String
        Dim Ds As New DataSet
        Dim objDBReadPCM As dlNpgSQL
        Dim objDBWritePCM As dlNpgSQL
        objDBWritePCM = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        objDBReadPCM = New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim RG As New Utilities.clsUtil

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

                        End Try
                    End If
                Catch ex As Exception
                    If (objDBReadPCM IsNot Nothing) Then
                        objDBReadPCM.CloseConnection()
                    End If

                End Try
            Next
        End If

        If (objDBWritePCM IsNot Nothing) Then
            objDBWritePCM.CloseConnection()
        End If

        If (objDBReadPCM IsNot Nothing) Then
            objDBReadPCM.CloseConnection()
        End If

    End Sub

    Public Function AccountsOpenedByStore(ByVal StartDate As String, ByVal EndDate As String) As DataTable

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

        Dim tmpSQL As String
        Dim Ds As New DataSet
        Dim objDBReadPCM As dlNpgSQL
        Dim objDBWritePCM As dlNpgSQL
        objDBWritePCM = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        objDBReadPCM = New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim RG As New Utilities.clsUtil

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

        If (objDBWritePCM IsNot Nothing) Then
            Try
                objDBWritePCM.CloseConnection()
            Catch ex As Exception

            End Try

        End If

        If (objDBReadPCM IsNot Nothing) Then
            Try
                objDBReadPCM.CloseConnection()
            Catch ex As Exception

            End Try

        End If

        Dim yData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " & _
                                     "dfp.first_purchase AS branch_code," & _
                                     "MAX(branch_details.branch_name) AS branch_name," & _
                                     "COUNT(dd.account_number) AS total_opened " & _
                                     "FROM " & _
                                     "debtor_dates AS dd " & _
                                     "INNER JOIN financial_balances ON dd.account_number = financial_balances.account_number " & _
                                     "INNER JOIN debtor_personal ON dd.account_number = debtor_personal.account_number " & _
                                     "INNER JOIN debtor_first_purchase AS dfp ON dd.account_number = dfp.account_number " & _
                                     "INNER JOIN branch_details ON dfp.first_purchase = branch_details.branch_code " & _
                                     "WHERE " & _
                                     "dd.date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " & _
                                     "GROUP BY dfp.first_purchase " & _
                                     "ORDER BY dfp.first_purchase ASC"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(yData)

        Catch exx As Exception
            Throw exx
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return yData

    End Function

    Public Function AccountsOpenedByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "dp.employee_number, " &
                                     "dp.account_number, " &
                                     "dp.first_name, " &
                                     "dp.last_name, " &
                                     "dp.cell_number, " &
                                     "financial_balances.credit_limit, " &
                                     "financial_balances.total_spent, " &
                                     "debtor_first_purchase.first_purchase AS branch_code, " &
                                     "w.sent_to, " &
                                     "financial_balances.total AS total_owing,financial_balances.current_balance AS current,financial_balances.p30," &
                                     "financial_balances.p60,financial_balances.p90,financial_balances.p120,financial_balances.p150 " &
                                     "FROM " &
                                     "debtor_personal dp " &
                                     "LEFT OUTER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
                                     "LEFT OUTER JOIN financial_balances ON dp.account_number = financial_balances.account_number " &
                                     "INNER JOIN debtor_first_purchase ON dp.account_number = debtor_first_purchase.account_number " &
                                     "LEFT OUTER JOIN " &
                                     "	(SELECT DISTINCT ON (id_number) id_number,sent_to FROM sms_incoming_sms) w ON w.id_number = dp.id_number " &
                                     "WHERE date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                                     "AND debtor_first_purchase.first_purchase IN (SELECT branch_code FROM branch_details)"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function AccountsOpenedByEmployeeTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " & _
            "(CASE WHEN total_spent > 50 THEN '1' ELSE '0' END) as spent, " & _
            "debtor_personal.account_number," & _
            "debtor_personal.first_name," & _
            "debtor_personal.last_name," & _
            "debtor_personal.status," & _
            "TRIM(LOWER(debtor_personal.employee_number)) AS employee_number, " & _
            "sis.sent_to,debtor_first_purchase.first_purchase " & _
            "FROM debtor_personal " & _
            "INNER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " & _
            "INNER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " & _
            "LEFT OUTER JOIN debtor_first_purchase ON debtor_personal.account_number = debtor_first_purchase.account_number " & _
            "LEFT OUTER JOIN sms_incoming_sms sis ON sis.id_number = debtor_personal.id_number " & _
            "WHERE date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " & _
            "AND sis.time_stamp BETWEEN '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59' " & _
            "AND (employee_number IS NOT NULL " & _
            "AND employee_number <> '')"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

            'records = EntityMappers.MapDataToBusinessEntityCollection(Of StatsByIndustry)(reader)


        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

End Class

Public Class NewAccounts
    Public employee_number As String
    Public total_opened As Integer
    Public total_active As Integer
    Public total_spent As Integer
    Public first_name As String
    Public last_name As String
End Class