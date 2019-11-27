Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class ManageEmployees
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

        If HttpContext.Current.IsDebuggingEnabled Then
            Session("current_company") = "010"
        Else
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageEmployees) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        'Me.Form.DefaultButton = cmdAccept.UniqueID

        Page.Server.ScriptTimeout = 300

        If (Not IsPostBack) Then
            InitializeControls()
        End If

    End Sub

    Private Sub InitializeControls()

        Try
            For i As Integer = 0 To cboClockNumber.Items.Count - 1
                Dim item As ListEditItem = cboClockNumber.Items(i)
                cboClockNumber.Items.Remove(item)
            Next

        Catch ex As Exception

        End Try

        cboClockNumber.Text = ""
        txtAccountNumber.Text = ""
        txtBranchCode.Text = ""
        txtFirstName.Text = ""
        txtLastLogin.Text = ""
        txtIDNumber.Text = ""
        txtLastLogout.Text = ""
        txtLastName.Text = ""
        txtCellphone.Text = ""
        txtEmailAddress.Text = ""
        chkEnabled.Checked = False
        chkLoggedIn.Enabled = False
        chkLoggedIn.Checked = False
        chkCasual.Checked = False
    End Sub

    Protected Sub FilterMinLengthSpinEdit_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        cboClockNumber.FilterMinLength = 2

    End Sub

    Protected Sub ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)

        If IsPostBack Then

            Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

            'Don't scroll
            If e.EndIndex > 9 Then
                Exit Sub
            End If

            Dim _BLayer As New ShopDownloadsBL()

            Dim dt As DataTable = _BLayer.GetEmployeeClockNumbers(e.Filter)

            Session.Remove("employees")

            Session("employees") = dt

            comboBox.DataSource = dt
            comboBox.DataBind()

        End If
    End Sub

    Protected Sub ASPxComboBox_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)
        Dim value As Long = 0
        If e.Value Is Nothing OrElse (Not Int64.TryParse(e.Value.ToString(), value)) Then
            Return
        End If

        Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

        Dim _BLayer As New ShopDownloadsBL()

        Dim dt As DataTable = _BLayer.GetEmployeeClockNumbers(e.Value.ToString)

        Session.Remove("employees")

        Session("employees") = dt

        comboBox.DataSource = dt
        comboBox.DataBind()

    End Sub


    Private Sub ManageEmployees_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(sender As Object, e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

        Dim _BLayer As New UsersBL()
        Dim _dt As New DataTable

        If hdWhichButton.Value = "cboClockNumbers" Then
            Dim dt As DataTable = Session("employees")

            Dim a As EnumerableRowCollection(Of DataRow) =
                    From p In dt Where p.Item("employee_number") = cboClockNumber.Text
                    Select p

            Try
                txtFirstName.Text = CType(a.ElementAt(0), DataRow).Item("first_name") & ""
            Catch ex As Exception
                txtFirstName.Text = ""
            End Try

            Try
                txtLastName.Text = CType(a.ElementAt(0), DataRow).Item("last_name") & ""
            Catch ex As Exception
                txtLastName.Text = ""
            End Try

            Try
                txtIDNumber.Text = CType(a.ElementAt(0), DataRow).Item("id_number") & ""
            Catch ex As Exception
                txtIDNumber.Text = ""
            End Try

            Try
                txtEmailAddress.Text = CType(a.ElementAt(0), DataRow).Item("email_address") & ""
            Catch ex As Exception
                txtEmailAddress.Text = ""
            End Try

            Try
                txtCellphone.Text = CType(a.ElementAt(0), DataRow).Item("cellphone") & ""
            Catch ex As Exception
                txtCellphone.Text = ""
            End Try

            Try
                txtBranchCode.Text = CType(a.ElementAt(0), DataRow).Item("bank_branch_code") & ""
            Catch ex As Exception
                txtBranchCode.Text = ""
            End Try

            Try
                txtAccountNumber.Text = CType(a.ElementAt(0), DataRow).Item("bank_account_number") & ""
            Catch ex As Exception
                txtAccountNumber.Text = ""
            End Try

            Try
                txtLastLogin.Text = CType(a.ElementAt(0), DataRow).Item("last_login") & ""
            Catch ex As Exception
                txtLastLogin.Text = ""
            End Try

            Try
                txtLastLogout.Text = CType(a.ElementAt(0), DataRow).Item("last_logout") & ""
            Catch ex As Exception
                txtLastLogout.Text = ""
            End Try

            Try
                If CType(a.ElementAt(0), DataRow).Item("is_active") = True Then
                    chkEnabled.Checked = True
                Else
                    chkEnabled.Checked = False
                End If
            Catch ex As Exception
                chkEnabled.Checked = False
            End Try

            Try
                If CType(a.ElementAt(0), DataRow).Item("is_logged_in") = True Then
                    chkLoggedIn.Checked = True
                Else
                    chkLoggedIn.Checked = False
                End If
            Catch ex As Exception
                chkLoggedIn.Checked = False
            End Try

            Try
                If CType(a.ElementAt(0), DataRow).Item("is_casual") = True Then
                    chkCasual.Checked = True
                Else
                    chkCasual.Checked = False
                End If
            Catch ex As Exception
                chkCasual.Checked = False
            End Try

            txtFirstName.Focus()

        End If

        If hdWhichButton.Value = "CalculateAge" Then
            CalculateAge()

        End If

        If hdWhichButton.Value = "Save" Then
            SaveEmployee()

        End If

        'Clear
        If hdWhichButton.Value = "Clear" Then
            'ClearScreen()
            InitializeControls()
        End If

    End Sub

    Private Sub CalculateAge()
        Dim Age As String
        Age = Mid$(txtIDNumber.Value, 1, 2)

        If Age > 0 And Age < 25 Then
            Age = "20" & Age
        ElseIf Age > 0 And age > 25 Then
            Age = "19" & Age
        End If

        Dim format As String = "yyyy"
        Dim Todaydate As Integer = DateTime.Now.ToString(format)

        If Todaydate - Convert.ToInt32(Age) >= 27 Then
            dxConfirmation.HeaderText = "Confirmation"
            lblConfirmation.Text = "Please note that this person 27 or older. Are you sure that you would like to add them?"
            dxConfirmation.ShowOnPageLoad = True
            Exit Sub
        End If
        SaveEmployee()


    End Sub

    Protected Sub SaveEmployee()
        Dim _BLayer As New UsersBL()

        dxConfirmation.ShowOnPageLoad = False
        Dim _EmployeeDetailsRequest As New EmployeeMaintenanceRequest

        _EmployeeDetailsRequest.ClockNumber = cboClockNumber.Text
        _EmployeeDetailsRequest.FirstName = txtFirstName.Text
        _EmployeeDetailsRequest.LastName = txtLastName.Text
        _EmployeeDetailsRequest.IDNumber = txtIDNumber.Text
        _EmployeeDetailsRequest.AccountNumber = txtAccountNumber.Text
        _EmployeeDetailsRequest.BranchCode = txtBranchCode.Text
        _EmployeeDetailsRequest.Enabled = chkEnabled.Checked
        _EmployeeDetailsRequest.EmailAddress = txtEmailAddress.Text
        _EmployeeDetailsRequest.Cellphone = txtCellphone.Text
        _EmployeeDetailsRequest.Casual = chkCasual.Checked


        Dim _Response As BaseResponse

        _Response = _BLayer.UpdateEmployee(_EmployeeDetailsRequest)



        If _Response.Success = False Then
            dxPopUpError.HeaderText = "Error"
        Else
            dxPopUpError.HeaderText = "Success"
            InitializeControls()
        End If

        lblError.Text = _Response.Message

        'ClearScreen()

        dxPopUpError.ShowOnPageLoad = True
        dxConfirmation.ShowOnPageLoad = False
    End Sub
End Class