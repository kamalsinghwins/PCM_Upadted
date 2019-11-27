Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports pcm.BusinessLayer


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class RTT
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function ReturnBranchDetails(ByVal wsPassword As String, ByVal CompanyCode As String, ByVal IBTOutNumber As String) As DataTable

        'Check if password is valid
        If wsPassword <> "rTt45$&34Sd" Then
            Return Nothing
        End If

        Dim _BLayer As New RTTBL

        Return _BLayer.ReturnBranchDetails(CompanyCode, IBTOutNumber)


    End Function

End Class