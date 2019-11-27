Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class EmployeeReviews
    Inherits System.Web.UI.Page

    Dim _Employee As EmployeeBusinessLayer = New EmployeeBusinessLayer
    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
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
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.HREmployeeReviews) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            InitialiseValues()
            InitialiseSearchFields()
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "SelectReport" Then
            SelectReport()
        End If

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

        If hdWhichButton.Value = "Lookup" Then
            grdEmployeeSearch.DataBind()
        End If

        If hdWhichButton.Value = "EmployeeSelected" Then
            GetSelectedEmployeeDetails()
        End If

        If hdWhichButton.Value = "LookupClockNumber" Then
            GetEmployeeDetails()
        End If

    End Sub

    Public Sub InitialiseValues()
        cboWarning.Items.Clear()
        cboWarning.Items.Add("Late Coming")
        cboWarning.Items.Add("Absenteeism")
        cboWarning.Items.Add("Non Compliance with company Policy and Procedure")
        cboWarning.Items.Add("Stock Loss")
        cboWarning.Items.Add("Neglect of duties")
        cboWarning.Items.Add("Negligence")
        cboWarning.Items.Add("Poor customer service")
        cboWarning.Items.Add("Till short – over and under")
        cboWarning.Items.Add("Poor Performance")

        cboTypeOfReport.Items.Clear()
        cboTypeOfReport.Items.Add("Warning")
        cboTypeOfReport.Items.Add("Review")

        cboWarning.Visible = False
        rating.Visible = False
        lblWarning.Visible = False
        'txtEmployeeName.ClientEnabled = False

        Dim _BLayer As New GeneralHOBL()
        Dim _dt As DataTable

        _dt = _BLayer.GetBranches

        For i As Integer = 0 To _dt.Rows.Count - 1
            cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
        Next

        lblWarningExpiry.Visible = False
        txtExpiryDate.Visible = False

    End Sub

    Private Sub InitialiseSearchFields()
        cboSearchType.Items.Add("Employee Number")
        cboSearchType.Items.Add("First Name")
        cboSearchType.Items.Add("Last Name")
    End Sub

    Public Sub SelectReport()
        If cboTypeOfReport.Text = "Warning" Then
            lblWarning.Visible = True
            cboWarning.Visible = True
            lblWarningExpiry.Visible = True
            txtExpiryDate.Visible = True
            txtExpiryDate.Text = Format(Now, "yyyy-MM-dd")
            rating.Visible = False
        Else

            lblWarning.Visible = False
            cboWarning.Visible = False
            rating.Visible = True
            lblWarningExpiry.Visible = False
            txtExpiryDate.Visible = False
        End If
    End Sub


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

        grdEmployeeSearch.EndUpdate()
        cboTypeOfReport.Text = ""
        cboWarning.Text = ""
        cboWarning.Visible = False
        rating.Visible = False
        txtNotes.Text = ""
        txtEmployeeName.ClientEnabled = False
    End Sub

    Private Sub GetSelectedEmployeeDetails()


        Dim selectedValues = New List(Of Object)()

        selectedValues = Nothing

        selectedValues = grdEmployeeSearch.GetSelectedFieldValues("employee_number")
        If selectedValues.Count > 0 Then

            txtClockNumber.Text = selectedValues(selectedValues.Count - 1)
            selectedValues = Nothing

            selectedValues = grdEmployeeSearch.GetSelectedFieldValues("first_name")
            txtEmployeeName.Text = selectedValues(selectedValues.Count - 1)
            selectedValues = Nothing

            selectedValues = grdEmployeeSearch.GetSelectedFieldValues("last_name")
            txtEmployeeName.Text &= " " & selectedValues(selectedValues.Count - 1)

            txtClockNumber.ClientEnabled = False
            txtEmployeeName.ClientEnabled = False

            LookupMain.ShowOnPageLoad = False
            dxPopUpError.ShowOnPageLoad = False

        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Details Found"
            dxPopUpError.ShowOnPageLoad = True
        End If
    End Sub

    Private Sub GetEmployeeDetails()

        If txtClockNumber.Text = "" Then Exit Sub

        Dim _dt As DataTable = _Employee.GetEmployee(txtClockNumber.Text)

        Try
            If _dt.Rows.Count > 0 Then

                txtEmployeeName.Text = _dt.Rows(0)("first_name") & " " & _dt.Rows(0)("last_name")

                txtClockNumber.ClientEnabled = False
                txtEmployeeName.ClientEnabled = False

                LookupMain.ShowOnPageLoad = False
                dxPopUpError.ShowOnPageLoad = False
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Details Found"
                dxPopUpError.ShowOnPageLoad = True
                txtEmployeeName.ClientEnabled = True
            End If
        Catch ex As Exception
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Details Found"
            dxPopUpError.ShowOnPageLoad = True
            txtEmployeeName.ClientEnabled = True

        End Try


    End Sub

    Public Sub Save()
        Dim employeeNoteRequest As New EmployeeNoteRequest
        Dim baseResponse As New BaseResponse

        employeeNoteRequest.EmployeeName = Session("username")
        employeeNoteRequest.BranchCode = Mid(cboBranch.Text, 1, 3)
        employeeNoteRequest.TypeOfReport = cboTypeOfReport.Text
        employeeNoteRequest.Warning = cboWarning.Text
        employeeNoteRequest.Rating = If(cboTypeOfReport.Text = "Warning", Nothing, rating.Value)
        employeeNoteRequest.Note = txtNotes.Text
        employeeNoteRequest.EmployeeNumber = txtClockNumber.Text
        employeeNoteRequest.WarningExpiryDate = txtExpiryDate.Text

        baseResponse = _Employee.SaveNotes(employeeNoteRequest)
        If baseResponse.Success = True Then
            Clear()
            ClearPopup()
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Review Saved"
            dxPopUpError.ShowOnPageLoad = True
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = baseResponse.Message
            dxPopUpError.ShowOnPageLoad = True
            If employeeNoteRequest.TypeOfReport = "Warning" Then
                cboWarning.Visible = True
                rating.Visible = False
                lblWarning.Visible = True
                lblWarningExpiry.Visible = True
                txtExpiryDate.Visible = True
            Else
                rating.Visible = True
                cboWarning.Visible = False
                lblWarningExpiry.Visible = False
                txtExpiryDate.Visible = False
            End If
            Exit Sub
        End If

    End Sub

    Public Sub Clear()
        txtClockNumber.Text = ""
        txtEmployeeName.Text = ""
        cboTypeOfReport.Text = ""
        cboWarning.Text = ""
        cboWarning.Visible = False
        rating.Visible = False
        lblWarningExpiry.Visible = False
        txtExpiryDate.Visible = False

        txtNotes.Text = ""

        cboBranch.Text = ""

        txtClockNumber.ClientEnabled = True
        txtEmployeeName.ClientEnabled = True

    End Sub

    Private Sub ClearPopup()
        Dim _tmpCBO As ASPxComboBox
        _tmpCBO = LookupMain.FindControl("cboSearchType")
        _tmpCBO.Items.Clear()

        _tmpCBO.Items.Add("Employee Number")
        _tmpCBO.Items.Add("First Name")
        _tmpCBO.Items.Add("Last Name")


        ASPxEdit.ClearEditorsInContainer(LookupMain)
    End Sub

End Class