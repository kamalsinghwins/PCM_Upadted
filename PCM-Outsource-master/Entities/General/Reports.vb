Public Class Reports
    Public Class SaveEmployeeReport
        Inherits Common
        Public Property All As Boolean
        Public Property Totals As Boolean
        Public Property EmployeeNumber As String

    End Class

    Public Class CashSummary
        Inherits Common
        Public Property Branches As String
    End Class

    Public Class CashTransactionsReport
        Inherits Common
        Public Property Sales As Boolean
        Public Property PaidIn As Boolean
        Public Property CreditNotes As Boolean
        Public Property PaidOut As Boolean
        Public Property Refunds As Boolean
        Public Property AccountPayments As Boolean

        Public Property Branches As String
    End Class

    Public Class BadDebtRequest
        Inherits Common
        Public Property Period As String

    End Class

    Public Class DeleteStockCodes
        Inherits Common
        Public Property MasterCode As String

    End Class

    Public Class Common
        Public Property IsAdmin As Boolean
        Public Property IPAddress As String
    End Class

    Public Class StockTransaction
        Inherits Common
        Public Property FromStockcode As String
        Public Property ToStockcode As String
        Public Property Category1 As String
        Public Property Category2 As String
        Public Property Category3 As String
        Public Property Sales As Boolean
        Public Property CreditNotes As Boolean
        Public Property Refunds As Boolean
        Public Property IBTIn As Boolean
        Public Property IBTOut As Boolean
        Public Property StockcodeAdjustments As Boolean
        Public Property GoodsReceieved As Boolean
        Public Property Branch As String
    End Class

    Public Class BestSellers
        Inherits Common
        Public Property MasterCode As Boolean
    End Class
    Public Class AccountsDrive
        Inherits Common
        Public Property Transactions As Boolean
    End Class
    Public Class CategorySummary
        Inherits Common
        Public Property Category1 As String
        Public Property Category2 As String
        Public Property Category3 As String
        Public Property RunSummaryOnly As Boolean
        Public Property AllBranches As Boolean
        Public Property Branches As String
    End Class
    Public Class ColourGrid
        Inherits Common
        Public Property Type As String
        Public Property FromStockCode As String
        Public Property ToStockCode As String
        Public Property Partials As String
        Public Property AllBranches As Boolean
        Public Property Branches As String
    End Class
    Public Class SearchResponse
        Inherits BaseResponse
        Public Property dt As DataTable
    End Class
    Public Class SearchStockcode
        Public Property SearchText As String
    End Class

    Public Class ColourGridRequest
        Public Property StartDate As String
        Public Property EndDate As String
        Public Property Email As String
        Public Property Username As String
        Public Property Type As String
        Public Property FromStockCode As String
        Public Property ToStockCode As String
        Public Property AllBranches As Boolean
        Public Property BranchList As String()
        Public Property PartialList As String()
        Public Property IsAdmin As String
        Public Property IPAddress As String
    End Class
    Public Class json
        Public Property FromStockCode As String
        Public Property ToStockCode As String
        Public Property BranchList As String()
        Public Property PartialList As String()
        Public Property IsAdmin As String
        Public Property IPAddress As String
        Public Property AllBranches As Boolean
        Public Property Type As String
    End Class
    Public Class GridSize
        Inherits Common
        Public Property Category1 As String
        Public Property Category2 As String
        Public Property Category3 As String
        Public Property SizeGrid As String
        Public Property StartCode As String
        Public Property EndCode As String
        Public Property Branches As String
        Public Property AllBranches As Boolean
    End Class
    Public Class AgingSummary
        Inherits Common
        Public Property TotalOutstanding As String
    End Class
    Public Class SOHReport
        Inherits Common
        Public Property Quantities As String
        Public Property Type As String
        Public Property From As String
        Public Property Mastercode As Boolean
        Public Property Blocked As String
        Public Property FromStockCode As String
        Public Property ToStockCode As String
        Public Property Branches As String
    End Class
End Class
