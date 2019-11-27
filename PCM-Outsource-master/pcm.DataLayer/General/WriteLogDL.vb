
Imports Microsoft.VisualBasic

Public Class WriteLogDL

    Dim _StrmWriter As IO.StreamWriter
    Dim RG As New Utilities.clsUtil

    Public Sub New(ByVal FileNameToCreate As String)

        _StrmWriter = New IO.StreamWriter(FileNameToCreate)



    End Sub

    Public Sub WriteLine(ByVal LineData As String)
        _StrmWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & " " & LineData)

    End Sub

    Public Sub CloseStream()
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Function Amp(ByVal funcString As String) As String

        If InStr(funcString, "&") Then
            funcString = funcString.Replace("&", "And")
        End If

        Return funcString

    End Function

End Class
