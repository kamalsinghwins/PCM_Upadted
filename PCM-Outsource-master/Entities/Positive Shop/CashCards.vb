'<Serializable()> _
'Public Class CardNumberData

'    Public Property Password As String
'    Public Property DataBase As String
'    Public Property CardNumber As String
'    Public Property BranchCode As String
'    Public Property IPAddress As String
'    Public Property IDNumber As String

'End Class

<Serializable()> _
Public Class CashCardCustomer

    Public Property Password As String
    Public Property DataBase As String
    Public Property FullName As String                  'Transactions only
    Public Property CardNumber As String
    Public Property PointsBalance As String             'Transactions only
    Public Property PointsToCash As String              'Transactions only
    Public Property ReturnMessage As String
    Public Property IDNumber As String                  'Maintenance only
    Public Property FirstName As String                 'Maintenance only
    Public Property LastName As String                  'Maintenance only
    Public Property EMail As String                     'Maintenance only
    Public Property ContactNumber As String             'Maintenance only
    Public Property Status As String                    'Maintenance only
    Public Property Province As String                  'Maintenance only
    Public Property Sex As Boolean                      'Maintenance only
    Public Property isPromo As Boolean                  'Maintenance only
    Public Property TotalSpent As String                'Maintenance only
    Public Property CurrentPointsBalance As String      'Maintenance only
    Public Property TotalPointsAccrued As String        'Maintenance only
    Public Property BranchCode As String                'Maintenance only
    Public Property IPAddress As String                 'Maintenance only
    Public Property Username As String                  'Maintenance only
    Public Property DayOfBirth As String                 'Maintenance only
    Public Property MonthOfBirth As String              'Maintenance only
    Public Property YearOfBirth As String               'Maintenance only
    Public Property OldCardNumber As String             'Card Assign only


End Class

<Serializable()> _
Public Class CashCardEntity

    Public Property GuID As String
    Public Property DataBase As String
    Public Property Password As String
    Public Property CardNumber As String
    Public Property TransactionNumber As String                      'For collecting points
    Public Property BranchCode As String
    Public Property TransactionType As String                        'For collecting points
    Public Property TransactionDate As String                        'For collecting points
    Public Property TransactionTime As String                        'For collecting points
    Public Property TransactionTotal As String                       'For collecting points
    Public Property IPAddress As String
    Public Property LineItems As New List(Of CashCardLineItems)      'For collecting points
    Public Property GiftCardNumber As String                         'For redeeming
    Public Property PointsToRedeem As String                         'For redeeming
    Public Property ReturnMessage As String                          'For redeeming  

End Class

<Serializable()> _
Public Class CashCardLineItems

    Public Property MasterCode As String
    Public Property GeneratedCode As String
    Public Property Quantity As String
    Public Property SellingPrice As String

End Class

<Serializable()> _
Public Class CashCardLineItemsList
    Implements ICollection
    Public CollectionName As String
    Private LineItemArray As ArrayList = New ArrayList()

    Default Public Overloads ReadOnly _
Property Item(ByVal index As Integer) As CashCardLineItems
        Get
            Return CType(LineItemArray(index), CashCardLineItems)
        End Get
    End Property

    Public Sub CopyTo(array As Array, index As Integer) _
        Implements ICollection.CopyTo
        LineItemArray.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Count = LineItemArray.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Return Me
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return LineItemArray.GetEnumerator()
    End Function

    Public Function Add(ByVal NewLineItem As  _
                      CashCardLineItems) As Integer
        LineItemArray.Add(NewLineItem)
        Return LineItemArray.Count
    End Function

End Class



