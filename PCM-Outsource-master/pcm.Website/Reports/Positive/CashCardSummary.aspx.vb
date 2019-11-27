Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class CashCardSummary
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CashCardSummary) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtAccountFrom.Text = Format(Now, "yyyy-MM-dd")
            txtAccountTo.Text = Format(Now, "yyyy-MM-dd")
            txtTransactionFrom.Text = Format(Now, "yyyy-MM-dd")
            txtTransactionTo.Text = Format(Now, "yyyy-MM-dd")

        End If

    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("Master")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetDetailData(ByVal masterRowKey As Object) As DataView
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Dim detail As DataTable = data.Tables("Detail")
            Dim columnName As String = data.Relations("MasterDetail").ParentColumns(0).ColumnName
            Return New DataView(detail, String.Format("[{0}] = '{1}'", columnName, masterRowKey), String.Empty, DataViewRowState.CurrentRows)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetData() As DataSet

        If Not IsNothing(Session("data")) Then
            Return Session("data")
        End If

        Dim ReportData As New ReportsPositiveCashCardsBL

        Dim masterData As New DataTable

        masterData = ReportData.ReturnCashCardSummary(txtAccountFrom.Text, txtAccountTo.Text, txtTransactionFrom.Text, txtTransactionTo.Text)

        Dim data As DataSet '= CType(Session("data"), DataSet)

        masterData.PrimaryKey = New DataColumn() {masterData.Columns("account_number")}
        masterData.TableName = "Master"

        Dim detailData As DataTable

        detailData = ReportData.ReturnCashCardSummaryLineItems(txtAccountFrom.Text, txtAccountTo.Text, txtTransactionFrom.Text, txtTransactionTo.Text)

        'detailData.PrimaryKey = New DataColumn() {detailData.Columns("employee_number")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        data.Relations.Add("MasterDetail", masterData.Columns("account_number"), detailData.Columns("account_number"))

        Session("data") = data

        Return data
    End Function

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        'gridView.Columns.Clear()
        'gridView.AutoGenerateColumns = True
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = data

        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "account_number" 'dataView.Table.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = dataView
    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click
        Session.Remove("data")
        Try
            gvMaster.DataBind()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click

        Dim pageKeys As List(Of Object) = gvMaster.GetCurrentPageRowValues(gvMaster.KeyFieldName)

        If pageKeys IsNot Nothing Then
            For Each key As Object In pageKeys
                Exporter.GridView.Selection.SetSelectionByKey(key, True)
            Next
        End If

        Exporter.WritePdfToResponse()

    End Sub

End Class

