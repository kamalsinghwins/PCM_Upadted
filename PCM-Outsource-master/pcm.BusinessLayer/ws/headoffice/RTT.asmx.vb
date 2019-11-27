Imports pcm.DataLayer

Public Class RTTBL

    Dim _DLayer As New RTTDL

    Public Function ReturnBranchDetails(ByVal CompanyCode As String, ByVal IBTOutNumber As String) As DataTable

        Return _DLayer.ReturnBranchDetails(CompanyCode, IBTOutNumber)

    End Function


End Class