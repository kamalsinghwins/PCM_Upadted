Imports Entities

Public Class SMSLogDL
    Inherits DataAccessLayerBase

    Public Sub SMSLogging(sms_log As SMSServiceResponse)

        tmpSQL = "insert into sent_sms_log(user_name,data,status,error_message,result,failed_enteries, total_phone_numbers_count, failed_phone_number_count,sent_message)" & "VALUES('" & RG.Apos(sms_log.UserName) & "','" & RG.Apos(sms_log.Data) & "','" & sms_log.Status & "','" & sms_log.ErrorMessage & "','" & sms_log.Result & "','" & sms_log.FailedEnteries & "', " & sms_log.TotalNumbers & ", " & sms_log.TotalFailedNumbers & ", " & sms_log.SentMessage & ")"
        Try
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
