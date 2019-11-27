Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class Transaction
    Inherits System.Web.UI.Page
    Dim _AccountsBL As AccountsBL = New AccountsBL
    Dim _DebtorMaintainence As DebtorsMaintainenceBusinessLayer = New DebtorsMaintainenceBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.PCMTransactions) Then
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
            InitialisePaymentValues()
            cboPayType.Visible = False
            PaymentTypeLabel.Visible = False

            'Dim _Log As New PCMUserLog
            '_Log.AccountNumber = ""
            '_Log.ActionType = "Page Load"
            '_Log.IPAddress = Session("LoggingIPAddress")
            '_Log.SearchCriteria = ""
            '_Log.UserComment = ""
            '_Log.UserName = Session("username")
            '_Log.WebPage = "Manual Transactions"

            '_Logging.WriteToLogPCM(_Log)
        End If

    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "ShowHideDate" Then
            ShowHideDate()
        End If

        If hdWhichButton.Value = "Insert" Then
            InsertTransaction
        End If

        If hdWhichButton.Value = "CheckCard" Then
            CheckCard()
        End If

        If hdWhichButton.Value = "ClearAll" Then
            ClearFields()
        End If

        If hdWhichButton.Value = "Lookup" Then
            grdDebtorsSearch.DataBind()
        End If


        If hdWhichButton.Value = "DebtorSelected" Then
            GetSelectedDebtorsDetails()
        End If

    End Sub

    Private Sub GetSelectedDebtorsDetails()

        'Clear all the controls
        ClearFields()

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
        _Log.AccountNumber = selectedValues(selectedValues.Count - 1)
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Manual Transactions"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        If selectedValues.Count > 0 Then
            Dim strAccountNumber As String = selectedValues(selectedValues.Count - 1)
            txtAccNum.Text = strAccountNumber
            txtCardNum.ReadOnly = True

            CheckAccount()
        Else
            txtCardNum.ReadOnly = False

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

        'If _tmpTXT.Text = "" Then Exit Sub

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
        _Log.WebPage = "Manual Transactions"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Dim data As DataTable = _DebtorMaintainence.GetDebtors(_tmpCBO.Text, _tmpTXT.Text.ToUpper, False, False)

        gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdDebtorsSearch.EndUpdate()
    End Sub

    Private Sub InitialisePaymentValues()
        cboTransactionType.Items.Clear()
        cboTransactionType.Items.Add("SALE")
        cboTransactionType.Items.Add("CN")
        cboTransactionType.Items.Add("PAY")

        cboPayType.Items.Clear()

        cboPayType.Items.Add("SHOP - CASH")
        cboPayType.Items.Add("SHOP - CHEQUE")
        cboPayType.Items.Add("SHOP - CREDIT CARD")
        cboPayType.Items.Add("SHOP - VOUCHER")
        cboPayType.Items.Add("DEBIT ORDER")
        cboPayType.Items.Add("INTERNET TRANSFER")
        cboPayType.Items.Add("DIRECT DEPOSIT")
        cboPayType.Items.Add("MAIL-IN")

        Dim _BLayer As New GeneralHOBL()
        Dim _dt As DataTable

        _dt = _BLayer.GetBranches

        For i As Integer = 0 To _dt.Rows.Count - 1
            cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
        Next

        txtAccNum.ReadOnly = True
        txtCustomer.ReadOnly = True
        txtID.ReadOnly = True

        cboSearchType.Items.Add("ACCOUNT NUMBER")
        cboSearchType.Items.Add("ID NUMBER")
        cboSearchType.Items.Add("LAST NAME")
        cboSearchType.Items.Add("CELLPHONE")
    End Sub

    Private Sub ShowHideDate()
        If cboTransactionType.Text = "PAY" Then
            cboPayType.Visible = True
            PaymentTypeLabel.Visible = True
        ElseIf cboTransactionType.Text = "SALES" Then
            InitialisePaymentValues()
            cboPayType.Visible = False
            PaymentTypeLabel.Visible = False
        Else
            cboPayType.Visible = False
            PaymentTypeLabel.Visible = False
        End If
    End Sub

    Private Sub CheckCard()
        Dim checkCard As New CheckCardRequest
        Dim checkCardResponse As New CheckCardResponse
        checkCard.CardNumber = txtCardNum.Text

        checkCardResponse = _AccountsBL.CheckCardnumber(checkCard)
        If checkCardResponse.Success = True Then
            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Checking Card Number"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = "Card Number: " & txtCardNum.Text
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Manual Transactions"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================

            txtAccNum.Text = checkCardResponse.AccountNumber
            txtCustomer.Text = checkCardResponse.Customer
            txtID.Text = checkCardResponse.ID
            txtCardNum.ReadOnly = True

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = checkCardResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If

        ShowHideDate()


    End Sub

    Private Sub CheckAccount()
        Dim checkCard As New CheckCardRequest
        Dim checkCardResponse As New CheckCardResponse
        'checkCard.CardNumber = txtCardNum.Text

        checkCard.AccountNumber = txtAccNum.Text

        checkCardResponse = _AccountsBL.CheckAccountNumber(checkCard)
        If checkCardResponse.Success = True Then
            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Getting Debtor Data"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = "Account Number: " & txtAccNum.Text
            _Log.AccountNumber = txtAccNum.Text
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Manual Transactions"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================

            txtCardNum.Text = checkCardResponse.CardNumber
            txtAccNum.Text = checkCardResponse.AccountNumber
            txtCustomer.Text = checkCardResponse.Customer
            txtID.Text = checkCardResponse.ID
            txtCardNum.ReadOnly = True

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = checkCardResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If

        ShowHideDate()


    End Sub

    Private Sub InsertTransaction()
        Dim processTransactionRequest As New PCMAccount
        Dim processTransactionResponse As New PCMAccount

        processTransactionRequest.AccountNumber = txtAccNum.Text
        processTransactionRequest.CardNumber = txtCardNum.Text
        processTransactionRequest.PayType = cboPayType.Text
        processTransactionRequest.TransactionNumber = Mid(cboBranch.Text.ToUpper, 1, 3)
        If cboTransactionType.Text = "SALE" Then
            processTransactionRequest.TransactionAmount = txtAmount.Text
        Else
            processTransactionRequest.TransactionAmount = "-" & txtAmount.Text
        End If
        processTransactionRequest.Notes = txtNotes.Text

        processTransactionRequest.UserName = txtShopA.Text.ToUpper & " CC: " & Session("username")
        processTransactionRequest.TransactionType = cboTransactionType.Value
        If chkPayDeposit.Checked = True Then
            processTransactionRequest.SpecialCondition = "Paid"
        End If

        processTransactionResponse = _AccountsBL.ProcessMultipleTransaction(processTransactionRequest)

        If processTransactionResponse.ReturnMessage = "Require Payment" Then
            dxPopUpError.HeaderText = "Question"
            lblError.Text = "The customer needs to pay a deposit of 1/6th of the sale amount. Please confirm."
            txtCardNum.ReadOnly = True
            chkPayDeposit.Visible = True
            lblPayDeposit.Visible = True
            dxPopUpError.ShowOnPageLoad = True
            txtShopA.ReadOnly = True
            cboBranch.ReadOnly = True
            txtAmount.ReadOnly = True
            cboTransactionType.ReadOnly = True
            Exit Sub
        End If

        If processTransactionResponse.Success = True Then
            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Insert Transaction"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = "Account Number: " & txtAccNum.Text & " Trans. Type: " & cboTransactionType.Text & " Amount: " & processTransactionRequest.TransactionAmount
            _Log.UserComment = txtNotes.Text
            _Log.AccountNumber = txtAccNum.Text
            _Log.UserName = Session("username")
            _Log.WebPage = "Manual Transactions"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================


            ClearFields()
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Your AuthCode: " & processTransactionResponse.AuthCode
            dxPopUpError.ShowOnPageLoad = True
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = processTransactionResponse.ReturnMessage
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub

    Private Sub ClearFields()
        txtCardNum.Text = ""
        txtAccNum.Text = ""
        txtCustomer.Text = ""
        txtID.Text = ""
        txtShopA.Text = ""
        cboBranch.Text = ""
        txtAmount.Text = ""
        cboTransactionType.Text = ""
        cboPayType.Text = ""
        txtNotes.Text = ""

        chkPayDeposit.Visible = False
        lblPayDeposit.Visible = False

        txtCardNum.ReadOnly = False

        txtShopA.ReadOnly = False
        cboBranch.ReadOnly = False
        txtAmount.ReadOnly = False
        cboTransactionType.ReadOnly = False

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

        'Dim _eDt As DataTable

        'grdDebtorsSearch.DataSource = _eDt
        'grdDebtorsSearch.DataBind()


    End Sub
End Class