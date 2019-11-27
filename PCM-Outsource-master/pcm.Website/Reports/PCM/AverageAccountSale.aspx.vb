Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class AverageAccountSale
    Inherits System.Web.UI.Page
    Dim _NewReport As New ReportsBusinessLayer
    Dim getTransactionsResponse As New GetTransactionsResponse



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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AverageAccountSale) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            loadDefaults()
        End If
    End Sub

    Private Sub loadDefaults()
        averageSaleChart.Width = 1000

        txtFromDate.MaxDate = Format(Now, "yyyy-MM-dd")
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")

        txtToDate.MaxDate = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "GetTransactions" Then
            averageSaleChart.DataBind()
        End If

    End Sub
    Private Sub AverageAccountSale_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub transactionChart_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim dt As New DataTable
        getTransactionsResponse = GetTransactions()

        dt = getTransactionsResponse.dt
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            averageSaleChart.DataSource = dt

            '' calculating total sum,count and average
            '===================================================================
            Dim sum As Double
            Dim count As Double
            Dim average As Double
            Dim totalObject As Object
            totalObject = dt.Compute("Sum(sum)", String.Empty)
            sum = totalObject
            totalObject = dt.Compute("Sum(count)", String.Empty)
            count = totalObject
            average = sum / count

            lblTotalSum.Text = Math.Round(sum, 2)
            lblTotalCount.Text = count
            lblTotalAverage.Text = Math.Round(average, 2)

            '====================================================================
        Else
            averageSaleChart.DataSource = Nothing
        End If

        averageSaleChart.Titles(0).Text = "Total Account Count: " & getTransactionsResponse.TotalCount
    End Sub

    Public Function GetTransactions() As GetTransactionsResponse

        Return _NewReport.GetTransactions(txtFromDate.Text, txtToDate.Text)

    End Function

    'Protected Sub SetEndDate()
    '    Dim startDate As Date
    '    Dim maxDate As Date
    '    startDate = txtFromDate.Text
    '    maxDate = startDate.AddDays(180)
    '    If maxDate > Date.Now Then
    '        txtToDate.MaxDate = Date.Now
    '        txtToDate.Date = startDate.AddDays(1)

    '    Else
    '        txtToDate.MaxDate = startDate.AddDays(180)
    '        txtToDate.Date = startDate.AddMonths(1)
    '    End If

    '    txtToDate.MinDate = startDate
    'End Sub
End Class