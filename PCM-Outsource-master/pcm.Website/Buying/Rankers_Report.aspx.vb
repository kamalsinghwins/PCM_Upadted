Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class Rankers_Report
    Inherits System.Web.UI.Page

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.RankersReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("current_company") = "010"
        End If

        If Not IsPostBack Then
            Session.Remove("data")
        End If

        gvMaster.DataBind()
    End Sub


    Private Function GetData() As DataSet

        If Not IsNothing(Session("data")) Then
            Return Session("data")
        End If

        Dim ReportData As New BuyingBL()
        Dim masterData As New DataTable

        masterData = ReportData.GetRankers

        Dim data As DataSet '= CType(Session("data"), DataSet)

        masterData.PrimaryKey = New DataColumn() {masterData.Columns("user_id")}
        masterData.TableName = "Master"

        Dim detailData As DataTable

        detailData = ReportData.GetRankersRankings

        'detailData.PrimaryKey = New DataColumn() {detailData.Columns("service_provider")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        data.Relations.Add("MasterDetail", masterData.Columns("user_id"), detailData.Columns("user_id"))

        Session("data") = data

        Return data
    End Function

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

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "user_id" 'data.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = data

        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "user_id" 'dataView.Table.PrimaryKey(0).ColumnName
        'gridView.Caption = "Username"
        gridView.DataSource = dataView
    End Sub

    Private Sub RankersReport_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
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