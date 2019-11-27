Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.Export
Public Class GrdEmployeeClocking
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _BLayer As New StockcodesHOBL
    Dim reports As New ReportsBusinessLayer
    Dim RG As New Entities.CommonFunctions.clsCommon
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeClockingReport) Then
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
    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "Search" Then
                grdEmployeeSearch.DataBind()

            End If

            If hdWhichButton.Value = "Select" Then
                SelectEmployee()

            End If

            If hdWhichButton.Value = "Run" Then
                Session.Remove("GridEmployeeClocking")
                gvMaster.DataBind()

            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Protected Sub grdEmployeeSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdEmployeeSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim dt As DataTable
        Dim employee As String
        employee = txtEmployeeSearch.Text

        dt = reports.GetEmployeesBySearch(employee)
        gridView.KeyFieldName = "employee_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = dt
        grdEmployeeSearch.EndUpdate()

    End Sub

    Protected Sub SelectEmployee()

        Dim selectedValues = New List(Of Object)()

        selectedValues = grdEmployeeSearch.GetSelectedFieldValues("employee_number")

        If selectedValues.Count > 0 Then
            txtEmployee.Text = selectedValues(0)
            pcMain.ShowOnPageLoad = False
        End If
    End Sub

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "employee_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        gvMaster.EndUpdate()

    End Sub

    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("Master")
        Else
            Return Nothing
        End If
    End Function


    Private Function GetData() As DataSet

        If Not IsNothing(Session("GridEmployeeClocking")) Then
            Return Session("GridEmployeeClocking")
        End If

        Dim masterData As New DataSet
        Dim StartDate As String
        Dim EndDate As String
        Dim Employee As String

        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        Employee = txtEmployee.Text


        masterData = reports.GetGridClockingEmployees(StartDate, EndDate, Employee)

        masterData.Tables(0).TableName = "Master"
        masterData.Tables(1).TableName = "Details"
        masterData.Relations.Add("MasterDetail", masterData.Tables(0).Columns("employee_number"), masterData.Tables(1).Columns("employee_number"))
        Session("GridEmployeeClocking") = masterData

        'Dim _BLUsers As New clsUsers(Session("database"))
        '_BLUsers.InsertUserRecord(Session("loggedinuser"), Session("is_admin"), Session("ipaddress"), "Run Graph", "grd_employee_clocking", "")

        'End If
        Return masterData
    End Function

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

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "guid" 'dataView.Table.PrimaryKey(0).ColumnName
        gridView.DataSource = dataView
    End Sub

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