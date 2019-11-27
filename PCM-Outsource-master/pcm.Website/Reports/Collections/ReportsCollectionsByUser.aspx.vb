
Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class ReportsCollectionsByUser
    Inherits System.Web.UI.Page

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CollectionsByUser) Then
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

        Dim ReportData As New UserReportSummaryBusinessLayer

        Dim masterData As New DataTable

        masterData = ReportData.CollectionsByEmployee(txtFromDate.Text, txtToDate.Text)

        Dim data As DataSet '= CType(Session("data"), DataSet)

        masterData.PrimaryKey = New DataColumn() {masterData.Columns("username")}
        masterData.TableName = "Master"

        Dim detailData As DataTable

        detailData = ReportData.CollectionsByEmployeeTransactions(txtFromDate.Text, txtToDate.Text)

        'detailData.PrimaryKey = New DataColumn() {detailData.Columns("employee_number")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        data.Relations.Add("MasterDetail", masterData.Columns("username"), detailData.Columns("username"))

        Session("data") = data

        Return data
    End Function

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        'gridView.Columns.Clear()
        'gridView.AutoGenerateColumns = True
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "username" 'data.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = data

        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "username" 'dataView.Table.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = dataView
    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("data")

        gvMaster.DataBind()
    End Sub

    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
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
