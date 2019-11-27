Imports Entities

Public Class UsersDataLayer


    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function CheckUser(ByVal objUser As Users) As Users

        On Error GoTo FErr

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        Dim _Users As New Users

        tmpSQL = "SELECT is_supervisor,permissions FROM users WHERE username = '" & RG.Apos(objUser.Username.ToUpper) & "' AND user_password = '" & RG.Apos(objUser.Password.ToUpper) & "'"
        ds = objDB.GetDataSet(tmpSQL)
        objDB.CloseConnection()
        If objDB.isR(ds) Then
            For Each dr As DataRow In ds.Tables(0).Rows
                _Users.Username = objUser.Username
                _Users.IPAddress = objUser.IPAddress
                _Users.Permissions = dr("permissions") & ""
                _Users.IsAdministrator = dr("is_supervisor")
            Next
        Else
            Return Nothing
        End If

        Return _Users

FErr:
        Return Nothing

    End Function

    Public Function ProcessLogin(ByRef objUser As Users) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "INSERT INTO user_logins (guid,date_time_of_login,user_name,is_administrator,permissions,ip_address) VALUES (" & _
                 "'" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd HH:mm") & "','" & RG.Apos(objUser.Username) & "'," & _
                 "'" & objUser.IsAdministrator & "','" & RG.Apos(objUser.Permissions) & "','" & RG.Apos(objUser.IPAddress) & "')"
        Try
            objDB.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return False
        Finally
            objDB.CloseConnection()
        End Try

        Return True

    End Function

End Class
