Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class TotalStockByBranch
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _branch As New ManageHOBL
    Dim dt As New DataTable
    Dim _report As New ReportsBusinessLayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not url.Contains("localhost") Then
            If Session("Username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.TotalStockByBranch) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            dt = _branch.GetAllBranches()
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    cboBranchType.Items.Add(dr("branch_code") & "-" & dr("branch_name"))
                Next
            End If

        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "Run" Then
                Run()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub TotalStockByBranch(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub Run()

        If cboBranchType.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select the branch"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Session.Remove("TotalBranchStock")
        gvReports.DataBind()
    End Sub
    Protected Sub gvReport_Databinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReports.BeginUpdate()
        Dim gridview As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim dt As DataTable = GetMasterData()
        gridview.KeyFieldName = "branch_code"
        gridview.DataSource = dt
        gridview.EndUpdate()
    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("TotalBranchStock")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetData() As DataSet
        If Not IsNothing(Session("TotalBranchStock")) Then
            Return Session("TotalBranchStock")
        End If

        dt = _report.GetTotalStockByBranch(Mid(cboBranchType.Text, 1, 3))
        Dim data As DataSet
        dt.TableName = "TotalBranchStock"
        data = New DataSet
        data.Tables.Add(dt)
        Session("TotalBranchStock") = data
        Return data
    End Function
End Class