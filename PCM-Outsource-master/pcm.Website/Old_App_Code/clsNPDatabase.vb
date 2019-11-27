'Imports System.Data
'Imports Npgsql


'Public Class clsNPDatabase
'    Dim _sqlConnection As NpgsqlConnection
'    Dim _sqlCommand As NpgsqlCommand
'    Dim _sqlDataAdapter As NpgsqlDataAdapter
'    Dim _dataset As DataSet

'    Public Sub New()
'        On Error GoTo ErrZ
'        _sqlConnection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionString").ConnectionString)

'        Exit Sub
'ErrZ:
'        'Er("clsNPDatabase New", "", Err.Number, Err.Description)
'        'primary_conninfo      = 'host=X.X.X.X port=5432 user=replication_user password=the_password'

'    End Sub

'    Public Sub New(ByVal ConnectionString As String)
'        On Error GoTo ErrZ
'        _sqlConnection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString)

'        Exit Sub
'ErrZ:
'        MsgBox(Err.Number & " " & Err.Description)
'        'primary_conninfo      = 'host=X.X.X.X port=5432 user=replication_user password=the_password'

'    End Sub

'    Public Function OpenConnection() As NpgsqlConnection

'        Try
'            If _sqlConnection.State = ConnectionState.Closed Then
'                _sqlConnection.Open()
'            End If
'        Catch ex As Exception
'            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
'            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

'            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
'            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

'            Msg3.IsBodyHtml = "False"

'            Dim localID As String = ""
'            Dim hostName = System.Net.Dns.GetHostName()
'            For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()

'                ' If you just want to write every IP
'                localID = localID & ("Name: " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf

'                ' If you want to look if the device is member of a specific network
'                'If hostAdr.ToString().StartsWith("192.168.1.") Then DoSomething() : Exit For

'                ' I think you get the idea ^^
'                ' ...
'            Next

'            Msg3.Subject = "Error Occurred in OpenConnection in PCM on " & hostName

'            Msg3.Body = Err.Description & vbCrLf & localID

'            MailObj3.UseDefaultCredentials = False
'            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
'            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
'            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

'            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
'                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
'                    MailObj3.Send(Msg3)
'                End If
'            End If


'            '                MsgBox(Err.Description)
'        End Try
'        Return _sqlConnection
'    End Function

'    Public Sub CloseConnection()

'        Try
'            If _sqlConnection.State = ConnectionState.Open Then
'                _sqlConnection.Close()
'            End If
'        Catch ex As Exception
'            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
'            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

'            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
'            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

'            Msg3.IsBodyHtml = "False"
'            Msg3.Subject = "Error Occurred in CloseConnection in PositiveLive.co.za"

'            Msg3.Body = Err.Description

'            MailObj3.UseDefaultCredentials = False
'            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
'            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
'            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

'            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
'                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
'                    MailObj3.Send(Msg3)
'                End If
'            End If

'        End Try

'    End Sub

'    Public Function GetDataSet(ByVal strQuery As String) As DataSet

'        'NpgsqlEventLog.Level = LogLevel.Normal
'        'NpgsqlEventLog.LogName = ("c:\npgsql.log")
'        'NpgsqlEventLog.EchoMessages = True

'        _dataset = New DataSet

'        Try
'            _sqlDataAdapter = New NpgsqlDataAdapter(strQuery, OpenConnection)
'            _sqlDataAdapter.Fill(_dataset)
'            CloseConnection()
'        Catch ex As Exception
'            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
'            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

'            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
'            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

'            Msg3.IsBodyHtml = "False"

'            Dim localID As String = ""
'            Dim hostName = System.Net.Dns.GetHostName()
'            For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()

'                ' If you just want to write every IP
'                localID = localID & ("Name: " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf

'                ' If you want to look if the device is member of a specific network
'                'If hostAdr.ToString().StartsWith("192.168.1.") Then DoSomething() : Exit For

'                ' I think you get the idea ^^
'                ' ...
'            Next

'            Msg3.Subject = "Error Occurred in GetDataSet in PCM on " & hostName

'            Msg3.Body = Err.Description & vbCrLf & strQuery & vbCrLf & localID

'            MailObj3.UseDefaultCredentials = False
'            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
'            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
'            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

'            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
'                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
'                    MailObj3.Send(Msg3)
'                End If
'            End If

'        End Try

'        Return _dataset

'    End Function

'    Public Function ReleaseDataSet(ByRef ds As DataSet) As Boolean
'        Try
'            ds.Clear()
'            ds.Dispose()
'        Catch ex As Exception
'            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
'            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

'            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
'            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

'            Msg3.IsBodyHtml = "False"
'            Msg3.Subject = "Error Occurred in ReleaseDataSet in PositiveLive.co.za"

'            Msg3.Body = Err.Description

'            MailObj3.UseDefaultCredentials = False
'            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
'            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
'            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

'            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
'                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
'                    MailObj3.Send(Msg3)
'                End If
'            End If

'            Return False
'        End Try
'        Return True
'    End Function

'    Public Function ExecuteQuery(ByVal strQuery As String) As Boolean

'        'NpgsqlEventLog.Level = LogLevel.Normal
'        'NpgsqlEventLog.LogName = ("c:\npgsql.log")
'        'NpgsqlEventLog.EchoMessages = True

'        Try
'            _sqlCommand = New NpgsqlCommand(strQuery, OpenConnection)
'            _sqlCommand.ExecuteNonQuery()
'            CloseConnection()
'        Catch ex As Exception
'            If Err.Description.Contains("source database ""template1"" is being accessed by other users") Then
'                '                 MsgBox("Database cannot be created." & vbCrLf & "Error : " & ex.Message)
'                'Global.System.Windows.Forms.Application.Exit()
'            End If
'            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
'            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

'            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
'            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

'            Msg3.IsBodyHtml = "False"

'            Dim localID As String = ""
'            Dim hostName = System.Net.Dns.GetHostName()
'            For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()

'                ' If you just want to write every IP
'                localID = localID & ("Name: " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf

'                ' If you want to look if the device is member of a specific network
'                'If hostAdr.ToString().StartsWith("192.168.1.") Then DoSomething() : Exit For

'                ' I think you get the idea ^^
'                ' ...
'            Next

'            Msg3.Subject = "Error Occurred in ExecuteQuery in PCM on " & hostName

'            Msg3.Body = Err.Description & vbCrLf & strQuery & vbCrLf & localID

'            MailObj3.UseDefaultCredentials = False
'            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
'            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
'            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

'            If Err.Description.ToString.Contains("Thread was being aborted") = False Then
'                If Err.Description.ToString.Contains("FATAL: 3D000: database") = False Then
'                    MailObj3.Send(Msg3)
'                End If
'            End If

'            Return False
'        End Try
'        Return True

'    End Function

'    Public Function isR(ByVal tmpDs As DataSet, Optional ByVal tablename As Integer = 0) As Boolean
'        Try
'            If tmpDs.Tables.Count > 0 Then
'                If tmpDs.Tables(0).Rows.Count > 0 Then
'                    isR = True
'                Else
'                    isR = False
'                End If
'            End If
'        Catch ex As Exception
'            'Debug.Print(Err.Description & vbCrLf & Err.Number)
'            isR = False
'        End Try
'        'Return 
'    End Function

'End Class

