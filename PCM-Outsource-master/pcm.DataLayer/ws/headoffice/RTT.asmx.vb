Imports Npgsql

Public Class RTTDL
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function ReturnBranchDetails(ByVal CompanyCode As String, ByVal IBTOutNumber As String) As DataTable

        Dim objDBRead As dlNpgSQL
        Dim objDBWrite As dlNpgSQL

        If Debugger.IsAttached Then
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting")
        Else
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        End If

        Dim curdata As New DataTable
        curdata.TableName = "dispatch"
        curdata.Columns.Add("error")
        curdata.Columns.Add("branch_code")
        curdata.Columns.Add("branch_name")
        curdata.Columns.Add("address1")
        curdata.Columns.Add("address2")
        curdata.Columns.Add("address3")
        curdata.Columns.Add("address4")
        curdata.Columns.Add("address5")

        tmpSQL = "SELECT branch_details.branch_name,ibt_transactions.receiving_branch_code," & _
                 "address_line_1,address_line_2,address_line_3,address_line_4,address_line_5 " & _
                 "FROM ibt_transactions " & _
                 "INNER JOIN branch_details ON ibt_transactions.receiving_branch_code = branch_details.branch_code " & _
                 "WHERE ibt_transactions.transaction_number = '" & RG.Apos(IBTOutNumber) & "' AND sending_branch_code = 'HHH' LIMIT 1"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'If dr("dispatched_timestamp") & "" <> "" Then
                    '    If (objDBRead IsNot Nothing) Then
                    '        objDBRead.CloseConnection()
                    '    End If
                    '    curdata.Rows.Add("IBT Out " & dr("transaction_number") & " has already been dispatched.")
                    '    Return curdata
                    '    'Return "IBT Out " & dr("transaction_number") & " has already been dispatched."
                    'End If
                    curdata.Rows.Add("", dr("branch_name"), dr("receiving_branch_code"), dr("address_line_1"), dr("address_line_2"),
                                     dr("address_line_3"), dr("address_line_4"), dr("address_line_5"))
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

        Return curdata

    End Function


End Class