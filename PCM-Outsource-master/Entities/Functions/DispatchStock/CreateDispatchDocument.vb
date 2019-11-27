

Public Class CreateDispatchDocument
    Dim _StrmWriter As IO.StreamWriter
    Dim RG As New CommonFunctions.clsCommon

    Private _Driver As String
    Private _Rego As String
    Private _KM As String
    Private _CurrentDate As String

    Public Sub New(ByVal FileNameToCreate As String, ByVal Driver As String, ByVal Rego As String, ByVal KM As String)

        _Driver = Driver
        _Rego = Rego
        _KM = KM
        _CurrentDate = Format(Now, "yyyy-MM-dd")

        _StrmWriter = New IO.StreamWriter(FileNameToCreate)
        _StrmWriter.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
        _StrmWriter.WriteLine("<Document>")
        _StrmWriter.WriteLine("<mainroot>")

    End Sub

    Public Sub CloseStream()
        _StrmWriter.WriteLine("</mainroot>")
        _StrmWriter.WriteLine("</Document>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Sub WriteLine(ByVal BranchCode As String, ByVal BranchName As String, ByVal NumberOfBoxes As String)

        _StrmWriter.WriteLine("<Boxes>")
        _StrmWriter.WriteLine("<Driver>" & RG.Amp(_Driver) & "</Driver>")
        _StrmWriter.WriteLine("<Rego>" & RG.Amp(_Rego) & "</Rego>")
        _StrmWriter.WriteLine("<KM>" & RG.Amp(_KM) & "</KM>")
        _StrmWriter.WriteLine("<Date>" & _CurrentDate & "</Date>")
        _StrmWriter.WriteLine("<BranchCode>" & RG.Amp(BranchCode) & "</BranchCode>")
        _StrmWriter.WriteLine("<BranchName>" & RG.Amp(BranchName) & "</BranchName>")
        _StrmWriter.WriteLine("<NumberOfBoxes>" & RG.Amp(NumberOfBoxes) & "</NumberOfBoxes>")
        _StrmWriter.WriteLine("</Boxes>")
    End Sub

End Class
