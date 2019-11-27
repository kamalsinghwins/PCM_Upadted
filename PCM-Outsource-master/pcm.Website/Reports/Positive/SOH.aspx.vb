Imports DevExpress.Web
Imports Entities
Imports Entities.Reports
Imports Newtonsoft.Json
Imports pcm.BusinessLayer

Public Class SOH
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _reports As New ReportsBusinessLayer
    Dim ds As New DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SOHReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            BindDropdown()
            ds = _reports.GetBranches
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                For Each drbranch As DataRow In ds.Tables(0).Rows
                    lstBranches.Items.Add(drbranch("branch_code") & "-" & drbranch("branch_name") & "")
                Next
            End If

            ShowHide()
        End If
    End Sub
    Private Sub SOH_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "Run" Then
                Run()
            End If

            If hdWhichButton.Value = "ShowHide" Then
                'ShowHide()
            End If

            If hdWhichButton.Value = "Search" Then
                SearchStockCodes()
            End If

            If hdWhichButton.Value = "Select" Then
                SelectStockCodes()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub ShowHide()
        If cboType.Text = "Range" Then
            lblFromStockCode.Visible = True
            lblToStockCode.Visible = True
            txtFromStockCode.Visible = True
            txtToStockCode.Visible = True
            FromStockPopUp.Visible = True
            ToStockPopUp.Visible = True
        Else
            lblFromStockCode.Visible = False
            lblToStockCode.Visible = False
            txtFromStockCode.Visible = False
            txtToStockCode.Visible = False
            FromStockPopUp.Visible = False
            ToStockPopUp.Visible = False
        End If

    End Sub
    Public Sub SearchStockCodes()

        Dim dt As New DataTable
        dt = _reports.GetStockcodesList(txtSearch.Text, chkMasterCode.Checked)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                If chkMasterCode.Checked = True Then
                    lstSearch.Items.Add(dr("master_code") & " - " & dr("description") & "")
                Else
                    lstSearch.Items.Add(dr("generated_code") & "-" & dr("description") & "")
                End If
            Next
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Stockcodes Found"
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub
    Public Sub SelectStockCodes()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, "-")
        If hdWhichStockCode.Value = "FromStockCode" Then
            txtFromStockCode.Text = arrArray(0)
        ElseIf hdWhichStockCode.Value = "ToStockCode" Then
            txtToStockCode.Text = arrArray(0)
        End If
        'ShowHide()
        txtSearch.Text = String.Empty
        lstSearch.Items.Clear()
        LookupMain.ShowOnPageLoad = False
    End Sub
    Private Sub Run()
        Dim listBranches As String = String.Empty
        Dim branches As String = String.Empty
        Dim ReturnString As String = String.Empty
        Dim username As String = String.Empty
        Dim email As String = String.Empty


        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
            email = Session("email")
        End If

        If chkAll.Checked = False Then
            If lstBranches.SelectedItems.Count = 0 Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select the branch to continue"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If


        For i = 0 To lstBranches.Items.Count - 1
            If chkAll.Checked = True Then
                branches += lstBranches.Items(i).Text & ","
            Else
                If lstBranches.Items(i).Selected = True Then
                    branches += lstBranches.Items(i).Text & ","

                End If
            End If

        Next
        listBranches = branches.Remove(branches.Length - 1)

        Dim soh As New SOHReport
        soh.Quantities = cboQuantities.Text
        soh.Type = cboType.Text
        soh.From = cboFrom.Text
        soh.Mastercode = chkMasterCode.Checked
        soh.Blocked = chkBlocked.Checked
        soh.FromStockCode = txtFromStockCode.Text
        soh.ToStockCode = txtToStockCode.Text
        soh.Branches = listBranches
        soh.IsAdmin = Session("is_pcm_admin")
        soh.IPAddress = Session("ipaddress")

        Dim json As String = JsonConvert.SerializeObject(soh)
        Try
            ReturnString = _reports.RunSOHReport(email, username, json)
            If ReturnString <> "Success" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something went wrong"
                dxPopUpError.ShowOnPageLoad = True
            Else
                clearForm()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Your report is running! Please check it on View Reports Page once it has completed."
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub clearForm()
        cboQuantities.SelectedIndex = 0
        cboType.SelectedIndex = 0
        cboFrom.SelectedIndex = 0
        txtFromStockCode.Text = String.Empty
        txtToStockCode.Text = String.Empty
        chkMasterCode.Checked = False
        chkBlocked.Checked = False
        chkAll.Checked = False
        lstBranches.UnselectAll()
        BindDropdown()
        ShowHide()
    End Sub
    Private Sub BindDropdown()
        cboQuantities.Items.Clear()
        cboQuantities.Items.Add("All")
        cboQuantities.Items.Add("Positive")
        cboQuantities.Items.Add("Zero")
        cboQuantities.Items.Add("Negative")
        cboQuantities.Items.Add("Positive And Negative")
        cboQuantities.SelectedIndex = 0

        cboType.Items.Clear()
        cboType.Items.Add("Range")
        cboType.Items.Add("Full")
        cboType.SelectedIndex = 0

        cboFrom.Items.Clear()
        cboFrom.Items.Add("Normal Stock")
        'cboFrom.Items.Add("LayBye Stock")
        'cboFrom.Items.Add("All")
        cboFrom.SelectedIndex = 0

    End Sub
End Class