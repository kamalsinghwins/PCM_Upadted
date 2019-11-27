Imports Entities
Imports Newtonsoft.Json
Imports Npgsql
Imports Npgsql.Logging
'Imports pcm.DataLayer.dlLoggingNpgSQL

Public Class SurveyDataLayer
    Inherits DataAccessLayerBase

    Private _baseResponse As New BaseResponse
    Private _saveSurveyResponse As New SaveSurveyResponse
    Private _surveyDetailsResponse As New SurveyDetailsResponse
    Private _saveAnswerResponse As New SaveAnswerResponse
    Private _checkIdResponse As New CheckIdResponse
    Private _checkSurveyCompletedResponse As New CheckSurveyCompletedResponse
    Private _getSurveyIdByQuestionIdResponse As New GetSurveyIdByQuestionIdResponse
    Private _SurveyDetails As New SurveyDetails

    Dim ds2 As DataSet
    Dim DebtorDataLayer As New DebtorsDataLayer


    Public Function SaveSurvey(ByVal _createSurveyRequest As SaveSurveyRequest
                                        ) As SaveSurveyResponse

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        Try
            If _createSurveyRequest.SurveyName = "" Then
                _saveSurveyResponse.Message = "Please enter Questionnaire Name."
                _saveSurveyResponse.Success = False
                Return _saveSurveyResponse
            End If

            If _createSurveyRequest.TypeOfSurvey = "" Then

                _saveSurveyResponse.Message = "Please select Type Of Questionnaire."
                _saveSurveyResponse.Success = False
                Return _saveSurveyResponse
            End If

            If _createSurveyRequest.MaxTimeAllowed = "" Then
                _saveSurveyResponse.Message = "Please select MaxTimeAllowed."
                _saveSurveyResponse.Success = False
                Return _saveSurveyResponse
            End If

            If _createSurveyRequest.SurveyId = "" Then
                Dim survey_id As String = System.Guid.NewGuid.ToString()

                tmpSQL = "Insert into survey_master(survey_id, survey_name, type_of_survey, is_active, max_time_allowed, created_by) 
                        values('" & survey_id & "', '" & RG.Apos(_createSurveyRequest.SurveyName) & "','" & RG.Apos(_createSurveyRequest.TypeOfSurvey) & "','" & _createSurveyRequest.IsActive & "','" & _createSurveyRequest.MaxTimeAllowed & "','" & _createSurveyRequest.CreatedBy & "')"

                _saveSurveyResponse.Message = "Questionnaire created successfully."
                _saveSurveyResponse.SurveyId = survey_id
                _saveSurveyResponse.IsRedirect = True
            Else
                Dim survey_id As String = _createSurveyRequest.SurveyId

                tmpSQL = "Update survey_master set survey_name = '" & RG.Apos(_createSurveyRequest.SurveyName) & "', type_of_survey = '" & RG.Apos(_createSurveyRequest.TypeOfSurvey) & "', is_active = '" & _createSurveyRequest.IsActive & "', max_time_allowed = '" & _createSurveyRequest.MaxTimeAllowed & "' where survey_id = '" & survey_id & "'"

                _saveSurveyResponse.Message = "Questionnaire updated successfully."
                _saveSurveyResponse.SurveyId = survey_id
                _saveSurveyResponse.IsRedirect = False
            End If
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            _saveSurveyResponse.Message = "Some issue occured. Please try again."
            _saveSurveyResponse.Success = False
            Return _saveSurveyResponse
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try

        _saveSurveyResponse.Success = True
        Return _saveSurveyResponse
    End Function

    Public Function SaveQuestion(ByVal SurveyId As String, ByVal QuestionDetailJsonString As String) As BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim checkSurveyCompletedResponse As New CheckSurveyCompletedResponse
        Dim data() As Byte
        Try
            checkSurveyCompletedResponse = CheckSurveyCompleted(SurveyId)
            If checkSurveyCompletedResponse.Success = True Then
                _baseResponse.Message = "You cannot add new questions!"
                _baseResponse.Success = False
                Return _baseResponse
            End If
            Dim SaveQuestionDetail As SaveQuestionRequest = JsonConvert.DeserializeObject(Of SaveQuestionRequest)(QuestionDetailJsonString)
            If SaveQuestionDetail.question_text = "" Then
                _baseResponse.Message = "Please enter question."
                _baseResponse.Success = False
                Return _baseResponse
            End If

            If SaveQuestionDetail.Options.Count < 2 Or SaveQuestionDetail.Options.Count > 5 Then
                If SaveQuestionDetail.Options.Count < 2 Then
                    _baseResponse.Message = "Please add at least two options."
                Else
                    _baseResponse.Message = "Please add at most five options."
                End If
                _baseResponse.Success = False
                Return _baseResponse
            End If

            Dim AnswerCheck As Boolean = False

            Dim optionsTempSQL = ""

            Dim updateTempSQLArray As New List(Of String)()

            Dim i As Integer = 1
            For Each AnswerOption As AnswerOption In SaveQuestionDetail.Options
                If AnswerOption.option_text = "" Then
                    _baseResponse.Message = "Please fill all options that you have added."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If

                If AnswerOption.is_correct = True Then
                    AnswerCheck = True
                End If

                data = System.Convert.FromBase64String(AnswerOption.option_text)
                AnswerOption.option_text = System.Text.ASCIIEncoding.ASCII.GetString(data)

                If AnswerOption.option_id > 0 Then
                    updateTempSQLArray.Add("UPDATE survey_questions_options SET option_text='" & AnswerOption.option_text & "', is_correct='" & AnswerOption.is_correct & "' WHERE question_id='{question_id}' and option_id='" & AnswerOption.option_id & "'")
                Else
                    If optionsTempSQL = "" Then
                        optionsTempSQL &= "Insert into survey_questions_options(survey_id, question_id, option_id, option_text, is_correct) values "
                    End If

                    optionsTempSQL &= "('" & SurveyId & "', '{question_id}', '" & i & "',  '" & RG.Apos(AnswerOption.option_text) & "' , '" & RG.Apos(AnswerOption.is_correct) & "'),"
                End If

                i += 1
            Next

            If AnswerCheck = False Then
                'Answer Error
                _baseResponse.Message = "Please select answer."
                _baseResponse.Success = False
                Return _baseResponse
            End If


            tmpSQL = "SELECT * FROM survey_master WHERE survey_id = '" & SurveyId & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)

            If objDBWrite.isR(ds) = False Then
                _baseResponse.Message = "Questionnaire id does not exist."
                _baseResponse.Success = False
                Return _baseResponse
                Exit Function
            End If

            data = System.Convert.FromBase64String(SaveQuestionDetail.question_text)
            SaveQuestionDetail.question_text = System.Text.ASCIIEncoding.ASCII.GetString(data)

            If SaveQuestionDetail.question_id <> "" Then
                Dim question_id As String = SaveQuestionDetail.question_id

                tmpSQL = "SELECT * FROM survey_questions_master WHERE survey_id = '" & SurveyId & "' and question_id='" & question_id & "'"
                ds = objDBWrite.GetDataSet(tmpSQL)
                If objDBWrite.isR(ds) = False Then
                    _baseResponse.Message = "Question id does not exist."
                    _baseResponse.Success = False
                    Return _baseResponse
                    Exit Function
                End If

                'Update Question 

                tmpSQL = "Update survey_questions_master SET question_text = '" & RG.Apos(SaveQuestionDetail.question_text) & "' " &
                         "WHERE survey_id='" & SurveyId & "' and question_id='" & question_id & "'"
                objDBWrite.ExecuteQuery(tmpSQL)

                'Insert New Options of Question
                If optionsTempSQL <> "" Then
                    optionsTempSQL = optionsTempSQL.Trim().Substring(0, optionsTempSQL.Length - 1)

                    optionsTempSQL = optionsTempSQL.Replace("{question_id}", question_id)

                    tmpSQL = optionsTempSQL
                    objDBWrite.ExecuteQuery(tmpSQL)
                End If


                'Update Old Option of Question

                For Each updateTempSQL As String In updateTempSQLArray
                    updateTempSQL = updateTempSQL.Replace("{question_id}", question_id)
                    tmpSQL = updateTempSQL
                    objDBWrite.ExecuteQuery(tmpSQL)
                Next

                _baseResponse.Message = "Question updated successfully."
            Else
                Dim question_id As String = RG.Apos(Guid.NewGuid.ToString)

                'Save Question 

                tmpSQL = "Insert into survey_questions_master (survey_id, question_id, question_text) 
                        values('" & SurveyId & "', '" & question_id & "','" & RG.Apos(SaveQuestionDetail.question_text) & "')"
                objDBWrite.ExecuteQuery(tmpSQL)

                'Save Options of Question

                optionsTempSQL = optionsTempSQL.Trim().Substring(0, optionsTempSQL.Length - 1)

                optionsTempSQL = optionsTempSQL.Replace("{question_id}", question_id)

                tmpSQL = optionsTempSQL
                objDBWrite.ExecuteQuery(tmpSQL)

                _baseResponse.Message = "Question created successfully."
                _baseResponse.Success = True
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return _baseResponse
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try


        Return _baseResponse
    End Function

    Public Function GetSurveyList() As DataSet

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        tmpSQL = "Select sqm.*, (Select count(question_id) From survey_questions_master where survey_id = sqm.survey_id) as no_questions From survey_master sqm order by sqm.time_stamp desc"
        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                Return ds
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
    End Function

    Public Function GetSelectedSurveyDetails(strSurveyId As String) As SurveyDetailsResponse
        If strSurveyId <> "" Then
            Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

            tmpSQL = "SELECT * FROM survey_master WHERE survey_id = '" & strSurveyId & "'"

            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    _surveyDetailsResponse.GetSelectedSurveyResponse = ds.Tables(0)
                    _surveyDetailsResponse.Success = True
                Else
                    _surveyDetailsResponse.Message = "Some issue occured. Please try again."
                    _surveyDetailsResponse.Success = False
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _surveyDetailsResponse.Message = "Some issue occured. Please try again."
                _surveyDetailsResponse.Success = False
            Finally
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
            End Try
        Else
            _surveyDetailsResponse.Message = "Questionnaire id is missing. Please try again."
            _surveyDetailsResponse.Success = False
        End If

        Return _surveyDetailsResponse
    End Function

    Public Function DeleteSurvey(survey_id As String) As BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim checkSurveyCompletedResponse As New CheckSurveyCompletedResponse
        Try
            If survey_id = "" Then
                _baseResponse.Message = "Please select Questionnaire."
                _baseResponse.Success = False
                Return _baseResponse
            End If
            checkSurveyCompletedResponse = CheckSurveyCompleted(survey_id)
            If checkSurveyCompletedResponse.Success = True Then
                _baseResponse.Message = checkSurveyCompletedResponse.Message
                _baseResponse.Success = False
                Return _baseResponse
            End If
            tmpSQL = "delete from survey_questions_options where survey_id='" & survey_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)

            tmpSQL = "delete from survey_questions_master where survey_id='" & survey_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)

            tmpSQL = "delete from survey_master where survey_id= '" & survey_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)


        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        _baseResponse.Message = "Questionnaire deleted successfully."
        _baseResponse.Success = True
        Return _baseResponse
    End Function

    Public Function GetAllQuestionsBySurveyId(survey_id As String) As GetAllQuestionsResponse
        Dim GetAllQuestionsDT As DataTable
        Dim getAllQuestionsResponse As New GetAllQuestionsResponse
        Dim checkSurveyCompletedResponse As New CheckSurveyCompletedResponse

        If survey_id = "" Then
            _baseResponse.Message = "Please specify a Questionnaire Id"
            _baseResponse.Success = False
            Return Nothing
        End If

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        checkSurveyCompletedResponse = CheckSurveyCompleted(survey_id)
        getAllQuestionsResponse.IsExists = checkSurveyCompletedResponse.Success
        tmpSQL = "select to_json(array_agg(R.*)) FROM (
                    select sqm.*, (select to_json(array_agg(Op.*)) as Options FROM (select question_id,option_id,option_text,survey_id, is_correct from survey_questions_options where question_id = sqm.question_id order by option_id asc) Op) from survey_questions_master sqm where 
                    sqm.survey_id = '" & survey_id & "' order by time_stamp asc ) R;"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                GetAllQuestionsDT = ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Dim SurveyQuestionsJsonString As String = GetAllQuestionsDT.Rows(GetAllQuestionsDT.Rows.Count - 1)("to_json").ToString

        If SurveyQuestionsJsonString <> Nothing And SurveyQuestionsJsonString <> "" Then
            Dim SurveyQuestionsList As New SurveyQuestionsList

            SurveyQuestionsList.QuestionsList = JsonConvert.DeserializeObject(Of List(Of SurveyQuestions))(SurveyQuestionsJsonString)
            getAllQuestionsResponse.QuestionsList = SurveyQuestionsList.QuestionsList

        End If
        getAllQuestionsResponse.Success = True
        Return getAllQuestionsResponse

    End Function

    Public Function EditQuestion(question_id As String) As GetSelectedQuestionResponse
        Dim _getSelectedQuestionResponse As New GetSelectedQuestionResponse
        Dim GetSelectedQuestionResponse As DataTable
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        If question_id = "" Then

            _baseResponse.Message = "Please specify a Questionnaire Id"
            _baseResponse.Success = False
            Return Nothing
        End If
        tmpSQL = "select to_json(R.*) FROM (
 Select  sqm.*, (select to_json(array_agg(Op.*)) as Options FROM (select * from survey_questions_options where question_id = sqm.question_id order by option_id asc) Op) from survey_questions_master sqm where 
    sqm.question_id = '" & question_id & "' ) R"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                GetSelectedQuestionResponse = ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Dim SelectedQuestionsJsonString As String = GetSelectedQuestionResponse.Rows(GetSelectedQuestionResponse.Rows.Count - 1)("to_json").ToString

        If SelectedQuestionsJsonString <> Nothing And SelectedQuestionsJsonString <> "" Then
            Dim SurveyQuestions As New SurveyQuestions

            _getSelectedQuestionResponse.SurveyQuestions = JsonConvert.DeserializeObject(Of SurveyQuestions)(SelectedQuestionsJsonString)
        End If
        _getSelectedQuestionResponse.Success = True

        Return _getSelectedQuestionResponse
    End Function

    Public Function DeleteQuestion(question_id As String) As BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim checkSurveyCompletedResponse As New CheckSurveyCompletedResponse
        Dim getSurveyIdByQuestionIdResponse As New GetSurveyIdByQuestionIdResponse
        Try
            getSurveyIdByQuestionIdResponse = GetSurveyIdByQuestionId(question_id)
            If getSurveyIdByQuestionIdResponse.Success = False Then
                _baseResponse.Success = False
                _baseResponse.Message = getSurveyIdByQuestionIdResponse.Message
                Return _baseResponse
            End If
            checkSurveyCompletedResponse = CheckSurveyCompleted(getSurveyIdByQuestionIdResponse.SurveyId)
            If checkSurveyCompletedResponse.Success = True Then
                _baseResponse.Message = "You cannot delete the question"
                _baseResponse.Success = False
                Return _baseResponse
            End If
            If question_id = "" Then
                _baseResponse.Message = "Please select the question."
                _baseResponse.Success = False
                Return _baseResponse
            End If
            tmpSQL = "delete from survey_questions_options where question_id='" & question_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)
            tmpSQL = "delete from survey_questions_master where question_id='" & question_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)
            _baseResponse.Message = "Question deleted successfully."
            _baseResponse.Success = True
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _baseResponse.Message = "Some issue occured. Please try again."
                _baseResponse.Success = False
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try

        Return _baseResponse
    End Function

    Public Function DeleteOption(ByVal question_id As String, option_id As Integer) As BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim Is_Correct As String
        Try
            tmpSQL = "SELECT * from  survey_questions_options where question_id='" & question_id & "' and option_id= '" & option_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                Is_Correct = ds.Tables(0).Rows(0)("is_correct")
                If Is_Correct = True Then
                    _baseResponse.Message = "You Cannot delete the answer"
                    _baseResponse.Success = False
                    Return _baseResponse
                Else
                    tmpSQL = "delete from survey_questions_options where question_id='" & question_id & "' and option_id= '" & option_id & "' "
                    objDBWrite.ExecuteQuery(tmpSQL)

                    tmpSQL = "update survey_questions_options set option_id = option_id-1 where  question_id = '" & question_id & "' and option_id > " & option_id
                    objDBWrite.ExecuteQuery(tmpSQL)

                    _baseResponse.Message = "Option Deleted successfully."
                    _baseResponse.Success = True
                End If
            Else
                _baseResponse.Message = "Invalid Option Id"
                _baseResponse.Success = False
                Return _baseResponse
            End If

        Catch ex As Exception

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _baseResponse.Message = "Some issue occured. Please try again."
                _baseResponse.Success = False
                Return _baseResponse
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try

        Return _baseResponse
    End Function

    Public Function CheckForCompletedQuestionnaire(_surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest,
                                        survey_id As String) As SaveEmployeeResponse

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim saveEmployeeResponse As New SaveEmployeeResponse

        Try
            tmpSQL = "SELECT * FROM survey_users " &
                      "WHERE id_number = '" & RG.Apos(_surveyEmployeeDetailsRequest.ID) & "' " &
                      "AND survey_id = '" & survey_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                saveEmployeeResponse.Message = "You have already completed this questionnaire. You cannot complete it again."
                saveEmployeeResponse.Success = False
            Else
                saveEmployeeResponse.Success = True
            End If
        Catch ex As Exception
            saveEmployeeResponse.Message = "An error occured: " & ex.Message
            saveEmployeeResponse.Success = False
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return saveEmployeeResponse
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return saveEmployeeResponse

    End Function

    Public Function CheckEmployeeExists(_surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest) As SaveEmployeeResponse

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim saveEmployeeResponse As New SaveEmployeeResponse

        Try
            tmpSQL = "SELECT employee_number FROM employee_details WHERE employee_number = '" & RG.Apos(_surveyEmployeeDetailsRequest.ID) & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                'Clock number is valid
                saveEmployeeResponse.Success = True
            Else
                saveEmployeeResponse.Message = "This Clock Number does not exist."
                saveEmployeeResponse.Success = False
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            saveEmployeeResponse.Message = "An error occured: " & ex.Message
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return saveEmployeeResponse

    End Function

    Public Function SaveSurveyEmployeeDetails(_surveyEmployeeDetailsRequest As SurveyEmployeeDetailsRequest, survey_id As String) As SaveEmployeeResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim saveEmployeeResponse As New SaveEmployeeResponse
        Dim SurveyId As String
        Dim SurveyName As String
        Dim SurveyType As String
        Dim MaxTime As Integer
        Dim SurveyQuestionsCount As Integer
        Dim checkIdResponse As New CheckIdResponse
        Try

            tmpSQL = "select sm.*, (select count(*) from survey_questions_master where survey_id = sm.survey_id) as questions_count from survey_master sm where sm.is_active=true and survey_id='" & survey_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                SurveyId = ds.Tables(0).Rows(0)("survey_id").ToString
                SurveyName = ds.Tables(0).Rows(0)("survey_name").ToString
                SurveyType = ds.Tables(0).Rows(0)("type_of_survey").ToString
                MaxTime = Convert.ToInt32(ds.Tables(0).Rows(0)("max_time_allowed").ToString)
                SurveyQuestionsCount = Convert.ToInt32(ds.Tables(0).Rows(0)("questions_count").ToString)
            Else
                saveEmployeeResponse.Message = "Questionnaire not found."
                saveEmployeeResponse.Success = False
                Return saveEmployeeResponse
            End If

            Dim completed_survey_id As String = System.Guid.NewGuid.ToString()
            Dim SurveyStartTime As DateTime = DateTime.Now

            tmpSQL = "Insert into survey_users(first_name, last_name, id_number, contact_number,survey_id,completed_survey_id," &
                     "time_stamp,ip_address) values " &
                     "('" & RG.Apos(_surveyEmployeeDetailsRequest.FirstName) & "'," &
                     "'" & RG.Apos(_surveyEmployeeDetailsRequest.LastName) & "'," &
                     "'" & RG.Apos(_surveyEmployeeDetailsRequest.ID) & "'," &
                     "'" & _surveyEmployeeDetailsRequest.ContactNumber & "','" & SurveyId & "'," &
                     "'" & completed_survey_id & "',now(),'" & RG.Apos(_surveyEmployeeDetailsRequest.IPAdress) & "')"
            objDBWrite.ExecuteQuery(tmpSQL)

            saveEmployeeResponse.CompletedSurveyId = completed_survey_id
            saveEmployeeResponse.SurveyId = SurveyId
            saveEmployeeResponse.SurveyName = SurveyName
            saveEmployeeResponse.SurveyType = SurveyType
            saveEmployeeResponse.MaxTime = MaxTime
            saveEmployeeResponse.StartTime = SurveyStartTime
            saveEmployeeResponse.SurveyQuestionsCount = SurveyQuestionsCount

            saveEmployeeResponse.Message = "Successfull."
            saveEmployeeResponse.Success = True
        Catch ex As Exception
            saveEmployeeResponse.Message = "Some issue occured. Please try again."
            saveEmployeeResponse.Success = False
            Return saveEmployeeResponse
        End Try
        Return saveEmployeeResponse
    End Function

    Public Function GetSingleQuestion(survey_id As String, CompletedSurveyId As String, QuestionOffset As Integer) As GetSingleQuestionResponse
        Dim _getSelectedQuestionResponse As New GetSingleQuestionResponse
        Dim GetSelectedQuestionResponse As DataTable
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        If survey_id = "" Then

            _baseResponse.Message = "Please specify a Questionnaire Id"
            _baseResponse.Success = False
            Return Nothing
        End If
        tmpSQL = " Select  to_json(R.*) FROM (
                        Select  sqm.*, (select selected_option from survey_completed where completed_survey_id='" & CompletedSurveyId & "' " &
                        "and survey_id = '" & survey_id & "' and question_id = sqm.question_id) AS selected_option, " &
                        "(select to_json(array_agg(Op.*)) as Options " &
                        "FROM (select option_id, option_text from survey_questions_options where question_id = sqm.question_id " &
                        "order by option_id asc) Op) from survey_questions_master sqm where sqm.survey_id= '" & survey_id & "' " &
                        "limit 1 offset " & QuestionOffset & " ) R"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                GetSelectedQuestionResponse = ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Dim SelectedQuestionsJsonString As String = GetSelectedQuestionResponse.Rows(GetSelectedQuestionResponse.Rows.Count - 1)("to_json").ToString

        If SelectedQuestionsJsonString <> Nothing And SelectedQuestionsJsonString <> "" Then
            Dim SurveyQuestions As New SingleSurveyQuestion

            _getSelectedQuestionResponse.SurveyQuestions = JsonConvert.DeserializeObject(Of SingleSurveyQuestion)(SelectedQuestionsJsonString)
        End If
        _getSelectedQuestionResponse.Success = True

        Return _getSelectedQuestionResponse
    End Function

    Public Function SaveAnswer(saveAnswerRequest As SaveAnswerRequest) As SaveAnswerResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim Is_correct As Boolean
        Try
            tmpSQL = "SELECT is_correct FROM survey_questions_options WHERE question_id = '" & RG.Apos(saveAnswerRequest.QuestionId) & "' and option_id='" & saveAnswerRequest.SelectedOption & "' "
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                Is_correct = ds.Tables(0).Rows(0)("is_correct")
            Else
                Is_correct = False
            End If

            tmpSQL = "select * from survey_completed where survey_id='" & RG.Apos(saveAnswerRequest.SurveyId) & "' and question_id='" & RG.Apos(saveAnswerRequest.QuestionId) & "' and completed_survey_id='" & RG.Apos(saveAnswerRequest.CompletedSurveyId) & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)

            If objDBWrite.isR(ds) Then
                tmpSQL = "Update survey_completed 
                            set selected_option='" & RG.Apos(saveAnswerRequest.SelectedOption) & "', 
                            Is_correct = '" & Is_correct & "' 
                            Where survey_id='" & RG.Apos(saveAnswerRequest.SurveyId) & "' and question_id='" & RG.Apos(saveAnswerRequest.QuestionId) & "' and completed_survey_id='" & RG.Apos(saveAnswerRequest.CompletedSurveyId) & "'"

                objDBWrite.ExecuteQuery(tmpSQL)
            Else
                tmpSQL = "Insert into survey_completed(survey_id, question_id, selected_option, is_correct,completed_survey_id) 
                        values('" & RG.Apos(saveAnswerRequest.SurveyId) & "', '" & RG.Apos(saveAnswerRequest.QuestionId) & "','" & RG.Apos(saveAnswerRequest.SelectedOption) & "','" & Is_correct & "','" & RG.Apos(saveAnswerRequest.CompletedSurveyId) & "')"
                objDBWrite.ExecuteQuery(tmpSQL)
                _saveAnswerResponse.Success = True
                _saveAnswerResponse.Message = "Successfull"
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            _saveAnswerResponse.Message = "Some issue occured. Please try again."
            _saveAnswerResponse.Success = False
            Return _saveAnswerResponse
        End Try
        Return _saveAnswerResponse
    End Function

    Private Function CheckCorrectAnswer() As Boolean

    End Function

    Public Function SurveyTimeout() As String
        Dim Message As String = "Your Time Is Up"
        Return Message
    End Function

    Public Function SaveCompletedTime(CompletedSurveyId As String, StartTime As DateTime) As BaseResponse
        Dim baseResponse As New BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            Dim CompletedTime As Integer
            CompletedTime = (DateTime.Now - StartTime).TotalSeconds
            tmpSQL = "update survey_users set time_to_complete='" & CompletedTime & "' where completed_survey_id='" & CompletedSurveyId & "'"
            objDBWrite.ExecuteQuery(tmpSQL)
            baseResponse.Success = True
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            baseResponse.Message = "Some issue occured. Please try again."
            baseResponse.Success = False
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        Return baseResponse
    End Function

    Public Function CheckSurveyById(survey_id As String) As SurveyDetails
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            tmpSQL = "select * from survey_master sm where sm.is_active=true and sm.survey_id='" & survey_id & "'"
            Ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(Ds) Then
                _SurveyDetails.QuestionnaireID = Ds.Tables(0).Rows(0)("type_of_survey")
                _SurveyDetails.Success = True
                _SurveyDetails.Message = "Questionnaire Found"
                _SurveyDetails.TimeLimit = Ds.Tables(0).Rows(0)("max_time_allowed")
                _SurveyDetails.SurveyName = Ds.Tables(0).Rows(0)("survey_name")

            Else
                _SurveyDetails.Message = "Questionnaire not found."
                _SurveyDetails.Success = False
                Return _SurveyDetails
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _SurveyDetails.Message = "Some issue occured. Please try again."
                _SurveyDetails.Success = False
                Return _SurveyDetails
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        Return _SurveyDetails
    End Function

    Public Function CheckIdExists(ID As Integer, survey_id As String) As CheckIdResponse

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            tmpSQL = "select * from survey_users where id_number='" & ID & "' and survey_id='" & survey_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                _checkIdResponse.Success = False
                _checkIdResponse.Message = "You have already completed the Questionnaire"
            Else
                _checkIdResponse.Message = "ID does not exist"
                _checkIdResponse.Success = True
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _baseResponse.Message = "Some issue occured. Please try again."
                _baseResponse.Success = False
                Return _baseResponse
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        Return _checkIdResponse
    End Function

    Public Function CopySurvey(survey_id As String) As BaseResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            Dim getAllQuestionsResponse As New GetAllQuestionsResponse
            If survey_id = "" Then
                _baseResponse.Message = "Please select Questionnaire."
                _baseResponse.Success = False
                Return _baseResponse
            End If

            Dim SurveyId As String = System.Guid.NewGuid.ToString()
            Dim QuestionId As String
            tmpSQL = "insert into survey_master(survey_id,created_by,survey_name,type_of_survey,is_active,max_time_allowed) select '" & SurveyId & "','', Concat('Copy Of ',survey_name),type_of_survey,is_active,max_time_allowed from survey_master where survey_id='" & survey_id & "'"
            objDBWrite.ExecuteQuery(tmpSQL)

            getAllQuestionsResponse = GetAllQuestionsBySurveyId(survey_id)

            Dim QuestionCopyQuery As String = ""
            Dim OptionsCopyQuery As String = ""

            For Each Question As SurveyQuestions In getAllQuestionsResponse.QuestionsList
                If QuestionCopyQuery = "" Then
                    QuestionCopyQuery &= "Insert into survey_questions_master (survey_id, question_id,  question_text) values "
                End If

                QuestionId = System.Guid.NewGuid.ToString()

                QuestionCopyQuery &= "('" & SurveyId & "', '" & QuestionId & "', '" & Question.question_text & "'),"

                For Each AnswerOption As SurveyOptions In Question.Options
                    If OptionsCopyQuery = "" Then
                        OptionsCopyQuery &= "Insert into survey_questions_options(survey_id, question_id, option_id, option_text, is_correct) values "
                    End If
                    OptionsCopyQuery &= "('" & SurveyId & "', '" & QuestionId & "', '" & AnswerOption.option_id & "',  '" & RG.Apos(AnswerOption.option_text) & "' , '" & RG.Apos(AnswerOption.is_correct) & "'),"
                Next
            Next

            'Executing Question Copy Query
            QuestionCopyQuery = QuestionCopyQuery.Trim().Substring(0, QuestionCopyQuery.Length - 1)

            tmpSQL = QuestionCopyQuery
            objDBWrite.ExecuteQuery(tmpSQL)

            'Executing QuestionOptions Copy Query
            OptionsCopyQuery = OptionsCopyQuery.Trim().Substring(0, OptionsCopyQuery.Length - 1)

            tmpSQL = OptionsCopyQuery
            objDBWrite.ExecuteQuery(tmpSQL)

        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _baseResponse.Message = "Some issue occured. Please try again."
                _baseResponse.Success = False
                Return _baseResponse
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        _baseResponse.Message = "Questionnaire copied successfully."
        _baseResponse.Success = True
        Return _baseResponse
    End Function

    Public Function CheckSurveyCompleted(survey_id As String) As CheckSurveyCompletedResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            If survey_id = "" Then
                _checkSurveyCompletedResponse.Message = "Please select Questionnaire."
                _checkSurveyCompletedResponse.Success = False
                Return _checkSurveyCompletedResponse
            End If
            tmpSQL = "select * from survey_completed where survey_id='" & survey_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            If objDBWrite.isR(ds) Then
                _checkSurveyCompletedResponse.Success = True
                _checkSurveyCompletedResponse.Message = "You cannot delete the Questionnaire"
            Else
                _checkSurveyCompletedResponse.Message = "successfull"
                _checkSurveyCompletedResponse.Success = False
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _checkSurveyCompletedResponse.Message = "Some issue occured. Please try again."
                _checkSurveyCompletedResponse.Success = False
                Return _checkSurveyCompletedResponse
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        Return _checkSurveyCompletedResponse
    End Function

    Public Function GetSurveyIdByQuestionId(question_id As String) As GetSurveyIdByQuestionIdResponse
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim SurveyId As String
        Try
            If question_id = "" Then
                _getSurveyIdByQuestionIdResponse.Message = "Please select the question."
                _getSurveyIdByQuestionIdResponse.Success = False
                Return _getSurveyIdByQuestionIdResponse
            End If
            tmpSQL = "select survey_id from survey_questions_master where question_id='" & question_id & "'"
            ds = objDBWrite.GetDataSet(tmpSQL)
            SurveyId = ds.Tables(0).Rows(0)("survey_id").ToString
            If objDBWrite.isR(ds) Then
                _getSurveyIdByQuestionIdResponse.Success = True
                _getSurveyIdByQuestionIdResponse.Message = "Questionnaire Id Found"
                _getSurveyIdByQuestionIdResponse.SurveyId = SurveyId
            Else
                _getSurveyIdByQuestionIdResponse.Message = "Questionnaire Id not found"
                _getSurveyIdByQuestionIdResponse.Success = False
                _getSurveyIdByQuestionIdResponse.SurveyId = SurveyId
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
                _getSurveyIdByQuestionIdResponse.Message = "Some issue occured. Please try again."
                _getSurveyIdByQuestionIdResponse.Success = False
                Return _getSurveyIdByQuestionIdResponse
            End If

        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
        Return _getSurveyIdByQuestionIdResponse
    End Function

    Public Function ReturnSurveysSummary(ByVal StartDate As String, ByVal EndDate As String, Survey As String, Optional EmployeeNumber As String = "") As DataSet
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim xData As New DataSet
        Dim WhereCondition As String = ""
        Try
            If StartDate <> "" And EndDate <> "" Then
                If WhereCondition = "" Then
                    WhereCondition &= " WHERE su.time_stamp between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59'"
                Else
                    WhereCondition &= " AND su.time_stamp between '" & StartDate & " 00:00:00' and '" & EndDate & " 23:59:59'"
                End If
            End If

            If Survey IsNot Nothing And Survey <> "" Then
                If WhereCondition = "" Then
                    WhereCondition &= " WHERE sm.survey_name ILIKE '%" & Survey & "%'"
                Else
                    WhereCondition &= " AND sm.survey_name ILIKE '%" & Survey & "%'"
                End If
            End If

            If EmployeeNumber <> "" Then
                WhereCondition &= If(WhereCondition = "", " WHERE ", " AND ") & " su.id_number = '" & EmployeeNumber & "'"
            End If


            tmpSQL = "select 
                    su.completed_survey_id, 
                    su.first_name,
                    su.last_name,
                    su.id_number,
                    su.contact_number,
                    su.time_to_complete,
                    case when su.time_to_complete is null 
                    	then su.time_stamp + (sm.max_time_allowed * interval '1 minute')
                     	else su.time_stamp + (su.time_to_complete * interval '1 second')
                     end as date_time_completed,
                    sm.survey_name,
                    sm.type_of_survey,
                    (select count(question_id) from survey_questions_master as sq where sq.survey_id=sm.survey_id) AS TotalQuestions,
                    (select count(is_correct) from survey_completed as sc where sc.completed_survey_id=su.completed_survey_id and sc.is_correct=true) AS CorrectAnswers,
                    (select count(is_correct) from survey_completed as sc where sc.completed_survey_id=su.completed_survey_id) AS QuestionsAnswered  
                    from public.survey_users as su 
                    join public.survey_master as sm on sm.survey_id=su.survey_id " & WhereCondition & " ORDER BY su.time_stamp DESC"

            Ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return xData

    End Function

    Public Function ReturnSurveyDetailsSummary(completed_survey_id As String) As DataSet
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Try
            tmpSQL = "select su.question_id, sqm.question_text, sqo.option_text, su.is_correct::text  " &
                     "from survey_completed su join survey_questions_master sqm ON su.question_id = sqm.question_id " &
                     "join survey_questions_options sqo ON su.question_id = sqo.question_id And su.selected_option = sqo.option_id " &
                     "where completed_survey_id = '" & completed_survey_id & "'"
            Ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
    End Function

    Public Function GetResult(ByVal completed_survey_id As String) As SaveAnswerResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim xData As New DataSet
        Dim WhereCondition As String = ""
        Try

            tmpSQL = "select 
                    (select count(question_id) from survey_questions_master as sq where sq.survey_id=sm.survey_id) AS TotalQuestions,
                    (select count(is_correct) from survey_completed as sc where sc.completed_survey_id=su.completed_survey_id and sc.is_correct=true) AS CorrectAnswers,
                    (select count(is_correct) from survey_completed as sc where sc.completed_survey_id=su.completed_survey_id) AS QuestionsAnswered  
                    from public.survey_users as su 
                    join public.survey_master as sm on sm.survey_id=su.survey_id where completed_survey_id= '" & completed_survey_id & "'ORDER BY su.time_stamp DESC"

            Ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                _saveAnswerResponse.Success = True
                _saveAnswerResponse.Score = Ds.Tables(0).Rows(0)("CorrectAnswers")
                _saveAnswerResponse.TotalQuestions = Ds.Tables(0).Rows(0)("TotalQuestions")
                _saveAnswerResponse.Attempt = Ds.Tables(0).Rows(0)("QuestionsAnswered")
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return _saveAnswerResponse
    End Function
End Class


