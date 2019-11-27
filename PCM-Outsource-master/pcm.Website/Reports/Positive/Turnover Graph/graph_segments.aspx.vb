Imports System.Globalization
Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class graph_segments
    Inherits System.Web.UI.Page
    Dim _NewReport As New ReportsBusinessLayer
    Dim dt As New DataTable
    Dim _blErrorLogging As New ErrorLogBL

    Dim getSegmentsResponse As New GetSegmentsResponse

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.GraphSegments) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If





        If Not IsPostBack Then
            lstBranches.Items.Clear()
            lblFromDate.Visible = False
            lblToDate.Visible = False

            txtFromDate.Visible = False
            txtToDate.Visible = False


            cboDateRange.Items.Clear()
            cboDateRange.Items.Add("Today")
            cboDateRange.Items.Add("Yesterday")
            cboDateRange.Items.Add("This Month")
            cboDateRange.Items.Add("Custom Date Range")
            cboDateRange.Value = "Today"


            cboSegments.Items.Clear()
            cboSegments.Items.Add("Hours")
            cboSegments.Items.Add("Weeks")
            cboSegments.Items.Add("Months")
            cboSegments.Items.Add("Day of Week")
            cboSegments.Value = "Hours"


            Try
                dt = _NewReport.GetBranchDetails()
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each drBranch As DataRow In dt.Rows
                        lstBranches.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                    Next
                End If
            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try
        End If




    End Sub

    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "DateRange" Then
            DateRangeIndexChanged()
        End If

        If hdWhichButton.Value = "cmdRun" Then
            GetSegments()
        End If


    End Sub

    Protected Sub GetSegments()
        Dim tmpArray() As String
        Dim arrayWeekday(8) As String
        Dim arrayValue(8) As String

        Dim strBranch As String = String.Empty

        If cboSegments.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Segment type"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim dsData As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("segment")
        dt.Columns.Add("value")
        dsData.Tables.Add(dt)

        Dim DateRange As String = cboDateRange.Text
        Dim Segment As String = cboSegments.Text
        Dim AllBranches As Boolean

        If chkAll.Checked = False Then
            AllBranches = False

            'Get the branches
            strBranch = ""
            For mLoop As Integer = 0 To lstBranches.Items.Count - 1
                If lstBranches.Items(mLoop).Selected = True Then
                    'Don't include this branch
                    tmpArray = Split(lstBranches.Items(mLoop).Text, " - ")

                    strBranch &= If(strBranch <> "", " OR ", "") & "m.branch_code = '" & tmpArray(0) & "'"

                End If
            Next
        Else
            AllBranches = False
        End If

        Dim StartDate As String = Format(Now, "yyyy-MM-dd")
        Dim EndDate As String = Format(Now, "yyyy-MM-dd")


        Dim tmpYesterday As Date
        tmpYesterday = Now.Date.AddDays(-1).ToString("yyyy-MM-dd")

        Dim ThisMonth As String = Format(Now, "yyyy-MM") & "-01"

        Dim reformatted As String = tmpYesterday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

        If DateRange = "Today" Then
            StartDate = Format(Now, "yyyy-MM-dd")
            EndDate = Format(Now, "yyyy-MM-dd")
        End If
        If DateRange = "Yesterday" Then
            StartDate = reformatted
            EndDate = reformatted
        End If
        If DateRange = "This Month" Then
            StartDate = ThisMonth
            EndDate = Format(Now, "yyyy-MM-dd")
        End If

        If DateRange = "Custom Date Range" Then
            StartDate = txtFromDate.Text
            EndDate = txtToDate.Text
        End If

        Try

            getSegmentsResponse = _NewReport.GetSegments(StartDate, EndDate, DateRange, Segment, AllBranches, strBranch)

            If getSegmentsResponse IsNot Nothing Then
                Dim TotalCount As Double = getSegmentsResponse.TotalCount
                If getSegmentsResponse.TotalCount > 5 Then
                    chartSegments.Width = 700 + TotalCount * 30
                Else
                    chartSegments.Width = 1000
                End If

                chartSegments.DataSource = getSegmentsResponse.dt
                chartSegments.DataBind()

                'Fix the grid
                chartSegments.ChartAreas("ChartArea1").AxisX.Interval = 1
                Dim gd As New DataVisualization.Charting.Grid
                gd.LineWidth = 0
                chartSegments.ChartAreas("ChartArea1").AxisX.MajorGrid = gd
                chartSegments.Series("Series1").IsValueShownAsLabel = True

                chartSegments.Titles("Title1").Text = "Date Range: " & StartDate & " to " & EndDate

                If DateRange = "Custom Date Range" Then
                    lblFromDate.Visible = True
                    lblToDate.Visible = True
                    txtFromDate.Visible = True
                    txtToDate.Visible = True
                    EndDate = txtToDate.Text
                End If

                _NewReport.InsertUserRecord(Session("username"), Session("is_pcm_admin"), Session("ipaddress"), "Run Graph", "graph_segments", "")
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Public Sub DateRangeIndexChanged()
        If cboDateRange.Text = "Custom Date Range" Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            Exit Sub
        Else
            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub

End Class