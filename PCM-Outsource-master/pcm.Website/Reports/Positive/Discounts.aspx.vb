Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json
Public Class Discounts
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _reports As New ReportsBusinessLayer
    Dim ds As New DataSet
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.Discounts) Then
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

            Try
                ds = _reports.GetBranches()
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
    Private Sub Discounts_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
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
    Private Sub Run()
        If chkAll.Checked = False Then
            If lstBranches.SelectedItems.Count = 0 Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please Select the branch"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If
        End If
        Session.Remove("Discounts")
        gvReports.DataBind()
    End Sub
    Protected Sub gvReports_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReports.BeginUpdate()
        Dim gridview As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gvReports.DataSource = data
        gvReports.EndUpdate()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("Discounts")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet
        If Not IsNothing(Session("Discounts")) Then
            Return Session("Discounts")
        End If

        Dim branches As String = String.Empty
        Dim reports As New DataTable

        For i = 0 To lstBranches.Items.Count - 1
            If chkAll.Checked = True Then
                branches += "'" & Mid$(lstBranches.Items(i).Text, 1, 3) & "'" & ","
            Else
                If lstBranches.Items(i).Selected = True Then
                    branches += "'" & Mid$(lstBranches.Items(i).Text, 1, 3) & "'" & ","
                End If
            End If
        Next

        branches = branches.Remove(branches.Length - 1)
        reports = _reports.GetDiscountReports(txtFromDate.Text, txtToDate.Text, branches)
        Dim data As DataSet
        reports.TableName = "Discounts"
        data = New DataSet
        data.Tables.Add(reports)
        Session("Discounts") = data
        Return data

    End Function
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