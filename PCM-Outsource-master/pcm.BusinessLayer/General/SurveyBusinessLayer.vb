Imports Entities
Imports pcm.DataLayer

Public Class SurveyBusinessLayer
    Private _dlSurvey As New SurveyDataLayer
    Private _baseResponse As New BaseResponse
    Private _saveSurveyResponse As New SaveSurveyResponse

    Public Function SaveSurvey(_createSurveyRequest As SaveSurveyRequest
                                       ) As SaveSurveyResponse
        _saveSurveyResponse = _dlSurvey.SaveSurvey(_createSurveyRequest)

        Return _saveSurveyResponse
    End Function


    Public Function GetSurveyList() As DataSet
        Dim _SurveyListDS As DataSet

        _SurveyListDS = _dlSurvey.GetSurveyList()

        If _SurveyListDS Is Nothing Then
            Return Nothing
        End If

        Return _SurveyListDS
    End Function

    Public Function GetSelectedSurveyDetails(strSurveyId As String) As SurveyDetailsResponse
        Dim _surveyDetailsResponse As New SurveyDetailsResponse

        _surveyDetailsResponse = _dlSurvey.GetSelectedSurveyDetails(strSurveyId)

        Return _surveyDetailsResponse
    End Function

    Public Function DeleteSurvey(survey_id As String) As BaseResponse
        _baseResponse = _dlSurvey.DeleteSurvey(survey_id)
        Return _baseResponse
    End Function

    Public Function GetAllQuestionsBySurveyId(survey_id As String) As GetAllQuestionsResponse
        Dim getAllQuestionsResponse As New GetAllQuestionsResponse
        getAllQuestionsResponse = _dlSurvey.GetAllQuestionsBySurveyId(survey_id)
        Return getAllQuestionsResponse
    End Function

    Public Function SaveQuestion(ByVal survey_id As String, ByVal SaveQuestionRequest As String) As BaseResponse
        _baseResponse = _dlSurvey.SaveQuestion(survey_id, SaveQuestionRequest)
        Return _baseResponse
    End Function

    Public Function EditQuestion(question_id As String) As GetSelectedQuestionResponse
        Dim getSelectedQuestionResponse As New GetSelectedQuestionResponse
        getSelectedQuestionResponse = _dlSurvey.EditQuestion(question_id)
        Return getSelectedQuestionResponse
    End Function

    Public Function DeleteQuestion(question_id As String) As BaseResponse
        _baseResponse = _dlSurvey.DeleteQuestion(question_id)
        Return _baseResponse
    End Function

    Public Function DeleteOption(ByVal question_id As String, option_id As Integer) As BaseResponse
        _baseResponse = _dlSurvey.DeleteOption(question_id, option_id)
        Return _baseResponse
    End Function

    Public Function SaveSurveyEmployeeDetails(surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest, survey_id As String) As SaveEmployeeResponse
        Dim saveEmployeeResponse As New SaveEmployeeResponse

        If surveyEmployeeDetailsRequest.FirstName = "" Then
            saveEmployeeResponse.Message = "Please enter your First Name."
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End If

        If surveyEmployeeDetailsRequest.LastName = "" Then
            saveEmployeeResponse.Message = "Please enter your Last Name."
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End If

        If Len(surveyEmployeeDetailsRequest.ID) < 2 Then
            saveEmployeeResponse.Message = "Please enter your ID Number."
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End If

        If surveyEmployeeDetailsRequest.ContactNumber = "" Then
            saveEmployeeResponse.Message = "Please enter your Contact Number."
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End If

        saveEmployeeResponse = _dlSurvey.SaveSurveyEmployeeDetails(surveyEmployeeDetailsRequest, survey_id)
        Return saveEmployeeResponse
    End Function

    Public Function CheckForCompletedQuestionnaire(surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest,
                                        survey_id As String) As SaveEmployeeResponse
        Dim saveEmployeeResponse As New SaveEmployeeResponse
        saveEmployeeResponse = _dlSurvey.CheckForCompletedQuestionnaire(surveyEmployeeDetailsRequest, survey_id)
        Return saveEmployeeResponse
    End Function

    Public Function CheckEmployeeExists(surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest) As SaveEmployeeResponse
        Dim saveEmployeeResponse As New SaveEmployeeResponse
        saveEmployeeResponse = _dlSurvey.CheckEmployeeExists(surveyEmployeeDetailsRequest)
        Return saveEmployeeResponse
    End Function


    Public Function GetSingleQuestion(survey_id As String, CompletedSurveyId As String, QuestionOffset As Integer) As GetSingleQuestionResponse
        Dim getSelectedQuestionResponse As New GetSingleQuestionResponse
        getSelectedQuestionResponse = _dlSurvey.GetSingleQuestion(survey_id, CompletedSurveyId, QuestionOffset)
        Return getSelectedQuestionResponse
    End Function

    Public Function SaveAnswer(saveAnswerRequest As SaveAnswerRequest) As SaveAnswerResponse
        Dim saveAnswerResponse As New SaveAnswerResponse
        saveAnswerResponse = _dlSurvey.SaveAnswer(saveAnswerRequest)
        Return saveAnswerResponse
    End Function

    Public Function SurveyTimeout() As String
        Return _dlSurvey.SurveyTimeout()
    End Function
    Public Function SaveCompletedTime(CompletedSurveyId As String, StartTime As DateTime)
        Dim baseResponse As New BaseResponse
        baseResponse = _dlSurvey.SaveCompletedTime(CompletedSurveyId, StartTime)
        Return baseResponse
    End Function

    Public Function CheckSurveyById(survey_id As String) As SurveyDetails
        _baseResponse = _dlSurvey.CheckSurveyById(survey_id)
        Return _baseResponse
    End Function

    Public Function CopySurvey(survey_id As String) As BaseResponse
        _baseResponse = _dlSurvey.CopySurvey(survey_id)
        Return _baseResponse
    End Function


    Public Function GetSurveysSummary(ByVal StartDate As String, ByVal EndDate As String, Survey As String) As DataSet
        Return _dlSurvey.ReturnSurveysSummary(StartDate, EndDate, Survey)
    End Function

    Public Function GetSurveyDetailsSummary(completed_survey_id As String) As DataSet
        Return _dlSurvey.ReturnSurveyDetailsSummary(completed_survey_id)
    End Function

    Public Function GetResult(ByVal completed_survey_id As String) As SaveAnswerResponse
        Return _dlSurvey.GetResult(completed_survey_id)
    End Function
End Class
