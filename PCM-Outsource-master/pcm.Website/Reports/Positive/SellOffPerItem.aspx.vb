Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer


Public Class SellOffPerItem
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blayer As New ReportsBusinessLayer
    Dim _items As New DataTable

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SellOffPerItem) Then
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
    Private Sub SellOffPerItem_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If
            If hdWhichButton.Value = "SearchStockCodes" Then
                SearchStockCodes()
            End If
            If hdWhichButton.Value = "SelectStockCode" Then
                SelectStockCode()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub Run()
        Session.Remove("SellOffItem")
        gvSellItem.DataBind()
    End Sub
    Protected Sub gvSellItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvSellItem.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "branch_code"
        gridView.DataSource = data
        gvSellItem.EndUpdate()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("SellOffItem")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet
        If Not IsNothing(Session("SellOffItem")) Then
            Return Session("SellOffItem")
        End If
        _items = _blayer.GetSellOffPerItems(txtFromDate.Text, txtToDate.Text, txtStockcode.Text)
        Dim data As DataSet
        _items.TableName = "SellOffItem"
        data = New DataSet()
        data.Tables.Add(_items)
        Session("SellOffItem") = data
        Return data
    End Function
    Private Sub SearchStockCodes()
        Dim stockcodes As DataTable
        Try
            stockcodes = _blayer.SearchStockcodes(txtSearch.Text)
            If stockcodes.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In stockcodes.Rows
                    lstSearch.Items.Add(drSCs.Item("master_code"))
                Next
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Stockcodes found"
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub SelectStockCode()
        txtStockcode.Text = lstSearch.Value
        txtSearch.Text = String.Empty
        LookupMain.ShowOnPageLoad = False
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