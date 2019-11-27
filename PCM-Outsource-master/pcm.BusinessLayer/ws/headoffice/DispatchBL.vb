Imports pcm.DataLayer

Public Class DispatchBL

    Dim _DLayer As New DispatchDL

    Public Function ReturnBranchDetails(ByVal CompanyCode As String, ByVal IBTOutNumber As String, ByVal isDispatch As Boolean) As DataTable

        Return _DLayer.ReturnBranchDetails(CompanyCode, IBTOutNumber, isDispatch)

    End Function

    Public Function DispatchIBT(ByVal CompanyCode As String, ByVal Driver As String,
                                ByVal KM As String, ByVal Rego As String, ByVal IBTOutNumbers As DataTable, Optional ByVal DispatchNumber As String = "") As String

        Return _DLayer.DispatchIBT(CompanyCode, Driver, KM, Rego, IBTOutNumbers, DispatchNumber)

    End Function

    Public Function ReturnToWarehouse(ByVal CompanyCode As String, ByVal IBTOutNumbers As DataTable) As String

        Return _DLayer.ReturnToWarehouse(CompanyCode, IBTOutNumbers)

    End Function

    Public Function GetDispatchNumber()

        Return _DLayer.GetDispatchNumber()

    End Function
End Class
