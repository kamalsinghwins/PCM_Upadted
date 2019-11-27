﻿Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress
Imports DevExpress.Web
Imports System.Threading
Imports System.Web.Services

Public Class investigations
    Inherits System.Web.UI.Page

    Dim _NewDebtor As New Debtor

    Dim _AgeAnalysis As List(Of Debtor_AgeAnalysis)
    Dim _ContactHistory As List(Of Debtor_ContactHistory)
    Dim _DebtorChanges As List(Of DebtorChangeLog)
    Dim _Transactions As List(Of Transactions)
    Dim _ClosingBalances As List(Of Debtor_ClosingBalances)
    Dim _PaymentPlans As List(Of Debtor_PaymentPlan)

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "GetDebtor" Then
            ClearForm(False)
            GetDebtor(txtAccountNumber.Text)

        End If

        If hdWhichButton.Value = "NewBureau" Then
            'Query bureau
            Dim _NewContact As New ContactBusinessLayer

            _NewContact.UpdateBureauData(Session("username"), txtAccountNumber.Text)

            'Rerun debtor
            GetDebtor(txtAccountNumber.Text)
            Exit Sub
        End If


        If hdWhichButton.Value = "BankingDetails" Then
            Dim _dlDebtors As New ContactBusinessLayer
            Dim message As String = "Rage Banking Details - Bank: Absa. Acc Name: Rage Distribution. Acc Num: 408 1269 231. " & _
                                    "Branch: 632 005. Reference: " & txtAccountNumber.Text
            _dlDebtors.SendSMS(txtAccountNumber.Text, txtContactNumber.Text, message, Session("username"), "Banking Details")
            '_dlDebtors.SendSMS(txtAccountNumber.Text, "082-771-2322", message, Session("username"), "Banking Details")
            lblStatus.Text = "SMS Sent!"

            dxPopUp.ShowOnPageLoad = True

            Exit Sub
        End If

        If hdWhichButton.Value = "Save" Or hdWhichButton.Value = "Next" Then
            Dim sIPAddress As String

            sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")

            Dim _ContactLog As New ContactInvestigationResult

            If cboPreferredLanguage.Text <> hdPreferredLanguage.Value Then
                _ContactLog.PreferredLanguage = cboPreferredLanguage.Text
            End If

            _ContactLog.AccountNumber = txtAccountNumber.Text
            _ContactLog.ActionResult = cboResult.Text
            _ContactLog.ContactNotes = txtNotes.Text

            If cboResult.Text = "PTP" Then
                _ContactLog.PTPAmount = Format(Val(txtPTPAmount.Text), "0.00")
                _ContactLog.PTPDate = txtPTPDate.Text & " 09:00:00"
            Else
                _ContactLog.PTPAmount = "0.00"
                _ContactLog.PTPDate = ""
            End If

            If hdTimeDataServed.Value <> "" Then
                _ContactLog.TimeRecordServed = hdTimeDataServed.Value
            End If

            'Have to deal with the strange return of the mask
            If txtContactNumber.Text = "   -   -" Then
                _ContactLog.ContactNumber = hdContactNumber.Value
            Else
                _ContactLog.ContactNumber = txtContactNumber.Text
            End If

            If txtAltNumber.Text = "   -   -" Then
                _ContactLog.AltNumber = hdAltNumber.Value
            Else
                _ContactLog.AltNumber = txtAltNumber.Text
            End If

            If txtHomeNumber1.Text = "   -   -" Then
                _ContactLog.HomeNumber1 = hdHomeNumber1.Value
            Else
                _ContactLog.HomeNumber1 = txtHomeNumber1.Text
            End If

            If txtHomeNumber2.Text = "   -   -" Then
                _ContactLog.HomeNumber2 = hdHomeNumber2.Value
            Else
                _ContactLog.HomeNumber2 = txtHomeNumber2.Text
            End If

            If txtNextOfKinNumber.Text = "   -   -" Then
                _ContactLog.NextOfKinNumber = hdNextOfKinNumber.Value
            Else
                _ContactLog.NextOfKinNumber = txtNextOfKinNumber.Text
            End If

            If txtWorkNumber.Text = "   -   -" Then
                _ContactLog.WorkNumber = hdWorkNumber.Value
            Else
                _ContactLog.WorkNumber = txtWorkNumber.Text
            End If

            If txtSpouseContactNumber.Text = "   -   -" Then
                _ContactLog.SpouseContactNumber = hdSpouseContactNumber.Value
            Else
                _ContactLog.SpouseContactNumber = txtSpouseContactNumber.Text
            End If

            _ContactLog.NextOfKin = txtNextOfKin.Text
            _ContactLog.SendPromos = chkSendPromos.Checked

            _ContactLog.OriginalContactNumber = hdContactNumber.Value
            _ContactLog.OriginalAltNumber = hdAltNumber.Value
            _ContactLog.OriginalHomeNumber1 = hdHomeNumber1.Value
            _ContactLog.OriginalHomeNumber2 = hdHomeNumber2.Value
            _ContactLog.OriginalNextOfKin = hdNextOfKin.Value
            _ContactLog.OriginalNextOfKinNumber = hdNextOfKinNumber.Value
            _ContactLog.OriginalWorkNumber = hdWorkNumber.Value
            _ContactLog.OriginalSpouseContactNumber = hdSpouseContactNumber.Value
            _ContactLog.OriginalSendPromos = hdSendPromos.Value

            Dim _NewContact As New ContactBusinessLayer
            If _NewContact.InvestigationUpdate(Session("username"), _ContactLog, sIPAddress) Then

                If hdWhichButton.Value = "Next" Then
                    ClearForm(True)

                    Dim _dlNextDebtor As New NextDebtorBusinessLayer

                    Dim tmpDebtor As New Debtor
                    tmpDebtor = _dlNextDebtor.ReturnNextInvestigationDebtor()

                    If Not IsNothing(tmpDebtor) Then
                        GetDebtor(tmpDebtor.AccountNumber)
                    End If

                Else
                    lblStatus.Text = "Account Updated!"

                    dxPopUp.ShowOnPageLoad = True

                    ClearForm(True)
                End If

                'lblStatus.Text = "Account Updated!"

                'dxPopUp.ShowOnPageLoad = True

                'ClearForm(True)

            Else
                dxPopUpError.ShowOnPageLoad = True
            End If
        End If

    End Sub

    Protected Sub ASPxNavBar2_ItemClick(source As Object, e As DevExpress.Web.NavBarItemEventArgs) Handles ASPxNavBar2.ItemClick

        Dim _dlNextDebtor As New NextDebtorBusinessLayer

        Dim tmpDebtor As New Debtor
        tmpDebtor = _dlNextDebtor.ReturnNextInvestigationDebtor()

        If Not IsNothing(tmpDebtor) Then
            GetDebtor(tmpDebtor.AccountNumber)
            'GetDebtor("100014641") 'No payments plans
        End If

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
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.Investigations) Then
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
            hdUsername.Value = Session("username")

            txtPTPDate.Text = Format(Now, "yyyy-MM-dd")
            txtPTPDate.MinDate = Format(Now, "yyyy-MM-dd")

            cboResult.Items.Add("")
            'cboResult.Items.Add("Primary Number updated successfully")
            'cboResult.Items.Add("Unable to obtain valid contact number")
            'cboResult.Items.Add("Original Primary Number was correct")
            cboResult.Items.Add("Number does not exist / Incorrect number")
            cboResult.Items.Add("Straight to Voice mail")
            cboResult.Items.Add("No answer / Engaged / Call Back Later")
            cboResult.Items.Add("PTP")
            cboResult.Items.Add("Refuses to Pay")
            cboResult.Items.Add("Fraud")
            cboResult.Items.Add("Debt Review")
            cboResult.Items.Add("Customer Absconded") 'LEGAL
            cboResult.Items.Add("Issue Resolved")

            cboPreferredLanguage.Items.Add("")
            cboPreferredLanguage.Items.Add("Zulu")
            cboPreferredLanguage.Items.Add("Xhosa")
            cboPreferredLanguage.Items.Add("Afrikaans")
            cboPreferredLanguage.Items.Add("English")
            cboPreferredLanguage.Items.Add("Sepedi")
            cboPreferredLanguage.Items.Add("Setswana")
            cboPreferredLanguage.Items.Add("Sotho")
            cboPreferredLanguage.Items.Add("Tsonga")
            cboPreferredLanguage.Items.Add("Swati")
            cboPreferredLanguage.Items.Add("Venda")
            cboPreferredLanguage.Items.Add("Ndebele")

            'If Session("collection_period") <> "" Then

            '    Dim _dlNextDebtor As New NextDebtorBusinessLayer

            '    Dim tmpDebtor As New Debtor
            '    tmpDebtor = _dlNextDebtor.ReturnNextDebtor(Session("collection_period"))

            '    If Not IsNothing(tmpDebtor) Then
            '        GetDebtor(tmpDebtor.AccountNumber)
            '    End If
            'End If

            'txtPTPDate.MinDate = Format(Now, "yyyy-MM-dd")

        End If

    End Sub

    Private Sub ClearForm(ByVal ClearAccountNumber As Boolean)

        If ClearAccountNumber = True Then
            txtAccountNumber.Text = ""
        End If
        txtBalance.Text = ""
        txtContactNumber.Text = ""
        txtCreditLimit.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtLastPaymentAmount.Text = ""
        txtLastPaymentDate.Text = ""
        txtLastSaleAmount.Text = ""
        txtLastSaleDate.Text = ""
        txtOverdue.Text = ""
        txtStatus.Text = ""

        cboResult.Text = ""

        txtContactNumber.Text = ""
        txtAltNumber.Text = ""
        txtHomeNumber1.Text = ""
        txtHomeNumber2.Text = ""
        txtNextOfKin.Text = ""
        txtNextOfKinNumber.Text = ""
        txtSpouseContactNumber.Text = ""
        txtWorkNumber.Text = ""

        txtConsumerRating.Text = ""

        chkSendPromos.Checked = False

        hdContactNumber.Value = ""
        hdAltNumber.Value = ""
        hdHomeNumber1.Value = ""
        hdHomeNumber2.Value = ""
        hdNextOfKin.Value = ""
        hdNextOfKinNumber.Value = ""
        hdSpouseContactNumber.Value = ""
        hdWorkNumber.Value = ""
        hdAltNumber.Value = ""

        hdSendPromos.Value = False

        lblContactLevel.Text = ""
        lblNextDate.Text = ""

        Session.Remove("_Transactions")
        Session.Remove("_ClosingBalances")
        Session.Remove("_AgeAnalysis")
        Session.Remove("_PaymentPlans")
        Session.Remove("_ContactHistory")
        Session.Remove("_DebtorChanges")

        cboPreferredLanguage.Text = ""

        hdPreferredLanguage.Value = ""

        cboResult.Text = ""

        cmdBankingDetails.Enabled = False
        cmdNewBureau.Enabled = False

        'grdAgeAnalysis.DataSource = Nothing
        'grdAgeAnalysis.DataBind()

    End Sub

    Private Sub GetDebtor()

        If Session("current_debtor") = "" Then
            Exit Sub
        End If

        Session.Remove("_AlternativeNumbers")
        Session.Remove("_Transactions")
        Session.Remove("_ClosingBalances")
        Session.Remove("_AgeAnalysis")
        Session.Remove("_PaymentPlans")
        Session.Remove("_ContactHistory")
        Session.Remove("_DebtorChanges")

        Dim _NewGetDebtor As DebtorsBusinessLayer = New DebtorsBusinessLayer

        _NewDebtor = _NewGetDebtor.GetDebtor(Session("current_debtor"))

        If Not IsNothing(_NewDebtor) Then
            Session("_AlternativeNumbers") = _NewDebtor.AlternativeContactNumbers
            Session("_Transactions") = _NewDebtor.Transactions
            Session("_ClosingBalances") = _NewDebtor.ClosingBalances
            Session("_AgeAnalysis") = _NewDebtor.AgeAnalysis
            Session("_PaymentPlans") = _NewDebtor.PaymentPlans
            Session("_ContactHistory") = _NewDebtor.ContactHistory
            Session("_DebtorChanges") = _NewDebtor.ChangeHistory

            '_Transactions = _NewDebtor.Transactions
            '_ClosingBalances = _NewDebtor.ClosingBalances
            '_AgeAnalysis = _NewDebtor.AgeAnalysis
            '_PaymentPlans = _NewDebtor.PaymentPlans
            '_ContactHistory = _NewDebtor.ContactHistory
            '_DebtorChanges = _NewDebtor.ChangeHistory

            'This GetDebtor only gets called for the gridview refresh
            'hdTimeDataServed.Value = Format(Now, "yyyy-MM-dd HH:mm:ss")

            grdAgeAnalysis.DataBind()

            Dim tmpGrd As ASPxGridView
            tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("Transactions").Items(0).FindControl("grdTransactions")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("PaymentPlans").Items(0).FindControl("grdPaymentPlans")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ChangeHistory").Items(0).FindControl("grdAccountHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("AlternativeNumbers").Items(0).FindControl("grdAlternativeNumbers")
            tmpGrd.DataBind()

            hdPreferredLanguage.Value = _NewDebtor.PreferredLanguage
            cboPreferredLanguage.Text = _NewDebtor.PreferredLanguage

            cmdBankingDetails.Enabled = True
            cmdNewBureau.Enabled = True

        End If

    End Sub

    Private Sub GetDebtor(ByVal AccountNumber As String)

        If AccountNumber = "" Then
            Exit Sub
        End If

        Session("current_debtor") = AccountNumber

        Session.Remove("_AlternativeNumbers")
        Session.Remove("_Transactions")
        Session.Remove("_ClosingBalances")
        Session.Remove("_AgeAnalysis")
        Session.Remove("_PaymentPlans")
        Session.Remove("_ContactHistory")
        Session.Remove("_DebtorChanges")

        Dim _NewGetDebtor As DebtorsBusinessLayer = New DebtorsBusinessLayer

        _NewDebtor = _NewGetDebtor.GetDebtorInvestigation(AccountNumber)

        If Not IsNothing(_NewDebtor) Then
            Session("_Transactions") = _NewDebtor.Transactions
            Session("_ClosingBalances") = _NewDebtor.ClosingBalances
            Session("_AgeAnalysis") = _NewDebtor.AgeAnalysis
            Session("_PaymentPlans") = _NewDebtor.PaymentPlans
            Session("_ContactHistory") = _NewDebtor.ContactHistory
            Session("_DebtorChanges") = _NewDebtor.ChangeHistory
            Session("_AlternativeNumbers") = _NewDebtor.AlternativeContactNumbers

            '_Transactions = _NewDebtor.Transactions
            '_ClosingBalances = _NewDebtor.ClosingBalances
            '_AgeAnalysis = _NewDebtor.AgeAnalysis
            '_PaymentPlans = _NewDebtor.PaymentPlans
            '_ContactHistory = _NewDebtor.ContactHistory
            '_DebtorChanges = _NewDebtor.ChangeHistory

            txtAccountNumber.Text = _NewDebtor.AccountNumber
            txtBalance.Text = _NewDebtor.Balance
            txtContactNumber.Text = _NewDebtor.ContactNumber
            txtCreditLimit.Text = _NewDebtor.CreditLimit
            txtFirstName.Text = _NewDebtor.FirstName
            txtLastName.Text = _NewDebtor.LastName
            txtLastPaymentAmount.Text = _NewDebtor.LastPaymentAmount
            txtLastPaymentDate.Text = _NewDebtor.LastPaymentDate
            txtLastSaleAmount.Text = _NewDebtor.LastSaleAmount
            txtLastSaleDate.Text = _NewDebtor.LastSaleDate
            txtOverdue.Text = _NewDebtor.Overdue
            txtStatus.Text = _NewDebtor.CurrentStatus

            txtConsumerRating.Text = _NewDebtor.ConsumerRating

            txtContactNumber.Text = _NewDebtor.ContactNumber
            txtAltNumber.Text = _NewDebtor.AltNumber
            txtHomeNumber1.Text = _NewDebtor.HomeNumber1
            txtHomeNumber2.Text = _NewDebtor.HomeNumber2
            txtNextOfKin.Text = _NewDebtor.NextOfKin
            txtNextOfKinNumber.Text = _NewDebtor.NextOfKinNumber
            txtSpouseContactNumber.Text = _NewDebtor.SpouseContactNumber
            txtWorkNumber.Text = _NewDebtor.WorkNumber

            chkSendPromos.Checked = _NewDebtor.SendPromos

            hdContactNumber.Value = _NewDebtor.ContactNumber
            hdAltNumber.Value = _NewDebtor.AltNumber
            hdHomeNumber1.Value = _NewDebtor.HomeNumber1
            hdHomeNumber2.Value = _NewDebtor.HomeNumber2
            hdNextOfKin.Value = _NewDebtor.NextOfKin
            hdNextOfKinNumber.Value = _NewDebtor.NextOfKinNumber
            hdSpouseContactNumber.Value = _NewDebtor.SpouseContactNumber
            hdWorkNumber.Value = _NewDebtor.WorkNumber
            hdStatus.Value = _NewDebtor.CurrentStatus

            hdSendPromos.Value = _NewDebtor.SendPromos

            lblContactLevel.Text = _NewDebtor.CurrentContactLevel
            lblNextDate.Text = _NewDebtor.NextContactTime

            hdTimeDataServed.Value = Format(Now, "yyyy-MM-dd HH:mm:ss")

            hdPreferredLanguage.Value = _NewDebtor.PreferredLanguage
            cboPreferredLanguage.Text = _NewDebtor.PreferredLanguage

            grdAgeAnalysis.DataBind()

            Dim tmpGrd As ASPxGridView
            tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("Transactions").Items(0).FindControl("grdTransactions")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("PaymentPlans").Items(0).FindControl("grdPaymentPlans")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("ChangeHistory").Items(0).FindControl("grdAccountHistory")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("AlternativeNumbers").Items(0).FindControl("grdAlternativeNumbers")
            tmpGrd.DataBind()

            cmdBankingDetails.Enabled = True
            cmdNewBureau.Enabled = True

        End If

    End Sub

    Protected Sub cmdGetDebtor_Click(sender As Object, e As EventArgs) Handles cmdGetDebtor.Click
        GetDebtor(txtAccountNumber.Text)
    End Sub

    Protected Sub gridAgeAnalysis_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        ' Assign the data source in grid_DataBinding
        If IsNothing(Session("_AgeAnalysis")) Then
            Exit Sub
        End If

        grdAgeAnalysis.DataSource = Session("_AgeAnalysis")

        '' Assign the data source in grid_DataBinding
        'If IsNothing(_AgeAnalysis) Then
        '    Exit Sub
        'End If

        'grdAgeAnalysis.DataSource = _AgeAnalysis

    End Sub


    Protected Sub gridTransactions_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding

        If IsNothing(Session("_Transactions")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("Transactions").Items(0).FindControl("grdTransactions")
        tmpGrd.DataSource = Session("_Transactions")

        'If IsNothing(_Transactions) Then
        '    Exit Sub
        'End If

        'Dim tmpGrd As ASPxGridView

        'tmpGrd = ASPxNavBar1.Groups.FindByName("Transactions").Items(0).FindControl("grdTransactions")
        'tmpGrd.DataSource = _Transactions

        'grdTransactions.DataSource = _Transactions

    End Sub

    Protected Sub gridAlternativeNumbers_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        ' Assign the data source in grid_DataBinding

        If IsNothing(Session("_AlternativeNumbers")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("AlternativeNumbers").Items(0).FindControl("grdAlternativeNumbers")
        'tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
        tmpGrd.DataSource = Session("_AlternativeNumbers")

        '' Assign the data source in grid_DataBinding

        'If IsNothing(_AgeAnalysis) Then
        '    Exit Sub
        'End If

        'Dim tmpGrd As ASPxGridView

        'tmpGrd = ASPxNavBar1.Groups.FindByName("ChangeHistory").Items(0).FindControl("grdAccountHistory")
        'tmpGrd.DataSource = _DebtorChanges


    End Sub

    Protected Sub gridClosingBalances_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        ' Assign the data source in grid_DataBinding
        If IsNothing(Session("_ClosingBalances")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")
        tmpGrd.DataSource = Session("_ClosingBalances")


        '' Assign the data source in grid_DataBinding
        'If IsNothing(_ClosingBalances) Then
        '    Exit Sub
        'End If

        'Dim tmpGrd As ASPxGridView

        'tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")
        'tmpGrd.DataSource = _ClosingBalances

        ''grdClosingBalances.DataSource = _ClosingBalances

    End Sub

    Protected Sub gridPayments_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        ' Assign the data source in grid_DataBinding
        If IsNothing(Session("_PaymentPlans")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("PaymentPlans").Items(0).FindControl("grdPaymentPlans")
        tmpGrd.DataSource = Session("_PaymentPlans")


        ''grdPaymentPlans.DataSource = _PaymentPlans

        '' Assign the data source in grid_DataBinding
        'If IsNothing(_PaymentPlans) Then
        '    Exit Sub
        'End If

        'Dim tmpGrd As ASPxGridView

        'tmpGrd = ASPxNavBar1.Groups.FindByName("PaymentPlans").Items(0).FindControl("grdPaymentPlans")
        'tmpGrd.DataSource = _PaymentPlans


        ''grdPaymentPlans.DataSource = _PaymentPlans

    End Sub

    Protected Sub gridAccountHistory_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding

        If IsNothing(Session("_DebtorChanges")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("ChangeHistory").Items(0).FindControl("grdAccountHistory")
        tmpGrd.DataSource = Session("_DebtorChanges")


    End Sub

    Protected Sub gridHistory_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        ' Assign the data source in grid_DataBinding

        If IsNothing(Session("_ContactHistory")) Then
            Exit Sub
        End If

        Dim tmpGrd As ASPxGridView

        tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
        tmpGrd.DataSource = Session("_ContactHistory")

        ''grdTransactions.DataSource = _Transactions
        '' Assign the data source in grid_DataBinding

        ''Checking the incorrect datasource as if the debtor had not been previously contacted, this would
        ''could the contact datasource to be nothing and result in a never ending loop
        'If IsNothing(_AgeAnalysis) Then
        '    Exit Sub
        'End If

        'Dim tmpGrd As ASPxGridView

        'tmpGrd = ASPxNavBar1.Groups.FindByName("ContactHistory").Items(0).FindControl("grdHistory")
        'tmpGrd.DataSource = _ContactHistory


    End Sub

End Class