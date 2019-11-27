Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.Web.UI
Imports System.IO
Public Class BlockStock
    Inherits System.Web.UI.Page
    Dim ds As New DataSet
    Dim _blockStock As New ReportsBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL


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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.BlockStock) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            GetBranches()
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Block" Then
            Block()
        End If

        If hdWhichButton.Value = "SetNostockDate" Then
            ShowBranchDate()
        End If
    End Sub
    Private Sub BlockStock_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub Block()
        Dim ReturnString As String = String.Empty
        Dim noStockUntil As String
        Dim branch As String = String.Empty

        If cboBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please Select the branch"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtDate.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please Select the date"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        branch = Mid(cboBranch.Text, 1, 3)
        noStockUntil = txtDate.Text

        Try

            ReturnString = _blockStock.BlockStock(branch, noStockUntil)

            If ReturnString <> "Success" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something went wrong"
                dxPopUpError.ShowOnPageLoad = True

            Else
                Clear()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "The stock has been blocked successfully"
                dxPopUpError.ShowOnPageLoad = True
            End If


        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
    Private Sub SetDate()
        Dim From As DateTime
        From = DateTime.Now
        txtDate.MinDate = From
    End Sub
    Private Sub ShowBranchDate()
        Dim dt As DataTable = Session("blockstock")
        Dim stockDate As DateTime
        Dim format As String = "yyyy-MM-dd"
        Dim noStock As String = String.Empty

        If Session("blockstock") IsNot Nothing Then

            Try
                Dim a As EnumerableRowCollection(Of DataRow) =
                    From p In dt Where p.Item("branch_code") = Mid(cboBranch.Text, 1, 3)
                    Select p
                stockDate = CType(a.ElementAt(0), DataRow).Item("no_stock_until") & ""
                noStock = stockDate.ToString(format)
                txtDate.Text = noStock

            Catch ex As Exception
                txtDate.Text = ""
            End Try
            SetDate()
        End If
    End Sub
    Private Sub GetBranches()
        ds = _blockStock.GetBranches()
        If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                cboBranch.Items.Add(dr("branch_code") & " - " & dr("branch_name"))
            Next
            Session("blockstock") = ds.Tables(0)
        End If

    End Sub
    Private Sub Clear()
        cboBranch.SelectedIndex = -1
        txtDate.Text = Format(Now, "yyyy-MM-dd")
    End Sub
End Class