Imports System.Globalization
Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class graph_current_turnover_new
    Inherits System.Web.UI.Page

    Dim _NewReport As New ReportsBusinessLayer
    Dim orderBy As String = String.Empty
    Dim dt As New DataTable
    Dim runningTotal As Long = 0
    Dim runningTotalProfit As Long = 0
    Dim TotalStores As Long = 0
    Private currentTurnoverResponse As New CurrentTurnoverResponse



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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CurrentNewTurnoverGraph) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then

            If Session("is_pcm_admin") = "True" Then
                cmdRunNow.Visible = True
            Else
                cmdRunNow.Visible = False
            End If

            txtDateFrom.Text = Format(Now, "yyyy-MM-dd")
            txtDateTo.Text = Format(Now, "yyyy-MM-dd")

            cboDateRange.Items.Add("Today")
            cboDateRange.Items.Add("Yesterday")
            cboDateRange.Items.Add("This Month")
            cboDateRange.Items.Add("Custom Date Range")

            cboOrderBy.Items.Add("By Branch Name")
            cboOrderBy.Items.Add("By Turnover")

            cboDateRange.Text = "Today"
            cboOrderBy.Text = "By Branch Name"

            txtDateFrom.Visible = False
            txtDateTo.Visible = False
            lblFromDate.Visible = False
            lblToDate.Visible = False

            deChart.DataBind()

        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If chkProfit.Checked = True Then
            lblProfit.Visible = True
            dProfit.Visible = True
            If deChart.Series.Count > 1 Then
                deChart.Series(1).Visible = True
            End If

        Else
            lblProfit.Visible = False
            dProfit.Visible = False
            If deChart.Series.Count > 1 Then
                deChart.Series(1).Visible = False
            End If
        End If

        If hdWhichButton.Value = "cboDateRangeIndexChanged" Then
            If cboDateRange.Text = "Custom Date Range" Then
                lblFromDate.Visible = True
                lblToDate.Visible = True
                txtDateFrom.Visible = True
                txtDateTo.Visible = True
                Exit Sub
            Else
                lblFromDate.Visible = False
                lblToDate.Visible = False
                txtDateFrom.Visible = False
                txtDateTo.Visible = False
            End If

            deChart.DataBind()
        End If

        If hdWhichButton.Value = "cboOrderByIndexChanged" Then
            deChart.DataBind()
        End If

        If hdWhichButton.Value = "cmdRun" Then
            deChart.DataBind()
        End If

        If hdWhichButton.Value = "cmdRunNow" Then
            Session("run_now") = True
            deChart.DataBind()
        End If

    End Sub
    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub deChart_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim ds As New DataTable

        If Session("run_now") = True Then
            ds = GenerateData()
        Else
            If cboDateRange.Text = "Today" Then
                ds = GetMaterializedView()
            Else
                ds = GenerateData()
            End If
        End If

        Session("run_now") = False

        deChart.DataSource = ds

    End Sub

    Private Function GenerateData() As DataTable
        Dim tmpYesterday As Date
        tmpYesterday = Now.Date.AddDays(-1).ToString("yyyy-MM-dd")
        Dim reformatted As String = tmpYesterday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Dim ThisMonth As String = Format(Now, "yyyy-MM") & "-01"
        Dim StartDate As String = Format(Now, "yyyy-MM-dd") 'Default to today
        Dim EndDate As String = Format(Now, "yyyy-MM-dd") 'Default to today

        If cboDateRange.Text = "Yesterday" Then
            StartDate = reformatted
            EndDate = reformatted
        End If
        If cboDateRange.Text = "This Month" Then
            StartDate = ThisMonth
            EndDate = Format(Now, "yyyy-MM-dd")
        End If
        If cboDateRange.Text = "Custom Date Range" Then
            StartDate = txtDateFrom.Text
            EndDate = txtDateTo.Text
        End If

        orderBy = cboOrderBy.Text
        currentTurnoverResponse = _NewReport.GenerateData(orderBy, StartDate, EndDate)
        If currentTurnoverResponse.GetDataResponse IsNot Nothing AndAlso currentTurnoverResponse.GetDataResponse.Rows.Count > 0 Then
            TotalStores = currentTurnoverResponse.TotalStores
            runningTotalProfit = currentTurnoverResponse.RunningTotalProfit
            runningTotal = currentTurnoverResponse.RunningTotal

            If TotalStores > 5 Then
                deChart.Width = 700 + TotalStores * 30
                deChart.Width = 700 + TotalStores * 30
            Else
                deChart.Width = 1000
                deChart.Width = 1000
            End If

            If TotalStores > 5 Then
                'Chart1.Width = TotalStores * 30 + 700
                deChart.Width = TotalStores * 30 + 700
            Else
                'Chart1.Width = 1000
                deChart.Width = 1000
            End If

            If chkProfit.Checked = False Then
                'Chart1.Series.RemoveAt(1)
                If deChart.Series.Count > 1 Then
                    deChart.Series.RemoveAt(1)
                End If

                lblProfit.Visible = False
                dProfit.Visible = False
            Else
                Dim totalprofit As String = runningTotalProfit.ToString("N")

                totalprofit = Mid(totalprofit, 1, Len(totalprofit) - 3)

                gProfit.DigitCount = Len(totalprofit)
                gProfit.Text = totalprofit

            End If

            Dim total As String = runningTotal.ToString("N")

            total = Mid(total, 1, Len(total) - 3)

            gTurnover.DigitCount = Len(total)
            gTurnover.Text = total
        End If



        Return currentTurnoverResponse.GetDataResponse

    End Function

    Private Function GetMaterializedView()


        orderBy = cboOrderBy.Text
        currentTurnoverResponse = _NewReport.GetCurrentTurnover(orderBy)

        If currentTurnoverResponse.GetDataResponse IsNot Nothing AndAlso currentTurnoverResponse.GetDataResponse.Rows.Count > 0 Then
            TotalStores = currentTurnoverResponse.TotalStores
            runningTotalProfit = currentTurnoverResponse.RunningTotalProfit
            runningTotal = currentTurnoverResponse.RunningTotal

            If TotalStores > 5 Then
                deChart.Width = 700 + TotalStores * 30
                deChart.Width = 700 + TotalStores * 30
            Else
                deChart.Width = 1000
                deChart.Width = 1000
            End If

            If TotalStores > 5 Then
                deChart.Width = TotalStores * 30 + 700
            Else
                deChart.Width = 1000
            End If

            If chkProfit.Checked = False Then
                If deChart.Series.Count > 1 Then
                    deChart.Series.RemoveAt(1)
                End If

                lblProfit.Visible = False
                dProfit.Visible = False
            Else
                Dim totalprofit As String = runningTotalProfit.ToString("N")

                totalprofit = Mid(totalprofit, 1, Len(totalprofit) - 3)

                gProfit.DigitCount = Len(totalprofit)
                gProfit.Text = totalprofit

            End If

            Dim total As String = runningTotal.ToString("N")

            total = Mid(total, 1, Len(total) - 3)

            gTurnover.DigitCount = Len(total)
            gTurnover.Text = total

        End If


        Return currentTurnoverResponse.GetDataResponse

    End Function
End Class