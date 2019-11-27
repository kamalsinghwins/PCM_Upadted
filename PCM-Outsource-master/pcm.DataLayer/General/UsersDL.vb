Imports Npgsql
Imports Entities

Public Class UsersDL

    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    'Public Sub New(ByVal CompanyCode As String)
    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite", "pos_" & CompanyCode)
    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)
    '    connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPositiveRead").ConnectionString & "database=" & ConfigurationManager.AppSettings("CurrentDatabase"))

    'End Sub

    Public Sub New()
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPositiveRead").ConnectionString)

    End Sub

    Public Function GetUsers() As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT username FROM wusers ORDER BY username"
            '01 - Standard (10.00)

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

    Public Function GetUserDetails(ByVal Username As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT * FROM wusers WHERE username = '" & RG.Apos(Username.ToUpper) & "'"
            '01 - Standard (10.00)

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

    Public Function CheckUser(ByVal objUser As Users) As Users

        Dim _Users As New Users

        tmpSQL = "SELECT processing_sequence,maintenance_sequence,email_address,reporting_sequence,ustatus,pcm_admin " &
                 "FROM wusers WHERE username = '" & RG.Apos(objUser.Username.ToUpper) & "' " &
                 "AND userpass = '" & RG.Apos(objUser.Password.ToUpper) & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("ustatus") <> "ACTIVE" Then
                        If (objDBRead IsNot Nothing) Then
                            objDBRead.CloseConnection()
                        End If
                        Return Nothing
                    End If

                    _Users.Username = objUser.Username
                    _Users.IPAddress = objUser.IPAddress
                    _Users.processing_permission_sequence = dr("processing_sequence") & ""
                    _Users.maintenance_permission_sequence = dr("maintenance_sequence") & ""
                    _Users.reporting_permission_sequence = dr("reporting_sequence") & ""
                    _Users.isPCMAdmin = dr("pcm_admin")
                    _Users.Email = dr("email_address").ToString


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
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Return _Users


    End Function

    Public Function ProcessLogin(ByRef objUser As Users) As Boolean

        tmpSQL = "INSERT INTO user_logins (date_time_of_login,user_name,permissions,ip_address,is_administrator) VALUES (" &
                 "'" & Format(Now, "yyyy-MM-dd HH:mm") & "','" & RG.Apos(objUser.Username.ToUpper) & "'," &
                 "'" & objUser.processing_permission_sequence & " " & objUser.maintenance_permission_sequence & " " & objUser.reporting_permission_sequence & "'," &
                 "'" & RG.Apos(objUser.IPAddress) & "','False')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return False
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return True

    End Function

    Public Function UpdateEmployee(ByVal _EmployeeMaintenanceRequest As EmployeeMaintenanceRequest) As BaseResponse

        Dim _Reponse As New BaseResponse

        tmpSQL = "SELECT employee_number " &
                 "FROM employee_details " &
                 "WHERE id_number = '" & RG.Apos(_EmployeeMaintenanceRequest.IDNumber.ToUpper) & "'"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                If ds.Tables(0).Rows(0)("employee_number").ToString.ToUpper <> _EmployeeMaintenanceRequest.ClockNumber.ToUpper Then
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    If (objDBWrite IsNot Nothing) Then
                        objDBWrite.CloseConnection()
                    End If

                    _Reponse.Success = False
                    '_Reponse.Message = "This ID Number is already in use for Employee: " & ds.Tables(0).Rows("employee_number").ToString().ToUpper()
                    _Reponse.Message = "This ID Number is already in use for Employee: " & ds.Tables(0).Rows(0)("employee_number").ToString().ToUpper()

                    Return _Reponse
                End If
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            _Reponse.Success = False
            _Reponse.Message = ex.Message

            Return _Reponse
        End Try

        'Check for duplicate bank account number
        '===================================================================

        If _EmployeeMaintenanceRequest.AccountNumber <> "" Then

            tmpSQL = "SELECT bank_account_number,employee_number" &
                 " FROM employee_details " &
                 "WHERE bank_account_number = '" & RG.Apos(_EmployeeMaintenanceRequest.AccountNumber.ToUpper) & "' " &
                  "and employee_number <>'" & RG.Apos(_EmployeeMaintenanceRequest.ClockNumber.ToUpper) & "' "

            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    If (objDBWrite IsNot Nothing) Then
                        objDBWrite.CloseConnection()
                    End If

                    _Reponse.Success = False
                    _Reponse.Message = "This bank account number is already in use for Employee Number: " & ds.Tables(0).Rows(0)("employee_number").ToString().ToUpper()
                    Return _Reponse
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If

                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If

                _Reponse.Success = False
                _Reponse.Message = ex.Message

                Return _Reponse
            End Try

        End If
        '==========================================================================



        tmpSQL = "SELECT employee_number " &
                 "FROM employee_details " &
                 "WHERE employee_number = '" & RG.Apos(_EmployeeMaintenanceRequest.ClockNumber.ToUpper) & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                'Employee Exists
                tmpSQL = "UPDATE employee_details SET " &
                         "first_name = '" & RG.Apos(Mid(_EmployeeMaintenanceRequest.FirstName.ToUpper, 1, 20)) & "'," &
                         "last_name = '" & RG.Apos(Mid(_EmployeeMaintenanceRequest.LastName.ToUpper, 1, 20)) & "'," &
                         "id_number = '" & RG.Apos(Mid(_EmployeeMaintenanceRequest.IDNumber.ToUpper, 1, 25)) & "'," &
                         "is_active = '" & RG.Apos(_EmployeeMaintenanceRequest.Enabled.ToUpper) & "'," &
                         "email_address = '" & RG.Apos(_EmployeeMaintenanceRequest.EmailAddress.ToUpper) & "'," &
                         "cellphone = '" & RG.Apos(_EmployeeMaintenanceRequest.Cellphone.ToUpper) & "'," &
                        "is_casual = '" & RG.Apos(_EmployeeMaintenanceRequest.Casual.ToUpper) & "'," &
                         "date_last_updated = '" & Format(Now, "yyyy-MM-dd") & "'," &
                         "bank_account_number = '" & RG.Apos(Mid(_EmployeeMaintenanceRequest.AccountNumber.ToUpper, 1, 30)) & "'," &
                         "bank_branch_code = '" & RG.Apos(Mid(_EmployeeMaintenanceRequest.BranchCode.ToUpper, 1, 30)) & "' " &
                         "WHERE employee_number = '" & RG.Apos(_EmployeeMaintenanceRequest.ClockNumber.ToUpper) & "'"
                objDBWrite.ExecuteQuery(tmpSQL)

                _Reponse.Success = True
                _Reponse.Message = "Employee Listing has been Updated"
            Else
                tmpSQL = "INSERT INTO employee_details " &
                         "(employee_number,first_name,last_name,id_number,email_address,cellphone,date_last_updated,bank_account_number,bank_branch_code,is_casual) " &
                         "VALUES " &
                         "('" & RG.Apos(Mid(_EmployeeMaintenanceRequest.ClockNumber.ToUpper, 1, 20)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.FirstName.ToUpper, 1, 20)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.LastName.ToUpper, 1, 20)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.IDNumber.ToUpper, 1, 20)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.EmailAddress.ToUpper, 1, 20)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.Cellphone.ToUpper, 1, 20)) & "'," &
                         "'" & Format(Now, "yyyy-MM-dd") & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.AccountNumber.ToUpper, 1, 30)) & "'," &
                         "'" & RG.Apos(Mid(_EmployeeMaintenanceRequest.BranchCode.ToUpper, 1, 30)) & "'," &
                         "'" & _EmployeeMaintenanceRequest.Casual & "')"

                objDBWrite.ExecuteQuery(tmpSQL)

                _Reponse.Success = True
                _Reponse.Message = "Employee Listing has been Updated"
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            _Reponse.Success = False
            _Reponse.Message = ex.Message

            Return _Reponse
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return _Reponse

    End Function

    Public Function UpdateUserFile(ByVal Username As String, ByVal Password As String, ByVal Email As String, ByVal isActive As String, ByVal processing_sequence As String,
                                   ByVal maintenance_sequence As String, ByVal reporting_sequence As String,
                                   ByVal isPCMAdmin As Boolean) As Boolean

        'tmpSQL = "DELETE FROM wusers WHERE username = '" & RG.Apos(Username.ToUpper) & "'"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    If (objDBWrite IsNot Nothing) Then
        '        objDBWrite.CloseConnection()
        '    End If
        '    Return False
        'Finally
        '    objDBWrite.CloseConnection()
        'End Try

        Dim status As String = String.Empty

        If isActive = True Then
            status = "ACTIVE"
        Else
            status = "INACTIVE"

        End If

        'Check User exist in the database

        If CheckRecordExists(Username, Password) = False Then

            If String.IsNullOrEmpty(Password) Then
                Return False
            End If

            tmpSQL = "INSERT INTO wusers         ( 
                                                  username,
                                                  userpass,
                                                  ustatus,
                                                  company,
                                                  processing_sequence,
                                                  maintenance_sequence,
                                                  reporting_sequence,
                                                  pcm_admin,email_address
                                                  ) " &
                                             "VALUES
                                                 (
                                                 '" & RG.Apos(Username.ToUpper) & "',
                                                 '" & RG.Apos(Password.ToUpper) & "',
                                                '" & status & "',
                                                 '','" & processing_sequence & "',
                                                 '" & maintenance_sequence & "',
                                                 '" & reporting_sequence & "',
                                                 '" & isPCMAdmin & "',
                                                 '" & Email & "'
                                                 )"



        Else
            If String.IsNullOrEmpty(Password) Then
                tmpSQL = "update wusers  set   ustatus='" & status & "',
                                                company='',
                                                processing_sequence='" & processing_sequence & "',
                                                maintenance_sequence='" & maintenance_sequence & "',
                                                reporting_sequence='" & reporting_sequence & "',
                                                pcm_admin='" & isPCMAdmin & "',
                                                email_address='" & Email & "'
                                                where username='" & Username & "'"
            Else
                tmpSQL = "update wusers set userpass='" & RG.Apos(Password.ToUpper) & "',
                                                ustatus='" & status & "',
                                                company='',
                                                processing_sequence='" & processing_sequence & "',
                                                maintenance_sequence='" & maintenance_sequence & "',
                                                reporting_sequence='" & reporting_sequence & "',
                                                pcm_admin='" & isPCMAdmin & "',
                                                email_address='" & Email & "'
                                                where username='" & Username & "'"

            End If


        End If



        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return False
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return True

    End Function

    Public Function CheckRecordExists(ByVal Username As String, ByVal Password As String) As Boolean
        tmpSQL = "Select  * from wusers where username ='" & RG.Apos(Username.ToUpper) & "'"
        Try
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return False
        Finally
            objDBWrite.CloseConnection()
        End Try
    End Function


End Class
