Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports pcm.DataLayer

Public Class CardAssign
    Inherits System.Web.UI.Page

    Dim _DebtorsBusinessLayer As DebtorsBusinessLayer = New DebtorsBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

    Dim RG As New Utilities.clsUtil

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("AccountInfo") = Nothing
            InitialiseValues()
        End If

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.PCMCardAllocations) Then
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
            '_Log.WebPage = "Assign New Cards"

            '_Logging.WriteToLogPCM(_Log)
            ''=====================================================================================================
        End If

    End Sub

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "AutoIncrease" Then
            CheckedChanged()
        End If


        If hdWhichButton.Value = "Accept" Then
            cmdAccept_Click()
        End If

        If hdWhichButton.Value = "CheckID" Then
            txtIDNumber_KeyUp()
        End If

        'If hdWhichButton.Value = "CardChanged" Then
        '    txtCardNumber_TextChanged()
        'End If


        If hdWhichButton.Value = "clear" Then
            ClearScreen()

        End If

    End Sub

    Private Sub InitialiseValues()
        cboLanguage.Items.Add("")
        cboLanguage.Items.Add("Zulu")
        cboLanguage.Items.Add("Xhosa")
        cboLanguage.Items.Add("Afrikaans")
        cboLanguage.Items.Add("English")
        cboLanguage.Items.Add("Sepedi")
        cboLanguage.Items.Add("Setswana")
        cboLanguage.Items.Add("Sotho")
        cboLanguage.Items.Add("Tsonga")
        cboLanguage.Items.Add("Swati")
        cboLanguage.Items.Add("Venda")
        cboLanguage.Items.Add("Ndebele")

        lblOverRide.Visible = True
        txtCardNumber.Text = ""
        ''txtCardNumber.PasswordChar = ""
        chkAutoIncrease.Checked = True

        Dim _branchListDS As DataSet
        _branchListDS = _DebtorsBusinessLayer.GetBranchList()

        If Not _branchListDS Is Nothing Then
            For Each dr As DataRow In _branchListDS.Tables(0).Rows
                cboBranch.Items.Add(dr("branch_code") & " - " & dr("branch_name"))
            Next
        End If
    End Sub

    Private Sub ClearScreen()

        Session.Remove("AccountInfo")

        txtIDNumber.Text = ""
        txtCellphone.Text = ""
        txtFirstName.Text = ""
        txtSurname.Text = ""
        chkAutoIncrease.Checked = True
        chkLostCard.Checked = False
        txtCellphone.ReadOnly = True
        txtFirstName.ReadOnly = True
        txtSurname.ReadOnly = True
        chkAutoIncrease.Enabled = False
        chkLostCard.Enabled = False
        txtCardNumber.ReadOnly = True
        txtEmployeeNumber.ReadOnly = True

        lblOverRide.Visible = False
        'txtCardNumber.PasswordChar = "*"

        txtCardNumber.Text = ""
        txtEmployeeNumber.Text = ""

        'AccountNumber = ""
        'FirstName = ""
        'Surname = ""
        'CellPhone = ""
        'CreditLimit = 0

        txtStatus.Text = ""
        txtCreditLimit.Text = ""

        cboLanguage.Text = ""

        lbllostCardCharge.Text = ""

        lblActivate.Visible = False

        txtIDNumber.ReadOnly = False
        txtStatus.ReadOnly = False
        txtCreditLimit.ReadOnly = False

        cboBranch.Text = ""

        lblLostCardCharge.Visible = False
        txtLostCardCharge.Text = "0"
        txtLostCardCharge.Visible = False

        txtIDNumber.Focus()

    End Sub

    Private Sub CheckedChanged()
        If chkAutoIncrease.Checked = False Then
            'If MsgBox("If you take off this tick the Customer will NOT GET AN AUTOMATIC INCREASE!!!" & vbCrLf & vbCrLf &
            '          "DOES THE CUSTOMER WANT AN AUTO-INCREASE?", MsgBoxStyle.Critical + vbYesNo, "WARNING!!!") = MsgBoxResult.Yes Then
            chkAutoIncrease.Checked = True
        Else
            chkAutoIncrease.Checked = False
            'End If

        End If
    End Sub

    Private Sub txtIDNumber_KeyUp()

        Dim selfactivate As Boolean = True

        If Len(txtIDNumber.Text) = 13 Then

            If RG.ValidID(txtIDNumber.Text) = False Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Invalid ID Number"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Get Debtor Account"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = "ID Number: " & txtIDNumber.Text
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Assign New Cards"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================

            Dim result As Debtor

            result = _DebtorsBusinessLayer.GetSelfActivateDetails(txtIDNumber.Text)

            Dim account As New Debtor

            account.AccountNumber = result.AccountNumber
            account.FirstName = result.FirstName.ToString.ToUpper
            account.LastName = result.LastName.ToString.ToUpper
            account.ContactNumber = result.ContactNumber
            account.CreditLimit = result.CreditLimit

            If Val(result.PayNewCard) <> 0 Then
                lblLostCardCharge.Visible = True
                txtLostCardCharge.Text = result.PayNewCard
                txtLostCardCharge.Visible = True
            End If

            Session("AccountInfo") = account

            txtStatus.Text = result.CurrentStatus
            txtCreditLimit.Text = result.CreditLimit
            'lbllostCardCharge.Text = result.PayNewCard

            txtIDNumber.ReadOnly = True
            txtStatus.ReadOnly = True
            txtCreditLimit.ReadOnly = True

            txtCellphone.ReadOnly = False
            txtFirstName.ReadOnly = False
            txtSurname.ReadOnly = False
            chkAutoIncrease.Enabled = True
            chkLostCard.Enabled = True
            txtCardNumber.ReadOnly = False
            txtEmployeeNumber.ReadOnly = False

            txtFirstName.Focus()

            If result.SelfActivate = False Then
                If result.ReturnMessage <> "" Then
                    txtIDNumber.ReadOnly = False
                    txtStatus.ReadOnly = False
                    txtCreditLimit.ReadOnly = False

                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = result.ReturnMessage
                    dxPopUpError.ShowOnPageLoad = True
                End If
            End If

            If Val(result.PayNewCard) = 0 Then
                If result.SelfActivate = False Then
                    cmdClear.Enabled = True
                    lblActivate.Visible = True
                Else

                    lblActivate.Visible = False
                End If
            Else
                'lbllostCardCharge.Text = Val(result.PayNewCard)
                dxPopUpError.HeaderText = "Error"
                lblError.Text = result.ReturnMessage
                dxPopUpError.ShowOnPageLoad = True
                lblActivate.Visible = False
            End If

        End If

    End Sub

    Private Sub cmdAccept_Click()
        If Session("AccountInfo") IsNot Nothing Then
            Dim account As New Debtor
            account = Session("AccountInfo")

            Dim strCardNumber As String
            Dim DebtorDetails As New Debtor

            If txtCardNumber.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please fill-in a Card Number."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtEmployeeNumber.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please fill-in the Employee Number."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            strCardNumber = txtCardNumber.Text

            If Mid(txtCardNumber.Text, 1, 4) <> "6501" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "The Card Number Is Invalid."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If


            If txtFirstName.Text.ToUpper <> account.FirstName.ToUpper Then
                dxPopUpError.HeaderText = "Positive"
                lblError.Text = "The First Name you entered does not match the details for the ID Number."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If cboLanguage.Text = "" Then
                dxPopUpError.HeaderText = "Please correct"
                lblError.Text = "Please select a Preferred Language for the Customer."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtSurname.Text.ToUpper <> account.LastName.ToUpper Then
                dxPopUpError.HeaderText = "Please correct"
                lblError.Text = "The Surname you entered does not match the details for the ID Number."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtCellphone.Text <> account.ContactNumber Then
                dxPopUpError.HeaderText = "Please correct"
                lblError.Text = "The Cellphone Number you entered does not match the details for the ID Number."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            '        If MsgBox("I confirm that the customer has presented to me their ID Book or Drivers License.", MsgBoxStyle.Question + MsgBoxStyle.YesNo,
            '              "Please Confirm") = MsgBoxResult.No Then
            '            Exit Sub
            '        End If

            'If Val(lbllostCardCharge.Text) <> 0 Then
            '    If MsgBox("I confirm that the customer acknowledges that they will be charged R" & Val(lbllostCardCharge.Text) &
            '          " for their new card.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo,
            '      "Please Confirm") = MsgBoxResult.No Then
            '        Exit Sub
            '    End If
            'End If

            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Assigning Card"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = RG.Apos(account.AccountNumber)
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Assign New Cards"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================



            Dim Current_Branch_Code As String = Mid$(cboBranch.Value, 1, 3)

            DebtorDetails.FirstName = account.FirstName.ToString.ToUpper
            DebtorDetails.LastName = account.LastName.ToString.ToUpper
            DebtorDetails.AccountNumber = RG.Apos(account.AccountNumber)
            DebtorDetails.CardNumber = RG.Apos(txtCardNumber.Text)
            DebtorDetails.EmployeeNumber = RG.Apos(txtEmployeeNumber.Text)
            DebtorDetails.Autoincrease = chkAutoIncrease.Checked
            DebtorDetails.LostCard = chkLostCard.Checked
            DebtorDetails.ContactNumber = txtCellphone.Text
            DebtorDetails.BranchCode = Current_Branch_Code
            DebtorDetails.PreferredLanguage = cboLanguage.Text
            DebtorDetails.PayNewCard = Val(txtLostCardCharge.Text)

            Dim Result As Debtor

            Result = _DebtorsBusinessLayer.InsertSelfActivated(DebtorDetails)

            If Result.SelfActivate = False Then
                If Result.ReturnMessage <> "" Then
                    lblError.Text = Result.ReturnMessage
                    dxPopUpError.ShowOnPageLoad = True
                    cmdClear.Enabled = True
                    cmdAccept.Enabled = True
                Else
                    cmdClear.Enabled = True
                    lblError.Text = Result.ReturnMessage
                    dxPopUpError.ShowOnPageLoad = True
                    cmdAccept.Enabled = True
                    lblActivate.Visible = True
                End If
            Else
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "The card is Active. The Customer has a CREDIT LIMIT OF R" & txtCreditLimit.Text
                dxPopUpError.ShowOnPageLoad = True
                ClearScreen()

            End If
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter an ID number before submitting"
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub
End Class