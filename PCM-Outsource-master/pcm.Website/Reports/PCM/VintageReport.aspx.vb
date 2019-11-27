Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json

Public Class VintageReport
    Inherits System.Web.UI.Page
    Dim _AgeAnalysis As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitialiseValues()
        End If

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.VintageReport) Then
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

    End Sub
    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Public Sub InitialiseValues()
        cboYear.Items.Add("2010")
        cboYear.Items.Add("2011")
        cboYear.Items.Add("2012")
        cboYear.Items.Add("2013")
        cboYear.Items.Add("2014")
        cboYear.Items.Add("2015")
        cboYear.Items.Add("2016")
        cboYear.Items.Add("2017")
        cboYear.Items.Add("2018")
        cboYear.Items.Add("2019")
        cboYear.Items.Add("2020")

        cboMonth.Items.Add("January")
        cboMonth.Items.Add("February")
        cboMonth.Items.Add("March")
        cboMonth.Items.Add("April")
        cboMonth.Items.Add("May")
        cboMonth.Items.Add("June")
        cboMonth.Items.Add("July")
        cboMonth.Items.Add("August")
        cboMonth.Items.Add("September")
        cboMonth.Items.Add("October")
        cboMonth.Items.Add("November")
        cboMonth.Items.Add("December")

        cboStatus.Items.Clear()
        cboStatus.Items.Add("A,L,D,DR,C,F,WO,B")
        cboStatus.Items.Add("ALL")
        cboStatus.Items.Add("ACTIVE")
        cboStatus.Items.Add("BLOCKED")
        cboStatus.Items.Add("DEBT REVIEW")
        cboStatus.Items.Add("DECEASED")
        cboStatus.Items.Add("DECLINED")
        cboStatus.Items.Add("FRAUD")
        cboStatus.Items.Add("LEGAL")
        cboStatus.Items.Add("PENDING")
        cboStatus.Items.Add("SUSPENDED")
        cboStatus.Items.Add("WRITE-OFF")

    End Sub

    Private Sub chkThick_Click()
        If chkthick.Value = 1 Then
            txtScore.Text = ""
            txtScore.Enabled = False
        Else
            txtScore.Enabled = True
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "Run" Then
                RunReport()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub RunReport()
        Dim username As String = String.Empty
        Dim ReturnString As String = ""
        Dim email As String = String.Empty

        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
            email = Session("email")
        End If

        If cboMonth.Value = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Month."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        If cboYear.Value = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Year."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        If cboStatus.Value = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Status."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtFile.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a valid file."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If



        Dim reportRequest As New ReportRequest
            Dim reportResponse As New ReportResponse
        reportRequest.Month = cboMonth.Text
        reportRequest.Year = cboYear.Text
        reportRequest.Status = cboStatus.Text
        reportRequest.FileName = txtFile.Text
        reportRequest.Score = txtScore.Text
            reportRequest.CheckThickFilesOnly = chkthick.Checked
            reportRequest.CheckMaleOnly = chkMale.Checked
            reportRequest.CheckIncludeAllPeriods = chkAllPeriods.Checked
            reportRequest.CheckZeroes = chkZeroes.Checked
        reportRequest.IsAdmin = Session("is_pcm_admin")
        reportRequest.IPAddress = Session("ipaddress")

        Dim json As String = JsonConvert.SerializeObject(reportRequest)

        ReturnString = _AgeAnalysis.RunVintageReport(email, username, json)


        If ReturnString <> "Success" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something went wrong"
                dxPopUpError.ShowOnPageLoad = True

            Else
                clearForm()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Your report is running! Please look for the completed report in the View Reports Page after it is done."
                dxPopUpError.ShowOnPageLoad = True
            End If


    End Sub

    Public Sub clearForm()
        txtFile.Text = String.Empty
        txtScore.Text = String.Empty
        chkthick.Checked = False
        chkMale.Checked = False
        chkAllPeriods.Checked = False
        chkZeroes.Checked = False
        cboMonth.SelectedIndex = -1
        cboYear.SelectedIndex = -1
        cboStatus.SelectedIndex = -1
    End Sub
End Class