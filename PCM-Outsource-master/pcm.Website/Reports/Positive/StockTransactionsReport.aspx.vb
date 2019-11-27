Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json
Public Class StockTransactionsReport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim stockReport As New ReportsBusinessLayer()

    Dim dsBranch As DataSet
    Dim dsCategories As DataSet
    Dim dtStockcodes As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.StockTransactionReport) Then
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

                txtFromDate.Text = Format(Now, "yyyy-MM-dd")
                txtToDate.Text = Format(Now, "yyyy-MM-dd")

                lstBranches.Items.Clear()
                cboItemcat1.Items.Clear()
                cboItemcat2.Items.Clear()
                cboItemcat3.Items.Clear()

                dsBranch = stockReport.GetBranches

                If dsBranch.Tables.Count > 0 AndAlso dsBranch.Tables(0).Rows.Count > 0 Then
                    For Each drBranch As DataRow In dsBranch.Tables(0).Rows
                        lstBranches.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                    Next
                End If


                cboItemcat1.Items.Add("ALL")
                'Update reflected from database change
                dsCategories = stockReport.GetCategories(1)

                If dsCategories.Tables.Count > 0 AndAlso dsCategories.Tables(0).Rows.Count > 0 Then
                    For Each drCategories As DataRow In dsCategories.Tables(0).Rows
                        cboItemcat1.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                    Next
                End If



                cboItemcat2.Items.Add("ALL")
                dsCategories = stockReport.GetCategories(2)

                If dsCategories.Tables.Count > 0 AndAlso dsCategories.Tables(0).Rows.Count > 0 Then
                    For Each drCategories As DataRow In dsCategories.Tables(0).Rows
                        cboItemcat2.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                    Next
                End If

                dsCategories.Clear()

                cboItemcat3.Items.Add("ALL")
                dsCategories = stockReport.GetCategories(3)

                If dsCategories.Tables.Count > 0 AndAlso dsCategories.Tables(0).Rows.Count > 0 Then
                    For Each drCategories As DataRow In dsCategories.Tables(0).Rows
                        cboItemcat3.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                    Next
                End If

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try

        End If
    End Sub
    Private Sub StockTransactionsReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "Run" Then
            Run()
        End If

        If hdWhichButton.Value = "FromStockCode" Then
            FromStockCode()
        End If

        If hdWhichButton.Value = "ToStockCode" Then
            ToStockCode()
        End If


        If hdWhichButton.Value = "SearchStockCode" Then
            SearchStockCode()
        End If

        If hdWhichButton.Value = "SelectStockCode" Then
            SelectStockCode()
        End If

    End Sub

    Public Sub FromStockCode()
        hdSearchWhat.Value = "from"
        searchStockcodePopup.ShowOnPageLoad = True

    End Sub

    Public Sub ToStockCode()
        hdSearchWhat.Value = "to"
        searchStockcodePopup.ShowOnPageLoad = True

    End Sub

    Public Sub SearchStockCode()
        Try

            dtStockcodes = stockReport.GetStockCodes(txtSearch.Text)
            If dtStockcodes.Rows.Count > 0 Then
                lstSearch.Items.Clear()

                For Each drSCs As DataRow In dtStockcodes.Rows
                    lstSearch.Items.Add(drSCs.Item("generated_code") & " - " & drSCs.Item("description") & "")
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

    Public Sub SelectStockCode()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")

        If hdSearchWhat.Value = "from" Then
            txtFromStockCode.Text = arrArray(0)
        Else
            txtToStockCode.Text = arrArray(0)
        End If
        txtSearch.Text = ""
        lstSearch.Items.Clear()
        searchStockcodePopup.ShowOnPageLoad = False

    End Sub

    Public Sub Run()

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


        Dim stockcodeTransaction As New Entities.Reports.StockTransaction
        stockcodeTransaction.FromStockcode = txtFromStockCode.Text.ToUpper
        stockcodeTransaction.ToStockcode = txtToStockCode.Text.ToUpper
        stockcodeTransaction.Category1 = cboItemcat1.Value
        stockcodeTransaction.Category2 = cboItemcat2.Value
        stockcodeTransaction.Category3 = cboItemcat3.Value
        stockcodeTransaction.Sales = chkSales.Checked
        stockcodeTransaction.CreditNotes = chkCn.Checked
        stockcodeTransaction.Refunds = chkRefunds.Checked
        stockcodeTransaction.IBTIn = chkIbtin.Checked
        stockcodeTransaction.IBTOut = chkIbtOut.Checked
        stockcodeTransaction.StockcodeAdjustments = chkStkAdj.Checked
        stockcodeTransaction.GoodsReceieved = chkGRV.Checked
        stockcodeTransaction.Branch = listBranches
        stockcodeTransaction.IPAddress = Session("ipaddress")
        stockcodeTransaction.IsAdmin = Session("is_pcm_admin")

        Dim json As String = JsonConvert.SerializeObject(stockcodeTransaction)

        Try

            ReturnString = stockReport.RunStockTransactionsReport(email, txtFromDate.Text, txtToDate.Text, json, username)

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

    Public Sub clearForm()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
        txtFromStockCode.Text = String.Empty
        txtToStockCode.Text = String.Empty
        cboItemcat1.SelectedIndex = -1
        cboItemcat2.SelectedIndex = -1
        cboItemcat3.SelectedIndex = -1
        chkSales.Checked = False
        chkCn.Checked = False
        chkRefunds.Checked = False
        chkIbtin.Checked = False
        chkIbtOut.Checked = False
        chkStkAdj.Checked = False
        chkGRV.Checked = False
        chkAll.Checked = False
        lstBranches.UnselectAll()
    End Sub
End Class