Imports pcm.DataLayer
Imports Entities
Public Class DeleteIBTHOBL
    Dim _DLayer As DeleteIBTHODL
    Public Sub New()
        _DLayer = New DeleteIBTHODL
    End Sub

    Public Function DeleteIBT(ByVal SentFrom As String, ByVal SentTo As String, ByVal Reason As String, ByVal AuthorisedBy As String, ByVal IBTNumber As String, ByVal Username As String) As BaseResponse

        Return _DLayer.DeleteIBT(SentFrom, SentTo, IBTNumber, Username, Reason, AuthorisedBy)

    End Function
    Public Function ManageIBT(ByVal SentFrom As String, ByVal SentTo As String, ByVal IBTNumber As String, ByVal Notes As String, ByVal Username As String) As BaseResponse

        Return _DLayer.ManageIBT(SentFrom, SentTo, IBTNumber, Notes, Username)

    End Function
End Class
