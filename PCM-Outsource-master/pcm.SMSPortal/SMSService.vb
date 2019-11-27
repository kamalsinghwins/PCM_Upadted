Imports Entities

Public Class SMSService
    Private _srSMS As New srSMS.API
    Private _SMSServiceResponse As New SMSServiceResponse

    Public Function Send_SMS_BY_DS_DS(ByVal username As String, ByVal password As String, ByVal default_date As String, ByVal default_time As String, ByVal message As String, ByVal Entries As DataTable) As SMSServiceResponse

        Dim DS As New DataSet("senddata")

        ' --------------------------------- Settings --------------------------------- 
        Dim DT_Settings As New DataTable("settings")
        DT_Settings.Columns.Add("live") 'OPTIONAL - True/False (default - true)
        DT_Settings.Columns.Add("return_credits") 'OPTIONAL - True/False
        DT_Settings.Columns.Add("return_msgs_success_count") 'OPTIONAL - True/False (default - false)
        DT_Settings.Columns.Add("return_msgs_failed_count") 'OPTIONAL - True/False (default - false)
        DT_Settings.Columns.Add("return_entries_success_status") 'OPTIONAL - True/False (default - false)
        DT_Settings.Columns.Add("return_entries_failed_status") 'OPTIONAL - True/False (default - false)
        DT_Settings.Columns.Add("default_senderid") 'OPTIONAL - 11 Char alphanumeric or 15 char numeric (default - Repliable)
        DT_Settings.Columns.Add("default_date") 'REQUIRED - dd/MMM/yyyy
        DT_Settings.Columns.Add("default_time") 'REQUIRED - HH:mm
        DT_Settings.Columns.Add("default_data1") 'OPTIONAL - ""
        DT_Settings.Columns.Add("default_data2") 'OPTIONAL - ""
        DT_Settings.Columns.Add("default_flash") 'OPTIONAL - True/False (default - false)
        DT_Settings.Columns.Add("default_type") 'OPTIONAL - SMS / WPUSH / VCARD / PORT (default - SMS)
        DT_Settings.Columns.Add("default_costcentre") 'OPTIONAL - ""

        Dim MainDR As DataRow = DT_Settings.NewRow
        MainDR.Item("live") = "true"
        MainDR.Item("return_credits") = "False"
        MainDR.Item("return_msgs_success_count") = "False"
        MainDR.Item("return_msgs_failed_count") = "False"
        MainDR.Item("return_entries_success_status") = "False"
        MainDR.Item("return_entries_failed_status") = "True"
        MainDR.Item("default_senderid") = "Repliable"
        MainDR.Item("default_date") = default_date
        MainDR.Item("default_time") = default_time
        MainDR.Item("default_data1") = message
        MainDR.Item("default_data2") = ""
        MainDR.Item("default_flash") = "False"
        MainDR.Item("default_type") = "SMS"
        MainDR.Item("default_costcentre") = ""
        DT_Settings.Rows.Add(MainDR)

        DS.Tables.Add(DT_Settings)

        Dim DT_Entries As New DataTable
        DT_Entries = Entries.Copy()
        DS.Tables.Add(DT_Entries)
        DS.Tables(1).TableName = "entries"
        Dim Result As DataSet

        Result = _srSMS.Send_DS_DS(username, password, DS)

        _SMSServiceResponse.Data = DS.GetXml()
        _SMSServiceResponse.TotalNumbers = DT_Entries.Rows.Count
        _SMSServiceResponse.Result = Result.GetXml()

        Dim FailedEntries As New DataTable
        Dim SendInfo As New DataTable
        Dim CallResult As New DataTable

        If Result IsNot Nothing Then
            If Result.Tables("failed_entries") IsNot Nothing Then
                FailedEntries = Result.Tables("failed_entries")
            End If

            If Result.Tables("send_info") IsNot Nothing Then
                SendInfo = Result.Tables("send_info")
            End If


            If Result.Tables("call_result") IsNot Nothing Then
                CallResult = Result.Tables("call_result")
            End If
        End If

        _SMSServiceResponse.FailedEnteries = FailedEntries.ToString()
        _SMSServiceResponse.TotalFailedNumbers = FailedEntries.Rows.Count
        If CallResult IsNot Nothing AndAlso CallResult.Rows.Count > 0 Then
            _SMSServiceResponse.Status = Convert.ToBoolean(CallResult.Rows(0)("result"))
            _SMSServiceResponse.ErrorMessage = Convert.ToString(CallResult.Rows(0)("error"))
        Else
            _SMSServiceResponse.Status = False
            _SMSServiceResponse.ErrorMessage = "Failed to send messages"
        End If

        Return _SMSServiceResponse
    End Function
End Class


