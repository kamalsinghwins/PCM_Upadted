Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.Web.UI
Imports System.IO
Imports System


Public Class GiftCardDetails
    Inherits System.Web.UI.Page
    Dim _AgeAnalysis As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.GiftCardDetails) Then
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

        'If Not IsPostBack Then
        '    '=====================================================================================================
        '    'LOGGING
        '    '=====================================================================================================
        '    Dim _Log As New PCMUserLog
        '    _Log.AccountNumber = ""
        '    _Log.ActionType = "Page Load"
        '    _Log.IPAddress = Session("LoggingIPAddress")
        '    _Log.SearchCriteria = ""
        '    _Log.UserComment = ""
        '    _Log.UserName = Session("username")
        '    _Log.WebPage = "Gift Card Report"

        '    _Logging.WriteToLogPCM(_Log)
        '    '=====================================================================================================
        'End If

    End Sub

    Protected Sub cmdQuery_Click(sender As Object, e As EventArgs) Handles cmdQuery.Click
        Dim giftCardDetailsRequest As New GiftCardDetailsRequest
        Dim giftCardDetailsResponse As New GiftCardDetailsResponse
        Dim TransTotal As Double = 0
        Dim TransItem As Integer = 0

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Running Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Card Number: " & txtAccNum.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Gift Card Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        giftCardDetailsRequest.CardNumber = txtAccNum.Text
        giftCardDetailsResponse = _AgeAnalysis.GetCardDetails(giftCardDetailsRequest)
        If giftCardDetailsResponse.Success = True Then
            If giftCardDetailsResponse.GiftCardDetails IsNot Nothing Then
                Session("_CardDetails") = giftCardDetailsResponse.GiftCardDetails
                grdGiftCardDetails.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Details Found"
                dxPopUpError.ShowOnPageLoad = True
            End If

            If giftCardDetailsResponse.CardTransactions IsNot Nothing Then
                Session("_CardTransactions") = giftCardDetailsResponse.CardTransactions
                grdCardtransactionsDetails.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Details Found"
                dxPopUpError.ShowOnPageLoad = True
            End If

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = giftCardDetailsResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub

    Protected Sub grdGiftCardDetails_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdGiftCardDetails.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_CardDetails")
        ''gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data
        grdGiftCardDetails.EndUpdate()

    End Sub

    Protected Sub grdCardtransactionsDetails_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdCardtransactionsDetails.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_CardTransactions")
        ''gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data
        grdCardtransactionsDetails.EndUpdate()
    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Exporting Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Card Number: " & txtAccNum.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Gift Card Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        'Exporter.WriteCsvToResponse()
        Dim ps As New PrintingSystem()

        Dim link1 As New PrintableComponentLink(ps)
        link1.Component = GridExporter1

        Dim link2 As New PrintableComponentLink(ps)
        link2.Component = GridExporter2

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link1, link2})

        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("filename", True, "csv", stream)
        End Using
        ps.Dispose()
    End Sub

    Private Sub WriteToResponse(ByVal fileName As String, ByVal saveAsFile As Boolean, ByVal fileFormat As String, ByVal stream As MemoryStream)
        If Page Is Nothing OrElse Page.Response Is Nothing Then
            Return
        End If
        Dim disposition As String = If(saveAsFile, "attachment", "inline")
        Page.Response.Clear()
        Page.Response.Buffer = False
        Page.Response.AppendHeader("Content-Type", String.Format("application/{0}", fileFormat))
        Page.Response.AppendHeader("Content-Transfer-Encoding", "binary")
        Page.Response.AppendHeader("Content-Disposition", String.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat))
        Page.Response.BinaryWrite(stream.GetBuffer())
        Page.Response.End()
    End Sub
End Class