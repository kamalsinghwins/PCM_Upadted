Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class JournalTransactions
    Inherits System.Web.UI.Page

    Dim _blDebtor As DebtorsBusinessLayer = New DebtorsBusinessLayer
    Dim _blAccounts As AccountsBL = New AccountsBL
    Dim _DebtorMaintainence As DebtorsMaintainenceBusinessLayer = New DebtorsMaintainenceBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.PCMJournals) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            initialize_values()
            radEffected.Checked = True

            ''=====================================================================================================
            ''LOGGING
            ''=====================================================================================================
            'Dim _Log As New PCMUserLog
            '_Log.AccountNumber = ""
            '_Log.ActionType = "Page Load"
            '_Log.IPAddress = Session("LoggingIPAddress")
            '_Log.SearchCriteria = ""
            '_Log.UserComment = ""
            '_Log.UserName = Session("username")
            '_Log.WebPage = "Journal Transactions"

            '_Logging.WriteToLogPCM(_Log)
            ''=====================================================================================================

        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "CheckAN" Then
            txtAccNum_KeyPress()
        End If


        If hdWhichButton.Value = "Insert" Then
            cmdInsert_Click()
        End If

        If hdWhichButton.Value = "Lookup" Then
            grdDebtorsSearch.DataBind()
        End If

        If hdWhichButton.Value = "Clear" Then
            reset_values()
        End If

        If hdWhichButton.Value = "DebtorSelected" Then
            GetSelectedDebtorsDetails()
        End If

    End Sub

    Private Sub GetSelectedDebtorsDetails()

        'Clear all the controls
        reset_values()

        Dim selectedValues = New List(Of Object)()

        selectedValues = Nothing

        selectedValues = grdDebtorsSearch.GetSelectedFieldValues("account_number")

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Getting Debtor Data"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Account Number: " & selectedValues(selectedValues.Count - 1)
        _Log.UserComment = ""
        _Log.AccountNumber = selectedValues(selectedValues.Count - 1)
        _Log.UserName = Session("username")
        _Log.WebPage = "Journal Transactions"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        If selectedValues.Count > 0 Then
            Dim strAccNum As String = selectedValues(selectedValues.Count - 1)
            txtAccNum.Text = strAccNum
            txtAccNum.ReadOnly = True
            txtName.ReadOnly = True

            txtAccNum_KeyPress()
        Else
            txtAccNum.ReadOnly = False
            txtName.ReadOnly = False

            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Details Found"
            dxPopUpError.ShowOnPageLoad = True
        End If



        LookupMain.ShowOnPageLoad = False

    End Sub

    Protected Sub grdDebtorsSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdDebtorsSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _tmpCBO As ASPxComboBox

        _tmpCBO = LookupMain.FindControl("cboSearchType")

        Dim _tmpTXT As ASPxTextBox

        _tmpTXT = LookupMain.FindControl("txtCriteria")

        'Dim _tmpCheck As ASPxCheckBox

        '_tmpCheck = LookupMain.FindControl("chkRageEmployeesOnly")

        'Dim _tmpLast100 As ASPxCheckBox

        '_tmpLast100 = LookupMain.FindControl("chkLast100")

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Searching for Debtor"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Search Criteria: " & _tmpCBO.Text & " Search: " & _tmpTXT.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Journal Transactions"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Dim data As DataTable = _DebtorMaintainence.GetDebtors(_tmpCBO.Text, _tmpTXT.Text.ToUpper, False, False)

        gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdDebtorsSearch.EndUpdate()
    End Sub


    Public Sub initialize_values()

        cboTransactionType.Items.Add("Credit")
        cboTransactionType.Items.Add("Debit")

        cboSearchType.Items.Add("ACCOUNT NUMBER")
        cboSearchType.Items.Add("ID NUMBER")
        cboSearchType.Items.Add("LAST NAME")
        cboSearchType.Items.Add("CELLPHONE")

    End Sub

    Public Sub reset_values()

        txtAccNum.Text = ""
        txtName.Text = ""
        txtAmount.Text = ""

        cboType.Items.Clear()
        cboType.Text = ""

        cboTransactionType.Items.Clear()
        cboTransactionType.Items.Add("Credit")
        cboTransactionType.Items.Add("Debit")

        txtAccNum.ReadOnly = False
        txtName.ReadOnly = False

        Dim _tmpCBO As ASPxComboBox
        _tmpCBO = LookupMain.FindControl("cboSearchType")
        _tmpCBO.Items.Clear()

        _tmpCBO.Items.Add("ACCOUNT NUMBER")
        _tmpCBO.Items.Add("ID NUMBER")
        _tmpCBO.Items.Add("LAST NAME")
        _tmpCBO.Items.Add("CELLPHONE")

        'Dim _tmpLast100 As ASPxCheckBox
        '_tmpLast100 = LookupMain.FindControl("chkLast100")
        '_tmpLast100.Checked = False

        'Dim _tmpRageEmployee As ASPxCheckBox
        '_tmpRageEmployee = LookupMain.FindControl("chkRageEmployeesOnly")
        '_tmpRageEmployee.Checked = False

        ASPxEdit.ClearEditorsInContainer(LookupMain)

        Dim _eDt As DataTable

        dxGrid.DataSource = _eDt
        dxGrid.DataBind()

        'grdDebtorsSearch.DataSource = _eDt
        'grdDebtorsSearch.DataBind()

        cboTransactionType.Text = ""
        cboType.Text = ""

        radEffected.Checked = True

        txtNotes.Text = ""

    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        If txtAccNum.Text = "" Then Exit Sub

        Dim _dt As DataTable = _blDebtor.CheckAccountNumber(txtAccNum.Text)

        If _dt IsNot Nothing Then
            If _dt.Rows.Count > 0 Then
                txtName.Text = _dt.Rows(0)("first_name") & " " & _dt.Rows(0)("last_name")
                dxGrid.DataSource = _dt
                Exit Sub
            End If
        End If

        txtAccNum.Text = ""
        txtName.Text = ""
        txtAmount.Text = ""

        cboType.Items.Clear()
        cboType.Text = ""

        cboTransactionType.Items.Clear()
        cboTransactionType.Items.Add("Credit")
        cboTransactionType.Items.Add("Debit")

        dxPopUpError.HeaderText = "Error"
        lblError.Text = "This Account Number does not Exist."
        dxPopUpError.ShowOnPageLoad = True


        'Dim _NewReport As New ReportsBusinessLayer

        'dxGrid.DataSource = _NewReport.ReturnAgingSummary(txtAmount.Text)

    End Sub


    Private Sub txtAccNum_KeyPress()

        dxGrid.DataBind()

    End Sub

    Public Sub cmdInsert_Click()
        'If txtCompleted.Text = "" Then
        '    dxPopUpError.HeaderText = "Error"
        '    lblError.Text = "Please specify who is completing this transaction."
        '    dxPopUpError.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        If txtAccNum.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select an account before you continue."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtName.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select an account before you continue."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If Val(txtAmount.Text) > 5000 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter a smaller amount."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If Val(txtAmount.Text) <= 0 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please input a valid amount."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboType.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a period before you continue."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboTransactionType.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a transaction type before you continue."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If Len(txtNotes.Text) < 5 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please input a proper note."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        cmdInsert.ClientEnabled = False


        Dim _JournalEntryRequest As New JournalEntryRequest
        _JournalEntryRequest.AccountNumber = txtAccNum.Text
        _JournalEntryRequest.TransactionType = cboTransactionType.Text
        _JournalEntryRequest.Amount = If(cboTransactionType.Text = "Credit", "-" & txtAmount.Text, txtAmount.Text)
        _JournalEntryRequest.BalanceAffected = If(radEffected.Checked = True, True, False)
        '_JournalEntryRequest.BalanceAffected = True
        _JournalEntryRequest.AffectedPeriod = cboType.Text
        _JournalEntryRequest.User = Session("username")
        _JournalEntryRequest.Notes = txtNotes.Text


        Dim ProcessJournal As String

        ProcessJournal = _blAccounts.ProcessJournalEntry(_JournalEntryRequest)
        If ProcessJournal = "PASSED" Then
            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Processing Journal"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = "Account Number: " & txtAccNum.Text & " Trans. Type: " & cboTransactionType.Text
            _Log.AccountNumber = txtAccNum.Text
            _Log.UserComment = txtNotes.Text
            _Log.UserName = Session("username")
            _Log.WebPage = "Journal Transactions"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================


            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Transaction was completed"
            dxPopUpError.ShowOnPageLoad = True
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = ProcessJournal
            dxPopUpError.ShowOnPageLoad = True
            cmdInsert.ClientEnabled = True
            Exit Sub
        End If

        reset_values()
        cmdInsert.ClientEnabled = True

    End Sub

    Protected Sub cboTransactionType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTransactionType.SelectedIndexChanged

        cboType.Items.Clear()

        If cboTransactionType.Text = "Credit" Then
            cboType.Items.Add("CURRENT")
            cboType.Items.Add("30 DAYS")
            cboType.Items.Add("60 DAYS")
            cboType.Items.Add("90 DAYS")
            cboType.Items.Add("120 DAYS")
            cboType.Items.Add("150 DAYS")
            cboType.Items.Add("FROM OLDEST PERIOD")

        ElseIf cboTransactionType.Text = "Debit" Then
            cboType.Items.Add("CURRENT")
            cboType.Items.Add("30 DAYS")
            cboType.Items.Add("60 DAYS")
            cboType.Items.Add("90 DAYS")
            cboType.Items.Add("120 DAYS")
            cboType.Items.Add("150 DAYS")
            cboType.Items.Add("ACROSS ALL PERIODS")
        End If

    End Sub
End Class