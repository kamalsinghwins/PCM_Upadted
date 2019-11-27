Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities


Public Class graph_account_sales
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim getAccountSalesResponse As New GetAccountSalesResponse
    Dim _NewReport As New ReportsBusinessLayer
    Dim runningTotal As Long = 0
    Dim runningPayments As Long = 0
    Dim TotalStores As Long = 0
    Dim dt As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AccountSales) Then
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

            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
            deChart.Visible = False
            Panel1.Visible = False


            cboDateRange.Items.Clear()
            cboDateRange.Items.Add("Today")
            cboDateRange.Items.Add("Yesterday")
            cboDateRange.Items.Add("This Month")
            cboDateRange.Items.Add("Custom Date Range")

            'Try
            '    GetAccountSales()
            'Catch ex As Exception
            '    _blErrorLogging.ErrorLogging(ex)
            'End Try

        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "DateRange" Then
            DateRangeIndexChanged()
        End If

        If hdWhichButton.Value = "cmdRun" Then
            deChart.DataBind()
        End If

    End Sub
    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Function GenerateData() As DataTable
        Dim dt As New DataTable
        Dim tmpYesterday As String
        tmpYesterday = Now.Date.AddDays(-1).ToString("yyyy-MM-dd")

        Dim ThisMonth As String = Format(Now, "yyyy-MM") & "-01"
        Dim StartDate As String = Format(Now, "yyyy-MM-dd") 'Default to today
        Dim EndDate As String = Format(Now, "yyyy-MM-dd") 'Default to today

        If cboDateRange.Text = "Yesterday" Then
            StartDate = tmpYesterday
            EndDate = tmpYesterday
        End If
        If cboDateRange.Text = "This Month" Then
            StartDate = ThisMonth
            EndDate = Format(Now, "yyyy-MM-dd")
        End If
        If cboDateRange.Text = "Custom Date Range" Then
            StartDate = txtFromDate.Text
            EndDate = txtToDate.Text
        End If

        Try
            getAccountSalesResponse = _NewReport.GetAccountSales(StartDate, EndDate)
            If getAccountSalesResponse.dt IsNot Nothing AndAlso getAccountSalesResponse.dt.Rows.Count > 0 Then
                TotalStores = getAccountSalesResponse.TotalStores
                runningTotal = getAccountSalesResponse.RunningTotal
                runningPayments = getAccountSalesResponse.RunningPayments

                If TotalStores > 5 Then
                    deChart.Width = TotalStores * 30 + 700
                End If

                If chkPayments.Checked = False Then
                    deChart.Series.RemoveAt(1)
                    deChart.Series(0).Name = "Turnover"
                    deChart.Titles(2).Text = ""
                Else
                    deChart.Titles(2).Text = "Payments: " & runningPayments
                End If

                'Adding Titles to the chart control
                '===================================================================================
                deChart.Titles(0).Text = "Date Range: " & StartDate & " to " & EndDate
                deChart.Titles(1).Text = "Total: " & runningTotal
                '===================================================================================

                'Setting Alignment of the titles
                '===================================================================================
                deChart.Titles(0).Alignment = Drawing.StringAlignment.Near
                deChart.Titles(1).Alignment = Drawing.StringAlignment.Near
                deChart.Titles(2).Alignment = Drawing.StringAlignment.Near
                '===================================================================================

            Else
                deChart.Titles(0).Text = "Date Range: " & StartDate & " to " & EndDate
                deChart.Titles(1).Text = "Total: 0"
                deChart.Titles(2).Text = "Payments: 0"
                deChart.Width = 1000

            End If
            dt = getAccountSalesResponse.dt
            Panel1.Visible = True
            deChart.Visible = True
            If cboDateRange.Text = "Custom Date Range" Then
                lblFromDate.Visible = True
                lblToDate.Visible = True
                txtFromDate.Visible = True
                txtToDate.Visible = True
            Else
                lblFromDate.Visible = False
                lblToDate.Visible = False
                txtFromDate.Visible = False
                txtToDate.Visible = False
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
        Return dt
    End Function
    Public Sub DateRangeIndexChanged()
        If cboDateRange.Text = "Custom Date Range" Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            Panel1.Visible = False
            Exit Sub
        Else
            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
            Panel1.Visible = False
        End If
    End Sub
    Protected Sub deChart_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim ds As New DataTable
        ds = GenerateData()
        deChart.DataSource = ds

    End Sub
End Class