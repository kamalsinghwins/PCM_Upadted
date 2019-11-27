Imports pcm.DataLayer
Imports Entities

Public Class ShopDownloadsBL

    Dim _DLayer As ShopDownloadsDL
    Dim _database As String

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New ShopDownloadsDL(CompanyCode)
    '    _database = CompanyCode
    'End Sub

    Public Sub New()
        _DLayer = New ShopDownloadsDL
        _database = "010"
    End Sub

    Public Function GetTillNumbers(ByVal LastInsertDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of TillNumbers)

        Return _DLayer.GetTillNumbers(LastInsertDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetColourGrids(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of ColourGrid)

        Return _DLayer.GetColourGrids(LastInsertDate, LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetTaxGroups(ByVal BranchCode As String, ByVal ServerPath As String,
                                 ByVal LastUpdateDate As String,
                                 ByVal sIPAddress As String) As List(Of TaxGroup)

        Return _DLayer.GetTaxGroups(BranchCode, ServerPath, LastUpdateDate, sIPAddress)

    End Function

    Public Function GetNewSpecials(ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal LastUpdateDate As String,
                                   ByVal sIPAddress As String) As List(Of Special)

        Return _DLayer.GetNewSpecials(BranchCode, ServerPath, LastUpdateDate, sIPAddress)

    End Function

    Public Function GetUsers(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                             ByVal BranchCode As String, ByVal ServerPath As String,
                             ByVal sIPAddress As String) As List(Of UserPermissions)

        Return _DLayer.GetUsers(LastInsertDate, LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetStockOnHand(ByVal LastUpdateDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of StockOnHand)

        Return _DLayer.GetStockOnHand(LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetAllStockOnHand(ByVal BranchCode As String, ByVal ServerPath As String,
                                      ByVal sIPAddress As String) As List(Of StockOnHand)

        Return _DLayer.GetAllStockOnHand(BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetEmployeeDetails(ByVal LastUpdateDate As String,
                                       ByVal BranchCode As String, ByVal ServerPath As String,
                                       ByVal sIPAddress As String) As List(Of EmployeeDetail)

        Return _DLayer.GetEmployeeDetails(LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetEmployeeClockNumbers(ByVal SearchText As String) As DataTable

        Return _DLayer.GetEmployeeClockNumbers(SearchText)

    End Function

    Public Function GetBranches(ByVal LastInsertDate As String,
                                ByVal LastUpdateDate As String,
                                ByVal BranchCode As String, ByVal ServerPath As String,
                                ByVal sIPAddress As String) As List(Of Branch)

        Return _DLayer.GetBranches(LastInsertDate, LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetSizeGrids(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                 ByVal BranchCode As String, ByVal ServerPath As String,
                                 ByVal sIPAddress As String) As List(Of SizeGrid)

        Return _DLayer.GetSizeGrids(LastInsertDate, LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function

    Public Function GetCompanyDetails(ByVal ServerPath As String,
                                      ByVal LastUpdateDate As String,
                                      ByVal BranchCode As String,
                                      ByVal sIPAddress As String) As CompanyDetails

        Return _DLayer.GetCompanyDetails(ServerPath, LastUpdateDate, BranchCode, sIPAddress)

    End Function

    Public Function GetStockcodes(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                 ByVal isFirstRun As Boolean, ByVal ServerPath As String,
                                 ByVal BranchCode As String, ByVal sIPAddress As String) As List(Of Stockcodes)

        Return _DLayer.GetStockcodes(LastInsertDate, LastUpdateDate, isFirstRun, ServerPath, BranchCode)

    End Function

    Public Function GetStockcodesByStockcode(ByVal Stockcode As String,
                                             ByVal ServerPath As String, ByVal sIPAddress As String) As List(Of Stockcodes)

        Return _DLayer.GetStockcodesByStockcode(Stockcode, ServerPath, sIPAddress)

    End Function

    Public Function GetListOfStockOnHandByStockcodeByBranch(ByVal Stockcode As String, ByVal BranchCode As String, ByVal ServerPath As String,
                                                            ByVal sIPAddress As String) As List(Of StockOnHand)

        Return _DLayer.GetListOfStockOnHandByStockcodeByBranch(Stockcode, BranchCode, ServerPath, sIPAddress)

    End Function
    Public Function GetListOfStockOnHandByStockcode(ByVal Stockcode As String,
                                                    ByVal ServerPath As String, ByVal sIPAddress As String) As List(Of StockOnHand)

        Return _DLayer.GetListOfStockOnHandByStockcode(Stockcode, ServerPath, sIPAddress)

    End Function

    Public Function GetSomeStockcodes(ByVal ServerPath As String) As List(Of Stockcodes)

        Return _DLayer.GetSomeStockcodes(ServerPath)

    End Function

    Public Function NeedsRestart(ByVal ServerPath As String) As String

        Return _DLayer.NeedsRestart(ServerPath)

    End Function

    Public Function NeedsRestartPOS(ByVal ServerPath As String) As String

        Return _DLayer.NeedsRestartPOS(ServerPath)

    End Function

    Public Function GetBackdatedStockcodes(ByVal DateFrom As String, ByVal DateTo As String,
                                           ByVal ServerPath As String,
                                           ByVal BranchCode As String) As List(Of Stockcodes)

        Return _DLayer.GetBackdatedStockcodes(DateFrom, DateTo, ServerPath, BranchCode)

    End Function

    Public Function GetSingleStockcode(ByVal ServerPath As String,
                                       ByVal MasterCode As String,
                                       ByVal GeneratedCode As String,
                                       ByVal BranchCode As String) As List(Of Stockcodes)

        Return _DLayer.GetSingleStockcode(MasterCode, GeneratedCode, ServerPath, BranchCode)

    End Function

    Public Function GetLoyaltyDiscounts(ByVal LastUpdateDate As String,
                                        ByVal BranchCode As String, ByVal ServerPath As String,
                                        ByVal sIPAddress As String) As List(Of LoyaltyDiscounts)

        Return _DLayer.GetLoyaltyDiscounts(LastUpdateDate, BranchCode, ServerPath, sIPAddress)

    End Function
    Public Function GetColourList(ByVal MasterCode As String, ByVal ServerPath As String, ByVal sIPAddress As String) As List(Of ColourGrid)
        Return _DLayer.GetColourList(MasterCode, ServerPath, sIPAddress)
    End Function
    Public Function GetSizesList(ByVal Mastercode As String, ByVal ServerPath As String, ByVal sIPAddress As String) As List(Of SizeGrid)
        Return _DLayer.GetSizesList(Mastercode, ServerPath, sIPAddress)
    End Function

    Public Function GetItemByURL(ByVal tURL As String, ByVal MasterCode As String, ByVal Colour As String,
                                 ByVal Size As String, ByVal ServerPath As String, ByVal sIPAddress As String) As StockResponse
        Return _DLayer.GetItemByURL(tURL, MasterCode, Colour, Size, ServerPath, sIPAddress)
    End Function

    Public Function GetItemByBranch(ByVal MasterCode As String, ByVal Colour As String, ByVal Size As String,
                                    ByVal ServerPath As String, ByVal sIPAddress As String) As StockResponse
        Return _DLayer.GetItemByBranch(MasterCode, Colour, Size, ServerPath, sIPAddress)
    End Function

    Public Function GetItemByLatitudeLongitude(ByVal Latitude As String, ByVal Longitude As String, ByVal MasterCode As String, ByVal Colour As String,
                                               ByVal Size As String,
                                               ByVal ServerPath As String, ByVal sIPAddress As String) As StockResponse
        Return _DLayer.GetItemByLatitudeLongitude(Latitude, Longitude, MasterCode, Colour, Size, ServerPath, sIPAddress)
    End Function
End Class

