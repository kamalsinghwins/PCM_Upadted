Imports Npgsql
Imports Entities

Public Class MaintenanceDL

    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function GetBranchDetails(ByVal CompanyCode As String, ByVal BranchCode As String,
                                     Optional ByVal GetInternalData As Boolean = True) As Branch

        Dim objDBRead As dlNpgSQL

        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim _Branch As New Branch

        tmpSQL = "SELECT * FROM branch_details WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                _Branch.branch_name = ds.Tables(0).Rows(0)("branch_name") & ""
                _Branch.telephone_number = ds.Tables(0).Rows(0)("telephone_number") & ""
                _Branch.fax_number = ds.Tables(0).Rows(0)("fax_number") & ""
                _Branch.email_address = ds.Tables(0).Rows(0)("email_address") & ""
                _Branch.address_line_1 = ds.Tables(0).Rows(0)("address_line_1") & ""
                _Branch.address_line_2 = ds.Tables(0).Rows(0)("address_line_2") & ""
                _Branch.address_line_3 = ds.Tables(0).Rows(0)("address_line_3") & ""
                _Branch.address_line_4 = ds.Tables(0).Rows(0)("address_line_4") & ""
                _Branch.address_line_5 = ds.Tables(0).Rows(0)("address_line_5") & ""
                If GetInternalData = True Then
                    _Branch.tax_number = ds.Tables(0).Rows(0)("tax_number") & ""
                    _Branch.pricelevel = ds.Tables(0).Rows(0)("pricelevel") & ""
                    _Branch.is_blocked = ds.Tables(0).Rows(0)("is_blocked") & ""
                    _Branch.branch_type = ds.Tables(0).Rows(0)("branch_type") & ""
                    _Branch.merchant_number = ds.Tables(0).Rows(0)("merchant_number") & ""
                End If

                _Branch.return_message = "success"
                Else
                    If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If

                _Branch.return_message = "Branch does not exist"

                Return _Branch
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Branch.return_message = ex.Message
            Return _Branch
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Branch

    End Function

    Public Function GetBranchDetailsByName(ByVal CompanyCode As String, ByVal BranchName As String,
                                           ByVal ServerPath As String, ByVal sIPAddress As String) As Branches

        Dim objDBRead As dlNpgSQL

        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim _Branches As New Branches
        Dim _ListOfBranches As New List(Of Branch)

        tmpSQL = "SELECT * FROM branch_details WHERE branch_name ILIKE '%" & RG.Apos(BranchName) & "%' " &
                 "AND is_blocked = False " &
                 "ORDER BY branch_name"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog("", "GetBranchDetailsByName", "", "", ds.Tables(0).Rows.Count, sIPAddress)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _Branch As New Branch
                    _Branch.branch_code = dr("branch_code") & ""
                    _Branch.branch_name = dr("branch_name") & ""
                    _Branch.telephone_number = dr("telephone_number") & ""
                    _Branch.fax_number = dr("fax_number") & ""
                    _Branch.email_address = dr("email_address") & ""
                    _Branch.address_line_1 = dr("address_line_1") & ""
                    _Branch.address_line_2 = dr("address_line_2") & ""
                    _Branch.address_line_3 = dr("address_line_3") & ""
                    _Branch.address_line_4 = dr("address_line_4") & ""
                    _Branch.address_line_5 = dr("address_line_5") & ""

                    _ListOfBranches.Add(_Branch)
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                DoWebServiceLog("", "GetBranchDetailsByName_Error", "", "", ds.Tables(0).Rows.Count, sIPAddress)
                _Branches.return_message = "Branch does Not exist"

                Return _Branches
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Branches.return_message = ex.Message
            Return _Branches
        End Try

        _Branches.ListOfBranches = _ListOfBranches

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Branches

    End Function

    Private Sub DoWebServiceLog(ByVal BranchCode As String, ByVal WebService As String,
                                ByVal FromDate As String, ByVal ToDate As String, ByVal ReturnRecords As String,
                                Optional sIPAddress As String = "")

        Dim objDBWrite As dlNpgSQL

        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        tmpSQL = "INSERT INTO web_service_logs (branch_code,web_service,from_date,to_date,records_returned,ip_address) VALUES " &
                 "('" & RG.Apos(BranchCode) & "','" & WebService & "','" & FromDate & "','" & ToDate & "'," &
                 "'" & ReturnRecords & "','" & Mid(RG.Apos(sIPAddress), 1, 50) & "')"
        objDBWrite.ExecuteQuery(tmpSQL)

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub

    Public Function GetAllBranches(ByVal CompanyCode As String,
                                   ByVal GetInternalData As Boolean,
                                   ByVal sIPAddress As String) As Branches

        Dim objDBRead As dlNpgSQL


        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim _Branches As New Branches
        Dim _ListOfBranches As New List(Of Branch)

        tmpSQL = "SELECT * FROM branch_details WHERE is_blocked = False ORDER BY branch_name"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            DoWebServiceLog("", "GetAllBranches", "", "", ds.Tables(0).Rows.Count, sIPAddress)

            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _Branch As New Branch
                    _Branch.branch_code = dr("branch_code") & ""
                    _Branch.branch_name = dr("branch_name") & ""
                    _Branch.telephone_number = dr("telephone_number") & ""
                    _Branch.fax_number = dr("fax_number") & ""
                    _Branch.email_address = dr("email_address") & ""
                    _Branch.address_line_1 = dr("address_line_1") & ""
                    _Branch.address_line_2 = dr("address_line_2") & ""
                    _Branch.address_line_3 = dr("address_line_3") & ""
                    _Branch.address_line_4 = dr("address_line_4") & ""
                    _Branch.address_line_5 = dr("address_line_5") & ""
                    _Branch.latitude = dr("latitude") & ""
                    _Branch.longitude = dr("longitude") & ""
                    _Branch.tURL = dr("url") & ""
                    If GetInternalData = True Then
                        _Branch.tax_number = dr("tax_number") & ""
                        _Branch.pricelevel = dr("pricelevel") & ""
                        _Branch.is_blocked = dr("is_blocked") & ""
                        _Branch.branch_type = dr("branch_type") & ""
                        _Branch.merchant_number = dr("merchant_number") & ""
                    End If

                    _ListOfBranches.Add(_Branch)
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If


                _Branches.return_message = "Branch does not exist"

                Return _Branches
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            DoWebServiceLog("", "GetAllBranches_Error", "", "", ds.Tables(0).Rows.Count, sIPAddress)
            _Branches.return_message = ex.Message
            Return _Branches
        End Try

        _Branches.ListOfBranches = _ListOfBranches

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Branches

    End Function

    Public Function UpdateBranchTargets(ByVal CompanyCode As String,
                                        ByVal TargetYear As String, ByVal TargetMonth As String,
                                        ByVal _Branches As DataTable) As String


        Dim objDBWrite As dlNpgSQL

        If Debugger.IsAttached Then
            objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting")
        Else
            objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        End If

        tmpSQL = "DELETE FROM branch_targets WHERE target_year = '" & TargetYear & "' AND target_month = '" & TargetMonth & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        For i As Integer = 0 To _Branches.Rows.Count - 1
            tmpSQL = "INSERT INTO branch_targets (branch_code,target,target_year,target_month,rent) VALUES " &
                     "('" & _Branches.Rows(i)("branch_code") & "','" & _Branches.Rows(i)("target") & "','" &
                     TargetYear & "','" & TargetMonth & "','0')"
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

    Public Function GetBranchTargets(ByVal CompanyCode As String,
                                     ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable

        Dim objDBRead As dlNpgSQL

        If Debugger.IsAttached Then
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting")
        Else
            objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        End If

        Dim curdata As New DataTable
        curdata.TableName = "branch"
        curdata.Columns.Add("error")
        curdata.Columns.Add("branch_code")
        curdata.Columns.Add("branch_name")
        curdata.Columns.Add("target")


        'tmpSQL = "SELECT branch_details.branch_name,ibt_transactions.receiving_branch_code " & _
        '         "FROM ibt_transactions " & _
        '         "INNER JOIN branch_details ON ibt_transactions.receiving_branch_code = branch_details.branch_code " & _
        '         "WHERE ibt_transactions.transaction_number = '" & RG.Apos(IBTOutNumber) & "'"
        tmpSQL = "SELECT " &
                 "branch_details.branch_code, " &
                 "branch_details.branch_name, " &
                 "branch_targets.target " &
                 "FROM branch_details " &
                 "INNER JOIN branch_targets ON branch_targets.branch_code = branch_details.branch_code " &
                 "WHERE target_month = '" & RG.Apos(TargetMonth) & "' AND target_year = '" & RG.Apos(TargetYear) & "' " &
                 "ORDER BY branch_details.branch_name"

        '01 - Standard (10.00)
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    curdata.Rows.Add("", dr("branch_code"), dr("branch_name"), dr("target"))
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                curdata.Rows.Add("no data")
                Return curdata

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
End Class
