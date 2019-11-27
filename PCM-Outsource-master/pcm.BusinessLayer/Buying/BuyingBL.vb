Imports Microsoft.VisualBasic
Imports System.Data
Imports pcm.DataLayer
Imports System.Net.Mail

Public Class BuyingBL
    Dim _DLayer As BuyingDL

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New BuyingDL(CompanyCode)
    'End Sub

    Public Sub New()
        _DLayer = New BuyingDL
    End Sub

    Public Function GetVouchers(ByVal UserID As String)

        Return _DLayer.GetVouchers(UserID)

    End Function

    Public Function CheckViewClothing(ByVal UserID As String) As String

        Return _DLayer.CheckViewClothing(UserID)

    End Function

    Public Function GetItemCodeRankings(ByVal StartDate As String, ByVal EndDate As String, ByVal isClothing As Boolean) As DataTable

        Return _DLayer.GetItemCodeRankings(StartDate, EndDate, isClothing)

    End Function

    Public Function GetRankers() As DataTable

        Return _DLayer.GetRankers()

    End Function

    Public Function GetRankersRankings() As DataTable

        Return _DLayer.GetRankersRankings()

    End Function

    Public Function GetItemCodeRankingsTransactions(ByVal StartDate As String, ByVal EndDate As String, ByVal isClothing As Boolean) As DataTable

        Return _DLayer.GetItemCodeRankingsTransactions(StartDate, EndDate, isClothing)

    End Function

    Public Function InsertApplication(ByVal FirstName As String, ByVal LastName As String, ByVal IDNumber As String, ByVal EMailAddress As String,
                                     ByVal ContactNumber As String, ByVal Province As String, ByVal Kids As String) As String

        Return _DLayer.InsertApplication(FirstName, LastName, IDNumber, EMailAddress, ContactNumber, Province, Kids)

    End Function

    Public Function RedeemVoucher(ByVal UserID As String, ByVal Tier As Integer) As String

        Return _DLayer.RedeemVoucher(UserID, Tier)

    End Function

    Public Function CheckResetPassword(ByVal EMailAddress As String, ByVal GuID As String) As DataTable

        Return _DLayer.CheckResetPassword(EMailAddress, GuID)

    End Function

    Public Function GetStockcodes(ByVal SearchString As String) As DataTable

        Return _DLayer.GetStockcodes(SearchString)

    End Function

    Public Function ResetPassword(ByVal UserID As String, ByVal NewPassword As String) As String

        Return _DLayer.ResetPassword(UserID, NewPassword)

    End Function

    Public Function GetStockcodeDetail(ByVal Stockcode As String) As DataTable

        Return _DLayer.GetStockcodeDetail(Stockcode)

    End Function

    Public Function ProcessLogin(ByVal EMailAddress As String, ByVal Password As String, ByVal IPAddress As String) As DataTable

        Return _DLayer.ProcessLogin(EMailAddress, Password, IPAddress)

    End Function

    Public Function UpdateProduct(ByVal Stockcode As String, ByVal Description As String, ByVal Material As String, _
                                  ByVal QTYOrdered As String, ByVal ImageLocation As String, ByVal Price As String, ByVal DisplayOrder As String,
                                  ByVal Category As String) As String

        Return _DLayer.UpdateProduct(Stockcode, Description, Material, QTYOrdered, ImageLocation, Price, DisplayOrder, Category)

    End Function

    Public Sub UpdateUserForClothing(ByVal UserID As String, ByVal ViewClothing As Boolean)

        _DLayer.UpdateUserForClothing(UserID, ViewClothing)

    End Sub

    Public Function InsertProduct(ByVal Stockcode As String, ByVal Description As String, ByVal Material As String, _
                                  ByVal QTYOrdered As String, ByVal ImageLocation As String, ByVal Price As String, ByVal DisplayOrder As String,
                                  ByVal Category As String) As String

        Return _DLayer.InsertProduct(Stockcode, Description, Material, QTYOrdered, ImageLocation, Price, DisplayOrder, Category)

    End Function

    Public Function InsertStockCodeRating(ByVal Stockcode As String, ByVal Rating As Integer,
                                          ByVal PriceRange As String, ByVal UserId As String,
                                          ByVal LengthOfAction As String, ByVal Comments As String) As String
        Return _DLayer.InsertStockCodeRating(Stockcode, Rating, PriceRange, UserId, LengthOfAction, Comments)
    End Function

    Public Function GetAllStockCodeImages(ByVal Stockcode As String) As DataTable
        Return _DLayer.GetAllStockCodeImages(Stockcode)

    End Function

    Public Function GetAllBuyingStockCodes(ByVal UserID As String, ByVal ShowClothing As Boolean) As DataTable
        Return _DLayer.GetAllBuyingStockCodes(UserID, ShowClothing)

    End Function

    Public Function GetUserDataValues(ByVal UserId As String) As DataTable
        Return _DLayer.GetUserDataValues(UserId)
    End Function

    Public Function UpdateUserPoints(ByVal Points As Integer, ByVal UserId As String) As String
        Return _DLayer.UpdateUserPoints(Points, UserId)
    End Function

    Public Function ResetPassword(ByVal EMailAddress As String) As String

        Dim returnstring = ""
        returnstring = _DLayer.GetGUIDForEmail(EMailAddress)

        If returnstring = "" Then
            Return "Fail"
        End If

        SendEmail(EMailAddress, returnstring)
        'Send email

        Return "Success"

    End Function

    Private Sub SendEmail(ByVal EMailAddress As String, ByVal TempUnlock As String)

        Dim Msg As MailMessage = New MailMessage()
        Dim MailObj As New SmtpClient("mail.ragesa.co.za")

        Msg.From = New MailAddress("buying_dept@ragesa.co.za", "Rage")
        Msg.IsBodyHtml = True
        MailObj.UseDefaultCredentials = False
        MailObj.Credentials = New Net.NetworkCredential("buying_dept@ragesa.co.za", "Dgdg7609")
        MailObj.DeliveryMethod = SmtpDeliveryMethod.Network

        Msg.Subject = "Password reset instructions"

        Msg.To.Clear()
        Try
            Msg.To.Add(New MailAddress(Trim(EMailAddress), Trim(EMailAddress)))
        Catch ex As Exception
            Exit Sub
        End Try

        Dim NewBody As String
        NewBody = "Hi," & vbCrLf & vbCrLf
        NewBody &= "To reset your Rage Phantom Buying password, please click on the following link and choose a new password:" & vbCrLf
        NewBody &= "http://www.myrage.co.za/buying/Activate.aspx?guid=" & TempUnlock & vbCrLf & vbCrLf
        NewBody &= "Please let us know should you need any further assistance." & vbCrLf
        NewBody &= "Regards," & vbCrLf
        NewBody &= "Rage"

        Msg.Body = NewBody

        Try
            MailObj.Send(Msg)
        Catch ex As Exception

        End Try

    End Sub

End Class
