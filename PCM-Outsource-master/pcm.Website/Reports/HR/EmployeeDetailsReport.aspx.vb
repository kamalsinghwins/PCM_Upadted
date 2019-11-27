Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Public Class EmployeeDetailsReport
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeDetailsReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            'txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            'txtToDate.Text = Format(Now, "yyyy-MM-dd")

            gvEmployeeDetails.DataBind()
        End If
    End Sub


    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "cmdRun" Then
            Session.Remove("employedetails")
            gvEmployeeDetails.DataBind()

        End If
    End Sub
    Private Function GetData() As DataTable
        'Dim data As New DataSet
        If Not IsNothing(Session("employedetails")) Then
            'data = Session("employedetails")
            Return Session("employedetails")
        End If
        Dim reportsBusinessLayer As New ReportsBusinessLayer
        Dim employedetails As New DataTable


        Try
            employedetails = reportsBusinessLayer.GetSentEmployeeDetails().Copy()
            employedetails.TableName = "EmployeeDetails"

            'data.Tables.Add(employedetails)
            Session("employedetails") = employedetails
            Return Session("employedetails")
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Function

    Protected Sub gvEmployeeDetails_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvEmployeeDetails.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim data As DataTable = GetData()
        gridView.KeyFieldName = "employee_number"
        gridView.DataSource = data

        gvEmployeeDetails.EndUpdate()

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