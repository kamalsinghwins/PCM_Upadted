Imports Entities
Imports Npgsql

Public Class UserReportsDataLayer
    Inherits DataAccessLayerBase

    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

    Public Function GetUserReportData(ByVal StartDate As String, ByVal EndDate As String) As List(Of UserReport)

        Dim dDateTime As DateTime
        Dim DateTimeString As String

        Dim dPTPDateTime As DateTime
        Dim PTPDateTimeString As String

        Dim _NewUserReportList As New List(Of UserReport)

        tmpSQL = "SELECT username,
                         timestamp_of_action AS datetime,
                         action_type AS action,
                         ua.account_number AS accountnumber,
                         dp.cell_number,
                         ptp_date AS ptpdate," &
                         "user_comment AS comment,
                         action_result AS result,
                         ptp_amount AS ptpamount,
                         (length_of_action / 60) AS timespent,
                         ip_address AS ipaddress, " &
                         "collections_period AS collectionsperiod,
                         web_page,search_criteria " &
                         "FROM users_actions ua left join debtor_personal dp on dp.account_number=ua.account_number " &
                         "WHERE timestamp_of_action BETWEEN '" & StartDate & " 00:00:00' AND '" & EndDate & " 23:59:59' " &
                        "ORDER BY timestamp_of_action ASC"
        Try

            Ds = objDB.GetDataSet(tmpSQL)
            objDB.CloseConnection()
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _NewUserReport As New UserReport
                    _NewUserReport.username = dr("username") & ""

                    If dr("datetime") & "" <> "" Then
                        dDateTime = dr("datetime") & ""
                        DateTimeString = Format(dDateTime, "yyyy-MM-dd HH:mm:ss")
                        _NewUserReport.datetime = DateTimeString
                    Else
                        _NewUserReport.datetime = ""
                    End If

                    _NewUserReport.action = dr("action") & ""
                    _NewUserReport.accountnumber = dr("accountnumber") & ""
                    _NewUserReport.comment = dr("comment") & ""
                    _NewUserReport.result = dr("result") & ""
                    _NewUserReport.ptpamount = dr("ptpamount") & ""

                    If dr("ptpdate") & "" <> "" Then
                        dPTPDateTime = dr("ptpdate") & ""
                        PTPDateTimeString = Format(dPTPDateTime, "yyyy-MM-dd")
                        _NewUserReport.ptpdate = PTPDateTimeString
                    Else
                        _NewUserReport.ptpdate = ""
                    End If

                    _NewUserReport.collectionsperiod = dr("collectionsperiod") & ""

                    _NewUserReport.timespent = RG.Num(dr("timespent") & "")
                    _NewUserReport.ipaddress = dr("ipaddress") & ""

                    _NewUserReport.web_page = dr("web_page") & ""
                    _NewUserReport.search_criteria = dr("search_criteria") & ""
                    _NewUserReport.cell_number = dr("cell_number") & ""

                    _NewUserReportList.Add(_NewUserReport)
                Next
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return _NewUserReportList


    End Function

    Public Function GenerateTransactionByBranch(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPCMRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " & _
                                     "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," & _
                                     "sale_time," & _
                                     "username," & _
                                     "account_number," & _
                                     "reference_number," & _
                                     "transaction_type," & _
                                     "transaction_amount," & _
                                     "pay_type," & _
                                     "branch_code " & _
                                     "FROM financial_transactions WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "'"

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
End Class
