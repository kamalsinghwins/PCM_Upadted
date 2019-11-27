Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class CardSummary
    Inherits System.Web.UI.Page
    Dim _singleCardSummary As DebtorsBusinessLayer = New DebtorsBusinessLayer
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AccountCardSummary) Then
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

    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "checkCard" Then
            checkCardNumber()
        End If

    End Sub

    Public Sub checkCardNumber()
        Dim cardSumRequest As New CardSumRequest
        Dim getDetailsResponse As New GetDetails
        cardSumRequest.CardNumber = txtCardNum.Text


        If cardSumRequest.CardNumber = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Not a Valid Card Number"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "account_number: " & txtAccN.Text & " CardNumber: " & txtCardNum.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Card Summary Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        getDetailsResponse = _singleCardSummary.GetCardDetails(cardSumRequest)

        If getDetailsResponse.Success = True Then
            Dim _dt As DataTable = getDetailsResponse.GetDatable

            If _dt.Rows.Count = 0 Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Details Found"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub

            Else

                txtAccN.Text = _dt.Rows(_dt.Rows.Count - 1)("account_number") & ""
                txtCBy.Text = _dt.Rows(_dt.Rows.Count - 1)("created_by") & ""
                txtAct.Text = _dt.Rows(_dt.Rows.Count - 1)("assigned_by") & ""
                txtShop.Text = _dt.Rows(_dt.Rows.Count - 1)("delivered_by") & ""
                txtCreate.Text = _dt.Rows(_dt.Rows.Count - 1)("date_created") & ""
                txtModify.Text = _dt.Rows(_dt.Rows.Count - 1)("date_modified") & ""
                txtDateAssigned.Text = _dt.Rows(_dt.Rows.Count - 1)("date_assigned") & ""
                txtDateLastUsed.Text = _dt.Rows(_dt.Rows.Count - 1)("date_last_used") & ""
                txtStatus.Text = _dt.Rows(_dt.Rows.Count - 1)("current_status") & ""
                txtBranch.Text = _dt.Rows(_dt.Rows.Count - 1)("assigned_at_branch") & ""
                txtName.Text = _dt.Rows(_dt.Rows.Count - 1)("title") & " " & _dt.Rows(_dt.Rows.Count - 1)("first_name") & " " & _dt.Rows(_dt.Rows.Count - 1)("last_name") & ""

            End If
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = getDetailsResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub

End Class