Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class Main
    Inherits System.Web.UI.Page
    Dim _Survey As SurveyBusinessLayer = New SurveyBusinessLayer
    Public Property surveyname As String = String.Empty


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(Now.AddSeconds(-1))
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")
        If IsPostBack = False Then
            CheckSurveyById()
            Dim sIPAddress As String
            sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If sIPAddress = "" Then sIPAddress = Request.ServerVariables("REMOTE_ADDR")
            Session("sIPAddress") = sIPAddress
        End If


    End Sub

    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim surveyEmployeeDetailsRequest As New SurveyEmployeeDetailsRequest
        Dim saveEmployeeResponse As New SaveEmployeeResponse
        surveyEmployeeDetailsRequest = SaveEmployeeDetails()


        'If Employee Test, check that clock number exists
        If Session("survey_type") = "Employee Test" Then
            saveEmployeeResponse = _Survey.CheckEmployeeExists(surveyEmployeeDetailsRequest)
            If saveEmployeeResponse.Success = False Then
                lblerror.Visible = True
                lblerror.Text = saveEmployeeResponse.Message
                lblerror.CssClass = "alert alert-danger display-block"
                Exit Sub
            End If
        End If

        'If surveyEmployeeDetailsRequest.ID <> "0000" Then
        '    'Check if the user has completed the questionnaire previously
        '    saveEmployeeResponse = _Survey.CheckForCompletedQuestionnaire(surveyEmployeeDetailsRequest, survey_id)
        '    If saveEmployeeResponse.Success = False Then
        '        lblerror.Visible = True
        '        lblerror.Text = saveEmployeeResponse.Message
        '        lblerror.CssClass = "alert alert-danger display-block"
        '        Exit Sub
        '    End If
        'End If
        Dim survey_id As String = Request.QueryString("survey_id")
        saveEmployeeResponse = _Survey.SaveSurveyEmployeeDetails(surveyEmployeeDetailsRequest, survey_id)
        If saveEmployeeResponse.Success = True Then
            Dim employeeSurveySession As New EmployeeSession
            employeeSurveySession.FirstName = surveyEmployeeDetailsRequest.FirstName
            employeeSurveySession.LastName = surveyEmployeeDetailsRequest.LastName
            employeeSurveySession.CompletedSurveyId = saveEmployeeResponse.CompletedSurveyId
            employeeSurveySession.SurveyId = saveEmployeeResponse.SurveyId
            employeeSurveySession.SurveyType = saveEmployeeResponse.SurveyType
            employeeSurveySession.SurveyName = saveEmployeeResponse.SurveyName
            employeeSurveySession.SurveyStartTime = saveEmployeeResponse.StartTime
            employeeSurveySession.TimeAllowed = saveEmployeeResponse.MaxTime
            employeeSurveySession.QuestionOffset = 0
            employeeSurveySession.SurveyQuestionsCount = saveEmployeeResponse.SurveyQuestionsCount
            Session("EmployeeInfo") = employeeSurveySession
            Session.Timeout = saveEmployeeResponse.MaxTime
            Response.Redirect("ExamPage.aspx")
        Else
            lblerror.Visible = True
            lblerror.Text = saveEmployeeResponse.Message
            lblerror.CssClass = "alert alert-danger display-block"
        End If

    End Sub

    Public Function SaveEmployeeDetails() As SurveyEmployeeDetailsRequest
        Dim _SurveyEmployeeDetailsRequest As New SurveyEmployeeDetailsRequest
        _SurveyEmployeeDetailsRequest.FirstName = txtFirstName.Text
        _SurveyEmployeeDetailsRequest.LastName = txtLastname.Text
        _SurveyEmployeeDetailsRequest.ID = txtIdNumber.Text
        _SurveyEmployeeDetailsRequest.ContactNumber = txtContactNumber.Text
        _SurveyEmployeeDetailsRequest.IPAdress = Session("sIPAddress")
        Return _SurveyEmployeeDetailsRequest
    End Function

    Public Sub CheckSurveyById()
        Dim baseresponse As New SurveyDetails

        Dim survey_id As String = Request.QueryString("survey_id")

        If survey_id = "" Then
            Response.Redirect("~/HR/surveyerrors.aspx")
        End If

        baseresponse = _Survey.CheckSurveyById(survey_id)
        If baseresponse.Success = False Then
            Response.Redirect("~/HR/surveyerrors.aspx")
        End If

        surveyname = baseresponse.SurveyName
        QuestionnaireHeading.InnerHtml = "This Questionnaire has a time limit of <strong>" & baseresponse.TimeLimit & " minutes</strong>. <br/>" &
            "The number of questions remaining will be shown on the bottom left of the screen.</br/>" &
            "Please make sure that you scroll down to the bottom of each page to see ALL the answer options.</br/>" &
            "<strong>Please complete ALL THE QUESTIONS!</strong></br/></br/>" &
            "<strong>You can only take this test ONCE!</strong>"

        If baseresponse.QuestionnaireID = "Pre-Employment Test" Then
            divIDClock.InnerText = "ID Number is required"
            txtIdNumber.Attributes.Add("placeholder", "ID Number")
            Session("survey_type") = "Pre-Employment Test"
        Else
            divIDClock.InnerText = "Clock Number is required"
            txtIdNumber.Attributes.Add("placeholder", "Clock Number")
            Session("survey_type") = "Employee Test"
        End If

    End Sub
End Class