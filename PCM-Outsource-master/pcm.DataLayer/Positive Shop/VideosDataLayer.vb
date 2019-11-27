Imports Npgsql
Imports Entities
Imports NpgsqlTypes
Public Class VideosDataLayer
    Inherits DataAccessLayerBase
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function GetVideoLocation() As String

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim video_location As String = ""

        tmpSQL = "SELECT video_location FROM video_location WHERE is_active = True"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                video_location = ds.Tables(0).Rows(0)("video_location")
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If

        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        Return video_location

    End Function

End Class
