Imports Entities.Login
Imports pcm.DataLayer
Public Class LoginBusinessLayer
    Dim _dlayer As New LoginDataLayer
    Public Function DoLogIn(username As String, password As String, till_number As String, branch As String) As BaseResponse
        Return _dlayer.DoLogIn(username, password, till_number, branch)
    End Function

End Class
