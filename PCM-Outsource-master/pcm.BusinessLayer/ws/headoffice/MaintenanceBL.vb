Imports pcm.DataLayer
Imports Entities

Public Class MaintenanceBL

    Dim _DLayer As New MaintenanceDL

    Public Function GetBranchDetails(ByVal CompanyCode As String, ByVal BranchCode As String) As Branch

        Return _DLayer.GetBranchDetails(CompanyCode, BranchCode)

    End Function

    Public Function GetBranchDetailsByName(ByVal CompanyCode As String, ByVal BranchName As String,
                                           ByVal ServerPath As String, ByVal sIPAddress As String) As Branches

        Return _DLayer.GetBranchDetailsByName(CompanyCode, BranchName, ServerPath, sIPAddress)

    End Function

    Public Function GetAllBranches(ByVal CompanyCode As String,
                                   Optional ByVal GetInternalData As Boolean = True,
                                   Optional ByVal sIPAddress As String = "") As Branches

        Return _DLayer.GetAllBranches(CompanyCode, GetInternalData, sIPAddress)

    End Function

    Public Function GetBranchTargets(ByVal CompanyCode As String,
                                    ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable

        Return _DLayer.GetBranchTargets(CompanyCode, TargetYear, TargetMonth)

    End Function

    Public Function UpdateBranchTargets(ByVal CompanyCode As String,
                                        ByVal TargetYear As String, ByVal TargetMonth As String,
                                        ByVal _Branches As DataTable) As String

        Return _DLayer.UpdateBranchTargets(CompanyCode, TargetYear, TargetMonth, _Branches)

    End Function
End Class
