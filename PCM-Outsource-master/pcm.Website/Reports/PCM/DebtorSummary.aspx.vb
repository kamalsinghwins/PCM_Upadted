Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class debtor_sum
    Inherits System.Web.UI.Page

    Dim _blDebtorOrder As DebtorsBusinessLayer = New DebtorsBusinessLayer

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
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Logging As UsersBL = New UsersBL
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Run Report"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = ""
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Debtor Summary Report"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================


            Dim _DebtorsSumResponse As New DebtorsSumResponse
            _DebtorsSumResponse = _blDebtorOrder.getDebtorsSumDetails()

            If _DebtorsSumResponse.Success = True Then
                lblTA.Text = _DebtorsSumResponse.lblTA

                lblLCP.Text = _DebtorsSumResponse.lblLCP
                lblAD.Text = _DebtorsSumResponse.lblAD
                lblPend.Text = _DebtorsSumResponse.lblPend
                lblFraud.Text = _DebtorsSumResponse.lblFraud
                lblSusp.Text = _DebtorsSumResponse.lblSusp
                lblWO.Text = _DebtorsSumResponse.lblWO
                lblBlock.Text = _DebtorsSumResponse.lblBlock
                lblDD.Text = _DebtorsSumResponse.lblDD
                lblLeg.Text = _DebtorsSumResponse.lblLeg

                'If Val(lblTA.Text) <> 0 Then
                '    lblLCPP.Text = _DebtorsSumResponse.lblLCPP
                '    lblAp.Text = _DebtorsSumResponse.lblAp
                '    lblPendP.Text = _DebtorsSumResponse.lblPendP
                '    lblFraudP.Text = _DebtorsSumResponse.lblFraudP
                '    lblSuspP.Text = _DebtorsSumResponse.lblSuspP
                '    lblWOP.Text = _DebtorsSumResponse.lblWOP
                '    lblBlockP.Text = _DebtorsSumResponse.lblBlockP
                '    lblDDP.Text = _DebtorsSumResponse.lblDDP
                '    lblLegP.Text = _DebtorsSumResponse.lblLegP
                'End If
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = _DebtorsSumResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

    End Sub

End Class