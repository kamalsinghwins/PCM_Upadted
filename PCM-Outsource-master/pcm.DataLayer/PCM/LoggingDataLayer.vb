Imports Entities

Public Class LoggingDataLayer
    Inherits DataAccessLayerBase
    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")


    Public Function WriteToLog(ByVal LogDetails As CollectionCallLog) As Boolean

        If LogDetails.PTPDate = "" Then
            tmpSQL = "INSERT INTO users_actions (guid,username,timestamp_of_action,action_type,account_number," &
                "user_comment,action_result,ptp_amount,length_of_action,ip_address,collections_period) VALUES (" &
                "'" & Guid.NewGuid.ToString & "','" & RG.Apos(LogDetails.UserName) & "','" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "'," &
                "'" & LogDetails.ActionType & "','" & LogDetails.AccountNumber & "','" & Mid$(RG.Apos(LogDetails.UserComment), 1, 495) & "','" & LogDetails.ActionResult & "'," &
                "'" & LogDetails.PTPAmount & "','" & Mid$(LogDetails.LengthOfAction, 1, 4) & "','" & LogDetails.IPAddress & "','" & LogDetails.CollectionsPeriod & "')"
        Else
            tmpSQL = "INSERT INTO users_actions (guid,username,timestamp_of_action,action_type,account_number," &
                "user_comment,action_result,ptp_amount,length_of_action,ip_address,ptp_date,collections_period) VALUES (" &
                "'" & Guid.NewGuid.ToString & "','" & RG.Apos(LogDetails.UserName) & "','" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "'," &
                "'" & LogDetails.ActionType & "','" & LogDetails.AccountNumber & "','" & Mid$(RG.Apos(LogDetails.UserComment), 1, 495) & "','" & LogDetails.ActionResult & "'," &
                "'" & LogDetails.PTPAmount & "','" & Mid$(LogDetails.LengthOfAction, 1, 4) & "','" & LogDetails.IPAddress & "','" & LogDetails.PTPDate & "','" & LogDetails.CollectionsPeriod & "')"
        End If

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

    Public Function WriteToLogPCM(ByVal LogDetails As PCMUserLog) As Boolean

        tmpSQL = "INSERT INTO users_actions (guid,username,timestamp_of_action,action_type,account_number," &
            "user_comment,ip_address,web_page,search_criteria) VALUES (" &
            "'" & Guid.NewGuid.ToString & "','" & RG.Apos(LogDetails.UserName) & "','" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "'," &
            "'" & LogDetails.ActionType & "','" & RG.Apos(LogDetails.AccountNumber) & "','" & Mid$(RG.Apos(LogDetails.UserComment), 1, 495) & "'," &
            "'" & LogDetails.IPAddress & "','" & LogDetails.WebPage & "','" & RG.Apos(LogDetails.SearchCriteria) & "')"

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

    Public Function AddLoginDetails(ByVal loginDetail As LoginDetails) As Boolean

        tmpSQL = "INSERT INTO user_logins (
                 id_number,
                 device,
                 browser,
                 status,
                 ip_address,
                 is_mobile
                 ) VALUES (
         '" & RG.Apos(loginDetail.IDNumber) & "',            
        '" & loginDetail.Device & "',
        '" & RG.Apos(loginDetail.Browser) & "',
        '" & loginDetail.Status & "',
        '" & loginDetail.IPAddress & "',
         '" & loginDetail.IsMobile & "'
               )"

        Try
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function
End Class
