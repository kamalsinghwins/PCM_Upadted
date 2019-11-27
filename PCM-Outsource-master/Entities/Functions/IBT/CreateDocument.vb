Public Class CreateDocument
    Dim RG As New CommonFunctions.clsCommon

    Dim _StrmWriter As IO.StreamWriter
    Dim _CompanyName As String
    Dim _BranchName As String
    Dim _BranchCode As String
    Dim _TransactionType As String
    Dim _Telephone As String
    Dim _Fax As String
    Dim _ReferenceNumber As String
    Dim _TransactionNumber As String
    Dim _Date As String
    Dim _toBranchCode As String
    Dim _toBranchName As String
    Dim _DeliveryLine1 As String
    Dim _DeliveryLine2 As String
    Dim _DeliveryLine3 As String
    Dim _DeliveryLine4 As String
    Dim _DeliveryLine5 As String
    Dim _TotalQTY As String
    Dim _SubTotal As String
    Dim _TotalTax As String
    Dim _TotalTotal As String
    Dim _TotalSentQTY As String

    Public Sub New(ByVal FileNameToCreate As String, ByVal CompanyName As String, ByVal BranchName As String, ByVal BranchCode As String,
                   ByVal TransactionType As String, ByVal ToBranchName As String, ByVal Telephone As String, ByVal Fax As String,
                   ByVal ReferenceNumber As String, ByVal TransactionNumber As String, ByVal tDate As String, ByVal toBranchCode As String,
                   ByVal DeliveryLine1 As String, ByVal DeliveryLine2 As String, ByVal DeliveryLine3 As String, ByVal DeliveryLine4 As String,
                   ByVal DeliveryLine5 As String, ByVal TotalQTY As String, ByVal SubTotal As String, ByVal TotalTax As String, ByVal TotalTotal As String,
                   Optional ByVal TotalSentQTY As String = "")

        _CompanyName = CompanyName
        _BranchName = BranchName
        _BranchCode = BranchCode
        _TransactionType = TransactionType
        _Telephone = Telephone
        _Fax = Fax
        _ReferenceNumber = ReferenceNumber
        _TransactionNumber = TransactionNumber
        _Date = tDate
        _toBranchCode = toBranchCode
        _toBranchName = ToBranchName
        _DeliveryLine1 = DeliveryLine1
        _DeliveryLine2 = DeliveryLine2
        _DeliveryLine3 = DeliveryLine3
        _DeliveryLine4 = DeliveryLine4
        _DeliveryLine5 = DeliveryLine5
        _TotalQTY = TotalQTY
        _SubTotal = SubTotal
        _TotalTax = TotalTax
        _TotalTotal = TotalTotal
        _TotalSentQTY = TotalSentQTY

        _StrmWriter = New IO.StreamWriter(FileNameToCreate)
        _StrmWriter.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
        _StrmWriter.WriteLine("<Document>")
        _StrmWriter.WriteLine("<mainroot>")

    End Sub

    Public Sub CloseStream()
        _StrmWriter.WriteLine("</mainroot>")
        _StrmWriter.WriteLine("</Document>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Sub WriteLine(ByVal Stockcode As String, ByVal Description As String, ByVal QTY As String,
                         ByVal UnitPrice As String, ByVal TaxAmount As String, ByVal LineTotal As String,
                         Optional ByVal SentQTY As String = "")
        _StrmWriter.WriteLine("<Invoices>")
        _StrmWriter.WriteLine("<CompanyName>" & RG.Amp(_CompanyName) & "</CompanyName>")
        _StrmWriter.WriteLine("<BranchName>" & RG.Amp(_BranchName) & "</BranchName>")
        _StrmWriter.WriteLine("<BranchCode>" & RG.Amp(_BranchCode) & "</BranchCode>")
        _StrmWriter.WriteLine("<TransactionType>" & RG.Amp(_TransactionType) & "</BranchCode>")
        _StrmWriter.WriteLine("<Telephone>" & RG.Amp(_Telephone) & "</Telephone>")
        _StrmWriter.WriteLine("<Fax>" & RG.Amp(_Fax) & "</Fax>")
        _StrmWriter.WriteLine("<TransactionType>" & RG.Amp(_TransactionType) & "</TransactionType>")
        _StrmWriter.WriteLine("<ReferenceNumber>" & RG.Amp(_ReferenceNumber) & "</ReferenceNumber>")
        _StrmWriter.WriteLine("<TransactionNumber>" & RG.Amp(_TransactionNumber) & "</TransactionNumber>")
        _StrmWriter.WriteLine("<Date>" & RG.Amp(_Date) & "</Date>")
        _StrmWriter.WriteLine("<ToBranchCode>" & RG.Amp(_toBranchCode) & "</ToBranchCode>")
        _StrmWriter.WriteLine("<ToBranchName>" & RG.Amp(_toBranchName) & "</ToBranchName>")
        _StrmWriter.WriteLine("<DeliverLine1>" & RG.Amp(_DeliveryLine1) & "</DeliverLine1>")
        _StrmWriter.WriteLine("<DeliverLine2>" & RG.Amp(_DeliveryLine2) & "</DeliverLine2>")
        _StrmWriter.WriteLine("<DeliverLine3>" & RG.Amp(_DeliveryLine3) & "</DeliverLine3>")
        _StrmWriter.WriteLine("<DeliverLine4>" & RG.Amp(_DeliveryLine4) & "</DeliverLine4>")
        _StrmWriter.WriteLine("<DeliverLine5>" & RG.Amp(_DeliveryLine5) & "</DeliverLine5>")
        _StrmWriter.WriteLine("<Stockcode>" & RG.Amp(Stockcode) & "</Stockcode>")
        _StrmWriter.WriteLine("<Description>" & RG.Amp(Description) & "</Description>")
        _StrmWriter.WriteLine("<Qty>" & RG.Amp(QTY) & "</Qty>")
        _StrmWriter.WriteLine("<UnitPrice>" & RG.Amp(UnitPrice) & "</UnitPrice>")
        _StrmWriter.WriteLine("<Tax>" & RG.Amp(TaxAmount) & "</Tax>")
        _StrmWriter.WriteLine("<LineTotal>" & RG.Amp(LineTotal) & "</LineTotal>")
        _StrmWriter.WriteLine("<TotalQTY>" & RG.Amp(_TotalQTY) & "</TotalQTY>")
        _StrmWriter.WriteLine("<SubTotal>" & RG.Amp(_SubTotal) & "</SubTotal>")
        _StrmWriter.WriteLine("<TotalTax>" & RG.Amp(_TotalTax) & "</TotalTax>")
        _StrmWriter.WriteLine("<TotalTotal>" & RG.Amp(_TotalTotal) & "</TotalTotal>")
        _StrmWriter.WriteLine("<SentQTY>" & RG.Amp(SentQTY) & "</SentQTY>")
        _StrmWriter.WriteLine("<TotalSentQTY>" & RG.Amp(_TotalSentQTY) & "</TotalSentQTY>")
        _StrmWriter.WriteLine("</Invoices>")
    End Sub

End Class
