Imports pcm.DataLayer
Imports Entities

Public Class UsersBL
    Dim _DLayer As UsersDL
    Private _dlLog As New LoggingDataLayer

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New UsersDL(CompanyCode)
    'End Sub

    Public Sub New()
        _DLayer = New UsersDL
    End Sub

    Public Function WriteToLogPCM(ByVal LogDetails As PCMUserLog) As Boolean
        Return _dlLog.WriteToLogPCM(LogDetails)
    End Function

    Public Function GetUsers() As DataTable

        Return _DLayer.GetUsers

    End Function

    Public Function GetUserDetails(ByVal Username As String) As DataTable

        Return _DLayer.GetUserDetails(Username)

    End Function

    Public Function DoLogin(ByRef objUser As Users) As Users

        Dim _RetrievedUser As Users

        Try
            _RetrievedUser = _DLayer.CheckUser(objUser)
        Catch ex As Exception
            Return Nothing
        End Try

        'Username / password doesn't exist
        If _RetrievedUser Is Nothing Then
            Return Nothing
        End If

        Try
            _DLayer.ProcessLogin(objUser)
        Catch ex As Exception
            Return Nothing
        End Try


        'Create the login record
        Dim _NewLog As New CollectionCallLog

        _NewLog.AccountNumber = ""
        _NewLog.ActionResult = "Success"
        _NewLog.ActionType = "User Login"
        _NewLog.IPAddress = _RetrievedUser.IPAddress
        _NewLog.LengthOfAction = "0"
        _NewLog.PTPAmount = "0"
        _NewLog.UserComment = ""
        _NewLog.UserName = _RetrievedUser.Username
        _NewLog.PTPDate = ""

        Try
            If _dlLog.WriteToLog(_NewLog) Then
                Return _RetrievedUser
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function UpdateUserFile(ByVal Username As String, ByVal Password As String, ByVal Email As String, ByVal isActive As String,
                                   ByVal processing_sequence As String,
                                   ByVal maintenance_sequence As String, ByVal reporting_sequence As String,
                                   ByVal isPCMAdmin As Boolean) As Boolean

        Return _DLayer.UpdateUserFile(Username, Password, Email, isActive, processing_sequence,
                                      maintenance_sequence, reporting_sequence, isPCMAdmin)

    End Function

    Public Function UpdateEmployee(ByVal _EmployeeMaintenanceRequest As EmployeeMaintenanceRequest) As BaseResponse

        Return _DLayer.UpdateEmployee(_EmployeeMaintenanceRequest)

    End Function

    Public Function AddLoginDetails(ByVal loginDetail As LoginDetails) As Boolean
        Return _dlLog.AddLoginDetails(loginDetail)
    End Function
End Class
