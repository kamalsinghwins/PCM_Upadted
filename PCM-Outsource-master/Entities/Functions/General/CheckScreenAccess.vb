Public NotInheritable Class CheckScreenAccess
    Public Shared Function CheckAccess(ByVal permission_string As String, ByVal screen_number As Integer) As Boolean
        If Mid$(permission_string, screen_number, 1) = "1" Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
