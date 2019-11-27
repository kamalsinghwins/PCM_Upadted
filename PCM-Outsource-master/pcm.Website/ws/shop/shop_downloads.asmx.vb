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
Public Class shop_downloads
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetStockcodes(ByVal Password As String, ByVal Database As String, ByVal LastInsertDate As String,
                                  ByVal LastUpdateDate As String,
                                  ByVal isFirstRun As Boolean, ByVal BranchCode As String) As List(Of Stockcodes)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetStockcodes(LastInsertDate, LastUpdateDate, isFirstRun,
                                     Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                     BranchCode, sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetSomeStockcodes() As List(Of Stockcodes)

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetSomeStockcodes(Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function NeedsRestart() As String

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.NeedsRestart(Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function NeedsRestartPOS() As String

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.NeedsRestartPOS(Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function GetBackdatedStockcodes(ByVal Password As String, ByVal Database As String,
                                           ByVal DateFrom As String, ByVal DateTo As String,
                                           ByVal BranchCode As String) As List(Of Stockcodes)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetBackdatedStockcodes(DateFrom, DateTo, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), BranchCode)

    End Function


    <WebMethod()>
    Public Function GetBranches(ByVal Password As String, ByVal Database As String, ByVal LastInsertDate As String,
                                ByVal LastUpdateDate As String,
                                ByVal BranchCode As String) As List(Of Branch)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetBranches(LastInsertDate, LastUpdateDate,
                                   BranchCode,
                                   Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                   sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetTillNumbers(ByVal Password As String, ByVal Database As String, ByVal LastInsertDate As String,
                                   ByVal BranchCode As String) As List(Of TillNumbers)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetTillNumbers(LastInsertDate,
                                      BranchCode,
                                      Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                      sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetUsers(ByVal Password As String, ByVal Database As String,
                             ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                             ByVal BranchCode As String) As List(Of UserPermissions)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetUsers(LastInsertDate, LastUpdateDate,
                                BranchCode,
                                Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetColourGrids(ByVal Password As String, ByVal Database As String, ByVal LastInsertDate As String,
                                   ByVal LastUpdateDate As String, ByVal BranchCode As String) As List(Of ColourGrid)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetColourGrids(LastInsertDate, LastUpdateDate,
                                      BranchCode,
                                      Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                      sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetStockOnHand(ByVal Password As String, ByVal Database As String,
                                   ByVal LastUpdateDate As String,
                                   ByVal BranchCode As String) As List(Of StockOnHand)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetStockOnHand(LastUpdateDate, BranchCode,
                                      Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                      sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetAllStockOnHand(ByVal Password As String, ByVal Database As String,
                                      ByVal BranchCode As String) As List(Of StockOnHand)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetAllStockOnHand(BranchCode,
                                         Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                         sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetEmployeeDetails(ByVal Password As String, ByVal Database As String,
                                       ByVal LastUpdateDate As String,
                                       ByVal BranchCode As String) As List(Of EmployeeDetail)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetEmployeeDetails(LastUpdateDate, BranchCode,
                                          Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                          sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetUpdatedTaxGroups(ByVal Password As String, ByVal Database As String,
                                        ByVal BranchCode As String,
                                        ByVal LastUpdateDate As String) As List(Of TaxGroup)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetTaxGroups(BranchCode,
                                    Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                    LastUpdateDate,
                                    sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetSizeGrids(ByVal Password As String, ByVal Database As String,
                                 ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                 ByVal BranchCode As String) As List(Of SizeGrid)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetSizeGrids(LastInsertDate, LastUpdateDate,
                                   BranchCode,
                                   Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                   sIPAddress)

    End Function

    <WebMethod()>
    Public Function GetCompanyDetails(ByVal Password As String, ByVal Database As String,
                                      ByVal LastUpdateDate As String,
                                      ByVal BranchCode As String) As CompanyDetails

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetCompanyDetails(Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                         LastUpdateDate, BranchCode, sIPAddress)

    End Function



    <WebMethod()>
    Public Function GetNewSpecials(ByVal Password As String, ByVal Database As String,
                                   ByVal BranchCode As String,
                                   ByVal LastUpdateDate As String) As List(Of Special)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetNewSpecials(BranchCode,
                                      Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                      LastUpdateDate,
                                      sIPAddress)

    End Function


    <WebMethod()>
    Public Function GetSingleStockcode(ByVal Password As String,
                                  ByVal Database As String,
                                  ByVal MasterCode As String,
                                  ByVal GeneratedCode As String,
                                  ByVal BranchCode As String) As List(Of Stockcodes)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetSingleStockcode(Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), MasterCode, GeneratedCode, BranchCode)

    End Function

    <WebMethod()>
    Public Function GetLoyaltyDiscounts(ByVal Password As String,
                                       ByVal LastUpdateDate As String,
                                       ByVal BranchCode As String) As List(Of LoyaltyDiscounts)

        If Password <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetLoyaltyDiscounts(LastUpdateDate, BranchCode,
                                          Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                          sIPAddress)

    End Function
End Class