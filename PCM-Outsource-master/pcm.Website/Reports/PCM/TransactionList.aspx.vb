Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class TransactionList
    Inherits System.Web.UI.Page
    Dim _AccountsBL As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.TransactionReport) Then
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
            StartD.Text = Format(Now, "yyyy-MM-dd")
            EndD.Text = Format(Now, "yyyy-MM-dd")

            'Dim _Log As New PCMUserLog
            '_Log.AccountNumber = ""
            '_Log.ActionType = "Page Load"
            '_Log.IPAddress = Session("LoggingIPAddress")
            '_Log.UserComment = ""
            '_Log.UserName = Session("username")
            '_Log.WebPage = "Quick Transaction Report"

            '_Logging.WriteToLogPCM(_Log)

        End If

    End Sub

    Protected Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdView.Click
        Dim transactionListRequest As New TransactionListRequest
        Dim transactionListResponse As New TransactionListResponse

        transactionListRequest.StartDate = StartD.Text
        transactionListRequest.EndDate = EndD.Text
        transactionListRequest.CheckPurchase = chkPur.Checked
        transactionListRequest.CheckGiftCardPurchase = chkGPur.Checked
        transactionListRequest.CheckPayments = chkPay.Checked
        transactionListRequest.CheckGiftCardPayments = chkGPay.Checked
        transactionListRequest.CheckCreditNotes = chkCn.Checked
        transactionListRequest.CheckGiftCardCreditNotes = chkGCn.Checked
        transactionListRequest.CheckLostCardProtection = chkLost.Checked
        transactionListRequest.CheckInterest = chkInt.Checked
        transactionListRequest.CheckCreditBalanceAffected = chkJCred.Checked
        transactionListRequest.CheckDebitBalanceAffected = chkJDeb.Checked
        transactionListRequest.CheckCreditBalanceNotAffected = chkJCredB.Checked
        transactionListRequest.CheckDebitBalanceNotAffected = chkJDebN.Checked
        transactionListRequest.BadDebtWriteOff = chkBadD.Checked

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & StartD.Text & " End Date: " & EndD.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Quick Transaction Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        transactionListResponse = _AccountsBL.GetTransactionListDetails(transactionListRequest)
        If transactionListResponse.Success = True Then
            If transactionListResponse.TransactionList IsNot Nothing Then
                Session("_Transactions") = transactionListResponse.TransactionList
                grdTransactions.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Details Found"
                dxPopUpError.ShowOnPageLoad = True
                Session("_Transactions") = Nothing
                grdTransactions.DataBind()
            End If
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = transactionListResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub


    Protected Sub grdTransactions_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdTransactions.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        ''Dim data As List(Of Transaction) = Session("_Transactions")
        gridView.DataSource = Session("_Transactions")
        grdTransactions.EndUpdate()

    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Export Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & StartD.Text & " End Date: " & EndD.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Quick Transaction Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Exporter.WriteCsvToResponse()
    End Sub
End Class