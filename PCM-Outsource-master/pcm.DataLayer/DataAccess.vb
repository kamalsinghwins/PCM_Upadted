Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Npgsql
Imports System.Configuration
Imports System.Web

Public Class DataAccess
    Private _connection As NpgsqlConnection = Nothing
    Public Sub New()
    End Sub

    Public ReadOnly Property DataBase() As NpgsqlConnection
        Get

            If Me._connection Is Nothing Then
                If HttpContext.Current.IsDebuggingEnabled Then
                    Return New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPCMRead").ConnectionString)
                Else
                    Return New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPCMRead").ConnectionString)
                End If

            End If
            Return Me._connection
        End Get
    End Property

    Public ReadOnly Property DataBase(ByVal ConnectionString As String, ByVal tDatabase As String) As NpgsqlConnection
        Get

            If Me._connection Is Nothing Then
                Return New NpgsqlConnection(ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString & "database=" & tDatabase & ";")
            End If
            Return Me._connection
        End Get
    End Property
End Class
