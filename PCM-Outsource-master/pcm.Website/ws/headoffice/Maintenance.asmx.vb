Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Entities
Imports pcm.BusinessLayer

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Maintenance
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetBranchDetails(ByVal wsPassword As String, ByVal CompanyCode As String, ByVal BranchCode As String) As Branch

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New MaintenanceBL

        Return _BLayer.GetBranchDetails(CompanyCode, BranchCode)


    End Function

    <WebMethod()>
    Public Function GetAllBranches(ByVal wsPassword As String, ByVal CompanyCode As String) As Branches

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New MaintenanceBL

        Return _BLayer.GetAllBranches(CompanyCode)

    End Function

    <WebMethod()>
    Public Function GetBranchTargets(ByVal wsPassword As String, ByVal CompanyCode As String,
                                     ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New MaintenanceBL

        Return _BLayer.GetBranchTargets(CompanyCode, TargetYear, TargetMonth)

    End Function

    <WebMethod()>
    Public Function UpdateBranchTargets(ByVal wsPassword As String, ByVal CompanyCode As String,
                                        ByVal TargetYear As String, ByVal TargetMonth As String,
                                        ByVal _Branches As DataTable) As String

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New MaintenanceBL

        Return _BLayer.UpdateBranchTargets(CompanyCode, TargetYear, TargetMonth, _Branches)

    End Function
End Class