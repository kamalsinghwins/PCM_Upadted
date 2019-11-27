Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.Web.UI
Imports System.IO

Public Class DebtorHistory
    Inherits System.Web.UI.Page

    Dim _DebtorMaintainence As DebtorsMaintainenceBusinessLayer = New DebtorsMaintainenceBusinessLayer
    Dim _DebtorDataLayer As DebtorsBusinessLayer = New DebtorsBusinessLayer

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.DebtorHistory) Then
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
            InitialiseSearchFields()
        End If

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Lookup" Then
            grdDebtorsSearch.DataBind()
        End If

        If hdWhichButton.Value = "DebtorSelected" Then
            GetSelectedDebtorsDetails()
        End If

        If hdWhichButton.Value = "Query" Then
            Query()
        End If
    End Sub

    Private Sub InitialiseSearchFields()

        cboSearchType.Items.Add("ACCOUNT NUMBER")
        cboSearchType.Items.Add("ID NUMBER")
        cboSearchType.Items.Add("LAST NAME")
        cboSearchType.Items.Add("CELLPHONE")

    End Sub



    Protected Sub grdDebtorsSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdDebtorsSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _tmpCBO As ASPxComboBox

        _tmpCBO = LookupMain.FindControl("cboSearchType")

        Dim _tmpTXT As ASPxTextBox

        _tmpTXT = LookupMain.FindControl("txtCriteria")

        Dim data As DataTable = _DebtorMaintainence.GetDebtors(_tmpCBO.Text, _tmpTXT.Text.ToUpper, False, False)

        gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdDebtorsSearch.EndUpdate()
    End Sub

    Private Sub ClearFields()

        'txtCriteria.Text = ""

        'cboSearchType.Text = ""

        'Dim lblCredit As ASPxLabel
        'lblCredit = (ASPxNavBar1.Groups.FindByName("Transaction").Items(0).FindControl("lblCredit"))
        'lblCredit.Text = "Credit :"

        'Dim _eDt As New DataTable

        ''grdDebtorsSearch.DataSource = _eDt
        ''grdDebtorsSearch.DataBind()

        'Dim _tmpGRID As ASPxGridView
        '_tmpGRID = LookupMain.FindControl("grdAccountChanges")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()

        '_tmpGRID = LookupMain.FindControl("grdTransaction")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()

        '_tmpGRID = LookupMain.FindControl("grdClosingBalances")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()

        '_tmpGRID = LookupMain.FindControl("grdAgeAnalysis")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()

        '_tmpGRID = LookupMain.FindControl("grdPayment")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()

        '_tmpGRID = LookupMain.FindControl("grdHidden")
        '_tmpGRID.DataSource = _eDt
        '_tmpGRID.DataBind()


    End Sub

    Private Sub GetSelectedDebtorsDetails()

        'Clear all the controls
        'ClearFields()

        Dim selectedValues = New List(Of Object)()

        selectedValues = Nothing


        selectedValues = grdDebtorsSearch.GetSelectedFieldValues("account_number")
        If selectedValues.Count > 0 Then
            Dim strAccountNumber As String = selectedValues(selectedValues.Count - 1)
            txtAccNum.Text = strAccountNumber
            txtAccNum.Text = selectedValues(selectedValues.Count - 1)

        Else

            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a record"
            dxPopUpError.ShowOnPageLoad = True
        End If

        LookupMain.ShowOnPageLoad = False

    End Sub


    Private Sub Query()

        Session.Remove("_Credit")

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "account_number: " & txtAccNum.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Debtor History Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Dim historyRequest As New HistoryRequest
        Dim getHistoryResponse As New GetHistoryResponse

        historyRequest.Accountnumber = txtAccNum.Text
        If historyRequest.Accountnumber = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please specify an Account Number before you continue."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Session.Remove("_AccountChanges")
        Session.Remove("_Transaction")
        Session.Remove("_ClosingBalances")
        Session.Remove("_AgeAnalysis")
        Session.Remove("_PaymentPlans")

        getHistoryResponse = _DebtorDataLayer.GetQueryResponse(historyRequest)
        If getHistoryResponse.Success = True Then

            If Not IsNothing(getHistoryResponse) Then
                Session("_AccountChanges") = getHistoryResponse.AccountChanges
                Session("_Transaction") = getHistoryResponse.Transactions
                Session("_ClosingBalances") = getHistoryResponse.ClosingBalances
                Session("_AgeAnalysis") = getHistoryResponse.AgeAnalysis
                Session("_PaymentPlans") = getHistoryResponse.PaymentPlans
                'Session("_Credit") = getHistoryResponse.Transactions

            End If


            Dim tmpGrd As ASPxGridView
            Dim lblCredit As ASPxLabel

            tmpGrd = ASPxNavBar1.Groups.FindByName("AccountChanges").Items(0).FindControl("grdAccountChanges")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("Transaction").Items(0).FindControl("grdTransaction")
            tmpGrd.DataBind()

            lblCredit = (ASPxNavBar1.Groups.FindByName("Transaction").Items(0).FindControl("lblCredit"))
            lblCredit.Text = "Credit :" & getHistoryResponse.CreditLimit & "       Total Owing: " & getHistoryResponse.Balance

            tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("AgeAnalysis").Items(0).FindControl("grdAgeAnalysis")
            tmpGrd.DataBind()

            tmpGrd = ASPxNavBar1.Groups.FindByName("Payment").Items(0).FindControl("grdPayment")
            tmpGrd.DataBind()

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = getHistoryResponse.Message
            dxPopUpError.ShowOnPageLoad = True

        End If

    End Sub
    Protected Sub grdAccountChanges_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding
        Dim tmpGrd As ASPxGridView
        tmpGrd = ASPxNavBar1.Groups.FindByName("AccountChanges").Items(0).FindControl("grdAccountChanges")

        If IsNothing(Session("_AccountChanges")) Then
            tmpGrd.DataSource = Nothing
        Else
            tmpGrd.DataSource = Session("_AccountChanges")
        End If
    End Sub

    Protected Sub grdTransaction_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding
        Dim tmpGrd As ASPxGridView
        tmpGrd = ASPxNavBar1.Groups.FindByName("Transaction").Items(0).FindControl("grdTransaction")

        If IsNothing(Session("_Transaction")) Then
            tmpGrd.DataSource = Nothing
        Else
            tmpGrd.DataSource = Session("_Transaction")
        End If
    End Sub



    Protected Sub grdClosingBalances_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding
        Dim tmpGrd As ASPxGridView
        tmpGrd = ASPxNavBar1.Groups.FindByName("ClosingBalances").Items(0).FindControl("grdClosingBalances")

        If IsNothing(Session("_ClosingBalances")) Then
            tmpGrd.DataSource = Nothing
        Else
            tmpGrd.DataSource = Session("_ClosingBalances")
        End If
    End Sub

    Protected Sub grdAgeAnalysis_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding
        Dim tmpGrd As ASPxGridView
        tmpGrd = ASPxNavBar1.Groups.FindByName("AgeAnalysis").Items(0).FindControl("grdAgeAnalysis")

        If IsNothing(Session("_AgeAnalysis")) Then
            tmpGrd.DataSource = Nothing
        Else
            tmpGrd.DataSource = Session("_AgeAnalysis")
        End If
    End Sub

    Protected Sub grdPayment_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        ' Assign the data source in grid_DataBinding
        Dim tmpGrd As ASPxGridView
        tmpGrd = ASPxNavBar1.Groups.FindByName("Payment").Items(0).FindControl("grdPayment")

        If IsNothing(Session("_PaymentPlans")) Then
            tmpGrd.DataSource = Nothing
        Else
            tmpGrd.DataSource = Session("_PaymentPlans")
        End If
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

    'Private Sub ClearPopup()

    '    Dim _tmpCBO As ASPxComboBox
    '    _tmpCBO = LookupMain.FindControl("cboSearchType")
    '    _tmpCBO.Items.Clear()

    '    _tmpCBO.Items.Add("Employee Number")
    '    _tmpCBO.Items.Add("First Name")
    '    _tmpCBO.Items.Add("Last Name")


    '    ASPxEdit.ClearEditorsInContainer(LookupMain)
    'End Sub


    Protected Sub cmdExportToCSV_Click(sender As Object, e As EventArgs) Handles cmdExportToCSV.Click
        Dim ps As New PrintingSystem()

        Dim link1 As New PrintableComponentLink(ps)
        link1.Component = GridExporter1

        Dim space1 As New PrintableComponentLink(ps)
        space1.Component = GridExporter6

        Dim link2 As New PrintableComponentLink(ps)
        link2.Component = GridExporter2

        Dim space2 As New PrintableComponentLink(ps)
        space2.Component = GridExporter6

        Dim link3 As New PrintableComponentLink(ps)
        link3.Component = GridExporter3

        Dim space3 As New PrintableComponentLink(ps)
        space3.Component = GridExporter6

        Dim link4 As New PrintableComponentLink(ps)
        link4.Component = GridExporter4

        Dim space4 As New PrintableComponentLink(ps)
        space4.Component = GridExporter6

        Dim link5 As New PrintableComponentLink(ps)
        link5.Component = GridExporter5


        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link1, space1, space1, space1, link2, space2, space2, space2, link3,
                                     space3, space3, space3, link4, space4, space4, space4, link5})

        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("filename", True, "csv", stream)
        End Using
        ps.Dispose()
    End Sub

End Class