Imports Npgsql

Public Class ReportsPositiveWebServiceLogsDL
    Inherits DataAccessLayerBase

    Private ReadOnly RG As New Utilities.clsUtil()

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    'Public Sub New(ByVal CompanyCode As String)
    '    connection = Me.DataBase("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)

    'End Sub

    Public Sub New()
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")

    End Sub


    Public Function GetWebServiceLogs(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " & _
                                     "TO_CHAR(log_date_time, 'YYYY-MM-DD HH24:MI:SS') AS log_date_time," & _
                                     "branch_code,web_service,comments,ip_address " & _
                                     "FROM webservice_log WHERE log_date_time BETWEEN '" & StartDate & " 00:00:01' AND '" & EndDate & " 23:59:59'"
            'Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
End Class
