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
Public Class cashcards
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetCustomerList(ByVal AccountDetails As CashCardCustomer,
                                    ByVal isSearchByID As Boolean, ByVal SearchCriteria As String) As List(Of CashCardCustomer)

        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        AccountDetails.IPAddress = sIPAddress

        Return Blayer.GetCustomerList(AccountDetails, isSearchByID, SearchCriteria)

    End Function

    <WebMethod()> _
    Public Function GetCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        AccountDetails.IPAddress = sIPAddress

        Return Blayer.GetCustomerDetails(AccountDetails)

    End Function

    <WebMethod()> _
    Public Function AssignCard(ByVal AccountDetails As CashCardCustomer) As String
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        AccountDetails.IPAddress = sIPAddress

        Return Blayer.AssignCard(AccountDetails)

    End Function

    <WebMethod()>
    Public Function PostCashCardTransaction(ByVal CashCardTransactionData As CashCardEntity) As Boolean
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        CashCardTransactionData.IPAddress = sIPAddress

        Return Blayer.PostCashCardTransaction(CashCardTransactionData, sIPAddress)


    End Function

    <WebMethod()>
    Public Function GetPointsBalance(ByVal CardData As CashCardCustomer) As CashCardCustomer
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        CardData.IPAddress = sIPAddress

        Return Blayer.GetPointsBalance(CardData)

    End Function

    <WebMethod()> _
    Public Function RedeemPoints(ByVal CardData As CashCardEntity) As String
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        CardData.IPAddress = sIPAddress

        Return Blayer.RedeemPoints(CardData)

    End Function

    <WebMethod()> _
    Public Function ProcessGiftCardSale(ByVal CardData As CashCardEntity) As String
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        CardData.IPAddress = sIPAddress

        Return Blayer.ProcessGiftCardSale(CardData)

    End Function

    <WebMethod()> _
    Public Function UpdateCustomerTable(ByVal AccountDetails As CashCardCustomer) As String

        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        AccountDetails.IPAddress = sIPAddress

        Return Blayer.UpdateCustomerTable(AccountDetails)

    End Function

    <WebMethod()> _
    Public Function CheckSystemStatus() As Boolean

        Return True

    End Function

End Class