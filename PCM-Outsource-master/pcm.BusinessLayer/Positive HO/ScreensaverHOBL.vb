Imports pcm.DataLayer
Imports Entities

Public Class ScreensaverHOBL

    Dim _DLayer As ScreensaverHODL

    Public Sub New(ByVal CompanyCode As String)
        _DLayer = New ScreensaverHODL(CompanyCode)
    End Sub

    Public Sub New()
        _DLayer = New ScreensaverHODL
    End Sub

    Public Function UploadScreensaverImage(ByVal ImageName As String, ByVal ImageDescription As String, ByVal FileLocation As String) As String

        Return _DLayer.UploadScreensaverImage(ImageName, ImageDescription, FileLocation)

    End Function

    Public Function GetImageList() As DataTable

        Return _DLayer.GetImageList

    End Function

    Public Function GetImages() As entScreensaverImages

        Return _DLayer.GetImages

    End Function

    Public Function UpdateImages(ByVal lstImages As entScreensaverImages) As String

        Return _DLayer.UpdateImages(lstImages)

    End Function
End Class
