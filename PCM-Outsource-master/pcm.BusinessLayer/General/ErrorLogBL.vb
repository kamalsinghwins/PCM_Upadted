Imports Entities
Imports pcm.DataLayer

Public Class ErrorLogBL
    Private _dlErrorLogging As New ErrorLogDL



    Public Sub ErrorLogging(_errorLog As Exception, Optional ByVal IPAddress As String = "")

        _dlErrorLogging.ErrorLogging(_errorLog, IPAddress)

    End Sub

End Class
