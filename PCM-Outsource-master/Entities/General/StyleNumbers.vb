Public Class StyleNumbers
    Public Property StyleNumberList As List(Of StyleNumber)
    Public Property ColourMatrix As List(Of ColourCodes)
End Class

Public Class StyleNumber
    Public Property Style As String
    Public Property Description As String
    Public Property Price As String
    Public Property ImageLocation As String
    Public Property CategoryDescription As String
    Public Property ColourMatrix As String
End Class

Public Class ColourCodes
    Public Property ColourCode As String
    Public Property ColourDescription As String
End Class
