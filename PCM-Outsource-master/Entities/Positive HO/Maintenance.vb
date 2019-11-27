Public Class Maintenance

End Class

Public Class EmployeeNoteRequest
    Public Property EmployeeName As String
    Public Property EmployeeNumber As String
    Public Property TypeOfReport As String
    Public Property Warning As String
    Public Property Rating As String
    Public Property Note As String
    Public Property BranchCode As String
    Public Property WarningExpiryDate As String

End Class
Public Class Branches

    Public Property ListOfBranches As List(Of Branch)
    Public Property return_message As String

End Class

Public Class Branch
    Public Property branch_code As String
    Public Property branch_name As String
    Public Property address_line_1 As String
    Public Property address_line_2 As String
    Public Property address_line_3 As String
    Public Property address_line_4 As String
    Public Property address_line_5 As String
    Public Property telephone_number As String
    Public Property fax_number As String
    Public Property email_address As String
    Public Property pricelevel As String
    Public Property tax_number As String
    Public Property is_blocked As Boolean
    Public Property is_head_office As Boolean
    Public Property branch_type As String
    Public Property merchant_number As String
    Public Property updated As String
    Public Property return_message As String
    Public Property target As String
    Public Property longitude As String
    Public Property latitude As String
    Public Property tURL As String


End Class


