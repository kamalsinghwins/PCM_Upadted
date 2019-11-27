Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json
Public Class CashDiscrepancy
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer
    Dim _BLayer As New ReportsBusinessLayer
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CashDiscrepancy) Then
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

            Try
                ds = reports.GetBranches()
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    For Each drBranch As DataRow In ds.Tables(0).Rows
                        lstBranches.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                    Next
                End If
            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try

        End If
    End Sub
    Private Sub CashDiscrepancy_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "Run" Then
                RunCashDiscrepancy()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Public Sub RunCashDiscrepancy()
        Dim ReturnString As String = ""
        Dim listBranches As String = ""
        Dim branches As String = ""
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
                lblError.Text = "Please Select the branch"
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

        Dim cashDiscrepancy As New Entities.Reports.CashSummary
        cashDiscrepancy.IPAddress = Session("ipaddress")
        cashDiscrepancy.IsAdmin = Session("is_pcm_admin")
        cashDiscrepancy.Branches = listBranches
        Dim json As String = JsonConvert.SerializeObject(cashDiscrepancy)

        ReturnString = _BLayer.RunCashDiscrepancyReport(txtFromDate.Text, txtToDate.Text, json, email, username)

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
    Public Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
        chkAll.Checked = False
        lstBranches.UnselectAll()
    End Sub

End Class