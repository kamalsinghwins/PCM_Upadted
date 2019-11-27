Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class BranchReport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim reports As New ReportsBusinessLayer
    Dim _BLayer As New ReportsBusinessLayer
    Dim ds As New DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.BranchReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            Session.Remove("Branches")

            'Session.Clear()
            gridBranches.DataBind()
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "Run" Then
                'RunCashDiscrepancy()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub BranchReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub gridBranches_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gridBranches.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridBranches.KeyFieldName = "branch_code"
        gridBranches.DataSource = data
        gridBranches.EndUpdate()

    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("Branches")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetReports() As DataSet
        If Not IsNothing(Session("Branches")) Then
            Return Session("Branches")
        End If
        Dim branches As New ReportsBusinessLayer
        Dim reports As New DataSet
        reports = branches.GetBranches(True)
        Dim data As DataSet
        reports.Tables(0).TableName = "Branches"
        data = New DataSet()
        data.Tables.Add(reports.Tables(0).Copy)
        Session("Branches") = data
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