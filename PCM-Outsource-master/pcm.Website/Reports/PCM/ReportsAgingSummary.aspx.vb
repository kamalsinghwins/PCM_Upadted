Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class ReportsAgingSummary
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AgingSummary) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

    End Sub
    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer

        dxGrid.DataSource = _NewReport.ReturnAgingSummary(txtAmount.Text)
        dxGrid.AutoGenerateColumns = True
    End Sub
    Protected Sub dxGrid_DataBinding_Calls(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer

        dxGridCalls.DataSource = _NewReport.GetAccountsToBeCalled(txtAmount.Text)
        dxGridCalls.AutoGenerateColumns = True
    End Sub
    Protected Sub dxGrid_DataBinding_Section(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer

        dxGridSections.DataSource = _NewReport.GetAccountsSections()
        dxGridSections.AutoGenerateColumns = True
    End Sub
    Protected Sub dxGrid_DataBinding_SectionActiveBalance(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer

        dxGridSectionsActiveBalance.DataSource = _NewReport.GetAccountsSectionsActiveBalance()
        dxGridSectionsActiveBalance.AutoGenerateColumns = True
    End Sub
    'Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
    '    Exporter.WritePdfToResponse()
    'End Sub

    'Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
    '    Exporter.WriteXlsxToResponse()
    'End Sub

    'Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
    '    Exporter.WriteCsvToResponse()
    'End Sub
    Private Sub ReportsAgingSummary_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub Run()
        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Amount: " & txtAmount.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Aging Summary Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        dxGrid.DataBind()
        dxGridCalls.DataBind()
        dxGridSections.DataBind()
        dxGridSectionsActiveBalance.DataBind()
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
End Class