Imports NLog
Imports Npgsql
Imports Npgsql.Logging
Imports pcm.DataLayer.dlLoggingNpgSQL


Public Class dlNpgSQLwithUsing
    Dim _connectionString As String
    Dim _sqlConnection As NpgsqlConnection
    Dim _sqlCommand As NpgsqlCommand
    Dim _sqlDataAdapter As NpgsqlDataAdapter
    Dim _dataset As DataSet
    Dim _datatable As DataTable

    Private _NLogLoggingProvider As New NLogLoggingProvider
    Private Sub InitializeLogggingProvider()
        If NpgsqlLogManager.IsParameterLoggingEnabled = False Then
            NpgsqlLogManager.IsParameterLoggingEnabled = True
            NpgsqlLogManager.Provider = New NLogLoggingProvider()
        End If
    End Sub

    Public Function GetConnectionStringByName(Optional ByVal ConnectionString As String = "")
        If String.IsNullOrEmpty(ConnectionString) Then
            Return ConfigurationManager.ConnectionStrings("PostgreRemoteConnectionString").ConnectionString
        Else
            Return ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString
        End If
    End Function

    Public Function GetDataSet(ByVal connectionStringName As String, ByVal strQuery As String, Optional ByVal BranchCode As String = "") As DataSet
        _dataset = New DataSet

        Try
            _connectionString = GetConnectionStringByName(connectionStringName)
            _NLogLoggingProvider.INpgsqlLoggingProvider_CreateLogger("Npgsql.NpgsqlConnection")

            Using _connection As New NpgsqlConnection(_connectionString)
                Using _sqlDataAdapter = New NpgsqlDataAdapter(strQuery, _connection)
                    _sqlDataAdapter.Fill(_dataset)
                End Using
            End Using
        Catch ex As Exception
            EmailError(ex, strQuery, BranchCode)
            Throw New System.Exception(ex.Message)
        End Try

        Return _dataset
    End Function

    Public Function GetDataTable(ByVal connectionStringName As String, ByVal strQuery As String, Optional ByVal BranchCode As String = "") As DataTable
        _datatable = New DataTable




        Try
            _connectionString = GetConnectionStringByName(connectionStringName)
            _NLogLoggingProvider.INpgsqlLoggingProvider_CreateLogger("Npgsql.NpgsqlConnection")

            Using _connection As New NpgsqlConnection(_connectionString)
                Using _sqlDataAdapter = New NpgsqlDataAdapter(strQuery, _connection)
                    _sqlDataAdapter.Fill(_datatable)
                End Using
            End Using
        Catch ex As Exception
            EmailError(ex, strQuery, BranchCode)
            Throw New System.Exception(ex.Message)
        End Try

        Return _datatable
    End Function


    Public Function ExecuteQuery(ByVal connectionStringName As String, ByVal strQuery As String, Optional ByVal BranchCode As String = "") As String

        Dim RecordsReturned As String = ""

        Try
            _connectionString = GetConnectionStringByName(connectionStringName)
            _NLogLoggingProvider.INpgsqlLoggingProvider_CreateLogger("Npgsql.NpgsqlConnection")

            Using _connection As New NpgsqlConnection(_connectionString)
                _connection.Open()
                _sqlCommand = New NpgsqlCommand(strQuery, _connection)
                _sqlCommand.ExecuteNonQuery()
                _connection.Close()
            End Using

        Catch ex As Exception

            Throw New System.Exception(ex.Message)

            Return ""
        End Try

        Return RecordsReturned

    End Function

    Public Function isR(ByVal tmpDs As DataSet, Optional ByVal tablename As Integer = 0) As Boolean
        Try
            If tmpDs.Tables.Count > 0 Then
                If tmpDs.Tables(0).Rows.Count > 0 Then
                    isR = True
                Else
                    isR = False
                End If
            End If
        Catch ex As Exception
            'Debug.Print(Err.Description & vbCrLf & Err.Number)
            isR = False
        End Try
        'Return 
    End Function

    Public Sub EmailError(ByVal ex As Exception, ByVal strQuery As String, Optional ByVal BranchCode As String = "")
        Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

        Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
        Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

        Msg3.IsBodyHtml = "False"

        Dim localID As String = ""
        Dim hostName = System.Net.Dns.GetHostName()
        For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()

            ' If you just want to write every IP
            localID = localID & ("Name: " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf

            ' If you want to look if the device is member of a specific network
            'If hostAdr.ToString().StartsWith("192.168.1.") Then DoSomething() : Exit For

            ' I think you get the idea ^^
            ' ...
        Next

        Msg3.Subject = "Error Occurred in ExecuteQuery in PCM on " & hostName

        Msg3.Body = Err.Description & vbCrLf & strQuery & vbCrLf & localID & vbCrLf &
            _sqlConnection.ConnectionString & vbCrLf & _sqlCommand.Connection.ConnectionString & vbCrLf &
            "Branch Code: " & BranchCode

        MailObj3.UseDefaultCredentials = False
        MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
        'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
        MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

        If Err.Description.ToString.Contains("Thread was being aborted") = False Then
            If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
                If Err.Description.ToString.Contains("duplicate key value") = False Then
                    If Not HttpContext.Current.IsDebuggingEnabled Then
                        MailObj3.Send(Msg3)
                    End If
                End If
            End If
        End If

        If HttpContext.Current.IsDebuggingEnabled Then
            MsgBox(Err.Description)
        End If

    End Sub

End Class
