Imports DevExpress.Web
Imports DevExpress.Web.ASPxTreeList
Imports pcm.BusinessLayer
Imports Entities



Public Class ManageUsers
    Inherits System.Web.UI.Page
    Private Sub ManageUsers_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim url As String = Request.Url.AbsoluteUri

        If HttpContext.Current.IsDebuggingEnabled Then
            Session("current_company") = "010"
        End If

        'Me.Form.DefaultButton = cmdAccept.UniqueID

        Page.Server.ScriptTimeout = 300

        If (Not IsPostBack) Then
            'Populate size grid
            Dim _BLayerStock As New UsersBL()
            Dim _dt As DataTable

            _dt = _BLayerStock.GetUsers

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboUsers.Items.Add(_dt(i)("username"))
            Next

        End If
        CreateNodes()


    End Sub
    Protected Sub treeList_CustomDataCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs)
        e.Result = treeListAll.SelectionCount.ToString()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageUsers) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If



    End Sub
    Private Function CreateNodeCore(ByVal key As Object, ByVal iconName As String, ByVal text As String, ByVal parentNode As TreeListNode) As TreeListNode
        Dim node As TreeListNode = treeListAll.AppendNode(key, parentNode)
        node("IconName") = iconName
        node("Name") = text
        Return node
    End Function
    Protected Function GetIconUrl(ByVal container As TreeListDataCellTemplateContainer) As String
        'Return "~/Images/blue_pixel.png"
        Return "~/Images/faq.png"
    End Function
    'Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click

    '    For i As Integer = 0 To treeList.Nodes.Count - 1

    '        MsgBox(treeList.Nodes(i).Selected & " " & treeList.Nodes(i).Key)
    '        If treeList.Nodes(i).HasChildren Then
    '            For x As Integer = 0 To treeList.Nodes(i).ChildNodes.Count - 1
    '                MsgBox(treeList.Nodes(i).ChildNodes(x).Selected & " " & treeList.Nodes(i).ChildNodes(x).Key)
    '            Next x
    '        End If

    '    Next

    'End Sub
    Private Sub ManageUsers_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Function TrueFalse(ByVal tmpTrueFalse As Boolean) As String

        If tmpTrueFalse = True Then
            Return "1"
        Else
            Return "0"
        End If

    End Function
    Protected Sub ASPxCallback1_Callback(sender As Object, e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

        Dim _BLayer As New UsersBL()
        Dim _dt As New DataTable

        If hdWhichButton.Value = "cboUsers" Then
            Try
                _dt = _BLayer.GetUserDetails(cboUsers.Text)
            Catch ex As Exception
                lblError.Text = "There was an error updating the User file."

                dxPopUpError.ShowOnPageLoad = True
            End Try


            If _dt.Rows.Count > 0 Then
                'txtPassword.Text = _dt.Rows(0)("userpass")
                txtEmail.Text = _dt.Rows(0)("email_address").ToString



                If _dt.Rows(0)("ustatus") = "ACTIVE" Then
                    chkActive.Checked = True
                Else
                    chkActive.Checked = False
                End If

                If _dt.Rows(0)("pcm_admin") = True Then
                    chkPCMAdmin.Checked = True
                Else
                    chkPCMAdmin.Checked = False
                End If

                Dim permission_string As String

                'clear treelist first
                '==========================================================================
                For i As Integer = 0 To treeListAll.Nodes.Count - 1

                    treeListAll.Nodes(i).Selected = False
                    If treeListAll.Nodes(i).HasChildren Then
                        For x As Integer = 0 To treeListAll.Nodes(i).ChildNodes.Count - 1
                            treeListAll.Nodes(i).ChildNodes(x).Selected = False
                        Next x
                    End If

                Next
                '=========================================================================

                'processing_sequence
                '==========================================================================
                permission_string = _dt.Rows(0)("processing_sequence") & ""

                If Mid$(permission_string, Screens.Processing.StockAllocationUpload, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_stock_allocation").Selected = True
                End If

                If Mid$(permission_string, Screens.Processing.DispatchStock, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_dispatch_stock").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.IBTOUT, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_ibt_out").Selected = True
                End If

                If Mid$(permission_string, Screens.Processing.IBTIN, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_ibt_in").Selected = True
                End If

                If Mid$(permission_string, Screens.Processing.DeleteIBT, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_delete_ibt").Selected = True
                End If


                If Mid$(permission_string, Screens.Processing.ManageIBT, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_manage_ibt").Selected = True
                End If

                If Mid$(permission_string, Screens.Processing.IBTOUT, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_stock_allocation").Selected = True
                End If

                If Mid$(permission_string, Screens.Processing.Collection, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_collections").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.ContactInvestigation, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_contact_investigation").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.Investigations, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_investigation").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.ManualSMSApplication, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_sms").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.PCMTransactions, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_pcm_transactions").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.PCMJournals, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_pcm_journals").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.PCMCardAllocations, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_card_allocations").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.HREmployeeReviews, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_employee_reviews").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.PCMUploadPayments, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_upload_payments").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.StockAdjustments, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_stock_adjustments").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.GRV, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_grv").Selected() = True
                End If

                If Mid$(permission_string, Screens.Processing.ReturnToWarehouse, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("processing_return_to_warehouse").Selected() = True

                End If

                '=====================================================================================


                'maintenance_sequence
                '=====================================================================================
                permission_string = _dt.Rows(0)("maintenance_sequence") & ""

                If Mid$(permission_string, Screens.Maintenance.StockcodeManager, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_stockcode_manager").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.PrePacks, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_pre_packs").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageScreensaverImages, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_screen_saver_images").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.UploadBuyingImages, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_buying_images").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageUsers, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_user_permissions").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageSelfAssistImages, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_self_assist_images").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.BankRecon, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_bank_recon").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.Questionnaire, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_create_questionnaire").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageDebtors, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_debtors").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManagePositiveUsers, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_positive_users").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageEmployees, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_employees").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.SendMarketingSMS, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_send_marketing_sms").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.LoyaltyDiscount, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_loyalty_discounts").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.BlockStock, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_block_stock").Selected() = True
                End If


                If Mid$(permission_string, Screens.Maintenance.ManageBranches, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_branches").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageTills, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_tills").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageTargets, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_targets").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ColourMatrix, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_colour_matrix").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.SizeMatrix, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_size_matrix").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.Specials, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_specials").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageRegions, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_regions").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageSuppliers, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_suppliers").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManagePoints, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_points").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageCategory, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_categories").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ManageMunicipalities, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_manage_municipalities").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ImportBranches, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_import_branches").Selected() = True
                End If

                If Mid$(permission_string, Screens.Maintenance.ImportStockQuantity, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("maintenance_import_stock_quantity").Selected() = True
                End If

                '=============================================================================================

                'reporting_sequence
                '=============================================================================================
                permission_string = _dt.Rows(0)("reporting_sequence") & ""

                If Mid$(permission_string, Screens.Reporting.Transactions, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_user_transactions").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.Summary, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_user_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AgingSummary, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_aging").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CollectionsAdmin, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_collections_admin").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SMSSending, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_sms_sending").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.PaymentsVSContacts, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_payments_vs_contacts").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CashCards, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_cash_card_transactions").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.WebServiceLogs, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_positive_cash_cards_web_service_logs").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.LastUpdateData, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_last_update_data").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.RankersReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_rankers_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BuyingReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_buying_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TransunionLog, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_transunion_log").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.IncomingSMS, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_incoming_sms").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.IncomingRageSMS, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_incoming_rage_sms").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AccountsByStore, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_accounts_opened_by_store").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AccountsDrive, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_accounts_drive").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BadDebtByStore, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_bad_debt_by_store").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CollectionsByUser, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_collections_by_user").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TransactionsByStore, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_transactions_by_store").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CashCardSummary, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_cash_card_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SalesPaymentsPerAccount, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_sales_payments_per_account").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.IncomingManualSaleSMS, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_incoming_manual_sale_sms").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BankReconReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_bank_recon").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CategoryReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_category_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SingleSOH, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_single_stock_on_hand_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.QuestionnaireReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_questionnaire_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AgeAnalysis, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_age_analysis_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AgeAnalysisSummary, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_age_analysis_summary_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.GiftCardDetails, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_gift_card_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TransactionReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_transaction_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.VintageReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_vintage_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.NewAccountsReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_new_accounts").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AccountCardSummary, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_account_card_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.DebtorHistory, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_debtor_history").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.HREmployeeReviewReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_employee_reviews").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.MarketingExport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_marketing_export").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SMSReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_sms_error_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.InterestPayments, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_interest_payments").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BadDebt, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_bad_debt").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.LimitIncreaseReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_limitincreasereport_report").Selected() = True
                End If


                If Mid$(permission_string, Screens.Reporting.ErrorReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_error_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CurrentNewTurnoverGraph, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_current_turnover_graph_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AverageAccountSale, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_average_account_sale").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.GraphSegments, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_graph_segments").Selected() = True
                End If


                If Mid$(permission_string, Screens.Reporting.AverageCashSale, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_average_cash_sale").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.EmployeeDetailsReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_employee_details_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AccountSales, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_account_sales_report").Selected() = True
                End If


                If Mid$(permission_string, Screens.Reporting.EmployeePerStore, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_employee_per_store_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.EmployeeClocking, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_employee_clocking_report").Selected() = True
                End If


                If Mid$(permission_string, Screens.Reporting.EmployeeClockingReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_grdemployee_clocking_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.NewCashUp, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_new_cashup_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CashDiscrepancy, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_cash_discrepancy_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CashTransactionsReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_cash_transactions_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.HistoricalClockingReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_historical_clocking_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.StockTransactionReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_stock_transactions_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BestSellers, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_best_sellers_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AccountsDriveReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_accounts_drive_B").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TaskReports, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_tasks_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.Reprints, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_reprints").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.DeletedIBT, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_deleted_ibt_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CreditCardAuth, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_credit_card_auth_report").Selected() = True
                End If

                'If Mid$(permission_string, Screens.Reporting.CashReports, 1) = "1" Then
                '    treeListAll.FindNodeByKeyValue("reporting_cash_report").Selected() = True
                'End If

                If Mid$(permission_string, Screens.Reporting.BranchReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_branch_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.ReprintDispatchedStock, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_reprint_dispatched_stock").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SellOffPerItem, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_sell_off_per_item").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.CategorySummaryReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_category_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.ColourGridReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_colour_grids_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SizeGridReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_size_grids").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.StockOnHandByBranch, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_stock_on_hand_by_branch").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.AgingSummaryReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_aging_summary").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.BestSellersReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_best_sellers").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TotalStockOnHand, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_total_stock_on_hand_report").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.TotalStockByBranch, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_total_stock_by_branch").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.StockHistory, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_stock_history").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.VoidsAndNoSales, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_voids_and_no_sales").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.Discounts, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_discounts").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.SOHReport, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_soh").Selected() = True
                End If

                If Mid$(permission_string, Screens.Reporting.WebServiceWebLogs, 1) = "1" Then
                    treeListAll.FindNodeByKeyValue("reporting_web_service_web_logs").Selected() = True
                End If

            End If

        End If

        If hdWhichButton.Value = "Save" Then

            Dim processing_string As String = getprocessing()
            Dim maintenance_string As String = getmaintenance()
            Dim reporting_string As String = getreporting()

            If Not _BLayer.UpdateUserFile(cboUsers.Text, txtPassword.Text, txtEmail.Text, chkActive.Checked, processing_string,
                                          maintenance_string, reporting_string, chkPCMAdmin.Checked) Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "There was an error updating the User file."
            End If

            ClearScreen()
            dxPopUpError.HeaderText = "Success"
            dxPopUpError.ShowOnPageLoad = True

        End If

        'Clear
        If hdWhichButton.Value = "Clear" Then
            ClearScreen()
        End If

    End Sub
    Private Sub CreateNodes()

        Dim Processing As TreeListNode = CreateNodeCore("processing_node", "Features", "<b>Processing</b>", Nothing)

        Dim pCollections As TreeListNode = CreateNodeCore("processing_collections_node", "Features", "Collections", Processing)
        CreateNodeCore("processing_collections", "Features", "Collections", pCollections)
        CreateNodeCore("processing_contact_investigation", "Features", "Contact Investigation", pCollections)
        CreateNodeCore("processing_investigation", "Features", "Investigation", pCollections)

        Dim pHR As TreeListNode = CreateNodeCore("processing_hr_node", "Features", "HR", Processing)
        CreateNodeCore("processing_employee_reviews", "Features", "Employee Reviews", pHR)

        Dim pPCM As TreeListNode = CreateNodeCore("processing_pcm_node", "Features", "PCM", Processing)
        CreateNodeCore("processing_card_allocations", "Features", "Card Allocations", pPCM)
        CreateNodeCore("processing_sms", "Features", "Manual SMS Application", pPCM)
        CreateNodeCore("processing_pcm_journals", "Features", "Process Journals", pPCM)
        CreateNodeCore("processing_pcm_transactions", "Features", "Process Transactions", pPCM)
        CreateNodeCore("processing_upload_payments", "Features", "Upload Payments", pPCM)

        Dim pPositive As TreeListNode = CreateNodeCore("processing_positive_node", "Features", "Positive", Processing)
        CreateNodeCore("processing_stock_allocation", "Features", "Stock Allocation Upload", pPositive)
        CreateNodeCore("processing_ibt_out", "Features", "IBT OUT", pPositive)
        CreateNodeCore("processing_ibt_in", "Features", "IBT IN", pPositive)
        CreateNodeCore("processing_delete_ibt", "Features", " Delete IBT ", pPositive)
        CreateNodeCore("processing_manage_ibt", "Features", " Manage IBT ", pPositive)
        CreateNodeCore("processing_dispatch_stock", "Features", "Dispatch Stock", pPositive)
        CreateNodeCore("processing_stock_adjustments", "Features", "Stock Adjustments", pPositive)
        CreateNodeCore("processing_grv", "Features", "GRV", pPositive)
        CreateNodeCore("processing_return_to_warehouse", "Features", "Return To Warehouse", pPositive)


        'Maintenance
        Dim Maintenance As TreeListNode = CreateNodeCore("maintenance_node", "Folder", "<b>Maintenance</b>", Nothing)

        Dim mPositive As TreeListNode = CreateNodeCore("maintenance_positive_node", "Folder", "Positive", Maintenance)

        CreateNodeCore("maintenance_bank_recon", "Features", "Bank Recon", Maintenance)
        CreateNodeCore("maintenance_buying_images", "Features", "Buying Images", mPositive)
        CreateNodeCore("maintenance_pre_packs", "Features", "Pre Packs", mPositive)
        CreateNodeCore("maintenance_employees", "Features", "Manage Employees", mPositive)
        CreateNodeCore("maintenance_positive_users", "Features", "Manage HO / Shop Users", mPositive)
        CreateNodeCore("maintenance_create_questionnaire", "Features", "Manage Questionnaires", mPositive)
        CreateNodeCore("maintenance_screen_saver_images", "Features", "Screen Saver Images", mPositive)
        CreateNodeCore("maintenance_self_assist_images", "Features", "Self Assist Images", mPositive)
        CreateNodeCore("maintenance_stockcode_manager", "Features", "Stockcode Manager", mPositive)
        CreateNodeCore("maintenance_loyalty_discounts", "Features", "Loyalty Discounts", mPositive)
        CreateNodeCore("maintenance_block_stock", "Features", "Block Stock", mPositive)
        CreateNodeCore("maintenance_manage_branches", "Features", "Manage Branches", mPositive)
        CreateNodeCore("maintenance_manage_tills", "Features", "Manage Tills", mPositive)
        CreateNodeCore("maintenance_manage_targets", "Features", "Manage Targets", mPositive)
        'CreateNodeCore("maintenance_manage_stationary", "Features", "Manage Stationary", mPositive)
        CreateNodeCore("maintenance_manage_colour_matrix", "Features", "Manage Colour Matrix", mPositive)
        CreateNodeCore("maintenance_manage_size_matrix", "Features", "Manage Size Matrix", mPositive)
        CreateNodeCore("maintenance_manage_specials", "Features", "Specials", mPositive)
        CreateNodeCore("maintenance_manage_regions", "Features", "Manage Regions", mPositive)
        CreateNodeCore("maintenance_manage_suppliers", "Features", "Manage Suppliers", mPositive)
        CreateNodeCore("maintenance_manage_points", "Features", "Manage Points", mPositive)
        CreateNodeCore("maintenance_manage_categories", "Features", "Manage Categories", mPositive)
        CreateNodeCore("maintenance_manage_municipalities", "Features", "Manage Municipalities", mPositive)
        CreateNodeCore("maintenance_import_branches", "Features", "Import Branch", mPositive)
        CreateNodeCore("maintenance_import_stock_quantity", "Features", "Stock Quantity Import", mPositive)

        Dim mPCM As TreeListNode = CreateNodeCore("maintenance_pcm_node", "Folder", "PCM", Maintenance)
        CreateNodeCore("maintenance_manage_debtors", "Features", "Manage Debtors", mPCM)

        CreateNodeCore("maintenance_user_permissions", "Features", "User Permissions", Maintenance)
        CreateNodeCore("maintenance_send_marketing_sms", "Features", "Send Marketing SMS", Maintenance)

        'Reporting
        Dim Reporting As TreeListNode = CreateNodeCore("reporting_node", "Folder", "<b>Reporting</b>", Nothing)

        Dim rCollections As TreeListNode = CreateNodeCore("reporting_collections_node", "Folder", "Collections", Reporting)
        CreateNodeCore("reporting_aging", "Features", "Aging Summary A", rCollections)
        CreateNodeCore("reporting_aging_summary", "Features", "Aging Summary B", rCollections)
        CreateNodeCore("reporting_collections_admin", "Features", "Collections Admin", rCollections)
        CreateNodeCore("reporting_sms_sending", "Features", "SMS Sending", rCollections)
        CreateNodeCore("reporting_payments_vs_contacts", "Features", "Payments VS Contacts", rCollections)

        Dim rMarketing As TreeListNode = CreateNodeCore("reporting_marketing_node", "Folder", "Marketing", Reporting)
        CreateNodeCore("reporting_marketing_export", "Features", "Marketing Export", rMarketing)
        CreateNodeCore("reporting_sms_error_report", "Features", "SMS Error Report", rMarketing)

        Dim rHR As TreeListNode = CreateNodeCore("hr_collections_node", "Folder", "HR", Reporting)
        CreateNodeCore("reporting_employee_reviews", "Features", "Employee Reviews", rHR)
        CreateNodeCore("reporting_questionnaire_report", "Features", "Questionnaire Report", rHR)
        CreateNodeCore("reporting_employee_details_report", "Features", "Employee Details Report", rHR)
        CreateNodeCore("reporting_employee_per_store_report", "Features", "Employee Per Store Report", rHR)
        CreateNodeCore("reporting_employee_clocking_report", "Features", "Employees Clocking", rHR)
        CreateNodeCore("reporting_grdemployee_clocking_report", "Features", "Employees Clocking Report", rHR)
        CreateNodeCore("reporting_historical_clocking_report", "Features", "Historical Clocking Report", rHR)


        Dim rUsers As TreeListNode = CreateNodeCore("reporting_users_node", "Folder", "Users", rCollections)
        CreateNodeCore("reporting_user_transactions", "Features", "Transactions", rUsers)
        CreateNodeCore("reporting_user_summary", "Features", "Summary", rUsers)

        Dim rPositive As TreeListNode = CreateNodeCore("reporting_positive_node", "Folder", "Positive", Reporting)
        CreateNodeCore("reporting_bank_recon", "Features", "Bank Recon Report", rPositive)
        CreateNodeCore("reporting_buying_report", "Features", "Buying Report", rPositive)
        CreateNodeCore("reporting_cash_card_summary", "Features", "Cash Card Summary", rPositive)
        CreateNodeCore("reporting_cash_card_transactions", "Features", "Cash Card Transactions", rPositive)
        CreateNodeCore("reporting_category_report", "Features", "Category Report", rPositive)
        CreateNodeCore("reporting_last_update_data", "Features", "Last Update Data", rPositive)
        CreateNodeCore("reporting_rankers_report", "Features", "Rankers Report", rPositive)
        CreateNodeCore("reporting_single_stock_on_hand_report", "Features", "Single Stock on Hand", rPositive)
        CreateNodeCore("reporting_positive_cash_cards_web_service_logs", "Features", "Web Service Log", rPositive)
        CreateNodeCore("reporting_average_cash_sale", "Features", "Average Cash Sale", rPositive)
        CreateNodeCore("reporting_current_turnover_graph_report", "Features", "Current Turnover Graph", rPositive)
        CreateNodeCore("reporting_graph_segments", "Features", "Graph Segments", rPositive)
        CreateNodeCore("reporting_account_sales_report", "Features", " Account Sale", rPositive)
        CreateNodeCore("reporting_new_cashup_report", "Features", " New CashUp", rPositive)
        CreateNodeCore("reporting_cash_discrepancy_report", "Features", " Cash Discrepancy", rPositive)
        CreateNodeCore("reporting_cash_transactions_report", "Features", " Cash Transactions Report", rPositive)
        CreateNodeCore("reporting_stock_transactions_report", "Features", " Stock Transactions Report", rPositive)
        CreateNodeCore("reporting_best_sellers_report", "Features", " Best Sellers Report A", rPositive)
        CreateNodeCore("reporting_tasks_report", "Features", " Tasks Report", rPositive)
        CreateNodeCore("reporting_reprints", "Features", " Reprints", rPositive)
        CreateNodeCore("reporting_deleted_ibt_report", "Features", "Deleted IBTs", rPositive)
        CreateNodeCore("reporting_credit_card_auth_report", "Features", "Credit Card Auth", rPositive)
        'CreateNodeCore("reporting_cash_report", "Features", "Cash Report", rPositive)
        CreateNodeCore("reporting_branch_report", "Features", "Branch Report", rPositive)
        CreateNodeCore("reporting_reprint_dispatched_stock", "Features", "Reprint Dispatched Stock", rPositive)
        CreateNodeCore("reporting_sell_off_per_item", "Features", "Sell Off Per Item", rPositive)
        CreateNodeCore("reporting_category_summary", "Features", "Category Summary", rPositive)
        CreateNodeCore("reporting_colour_grids_summary", "Features", "Colour Grids", rPositive)
        CreateNodeCore("reporting_size_grids", "Features", "Size Grids", rPositive)
        CreateNodeCore("reporting_stock_on_hand_by_branch", "Features", "Stock On Hand By Branch", rPositive)
        CreateNodeCore("reporting_best_sellers", "Features", "Best Sellers Report B", rPositive)
        CreateNodeCore("reporting_total_stock_on_hand_report", "Features", "Total Stock On Hand", rPositive)
        CreateNodeCore("reporting_total_stock_by_branch", "Features", "Total Stock By Branch", rPositive)
        CreateNodeCore("reporting_stock_history", "Features", "Stock History", rPositive)
        CreateNodeCore("reporting_voids_and_no_sales", "Features", "Voids And No Sales", rPositive)
        CreateNodeCore("reporting_discounts", "Features", "Discount", rPositive)
        CreateNodeCore("reporting_soh", "Features", "SOH", rPositive)
        CreateNodeCore("reporting_web_service_web_logs", "Features", "Web Service Web Logs", rPositive)

        Dim rPCM As TreeListNode = CreateNodeCore("reporting_pcm_node", "Folder", "PCM", Reporting)

        CreateNodeCore("reporting_account_card_summary", "Features", "Account Card Summary", rPCM)
        CreateNodeCore("reporting_accounts_drive", "Features", "Accounts Drive A", rPCM)
        CreateNodeCore("reporting_accounts_opened_by_store", "Features", "Accounts Opened Per Store", rPCM)
        CreateNodeCore("reporting_age_analysis_report", "Features", "Age Analysis", rPCM)
        CreateNodeCore("reporting_age_analysis_summary_report", "Features", "Age Analysis Summary", rPCM)
        CreateNodeCore("reporting_bad_debt", "Features", "Bad Debt", rPCM)
        CreateNodeCore("reporting_bad_debt_by_store", "Features", "Bad Debt By Store", rPCM)
        CreateNodeCore("reporting_collections_by_user", "Features", "Collections By User", rPCM)
        CreateNodeCore("reporting_debtor_history", "Features", "Debtor History", rPCM)
        CreateNodeCore("reporting_gift_card_report", "Features", "Gift Card Details", rPCM)
        CreateNodeCore("reporting_incoming_manual_sale_sms", "Features", "Incoming Manual Sale SMS", rPCM)
        CreateNodeCore("reporting_incoming_rage_sms", "Features", "Incoming Rage SMS", rPCM)
        CreateNodeCore("reporting_incoming_sms", "Features", "Incoming SMS", rPCM)
        CreateNodeCore("reporting_interest_payments", "Features", "Interest Payments", rPCM)
        CreateNodeCore("reporting_new_accounts", "Features", "New Accounts", rPCM)
        CreateNodeCore("reporting_sales_payments_per_account", "Features", "Sales And Payments Per Account", rPCM)
        CreateNodeCore("reporting_transaction_report", "Features", "Quick Transaction Report", rPCM)
        CreateNodeCore("reporting_transactions_by_store", "Features", "Transactions By Store", rPCM)
        CreateNodeCore("reporting_transunion_log", "Features", "Transunion Web Service Log", rPCM)
        CreateNodeCore("reporting_vintage_report", "Features", "Vintage Report", rPCM)
        CreateNodeCore("reporting_limitincreasereport_report", "Features", "Limit Increase Report", rPCM)
        CreateNodeCore("reporting_error_report", "Features", "Error Report", rPCM)
        CreateNodeCore("reporting_average_account_sale", "Features", "Average Account Sale", rPCM)
        CreateNodeCore("reporting_accounts_drive_B", "Features", "Accounts Drive B", rPCM)

    End Sub
    Private Function getprocessing() As String

        Dim permstring As String = ""

        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_stock_allocation").Selected()) '1
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_collections").Selected()) '2
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_contact_investigation").Selected()) '3
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_investigation").Selected()) '4
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_sms").Selected()) '5
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_pcm_transactions").Selected()) '6
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_pcm_journals").Selected()) '7
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_card_allocations").Selected()) '8
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_employee_reviews").Selected()) '9
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_upload_payments").Selected()) '10
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_ibt_out").Selected()) '11
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_ibt_in").Selected()) '12
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_delete_ibt").Selected()) '13
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_manage_ibt").Selected()) '14
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_dispatch_stock").Selected()) '15
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_stock_adjustments").Selected()) '16
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_grv").Selected()) '17
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("processing_return_to_warehouse").Selected()) '18
        Return permstring

    End Function
    Private Function getmaintenance() As String

        Dim permstring As String = ""

        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_stockcode_manager").Selected()) '1
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_pre_packs").Selected()) '2
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_screen_saver_images").Selected()) '3
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_buying_images").Selected()) '4
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_user_permissions").Selected()) '5
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_self_assist_images").Selected()) '6
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_bank_recon").Selected()) '7
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_create_questionnaire").Selected()) '8
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_debtors").Selected()) '9
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_positive_users").Selected()) '10
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_employees").Selected()) '11
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_send_marketing_sms").Selected()) '12
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_loyalty_discounts").Selected()) '13
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_block_stock").Selected()) '14
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_branches").Selected()) '15
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_tills").Selected()) '16
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_targets").Selected()) '17
        'permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_stationary").Selected()) '18
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_colour_matrix").Selected()) '18
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_size_matrix").Selected()) '19
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_specials").Selected()) '20
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_regions").Selected()) '21
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_suppliers").Selected()) '22
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_points").Selected()) '23
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_categories").Selected()) '24
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_manage_municipalities").Selected()) '25
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_import_branches").Selected()) '26
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("maintenance_import_stock_quantity").Selected()) '27

        Return permstring

    End Function
    Private Function getreporting() As String

        Dim permstring As String = ""

        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_user_transactions").Selected()) '1
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_user_summary").Selected()) '2
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_aging").Selected()) '3
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_collections_admin").Selected()) '4
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_sms_sending").Selected()) '5
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_payments_vs_contacts").Selected()) '6
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_cash_card_transactions").Selected()) '7
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_positive_cash_cards_web_service_logs").Selected()) '8
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_last_update_data").Selected()) '9
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_rankers_report").Selected()) '10
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_buying_report").Selected()) '11
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_transunion_log").Selected()) '12
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_incoming_sms").Selected()) '13
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_incoming_rage_sms").Selected()) '14
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_accounts_opened_by_store").Selected()) '15
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_accounts_drive").Selected()) '16
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_bad_debt_by_store").Selected()) '17
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_collections_by_user").Selected()) '18
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_transactions_by_store").Selected()) '19
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_cash_card_summary").Selected()) '20
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_sales_payments_per_account").Selected()) '21
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_incoming_manual_sale_sms").Selected()) '22
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_bank_recon").Selected()) '23
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_category_report").Selected()) '24
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_single_stock_on_hand_report").Selected()) '25
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_questionnaire_report").Selected()) '26
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_age_analysis_report").Selected()) '27
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_age_analysis_summary_report").Selected()) '28
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_gift_card_report").Selected()) '29
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_transaction_report").Selected()) '30
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_vintage_report").Selected()) '31
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_new_accounts").Selected()) '32
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_account_card_summary").Selected()) '33
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_debtor_history").Selected()) '34
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_employee_reviews").Selected()) '35
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_marketing_export").Selected()) '36
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_interest_payments").Selected()) '37
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_bad_debt").Selected()) '38
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_limitincreasereport_report").Selected()) '39
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_error_report").Selected()) '40
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_current_turnover_graph_report").Selected()) '41
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_average_account_sale").Selected()) '42
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_graph_segments").Selected()) '43
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_sms_error_report").Selected()) '44
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_average_cash_sale").Selected()) '45
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_employee_details_report").Selected()) '46
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_account_sales_report").Selected()) '47
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_employee_per_store_report").Selected()) '48
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_employee_clocking_report").Selected()) '49
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_grdemployee_clocking_report").Selected()) '50
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_new_cashup_report").Selected()) '51
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_cash_discrepancy_report").Selected()) '52
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_cash_transactions_report").Selected()) '53
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_historical_clocking_report").Selected()) '54
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_stock_transactions_report").Selected()) '55
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_best_sellers_report").Selected()) '56
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_accounts_drive_B").Selected()) '57
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_tasks_report").Selected()) '58
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_reprints").Selected()) '59
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_deleted_ibt_report").Selected()) '60
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_credit_card_auth_report").Selected()) '61
        'permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_cash_report").Selected()) '62
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_branch_report").Selected()) '63
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_reprint_dispatched_stock").Selected()) '64
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_sell_off_per_item").Selected()) '64
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_category_summary").Selected()) '65
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_colour_grids_summary").Selected()) '66
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_size_grids").Selected()) '67
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_stock_on_hand_by_branch").Selected()) '68
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_aging_summary").Selected()) '69
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_best_sellers").Selected()) '70
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_total_stock_on_hand_report").Selected()) '71
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_total_stock_by_branch").Selected()) '72
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_stock_history").Selected()) '73
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_voids_and_no_sales").Selected()) '74
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_discounts").Selected()) '75
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_soh").Selected()) '76
        permstring &= TrueFalse(treeListAll.FindNodeByKeyValue("reporting_web_service_web_logs").Selected()) '76
        Return permstring

    End Function
    Private Sub ClearScreen()

        cboUsers.Text = ""

        txtPassword.Text = ""

        For i As Integer = 0 To treeListAll.Nodes.Count - 1

            treeListAll.Nodes(i).Selected = False
            If treeListAll.Nodes(i).HasChildren Then
                For x As Integer = 0 To treeListAll.Nodes(i).ChildNodes.Count - 1
                    treeListAll.Nodes(i).ChildNodes(x).Selected = False
                Next x
            End If

        Next

        cboUsers.Text = ""
        cboUsers.Items.Clear()
        txtEmail.Text = ""
        chkActive.Checked = False

        chkPCMAdmin.Checked = False

        Dim _BLayerStock As New UsersBL()
        Dim _dt As DataTable

        _dt = _BLayerStock.GetUsers

        For i As Integer = 0 To _dt.Rows.Count - 1
            cboUsers.Items.Add(_dt(i)("username"))
        Next

    End Sub
End Class