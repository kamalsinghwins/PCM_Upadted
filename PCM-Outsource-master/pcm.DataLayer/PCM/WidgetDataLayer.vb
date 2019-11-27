Imports Entities

Public Class WidgetDataLayer

    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil


    Public Function CallsForUser(ByVal tmpUser As String) As CallWidgets

        On Error GoTo FErr

        Dim tmpCallWidgets As New CallWidgets

        tmpSQL = "SELECT count(guid) AS calls, " & _
                 "COUNT(CASE WHEN action_result LIKE 'PTP%' THEN 1 END) AS numberofptps," & _
                 "SUM(CASE WHEN action_result like 'PTP%' THEN ptp_amount END) AS amountofptps " & _
                 "FROM users_actions " & _
                 "WHERE timestamp_of_action > '" & Format(Now, "yyyy-MM-dd") & " 00:00:00' " & _
                 "AND username = '" & RG.Apos(tmpUser.ToUpper) & "' AND action_type = 'Call'"
        ds = objDB.GetDataSet(tmpSQL)
        objDB.CloseConnection()
        If objDB.isR(ds) Then
            For Each dr As DataRow In ds.Tables(0).Rows
                tmpCallWidgets.CallsForToday = Val(dr("calls") & "")
                tmpCallWidgets.PTPsForToday = Val(dr("numberofptps") & "")
                tmpCallWidgets.AmountOfPTPs = RG.Num(Val(dr("amountofptps") & ""))
            Next
        Else
            Return Nothing
        End If

        Return tmpCallWidgets

FErr:
        Return Nothing

    End Function

End Class
