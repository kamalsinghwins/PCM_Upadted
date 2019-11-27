Imports pcm.DataLayer
Imports Entities

Public Class LoggingBusinessLayer

    Private _dlLog As New LoggingDataLayer


    Public Function InsertUserLogRecord(ByVal NewLogRecord As CollectionCallLog) As Boolean

        If Not _dlLog.WriteToLog(NewLogRecord) Then
            Return False
        End If

        Return True

    End Function


End Class
