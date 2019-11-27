Imports Microsoft.VisualBasic

Public Class clsRegality

    Public Function Apos(ByVal sFieldString As String) As String
        If InStr(sFieldString, "'") Then
            Dim iLen As Integer
            Dim I As Integer
            Dim apostr As Integer
            iLen = Len(sFieldString)
            I = 1

            Do While I <= iLen
                If Mid$(sFieldString, I, 1) = "'" Then
                    apostr = I
                    sFieldString = Left$(sFieldString, apostr) & "'" & _
                    Right$(sFieldString, iLen - apostr)
                    iLen = Len(sFieldString)
                    I = I + 1
                End If
                I = I + 1
            Loop
        End If
        Apos = sFieldString
    End Function

    Public Function Numb(ByVal strString As String) As String
        Numb = Format(Val(strString), "0.00")
    End Function

End Class
