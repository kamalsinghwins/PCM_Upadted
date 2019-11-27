
Imports Entities
Imports pcm.DataLayer
Public Class EmployeeBusinessLayer
    Private _dlEmployee As New EmployeeDataLayer
    Private _baseResponse As New BaseResponse
    Private _dlSurvey As New SurveyDataLayer


    Public Function GetEmployees(Field As String, Criteria As String) As DataTable

        Dim _getEmployeeResponse As DataTable
        _getEmployeeResponse = _dlEmployee.GetEmployees(Field, Criteria)
        Return _getEmployeeResponse
    End Function

    Public Function GetEmployee(ByVal ClockNumber As String) As DataTable
        Dim _getEmployeeResponse As DataTable
        _getEmployeeResponse = _dlEmployee.GetEmployee(ClockNumber)
        Return _getEmployeeResponse

    End Function

    Public Function SaveNotes(_employeeNoteRequest As EmployeeNoteRequest) As BaseResponse

        If _employeeNoteRequest.EmployeeName = "" Then
            _baseResponse.Success = False
            _baseResponse.Message = "Please enter Employee number."
            Return _baseResponse
        End If

        If _employeeNoteRequest.TypeOfReport = "" Then
            _baseResponse.Success = False
            _baseResponse.Message = "Please select Type of report."
            Return _baseResponse
        End If

        If _employeeNoteRequest.TypeOfReport = "Warning" And _employeeNoteRequest.Warning = "" Then
            _baseResponse.Success = False
            _baseResponse.Message = "Please select warning."
            Return _baseResponse
        End If

        If _employeeNoteRequest.TypeOfReport = "Review" And _employeeNoteRequest.Rating = "0" Then
            _baseResponse.Success = False
            _baseResponse.Message = "Please select rating."
            Return _baseResponse
        End If

        If _employeeNoteRequest.Note = "" Then
            _baseResponse.Success = False
            _baseResponse.Message = "Please enter the notes."
            Return _baseResponse
        End If

        _baseResponse = _dlEmployee.SaveNotes(_employeeNoteRequest)
        Return _baseResponse

    End Function

    Public Function GetReviewsSummary(ByVal StartDate As String, ByVal EndDate As String, ByVal ClockNumber As String) As DataSet
        Return _dlEmployee.ReturnReviewsSummary(StartDate, EndDate, ClockNumber)
    End Function

    Public Function GetEmployeeReviews(ByVal EmployeeNumber As String) As DataSet
        Return _dlEmployee.GetEmployeeReviews(EmployeeNumber)
    End Function

    Public Function GetSurveysSummary(ByVal EmployeeNumber As String) As DataSet
        Return _dlSurvey.ReturnSurveysSummary("", "", "", EmployeeNumber)
    End Function

    Public Function GetSurveyDetailsSummary(ByVal CompletedSurveyId As String) As DataSet
        Return _dlSurvey.ReturnSurveyDetailsSummary(CompletedSurveyId)
    End Function

    Public Function GetEmployees(Optional ByVal EmployeeNumber As String = "") As DataTable
        Return _dlEmployee.GetEmployees(EmployeeNumber)
    End Function


End Class
