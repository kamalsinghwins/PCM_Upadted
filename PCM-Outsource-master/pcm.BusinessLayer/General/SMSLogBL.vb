Imports Entities
Imports pcm.DataLayer
Public Class SMSLogBL
    Private _dlLogging As New SMSLogDL

    Public Sub SMSLogging(sms_error_log As SMSServiceResponse)

        _dlLogging.SMSLogging(sms_error_log)

    End Sub

End Class
