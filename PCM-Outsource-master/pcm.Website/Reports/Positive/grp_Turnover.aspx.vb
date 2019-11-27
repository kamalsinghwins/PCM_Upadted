Imports pcm.BusinessLayer
Imports DevExpress.Web

Public Class grp_Turnover
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")

            drpDateRange.Items.Add("Today")
            drpDateRange.Items.Add("Yesterday")
            drpDateRange.Items.Add("This Month")
            drpDateRange.Items.Add("Custom Date Range")

            drpDateRange.Text = "Today"
        End If

        If HttpContext.Current.IsDebuggingEnabled Then
            Session("current_company") = "010"
        End If

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            End If
        End If

        Dim _NewReport As New ReportsBusinessLayer(Session("current_company"))

        Dim _dt As DataTable

        _dt = _NewReport.ReturnLastUpdateData()
        'pvGrid.DataSource = _dt

    End Sub

    Private Sub grp_Turnover_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub drpDateRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpDateRange.SelectedIndexChanged


    End Sub

    Protected Sub ASPxCallback1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback
        'Search
        If hdWhichButton.Value = "OnChange" Then
            If drpDateRange.Text = "Custom Date Range" Then
                pnlDate.Visible = True
                Exit Sub
            Else
                pnlDate.Visible = False
            End If

            Dim tmpYesterday As String
            tmpYesterday = Now.Date.AddDays(-1)
            'tmpYesterday = Now.Year.ToString("yyyy") & "-" & Now.Month.ToString("MM") & -
            'tmpYesterday.ToString("yyyy-MM-dd")
            'tmpYesterday = Mid$(tmpYesterday, 7, 4) & "-" & Mid$(tmpYesterday, 4, 2) & "-" & Mid$(tmpYesterday, 1, 2)

            Dim ThisMonth As String = Format(Now, "yyyy-MM") & "-01"

            Dim StartDate As String = Format(Now, "yyyy-MM-dd") 'Default to today
            Dim EndDate As String = Format(Now, "yyyy-MM-dd") 'Default to today

            If drpDateRange.Text = "Yesterday" Then
                StartDate = tmpYesterday
                EndDate = tmpYesterday
            End If
            If drpDateRange.Text = "This Month" Then
                StartDate = ThisMonth
                EndDate = Format(Now, "yyyy-MM-dd")
            End If

            txtFromDate.Text = StartDate
            txtToDate.Text = EndDate

           

        End If
    End Sub
End Class