Public Class CreateDeliveryNote
    Dim _StrmWriter As IO.StreamWriter
    Dim RG As New CommonFunctions.clsCommon

    Public Sub New(ByVal FileNameToCreate As String)

        _StrmWriter = New IO.StreamWriter(FileNameToCreate)
        _StrmWriter.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
        _StrmWriter.WriteLine("<mainroot>")

    End Sub

    Public Sub CloseStream()
        _StrmWriter.WriteLine("</mainroot>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Sub WriteLine(ByVal BranchCode As String, ByVal BranchName As String, ByVal address1 As String, ByVal address2 As String,
                         ByVal address3 As String, ByVal address4 As String, ByVal address5 As String, ByVal ibt_number As String,
                         ByVal Driver As String, ByVal Rego As String, ByVal KM As String)

        _StrmWriter.WriteLine("<doc>")
        _StrmWriter.WriteLine("<Driver>" & RG.Amp(Driver) & "</Driver>")
        _StrmWriter.WriteLine("<Date>" & Format(Now, "yyyy-MM-dd") & "</Date>")
        _StrmWriter.WriteLine("<Rego>" & RG.Amp(Rego) & "</Rego>")
        _StrmWriter.WriteLine("<KM>" & RG.Amp(KM) & "</KM>")
        _StrmWriter.WriteLine("<branch_code>" & RG.Amp(BranchCode) & "</branch_code>")
        _StrmWriter.WriteLine("<branch_name>" & RG.Amp(BranchName) & "</branch_name>")
        _StrmWriter.WriteLine("<address1>" & RG.Amp(address1) & "</address1>")
        _StrmWriter.WriteLine("<address2>" & RG.Amp(address2) & "</address2>")
        _StrmWriter.WriteLine("<address3>" & RG.Amp(address3) & "</address3>")
        _StrmWriter.WriteLine("<address4>" & RG.Amp(address4) & "</address4>")
        _StrmWriter.WriteLine("<address5>" & RG.Amp(address5) & "</address5>")
        _StrmWriter.WriteLine("<ibt_number>" & RG.Amp(ibt_number) & "</ibt_number>")
        _StrmWriter.WriteLine("</doc>")
    End Sub

    Public Sub ShopAttribute(ByVal isOpen As Boolean)

        If isOpen Then
            _StrmWriter.WriteLine("<Shops>")
        Else
            _StrmWriter.WriteLine("</Shops>")
        End If
    End Sub
End Class
