Imports Microsoft.VisualBasic

Namespace UTIL

    Public Class clsUtil

        Public Function Comma(ByVal theString As String) As String
            Dim zLoop As Long
            Dim NewString As String

            NewString = ""

            For zLoop = 1 To Len(theString)
                If Mid$(theString, zLoop, 1) = "," Then
                    NewString = NewString & " "
                Else
                    NewString = NewString & Mid$(theString, zLoop, 1)
                End If
            Next zLoop

            Comma = NewString
        End Function

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

        Public Function Num(ByVal strString As String) As String
            Num = Format(Val(strString), "0.00")
        End Function

        Public Function Num(ByVal dVal As Double) As String
            Num = Format(dVal, "0.00")
        End Function

    End Class

End Namespace
