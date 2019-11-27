Imports Npgsql
Imports NLog
Imports Npgsql.Logging

Namespace dlLoggingNpgSQL
    Public Class NLogLoggingProvider
        Implements INpgsqlLoggingProvider

        Public Function INpgsqlLoggingProvider_CreateLogger(name As String) As NpgsqlLogger Implements INpgsqlLoggingProvider.CreateLogger
            Return New NLogLogger(name)
        End Function
    End Class

    Public Class NLogLogger
        Inherits NpgsqlLogger

        ReadOnly _log As Logger

        Public Sub New(name As String)
            _log = LogManager.GetLogger(name)
        End Sub

        Public Overrides Function IsEnabled(level As NpgsqlLogLevel) As Boolean
            Return _log.IsEnabled(ToNLogLogLevel(level))
        End Function

        Public Overrides Sub Log(level As NpgsqlLogLevel, connectorId As Integer, msg As String, Optional exception As Exception = Nothing)
            Dim ev = New LogEventInfo(ToNLogLogLevel(level), "", msg)
            If exception Is Nothing Then
                ev.Exception = exception
            End If

            If connectorId <> 0 Then
                ev.Properties("ConnectorId") = connectorId
            End If

            _log.Log(ev)
            '_log.Log(ev.Level, msg)
        End Sub

        Public Function ToNLogLogLevel(level As NpgsqlLogLevel) As LogLevel
            Select Case level
                Case NpgsqlLogLevel.Trace
                    Return LogLevel.Trace
                Case NpgsqlLogLevel.Debug
                    Return LogLevel.Debug
                Case NpgsqlLogLevel.Info
                    Return LogLevel.Info
                Case NpgsqlLogLevel.Warn
                    Return LogLevel.Warn
                Case NpgsqlLogLevel.Error
                    Return LogLevel.Error
                Case NpgsqlLogLevel.Fatal
                    Return LogLevel.Fatal
                Case Else
                    Throw New ArgumentOutOfRangeException("level")
            End Select
        End Function
    End Class
End Namespace
