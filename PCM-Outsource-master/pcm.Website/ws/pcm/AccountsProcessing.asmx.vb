Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports pcm.BusinessLayer
Imports Entities

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AccountsProcessing
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function ProcessAccountTransaction(ByVal AccountDetails As PCMAccount) As PCMAccount

        Dim _BLayer As New AccountsBL
        Return _BLayer.ProcessAccountTransaction(AccountDetails)

    End Function

    <WebMethod()> _
    Public Function ProcessManualPayment(ByVal AccountDetails As PCMAccount) As PCMAccount

        Dim _BLayer As New AccountsBL
        Return _BLayer.ProcessManualPayment(AccountDetails)

    End Function

    <WebMethod()> _
    Public Function CheckSystemStatus() As Boolean

        Return True

    End Function

    <WebMethod()> _
    Public Function GetSelfActivateDetails(ByVal Password As String, ByVal IDNumber As String, ByVal BranchCode As String) As Debtor

        Dim _BLayer As New AccountsBL
        Return _BLayer.GetSelfActivateDetails(Password, IDNumber, BranchCode)

    End Function

    <WebMethod()>
    Public Function InsertSelfActivated(ByVal Password As String, ByVal DebtorDetails As Debtor) As Debtor

        Dim _BLayer As New AccountsBL
        Return _BLayer.InsertSelfActivated(Password, DebtorDetails)

    End Function

    <WebMethod()>
    Public Sub UpdateCellPhoneNumber(ByVal Password As String, ByVal strAccountNumber As String,
                                     ByVal strEMailAddress As String,
                                     ByVal strUser As String, ByVal tOldVal As String, ByVal tNewVal As String)

        Dim _BLayer As New AccountsBL
        _BLayer.UpdateCellPhoneNumber(Password, strAccountNumber, strUser, tOldVal, tNewVal, strEMailAddress)

    End Sub

    <WebMethod()> _
    Public Function GetCardNumber(ByVal Password As String, ByVal IDNumber As String) As String

        Dim _BLayer As New AccountsBL
        Return _BLayer.GetCardNumber(Password, IDNumber)

    End Function

End Class