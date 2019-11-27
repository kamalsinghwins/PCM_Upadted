Public Class Users

    Public Property Username As String
    Public Property Password As String
    Public Property Permissions As String
    Public Property processing_permission_sequence As String
    Public Property maintenance_permission_sequence As String
    Public Property reporting_permission_sequence As String
    Public Property IsAdministrator As Boolean
    Public Property IPAddress As String
    Public Property isPCMAdmin As Boolean
    Public Property Email As String


End Class

Public Class LoginDetails
    Public Property Status As Boolean
    Public Property Browser As String
    Public Property Device As String
    Public Property IPAddress As String
    Public Property IDNumber As String
    Public Property IsMobile As Boolean

End Class
