Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class CashCardCustomersTransactions
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CashCards) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If

    End Sub

    Private Sub CashCardCustomersTransactions_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If

            If hdWhichButton.Value = "SearchAccount" Then
                SearchAccount
            End If
            If hdWhichButton.Value = "SelectAccount" Then
                SelectAccount()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try


    End Sub
    Protected Sub grdAccountNumber_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdAccountNumber.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _BLayer As New ReportsPositiveCashCardsBL(Session("current_company"))

        Dim data As DataTable = _BLayer.GetCashCardAccounts(txtAccountSearch.Text)

        gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdAccountNumber.EndUpdate()

    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        dxGrid.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "guid"
        gridView.DataSource = data
        dxGrid.EndUpdate()

    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("CashCardCustomerTransactions")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetReports() As DataSet

        If Not IsNothing(Session("cashcardtransactions")) Then
            Return Session("cashcardtransactions")
        End If

        Dim _BLayer As New ReportsPositiveCashCardsBL(Session("current_company"))
        Dim reports As New DataTable

        reports = _BLayer.ReturnTransactionListing(txtFromDate.Text, txtToDate.Text, txtAccount.Text)
        Dim data As DataSet
        reports.TableName = "CashCardCustomerTransactions"
        data = New DataSet()
        data.Tables.Add(reports)
        Session("cashcardtransactions") = data
        Return data
    End Function


    Protected Sub SearchAccount()
        grdAccountNumber.DataBind()
    End Sub

    Protected Sub Run()
        Session.Remove("cashcardtransactions")
        dxGrid.DataBind()

    End Sub

    Protected Sub SelectAccount()
        Dim selectedValues = New List(Of Object)()

        selectedValues = grdAccountNumber.GetSelectedFieldValues("account_number")

        If selectedValues.Count > 0 Then
            txtAccount.Text = selectedValues(0)
            pcMain.ShowOnPageLoad = False
        End If
    End Sub

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse()
    End Sub


End Class