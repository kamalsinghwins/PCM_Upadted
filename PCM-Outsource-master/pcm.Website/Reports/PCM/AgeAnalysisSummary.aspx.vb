Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class frmAgeAnalysisSummary
    Inherits System.Web.UI.Page
    Dim _AgeAnalysis As PCMReportingBusinessLayer = New PCMReportingBusinessLayer

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AgeAnalysisSummary) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            InitialiseValues()
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "opt1" Then
            opt1_Click()
        End If

        If hdWhichButton.Value = "opt2" Then
            opt2_Click()
        End If

        If hdWhichButton.Value = "chkAll" Then
            chkAll_CheckedChanged()
        End If

        If hdWhichButton.Value = "Ok" Then
            cmdOk_Click()
            checkOption()
        End If

        If hdWhichButton.Value = "checkOption" Then
            checkOption()
        End If
    End Sub
    Private Sub InitialiseValues()
        cboOther.Items.Clear()
        cboOther.Items.Add("ALL")
        cboOther.Items.Add("BLOCKED")
        cboOther.Items.Add("DEBT REVIEW")
        cboOther.Items.Add("DECEASED")
        cboOther.Items.Add("DECLINED")
        cboOther.Items.Add("FRAUD")
        cboOther.Items.Add("LEGAL")
        cboOther.Items.Add("PENDING")
        cboOther.Items.Add("SUSPENDED")
        cboOther.Items.Add("WRITE-OFF")
        cboOther.Text = ""

        cboCurrent.Items.Add(">")
        cboCurrent.Items.Add("=")
        cboCurrent.Items.Add("<")
        cboCurrent.Items.Add(">")
        cboCurrent.Items.Add("<>")
        cboCurrent.Items.Add(">=")
        cboCurrent.Items.Add("<=")
        cboCurrent.Text = ""

        cbo30Days.Items.Add(">")
        cbo30Days.Items.Add("=")
        cbo30Days.Items.Add("<")
        cbo30Days.Items.Add(">")
        cbo30Days.Items.Add("<>")
        cbo30Days.Items.Add(">=")
        cbo30Days.Items.Add("<=")
        cbo30Days.Text = ""

        cbo60Days.Items.Add(">")
        cbo60Days.Items.Add("=")
        cbo60Days.Items.Add("<")
        cbo60Days.Items.Add(">")
        cbo60Days.Items.Add("<>")
        cbo60Days.Items.Add(">=")
        cbo60Days.Items.Add("<=")
        cbo60Days.Text = ""

        cbo90Days.Items.Add(">")
        cbo90Days.Items.Add("=")
        cbo90Days.Items.Add("<")
        cbo90Days.Items.Add(">")
        cbo90Days.Items.Add("<>")
        cbo90Days.Items.Add(">=")
        cbo90Days.Items.Add("<=")
        cbo90Days.Text = ""

        cbo120Days.Items.Add(">")
        cbo120Days.Items.Add("=")
        cbo120Days.Items.Add("<")
        cbo120Days.Items.Add(">")
        cbo120Days.Items.Add("<>")
        cbo120Days.Items.Add(">=")
        cbo120Days.Items.Add("<=")
        cbo120Days.Text = ""

        cbo150Days.Items.Add(">")
        cbo150Days.Items.Add("=")
        cbo150Days.Items.Add("<")
        cbo150Days.Items.Add(">")
        cbo150Days.Items.Add("<>")
        cbo150Days.Items.Add(">=")
        cbo150Days.Items.Add("<=")
        cbo150Days.Text = ""

        cboTotal.Items.Add(">")
        cboTotal.Items.Add("=")
        cboTotal.Items.Add("<")
        cboTotal.Items.Add(">")
        cboTotal.Items.Add("<>")
        cboTotal.Items.Add(">=")
        cboTotal.Items.Add("<=")
        cboTotal.Items.Add("BETWEEN")
        cboTotal.Text = ""
        txtToTotal.Visible = False

        'txtFrom.ClientEnabled = False
        'txtTo.ClientEnabled = False
        'txtFrom.Text = "ALL"
        'txtTo.Text = "ALL"
        'chkAll.ClientEnabled = False
        'chkAll.Checked = True

        txtStart.Text = Format(Now, "yyyy-MM-dd")
        txtEnd.Text = Format(Now, "yyyy-MM-dd")
        txtLastTransactionStartDate.Text = Format(Now, "yyyy-MM-dd")
        txtLastTransactionEndDate.Text = Format(Now, "yyyy-MM-dd")
        chkOpenedBetween.Checked = False
        chkUseLastTransaction.Checked = False
        opt2.Checked = False
    End Sub
    Private Sub opt1_Click()
        opt2.Checked = False
        cboOther.ClientEnabled = False
        cboOther.Text = ""
    End Sub
    Private Sub opt2_Click()
        opt1.Checked = False
        cboOther.ClientEnabled = True
        opt2.Checked = True
    End Sub
    Private Sub chkAll_CheckedChanged()
        'If chkAll.Checked = True Then
        '    txtFrom.ClientEnabled = False
        '    txtTo.ClientEnabled = False
        '    txtFrom.Text = "ALL"
        '    txtTo.Text = "ALL"
        'ElseIf chkAll.Checked = False Then
        '    txtFrom.ClientEnabled = True
        '    txtTo.ClientEnabled = True
        '    txtFrom.Text = ""
        '    txtTo.Text = ""
        'End If
    End Sub
    Protected Sub cmdOk_Click()

        If opt1.Checked = False And opt2.Checked = False Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Status option."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If opt2.Checked = True And cboOther.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select an option for Other Status"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If chkUseCurrent.Checked = True Then
            If cboCurrent.Value = "" Or txtWhereCurrent.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkUse30.Checked = True Then
            If cbo30Days.Value = "" Or txtWhere30Days.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkUse60.Checked = True Then
            If cbo60Days.Value = "" Or txtWhere60Days.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkuse90.Checked = True Then
            If cbo90Days.Value = "" Or txtWhere90Days.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
            End If
        End If

        If chkUse120.Checked = True Then
            If cbo120Days.Value = "" Or _txtWhere120Days.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkUse150.Checked = True Then
            If cbo150Days.Value = "" Or txtWhere150Days.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkUseTotal.Checked = True Then
            If cboTotal.Value = "" Or txtWheretotal.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select corresponding option and fill value"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
            If cboTotal.Value = "BETWEEN" Then
                If txtWheretotal.Text = "" Or txtToTotal.Text = "" Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = "Please input FROM and TO values"
                    dxPopUpError.ShowOnPageLoad = True
                    Exit Sub
                End If
            End If
        End If

        If chkOpenedBetween.Checked = True Then
            If txtStart.Text = "" Or txtEnd.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select start date and end date"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        If chkUseLastTransaction.Checked = True Then
            If txtLastTransactionStartDate.Text = "" Or txtLastTransactionEndDate.Text = "" Then
                lblError.Text = "Please select start and end date of last transaction"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If

        cmdOk.Enabled = False
        Dim _getDetailsResponse As GetDetailsResponse
        Dim getDetailsRequest As New GetDetailsRequest

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "ActiveOnly: " & opt1.Checked & " Other: " & cboOther.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Age Analysis Summary Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================



        '------ Account Options ----------

        'getDetailsRequest.FromAccount = txtFrom.Text
        'getDetailsRequest.ToAccount = txtTo.Text
        'getDetailsRequest.CheckAll = chkAll.Checked

        getDetailsRequest.ActiveOnly = opt1.Checked
        getDetailsRequest.OtherStatus = opt2.Checked
        getDetailsRequest.Other = cboOther.Text

        '------ Date Options ----------

        getDetailsRequest.AccountsOpenedBetween = chkOpenedBetween.Checked
        getDetailsRequest.StartDate = txtStart.Text
        getDetailsRequest.EndDate = txtEnd.Text

        getDetailsRequest.LastTransaction = chkUseLastTransaction.Checked
        getDetailsRequest.LastDateTransactionStartDate = txtLastTransactionStartDate.Text
        getDetailsRequest.LastDateTransactionEndDate = txtLastTransactionEndDate.Text


        '------ Period Options ----------

        getDetailsRequest.CboCurrent = cboCurrent.Value
        getDetailsRequest.WhereCurrent = txtWhereCurrent.Text
        getDetailsRequest.CheckCurrentUse = chkUseCurrent.Checked


        getDetailsRequest.Cbo30Days = cbo30Days.Value
        getDetailsRequest.Where30Days = txtWhere30Days.Text
        getDetailsRequest.CheckUse30 = chkUse30.Checked

        getDetailsRequest.Cbo60Days = cbo60Days.Value
        getDetailsRequest.Where60Days = txtWhere60Days.Text
        getDetailsRequest.CheckUse60 = chkUse60.Checked

        getDetailsRequest.Cbo90Days = cbo90Days.Value
        getDetailsRequest.Where90Days = txtWhere90Days.Text
        getDetailsRequest.CheckUse90 = chkuse90.Checked

        getDetailsRequest.Cbo120Days = cbo120Days.Value
        getDetailsRequest.Where120Days = txtWhere120Days.Text
        getDetailsRequest.CheckUse120 = chkUse120.Checked

        getDetailsRequest.Cbo150Days = cbo150Days.Value
        getDetailsRequest.Where150Days = txtWhere150Days.Text
        getDetailsRequest.CheckUse150 = chkUse150.Checked

        getDetailsRequest.CboTotal = cboTotal.Value
        getDetailsRequest.Wheretotal = txtWheretotal.Text
        getDetailsRequest.ToTotal = txtToTotal.Text
        getDetailsRequest.CheckUsetotal = chkUseTotal.Checked

        _getDetailsResponse = _AgeAnalysis.GetDetails(getDetailsRequest)

        If _getDetailsResponse.Success = True Then
            Dim _dt As DataTable = _getDetailsResponse.GetSelectedResponse

            txtCurrent.Text = (_dt.Rows(_dt.Rows.Count - 1)("balance").ToString)
            txt30Days.Text = (_dt.Rows(_dt.Rows.Count - 1)("p30").ToString)
            txt60Days.Text = (_dt.Rows(_dt.Rows.Count - 1)("p60").ToString)
            txt90Days.Text = (_dt.Rows(_dt.Rows.Count - 1)("p90").ToString)
            txt120Days.Text = (_dt.Rows(_dt.Rows.Count - 1)("p120").ToString)
            txt150Days.Text = (_dt.Rows(_dt.Rows.Count - 1)("p150").ToString)
            txtTotal.Text = (_dt.Rows(_dt.Rows.Count - 1)("total").ToString)

            txtCurrentAccounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_current").ToString
            txt30Accounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_30").ToString
            txt60Accounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_60").ToString
            txt90Accounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_90").ToString
            txt120Accounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_120").ToString
            txt150Accounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_150").ToString
            txtTotalAccounts.Text = _dt.Rows(_dt.Rows.Count - 1)("number_of_accounts").ToString

            txtTotalSpent.Text = _dt.Rows(_dt.Rows.Count - 1)("total_spent").ToString

            ''InitialiseValues()
        Else
            'txtFrom.ClientEnabled = True
            'txtTo.ClientEnabled = True

            lblError.Text = _getDetailsResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If
        If opt1.Checked = True Then
            cboOther.ClientEnabled = False
            opt2.Checked = False
        Else
            cboOther.ClientEnabled = True
            opt2.Checked = True
        End If
        cmdOk.Enabled = True
    End Sub
    Private Sub checkOption()
        If cboTotal.Text = "BETWEEN" Then
            txtToTotal.Visible = True
        Else
            txtToTotal.Visible = False

        End If
    End Sub

End Class