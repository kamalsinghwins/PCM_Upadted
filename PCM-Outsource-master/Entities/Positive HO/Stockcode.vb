Public Class Stockcode
    Public Property MasterCode As String
    Public Property GeneratedCode As String
    Public Property Description As String
    Public Property Barcode As String
    Public Property PurchaseTaxGroup As String
    Public Property SalesTaxGroup As String
    Public Property Supplier As String
    Public Property SupplierCode As String
    Public Property Category1 As String
    Public Property Category2 As String
    Public Property Category3 As String
    Public Property CostExclusive As String
    Public Property CostAverage As String
    Public Property SellingPrice1 As String
    Public Property SellingPrice2 As String
    Public Property SellingPrice3 As String
    Public Property SizeGrid As String
    Public Property ColourString As String
    Public Property MinumumLevel As String
    Public Property ItemSize As String
    Public Property ItemColour As String
    Public Property isServiceItem As Boolean
    Public Property isNotDiscountable As Boolean
    Public Property isBlocked As Boolean
    Public Property isForUpdate As Boolean
    Public Property OriginalColourString As String              'Used for stockcode manager code update
    Public Property isGeneratedCode As Boolean                  'Used for stockcode manager code update
    Public Property OriginalBarcode As String                   'Used for stockcode manager code update

End Class

Public Class PrePack

    Public Property PrePackCode As String
    Public Property SizeGrid As String
    Public Property PrePackGrid As New List(Of PrePackGrid)

End Class

Public Class PrePackGrid

    Public Property Size As String
    Public Property Quantity As String

End Class
Public Class DeleteStockcode
    Public Property StockCode As String
    Public Property IPAddress As String
    Public Property IsAdmin As Boolean
End Class