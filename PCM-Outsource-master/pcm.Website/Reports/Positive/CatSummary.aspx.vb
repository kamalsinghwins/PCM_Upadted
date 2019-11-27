Imports DevExpress.Web
Imports Entities
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Public Class CatSummary
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blayer As New ReportsBusinessLayer
    Dim ds As New DataSet
    Dim dt As New DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CategorySummaryReport) Then
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
            radTotalsOnly.Checked = True
            lstBranches.ClientEnabled = False
            chkAll.Visible = False
            lstBranches.Items.Clear()
            BindCategories()
            BindBranches()
        End If
    End Sub
    Private Sub CatSummary_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If
            If hdWhichButton.Value = "Toggle" Then
                Toggle()
            End If
            If hdWhichButton.Value = "DoToggle" Then
                DoToggle()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub Run()
        Dim ReturnString As String = String.Empty
        Dim listBranches As String = String.Empty
        Dim branches As String = String.Empty
        Dim username As String = String.Empty
        Dim email As String = String.Empty

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
            email = Session("email")
        End If
        If radBranches.Checked = True Then
            If chkAll.Checked = False Then
                If lstBranches.SelectedItems.Count = 0 Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = "Please Select the branch"
                    dxPopUpError.ShowOnPageLoad = True
                    loadDefaults()
                    Exit Sub
                Else
                    For i = 0 To lstBranches.Items.Count - 1
                        If lstBranches.Items(i).Selected = True Then
                            branches += lstBranches.Items(i).Text & ","
                        End If
                    Next
                End If

            Else
                For i = 0 To lstBranches.Items.Count - 1
                    branches += lstBranches.Items(i).Text & ","
                Next
            End If
            listBranches = branches.Remove(branches.Length - 1)
        End If
        Dim catSummary As New Entities.Reports.CategorySummary
        catSummary.IPAddress = Session("ipaddress")
        catSummary.IsAdmin = Session("is_pcm_admin")
        catSummary.Branches = listBranches
        catSummary.Category1 = cboCategory1.Text
        catSummary.Category2 = cboCategory2.Text
        catSummary.Category3 = cboCategory3.Text
        catSummary.RunSummaryOnly = radTotalsOnly.Checked
        catSummary.AllBranches = radBranches.Checked
        Dim json As String = JsonConvert.SerializeObject(catSummary)

        ReturnString = _blayer.RunCategorySummaryReport(txtFromDate.Text, txtToDate.Text, email, username, json)

        If ReturnString <> "Success" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Something went wrong"
            dxPopUpError.ShowOnPageLoad = True

        Else
            clearForm()
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Your report is running! Please look for the completed report in the View Reports Page after it is done."
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub
    Private Sub BindCategories()
        Try

            ''Bind Category1 Dropdown
            cboCategory1.Items.Clear()
            cboCategory1.Items.Add("ALL")
            cboCategory1.Items.Add("N/A")
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
            cboCategory2.Items.Add("N/A")
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
            cboCategory3.Items.Add("N/A")
            dt = _blayer.GetCategory("3")
            If dt.Rows.Count > 0 AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboCategory3.Items.Add(drBranch.Item("category_code") & " - " & drBranch.Item("category_description") & "")
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
    Private Sub Toggle()
        lstBranches.ClientEnabled = False
        chkAll.Visible = False
    End Sub
    Private Sub DoToggle()
        lstBranches.ClientEnabled = True
        chkAll.Visible = True
    End Sub
    Private Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
        cboCategory1.SelectedIndex = -1
        cboCategory2.SelectedIndex = -1
        cboCategory3.SelectedIndex = -1
        radTotalsOnly.Checked = True
        radBranches.Checked = False
        chkAll.Visible = False
        lstBranches.UnselectAll()
    End Sub
    Private Sub loadDefaults()
        If radBranches.Checked = True Then
            lstBranches.ClientEnabled = True
            chkAll.Visible = True
        Else
            lstBranches.ClientEnabled = False
            chkAll.Visible = False
        End If
    End Sub
End Class