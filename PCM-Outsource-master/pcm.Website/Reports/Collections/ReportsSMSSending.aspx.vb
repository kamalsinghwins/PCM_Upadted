Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class ReportsSMSSending
    Inherits System.Web.UI.Page
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SMSSending) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            InitialiseValues()
        End If

    End Sub
    Private Sub ReportsSMSSending_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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
        Session.Remove("SMSSendingReport")
        grdSMS.DataBind()
    End Sub
    Private Function GetData() As DataSet
        If Not IsNothing(Session("SMSSendingReport")) Then
            Return Session("SMSSendingReport")
        End If
        Dim GetTasksReports As New ReportsBusinessLayer
        Dim reports As New DataTable
        reports = GetTasksReports.ReturnSMSReportData(cboType.Text, txtFromDate.Text, txtToDate.Text)

        Dim data As DataSet
        reports.TableName = "SMSSendingReport"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("SMSSendingReport") = data
        Return data
    End Function
    Private Sub InitialiseValues()
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")

        cboType.Items.Add("All")
        cboType.Items.Add("60 days Overdue")
        cboType.Items.Add("90 days Overdue")
        cboType.Items.Add("120 days Overdue")
        cboType.Items.Add("150 days Overdue")
        cboType.Items.Add("Birthday SMS")
        cboType.Items.Add("Card Reminder")
        cboType.Items.Add("Cash Card Birthday")
        cboType.Items.Add("PTP")
        cboType.Items.Add("Reminder To Buy")
        cboType.Items.Add("Statement")
        cboType.Text = "All"
    End Sub
    Protected Sub grdSMS_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdSMS.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.DataSource = data
        grdSMS.EndUpdate()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("SMSSendingReport")
        Else
            Return Nothing
        End If
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