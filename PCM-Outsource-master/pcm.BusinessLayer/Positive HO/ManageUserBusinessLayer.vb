
Imports Entities
Imports pcm.DataLayer
Public Class ManageUserBusinessLayer
    Private _dlManageUser As New ManageUserDataLayer
    Dim _dataTableResponse As DataTableResponse

    Public Function GetBranchDetails() As DataTableResponse
        _dataTableResponse = _dlManageUser.GetBranchDetails()
        Return _dataTableResponse
    End Function


    Public Function GetUserPermissions(ByVal BranchCode() As String, User As String) As DataTableResponse
        _dataTableResponse = _dlManageUser.GetUserPermissions(BranchCode, User)
        Return _dataTableResponse
    End Function

    Public Function DeleteUser(deleteRequest As DeleteRequest, BranchCode() As String) As DataTableResponse
        _dataTableResponse = _dlManageUser.DeleteUser(deleteRequest, BranchCode)
        Return _dataTableResponse
    End Function

    Public Function SaveUser(saveUserRequest As SaveUserRequest, BranchCode() As String, strPOS As String,
                             strMaint As String, strTrans As String, strOther As String) As DataTableResponse

        Dim dataTableResponse As New DataTableResponse

        If saveUserRequest.Branch = "" Then
            dataTableResponse.Message = "Please select a branch before continuing"
            dataTableResponse.Success = False
            Return dataTableResponse
        End If

        If saveUserRequest.User = "" Then
            dataTableResponse.Message = "Please input or select a user before continuing"
            dataTableResponse.Success = False
            Return dataTableResponse
        End If

        If saveUserRequest.Password = "" Then
            dataTableResponse.Message = "Please input a password before continuing"
            dataTableResponse.Success = False
            Return dataTableResponse
        End If

        _dataTableResponse = _dlManageUser.SaveUser(saveUserRequest, BranchCode, strPOS, strMaint, strTrans, strOther)
        Return _dataTableResponse

    End Function
End Class
