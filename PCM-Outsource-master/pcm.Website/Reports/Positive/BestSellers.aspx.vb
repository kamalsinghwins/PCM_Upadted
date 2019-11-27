Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Imports Entities.Reports
Imports DevExpress.Export
Public Class BestSellers
    Inherits System.Web.UI.Page
    Dim Bestseller As New ReportsBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL
    Private Sub BestSellers_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.BestSellersReport) Then
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
    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If
        Catch ex As Exception
            Dim _blErrorLogging As New ErrorLogBL
        End Try
    End Sub
    Private Sub Run()
        Session.Remove("BestSellers")
        gvMaster.DataBind()
    End Sub
    Private Function GetData()
        If Not IsNothing(Session("BestSellers")) Then
            Return Session("BestSellers")
        End If

        Dim masterData As New DataTable
        Dim data As DataSet

        masterData = Bestseller.GetMasterBestSellers(txtFromDate.Text, txtToDate.Text, chkmastercode.Checked)
        masterData.PrimaryKey = New DataColumn() {masterData.Columns("stockcode")}
        masterData.TableName = "Master"


        Dim detailData As DataTable

        detailData = Bestseller.GetDetailBestSellers(txtFromDate.Text, txtToDate.Text, chkmastercode.Checked)
        masterData.PrimaryKey = New DataColumn() {masterData.Columns("guid")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        data.Relations.Add("MasterDetail", masterData.Columns("stockcode"), detailData.Columns("stockcode"))
        Session("BestSellers") = data
        Return data

    End Function
    Private Function GetMasterDetail() As DataTable
        Dim masterdetail As DataSet = GetData()
        If masterdetail IsNot Nothing Then
            Return masterdetail.Tables("Master")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetDetail(ByVal masterRowKey As Object) As DataView
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Dim detail As DataTable = data.Tables("Detail")
            Dim columnName As String = data.Relations("MasterDetail").ParentColumns(0).ColumnName
            Return New DataView(detail, String.Format("[{0}] = '{1}'", columnName, masterRowKey), String.Empty, DataViewRowState.CurrentRows)
        Else
            Return Nothing
        End If
    End Function
    Protected Sub gvMaster_DataBinding(sender As Object, e As EventArgs)
        gvMaster.BeginUpdate()
        Dim gridview As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim bestSellers As DataTable = GetMasterDetail()
        gridview.KeyFieldName = "stockcode"
        gridview.DataSource = bestSellers
        gvMaster.EndUpdate()
    End Sub
    Protected Sub gvDetail_DataBinding(sender As Object, e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim Newreport As DataView = GetDetail(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "guid"
        gridView.DataSource = Newreport

    End Sub
    Protected Sub btnPDF_Click(sender As Object, e As EventArgs)
        gridexporter.WritePdfToResponse()
    End Sub
    Protected Sub btnExcel_Click(sender As Object, e As EventArgs)
        gridexporter.WriteXlsxToResponse()
    End Sub
    Protected Sub btnCSV_Click(sender As Object, e As EventArgs)
        gridexporter.WriteCsvToResponse()
    End Sub

End Class