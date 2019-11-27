Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.Export


Public Class EmployeesPerStore
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.EmployeePerStore) Then
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
            radAll.Checked = True
        End If
    End Sub

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "GetEmployees" Then
            Session.Remove("EmployeeStore")
            Try
                gvMaster.DataBind()

            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try

        End If

    End Sub

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "branch_code" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        gvMaster.EndUpdate()

    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("MasterEmployeeData")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetData() As DataSet
        If Not IsNothing(Session("EmployeeStore")) Then
            Return Session("EmployeeStore")
        End If

        Dim GetEmployees As New ReportsBusinessLayer

        Dim Permanent As Boolean
        Dim Casual As Boolean
        Dim All As Boolean
        Dim StartDate As String
        Dim EndDate As String


        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        Permanent = radPermanent.Checked
        Casual = radCasual.Checked
        All = radAll.Checked
        Dim masterData As New DataSet

        masterData = GetEmployees.GetMasterEmployeesPerStore(Permanent, Casual, All, StartDate, EndDate)

        masterData.Tables(0).TableName = "MasterEmployeeData"
        masterData.Tables(1).TableName = "Details"
        masterData.Relations.Add("MasterDetail", masterData.Tables(0).Columns("branch_code"), masterData.Tables(1).Columns("branch_code"))

        Session("EmployeeStore") = masterData

        'GetEmployees.InsertUserRecord(Session("username"), Session("is_pcm_admin"), Session("ipaddress"), "Run Graph", "grd_employees_per_store", "")


        Return masterData
    End Function




    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "branch_code"
        gridView.DataSource = dataView
    End Sub


    Private Function GetDetailData(ByVal masterRowKey As Object) As DataView
        Dim data As DataSet = GetData()

        If data IsNot Nothing Then
            Dim detail As DataTable = data.Tables("Details")
            Dim columnName As String = data.Relations("MasterDetail").ParentColumns(0).ColumnName
            Return New DataView(detail, String.Format("[{0}] = '{1}'", columnName, masterRowKey), String.Empty, DataViewRowState.CurrentRows)
        Else
            Return Nothing
        End If
    End Function

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse(New XlsxExportOptionsEx() With {.ExportType = ExportType.WYSIWYG})

    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse(New CsvExportOptionsEx() With {.ExportType = ExportType.WYSIWYG})
    End Sub

End Class