Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Entities
Imports pcm.BusinessLayer

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class shop_transactions
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function CashTransaction(ByVal CashTransactions As CashTransaction) As String

        'If CashTransactions.DataBase = "" Then
        '    SendEmail("CashTransaction", "CashTransactions.DataBase =''")
        '    Return CashTransactions.transaction_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.ProcessCashTransaction(CashTransactions, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function CreditCardAuth(ByVal CreditCardAuthoristaion As CreditCardAuth) As String

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.CreditCardAuth(CreditCardAuthoristaion, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function NewVAT(ByVal Password As String, ByVal BranchCode As String, ByVal RecordsUpdated As String,
                           ByVal TimeStampCompleted As String) As String


        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.NewVAT(Password, BranchCode, RecordsUpdated, TimeStampCompleted)

    End Function

    <WebMethod()>
    Public Function ReprintTransaction(ByVal ReprintTransactions As ReprintTransaction) As String

        'If ReprintTransactions.DataBase = "" Then
        '    SendEmail("ReprintTransaction", "ReprintTransactions.DataBase =''")
        '    Return ReprintTransactions.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.ProcessReprints(ReprintTransactions, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()> _
    Public Function ProcessHeader(ByVal TransactionData As TransactionMaster) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("ProcessHeader", "TransactionData.DataBase =''")
        '    Return TransactionData.transaction_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.ProcessHeader(TransactionData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function ProcessLineItems(ByVal TransactionData As TransactionLineItems) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("ProcessLineItems", "TransactionData.DataBase =''")
        '    Return TransactionData.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.ProcessLineItems(TransactionData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function ProcessCashUp(ByVal TransactionData As CashUpData) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("ProcessCashUp", "TransactionData.DataBase =''")
        '    Return TransactionData.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.ProcessCashUp(TransactionData)

    End Function

    <WebMethod()>
    Public Function UpdateIBTTransactions(ByVal TransactionData As IBTTransactionData) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("UpdateIBTTransactions", "TransactionData.DataBase =''")
        '    Return TransactionData.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.UpdateIBTTransactions(TransactionData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function InsertIBTTransactions(ByVal TransactionData As IBTTransactionData) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("InsertIBTTransactions", "TransactionData.DataBase =''")
        '    Return TransactionData.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.InsertIBTTransactions(TransactionData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function EmployeeLogout(ByVal EmployeeData As Employee) As String

        'If EmployeeData.database = "" Then
        '    SendEmail("EmployeeLogout", "EmployeeData.DataBase =''")
        '    Return EmployeeData.upload_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.EmployeeLogout(EmployeeData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function EmployeeAbsent(ByVal EmployeeData As Employee) As String

        'If EmployeeData.database = "" Then
        '    SendEmail("EmployeeAbsent", "EmployeeData.DataBase =''")
        '    Return EmployeeData.upload_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.EmployeeAbsent(EmployeeData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function EmployeeUpdateTimeWorked(ByVal EmployeeData As Employee) As String

        'If EmployeeData.database = "" Then
        '    SendEmail("EmployeeUpdateTimeWorked", "EmployeeData.DataBase =''")
        '    Return EmployeeData.upload_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.EmployeeUpdateTimeWorked(EmployeeData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function EmployeeNoClockOut(ByVal EmployeeData As Employee) As String

        'If EmployeeData.database = "" Then
        '    SendEmail("EmployeeNoClockOut", "EmployeeData.DataBase =''")
        '    Return EmployeeData.upload_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.EmployeeNoClockOut(EmployeeData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function EmployeeLogIn(ByVal EmployeeData As Employee) As String

        'If EmployeeData.database = "" Then
        '    SendEmail("EmployeeLogIn", "EmployeeData.DataBase =''")
        '    Return EmployeeData.upload_guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.EmployeeLogIn(EmployeeData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function LastUpdate(ByVal wsPassword As String, ByVal Database As String,
                               ByVal ShopVersion As String, ByVal HOVersion As String,
                               ByVal UploaderVersion As String, ByVal ConfigVersion As String,
                               ByVal LastError As String, ByVal Queries As String,
                               ByVal IPAddess As String, ByVal BranchCode As String) As String

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.LastUpdate(wsPassword, ShopVersion, HOVersion, UploaderVersion,
                                  ConfigVersion, LastError, Queries, IPAddess, BranchCode)

    End Function

    <WebMethod()>
    Public Function InsertVoid(ByVal wsPassword As String, ByVal Database As String,
                               ByVal _void As Void) As String

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.InsertVoid(wsPassword, _void)

    End Function



    <WebMethod()>
    Public Function UpdateStationaryCount(ByVal TransactionData As StationaryTransactionData) As String

        'If TransactionData.DataBase = "" Then
        '    SendEmail("UpdateStationaryCount", "TransactionData.DataBase =''")
        '    Return TransactionData.Guid
        'End If

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.UpdateStationaryCount(TransactionData, Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function


    <WebMethod()>
    Public Function GetIBTOut(ByVal password As String,
                              ByVal receiving_branch_code As String,
                              ByVal sending_branch_code As String,
                              ByVal database As String, ByVal IBTOutNumber As String) As IBTOut

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.GetIBTOut(password, receiving_branch_code, sending_branch_code, IBTOutNumber,
                                 Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))

    End Function

    <WebMethod()>
    Public Function PostCashCardTransaction(ByVal CashCardTransactionData As CashCardEntity) As Boolean
        Dim Blayer As New CashCardBusinessLayer

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        CashCardTransactionData.IPAddress = sIPAddress

        Return Blayer.PostCashCardTransaction(CashCardTransactionData, sIPAddress)


    End Function

    <WebMethod()>
    Public Function StockEx(ByVal Password As String, ByVal BranchCode As String, ByVal GeneratedCode As String) As String

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Dim _BLayer As New ShopTransactionsBL()

        Return _BLayer.StockEx(Password, BranchCode, GeneratedCode, sIPAddress,
                               Server.MapPath("~/logs/ws-" & Format(Now, "yyyy-MM-dd") & ".txt"))


    End Function

    Private Sub SendEmail(ByVal Subject As String, ByVal EMailMessage As String)

        Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

        Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "PositiveLive Error")
        Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

        Msg3.IsBodyHtml = "False"

        Dim localID As String = ""
        Dim hostName = System.Net.Dns.GetHostName()
        For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()
            localID = localID & ("Name:  " & hostName & " IP Address: " & hostAdr.ToString()) & vbCrLf
        Next

        Msg3.Subject = Subject & " Error in PCM on " & hostName

        Msg3.Body = EMailMessage & vbCrLf & localID

        MailObj3.UseDefaultCredentials = False
        MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
        'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
        MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

        MailObj3.Send(Msg3)


    End Sub
End Class