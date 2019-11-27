Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Entities
Imports pcm.BusinessLayer
Imports System.Xml.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ws
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetAllBranches(ByVal Password As String) As Branches
        Dim _dt As New Branches

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim Blayer As New MaintenanceBL

        Return Blayer.GetAllBranches("010", False, sIPAddress)

        Return _dt


    End Function
    <WebMethod()>
    Public Function GetListOfBranchesByName(ByVal Password As String, ByVal BranchName As String) As Branches
        Dim _dt As New Branches

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim Blayer As New MaintenanceBL

        Return Blayer.GetBranchDetailsByName("010", BranchName, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"),
                                             sIPAddress)

        Return _dt


    End Function
    <WebMethod()>
    Public Function GetListOfStockcodes(ByVal Password As String, ByVal Stockcode As String) As List(Of Stockcodes)

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetStockcodesByStockcode(Stockcode, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)


    End Function
    <WebMethod()>
    Public Function GetListOfStockOnHandByStockcodeByBranch(ByVal Password As String, ByVal Stockcode As String,
                                                            ByVal BranchCode As String) As List(Of StockOnHand)

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetListOfStockOnHandByStockcodeByBranch(Stockcode, BranchCode,
                                                               Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)


    End Function
    <WebMethod()>
    Public Function GetListOfStockOnHandByStockcode(ByVal Password As String, ByVal Stockcode As String) As List(Of StockOnHand)

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopDownloadsBL()

        Return _BLayer.GetListOfStockOnHandByStockcode(Stockcode,
                                                       Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)


    End Function
    <WebMethod()>
    Public Function GetColourList(ByVal Password As String, ByVal Mastercode As String) As List(Of ColourGrid)
        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Dim _BLayer As New ShopDownloadsBL
        Return _BLayer.GetColourList(Mastercode, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)
    End Function
    <WebMethod()>
    Public Function GetSizesList(ByVal Password As String, ByVal Mastercode As String) As List(Of SizeGrid)
        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Dim _BLayer As New ShopDownloadsBL
        Return _BLayer.GetSizesList(Mastercode, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)
    End Function
    <WebMethod(), XmlInclude(GetType(StockResponse))>
    Public Function GetItemByStore(ByVal Password As String, ByVal MasterCode As String, ByVal Colour As String, ByVal Size As String)
        Dim sIPAddress As String

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Dim _BLayer As New ShopDownloadsBL
        Return _BLayer.GetItemByBranch(MasterCode, Colour, Size, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)
    End Function

    <WebMethod(), XmlInclude(GetType(StockResponse))>
    Public Function GetItemByURL(ByVal Password As String, ByVal MasterCode As String,
                                 ByVal Colour As String, ByVal Size As String, ByVal tURL As String)
        Dim sIPAddress As String

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Dim _BLayer As New ShopDownloadsBL
        Return _BLayer.GetItemByURL(tURL, MasterCode, Colour, Size, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)
    End Function

    <WebMethod(), XmlInclude(GetType(StockResponse))>
    Public Function GetItemByLatitudeLongitude(ByVal Password As String, ByVal MasterCode As String,
                                 ByVal Colour As String, ByVal Size As String, ByVal Latitude As String,
                                 ByVal Longitude As String)
        Dim sIPAddress As String

        If Password <> "hYYamk1@9Iqq1" Then
            Return Nothing
        End If

        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Dim _BLayer As New ShopDownloadsBL
        Return _BLayer.GetItemByLatitudeLongitude(Latitude, Longitude, MasterCode, Colour, Size,
                                    Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"), sIPAddress)
    End Function
End Class