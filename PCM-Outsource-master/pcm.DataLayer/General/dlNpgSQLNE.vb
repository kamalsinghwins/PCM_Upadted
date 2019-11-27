Imports Npgsql
Imports Npgsql.Logging
'Imports NLog
Imports pcm.DataLayer.dlLoggingNpgSQL

Public Class dlNpgSQLNE

    'Same as dlNpgSQL but without Try...Catch blocks in the Execute / GetDataSet
    Dim _sqlConnection As NpgsqlConnection
    Dim _sqlCommand As NpgsqlCommand
    Dim _sqlDataAdapter As NpgsqlDataAdapter
    Dim _dataset As DataSet
    Private _NLogLoggingProvider As New NLogLoggingProvider

    Private Sub InitializeLogggingProvider()
        If NpgsqlLogManager.IsParameterLoggingEnabled = False Then
            NpgsqlLogManager.IsParameterLoggingEnabled = True
            NpgsqlLogManager.Provider = New NLogLoggingProvider()
        End If
    End Sub

    Public Sub New()
        On Error GoTo ErrZ
        InitializeLogggingProvider()
        _sqlConnection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreRemoteConnectionString").ConnectionString)

        Exit Sub
ErrZ:

    End Sub

    Public Sub New(ByVal WhichConnectionString As String)
        On Error GoTo ErrZ
        InitializeLogggingProvider()
        _sqlConnection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings(WhichConnectionString).ConnectionString)

        Exit Sub
ErrZ:

    End Sub

    Public Sub New(ByVal WhichConnectionString As String, ByVal WhichDB As String)
        On Error GoTo ErrZ
        InitializeLogggingProvider()
        _sqlConnection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings(WhichConnectionString).ConnectionString & "database=" & WhichDB & ";")

        Exit Sub
ErrZ:

    End Sub

    Public Function OpenConnection() As NpgsqlConnection

        Dim firstEx As Exception = Nothing
        For i As Integer = 0 To 4
            Try
                If _sqlConnection.State = ConnectionState.Closed Then
                    _sqlConnection.Open()
                End If
                firstEx = Nothing
                Exit For
            Catch ex As Exception
                If firstEx Is Nothing Then
                    firstEx = ex
                End If
                Threading.Thread.Sleep(100 * (i + 1))
            End Try
        Next

        If firstEx IsNot Nothing Then
            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PCM Error")
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

            Msg3.Subject = "dlNpgSQLNE Error Occurred in OpenConnection in PCM on" & hostName

            Msg3.Body = "Error: " & Err.Description & vbCrLf & localID & vbCrLf & "Connection String: " & _sqlConnection.ConnectionString

            MailObj3.UseDefaultCredentials = False
            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
                    MailObj3.Send(Msg3)
                End If
            End If

            If HttpContext.Current.IsDebuggingEnabled Then
                If Err.Description.ToString.Contains("Thread was being aborted") = False Then
                    If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
                        MsgBox(Err.Description)
                    End If
                End If
            End If
        End If

        Return _sqlConnection

        'Try
        '    If _sqlConnection.State = ConnectionState.Closed Then
        '        _sqlConnection.Open()
        '    End If
        'Catch ex As Exception
        '    Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        '    Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.pricenet.co.za")

        '    Msg3.From = New System.Net.Mail.MailAddress("submit@pricenet.co.za", "PCM Error")
        '    Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

        '    Msg3.IsBodyHtml = "False"

        '    Dim localID As String = ""
        '    Dim hostName = System.Net.Dns.GetHostName()
        '    For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()

        '        ' If you just want to write every IP
        '        localID = localID & ("Name: " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf

        '        ' If you want to look if the device is member of a specific network
        '        'If hostAdr.ToString().StartsWith("192.168.1.") Then DoSomething() : Exit For

        '        ' I think you get the idea ^^
        '        ' ...
        '    Next

        '    Msg3.Subject = "Error Occurred in OpenConnection in PCM on" & hostName

        '    Msg3.Body = Err.Description & vbCrLf & localID & vbCrLf & "Connection String: " & _sqlConnection.ConnectionString

        '    MailObj3.UseDefaultCredentials = False
        '    MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
        '    MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

        '    If Err.Description.ToString.Contains("Thread was being aborted") = False Then
        '        If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
        '            MailObj3.Send(Msg3)
        '        End If
        '    End If

        '    If HttpContext.Current.IsDebuggingEnabled Then
        '        If Err.Description.ToString.Contains("Thread was being aborted") = False Then
        '            If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
        '                MsgBox(Err.Description)
        '            End If
        '        End If
        '    End If
        'End Try

        'Return _sqlConnection

    End Function

    Public Sub CloseConnection()

        Try
            'If _sqlConnection.State = ConnectionState.Open Then
            _sqlConnection.Close()
            'End If
        Catch ex As Exception
            'Just let it have an error. No need to report

            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

            Msg3.IsBodyHtml = "False"
            Msg3.Subject = "Error Occurred in CloseConnection in PCM"

            Msg3.Body = "Error: " & Err.Description

            MailObj3.UseDefaultCredentials = False
            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
                    MailObj3.Send(Msg3)
                End If
            End If

            If HttpContext.Current.IsDebuggingEnabled Then
                MsgBox(Err.Description)
            End If

        End Try

    End Sub

    Public Function GetDataSet(ByVal strQuery As String) As DataSet

        'NpgsqlEventLog.Level = LogLevel.Normal
        'NpgsqlEventLog.LogName = ("c:\npgsql.log")
        'NpgsqlEventLog.EchoMessages = True
        'InitializeLogggingProvider()

        'NpgsqlLogManager.Provider = New ConsoleLoggingProvider(NpgsqlLogLevel.Debug)
        'NpgsqlLogManager.IsParameterLoggingEnabled = True

        '_NLogLoggingProvider.INpgsqlLoggingProvider_CreateLogger("Npgsql.NpgsqlConnection")

        _dataset = New DataSet

        _sqlDataAdapter = New NpgsqlDataAdapter(strQuery, OpenConnection)
        _sqlDataAdapter.Fill(_dataset)

        Return _dataset

    End Function

    Public Function ReleaseDataSet(ByRef ds As DataSet) As Boolean
        Try
            ds.Clear()
            ds.Dispose()
        Catch ex As Exception
            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

            Msg3.IsBodyHtml = "False"
            Msg3.Subject = "Error Occurred in ReleaseDataSet in PCM"

            Msg3.Body = "Error: " & Err.Description

            MailObj3.UseDefaultCredentials = False
            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
                    MailObj3.Send(Msg3)
                End If
            End If

            Return False
        End Try

        Return True

    End Function

    Public Function ExecuteQuery(ByVal strQuery As String) As String

        'NpgsqlEventLog.Level = LogLevel.Normal
        'NpgsqlEventLog.LogName = ("c:\npgsql.log")
        'NpgsqlEventLog.EchoMessages = True
        'InitializeLogggingProvider()

        'NpgsqlLogManager.Provider = New ConsoleLoggingProvider(NpgsqlLogLevel.Debug)
        'NpgsqlLogManager.IsParameterLoggingEnabled = True

        '_NLogLoggingProvider.INpgsqlLoggingProvider_CreateLogger("Npgsql.NpgsqlConnection")

        Dim RecordsReturned As String = ""

        _sqlCommand = New NpgsqlCommand(strQuery, OpenConnection)
        RecordsReturned = _sqlCommand.ExecuteNonQuery()

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

End Class
