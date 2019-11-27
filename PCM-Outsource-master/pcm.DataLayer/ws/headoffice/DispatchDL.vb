Imports Npgsql

Public Class DispatchDL
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function ReturnToWarehouse(ByVal CompanyCode As String, ByVal IBTOutNumbers As DataTable) As String

        Dim objDBRead As dlNpgSQL
        Dim objDBWrite As dlNpgSQL

        'If Debugger.IsAttached Then
        'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting")
        'objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting")
        'Else
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        'End If

        If IBTOutNumbers.Rows.Count = 0 Then
            Return "Failed. Empty datatable"
        End If

        For Each dr As DataRow In IBTOutNumbers.Rows
            tmpSQL = "UPDATE ibt_transactions SET dispatched_timestamp = NULL, " &
                     "driver = NULL,km = NULL, " &
                     "rego = NULL WHERE sending_branch_code = 'HHH' AND transaction_number = '" & dr(0) & "'"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            Finally
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

        Next

        Return "Success"

    End Function

    Public Function DispatchIBT(ByVal CompanyCode As String, ByVal Driver As String,
                                ByVal KM As String, ByVal Rego As String, ByVal IBTOutNumbers As DataTable, ByVal DispatchNumber As String) As String

        Dim objDBWrite As dlNpgSQL

        If Debugger.IsAttached Then
            objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting")
        Else
            objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        End If

        If IBTOutNumbers.Rows.Count = 0 Then
            Return "Failed. Empty datatable"
        End If

        For Each dr As DataRow In IBTOutNumbers.Rows
            tmpSQL = "UPDATE ibt_transactions SET dispatched_timestamp = now(), " &
                     "driver = '" & RG.Apos(Driver) & "',dispatch_number='" & DispatchNumber & "',km = '" & RG.Apos(KM) & "', " &
                     "rego = '" & RG.Apos(Rego) & "' WHERE sending_branch_code = 'HHH' AND transaction_number = '" & dr(2) & "'"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            End Try

        Next

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "Success"

    End Function

    Public Function ReturnBranchDetails(ByVal CompanyCode As String, ByVal IBTOutNumber As String,
                                        ByVal isDispatch As Boolean) As DataTable

        Dim objDBRead As dlNpgSQL
        If Debugger.IsAttached Then
            'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting")
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Else
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        End If

        'Dim xData As String = ""

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPositiveRead").ConnectionString)

        Dim curdata As New DataTable
        curdata.TableName = "dispatch"
        curdata.Columns.Add("error")
        curdata.Columns.Add("branch_name")
        curdata.Columns.Add("branch_code")
        curdata.Columns.Add("address1")
        curdata.Columns.Add("address2")
        curdata.Columns.Add("address3")
        curdata.Columns.Add("address4")
        curdata.Columns.Add("address5")

        'tmpSQL = "SELECT branch_details.branch_name,ibt_transactions.receiving_branch_code " & _
        '         "FROM ibt_transactions " & _
        '         "INNER JOIN branch_details ON ibt_transactions.receiving_branch_code = branch_details.branch_code " & _
        '         "WHERE ibt_transactions.transaction_number = '" & RG.Apos(IBTOutNumber) & "'"

        tmpSQL = "SELECT branch_details.branch_name,ibt_transactions.receiving_branch_code,ibt_transactions.receiving_date," &
                 "ibt_transactions.dispatched_timestamp,ibt_transactions.transaction_number, " &
                 "address_line_1,address_line_2,address_line_3,address_line_4,address_line_5,no_stock_until " &
                 "FROM ibt_transactions " &
                 "INNER JOIN branch_details ON ibt_transactions.receiving_branch_code = branch_details.branch_code " &
                 "WHERE ibt_transactions.transaction_number = '" & RG.Apos(IBTOutNumber) & "' AND sending_branch_code = 'HHH' " &
                 "LIMIT 1"
        '01 - Standard (10.00)
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If isDispatch = True Then
                        If dr("no_stock_until") & "" <> "2010-01-01" Then
                            'no_stock_until <= '" & DateTime.Now.Date.ToString("yyyy-MM-dd") & "' 
                            Dim edate = dr("no_stock_until")
                            Dim expenddt As Date = Format(edate, "yyyy-MM-dd")

                            If expenddt > DateTime.Now.Date.ToString("yyyy-MM-dd") Then
                                curdata.Rows.Add(dr("receiving_branch_code") & " has been blocked from receiving stock.")
                                Return curdata
                            End If
                        End If

                        If dr("dispatched_timestamp") & "" <> "" Then
                            If (objDBRead IsNot Nothing) Then
                                objDBRead.CloseConnection()
                            End If
                            curdata.Rows.Add("IBT Out " & dr("receiving_date") & " has already been dispatched.")
                            Return curdata
                            'Return "IBT Out " & dr("transaction_number") & " has already been dispatched."
                        End If

                        If dr("receiving_date") & "" <> "" Then
                            If (objDBRead IsNot Nothing) Then
                                objDBRead.CloseConnection()
                            End If
                            curdata.Rows.Add("IBT Out " & dr("transaction_number") & " has already been dispatched.")
                            Return curdata
                            'Return "IBT Out " & dr("transaction_number") & " has already been dispatched."
                        End If

                    Else
                        If dr("dispatched_timestamp") & "" = "" Then
                            If (objDBRead IsNot Nothing) Then
                                objDBRead.CloseConnection()
                            End If
                            curdata.Rows.Add("IBT Out " & dr("receiving_date") & " has not been dispatched.")
                            Return curdata
                            'Return "IBT Out " & dr("transaction_number") & " has already been dispatched."
                        End If

                        If dr("receiving_date") & "" <> "" Then
                            If (objDBRead IsNot Nothing) Then
                                objDBRead.CloseConnection()
                            End If
                            curdata.Rows.Add("IBT Out " & dr("transaction_number") & " has not been dispatched.")
                            Return curdata
                            'Return "IBT Out " & dr("transaction_number") & " has already been dispatched."
                        End If

                    End If



                    curdata.Rows.Add("", dr("branch_name"), dr("receiving_branch_code"), dr("address_line_1"), dr("address_line_2"),
                                     dr("address_line_3"), dr("address_line_4"), dr("address_line_5"))
                    'xData = dr("branch_name") & "|" & dr("receiving_branch_code")
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                curdata.Rows.Add("IBT Out Number: " & RG.Apos(IBTOutNumber) & " does not exist.")
                Return curdata
                'Return "IBT Out Number: " & RG.Apos(IBTOutNumber) & " does not exist."
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            'Return ex.Message
            curdata.Rows.Add(ex.Message)
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        'Return xData

        Return curdata

    End Function
    Public Function GetDispatchNumber() As String

        Dim objDBWrite As dlNpgSQL
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim dispatchNumber As String = String.Empty

        tmpSQL = "SELECT nextval('hhh_" & "dispatch_number_seq')"
        Try
            Try
                ds = objDBWrite.GetDataSet(tmpSQL)
                If objDBWrite.isR(ds) Then
                    dispatchNumber = ds.Tables(0).Rows(0)("nextval")
                End If
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            Finally
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

            Return dispatchNumber
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
