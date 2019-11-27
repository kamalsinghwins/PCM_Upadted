Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class CreditCardAuth
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CreditCardAuth) Then
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
        Session.Remove("CreditCardAuth")
        gvReports.DataBind()
    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetReports()
        If data IsNot Nothing Then
            Return data.Tables("CreditCardAuth")
        Else
            Return Nothing
        End If
    End Function
    Private Function GetReports() As DataSet

        If Not IsNothing(Session("CreditCardAuth")) Then
            Return Session("CreditCardAuth")
        End If

        Dim GetAuthReports As New ReportsBusinessLayer

        Dim reports As New DataTable

        reports = GetAuthReports.GetCreditCardAuth(txtFromDate.Text, txtToDate.Text)

        Dim data As DataSet
        reports.TableName = "CreditCardAuth"

        data = New DataSet()
        data.Tables.Add(reports)
        Session("CreditCardAuth") = data

        Return data
    End Function

    Protected Sub gvReport_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReports.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "transaction_guid"
        gridView.DataSource = data
        gvReports.EndUpdate()

    End Sub

    Private Sub CreditCardAuth_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
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