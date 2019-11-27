Imports Entities
Imports pcm.DataLayer

Public Class CashCardBusinessLayer

    Public Function AssignCard(ByVal AccountDetails As CashCardCustomer) As String
        Dim dLayer As New CashCardDataLayer

        Return dLayer.AssignCard(AccountDetails)

    End Function

    Public Function GetCustomerList(ByVal AccountDetails As CashCardCustomer,
                                    ByVal isSearchByID As Boolean, ByVal SearchCriteria As String) As List(Of CashCardCustomer)
        Dim dLayer As New CashCardDataLayer

        Return dLayer.GetCustomerList(AccountDetails, isSearchByID, SearchCriteria)

    End Function

    Public Function PostCashCardTransaction(ByVal CashCardTransactionData As CashCardEntity, ByVal IPAddress As String) As Boolean

        Dim dLayer As New CashCardDataLayer

        Return dLayer.PostCashCardTransactionDL(CashCardTransactionData, IPAddress)

    End Function

    Public Function GetPointsBalance(ByVal CardNumber As CashCardCustomer) As CashCardCustomer

        Dim dLayer As New CashCardDataLayer

        Return dLayer.ReturnPointsBalance(CardNumber)

    End Function

    Public Function GetCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer

        Dim dLayer As New CashCardDataLayer

        Return dLayer.GetCustomerDetails(AccountDetails)

    End Function

    Public Function RedeemPoints(ByVal CardNumber As CashCardEntity) As String

        Dim dLayer As New CashCardDataLayer

        Return dLayer.RedeemPoints(CardNumber)

    End Function

    Public Function ProcessGiftCardSale(ByVal CardNumber As CashCardEntity) As String

        Dim dLayer As New CashCardDataLayer

        Return dLayer.ProcessGiftCardSale(CardNumber)

    End Function

    Public Function UpdateCustomerTable(ByVal AccountDetails As CashCardCustomer) As String

        Dim dLayer As New CashCardDataLayer

        Return dLayer.UpdateCustomerTable(AccountDetails)

    End Function


End Class


