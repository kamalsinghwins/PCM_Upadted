Imports pcm.DataLayer
Imports Entities
Imports Entities.Reports
Imports System.Web.Script.Serialization

Public Class ReportsBusinessLayer
    Dim _DLayer As ReportsDataLayer
    Dim NewReport As New DataTable

    Public Sub New(ByVal CompanyCode As String)
        _DLayer = New ReportsDataLayer(CompanyCode)
    End Sub

    Public Sub New()
        _DLayer = New ReportsDataLayer()
    End Sub

    Public Function ReturnWebLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        NewReport = _DLayer.ReturnBureauLog(StartDate, EndDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetSalesPayments(ByVal OpenedFrom As String, ByVal OpenedTo As String,
                                     ByVal SalesFrom As String, ByVal SalesTo As String,
                                     ByVal PaymentsFrom As String, ByVal PaymentsTo As String) As DataTable

        NewReport = _DLayer.GetSalesPayments(OpenedFrom, OpenedTo, SalesFrom, SalesTo, PaymentsFrom, PaymentsTo)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If
    End Function

    Public Function ReturnAgingSummary(ByVal MinimumAmount As String) As DataTable

        NewReport = _DLayer.GetAgingSummary(MinimumAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetInternalPeriods() As DataTable

        NewReport = _DLayer.GetInternalPeriods()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetBadDebtData(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetBadDebtData(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetBadDebtRecoveredData(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetBadDebtRecoveredData(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetReductionIn150(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetReductionIn150(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function


    Public Function BadDebtByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        NewReport = _DLayer.BadDebtByStoreTransactions(StartDate, EndDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function BadDebtByStoreMaster(ByVal StartDate As String, ByVal EndDate As String,
                                     ByVal Period As String, ByVal BadDebtAmount As Double) As DataTable

        NewReport = _DLayer.BadDebtByStoreMaster(StartDate, EndDate, Period, BadDebtAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function


    Public Function GetAccountsToBeCalled(ByVal MinimumAmount As String) As DataTable

        NewReport = _DLayer.GetAccountsToBeCalled(MinimumAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetAccountsSections() As DataTable

        NewReport = _DLayer.GetAccountsSections()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetAccountsSectionsActiveBalance() As DataTable

        NewReport = _DLayer.GetAccountsSectionsActiveBalance()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function



    Public Function ReturnIncomingSMS(ByVal StartDate As String, ByVal ToDate As String) As DataTable

        NewReport = _DLayer.GetIncomingSMS(StartDate, ToDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnLastUpdateData() As DataTable

        NewReport = _DLayer.GetLastUpdateData()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnAdminReportData(ByVal Status As String, ByVal PTPDate As String) As DataTable

        If Status = "PTP" Then
            NewReport = _DLayer.ReturnPTPListing(PTPDate)
        Else
            NewReport = _DLayer.ReturnInvestigationListing(Status)
        End If

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnSMSReportData(ByVal SMSType As String, ByVal StartDate As String, ByVal EndDate As String) As DataTable
        Return _DLayer.ReturnSMSReportData(SMSType, StartDate, EndDate)
    End Function

    Public Function ReturnPaymentsVSContactsReportData(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        NewReport = _DLayer.ReturnPaymentsVSContactsReportData(FromDate, ToDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetLimitIncreaseReport(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetLimitIncreaseReport(StartDate, EndDate)

    End Function
    Public Function GetErrors(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetErrors(StartDate, EndDate)

    End Function

    Public Function GetCurrentTurnover(ByVal OrderBy As String) As CurrentTurnoverResponse
        Return _DLayer.GetCurrentTurnover(OrderBy)
    End Function

    Public Function GenerateData(ByVal OrderBy As String, ByVal StartDate As String, ByVal EndDate As String) As CurrentTurnoverResponse
        Return _DLayer.GenerateData(OrderBy, StartDate, EndDate)
    End Function

    Public Function GetTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse
        Return _DLayer.GetTransactions(StartDate, EndDate)
    End Function
    Public Function GetBranchDetails() As DataTable
        Return _DLayer.GetBranchDetails()
    End Function

    Public Function GetSegments(StartDate As String, EndDate As String, DateRange As String, Segment As String, AllBranches As Boolean, Optional ByVal strBranch As String = "") As GetSegmentsResponse
        Return _DLayer.GetSegments(StartDate, EndDate, DateRange, Segment, AllBranches, strBranch)
    End Function

    Public Sub InsertUserRecord(ByVal Username As String, ByVal isManager As String,
                                ByVal IPAddress As String, ByVal TypeOfAction As String,
                                ByVal ReportName As String, ByVal ReportOptions As String)
        _DLayer.InsertUserRecord(Username, isManager, IPAddress, TypeOfAction, ReportName, ReportOptions)
    End Sub

    Public Function GetSentEmployeeDetails() As DataTable

        Return _DLayer.GetSentEmployeeDetails()

    End Function

    Public Function GetSentSMSLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetSentSMSLog(StartDate, EndDate)

    End Function

    Public Function GetBestSellersReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Trim As String) As DataSet

        Return _DLayer.GetBestSellersReport(StartDate, EndDate, Trim)

    End Function

    Public Function GetCashTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse
        Return _DLayer.GetCashTransactions(StartDate, EndDate)
    End Function

    Public Function GetAccountSales(StartDate As String, EndDate As String) As GetAccountSalesResponse
        Return _DLayer.GetAccountSales(StartDate, EndDate)
    End Function

    Public Function GetMasterEmployeesPerStore(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Return _DLayer.GetMasterEmployeesPerStore(Permanent, Casual, All, StartDate, EndDate)

    End Function

    Public Function GetClockingEmployees(ByVal EmployeeNumber As String) As DataTable
        Return _DLayer.GetClockingEmployees(EmployeeNumber)
    End Function

    Public Function GetEmployeesBySearch(ByVal Employee As String) As DataTable
        Return _DLayer.GetEmployeesBySearch(Employee)
    End Function

    Public Function GetGridClockingEmployees(ByVal StartDate As String, ByVal EndDate As String, ByVal Employee As String) As DataSet
        Return _DLayer.GetGridClockingEmployees(StartDate, EndDate, Employee)
    End Function

    Public Function GetBranches() As DataSet
        Return _DLayer.GetBranches()
    End Function

    Public Function GetBranches(ByVal All As Boolean) As DataSet
        Return _DLayer.GetBranches(True)
    End Function
    Public Function RunEmployeeReport(ByVal EMailAddresses As String,
                                     ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String

        Return _DLayer.RunEmployeeReport(EMailAddresses, StartDate, EndDate, json, Username)

    End Function

    Public Function RunCashSummaryReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Json As String, ByVal Email As String, ByVal Username As String) As String
        Return _DLayer.RunCashSummaryReport(StartDate, EndDate, Json, Email, Username)

    End Function

    Public Function RunBadDebtReport(ByVal json As String, ByVal Email As String, ByVal Username As String) As String
        Return _DLayer.RunBadDebtReport(json, Email, Username)

    End Function

    Public Function RunCashDiscrepancyReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Json As String, ByVal Email As String, ByVal Username As String) As String
        Return _DLayer.RunCashDiscrepancyReport(StartDate, EndDate, Json, Email, Username)

    End Function
    Public Function RunCashTransactionsReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As String
        Return _DLayer.RunCashTransactionsReport(StartDate, EndDate, Email, Username, json)

    End Function

    Public Function RunAccountOpenedByEmployee(ByVal StartDate As String, ByVal EndDate As String, ByVal Email As String, ByVal Username As String, ByVal json As String) As String
        Return _DLayer.RunAccountOpenedByEmployee(StartDate, EndDate, Email, Username, json)

    End Function
    Public Function GetHistoricalClockingReport(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Return _DLayer.GetHistoricalClockingReport(Permanent, Casual, All, StartDate, EndDate)

    End Function

    Public Function GetCategories(ByVal CategoryNumber As Integer) As DataSet
        Return _DLayer.GetCategories(CategoryNumber)
    End Function
    Public Function GetStockCodes(ByVal Stockcode As String) As DataTable
        Return _DLayer.GetStockCodes(Stockcode)
    End Function

    Public Function RunStockTransactionsReport(ByVal EMailAddresses As String,
                                     ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String

        Return _DLayer.RunStockTransactionsReport(EMailAddresses, StartDate, EndDate, json, Username)

    End Function

    Public Function RunBestSellersReport(ByVal EMailAddresses As String,
                                   ByVal StartDate As String, ByVal EndDate As String, ByVal json As String, ByVal Username As String) As String

        Return _DLayer.RunBestSellersReport(EMailAddresses, StartDate, EndDate, json, Username)

    End Function

    Public Function BlockStock(ByVal Branch As String, ByVal NoStockUntil As String)
        Return _DLayer.BlockStock(Branch, NoStockUntil)
    End Function

    Public Function GetReports(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetReports(StartDate, EndDate)

    End Function

    Public Function GetReprints(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetReprints(StartDate, EndDate)

    End Function

    Public Function DeletedIBTs(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.DeletedIBTs(StartDate, EndDate)

    End Function

    Public Function GetCreditCardAuth(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetCreditCardAuth(StartDate, EndDate)

    End Function

    Public Function GetTillNumber(ByVal BranchCode As String) As DataTable

        Return _DLayer.GetTillNumber(BranchCode)

    End Function

    Public Function GetTotal(ByVal TillNumber As String, ByVal TransactionType As String, ByVal Column As String, ByVal SummaryDate As String) As String

        Return _DLayer.GetTotal(TillNumber, TransactionType, Column, SummaryDate)

    End Function

    Public Function GetCashTransaction(ByVal TillNumber As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Return _DLayer.GetCashTransaction(TillNumber, FromDate, ToDate)

    End Function
    Public Function GetCashTransactionByType(ByVal TillNumber As String, ByVal FromDate As String, ByVal ToDate As String, ByVal TransactionType As String) As DataTable

        Return _DLayer.GetCashTransactionByType(TillNumber, FromDate, ToDate, TransactionType)

    End Function
    Public Function GetContactLevels()
        Return _DLayer.GetContactLevels()
    End Function
    Public Function GetReprintDocuments(ByVal StartDate As String, ByVal EndDate As String, ByVal DispatchNumber As String)
        Return _DLayer.GetReprintDocuments(StartDate, EndDate, DispatchNumber)
    End Function
    Public Function GetDispatchDocuments(ByVal DispatchNumber As String) As DataTable
        Return _DLayer.GetDispatchDocuments(DispatchNumber)
    End Function
    Public Function GetSellOffPerItems(ByVal StartDate As String, ByVal EndDate As String, ByVal StockCode As String) As DataTable
        Return _DLayer.GetSellOffPerItems(StartDate, EndDate, StockCode)
    End Function
    Public Function SearchStockcodes(ByVal SearchText As String) As DataTable
        Return _DLayer.SearchStockcodes(SearchText)
    End Function
    Public Function GetCategory(ByVal CategoryNumber As String) As DataTable
        Return _DLayer.GetCategory(CategoryNumber)
    End Function
    Public Function RunCategorySummaryReport(ByVal StartDate As String, ByVal EndDate As String, ByVal EMailAddresses As String, ByVal Username As String, ByVal json As String) As String
        Return _DLayer.RunCategorySummaryReport(StartDate, EndDate, EMailAddresses, Username, json)
    End Function
    Public Function GetAllStockCodes(ByVal SearchText As String, ByVal MasterCode As Boolean) As SearchResponse
        Return _DLayer.GetAllStockCodes(SearchText, MasterCode)
    End Function
    Public Function RunColourGridsReport(ByVal colourGridRequest As ColourGridRequest) As BaseResponse
        Dim json As String = String.Empty
        Dim request As New json
        Dim serializer As New JavaScriptSerializer()
        request.FromStockCode = colourGridRequest.FromStockCode
        request.ToStockCode = colourGridRequest.ToStockCode
        request.ToStockCode = colourGridRequest.ToStockCode
        request.AllBranches = colourGridRequest.AllBranches
        request.PartialList = colourGridRequest.PartialList
        request.BranchList = colourGridRequest.BranchList
        request.IsAdmin = colourGridRequest.IsAdmin
        request.IPAddress = colourGridRequest.IPAddress
        request.Type = colourGridRequest.Type
        json = serializer.Serialize(request)

        Return _DLayer.RunColourGridsReport(colourGridRequest.StartDate, colourGridRequest.EndDate, colourGridRequest.Email, colourGridRequest.Username, json)
    End Function
    Public Function GetALLBranches() As DataTable
        Return _DLayer.GetALLBranches()
    End Function
    Public Function GetGridSize() As DataTable
        Return _DLayer.GetGridSize()
    End Function
    Public Function RunGridSizeReport(ByVal StartDate As String, ByVal EndDate As String, ByVal EMailAddresses As String, ByVal Username As String, ByVal json As String) As String
        Return _DLayer.RunGridSizeReport(StartDate, EndDate, EMailAddresses, Username, json)
    End Function
    Public Function RunAgingSummaryReport(ByVal json As String, ByVal Email As String, ByVal Username As String) As String
        Return _DLayer.RunAgingSummaryReport(json, Email, Username)
    End Function
    Public Function GetMasterBestSellers(ByVal StartDate As String, ByVal EndDate As String, ByVal MasterCode As Boolean) As DataTable
        Return _DLayer.GetMasterBestSellers(StartDate, EndDate, MasterCode)
    End Function
    Public Function GetDetailBestSellers(ByVal StartDate As String, ByVal EndDate As String, ByVal MasterCode As Boolean) As DataTable
        Return _DLayer.GetDetailBestSellers(StartDate, EndDate, MasterCode)
    End Function
    Public Function GetTotalStockOnHand() As DataSet
        Return _DLayer.GetTotalStockOnHand()
    End Function
    Public Function GetTotalStockByBranch(ByVal BranchCode As String) As DataTable
        Return _DLayer.GetTotalStockByBranch(BranchCode)
    End Function
    Public Function GetBranchStockHistory(ByVal StartDate As String) As DataTable

        Return _DLayer.GetBranchStockHistory(StartDate)

    End Function
    Public Function GetVoidsAndNoSalesReports(ByVal StartDate As String, ByVal EndDate As String, ByVal BranchList As String) As DataTable
        Return _DLayer.GetVoidsAndNoSalesReports(StartDate, EndDate, BranchList)
    End Function
    Public Function GetDiscountReports(ByVal StartDate As String, ByVal EndDate As String, ByVal BranchList As String) As DataTable
        Return _DLayer.GetDiscountReports(StartDate, EndDate, BranchList)
    End Function
    Public Function GetStockcodesList(ByVal StockCode As String, ByVal MasterCode As Boolean) As DataTable
        Return _DLayer.GetStockcodesList(StockCode, MasterCode)
    End Function
    Public Function RunSOHReport(ByVal Email As String, ByVal Username As String, ByVal json As String) As String
        Return _DLayer.RunSOHReport(Email, Username, json)
    End Function
    Public Function GetWebServiceWebLogs(ByVal StartDate As String, ByVal EndDate As String) As DataTable
        Return _DLayer.GetWebServiceWebLogs(StartDate, EndDate)
    End Function
End Class

