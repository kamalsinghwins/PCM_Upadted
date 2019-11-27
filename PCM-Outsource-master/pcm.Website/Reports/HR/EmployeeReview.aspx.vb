Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class EmployeeReview
    Inherits System.Web.UI.Page
    Dim _Employee As EmployeeBusinessLayer = New EmployeeBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.HREmployeeReviewReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            opt1.Checked = True
            opt2.Checked = False
            txtFromDate.ClientEnabled = True
            txtToDate.ClientEnabled = True
            'GetReports()
            InitialiseSearchFields()

            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If
    End Sub

    Private Sub InitialiseSearchFields()
        cboSearchType.Items.Add("Employee Number")
        cboSearchType.Items.Add("First Name")
        cboSearchType.Items.Add("Last Name")
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "opt1_changed" Then
            opt1_changed()
        End If

        If hdWhichButton.Value = "opt2_changed" Then
            opt2_changed()
        End If

        If hdWhichButton.Value = "Run" Then
            GetReports()
        End If

        If hdWhichButton.Value = "Lookup" Then
            grdEmployeeSearch.DataBind()
        End If

        If hdWhichButton.Value = "EmployeeSelected" Then
            GetSelectedEmployeeDetails()
        End If

        'If hdWhichButton.Value = "Export" Then
        '    ExportGrid()
        'End If

    End Sub

    'Private Sub ExportGrid()

    '    '=====================================================================================================
    '    'LOGGING
    '    '=====================================================================================================
    '    Dim _Log As New PCMUserLog
    '    _Log.AccountNumber = ""
    '    _Log.ActionType = "Export Report"
    '    _Log.IPAddress = Session("LoggingIPAddress")
    '    _Log.SearchCriteria = "Start Date: " & txtFromDate.Text & " End Date: " & txtToDate.Text & " Clock Number: " & txtClockNumber.Text
    '    _Log.UserComment = ""
    '    _Log.UserName = Session("username")
    '    _Log.WebPage = "Employee Review Report"

    '    _Logging.WriteToLogPCM(_Log)
    '    '=====================================================================================================


    '    Exporter.WriteCsvToResponse()

    'End Sub
    Public Function GetReports()

        Dim _ReportList As DataSet
        _ReportList = _Employee.GetReviewsSummary(txtFromDate.Text, txtToDate.Text, txtClockNumber.Text)
        Session("_ReviewDS") = _ReportList
        gvReview.DataBind()
        If opt1.Checked = True Then
            opt1_changed()
        Else
            opt2_changed()
        End If
        Return Session("_ReviewDS")

    End Function

    Protected Sub grdEmployeesSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        grdEmployeeSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _tmpCBO As ASPxComboBox

        _tmpCBO = LookupMain.FindControl("cboSearchType")

        Dim _tmpTXT As ASPxTextBox

        _tmpTXT = LookupMain.FindControl("txtCriteria")

        Dim data As DataTable = _Employee.GetEmployees(_tmpCBO.Text, _tmpTXT.Text.ToUpper)

        gridView.KeyFieldName = "employee_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

    End Sub

    Private Sub GetSelectedEmployeeDetails()

        Dim selectedValues = New List(Of Object)()

        selectedValues = Nothing

        selectedValues = grdEmployeeSearch.GetSelectedFieldValues("employee_number")
        If selectedValues.Count > 0 Then

            txtClockNumber.Text = selectedValues(selectedValues.Count - 1)
            selectedValues = Nothing

            LookupMain.ShowOnPageLoad = False
            dxPopUpError.ShowOnPageLoad = False

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Details Found"
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub

    Protected Sub gvReview_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvReview.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataSet = Session("_ReviewDS")
        gridView.KeyFieldName = "employee_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data
        gvReview.EndUpdate()

    End Sub
    Public Sub opt1_changed()
        opt2.Checked = False
        txtClockNumber.Text = ""
        txtFromDate.ClientEnabled = True
        txtToDate.ClientEnabled = True

        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")

    End Sub

    Public Sub opt2_changed()
        opt1.Checked = False

        txtFromDate.ClientEnabled = False
        txtToDate.ClientEnabled = False
        txtFromDate.Text = ""
        txtToDate.Text = ""

    End Sub

    Protected Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Export Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Start Date: " & txtFromDate.Text & " End Date: " & txtToDate.Text & " Clock Number: " & txtClockNumber.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Employee Review Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Exporter.WriteCsvToResponse()

    End Sub
End Class