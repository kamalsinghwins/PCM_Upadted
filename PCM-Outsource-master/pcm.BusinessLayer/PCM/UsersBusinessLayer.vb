Imports pcm.DataLayer
Imports Entities

Public Class UsersBusinessLayer

    Private _dlUsers As New UsersDataLayer
    Private _dlLog As New LoggingDataLayer

    Public Function DoLogin(ByRef objUser As Users) As Users

        Dim _RetrievedUser As Users

        _RetrievedUser = _dlUsers.CheckUser(objUser)

        'Username / password doesn't exist
        If _RetrievedUser Is Nothing Then
            Return Nothing
        End If

        'Create the login record
        Dim _NewLog As New CollectionCallLog

        _NewLog.AccountNumber = ""
        _NewLog.ActionResult = "Success"
        _NewLog.ActionType = "User Login"
        _NewLog.IPAddress = _RetrievedUser.IPAddress
        _NewLog.LengthOfAction = "0"
        _NewLog.PTPAmount = "0"
        _NewLog.UserComment = ""
        _NewLog.UserName = _RetrievedUser.Username
        _NewLog.PTPDate = ""

        If _dlLog.WriteToLog(_NewLog) Then
            Return _RetrievedUser
        Else
            Return Nothing
        End If




    End Function

    Public Sub WriteToLog()

    End Sub
End Class
