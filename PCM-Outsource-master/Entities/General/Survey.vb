Public Class SaveSurveyRequest
    Public Property SurveyId As String
    Public Property SurveyName As String
    Public Property TypeOfSurvey As String
    Public Property IsActive As Boolean
    Public Property MaxTimeAllowed As String
    Public Property CreatedBy As String
End Class

Public Class CreateQuestion
    Public Property SurveyId As String
    Public Property QuestionId As String
    Public Property QuestionText As String
    Public Property OptionId As Integer
    Public Property OptionText As String
    Public Property IsCorrect As Boolean
End Class

Public Class SaveQuestionRequest
    Public Property question_id As String
    Public Property question_text As String
    Public Property Options As List(Of AnswerOption)
End Class
Public Class AnswerOption
    Public Property option_id As Integer?
    Public Property option_text As String
    Public Property is_correct As Boolean
End Class

Public Class Surveys
        Public Property Surveys As List(Of SaveSurveyRequest)
    End Class


    Public Class SaveSurveyResponse
        Inherits BaseResponse
        Public Property SurveyId As String
        Public Property IsRedirect As Boolean
    End Class

    Public Class SurveyDetailsResponse
        Inherits BaseResponse
        Public Property GetSelectedSurveyResponse As DataTable
    End Class

    Public Class SurveyQuestions
        Public Property survey_id As String
        Public Property question_id As String
        Public Property question_text As String
        Public Property Options As List(Of SurveyOptions)
    End Class


    Public Class SurveyOptions
        Public Property survey_id As String
        Public Property question_id As String
        Public Property option_id As Integer
        Public Property option_text As String
        Public Property is_correct As Boolean
    End Class

    Public Class SurveyQuestionsList
        Public Property QuestionsList As List(Of SurveyQuestions)
    End Class
    Public Class GetAllQuestionsResponse
        Inherits BaseResponse
    Public Property QuestionsList As List(Of SurveyQuestions)
    Public Property IsExists As Boolean
End Class

    Public Class GetSelectedQuestionResponse
        Inherits BaseResponse
        Public Property SurveyQuestions As SurveyQuestions
    End Class

Public Class SendSingleQuestionResponse
    Inherits BaseResponse
    Public Property SurveyQuestions As SingleSurveyQuestion
    Public Property CurrentQuestion As Integer
    Public Property TotalQuestions As Integer
    Public Property IsFirst As Boolean
    Public Property IsLast As Boolean
    Public Property QuestionId As String
End Class

Public Class SaveEditQuestionRequest
    Public Property Question As String
    Public Property QuestionId As String
    Public Property Options As List(Of AnswerOption)
End Class

Public Class SurveyEmployeeDetailsRequest
    Public Property FirstName As String
    Public Property LastName As String
    Public Property ID As String
    Public Property ContactNumber As String
    Public Property SurveyType As String
    Public Property IPAdress As String
End Class

Public Class EmployeeSession
    Public Property CompletedSurveyId As String
    Public Property SurveyId As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property QuestionOffset As Integer
    Public Property QuestionId As String
    Public Property SurveyName As String
    Public Property SurveyType As String
    Public Property SurveyQuestionsCount As Integer
    Public Property TimeAllowed As Integer
    Public Property SurveyStartTime As DateTime
End Class

Public Class SaveEmployeeResponse
    Inherits BaseResponse
    Public Property CompletedSurveyId As String
    Public Property SurveyId As String
    Public Property SurveyName As String
    Public Property SurveyType As String
    Public Property MaxTime As Integer
    Public Property SurveyQuestionsCount As Integer
    Public Property StartTime As DateTime?
End Class
Public Class SaveAnswerResponse
    Inherits BaseResponse
    Public Property TotalQuestions As String
    Public Property Attempt As String
    Public Property Score As String
End Class
Public Class SaveAnswerRequest
    Public Property SurveyId As String
    Public Property QuestionId As String
    Public Property SelectedOption As Integer
    Public Property CompletedSurveyId As String
End Class

Public Class SingleSurveyQuestion
    Public Property survey_id As String
    Public Property question_id As String
    Public Property question_text As String
    Public Property selected_option As Integer?
    Public Property Options As List(Of QuestionOptions)
End Class


Public Class QuestionOptions
    Public Property option_id As Integer
    Public Property option_text As String
End Class

Public Class GetSingleQuestionResponse
    Inherits BaseResponse
    Public Property SurveyQuestions As SingleSurveyQuestion
End Class

Public Class CheckIdResponse
    Inherits BaseResponse
End Class

Public Class CheckSurveyCompletedResponse
    Inherits BaseResponse
End Class
Public Class GetSurveyIdByQuestionIdResponse
    Inherits BaseResponse
    Public Property SurveyId As String
End Class

Public Class SurveyDetails
    Inherits BaseResponse
    Public Property TimeLimit As String
    Public Property NumberOfQuestions As String
    Public Property SurveyName As String
End Class
