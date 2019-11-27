Public Class CreatePDF
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

        _StrmWriter.WriteLine("<html>")
        _StrmWriter.WriteLine("<body>")
        _StrmWriter.WriteLine("<div>")

        _StrmWriter.WriteLine("<table border='1' cellspacing='0'>")

        '_StrmWriter.WriteLine("<tr>")
        '_StrmWriter.WriteLine("<th>CompanyName</th>")
        '_StrmWriter.WriteLine("<th>BranchName</th>")
        '_StrmWriter.WriteLine("<th>BranchCode</th>")
        '_StrmWriter.WriteLine("<th>TransactionType</th>")
        '_StrmWriter.WriteLine("<th>Telephone</th>")
        '_StrmWriter.WriteLine("<th>Fax</th>")
        '_StrmWriter.WriteLine("<th>ReferenceNumber</th>")
        '_StrmWriter.WriteLine("<th>TransactionNumber</th>")
        '_StrmWriter.WriteLine("<th>Date</th>")
        '_StrmWriter.WriteLine("<th>ToBranchCode</th>")
        '_StrmWriter.WriteLine("<th>ToBranchName</th>")
        '_StrmWriter.WriteLine("<th>DeliverLine1</th>")
        '_StrmWriter.WriteLine("<th>DeliverLine2</th>")
        '_StrmWriter.WriteLine("<th>DeliverLine3</th>")
        '_StrmWriter.WriteLine("<th>DeliverLine4</th>")
        '_StrmWriter.WriteLine("<th>DeliverLine5</th>")
        '_StrmWriter.WriteLine("<th>Stockcode</th>")
        '_StrmWriter.WriteLine("<th>Description</th>")
        '_StrmWriter.WriteLine("<th>Qty</th>")
        '_StrmWriter.WriteLine("<th>Unit Price</th>")
        '_StrmWriter.WriteLine("<th>Tax Amount</th>")
        '_StrmWriter.WriteLine("<th>LineTotal</th>")
        '_StrmWriter.WriteLine("<th>Total QTY</th>")
        '_StrmWriter.WriteLine("<th>SubTotal</th>")
        '_StrmWriter.WriteLine("<th>TotalTax</th>")
        '_StrmWriter.WriteLine("<th>TotalTotal</th>")
        '_StrmWriter.WriteLine("<th>SentQTY</th>")
        '_StrmWriter.WriteLine("<th>TotalSentQTY</th>")
        '_StrmWriter.WriteLine("</tr>")


    End Sub

    Public Sub CloseStream()
        _StrmWriter.WriteLine("</table>")
        _StrmWriter.WriteLine("<input type='button' onclick='Print()' value='print' >")
        _StrmWriter.WriteLine("</div>")

        _StrmWriter.WriteLine("<script> function Print(){window.print()} Print()</script>")
        _StrmWriter.WriteLine("</body>")
        _StrmWriter.WriteLine("</html>")
        _StrmWriter.Flush()
        _StrmWriter.Close()
    End Sub

    Public Sub WriteLine(ByVal Stockcode As String, ByVal Description As String, ByVal QTY As String,
                         ByVal UnitPrice As String, ByVal TaxAmount As String, ByVal LineTotal As String,
                         Optional ByVal SentQTY As String = "")
        _StrmWriter.WriteLine("<tr>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_CompanyName) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_BranchName) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_BranchCode) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_Telephone) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_Fax) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TransactionType) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_ReferenceNumber) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TransactionNumber) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_Date) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_toBranchCode) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_toBranchName) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_DeliveryLine1) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_DeliveryLine2) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_DeliveryLine3) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_DeliveryLine4) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_DeliveryLine5) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(Stockcode) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(Description) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(QTY) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(UnitPrice) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(TaxAmount) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(LineTotal) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TotalQTY) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_SubTotal) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TotalTax) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TotalTotal) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(SentQTY) & "</td>")
        _StrmWriter.WriteLine("<td>" & RG.Amp(_TotalSentQTY) & "</td>")
        _StrmWriter.WriteLine("</tr>")
    End Sub

    End Class

