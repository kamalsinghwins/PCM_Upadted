Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Public Class SMSReport
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SMSReport) Then
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
        End If
    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("SMSErrorLogs")
        Else
            Return Nothing
        End If
    End Function

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "cmdRun" Then
            Session.Remove("smserror")
            gvSMSError.DataBind()
        End If


    End Sub
    Private Function GetData() As DataSet

        If Not IsNothing(Session("smserror")) Then
            Return Session("smserror")
        End If
        Dim reportsBusinessLayer As New ReportsBusinessLayer
        Dim smslogs As New DataTable
        Dim data As New DataSet

        Try

            smslogs = reportsBusinessLayer.GetSentSMSLog(txtFromDate.Text, txtToDate.Text).Copy()
            smslogs.TableName = "SMSErrorLogs"

            data.Tables.Add(smslogs)
            Session("smserror") = data
            Return data
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Function

    Protected Sub gvSMSError_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvSMSError.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "sms_log_id"
        gridView.DataSource = data

        gvSMSError.EndUpdate()

    End Sub



    Private Sub SMSReport_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

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