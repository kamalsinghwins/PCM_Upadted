Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Imports pcm.DataLayer

Public Class CashReports
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim RG As New Utilities.clsUtil
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CashReports) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            txtSummaryDate.Text = Format(Now, "yyyy-MM-dd")
            Try

                ds = reports.GetBranches()
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        cboBranch.Items.Add(dr("branch_code") & " - " & dr("branch_name"))
                    Next
                End If
            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try

        End If
    End Sub
    Private Sub CashReports_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "RunAdvanceSummary" Then
                RunAdvanceSummary()
            End If
            If hdWhichButton.Value = "RunCashUpSummary" Then
                RunCashSummary()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub RunCashSummary()
        If cboBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please Select the branch"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        Session.Remove("CashUpSummary")
        gridCashSummary.DataBind()

    End Sub
    Private Sub RunAdvanceSummary()
        If cboBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please Select the branch"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Session.Remove("AdvanceSummary")
        grid.DataBind()
    End Sub
    Protected Sub grid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grid.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        grid.KeyFieldName = "guid"
        grid.DataSource = data
        grid.EndUpdate()

    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("AdvanceSummary")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet
        Dim dt1 As New DataTable
        dt1 = New DataTable
        dt1.Columns.Add("Sale Date")
        dt1.Columns.Add("Till Number")
        dt1.Columns.Add("Transaction Type")
        dt1.Columns.Add("Transaction Number")
        dt1.Columns.Add("Cash")
        dt1.Columns.Add("Cheque")
        dt1.Columns.Add("Credit Card")
        dt1.Columns.Add("Voucher")
        dt1.Columns.Add("Account")
        dt1.Columns.Add("Sale Total")
        dt1.Columns.Add("Payment Details")
        If Not IsNothing(Session("AdvanceSummary")) Then
            Return Session("AdvanceSummary")
        End If

        Dim advance As New DataTable

        'First loop through the tills
        dt = reports.GetTillNumber(Mid(cboBranch.Text, 1, 3))
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                If dr("till_number") <> "" Then
                    'POSSALE
                    advance = reports.GetCashTransaction(dr("till_number"), txtFromDate.Text, txtToDate.Text)
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "Sales",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next

                    advance.Clear()

                    '------------------------------------------------------------------------

                    'CREDIT NOTES

                    advance = reports.GetCashTransactionByType(dr("till_number"), txtFromDate.Text, txtToDate.Text, "POSCN")
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "C/N",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next
                    advance.Clear()
                    '------------------------------------------------------------------------

                    'REFUNDS
                    advance = reports.GetCashTransactionByType(dr("till_number"), txtFromDate.Text, txtToDate.Text, "POSREF")
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "Refunds",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next
                    advance.Clear()

                    '------------------------------------------------------------------------

                    'PAID OUT

                    advance = reports.GetCashTransactionByType(dr("till_number"), txtFromDate.Text, txtToDate.Text, "POUT")
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "Paid Out",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next
                    advance.Clear()
                    '------------------------------------------------------------------------

                    'PAID IN
                    advance = reports.GetCashTransactionByType(dr("till_number"), txtFromDate.Text, txtToDate.Text, "PIN")
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "Paid In",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next
                    advance.Clear()

                    '------------------------------------------------------------------------

                    'ACCOUNT PAYMENTS

                    advance = reports.GetCashTransactionByType(dr("till_number"), txtFromDate.Text, txtToDate.Text, "ACCPAY")
                    For Each dr2 As DataRow In advance.Rows
                        If dr2("transaction_number") <> "" Then
                            dt1.Rows.Add(dr2("sale_date"), dr2("till_number"), "Account Payments",
                                                              dr2("transaction_number"), dr2("cash"),
                                                              dr2("cheque"), dr2("credit_card"), dr2("voucher"),
                                                              dr2("account"), dr2("sale_total"), dr2("payment_details"))
                        End If
                    Next
                    advance.Clear()


                End If
            Next

        End If

        Dim data As DataSet
        dt1.TableName = "AdvanceSummary"

        data = New DataSet()
        data.Tables.Add(dt1)
        Session("AdvanceSummary") = data

        Return data
    End Function
    Private Function GetCashUpData() As DataTable
        Dim data As DataSet = GetCashReports()
        If data IsNot Nothing Then
            Return data.Tables("CashUpSummary")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetCashReports() As DataSet

        If Not IsNothing(Session("CashUpSummary")) Then
            Return Session("CashUpSummary")
        End If

        Dim TurnoverTotal(6) As Double
        Dim TotalBanking(6) As Double
        Dim dt As New DataTable

        Dim Grd(10, 9) As String
        Dim GrdTotal(10, 9) As String

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim col As Integer

        Grd(1, 1) = "Type"
        Grd(1, 2) = "Cash"
        Grd(1, 3) = "Cheque"
        Grd(1, 4) = "Credit Card"
        Grd(1, 5) = "Vouchers"
        Grd(1, 6) = "Accounts"
        Grd(1, 7) = "Credits"
        Grd(1, 8) = "Totals"

        Dim Cashup As New DataTable
        Cashup.Columns.Add("Till Number")
        Cashup.Columns.Add("Type")
        Cashup.Columns.Add("Cash")
        Cashup.Columns.Add("Cheque")
        Cashup.Columns.Add("Credit Card")
        Cashup.Columns.Add("Vouchers")
        Cashup.Columns.Add("Accounts")
        Cashup.Columns.Add("Totals")


        For j = 1 To 9
            For k = 1 To 8
                GrdTotal(j, k) = "0.00"
            Next
        Next



        'First loop through the tills
        dt = reports.GetTillNumber(Mid(cboBranch.Text, 1, 3))
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                If dr("till_number") <> "" Then
                    'Loop through the tills
                    Grd(2, 1) = "Sales"
                    Grd(2, 2) = reports.GetTotal(dr("till_number"), "POSSALE", "cash", txtSummaryDate.Text)
                    Grd(2, 3) = reports.GetTotal(dr("till_number"), "POSSALE", "cheque", txtSummaryDate.Text)
                    Grd(2, 4) = reports.GetTotal(dr("till_number"), "POSSALE", "credit_card", txtSummaryDate.Text)
                    Grd(2, 5) = reports.GetTotal(dr("till_number"), "POSSALE", "voucher", txtSummaryDate.Text)
                    Grd(2, 6) = reports.GetTotal(dr("till_number"), "POSSALE", "account", txtSummaryDate.Text)
                    Grd(2, 7) = "-" 'GetTotal(dr("till_number"), "POSSALE", "cn")
                    Grd(2, 8) = RG.Numb(Val(Grd(2, 2)) + Val(Grd(2, 3)) + Val(Grd(2, 4)) + Val(Grd(2, 5)) + Val(Grd(2, 6)) + Val(Grd(2, 7)))

                    Cashup.Rows.Add(dr("till_number"), "Sales", Grd(2, 2),
                                                  Grd(2, 3), Grd(2, 4),
                                                  Grd(2, 5), Grd(2, 6),
                                                   Grd(2, 8))

                    Grd(3, 1) = "C/Notes"
                    Grd(3, 2) = Val(reports.GetTotal(dr("till_number"), "POSCN", "cash", txtSummaryDate.Text))
                    Grd(3, 3) = Val(reports.GetTotal(dr("till_number"), "POSCN", "cheque", txtSummaryDate.Text))
                    Grd(3, 4) = Val(reports.GetTotal(dr("till_number"), "POSCN", "credit_card", txtSummaryDate.Text))
                    Grd(3, 5) = Val(reports.GetTotal(dr("till_number"), "POSCN", "voucher", txtSummaryDate.Text))
                    Grd(3, 6) = Val(reports.GetTotal(dr("till_number"), "POSCN", "account", txtSummaryDate.Text))
                    Grd(3, 7) = "-"
                    Grd(3, 8) = RG.Numb(Val(Grd(3, 2)) + Val(Grd(3, 3)) + Val(Grd(3, 4)) + Val(Grd(3, 5)) + Val(Grd(3, 6)))

                    Cashup.Rows.Add(dr("till_number"), "C/Notes", RG.Numb(Grd(3, 2)),
                                                  RG.Numb(Grd(3, 3)), RG.Numb(Grd(3, 4)),
                                                  RG.Numb(Grd(3, 5)), RG.Numb(Grd(3, 6)),
                                                  RG.Numb(Grd(3, 8)))

                    Grd(4, 1) = "Refunds"
                    Grd(4, 2) = reports.GetTotal(dr("till_number"), "POSREF", "cash", txtSummaryDate.Text)
                    Grd(4, 3) = reports.GetTotal(dr("till_number"), "POSREF", "cheque", txtSummaryDate.Text)
                    Grd(4, 4) = reports.GetTotal(dr("till_number"), "POSREF", "credit_card", txtSummaryDate.Text)
                    Grd(4, 5) = reports.GetTotal(dr("till_number"), "POSREF", "voucher", txtSummaryDate.Text)
                    Grd(4, 6) = reports.GetTotal(dr("till_number"), "POSREF", "account", txtSummaryDate.Text)
                    Grd(4, 7) = "-"
                    Grd(4, 8) = RG.Numb(Val(Grd(4, 2)) + Val(Grd(4, 3)) + Val(Grd(4, 4)) + Val(Grd(4, 5)) + Val(Grd(4, 6)))

                    Cashup.Rows.Add(dr("till_number"), "Refunds", Grd(4, 2),
                                                  Grd(4, 3), Grd(4, 4),
                                                  Grd(4, 5), Grd(4, 6),
                                                  Grd(4, 8))

                    Grd(5, 1) = "Paid In"
                    Grd(5, 2) = reports.GetTotal(dr("till_number"), "PIN", "cash", txtSummaryDate.Text)
                    Grd(5, 3) = reports.GetTotal(dr("till_number"), "PIN", "cheque", txtSummaryDate.Text)
                    Grd(5, 4) = reports.GetTotal(dr("till_number"), "PIN", "credit_card", txtSummaryDate.Text)
                    Grd(5, 5) = reports.GetTotal(dr("till_number"), "PIN", "voucher", txtSummaryDate.Text)
                    Grd(5, 6) = "-"
                    Grd(5, 7) = "-"
                    Grd(5, 8) = RG.Numb(Val(Grd(5, 2)) + Val(Grd(5, 3)) + Val(Grd(5, 4)) + Val(Grd(5, 5)) + Val(Grd(5, 6)))

                    Cashup.Rows.Add(dr("till_number"), "Paid In", Grd(5, 2),
                                                  Grd(5, 3), Grd(5, 4),
                                                  Grd(5, 5), Grd(5, 6),
                                                  Grd(5, 8))

                    Grd(6, 1) = "Paid Out"
                    Grd(6, 2) = reports.GetTotal(dr("till_number"), "POUT", "cash", txtSummaryDate.Text)
                    Grd(6, 3) = reports.GetTotal(dr("till_number"), "POUT", "cheque", txtSummaryDate.Text)
                    Grd(6, 4) = reports.GetTotal(dr("till_number"), "POUT", "credit_card", txtSummaryDate.Text)
                    Grd(6, 5) = reports.GetTotal(dr("till_number"), "POUT", "voucher", txtSummaryDate.Text)
                    Grd(6, 6) = "-"
                    Grd(6, 7) = "-"
                    Grd(6, 8) = RG.Numb(Val(Grd(6, 2)) + Val(Grd(6, 3)) + Val(Grd(6, 4)) + Val(Grd(6, 5)) + Val(Grd(6, 6)))

                    Cashup.Rows.Add(dr("till_number"), "Paid Out", Grd(6, 2),
                                                  Grd(6, 3), Grd(6, 4),
                                                  Grd(6, 5), Grd(6, 6),
                                                  Grd(6, 8))

                    Grd(7, 1) = "Acc Pmt"
                    Grd(7, 2) = reports.GetTotal(dr("till_number"), "ACCPAY", "cash", txtSummaryDate.Text)
                    Grd(7, 3) = reports.GetTotal(dr("till_number"), "ACCPAY", "cheque", txtSummaryDate.Text)
                    Grd(7, 4) = reports.GetTotal(dr("till_number"), "ACCPAY", "credit_card", txtSummaryDate.Text)
                    Grd(7, 5) = reports.GetTotal(dr("till_number"), "ACCPAY", "voucher", txtSummaryDate.Text)
                    Grd(7, 6) = "-"
                    Grd(7, 7) = "-"
                    Grd(7, 8) = RG.Numb(Val(Grd(7, 2)) + Val(Grd(7, 3)) + Val(Grd(7, 4)) + Val(Grd(7, 5)) + Val(Grd(7, 6)))

                    Cashup.Rows.Add(dr("till_number"), "Acc Pmt", Grd(7, 2),
                                                  Grd(7, 3), Grd(7, 4),
                                                  Grd(7, 5), Grd(7, 6),
                                                  Grd(7, 8))

                    Grd(8, 1) = "Turnover"
                    Grd(8, 2) = Val(Grd(2, 2)) - Val(Grd(3, 2)) - Val(Grd(4, 2))
                    Grd(8, 3) = Val(Grd(2, 3)) - Val(Grd(3, 3)) - Val(Grd(4, 3))
                    Grd(8, 4) = Val(Grd(2, 4)) - Val(Grd(3, 4)) - Val(Grd(4, 4))
                    Grd(8, 5) = Val(Grd(2, 5)) - Val(Grd(3, 5)) - Val(Grd(4, 5))
                    Grd(8, 6) = Val(Grd(2, 6)) - Val(Grd(3, 6)) - Val(Grd(4, 6))
                    Grd(8, 7) = Val(Grd(2, 7))
                    Grd(8, 8) = Val(Grd(2, 8)) - Val(Grd(3, 8)) - Val(Grd(4, 8))

                    Cashup.Rows.Add(dr("till_number"), "Turnover", RG.Numb(Grd(8, 2)),
                                                  RG.Numb(Grd(8, 3)), RG.Numb(Grd(8, 4)),
                                                  RG.Numb(Grd(8, 5)), RG.Numb(Grd(8, 6)),
                                                  RG.Numb(Grd(8, 8)))

                    Grd(9, 1) = "Banking"
                    Grd(9, 2) = Val(Grd(2, 2)) - Val(Grd(4, 2)) + Val(Grd(5, 2)) - Val(Grd(6, 2)) + Val(Grd(7, 2))
                    Grd(9, 3) = Val(Grd(2, 3)) - Val(Grd(4, 3)) + Val(Grd(5, 3)) - Val(Grd(6, 3)) + Val(Grd(7, 3))
                    Grd(9, 4) = Val(Grd(2, 4)) - Val(Grd(4, 4)) + Val(Grd(5, 4)) - Val(Grd(6, 4)) + Val(Grd(7, 4))
                    Grd(9, 5) = Val(Grd(2, 5)) - Val(Grd(4, 5)) + Val(Grd(5, 5)) - Val(Grd(6, 5)) + Val(Grd(7, 5))
                    Grd(9, 6) = Val(Grd(2, 6)) - Val(Grd(4, 6)) + Val(Grd(5, 6)) - Val(Grd(6, 6)) + Val(Grd(7, 6))
                    Grd(9, 7) = "0.00"
                    Grd(9, 8) = Val(Grd(2, 8)) - Val(Grd(4, 8)) + Val(Grd(5, 8)) - Val(Grd(6, 8)) + Val(Grd(7, 8))

                    Cashup.Rows.Add(dr("till_number"), "Banking", RG.Numb(Grd(9, 2)),
                                                  RG.Numb(Grd(9, 3)), RG.Numb(Grd(9, 4)),
                                                  RG.Numb(Grd(9, 5)), RG.Numb(Grd(9, 6)),
                                                  RG.Numb(Grd(9, 8)))


                    For k = 1 To 8
                        GrdTotal(1, k) = Grd(1, k)

                        If k = 1 Then
                            GrdTotal(2, k) = Grd(2, k)
                            GrdTotal(3, k) = Grd(3, k)
                            GrdTotal(4, k) = Grd(4, k)
                            GrdTotal(5, k) = Grd(5, k)
                            GrdTotal(6, k) = Grd(6, k)
                            GrdTotal(7, k) = Grd(7, k)
                            GrdTotal(8, k) = Grd(8, k)
                            GrdTotal(9, k) = Grd(9, k)
                        Else
                            GrdTotal(2, k) = Val(GrdTotal(2, k)) + Val(Grd(2, k))
                            GrdTotal(3, k) = Val(GrdTotal(3, k)) + Val(Grd(3, k))
                            GrdTotal(4, k) = Val(GrdTotal(4, k)) + Val(Grd(4, k))
                            GrdTotal(5, k) = Val(GrdTotal(5, k)) + Val(Grd(5, k))
                            GrdTotal(6, k) = Val(GrdTotal(6, k)) + Val(Grd(6, k))
                            GrdTotal(7, k) = Val(GrdTotal(7, k)) + Val(Grd(7, k))
                            GrdTotal(8, k) = Val(GrdTotal(8, k)) + Val(Grd(8, k))
                            GrdTotal(9, k) = Val(GrdTotal(9, k)) + Val(Grd(9, k))
                        End If
                    Next k
                End If
            Next
        End If
        For k = 2 To 9
            Cashup.Rows.Add("Total", GrdTotal(k, 1), RG.Numb(GrdTotal(k, 2)),
                                                  RG.Numb(GrdTotal(k, 3)), RG.Numb(GrdTotal(k, 4)),
                                                  RG.Numb(GrdTotal(k, 5)), RG.Numb(GrdTotal(k, 6)),
                                                  RG.Numb(GrdTotal(k, 8)))
        Next

        Dim data As DataSet
        Cashup.TableName = "CashUpSummary"

        data = New DataSet()
        data.Tables.Add(Cashup)
        Session("CashUpSummary") = data

        Return data
    End Function
    Protected Sub gridCashSummary_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gridCashSummary.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetCashUpData()
        gridCashSummary.DataSource = data
        gridCashSummary.EndUpdate()

    End Sub
    Protected Sub btnExportCashSummary_Click(sender As Object, e As EventArgs) Handles btnExportCashSummary.Click
        cmdExportCashSummary.WriteXlsxToResponse()
    End Sub
    Protected Sub btnExportAdvance_Click(sender As Object, e As EventArgs) Handles btnExportAdvance.Click
        cmdExportAdvanceSummary.WriteXlsxToResponse()
    End Sub
End Class