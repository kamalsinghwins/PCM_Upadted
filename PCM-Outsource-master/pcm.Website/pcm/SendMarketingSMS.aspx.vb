Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports pcm.SMSPortal
Imports System.Globalization

Public Class SendMarketingSMS
    Inherits System.Web.UI.Page
    Dim _ReportBL As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
    Dim _SMSLog As SMSLogBL = New SMSLogBL
    Dim _blErrorLogging As New ErrorLogBL
    Dim _Logging As UsersBL = New UsersBL
    Dim _SendSMS As SMSService = New SMSService


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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.SendMarketingSMS) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Dim _BLayer As New GeneralHOBL()
            Dim _dt As DataTable

            _dt = _BLayer.GetBranches

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
            Next

            loadDefaults()
            HideDateTime()

        End If
    End Sub

    Private Sub SendMarketingSMS_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try
            If hdWhichButton.Value = "PhoneNumbersBuy" Then
                HideBranch()
            End If

            If hdWhichButton.Value = "PhoneNumbersAll" Then
                HideBranch()
            End If

            If hdWhichButton.Value = "Branch" Then
                ShowBranch()
            End If

            If hdWhichButton.Value = "Now" Then
                HideDateTime()
            End If

            If hdWhichButton.Value = "Later" Then
                ShowDateTime()
            End If
            If hdWhichButton.Value = "Save" Then
                SendSMS()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub
    Public Sub HideBranch()

        cboBranch.Visible = False
        If radNow.Checked = True Then
            HideDateTime()
        Else
            ShowDateTime()
        End If
    End Sub

    Public Sub ShowBranch()
        cboBranch.Visible = True
        If radNow.Checked = True Then
            HideDateTime()
        Else
            ShowDateTime()
        End If
    End Sub

    Public Sub HideDateTime()
        lblDate.Visible = False
        'lblTime.Visible = False
        txtDate.Visible = False
        txtTime.Visible = False
        If radBranch.Checked = True Then
            cboBranch.Visible = True
        Else
            cboBranch.Visible = False
        End If

    End Sub
    Public Sub ShowDateTime()
        lblDate.Visible = True
        'lblTime.Visible = True
        txtDate.Visible = True
        txtTime.Visible = True
        If radBranch.Checked = True Then
            cboBranch.Visible = True
        Else
            cboBranch.Visible = False
        End If

    End Sub

    Public Sub loadDefaults()

        txtDate.Text = Format(Now, "yyyy-MM-dd")
        txtTime.Text = Format(Now, "HH:mm tt")

        radCellPhoneNumbersBuy.Checked = True
        radNow.Checked = True
        txtMessage.Text = "RAGE:"
        cboBranch.Visible = False

    End Sub

    Protected Sub SendSMS()
        Dim _dt As New DataTable
        Dim type As String = String.Empty
        Dim default_date As String = String.Empty
        Dim default_time As String = String.Empty
        Dim message As String = String.Empty

        Dim branchCode As String = Mid(cboBranch.Value, 1, 3)

        If radCellPhoneNumbersBuy.Checked = True Then
            type = "PhoneNumbersBuy"
        ElseIf radCellPhoneNumbersAll.Checked = True Then
            type = "PhoneNumbersAll"
        ElseIf radBranch.Checked = True Then
            type = "Branch"
        End If

        If type = "Branch" AndAlso branchCode = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a branch"
            dxPopUpError.ShowOnPageLoad = True
            cboBranch.Visible = True
            cboBranch.Focus()
            If radLater.Checked = True Then
                txtDate.Visible = True
                txtTime.Visible = True
            End If
            Exit Sub
        End If

        If radNow.Checked = True Then
            default_date = Date.Now.ToString("dd/MM/yyyy")
            'default_date = Mid(DateTime.Now, 9, 2) & "/" & Mid(DateTime.Now, 6, 2) & "/" & Mid(DateTime.Now, 1, 4)
            default_time = Date.Now.ToString("HH:mm")
        Else
            If txtDate.Text = "" Or txtTime.Text = "" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please enter date and time "
                dxPopUpError.ShowOnPageLoad = True
                txtDate.Visible = True
                txtTime.Visible = True
                Exit Sub
            End If
            default_date = Mid(txtDate.Text, 9, 2) & "/" & Mid(txtDate.Text, 6, 2) & "/" & Mid(txtDate.Text, 1, 4)
            'default_date = Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture)
            default_time = Convert.ToDateTime(txtTime.Text).ToString("HH:mm")
        End If

        If txtMessage.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter a message"
            dxPopUpError.ShowOnPageLoad = True

            If type = "Branch" Then
                cboBranch.Visible = True
            End If

            If radLater.Checked = True Then
                txtDate.Visible = True
                txtTime.Visible = True
            End If
            Exit Sub
        Else
            message = txtMessage.Text
        End If

        Dim result As New SMSServiceResponse
        _dt = _ReportBL.GetNumbersForSMS(type, branchCode)

        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            Dim username As String = ConfigurationManager.AppSettings("SMSPortalUserName")
            Dim password As String = ConfigurationManager.AppSettings("SMSPortalPassword")
            result = _SendSMS.Send_SMS_BY_DS_DS(username, password, default_date, default_time, message, _dt)
            result.UserName = Session("username")
            result.SentMessage = message

            'SMS Logging
            _SMSLog.SMSLogging(result)

            If result.Status = True Then
                result.ErrorMessage = "Messages Result: Total Phone Numbers=" & result.TotalNumbers & " & Failed Phone Numbers=" & result.TotalFailedNumbers
            End If

            dxPopUpError.HeaderText = If(result.Status = True, "Success", "Error")
            dxPopUpError.ShowOnPageLoad = True
            lblError.Text = result.ErrorMessage
            loadDefaults()
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "No Numbers found"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        Exit Sub
    End Sub
End Class
