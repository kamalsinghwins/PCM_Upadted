Imports pcm.DataLayer
Imports Entities

Public Class WidgetBusinessLayer

    Dim _dlWidgetCallsForToday As New WidgetDataLayer

    Public Function GetCallsForToday(ByVal tmpUsername As String) As CallWidgets

        Return _dlWidgetCallsForToday.CallsForUser(tmpUsername)

    End Function
End Class
