Imports pcm.DataLayer
Imports Entities

Public Class StatementBL

    Dim _DLayer As StatementDL
    Private _dlLog As New LoggingDataLayer

    Public Sub New()
        _DLayer = New StatementDL
    End Sub

    Public Function WriteToLogPCM(ByVal LogDetails As PCMUserLog) As Boolean
        Return _dlLog.WriteToLogPCM(LogDetails)
    End Function

    Public Function PrintStatement(ByVal AccountNumber As String) As String

        Return _DLayer.PrintStatement(AccountNumber)

    End Function

    Public Function GetAccountNumber(ByVal IDNumber As String, ByVal LastName As String) As String

        Return _DLayer.GetAccountNumber(IDNumber, LastName)

    End Function

End Class
