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
Public Class self_assist
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetStockOnHand(ByVal wsPassword As String, ByVal BranchCode As String, ByVal Stockcode As String) As DataSet

        Dim Blayer As New SelfAssistBL

        Return Blayer.GetStockOnHand(wsPassword, BranchCode, Stockcode)

    End Function

    <WebMethod()> _
    Public Function GetAccountDetails(ByVal wsPassword As String, ByVal AccountDetails As Debtor) As Debtor

        Dim Blayer As New SelfAssistBL

        Return Blayer.GetAccountDetails(wsPassword, AccountDetails)

    End Function

    <WebMethod()> _
    Public Function GetLoyaltyCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer

        Dim NewCashCardCustomer As New CashCardCustomer

        'Check if password is valid
        If AccountDetails.Password <> "JaiRL10nFMNo$forany" Then
            NewCashCardCustomer.ReturnMessage = "General Error"
            Return NewCashCardCustomer
        End If

        Dim Blayer As New SelfAssistBL

        Return Blayer.GetCustomerDetails(AccountDetails)

    End Function

    <WebMethod()> _
    Public Function RunStatement(ByVal wsPassword As String, ByVal AccountNumber As String) As ShopMiniStatement

        Dim Blayer As New SelfAssistBL

        Return Blayer.RunStatement(wsPassword, AccountNumber)

    End Function

    <WebMethod()> _
    Public Function ReturnStatementsFromAccountNumber(ByVal wsPassword As String,
                                                      ByVal strAccountNumber As String) As ShopMiniStatement

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim MiniStatement As New ShopMiniStatementBusinessLayer
        Return MiniStatement.GetData("", strAccountNumber)

    End Function

    <WebMethod()> _
    Public Function GetLatestStyles(ByVal wsPassword As String, ByVal BranchCode As String) As StyleNumbers

        Dim Blayer As New SelfAssistBL

        Return Blayer.GetLatestStyles(wsPassword, BranchCode)

    End Function

    <WebMethod()> _
    Public Function GetScreenSaverImages(ByVal wsPassword As String) As entScreensaverImages

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim Blayer As New SelfAssistBL

        Return Blayer.GetScreenSaverImages()

    End Function

End Class