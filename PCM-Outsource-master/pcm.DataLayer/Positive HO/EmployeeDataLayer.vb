Imports Entities
Public Class EmployeeDataLayer
    Inherits DataAccessLayerBase

    Private _baseResponse As New BaseResponse
    Dim DebtorDataLayer As New DebtorsDataLayer


    Public Function GetEmployees(Field As String, Criteria As String) As DataTable
        Dim GetEmployeesResponse As DataTable

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        If Field = "" Then
            Field = "Employee Number"
            '    _baseResponse.Message = "Please specify a Search Field."
            '    _baseResponse.Success = False
            '    Return Nothing
        End If

        If Criteria = "" Then
            Criteria = ""
            '    _baseResponse.Message = "Please enter a search criteria."
            '    _baseResponse.Success = False
            '    Return Nothing
        End If

        If Field = "Employee Number" Then
            tmpSQL = "Select employee_number,first_name,last_name FROM employee_details WHERE employee_number Like '" & Criteria & "%' "


        ElseIf Field = "First Name" Then
            tmpSQL = "Select employee_number,first_name,last_name FROM employee_details WHERE first_name Like '" & Criteria & "%' "


        ElseIf Field = "Last Name" Then

            tmpSQL = "Select employee_number,first_name,last_name FROM employee_details WHERE last_name Like '" & Criteria & "%' "

        End If

        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                GetEmployeesResponse = Ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        Return GetEmployeesResponse

    End Function

    Public Function GetEmployee(ByVal ClockNumber As String) As DataTable
        Dim GetEmployeesResponse As DataTable

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        tmpSQL = "Select first_name,last_name FROM employee_details WHERE employee_number = '" & ClockNumber & "' "

        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                GetEmployeesResponse = Ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        Return GetEmployeesResponse

    End Function

    Public Function SaveNotes(employeeNoteRequest As EmployeeNoteRequest) As BaseResponse


        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        Try
            If employeeNoteRequest.TypeOfReport = "Warning" Then
                tmpSQL = "INSERT INTO employee_reviews (employee_number,type_of_comment,comment,user_name,type_of_warning,branch_code,warning_expiry_date) VALUES " &
                         "('" & RG.Apos(employeeNoteRequest.EmployeeNumber.ToString.ToUpper) & "','" & RG.Apos(employeeNoteRequest.TypeOfReport) & "'," &
                         "'" & RG.Apos(employeeNoteRequest.Note) & "','" & RG.Apos(employeeNoteRequest.EmployeeName) & "','" & RG.Apos(employeeNoteRequest.Warning) & "'," &
                         "'" & employeeNoteRequest.BranchCode & "','" & employeeNoteRequest.WarningExpiryDate & "')"
            Else
                tmpSQL = "INSERT INTO employee_reviews (employee_number,type_of_comment,rating,comment,user_name,branch_code) VALUES " &
                         "('" & RG.Apos(employeeNoteRequest.EmployeeNumber.ToString.ToUpper) & "','" & RG.Apos(employeeNoteRequest.TypeOfReport) & "'," &
                         "'" & RG.Apos(employeeNoteRequest.Rating) & "','" & RG.Apos(employeeNoteRequest.Note) & "','" & RG.Apos(employeeNoteRequest.EmployeeName) & "'," &
                         "'" & employeeNoteRequest.BranchCode & "')"
            End If

            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            _baseResponse.Success = False
            _baseResponse.Message = "Something Went Wrong"
            Return _baseResponse
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        _baseResponse.Success = True
        Return _baseResponse
    End Function

    Public Function ReturnReviewsSummary(ByVal StartDate As String, ByVal EndDate As String, ByVal ClockNumber As String) As DataSet
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim WhereCondition As String = ""
        Try
            If StartDate <> "" And EndDate <> "" Then
                If WhereCondition = "" Then
                    WhereCondition &= " WHERE er.time_stamp between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59'"
                Else
                    WhereCondition &= " AND er.time_stamp between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59'"
                End If
            End If

            If ClockNumber IsNot Nothing And ClockNumber <> "" Then
                If WhereCondition = "" Then
                    WhereCondition &= " WHERE ed.employee_number = '" & ClockNumber & "'"
                Else
                    WhereCondition &= " AND ed.employee_number = '" & ClockNumber & "'"
                End If
            End If

            tmpSQL = "select er.employee_number,
                      er.type_of_comment,
                      er.rating,
                      er.comment,
                      er.time_stamp,
                      er.type_of_warning,
                      TO_CHAR(er.warning_expiry_date, 'YYYY-MM-DD' ) AS warning_expiry_date,
                      concat(ed.first_name,' ', ed.last_name) as name,
                      er.branch_code,bd.branch_name,
                      ed.id_number
                      from public.employee_reviews as er
                      left outer join branch_details bd on bd.branch_code = er.branch_code
                      join public.employee_details as ed on ed.employee_number=er.employee_number" & WhereCondition & " ORDER BY er.time_stamp DESC"

            'Ds = objDB.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try
    End Function

    Public Function GetEmployeeReviews(ByVal EmployeeNumber As String) As DataSet


        tmpSQL = "select er.employee_number,
                      er.type_of_comment,
                      er.rating,
                      er.comment,
                      er.time_stamp,
                      er.type_of_warning,
                      TO_CHAR(er.warning_expiry_date, 'YYYY-MM-DD' ) AS warning_expiry_date,
                      concat(ed.first_name,' ', ed.last_name) as name,
                      er.branch_code,
                      bd.branch_name
                      from public.employee_reviews as er
                      left outer join branch_details bd on bd.branch_code = er.branch_code
                      join public.employee_details as ed on ed.employee_number=er.employee_number where ed.employee_number='" & EmployeeNumber & "' ORDER BY er.time_stamp DESC"
        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetEmployees(Optional ByVal EmployeeNumber As String = "") As DataTable
        If EmployeeNumber <> "" Then
            tmpSQL = "select employee_number,
                            id_number,
                            first_name, 
                            last_name,
                            email_address,
                            cellphone, 
                            bank_account_number,
                            bank_branch_code
                            from employee_details where employee_number='" & EmployeeNumber & "'"
        Else
            tmpSQL = "select employee_number,
                            id_number,
                            first_name, 
                            last_name,
                            email_address,
                            cellphone, 
                            bank_account_number,
                            bank_branch_code
                            from employee_details "
        End If

        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function



End Class
