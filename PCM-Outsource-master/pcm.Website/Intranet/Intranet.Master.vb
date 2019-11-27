Imports pcm.BusinessLayer
Imports Entities

Public Class Intranet
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'This code checks whether the left pane should be expanded or collapsed
        '======================================================================
        If Request.Browser.Cookies Then
            If Request.Cookies("Sidebar") IsNot Nothing Then
                Dim isCollapsed As String = String.Empty
                isCollapsed = Request.Cookies("Sidebar").Value
                If isCollapsed = "true" Then
                    ASPxSplitter1.GetPaneByName("LeftPane").Collapsed = True
                Else
                    ASPxSplitter1.GetPaneByName("LeftPane").Collapsed = False
                End If
            End If
        End If
        '======================================================================

        'Session("processing_permission_sequence")
        'Session("maintenance_permission_sequence")
        'Session("reporting_permission_sequence")

        'processing
        '============================================================================================================================================

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.StockAllocationUpload, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockAllocationUpload").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockAllocationUpload").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.DispatchStock, 1) = "1" Then
            HeaderMenu.Items.FindByName("DispatchStock").Enabled = True
        Else
            HeaderMenu.Items.FindByName("DispatchStock").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.GRV, 1) = "1" Then
            HeaderMenu.Items.FindByName("GRV").Enabled = True
        Else
            HeaderMenu.Items.FindByName("GRV").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.IBTOUT, 1) = "1" Then
            HeaderMenu.Items.FindByName("IBTOUT").Enabled = True
        Else
            HeaderMenu.Items.FindByName("IBTOUT").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.IBTIN, 1) = "1" Then
            HeaderMenu.Items.FindByName("IBTIN").Enabled = True
        Else
            HeaderMenu.Items.FindByName("IBTIN").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.DeleteIBT, 1) = "1" Then
            HeaderMenu.Items.FindByName("DeleteIBT").Enabled = True
        Else
            HeaderMenu.Items.FindByName("DeleteIBT").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.ManageIBT, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageIBT").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageIBT").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.ReturnToWarehouse, 1) = "1" Then
            HeaderMenu.Items.FindByName("ReturnToWarehouse").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ReturnToWarehouse").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.StockAdjustments, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockAdjustments").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockAdjustments").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.Collection, 1) = "1" Then
            HeaderMenu.Items.FindByName("Collection").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Collection").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.ContactInvestigation, 1) = "1" Then
            HeaderMenu.Items.FindByName("ContactInvestigation").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ContactInvestigation").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.Investigations, 1) = "1" Then
            HeaderMenu.Items.FindByName("Investigations").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Investigations").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.ManualSMSApplication, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManualSMSApplication").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManualSMSApplication").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.PCMTransactions, 1) = "1" Then
            HeaderMenu.Items.FindByName("PCMTransactions").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PCMTransactions").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.PCMJournals, 1) = "1" Then
            HeaderMenu.Items.FindByName("PCMJournals").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PCMJournals").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.PCMCardAllocations, 1) = "1" Then
            HeaderMenu.Items.FindByName("PCMCardAllocations").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PCMCardAllocations").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.HREmployeeReviews, 1) = "1" Then
            HeaderMenu.Items.FindByName("HREmployeeReviews").Enabled = True
        Else
            HeaderMenu.Items.FindByName("HREmployeeReviews").Enabled = False
        End If

        If Mid$(Session("processing_permission_sequence"), Screens.Processing.PCMUploadPayments, 1) = "1" Then
            HeaderMenu.Items.FindByName("PCMUploadPayments").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PCMUploadPayments").Enabled = False
        End If

        '===============================================================================================================================================

        'maintenance
        '===============================================================================================================================================
        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.StockcodeManager, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockcodeManager").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockcodeManager").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.PrePacks, 1) = "1" Then
            HeaderMenu.Items.FindByName("PrePacks").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PrePacks").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.UploadBuyingImages, 1) = "1" Then
            HeaderMenu.Items.FindByName("UploadBuyingImages").Enabled = True
        Else
            HeaderMenu.Items.FindByName("UploadBuyingImages").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageScreensaverImages, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageScreensaverImages").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageScreensaverImages").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageUsers, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageUsers").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageUsers").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageSelfAssistImages, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageSelfAssistImages").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageSelfAssistImages").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.BankRecon, 1) = "1" Then
            HeaderMenu.Items.FindByName("BankRecon").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BankRecon").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.Questionnaire, 1) = "1" Then
            HeaderMenu.Items.FindByName("Questionnaire").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Questionnaire").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageDebtors, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageDebtors").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageDebtors").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManagePositiveUsers, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManagePositiveUsers").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManagePositiveUsers").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageEmployees, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageEmployees").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageEmployees").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.SendMarketingSMS, 1) = "1" Then
            HeaderMenu.Items.FindByName("SendMarketingSMS").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SendMarketingSMS").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.LoyaltyDiscount, 1) = "1" Then
            HeaderMenu.Items.FindByName("LoyaltyDiscounts").Enabled = True
        Else
            HeaderMenu.Items.FindByName("LoyaltyDiscounts").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.BlockStock, 1) = "1" Then
            HeaderMenu.Items.FindByName("BlockStock").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BlockStock").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageBranches, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageBranches").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageBranches").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageTills, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageTills").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageTills").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageTargets, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageTargets").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageTargets").Enabled = False
        End If

        'If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageStationary, 1) = "1" Then
        '    HeaderMenu.Items.FindByName("ManageStationary").Enabled = True
        'Else
        '    HeaderMenu.Items.FindByName("ManageStationary").Enabled = False
        'End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ColourMatrix, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageColourMatrix").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageColourMatrix").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.SizeMatrix, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageSizeMatrix").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageSizeMatrix").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.Specials, 1) = "1" Then
            HeaderMenu.Items.FindByName("Specials").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Specials").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageRegions, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageRegions").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageRegions").Enabled = False
        End If


        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageSuppliers, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageSuppliers").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageSuppliers").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManagePoints, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManagePoints").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManagePoints").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageCategory, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageCategories").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageCategories").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageMunicipalities, 1) = "1" Then
            HeaderMenu.Items.FindByName("ManageMunicipalities").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ManageMunicipalities").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ImportBranches, 1) = "1" Then
            HeaderMenu.Items.FindByName("ImportBranches").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ImportBranches").Enabled = False
        End If

        If Mid$(Session("maintenance_permission_sequence"), Screens.Maintenance.ImportStockQuantity, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockQuantityImport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockQuantityImport").Enabled = False
        End If
        '=================================================================================================================================================================



        'Users
        '====================================================================================================================================================================
        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.Transactions, 1) = "1" Then
            HeaderMenu.Items.FindByName("Transactions").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Transactions").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.Summary, 1) = "1" Then
            HeaderMenu.Items.FindByName("Summary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Summary").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AgingSummary, 1) = "1" Then
            HeaderMenu.Items.FindByName("AgingSummary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AgingSummary").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AgingSummaryReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("AgingSummaryReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AgingSummaryReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CollectionsAdmin, 1) = "1" Then
            HeaderMenu.Items.FindByName("CollectionsAdmin").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CollectionsAdmin").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.SMSSending, 1) = "1" Then
            HeaderMenu.Items.FindByName("SMSSending").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SMSSending").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.PaymentsVSContacts, 1) = "1" Then
            HeaderMenu.Items.FindByName("PaymentsVSContacts").Enabled = True
        Else
            HeaderMenu.Items.FindByName("PaymentsVSContacts").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TransunionLog, 1) = "1" Then
            HeaderMenu.Items.FindByName("TransunionLog").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TransunionLog").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.IncomingSMS, 1) = "1" Then
            HeaderMenu.Items.FindByName("IncomingSMS").Enabled = True
        Else
            HeaderMenu.Items.FindByName("IncomingSMS").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CashCards, 1) = "1" Then
            HeaderMenu.Items.FindByName("CashCards").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CashCards").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.WebServiceLogs, 1) = "1" Then
            HeaderMenu.Items.FindByName("WebServiceLogs").Enabled = True
        Else
            HeaderMenu.Items.FindByName("WebServiceLogs").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.LastUpdateData, 1) = "1" Then
            HeaderMenu.Items.FindByName("LastUpdateData").Enabled = True
        Else
            HeaderMenu.Items.FindByName("LastUpdateData").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.RankersReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("RankersReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("RankersReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BuyingReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("BuyingReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BuyingReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.IncomingRageSMS, 1) = "1" Then
            HeaderMenu.Items.FindByName("IncomingRageSMS").Enabled = True
        Else
            HeaderMenu.Items.FindByName("IncomingRageSMS").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AccountsByStore, 1) = "1" Then
            HeaderMenu.Items.FindByName("AccountsByStore").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AccountsByStore").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AccountsDrive, 1) = "1" Then
            HeaderMenu.Items.FindByName("AccountsDrive").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AccountsDrive").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BadDebtByStore, 1) = "1" Then
            HeaderMenu.Items.FindByName("BadDebtByStore").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BadDebtByStore").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CollectionsByUser, 1) = "1" Then
            HeaderMenu.Items.FindByName("CollectionsByUser").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CollectionsByUser").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TransactionsByStore, 1) = "1" Then
            HeaderMenu.Items.FindByName("TransactionsByStore").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TransactionsByStore").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CashCardSummary, 1) = "1" Then
            HeaderMenu.Items.FindByName("CashCardsOverview").Enabled = True
            HeaderMenu.Items.FindByName("CashCardSummary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CashCardsOverview").Enabled = False
            HeaderMenu.Items.FindByName("CashCardSummary").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.SalesPaymentsPerAccount, 1) = "1" Then
            HeaderMenu.Items.FindByName("SalesPaymentsPerAccount").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SalesPaymentsPerAccount").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.IncomingManualSaleSMS, 1) = "1" Then
            HeaderMenu.Items.FindByName("IncomingManualSaleSMS").Enabled = True
        Else
            HeaderMenu.Items.FindByName("IncomingManualSaleSMS").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BankReconReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("BankReconReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BankReconReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CategoryReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("CategoryReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CategoryReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BestSellers, 1) = "1" Then
            HeaderMenu.Items.FindByName("BestSellers").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BestSellers").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.StockOnHandByBranch, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockOnHandByBranch").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockOnHandByBranch").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.SingleSOH, 1) = "1" Then
            HeaderMenu.Items.FindByName("SingleSOH").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SingleSOH").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.QuestionnaireReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("QuestionnaireReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("QuestionnaireReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AgeAnalysis, 1) = "1" Then
            HeaderMenu.Items.FindByName("AgeAnalysis").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AgeAnalysis").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AgeAnalysisSummary, 1) = "1" Then
            HeaderMenu.Items.FindByName("AgeAnalysisSummary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AgeAnalysisSummary").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.GiftCardDetails, 1) = "1" Then
            HeaderMenu.Items.FindByName("GiftCardDetails").Enabled = True
        Else
            HeaderMenu.Items.FindByName("GiftCardDetails").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TransactionReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("TransactionReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TransactionReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.VintageReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("VintageReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("VintageReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.NewAccountsReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("NewAccountsReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("NewAccountsReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AccountCardSummary, 1) = "1" Then
            HeaderMenu.Items.FindByName("AccountCardSummary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AccountCardSummary").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.DebtorHistory, 1) = "1" Then
            HeaderMenu.Items.FindByName("DebtorHistory").Enabled = True
        Else
            HeaderMenu.Items.FindByName("DebtorHistory").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.HREmployeeReviewReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("HREmployeeReviewReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("HREmployeeReviewReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.MarketingExport, 1) = "1" Then
            HeaderMenu.Items.FindByName("MarketingExport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("MarketingExport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.InterestPayments, 1) = "1" Then
            HeaderMenu.Items.FindByName("InterestPayments").Enabled = True
        Else
            HeaderMenu.Items.FindByName("InterestPayments").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BadDebt, 1) = "1" Then
            HeaderMenu.Items.FindByName("BadDebt").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BadDebt").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.LimitIncreaseReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("LimitIncreaseReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("LimitIncreaseReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.ErrorReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("ErrorReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ErrorReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CurrentNewTurnoverGraph, 1) = "1" Then
            HeaderMenu.Items.FindByName("CurrentTurnoverGraph").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CurrentTurnoverGraph").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AverageAccountSale, 1) = "1" Then
            HeaderMenu.Items.FindByName("AverageAccountSale").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AverageAccountSale").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.GraphSegments, 1) = "1" Then
            HeaderMenu.Items.FindByName("GraphSegments").Enabled = True
        Else
            HeaderMenu.Items.FindByName("GraphSegments").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AccountSales, 1) = "1" Then
            HeaderMenu.Items.FindByName("AccountSales").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AccountSales").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.SMSReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("SMSReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SMSReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AverageCashSale, 1) = "1" Then
            HeaderMenu.Items.FindByName("AverageCashSale").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AverageCashSale").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeDetailsReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("EmployeesDetailsReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("EmployeesDetailsReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.EmployeePerStore, 1) = "1" Then
            HeaderMenu.Items.FindByName("EmployeesPerStore").Enabled = True
        Else
            HeaderMenu.Items.FindByName("EmployeesPerStore").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeClocking, 1) = "1" Then
            HeaderMenu.Items.FindByName("EmployeesClocking").Enabled = True
        Else
            HeaderMenu.Items.FindByName("EmployeesClocking").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeClockingReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("GridEmployeesClocking").Enabled = True
        Else
            HeaderMenu.Items.FindByName("GridEmployeesClocking").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.NewCashUp, 1) = "1" Then
            HeaderMenu.Items.FindByName("NewCashUp").Enabled = True
        Else
            HeaderMenu.Items.FindByName("NewCashUp").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CashDiscrepancy, 1) = "1" Then
            HeaderMenu.Items.FindByName("CashDiscrepancy").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CashDiscrepancy").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CashTransactionsReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("CashTransactionsReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CashTransactionsReport").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CreditCardAuth, 1) = "1" Then
            HeaderMenu.Items.FindByName("CreditCardAuth").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CreditCardAuth").Enabled = False
        End If

        'If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.CashReports, 1) = "1" Then
        '    HeaderMenu.Items.FindByName("CashReports").Enabled = True
        'Else
        '    HeaderMenu.Items.FindByName("CashReports").Enabled = False
        'End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.HistoricalClockingReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("HistoricalClockingReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("HistoricalClockingReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.StockTransactionReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockTransaction").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockTransaction").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.AccountsDriveReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("AccountsDriveReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("AccountsDriveReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TaskReports, 1) = "1" Then
            HeaderMenu.Items.FindByName("TasksReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TasksReport").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.Reprints, 1) = "1" Then
            HeaderMenu.Items.FindByName("Reprints").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Reprints").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.DeletedIBT, 1) = "1" Then
            HeaderMenu.Items.FindByName("DeletedIBTs").Enabled = True
        Else
            HeaderMenu.Items.FindByName("DeletedIBTs").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.BranchReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("Branches").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Branches").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.HREmployeeReviewReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("EmployeesHistoryReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("EmployeesHistoryReport").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.ReprintDispatchedStock, 1) = "1" Then
            HeaderMenu.Items.FindByName("ReprintDispatchedStock").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ReprintDispatchedStock").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.SellOffPerItem, 1) = "1" Then
            HeaderMenu.Items.FindByName("SellOffPerItem").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SellOffPerItem").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.CategorySummaryReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("CategorySummary").Enabled = True
        Else
            HeaderMenu.Items.FindByName("CategorySummary").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.ColourGridReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("ColourGrids").Enabled = True
        Else
            HeaderMenu.Items.FindByName("ColourGrids").Enabled = False
        End If

        If Mid(Session("reporting_permission_sequence"), Screens.Reporting.SizeGridReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("SizeGridReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SizeGridReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.BestSellersReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("BestSellersReport").Enabled = True
        Else
            HeaderMenu.Items.FindByName("BestSellersReport").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TotalStockOnHand, 1) = "1" Then
            HeaderMenu.Items.FindByName("TotalStockOnHand").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TotalStockOnHand").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.TotalStockByBranch, 1) = "1" Then
            HeaderMenu.Items.FindByName("TotalStockByBranch").Enabled = True
        Else
            HeaderMenu.Items.FindByName("TotalStockByBranch").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.StockHistory, 1) = "1" Then
            HeaderMenu.Items.FindByName("StockHistory").Enabled = True
        Else
            HeaderMenu.Items.FindByName("StockHistory").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.VoidsAndNoSales, 1) = "1" Then
            HeaderMenu.Items.FindByName("VoidsAndNoSales").Enabled = True
        Else
            HeaderMenu.Items.FindByName("VoidsAndNoSales").Enabled = False
        End If


        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.Discounts, 1) = "1" Then
            HeaderMenu.Items.FindByName("Discount").Enabled = True
        Else
            HeaderMenu.Items.FindByName("Discount").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.SOHReport, 1) = "1" Then
            HeaderMenu.Items.FindByName("SOH").Enabled = True
        Else
            HeaderMenu.Items.FindByName("SOH").Enabled = False
        End If

        If Mid$(Session("reporting_permission_sequence"), Screens.Reporting.WebServiceWebLogs, 1) = "1" Then
            HeaderMenu.Items.FindByName("WebServiceWebLogs").Enabled = True
        Else
            HeaderMenu.Items.FindByName("WebServiceWebLogs").Enabled = False
        End If
        '==================================================================================================================================================
    End Sub

    Protected Sub HeaderMenu_ItemClick(source As Object, e As DevExpress.Web.MenuItemEventArgs) Handles HeaderMenu.ItemClick
        Select Case e.Item.Name
            Case "Collections"
                'Response.Redirect("hud.aspx")


            Case "Logout"
                If (Request.Cookies("PCMLOGIN") IsNot Nothing) Then
                    '  'Expire the login cookie
                    Response.Cookies("PCMLOGIN").Expires = DateTime.Now.AddDays(-30)
                End If

                Dim sIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")


                Dim _dlLog As New LoggingBusinessLayer


                'Create the login record
                Dim _NewLog As New CollectionCallLog

                _NewLog.AccountNumber = ""
                _NewLog.ActionResult = "Success"
                _NewLog.ActionType = "User Logout"
                _NewLog.IPAddress = sIPAddress
                _NewLog.LengthOfAction = "0"
                _NewLog.PTPAmount = "0"
                _NewLog.UserComment = ""
                _NewLog.UserName = Session("username")
                _NewLog.PTPDate = ""

                _dlLog.InsertUserLogRecord(_NewLog)

                Session("username") = ""
                Session("user_permissions") = ""
                Session("user_is_administrator") = ""

                Response.Redirect("~/Intranet/Default.aspx")

            Case "ViewReports"
                Response.Redirect("~/Reports/View/ViewReports.aspx")
        End Select


    End Sub
End Class