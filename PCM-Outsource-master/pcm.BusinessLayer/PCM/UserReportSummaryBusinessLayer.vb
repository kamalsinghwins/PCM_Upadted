Imports Entities
Imports pcm.DataLayer

Public Class UserReportSummaryBusinessLayer

    Dim dlUserSummary As New UserReportSummaryDataLayer

    Public Function GetUsersSummary(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.ReturnUsersSummary(StartDate, EndDate)

    End Function

    Public Sub InsertBranchesIntoPCM()

        dlUserSummary.InsertBranchesIntoPCM()

    End Sub

    Public Function GetUserSummaryDetail(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.ReturnUsersSummaryTransactions(StartDate, EndDate)

    End Function

    Public Function GetAccountsOpenedByEmployee(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.AccountsOpenedByEmployee(StartDate, EndDate)

    End Function

    Public Function CollectionsByEmployee(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.CollectionsByEmployee(StartDate, EndDate)

    End Function

    Public Function CollectionsByEmployeeTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.CollectionsByEmployeeTransactions(StartDate, EndDate)

    End Function

    Public Function GetAccountsOpenedByStore(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.AccountsOpenedByStore(StartDate, EndDate)

    End Function

    Public Function GetAccountsOpenedByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.AccountsOpenedByStoreTransactions(StartDate, EndDate)

    End Function

    Public Function GetAccountsOpenedByEmployeeDetail(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return dlUserSummary.AccountsOpenedByEmployeeTransactions(StartDate, EndDate)

    End Function

End Class