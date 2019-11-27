Public Class DispatchStock
    Public Class Common
        Public Property Path1 As String
        Public Property Path2 As String

    End Class
    Public Class GetStockcodeResponse
        Public Property dt As DataTable
    End Class
    Public Class GetStockDetails
        Public Property IBTOutNumber As String
    End Class
    Public Class DispatchStockCode
        Public Property Driver As String
        Public Property Registration As String
        Public Property KM As String
        Public Property ListData As List(Of DispatchList)

    End Class
    Public Class DispatchList
        Public Property IBTOutNumber As String
        Public Property BranchName As String
        Public Property BranchCode As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property Address4 As String
        Public Property Address5 As String
    End Class
    Public Class GetStock
        Inherits BaseResponse
        Public Property dt As String
        Public Property ErrorMessages As String
    End Class
    Public Class DispatchStockResponse
        Inherits BaseResponse
        Public Property DriverLogPath As String
        Public Property DeliveryNotePath As String
    End Class
    Public Class DeleteReceipts
        Public Property DeliveryNotesPath As String
        Public Property DriverLogPath As String

    End Class
End Class
