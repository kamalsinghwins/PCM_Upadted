Imports DevExpress.Web
Imports Entities
Imports Entities.Reports
Imports pcm.BusinessLayer
Imports Newtonsoft.Json
Public Class SizeGridsReport
    Inherits System.Web.UI.Page
    Dim _blayer As New ReportsBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL
    Dim ds As New DataSet
    Dim dt As New DataTable
    Private searchResponse As New SearchResponse


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SizeGridReport) Then
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
            lstBranches.Items.Clear()
            BindCategories()
            BindBranches()
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Search" Then
            SearchStockcode()
        End If
        If hdWhichButton.Value = "Select" Then
            SelectStockcode()
        End If
        If hdWhichButton.Value = "Run" Then
            Run()
        End If
    End Sub
    Private Sub SizeGridsReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub Run()
        Dim email As String = String.Empty
        Dim Username As String = String.Empty
        Dim listBranches As String = String.Empty
        Dim branches As String = String.Empty
        Dim ReturnString As String = String.Empty

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            Username = Session("username")
            email = Session("email")
        End If
        If cboCategory1.Text = "" Then
            cboCategory1.Text = "ALL"
        End If
        If cboCategory2.Text = "" Then
            cboCategory2.Text = "ALL"
        End If
        If cboCategory3.Text = "" Then
            cboCategory3.Text = "ALL"
        End If
        If cboSizeGrid.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Size Grid"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        If chkAll.Checked = False Then
            If lstBranches.SelectedItems.Count = 0 Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select a branch"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub

            End If
        End If
        For i = 0 To lstBranches.Items.Count - 1
            If chkAll.Checked = True Then
                Branches += lstBranches.Items(i).Text & ","
            Else
                If lstBranches.Items(i).Selected = True Then
                    Branches += lstBranches.Items(i).Text & ","

                End If
            End If
        Next

        listBranches = branches.Remove(branches.Length - 1)
        Dim gridSize As New Entities.Reports.GridSize
        gridSize.Category1 = cboCategory1.Text
        gridSize.Category2 = cboCategory2.Text
        gridSize.Category3 = cboCategory3.Text
        gridSize.SizeGrid = cboSizeGrid.Text
        gridSize.StartCode = txtStartCode.Text
        gridSize.EndCode = txtEndCode.Text
        gridSize.AllBranches = chkAll.Checked
        gridSize.Branches = listBranches
        gridSize.IPAddress = Session("ipaddress")
        gridSize.IsAdmin = Session("is_pcm_admin")

        Dim json As String = JsonConvert.SerializeObject(gridSize)

        Try
            ReturnString = _blayer.RunGridSizeReport(txtFromDate.Text, txtToDate.Text, email, Username, json)
            If ReturnString <> "Success" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something went wrong"
                dxPopUpError.ShowOnPageLoad = True

            Else
                clearForm()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Your report is running! Please look for the completed report in the View Reports Page after it is done.."
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub BindCategories()
        Try

            ''Bind Category1 Dropdown
            cboCategory1.Items.Clear()
            cboCategory1.Items.Add("ALL")
            dt = _blayer.GetCategory("1")
            If dt.Rows.Count > 0 AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboCategory1.Items.Add(drBranch.Item("category_code") & " - " & drBranch.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            ''Bind Category2 Dropdown
            cboCategory2.Items.Clear()
            cboCategory2.Items.Add("ALL")
            dt = _blayer.GetCategory("2")
            If dt.Rows.Count > 0 AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboCategory2.Items.Add(drBranch.Item("category_code") & " - " & drBranch.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            ''Bind Category3 Dropdown
            cboCategory3.Items.Clear()
            cboCategory3.Items.Add("ALL")
            dt = _blayer.GetCategory("3")
            If dt.Rows.Count > 0 AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboCategory3.Items.Add(drBranch.Item("category_code") & " - " & drBranch.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            ''Bind Size Grid Dropdown
            cboSizeGrid.Items.Clear()
            dt = _blayer.GetGridSize()
            If dt.Rows.Count > 0 AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboSizeGrid.Items.Add(drBranch.Item("grid_number") & " - " & drBranch.Item("grid_description") & "")
                Next
            End If
            dt.Clear()

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub BindBranches()
        Try
            ds = _blayer.GetBranches()
            ''Bind Branches Listbox
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                For Each drBranch As DataRow In ds.Tables(0).Rows
                    lstBranches.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                Next
            End If
            ds.Clear()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub SearchStockcode()
        Try
            searchResponse = _blayer.GetAllStockCodes(txtSearch.Text, True)
            If searchResponse.dt.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In searchResponse.dt.Rows
                    lstSearch.Items.Add(drSCs.Item("master_code") & " - " & drSCs.Item("description"))
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
    Private Sub SelectStockcode()
        Dim arrArray() As String
        Try
            arrArray = Split(lstSearch.Value, " - ")
            If hdSearchWhat.Value = "from" Then
                txtStartCode.Text = arrArray(0)
            Else
                txtEndCode.Text = arrArray(0)
            End If
            txtSearch.Text = String.Empty
            LookupMain.ShowOnPageLoad = False
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
        cboCategory1.SelectedIndex = -1
        cboCategory2.SelectedIndex = -1
        cboCategory3.SelectedIndex = -1
        cboSizeGrid.SelectedIndex = -1
        txtStartCode.Text = String.Empty
        txtEndCode.Text = String.Empty
        lstBranches.UnselectAll()
    End Sub
End Class