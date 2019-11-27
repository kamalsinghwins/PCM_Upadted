Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class StockOnHandReport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blayer As New StockcodesHOBL
    Dim dt As New DataTable

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.StockOnHandByBranch) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Session.Remove("StockonHandByBranch")
            grdBranchStock.Visible = False
            cmdExportCSV.Visible = False
            cmdExportExcel.Visible = False
            cmdExportPDF.Visible = False
        End If
    End Sub
    Private Sub StockOnHandReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "Run" Then
                Run()
            End If

            If hdWhichButton.Value = "Search" Then
                Session.Remove("MastercodeSearch")
                grdMastercodeSearch.DataBind()
            End If

            If hdWhichButton.Value = "Select" Then
                SelectStockcode()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub SelectStockcode()
        Dim selectedValues = New List(Of Object)()
        selectedValues = Nothing
        selectedValues = grdMastercodeSearch.GetSelectedFieldValues("stockcode")
        If selectedValues.Count > 0 Then
            txtStockcode.Text = selectedValues(selectedValues.Count - 1)
        End If
        pcMain.ShowOnPageLoad = False
    End Sub
    Protected Sub grdMastercodeSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdMastercodeSearch.BeginUpdate()
        Dim gridview As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim dt As DataTable = GetMastercodes()
        gridview.KeyFieldName = "stockcode"
        gridview.DataSource = dt
        grdMastercodeSearch.EndUpdate()

    End Sub
    Private Function GetMastercodes() As DataTable
        Dim data As DataSet = GetCodes()
        If data IsNot Nothing Then
            Return data.Tables("MastercodeSearch")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetCodes() As DataSet
        If Not IsNothing(Session("MastercodeSearch")) Then
            Return Session("MastercodeSearch")
        End If

        Dim mastercode As New DataTable

        mastercode = _blayer.GetStockcodes(txtMastercodeSearch.Text)

        Dim ds As DataSet
        mastercode.TableName = "MastercodeSearch"

        ds = New DataSet()
        ds.Tables.Add(mastercode)
        Session("MastercodeSearch") = ds

        Return ds
    End Function



    Private Sub Run()
        If txtStockcode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter mastercode"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        Session.Remove("StockonHandByBranch")
        grdBranchStock.Columns.Clear()
        grdBranchStock.DataBind()
    End Sub
    Protected Sub grdBranchStock_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdBranchStock.BeginUpdate()
        Dim data As New DataTable
        data = GetMasterData()
        If data IsNot Nothing AndAlso data.Rows.Count > 0 Then
            grdBranchStock.KeyFieldName = "branchcode"
            grdBranchStock.DataSource = data
            grdBranchStock.AutoGenerateColumns = True
            grdBranchStock.EndUpdate()
            grdBranchStock.Visible = True
            cmdExportCSV.Visible = True
            cmdExportExcel.Visible = True
            cmdExportPDF.Visible = True
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No data found"
            dxPopUpError.ShowOnPageLoad = True
            grdBranchStock.Visible = False
            cmdExportCSV.Visible = False
            cmdExportExcel.Visible = False
            cmdExportPDF.Visible = False
        End If

    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("StockonHandByBranch")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet

        If Not IsNothing(Session("StockonHandByBranch")) Then
            Return Session("StockonHandByBranch")
        End If

        Dim reports As New DataTable

        reports = _blayer.GetStockOnHandByBranch(txtStockcode.Text)

        Dim data As DataSet
        reports.TableName = "StockonHandByBranch"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("StockonHandByBranch") = data

        Return data
    End Function
    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub
    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub
    Protected Sub cmdExportCSV_CLick(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse()
    End Sub
End Class