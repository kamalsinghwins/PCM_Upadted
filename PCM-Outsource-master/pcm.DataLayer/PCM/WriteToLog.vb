Public Class WriteToLog
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Sub WriteLog(ByVal BranchCode As String, ByVal Comments As String, ByVal IPAddress As String)

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        tmpSQL = "INSERT INTO webservice_log (log_date_time,branch_code,web_service,comments,ip_address) " &
                 "VALUES (localtimestamp,'" & BranchCode & "','CashCardTransaction','" & Comments & "','" & IPAddress & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
    End Sub

End Class
