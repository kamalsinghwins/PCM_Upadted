Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.IO


Public Class ReportsPTP
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Shared _status As String = String.Empty

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CollectionsAdmin) Then
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

            cboStatus.Items.Add("PTP")
            cboStatus.Items.Add("Contact Investigation")
            cboStatus.Items.Add("Under Investigation")
            cboStatus.Items.Add("Legal")
            cboStatus.Items.Add("Out of Queue")
            cboStatus.Items.Add("Debt Review")
            cboStatus.Items.Add("All Active")
            cboStatus.Items.Add("Fraud")
            cboStatus.Text = "PTP"
            Session.Remove("ReportsCollection")
            grdPTP.Visible = False
            dxGrid.Visible = False
            grdContactLevel.Visible = False
            cmdExportPDF.Visible = False
            cmdExportExcel.Visible = False
            cmdExportCSV.Visible = False
        End If

    End Sub
    Private Sub ReportsPTP_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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
    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        dxGrid.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.DataSource = data
        dxGrid.EndUpdate()

    End Sub
    Protected Sub grdPTP_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdPTP.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        grdPTP.DataSource = data
        grdPTP.EndUpdate()

    End Sub
    Protected Sub Run()
        Session.Remove("ReportsCollection")

        If cboStatus.Text = "PTP" Then
            grdPTP.DataBind()
            dxGrid.Visible = False
            grdPTP.Visible = True
            cmdExportPDF.Visible = True
            cmdExportExcel.Visible = True
            cmdExportCSV.Visible = True
            _status = "PTP"

        Else
            dxGrid.DataBind()
            'If cboStatus.Text = "Debt Review" Then
            '    dxGrid.Columns("date_of_last_payment").Visible = True
            'Else
            '    dxGrid.Columns("date_of_last_payment").Visible = False

            'End If
            grdPTP.Visible = False
            dxGrid.Visible = True
            cmdExportPDF.Visible = True
            cmdExportExcel.Visible = True
            cmdExportCSV.Visible = True
            _status = "Other"

        End If

        Session.Remove("ContactLevel")
        grdContactLevel.DataBind()
        grdContactLevel.Visible = True
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("ReportsCollection")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet

        If Not IsNothing(Session("ReportsCollection")) Then
            Return Session("ReportsCollection")
        End If

        Dim _BLayer As New ReportsBusinessLayer(Session("current_company"))

        Dim reports As New DataTable

        reports = _BLayer.ReturnAdminReportData(cboStatus.Text, txtFromDate.Text)

        Dim data As DataSet
        reports.TableName = "ReportsCollection"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("ReportsCollection") = data

        Return data
    End Function
    Public Function GetContactLevels()
        If Not IsNothing(Session("ContactLevel")) Then
            Return Session("ContactLevel")
        End If

        Dim _BLayer As New ReportsBusinessLayer(Session("current_company"))

        Dim levels As New DataTable
        levels = _BLayer.GetContactLevels()

        Dim data As DataSet
        levels.TableName = "ContactLevel"

        data = New DataSet()
        data.Tables.Add(levels)
        Session("ContactLevel") = data

        Return data

    End Function
    Protected Sub grdContactLevel_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdContactLevel.BeginUpdate()
        Dim gridview As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetCurrentContactLevel()
        grdContactLevel.DataSource = data
        grdContactLevel.EndUpdate()

    End Sub
    Private Function GetCurrentContactLevel() As DataTable
        Dim data As DataSet = GetContactLevels()
        If data IsNot Nothing Then
            Return data.Tables("ContactLevel")
        Else
            Return Nothing
        End If
    End Function

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        If _status = "PTP" Then
            Exporter.GridViewID = grdPTP.ID
        Else
            Exporter.GridViewID = dxGrid.ID
        End If

        Dim ps As New PrintingSystem()

        Dim link1 As New PrintableComponentLink(ps)
        link1.Component = Exporter

        Dim link2 As New PrintableComponentLink(ps)
        link2.Component = GridExporter2

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link1, link2})

        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToPdf(stream)
            WriteToResponse("filename", True, "pdf", stream)
        End Using
        ps.Dispose()
    End Sub
    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click

        If _status = "PTP" Then
            Exporter.GridViewID = grdPTP.ID

        Else
            Exporter.GridViewID = dxGrid.ID

        End If

        Dim ps As New PrintingSystem()

        Dim link1 As New PrintableComponentLink(ps)
        link1.Component = Exporter

        Dim link2 As New PrintableComponentLink(ps)
        link2.Component = GridExporter2

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link1, link2})

        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToXlsx(stream)
            WriteToResponse("filename", True, "xlsx", stream)
        End Using
        ps.Dispose()
    End Sub
    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click

        If _status = "PTP" Then
            Exporter.GridViewID = grdPTP.ID

        Else
            Exporter.GridViewID = dxGrid.ID
        End If
        Dim ps As New PrintingSystem()

        Dim link1 As New PrintableComponentLink(ps)
        link1.Component = Exporter

        Dim link2 As New PrintableComponentLink(ps)
        link2.Component = GridExporter2

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link1, link2})

        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("filename", True, "csv", stream)
        End Using
        ps.Dispose()
    End Sub
    Private Sub WriteToResponse(ByVal fileName As String, ByVal saveAsFile As Boolean, ByVal fileFormat As String, ByVal stream As MemoryStream)
        If Page Is Nothing OrElse Page.Response Is Nothing Then
            Return
        End If
        Dim disposition As String = If(saveAsFile, "attachment", "inline")
        Page.Response.Clear()
        Page.Response.Buffer = False
        Page.Response.AppendHeader("Content-Type", String.Format("application/{0}", fileFormat))
        Page.Response.AppendHeader("Content-Transfer-Encoding", "binary")
        Page.Response.AppendHeader("Content-Disposition", String.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat))
        Page.Response.BinaryWrite(stream.GetBuffer())
        Page.Response.End()
    End Sub

End Class