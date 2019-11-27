Imports pcm.DataLayer

Public Class GeneralHOBL

    Dim _DLayer As GeneralHODL

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New GeneralHODL(CompanyCode)
    'End Sub

    Public Sub New()
        _DLayer = New GeneralHODL
    End Sub


    Public Function GetTaxGroups() As DataTable

        Return _DLayer.GetTaxGroups()

    End Function

    Public Function GetCompanySettings() As DataTable

        Return _DLayer.GetCompanySettings()

    End Function

    Public Function CheckRecordExists(ByVal TableName As String, ByVal ColumnName As String, ByVal RecordToCheck As String,
                                      Optional ByVal ColumnName2 As String = "", Optional ByVal RecordToCheck2 As String = "",
                                      Optional ByVal ColumnName3 As String = "", Optional ByVal RecordToCheck3 As String = "") As Boolean

        Return _DLayer.CheckRecordExists(TableName, ColumnName, RecordToCheck, ColumnName2, RecordToCheck2, ColumnName3, RecordToCheck3)

    End Function

    Public Function RunStatementUpload(ByVal FileName As String, ByVal TypeOfFile As String) As String

        Return _DLayer.RunStatementUpload(FileName, TypeOfFile)

    End Function

    Public Function BankReconReport(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.BankReconReport(StartDate, EndDate)

    End Function

    Public Function BankReconReportDetails(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.BankReconReportDetails(StartDate, EndDate)

    End Function


    Public Function GetBranches() As DataTable

        Return _DLayer.GetBranches

    End Function

    Public Function InsertCashupComment(ByVal SaleDate As String, ByVal BranchCode As String,
                                         ByVal Comments As String, ByVal Username As String) As String

        Return _DLayer.InsertCashupComment(SaleDate, BranchCode, Comments, Username)

    End Function

    Public Function GetEFTIDS(ByVal BranchCode As String) As DataTable

        Return _DLayer.GetEFTIDS(BranchCode)

    End Function

    Public Function DeleteEFTID(ByVal BranchCode As String, ByVal EFTId As String) As String

        Return _DLayer.DeleteEFTID(BranchCode, EFTId)

    End Function

    Public Function UpdateEFTID(ByVal BranchCode As String, ByVal BranchName As String,
                                ByVal EFTId As String) As String

        Return _DLayer.UpdateEFTID(BranchCode, BranchName, EFTId)

    End Function

End Class
