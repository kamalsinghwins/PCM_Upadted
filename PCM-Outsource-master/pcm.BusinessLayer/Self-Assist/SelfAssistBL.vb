Imports pcm.DataLayer
Imports Entities
Public Class SelfAssistBL

    Public Function UploadScreensaverImage(ByVal ImageName As String, ByVal ImageDescription As String, ByVal FileLocation As String) As String

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.UploadImage(ImageName, ImageDescription, FileLocation)

    End Function

    Public Function GetImageList() As DataTable

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetImageList

    End Function

    Public Function GetCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer

        Dim dLayer As New SelfAssistDL

        Return dLayer.GetCustomerDetails(AccountDetails)

    End Function

    Public Function GetImages() As entScreensaverImages

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetImages

    End Function

    Public Function UpdateImages(ByVal lstImages As entScreensaverImages) As String

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.UpdateImages(lstImages)

    End Function

    Public Function GetStockOnHand(ByVal wsPassword As String, ByVal BranchCode As String, ByVal Stockcode As String) As DataSet

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        If BranchCode = "" Then
            Return Nothing
        End If

        If Stockcode = "" Then
            Return Nothing
        End If

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetStockOnHand(BranchCode, Stockcode)

    End Function

    Public Function GetAccountDetails(ByVal wsPassword As String, ByVal AccountDetails As Debtor) As Debtor

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetAccountDetails(AccountDetails)

    End Function

    Public Function RunStatement(ByVal wsPassword As String, ByVal AccountNumber As String) As ShopMiniStatement

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.RunStatement(AccountNumber)

    End Function

    Public Function GetLatestStyles(ByVal wsPassword As String, ByVal BranchCode As String) As StyleNumbers

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetLatestStyles(BranchCode)

    End Function

    Public Function GetScreenSaverImages() As entScreensaverImages

        Dim Dlayer As New SelfAssistDL

        Return Dlayer.GetImages

    End Function


End Class
