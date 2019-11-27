Imports Entities.Login

Public Class LoginDataLayer
    Inherits DataAccessLayerBase
    Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
    Dim data As New DataSet
    Dim baseResponse As New BaseResponse

    Public Function DoLogIn(ByVal username As String, ByVal password As String, ByVal till_number As String, ByVal branch As String) As BaseResponse
        tmpSQL = "SELECT * FROM user_permissions WHERE user_name='" & username.ToUpper & "'And user_password='" & password.ToUpper & "'And branch_code='" & branch.ToUpper & "'"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                If Ds.Tables(0).Rows(0)("is_locked_to_branch") = True Then
                    baseResponse.Sucess = False
                    baseResponse.Message = "This Branch Is Locked"
                    Return baseResponse
                End If
                If Ds.Tables(0).Rows(0)("is_head_office_user") = True Then
                    baseResponse.Sucess = False
                    baseResponse.Message = "The Username / Password combination you entered is a Head Office user and cannot be used for a Shop branch."
                    Return baseResponse
                End If

                tmpSQL = "SELECT till_number FROM till_numbers WHERE branch_code = '" & branch.ToUpper & "' AND till_number = '" & till_number.ToUpper & "'"
                data = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If Not objDBRead.isR(data) Then
                    baseResponse.Sucess = False
                    baseResponse.Message = "The Username / Password combination you entered is not valid with this TillNumber."
                    Return baseResponse
                End If
            Else
                baseResponse.Sucess = False
                baseResponse.Message = "Please Check Username , Password and Branch"
                Return baseResponse
            End If

        Catch ex As Exception
            Throw ex
        End Try
        baseResponse.Sucess = True
        baseResponse.Message = "Login Sucessfully"

        Return baseResponse
    End Function
End Class
