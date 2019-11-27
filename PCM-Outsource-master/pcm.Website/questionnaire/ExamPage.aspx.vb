Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.Configuration
Imports System.Web.Configuration

Public Class ExamPage
    Inherits System.Web.UI.Page
    Public Shared _Survey As SurveyBusinessLayer = New SurveyBusinessLayer
    Public Property survey_id As String
    Public Property employeeSession As EmployeeSession
    Public Property RemainingTime As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(Now.AddSeconds(-1))
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")
        employeeSession = Session("EmployeeInfo")
        If Session("EmployeeInfo") IsNot Nothing Then
            survey_id = employeeSession.SurveyId
            Dim CompletedSurveyId As String = employeeSession.CompletedSurveyId
            Dim SurveyStartTime = employeeSession.SurveyStartTime
            Dim MaxTimeAllowed As Integer = employeeSession.TimeAllowed
            RemainingTime = MaxTimeAllowed * 60 - (DateTime.Now - SurveyStartTime).TotalSeconds
        Else
            Response.Redirect("Main.aspx")
        End If
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetAllQuestionsBySurveyId() As GetAllQuestionsResponse
        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")
        Dim getAllQuestionsResponse As New GetAllQuestionsResponse
        If employeeSession IsNot Nothing Then
            getAllQuestionsResponse = _Survey.GetAllQuestionsBySurveyId(employeeSession.SurveyId)
        Else
            getAllQuestionsResponse.Success = False
            getAllQuestionsResponse.Message = "Session is expired"
        End If

        Return getAllQuestionsResponse
    End Function

    Private Shared Function GetSingleQuestionByOffset() As SendSingleQuestionResponse
        Dim getSingleQuestionResponse As New GetSingleQuestionResponse
        Dim sendSingleQuestionResponse As New SendSingleQuestionResponse
        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")


        getSingleQuestionResponse = _Survey.GetSingleQuestion(employeeSession.SurveyId, employeeSession.CompletedSurveyId, employeeSession.QuestionOffset)

        employeeSession.QuestionId = getSingleQuestionResponse.SurveyQuestions.question_id.ToString
        Dim test = HttpContext.Current.Session("EmployeeInfo")
        HttpContext.Current.Session("EmployeeInfo") = employeeSession

        sendSingleQuestionResponse.CurrentQuestion = Convert.ToInt32(employeeSession.QuestionOffset) + 1
        sendSingleQuestionResponse.TotalQuestions = Convert.ToInt32(employeeSession.SurveyQuestionsCount)
        sendSingleQuestionResponse.SurveyQuestions = getSingleQuestionResponse.SurveyQuestions


        If sendSingleQuestionResponse.CurrentQuestion = sendSingleQuestionResponse.TotalQuestions Then
            sendSingleQuestionResponse.IsFirst = False
            sendSingleQuestionResponse.IsLast = True
        ElseIf sendSingleQuestionResponse.CurrentQuestion = 1 Then
            sendSingleQuestionResponse.IsFirst = True
            sendSingleQuestionResponse.IsLast = False
        End If

        sendSingleQuestionResponse.Success = getSingleQuestionResponse.Success
        sendSingleQuestionResponse.Message = getSingleQuestionResponse.Message

        Return sendSingleQuestionResponse
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetSingleQuestion() As SendSingleQuestionResponse
        Dim sendSingleQuestionResponse As New SendSingleQuestionResponse
        Dim test = HttpContext.Current.Session("EmployeeInfo")
        If HttpContext.Current.Session("EmployeeInfo") IsNot Nothing Then
            sendSingleQuestionResponse = GetSingleQuestionByOffset()
        Else
            sendSingleQuestionResponse.Success = False
            sendSingleQuestionResponse.Message = "Session is expired"
        End If
        Return sendSingleQuestionResponse
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Increment(ByVal option_id As String) As SendSingleQuestionResponse
        Dim sendSingleQuestionResponse As New SendSingleQuestionResponse


        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")

        If employeeSession IsNot Nothing Then
            Dim saveAnswerResponse As New SaveAnswerResponse
            Dim saveAnswerRequest As New SaveAnswerRequest

            saveAnswerRequest.QuestionId = employeeSession.QuestionId
            saveAnswerRequest.SurveyId = employeeSession.SurveyId
            saveAnswerRequest.CompletedSurveyId = employeeSession.CompletedSurveyId
            saveAnswerRequest.SelectedOption = Convert.ToInt32(option_id)
            saveAnswerResponse = _Survey.SaveAnswer(saveAnswerRequest)
            If saveAnswerResponse.Success = True Then
                employeeSession.QuestionOffset = employeeSession.QuestionOffset + 1
                HttpContext.Current.Session("EmployeeInfo") = employeeSession
                sendSingleQuestionResponse = GetSingleQuestionByOffset()
            End If
        Else
            sendSingleQuestionResponse.Success = False
            sendSingleQuestionResponse.Message = "Session is expired"
        End If
        Return sendSingleQuestionResponse
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Decrement() As SendSingleQuestionResponse
        Dim sendSingleQuestionResponse As New SendSingleQuestionResponse

        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")

        If employeeSession IsNot Nothing Then
            employeeSession.QuestionOffset = employeeSession.QuestionOffset - 1
            HttpContext.Current.Session("EmployeeInfo") = employeeSession
            sendSingleQuestionResponse = GetSingleQuestionByOffset()
        Else
            sendSingleQuestionResponse.Success = False
            sendSingleQuestionResponse.Message = "Session is expired"
        End If
        Return sendSingleQuestionResponse
    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Finish(ByVal option_id As String) As SaveAnswerResponse

        Dim saveAnswerResponse As New SaveAnswerResponse
        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")
        If employeeSession IsNot Nothing Then
            Dim saveCompletedTimeResponse As New BaseResponse
            Dim saveAnswerRequest As New SaveAnswerRequest

            Dim StartTime As DateTime = employeeSession.SurveyStartTime
            Dim CompletedSurveyId As String = employeeSession.CompletedSurveyId

            saveAnswerRequest.QuestionId = employeeSession.QuestionId
            saveAnswerRequest.SurveyId = employeeSession.SurveyId
            saveAnswerRequest.CompletedSurveyId = employeeSession.CompletedSurveyId
            saveAnswerRequest.SelectedOption = Convert.ToInt32(option_id)
            saveAnswerResponse = _Survey.SaveAnswer(saveAnswerRequest)

            saveCompletedTimeResponse = _Survey.SaveCompletedTime(CompletedSurveyId, StartTime)
            If saveAnswerResponse.Success = True Then

                ''Get Result
                saveAnswerResponse = _Survey.GetResult(CompletedSurveyId)

                saveAnswerResponse.Message = "Thank you for completing this application. It will be reviewed shortly."
                HttpContext.Current.Session.Remove("EmployeeInfo")
            End If
        Else
            saveAnswerResponse.Success = False
            saveAnswerResponse.Message = "Session is expired"
        End If

        Return saveAnswerResponse
    End Function
    '<System.Web.Services.WebMethod(EnableSession:=True)>
    'Public Shared Function SurveyTimeout() As BaseResponse
    '    Dim baseResponse As New BaseResponse
    '    baseResponse = _Survey.SurveyTimeout()
    '    HttpContext.Current.Session.Remove("EmployeeInfo")
    '    Return baseResponse
    'End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function SurveyTimeout() As SaveAnswerResponse
        Dim baseResponse As New SaveAnswerResponse
        Dim employeeSession As EmployeeSession = HttpContext.Current.Session("EmployeeInfo")
        Dim CompletedSurveyId As String = EmployeeSession.CompletedSurveyId

        baseResponse.Message = _Survey.SurveyTimeout()
        baseResponse = _Survey.GetResult(CompletedSurveyId)

        HttpContext.Current.Session.Remove("EmployeeInfo")
        Return baseResponse
    End Function
End Class