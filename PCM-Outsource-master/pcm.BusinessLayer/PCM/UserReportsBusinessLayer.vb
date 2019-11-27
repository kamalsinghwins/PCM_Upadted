Imports pcm.DataLayer
Imports Entities

Public Class UserReportsBusinessLayer

    Private _dluserreports As New UserReportsDataLayer

    Public Function GenerateUserReport(ByVal StartDate As String, ByVal EndDate As String) As List(Of UserReport)

        Dim _NewUserReportsList As New List(Of UserReport)

        _NewUserReportsList = _dluserreports.GetUserReportData(StartDate, EndDate)

        If Not IsNothing(_NewUserReportsList) Then
            Return _NewUserReportsList
        Else
            Return Nothing
        End If


    End Function

    Public Function GenerateTransactionByBranch(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim _NewUserReportsList As DataTable

        _NewUserReportsList = _dluserreports.GenerateTransactionByBranch(StartDate, EndDate)

        If Not IsNothing(_NewUserReportsList) Then
            Return _NewUserReportsList
        Else
            Return Nothing
        End If


    End Function

End Class
