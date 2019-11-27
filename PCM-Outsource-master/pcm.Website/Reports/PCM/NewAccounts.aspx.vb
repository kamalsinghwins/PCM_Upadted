Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class NewAccounts
    Inherits System.Web.UI.Page
    Dim _PCMReportingBusinessLayer As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.NewAccountsReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            InitialiseStatusValues()
        End If

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
        '_Log.WebPage = "New Accounts Report"

        '_Logging.WriteToLogPCM(_Log)
        ''=====================================================================================================

    End Sub

    Protected Sub cmdCardIssued_Click(sender As Object, e As EventArgs) Handles cmdCardIssued.Click
        Dim newAccountRequest As New NewAccountRequest
        Dim cardIssuedResponse As New CardIssuedResponse

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & txtStart.Text & " End Date: " & txtEnd.Text & " Cards Issued"
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "New Accounts Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        newAccountRequest.StartDate = txtStart.Text
        newAccountRequest.EndDate = txtEnd.Text
        newAccountRequest.Status = cboStat.Value
        newAccountRequest.CheckCardIssued = chkCardIssued.Checked
        cardIssuedResponse = _PCMReportingBusinessLayer.CardIssued(newAccountRequest)

        If cardIssuedResponse.Success = True Then
            lblResult.Text = "Result : " & cardIssuedResponse.Count
            'lblCards.Visible = True
            'lblExperian.Visible = False
            'lblNotExperian.Visible = False
            'lblTU.Visible = False
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = cardIssuedResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub

    Protected Sub cmdQuery_Click(sender As Object, e As EventArgs) Handles cmdQuery.Click
        Dim newAccountRequest As New NewAccountRequest
        Dim queryResponse As New GetQueryResponse

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & txtStart.Text & " End Date: " & txtEnd.Text & " New Accounts"
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "New Accounts Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        newAccountRequest.StartDate = txtStart.Text
        newAccountRequest.EndDate = txtEnd.Text
        newAccountRequest.Status = cboStat.Value
        newAccountRequest.CheckCardIssued = chkCardIssued.Checked
        queryResponse = _PCMReportingBusinessLayer.GetQuery(newAccountRequest)
        If queryResponse.Success = True Then
            lblResult.Text = "Result : " & queryResponse.Count
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = queryResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub

    Private Sub InitialiseStatusValues()
        cboStat.Items.Clear()
        cboStat.Items.Add("ALL")
        cboStat.Items.Add("ACTIVE")
        cboStat.Items.Add("BLOCKED")
        cboStat.Items.Add("DEBT REVIEW")
        cboStat.Items.Add("DECEASED")
        cboStat.Items.Add("DECLINED")
        cboStat.Items.Add("FRAUD")
        cboStat.Items.Add("LEGAL")
        cboStat.Items.Add("PENDING")
        cboStat.Items.Add("SUSPENDED")
        cboStat.Items.Add("WRITE-OFF")
        cboStat.Text = ""

        txtStart.Text = Format(Now, "yyyy-MM-dd")
        txtEnd.Text = Format(Now, "yyyy-MM-dd")
    End Sub
End Class