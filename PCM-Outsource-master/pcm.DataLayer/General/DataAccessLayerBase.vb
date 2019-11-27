Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Npgsql
Imports System.Configuration
Imports Npgsql.Logging
'Imports NLog
Imports pcm.DataLayer.dlLoggingNpgSQL


Public Class DataAccessLayerBase
    Private _connection As NpgsqlConnection = Nothing

    Public ReadOnly _PCMReadConnectionString As String = "PostgreConnectionStringPCMRead"
    Public ReadOnly _POSReadConnectionString As String = "PostgreConnectionStringPositiveRead"
    Public ReadOnly _PCMWriteConnectionString As String = "PostgreConnectionStringPCMWrite"
    Public ReadOnly _POSWriteConnectionString As String = "PostgreConnectionStringPositiveWrite"

    'New Using Class Object
    Public usingObjDB As dlNpgSQLwithUsing
    Public RG As Utilities.clsUtil

    'Common Usable Variables
    Public tmpSQL As String
    Public Ds As New DataSet
    Public dt As New DataTable

    Private _NLogLoggingProvider As New NLogLoggingProvider

    Private Sub InitializeLogggingProvider()
        If NpgsqlLogManager.IsParameterLoggingEnabled = False Then
            NpgsqlLogManager.IsParameterLoggingEnabled = True
            NpgsqlLogManager.Provider = New NLogLoggingProvider()
        End If
    End Sub

    Public Sub New()
        InitializeLogggingProvider()

        usingObjDB = New dlNpgSQLwithUsing
        RG = New Utilities.clsUtil
    End Sub

    Public ReadOnly Property DataBase(ByVal ConnectionString As String) As NpgsqlConnection

        Get

            If Me._connection Is Nothing Then
                InitializeLogggingProvider()

                Return New NpgsqlConnection(ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString)
            End If
            Return Me._connection
        End Get
    End Property

    'Public ReadOnly Property DataBase(ByVal ConnectionString As String) As NpgsqlConnection
    '    Get

    '        If Me._connection Is Nothing Then
    '            'InitializeLogggingProvider()

    '            Return New NpgsqlConnection(ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString)
    '        End If
    '        Return Me._connection
    '    End Get
    'End Property
End Class
