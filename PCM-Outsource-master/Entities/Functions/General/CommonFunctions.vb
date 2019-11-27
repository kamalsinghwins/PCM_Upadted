Public Class CommonFunctions
    Public Class clsCommon

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
                        sFieldString = Left$(sFieldString, apostr) & "'" &
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

        Public Function Numb(ByVal NumberToRound As String, Optional ByVal RoundDigits As Long = 2) As String
            'ORIGINAL JACQUES STEENEBRG 02/06/2007

            Dim tmpWorkingNumber As String

            On Error GoTo FErr

            'NOTE
            '2 global variables get used in this function
            'CurrencySymbol and CurrencyLocationLorR
            'These variables get values allocated at program start, when getting system defaults

            tmpWorkingNumber = NumberToRound

            Select Case RoundDigits
                Case 0 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00}")
                    tmpWorkingNumber = String.Format("{0:0}", Val(tmpWorkingNumber))
                Case 1 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.0}")
                    tmpWorkingNumber = String.Format("{0:0.0}", tmpWorkingNumber)
                Case 2 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.00}")
                    tmpWorkingNumber = String.Format("{0:0.00}", Val(tmpWorkingNumber))
                Case 3 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.000}")
                    tmpWorkingNumber = String.Format("{0:0.000}", Val(tmpWorkingNumber))
                Case 4 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.0000}")
                    tmpWorkingNumber = String.Format("{0:0.0000}", Val(tmpWorkingNumber))
                Case 5 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.00000}")
                    tmpWorkingNumber = String.Format("{0:0.00000}", Val(tmpWorkingNumber))
                Case 6 'tmpWorkingNumber = Format(Val(tmpWorkingNumber), "{0:00.000000}")
                    tmpWorkingNumber = String.Format("{0:0.000000}", Val(tmpWorkingNumber))
            End Select

            Return tmpWorkingNumber

            Exit Function
FErr:


        End Function

        Public Function ValidID(ByVal IdNum As String) As Boolean
            Dim rOdds As String
            Dim rEvens As String
            Dim lsEvens As String
            Dim lsOdds As String
            Dim Toae As String
            Dim vDiffs As String

            If Len(IdNum) <> 13 Then
                ValidID = False
                Exit Function
            End If

            rOdds = Mid$(IdNum, 1, 1) & Mid$(IdNum, 3, 1) & Mid$(IdNum, 5, 1) & Mid$(IdNum, 7, 1) & Mid$(IdNum, 9, 1) & Mid$(IdNum, 11, 1)
            rEvens = Mid$(IdNum, 2, 1) & Mid$(IdNum, 4, 1) & Mid$(IdNum, 6, 1) & Mid$(IdNum, 8, 1) & Mid$(IdNum, 10, 1) & Mid$(IdNum, 12, 1)

            rEvens = Val(rEvens) * 2
            'done with rough odds and evens

            lsEvens = "0"
            lsOdds = "0"

            For mLoop = 1 To Len(rOdds)
                lsOdds = Val(lsOdds) + Val(Mid$(rOdds, mLoop, 1))
            Next mLoop

            For mLoop = 1 To Len(rEvens)
                lsEvens = Val(lsEvens) + Val(Mid$(rEvens, mLoop, 1))
            Next mLoop

            Toae = Val(lsEvens) + Val(lsOdds) + Mid$(IdNum, 13, 1)

            vDiffs = Val(Toae) / 10

            If Len(vDiffs) > 2 Then
                ValidID = False
            Else
                ValidID = True
            End If


        End Function

        Public Function FormatCurrency(ByVal currency As String) As String
            Return currency.Replace(" ", "").Replace(",", ".")
        End Function
        Public Function Amp(ByVal funcString As String)

            If InStr(funcString, "&") Then
                funcString = funcString.Replace("&", "And")
            End If

            Return funcString

        End Function

    End Class
End Class
