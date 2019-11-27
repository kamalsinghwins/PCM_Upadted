Imports pcm.DataLayer

Public Class VideosBusinessLayer

    Public Function GetVideoLocation() As String

        Dim dLayer As New VideosDataLayer

        Return dLayer.GetVideoLocation()

    End Function
End Class
