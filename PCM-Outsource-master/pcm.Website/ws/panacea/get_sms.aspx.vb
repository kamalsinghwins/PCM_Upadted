Imports System.IO
Imports pcm.BusinessLayer


Public Class get_sms
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim tonumber As String = Request.QueryString("to")
        Dim fromnumber As String = Request.QueryString("from")
        Dim message As String = Request.QueryString("message")

        Using tmpStreamWriter As New StreamWriter(Server.MapPath("~/logs/sms-" & Format(Now, "yyyy-MM-dd") & ".txt"), True)
            tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",to," & tonumber & ",from," & fromnumber & ",message," & message)
        End Using

        Dim _BLayer As New IncomingRageSMSBusinessLayer

        _BLayer.InsertRageSMS(fromnumber, tonumber, message)

        If fromnumber = "" Then Exit Sub
        If message = "" Then Exit Sub

        'Dealing with Yes replies to limit increases

        If message.ToLower.Contains("yes") Then
            _BLayer.DoLimitIncrease(fromnumber)
        End If

    End Sub

End Class