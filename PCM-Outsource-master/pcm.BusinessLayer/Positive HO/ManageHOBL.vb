Imports pcm.DataLayer
Imports Entities
Imports Entities.Manage
Imports Entities.SizeMatrix

Public Class ManageHOBL
    Dim _DLayer As ManageHODL
    Dim _blErrorLogging As New ErrorLogBL
    Dim baseResponse As New BaseResponse
    Dim gridResponse As New GridResponse
    Dim searchResponse As New SearchResponse
    Dim addStockResponse As New AddStockResponse
    Dim specialResponse As New SaveSpecial


    Public Sub New()
        _DLayer = New ManageHODL
    End Sub
#Region "Branch"
    Public Function GetBranches(ByVal SearchText As String, ByVal SearchDetails As String) As DataTable
        Return _DLayer.GetBranches(SearchText, SearchDetails)
    End Function
    Public Function GetBranchDetails(ByVal BranchCode As String) As DataTable
        Return _DLayer.GetBranchDetails(BranchCode)
    End Function
    Public Function DeleteBranch(ByVal BranchCode As String, ByVal BranchName As String) As BaseResponse
        Return _DLayer.DeleteBranch(BranchCode, BranchName)
    End Function
    Public Function SaveBranch(ByVal BranchDetails As BranchDetails) As BaseResponse
        Return _DLayer.SaveBranch(BranchDetails)
    End Function
    Public Function GetAllBranches() As DataTable
        Return _DLayer.GetAllBranches()

    End Function
#End Region
#Region "Till"
    Public Function GetTills(ByVal BranchCode As String) As DataTable
        Return _DLayer.GetTills(BranchCode)

    End Function
    Public Function DeleteTill(ByVal BranchCode As String, ByVal TillNumber As String) As BaseResponse
        Return _DLayer.DeleteTill(BranchCode, TillNumber)

    End Function
    Public Function SaveTill(ByVal BranchCode As String, ByVal TillNumber As String) As BaseResponse
        Return _DLayer.SaveTill(BranchCode, TillNumber)

    End Function
#End Region
#Region "Stationary"

    Public Function GetStationaryCodes(ByVal SearchType As String,
                                  ByVal SearchText As String) As DataTable

        Return _DLayer.GetStationaryCodes(SearchType, SearchText)

    End Function
    Public Function GetStationaryDetails(ByVal StationaryCode As String) As DataTable
        Return _DLayer.GetStationaryDetails(StationaryCode)

    End Function
    Public Function SaveStationary(ByVal Code As String, ByVal Description As String, ByVal Read As Boolean) As BaseResponse
        Return _DLayer.SaveStationary(Code, Description, Read)

    End Function
    Public Function DeleteStationary(ByVal StationaryCode As String) As BaseResponse
        Return _DLayer.DeleteStationary(StationaryCode)

    End Function

#End Region
#Region "ColourMatrix"

    Public Function GetColourCodes(ByVal SearchType As String,
                                  ByVal SearchText As String) As DataTable

        Return _DLayer.GetColourCodes(SearchType, SearchText)

    End Function
    Public Function SaveColourCode(ByVal Code As String, ByVal Description As String) As BaseResponse
        Return _DLayer.SaveColourCode(Code, Description)

    End Function
    'Public Function DeleteColourCode(ByVal ColourCode As String) As BaseResponse
    '    Return _DLayer.DeleteStationary(ColourCode)

    'End Function
#End Region
#Region "SizeMatrix"

    Public Function GetGrids(ByVal SearchType As String, ByVal SearchText As String) As GridResponse
        Try
            gridResponse = _DLayer.GetGrids(SearchType, SearchText)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            gridResponse.Message = "Something went wrong"
            gridResponse.Success = False
        End Try
        Return gridResponse
    End Function
    Public Function GetGrids(ByVal GridNumber As String) As GridResponse
        Try
            gridResponse = _DLayer.GetSizeGridDetails(GridNumber)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            gridResponse.Message = "Something went wrong"
            gridResponse.Success = False
        End Try
        Return gridResponse
    End Function
    Public Function SaveSizeGrid(ByVal SaveGrid As SaveGrid) As BaseResponse
        Try
            baseResponse = _DLayer.SaveSizeGrid(SaveGrid)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            baseResponse.Message = "Something went wrong"
            baseResponse.Success = False
        End Try
        Return baseResponse

    End Function

#End Region
#Region "Specials"
    Public Function SearchSpecial(ByVal SpecialName As String) As SearchResponse
        Try
            searchResponse = _DLayer.SearchSpecial(SpecialName)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            searchResponse.Message = "Something went wrong"
            searchResponse.Success = False
        End Try
        Return searchResponse
    End Function
    Public Function SearchCode(ByVal SearchType As String, ByVal SearchText As String, ByVal Master As Boolean) As SearchResponse
        Try
            searchResponse = _DLayer.SearchCode(SearchType, SearchText, Master)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            searchResponse.Message = "Something went wrong"
            searchResponse.Success = False
        End Try
        Return searchResponse
    End Function
    Public Function AddStock(ByVal Stockcode As String) As AddStockResponse
        Try
            addStockResponse = _DLayer.AddStock(Stockcode)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            addStockResponse.Message = "Something went wrong"
            addStockResponse.Success = False
        End Try
        Return addStockResponse
    End Function
    Public Function SaveSpecials(ByVal SpecialRequest As SaveSpecial) As BaseResponse
        Try
            baseResponse = _DLayer.SaveSpecials(SpecialRequest)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            baseResponse.Message = "Something went wrong"
            baseResponse.Success = False
        End Try
        Return baseResponse
    End Function
    Public Function Populate(ByVal SpecialName As String) As SaveSpecial
        Try
            specialResponse = _DLayer.Populate(SpecialName)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
            specialResponse.Message = "Something went wrong"
            specialResponse.Success = False
        End Try
        Return specialResponse
    End Function
#End Region
#Region "Regions"
    Public Function SaveRegion(ByVal Region As String) As BaseResponse
        Return _DLayer.SaveRegion(Region)
    End Function
    Public Function GetRegions() As DataTable
        Return _DLayer.GetRegions()
    End Function
    Public Function GetProvinces() As DataTable
        Return _DLayer.GetProvinces()
    End Function
    Public Function DeleteRegion(ByVal Region As String) As BaseResponse
        Return _DLayer.DeleteRegion(Region)
    End Function
#End Region
#Region "Suppliers"
    Public Function SaveUpdateSuppliers(ByVal supplier As SaveUpdateSupplier) As BaseResponse
        Return _DLayer.SaveUpdateSuppliers(supplier)
    End Function
    Public Function DeleteSupplier(ByVal SupplierCode As String) As BaseResponse
        Return _DLayer.DeleteSupplier(SupplierCode)
    End Function
    Public Function SearchSupplier(ByVal SearchType As String, ByVal SearchText As String) As DataTable
        Return _DLayer.SearchSupplier(SearchType, SearchText)
    End Function
    Public Function GetSupplierDetails(ByVal SupplierCode As String) As DataTable
        Return _DLayer.GetSupplierDetails(SupplierCode)
    End Function
#End Region
#Region "PointsMaintenance"
    Public Function GetCompanySettings() As DataTable
        Return _DLayer.GetCompanySettings
    End Function
    Public Function SaveCompanySettings(ByVal Dollars As String, ByVal Points As String) As BaseResponse
        Return _DLayer.SaveCompanySettings(Dollars, Points)
    End Function
#End Region
#Region "CategoryMaintenance"
    Public Function GetCategories(ByVal CategoryType As String, ByVal CategoryNumber As Integer) As DataTable
        Return _DLayer.GetCategories(CategoryType, CategoryNumber)
    End Function

    Public Function SaveCategory(ByVal categories As Categories) As BaseResponse
        Return _DLayer.SaveCategory(categories)
    End Function

    Public Function DeleteCategory(ByVal categories As Categories) As BaseResponse
        Return _DLayer.DeleteCategory(categories)
    End Function
#End Region
#Region "Municipality"
    Public Function SaveMunicipality(ByVal Municipality As String, ByVal BranchCode As String) As BaseResponse
        Return _DLayer.SaveMunicipality(Municipality, BranchCode)
    End Function
    Public Function UpdateMunicipality(ByVal BranchCode As String) As BaseResponse
        Return _DLayer.UpdateMunicipality(BranchCode)
    End Function
#End Region

#Region "Import"
    Public Function RunImport(ByVal Filename As String, ByVal Email As String, ByVal ImportType As String, ByVal Username As String) As String
        Return _DLayer.RunBranchImport(Filename, Email, ImportType, Username)
    End Function
#End Region
End Class
