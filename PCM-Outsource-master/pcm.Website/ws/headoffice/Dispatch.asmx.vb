Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports pcm.BusinessLayer

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Dispatch
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function ReturnBranchDetails(ByVal wsPassword As String, ByVal CompanyCode As String,
                                        ByVal IBTOutNumber As String, ByVal isDispatch As Boolean) As DataTable

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New DispatchBL

        Return _BLayer.ReturnBranchDetails(CompanyCode, IBTOutNumber, isDispatch)


    End Function

    <WebMethod()>
    Public Function DispatchIBT(ByVal wsPassword As String, ByVal CompanyCode As String, ByVal Driver As String,
                                ByVal KM As String, ByVal Rego As String, ByVal IBTOutNumbers As DataTable) As String

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New DispatchBL

        Return _BLayer.DispatchIBT(CompanyCode, Driver, KM, Rego, IBTOutNumbers)

    End Function

    <WebMethod()>
    Public Function ReturnToWarehouse(ByVal wsPassword As String, ByVal CompanyCode As String, ByVal IBTOutNumbers As DataTable) As String

        'Check if password is valid
        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim _BLayer As New DispatchBL

        Return _BLayer.ReturnToWarehouse(CompanyCode, IBTOutNumbers)

    End Function

End Class